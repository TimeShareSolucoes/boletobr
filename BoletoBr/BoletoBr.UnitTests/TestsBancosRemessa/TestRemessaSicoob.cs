using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BoletoBr.Arquivo;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Enums;
using BoletoBr.Fabricas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancosRemessa
{
    [TestClass]
    public class TestRemessaSicoob
    {
        [TestMethod]
        public void TestGeracaoArquivoRemessaSicoob()
        {

            var database = 2010;
            var dataatual = 2012;
            var periodo = 2;
            var resultado = (dataatual- dataatual) % periodo;



            var dadosRemessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("756", "0");

            var contaBancariaCedente = new ContaBancaria("3249", "2", "7341", "5");

            var cedente = new Cedente("999999", "123456", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

            var sacado = new Sacado("Sacado Fulano de Tal", "99.999.999/9999-98", new Endereco
            {
                TipoLogradouro = "R",
                Logradouro = "1",
                Bairro = "Bairro X",
                Cidade = "Cidade X",
                SiglaUf = "XX",
                Cep = "12345-000",
                Complemento = "Comp X",
                Numero = "9",
            });

            var carteira = new CarteiraCobranca { Codigo = "1/01" };

            var boleto = new Boleto(carteira, cedente, sacado, dadosRemessa)
            {
                BancoBoleto = banco,
                NumeroDocumento = "19",
                ValorBoleto = (decimal)1000.51,
                IdentificadorInternoBoleto = "19",
                DataVencimento = new DateTime(2014, 06, 30),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil),
            };

            var listaBoleto = new List<Boleto>();

            listaBoleto.Add(boleto);

            banco.FormatarBoleto(boleto);

            #region GERAÇÃO 1

            var remessa = new RemessaCnab240();

            //var listaDetalhes = new List<DetalheRemessaCnab240>();

            remessa.Header = new HeaderRemessaCnab240(listaBoleto.FirstOrDefault(), 1);

            //var detalheSegmentoP = new DetalheSegmentoPRemessaCnab240(boleto)
            //{
            //    CodigoCedente = "123456",
            //    NossoNumero = "123456",
            //    NumeroDocumento = "123456",
            //    CodigoOcorrencia = new CodigoOcorrencia(01),
            //    Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil),
            //    Aceite = "N",
            //    DataVencimento = new DateTime(2014, 10, 01),
            //    ValorBoleto = (decimal)100.51,
            //};

            remessa.Lotes = new List<LoteRemessaCnab240> { };

            var loteAdd = new LoteRemessaCnab240();
            loteAdd.HeaderLote = new HeaderLoteRemessaCnab240(listaBoleto.FirstOrDefault(), 1);

            loteAdd.TrailerLote = new TrailerLoteRemessaCnab240(1);

            //loteAdd.RegistrosDetalheSegmentos = new List<DetalheRemessaCnab240>();
            //var detalheRemessaAdd = new DetalheRemessaCnab240();
            //detalheRemessaAdd.SegmentoP = detalheSegmentoP;
            //loteAdd.RegistrosDetalheSegmentos.Add(detalheRemessaAdd);

            remessa.Lotes.Add(loteAdd);

            remessa.Trailer = new TrailerRemessaCnab240(1, 1);

            #endregion GERAÇÃO 1

            var fabricaRemessa = new RemessaFactory();

            var remessaPronta = fabricaRemessa.GerarRemessa(remessa.Header, loteAdd.HeaderLote, listaBoleto, loteAdd.TrailerLote, remessa.Trailer);

            var escritor = EscritorArquivoRemessaFactory.ObterEscritorRemessa(remessa);

            var linhasEscrever = escritor.EscreverTexto(remessaPronta);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var data = String.Format("{0}_{1}", DateTime.Now.ToString("ddMMyyyy"), DateTime.Now.ToString("HHmmss"));

            var nomeArquivo = string.Format("{0}{1}{2}{3}", banco.CodigoBanco, @"_REMESSA_", data, ".txt");

            StringBuilder sb = new StringBuilder();
            foreach (var linha in linhasEscrever)
            {
                sb.AppendLine(linha);
            }
            //Assert.Equals(1, 1);
            File.WriteAllLines($@"{path}\\{nomeArquivo}", linhasEscrever.ToArray());
            var esperadoHeader = "75600000         299999999999999                    0324920000009999990 RAZÃO SOCIAL X                SICOOB                                  10902201808483100000108100000                                                                     ";
            Assert.IsTrue(linhasEscrever[0].Length == 240 && linhasEscrever[0].Substring(0,143) == esperadoHeader.Substring(0, 143) && esperadoHeader.Substring(157, esperadoHeader.Length - 157) == linhasEscrever[0].Substring(157, esperadoHeader.Length - 157));
            Assert.IsTrue(linhasEscrever[1].Length == 240 && linhasEscrever[1] == "75600011R01  040 2099999999999999                    0324920000009999990 Razão Social X                                                                                                000000011202201800000000                                 ");
            Assert.IsTrue(linhasEscrever[2].Length == 240 && linhasEscrever[2] == "7560001300001P 010324920000009999990 00000019-401014     10 220000000000000193006201400000000010005100000 02N12022018030062014000000000000000000000000000000000000000000000000000000000000000000000DOC0000019-4             0000   090000000000 ");
            Assert.IsTrue(linhasEscrever[3].Length == 240 && linhasEscrever[3] == "7560001300002Q 012099999999999998SACADO FULANO DE TAL                    1,9,COMP X                              BAIRRO X       12345000CIDADE X       XX0000000000000000                                        000                            ");
            Assert.IsTrue(linhasEscrever[4].Length == 240 && linhasEscrever[4] == "75600015         00000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000                                                                                                                             ");
            Assert.IsTrue(linhasEscrever[5].Length == 240 && linhasEscrever[5] == "75699999         000001000001000000                                                                                                                                                                                                             ");
        }
    }
}
