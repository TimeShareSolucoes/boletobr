using System;
using BoletoBr.Bancos.Cef;
using BoletoBr.Dominio;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancosRemessa
{
    [TestClass]
    public class TestRemessaCef
    {
        [TestMethod]
        public void TestGerarHeaderArquivoRemessaCefCnab240()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("007", "8", "1234", "0");

            var cedente = new Cedente("999999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

            var sacado = new Sacado("Sacado Fulano de Tal", "99.999.999/9999-99", new Endereco
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

            var carteira = new CarteiraCobranca {Codigo = "SR"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "19",
                ValorBoleto = (decimal) 1000.51,
                SequencialNossoNumero = "19",
                DataVencimento = new DateTime(2014, 06, 30),
                Especie = new EspecieDocumento(02, "DUPLICATA MERCANTIL", "DM")
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const int numeroRegistro = 1;

            var escritor = new EscritorRemessaCnab240CefSicgb();

            var linhasEscrever = escritor.EscreverHeader(boleto, numeroRegistro);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var data = DateTime.Now.ToString("d").Replace("/", "");

            var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_HEADER", ".txt");
            
            var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            arquivo.WriteLine(linhasEscrever);

            arquivo.Close();
        }

        [TestMethod]
        public void TestGerarHeaderLoteArquivoRemessaCefCnab240()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("007", "8", "1234", "0");

            var cedente = new Cedente("999999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

            var sacado = new Sacado("Sacado Fulano de Tal", "99.999.999/9999-99", new Endereco
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

            var carteira = new CarteiraCobranca {Codigo = "SR"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "19",
                ValorBoleto = (decimal) 1000.51,
                SequencialNossoNumero = "19",
                DataVencimento = new DateTime(2014, 06, 30),
                Especie = new EspecieDocumento(02, "DUPLICATA MERCANTIL", "DM")
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const int numeroLote = 1;
            const int numeroRegistro = 1;
            const int numeroRemessa = 1;

            var escritor = new EscritorRemessaCnab240CefSicgb();

            var linhasEscrever = escritor.EscreverHeaderDeLote(boleto, numeroRemessa, numeroLote, numeroRegistro);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var data = DateTime.Now.ToString("d").Replace("/", "");

            var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_HEADERdeLOTE", ".txt");

            var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            arquivo.WriteLine(linhasEscrever);

            arquivo.Close();
        }

        [TestMethod]
        public void TestGerarDetalheSegmentoPArquivoRemessaCefCnab240()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("007", "8", "1234", "0");

            var cedente = new Cedente("999999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

            var sacado = new Sacado("Sacado Fulano de Tal", "99.999.999/9999-99", new Endereco
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

            var carteira = new CarteiraCobranca {Codigo = "SR"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "19",
                ValorBoleto = (decimal) 1000.51,
                SequencialNossoNumero = "19",
                DataVencimento = new DateTime(2014, 06, 30),
                Especie = new EspecieDocumento(02, "DUPLICATA MERCANTIL", "DM")
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const int numeroLote = 1;
            const int numeroRegistro = 1;

            var escritor = new EscritorRemessaCnab240CefSicgb();

            var linhasEscrever = escritor.EscreverDetalheSegmentoP(boleto, numeroLote, numeroRegistro);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var data = DateTime.Now.ToString("d").Replace("/", "") ;

            var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_DETALHESegmentoP", ".txt");

            var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            arquivo.WriteLine(linhasEscrever);

            arquivo.Close();
        }

        [TestMethod]
        public void TestGerarDetalheSegmentoQArquivoRemessaCefCnab240()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("007", "8", "1234", "0");

            var cedente = new Cedente("999999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

            var sacado = new Sacado("Sacado Fulano de Tal", "99.999.999/9999-99", new Endereco
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

            var carteira = new CarteiraCobranca {Codigo = "SR"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "19",
                ValorBoleto = (decimal) 1000.51,
                SequencialNossoNumero = "19",
                DataVencimento = new DateTime(2014, 06, 30),
                Especie = new EspecieDocumento(02, "DUPLICATA MERCANTIL", "DM")
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const int numeroLote = 1;
            const int numeroRegistroNoLote = 1;

            var escritor = new EscritorRemessaCnab240CefSicgb();

            var linhasEscrever = escritor.EscreverDetalheSegmentoQ(boleto, numeroLote, numeroRegistroNoLote);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var data = DateTime.Now.ToString("d").Replace("/", "");

            var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_DETALHESegmentoP", ".txt");

            var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            arquivo.WriteLine(linhasEscrever);

            arquivo.Close();
        }

        [TestMethod]
        public void TestGerarTrailerLoteArquivoRemessaCefCnab240()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("007", "8", "1234", "0");

            var cedente = new Cedente("999999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

            var sacado = new Sacado("Sacado Fulano de Tal", "99.999.999/9999-99", new Endereco
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

            var carteira = new CarteiraCobranca {Codigo = "SR"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "19",
                ValorBoleto = (decimal) 1000.51,
                SequencialNossoNumero = "19",
                DataVencimento = new DateTime(2014, 06, 30),
                Especie = new EspecieDocumento(02, "DUPLICATA MERCANTIL", "DM")
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const int numeroLote = 1;
            const int numeroRegistro = 1;

            const int qtdTotalCobrancaSimples = 1;
            const decimal vlTotalCobrancaSimples = (decimal) 355.43;

            const int qtdTotalCobrancaCaucionada = 1;
            const decimal vlTotalCobrancaCaucionada = (decimal) 299.98;

            const int qtdTotalCobrancaDescontada = 1;
            const decimal vlTotalCobrancaDescontada = (decimal) 157.37;

            var escritor = new EscritorRemessaCnab240CefSicgb();

            var linhasEscrever = escritor.EscreverTrailerDeLote(
                qtdTotalCobrancaSimples,
                vlTotalCobrancaSimples,
                qtdTotalCobrancaCaucionada,
                vlTotalCobrancaCaucionada,
                qtdTotalCobrancaDescontada,
                vlTotalCobrancaDescontada,
                numeroLote,
                numeroRegistro);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var data = DateTime.Now.ToString("d").Replace("/", "");

            var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_TRAILERdeLOTE", ".txt");

            var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            arquivo.WriteLine(linhasEscrever);

            arquivo.Close();
        }

        [TestMethod]
        public void TestGerarTrailerArquivoRemessaCefCnab240()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("007", "8", "1234", "0");

            var cedente = new Cedente("999999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

            var sacado = new Sacado("Sacado Fulano de Tal", "99.999.999/9999-99", new Endereco
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

            var carteira = new CarteiraCobranca {Codigo = "SR"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "19",
                ValorBoleto = (decimal) 1000.51,
                SequencialNossoNumero = "19",
                DataVencimento = new DateTime(2014, 06, 30),
                Especie = new EspecieDocumento(02, "DUPLICATA MERCANTIL", "DM")
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const int qtdLotes = 1;
            const int qtdRegistros = 1;

            var escritor = new EscritorRemessaCnab240CefSicgb();

            var linhasEscrever = escritor.EscreverTrailer(qtdLotes, qtdRegistros);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var data = DateTime.Now.ToString("d").Replace("/", "");

            var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_TRAILER", ".txt");

            var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            arquivo.WriteLine(linhasEscrever);

            arquivo.Close();
        }
    }
}
