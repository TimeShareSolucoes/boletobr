using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos.Santander;
using BoletoBr.Dominio;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancosRemessa
{
    [TestClass]
    public class TestRemessaSantander
    {
        [TestMethod]
        public void TestGerarHeaderArquivoRemessaSantanderCnab400()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("033", "7");

            var contaBancariaCedente = new ContaBancaria("0319", "", "", "");

            var cedente = new Cedente("527654", "1", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca {Codigo = "101"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "001N002",
                ValorBoleto = Convert.ToDecimal(80.55),
                SequencialNossoNumero = "000000000027",
                DataVencimento = new DateTime(2012, 11, 26),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil),
                CodigoOcorrenciaRemessa = new CodigoOcorrencia(01),
            };

            banco.FormatarBoleto(boleto);

            const int numeroRegistro = 1;

            var escritor = new EscritorRemessaCnab400Santander();

            var linhasEscrever = escritor.EscreverHeader(boleto, numeroRegistro);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var data = DateTime.Now.ToString("d").Replace("/", "");

            var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_HEADER", ".txt");

            var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            arquivo.WriteLine(linhasEscrever);

            arquivo.Close();
        }

        [TestMethod]
        public void TestGerarDetalheArquivoRemessaSantanderCnab400()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("033", "7");

            var contaBancariaCedente = new ContaBancaria("0020", "0", "13003695", "9");

            var cedente = new Cedente("527654", "1", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca { Codigo = "101" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "001N002",
                ValorBoleto = Convert.ToDecimal(80.55),
                JurosMora = Convert.ToDecimal(15),
                SequencialNossoNumero = "000000000027",
                //SequencialNossoNumero = "566612457800",
                DataVencimento = new DateTime(2012, 11, 26),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil),
                CodigoOcorrenciaRemessa = new CodigoOcorrencia(01),
            };

            banco.FormatarBoleto(boleto);

            const int numeroRegistro = 1;

            var escritor = new EscritorRemessaCnab400Santander();

            var linhasEscrever = escritor.EscreverDetalhe(boleto, numeroRegistro);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var data = DateTime.Now.ToString("d").Replace("/", "");

            var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_DETALHE", ".txt");

            var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            arquivo.WriteLine(linhasEscrever);

            arquivo.Close();
        }

        [TestMethod]
        public void TestGerarTrailerArquivoRemessaSantanderCnab400()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("033", "7");

            var contaBancariaCedente = new ContaBancaria("0020", "0", "13003695", "9");

            var cedente = new Cedente("527654", "1", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca { Codigo = "101" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "001N002",
                ValorBoleto = Convert.ToDecimal(80.55),
                JurosMora = Convert.ToDecimal(15),
                SequencialNossoNumero = "000000000027",
                //SequencialNossoNumero = "566612457800",
                DataVencimento = new DateTime(2012, 11, 26),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil),
                CodigoOcorrenciaRemessa = new CodigoOcorrencia(01),
            };

            banco.FormatarBoleto(boleto);

            const int numeroRegistro = 1;
            const decimal valorTitulos = (decimal) 10853.37;

            var escritor = new EscritorRemessaCnab400Santander();

            var linhasEscrever = escritor.EscreverTrailer(numeroRegistro, valorTitulos);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var data = DateTime.Now.ToString("d").Replace("/", "");

            var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_TRAILER", ".txt");

            var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            arquivo.WriteLine(linhasEscrever);

            arquivo.Close();
        }
    }
}
