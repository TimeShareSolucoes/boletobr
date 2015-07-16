using System;
using System.Collections.Generic;
using BoletoBr.Arquivo;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Bancos.BRB;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;
using BoletoBr.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancosRemessa
{
    [TestClass]
    public class TestRemessaBRB
    {
        [TestMethod]
        public void TestGerarHeaderArquivoRemessaBRBCnab400()
        {
            var dadosRemessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro,
                "");
            var banco = Fabricas.BancoFactory.ObterBanco("070");
            var contaBancariaCedente = new ContaBancaria("201", "", "29088", "1");
            var cedente = new Cedente("99999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);
            var sacado = new Sacado("Sacado Fulano de Tal", "99.999.999/9999-99", new Endereco()
            {
                TipoLogradouro = "R",
                Logradouro = "1",
                Bairro = "Bairro X",
                Cidade = "Cidade X",
                SiglaUf = "XX",
                Cep = "12345-000",
                Complemento = "Comp X",
                Numero = "9"
            });

            var carteira = new CarteiraCobranca {Codigo = "1", Tipo = "1"};

            var boleto = new Boleto(carteira, cedente, sacado, dadosRemessa)
            {
                NumeroDocumento = "279141",
                ValorBoleto = Convert.ToDecimal(222.75),
                IdentificadorInternoBoleto = "279141",
                DataVencimento = new DateTime(2015, 03, 16),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.Diversos),
                CodigoOcorrenciaRemessa = new CodigoOcorrencia(01),
                BancoBoleto = banco,
                TipoCobrancaJuro = TipoCobrancaJuro.JurosDiario
            };

            banco.FormatarBoleto(boleto);

            var remessa = new RemessaCnab400();

            remessa.Header = new HeaderRemessaCnab400(boleto, 1, 1, DateTime.Now);
            var detalheIndividual = new DetalheRemessaCnab400(boleto, 1);
            remessa.RegistrosDetalhe = new List<DetalheRemessaCnab400>
            {
                detalheIndividual
            };
            var escritor = new EscritorRemessaCnab400BRB(remessa);

            var linhasEscrever = escritor.EscreverTexto(remessa);
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var data = String.Format("{0}_{1}", DateTime.Now.ToString("ddMMyyyy"), DateTime.Now.ToString("HHmmss"));

            var nomeArquivo = string.Format("{0}{1}{2}{3}", banco.CodigoBanco, @"_REMESSA_", data, ".txt");
            var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);

            foreach (var linha in linhasEscrever)
            {
                arquivo.WriteLine(linha);    
            }
            arquivo.Close();
        }
    }
}
