using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos.Brasil;
using BoletoBr.Dominio;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancosRemessa
{
    [TestClass]
    public class TestRemessaBancoBrasil
    {
        [TestMethod]
        public void TestGerarHeaderArquivoRemessaBancoBrasilCnab400()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");

            var contaBancariaCedente = new ContaBancaria("2374", "4", "0165199", "4");

            var cedente = new Cedente("9999999", "000123", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

            var sacado = new Sacado("Sacado Fulano de Tal", "999.999.999-99", new Endereco()
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

            var carteira = new CarteiraCobranca { Codigo = "16" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "1",
                ValorBoleto = Convert.ToDecimal(221.40),
                SequencialNossoNumero = "1",
                DataVencimento = new DateTime(2014, 07, 10),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil),
                TipoModalidade = "21"
            };

            banco.FormatarBoleto(boleto);

            const int numeroRemessa = 1;
            const int numeroRegistro = 1;

            var escritor = new EscritorRemessaCnab400BancoDoBrasil();

            var linhasEscrever = escritor.EscreverHeader(boleto, numeroRemessa, numeroRegistro);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var data = DateTime.Now.ToString("ddMMyyyy");

            var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_HEADER", ".txt");

            var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            arquivo.WriteLine(linhasEscrever);

            arquivo.Close();
        }

        [TestMethod]
        public void TestGerarDetalheArquivoRemessaBancoBrasilCnab400()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");

            var contaBancariaCedente = new ContaBancaria("2374", "4", "0165199", "4");

            var cedente = new Cedente("9999999", "000123", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

            var sacado = new Sacado("Sacado Fulano de Tal", "999.999.999-99", new Endereco()
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

            var carteira = new CarteiraCobranca { Codigo = "16" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "1",
                ValorBoleto = Convert.ToDecimal(221.40),
                SequencialNossoNumero = "1",
                DataVencimento = new DateTime(2014, 07, 10),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil),
                TipoModalidade = "21",
                CodigoOcorrenciaRemessa = new CodigoOcorrencia(01),
            };

            banco.FormatarBoleto(boleto);

            const int numeroRegistro = 1;

            var escritor = new EscritorRemessaCnab400BancoDoBrasil();

            var linhasEscrever = escritor.EscreverDetalhe(boleto, numeroRegistro);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var data = DateTime.Now.ToString("ddMMyyyy");

            var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_REGISTRO_DETALHE", ".txt");

            var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            arquivo.WriteLine(linhasEscrever);

            arquivo.Close();
        }

        [TestMethod]
        public void TestGerarTrailerArquivoRemessaBancoBrasilCnab400()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");

            var contaBancariaCedente = new ContaBancaria("2374", "4", "0165199", "4");

            var cedente = new Cedente("9999999", "000123", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

            var sacado = new Sacado("Sacado Fulano de Tal", "999.999.999-99", new Endereco()
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

            var carteira = new CarteiraCobranca { Codigo = "16" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "1",
                ValorBoleto = Convert.ToDecimal(221.40),
                SequencialNossoNumero = "1",
                DataVencimento = new DateTime(2014, 07, 10),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil),
                TipoModalidade = "21"
            };

            banco.FormatarBoleto(boleto);

            const int numeroRegistro = 1;

            var escritor = new EscritorRemessaCnab400BancoDoBrasil();

            var linhasEscrever = escritor.EscreverTrailer(numeroRegistro);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var data = DateTime.Now.ToString("ddMMyyyy");

            var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_TRAILER", ".txt");

            var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            arquivo.WriteLine(linhasEscrever);

            arquivo.Close();
        }
    }
}
