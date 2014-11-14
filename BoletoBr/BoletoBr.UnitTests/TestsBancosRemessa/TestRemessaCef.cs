using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BoletoBr.Arquivo;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Bancos.Cef;
using BoletoBr.Dominio;
using BoletoBr.Enums;
using BoletoBr.Fabricas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancosRemessa
{
    [TestClass]
    public class TestRemessaCef
    {
        [TestMethod]
        public void TestGeracaoArquivoRemessaCef()
        {
            var dadosRemessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("007", "8", "1234", "0");

            var cedente = new Cedente("999999", "123456", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

            var sacado = new Sacado("Sacado Fulano de Tal", "99.999.999/9999-99", new Endereco
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

            var carteira = new CarteiraCobranca { Codigo = "SR" };

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

            File.WriteAllLines("C:\\REMESSA.TXT", linhasEscrever.ToArray());
        }

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
                IdentificadorInternoBoleto = "19",
                DataVencimento = new DateTime(2014, 06, 30),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil)
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            //const int numeroRegistro = 1;

            //var escritor = new EscritorRemessaCnab240CefSicgb();

            //var linhasEscrever = escritor.EscreverHeader(boleto, numeroRegistro);

            //var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //var data = DateTime.Now.ToString("d").Replace("/", "");

            //var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_HEADER", ".txt");
            
            //var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            //arquivo.WriteLine(linhasEscrever);

            //arquivo.Close();
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
                IdentificadorInternoBoleto = "19",
                DataVencimento = new DateTime(2014, 06, 30),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil)
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            //const int numeroLote = 1;
            //const int numeroRegistro = 1;
            //const int numeroRemessa = 1;

            //var escritor = new EscritorRemessaCnab240CefSicgb();

            //var linhasEscrever = escritor.EscreverHeaderDeLote(boleto, numeroRemessa, numeroLote, numeroRegistro);

            //var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //var data = DateTime.Now.ToString("d").Replace("/", "");

            //var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_HEADERdeLOTE", ".txt");

            //var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            //arquivo.WriteLine(linhasEscrever);

            //arquivo.Close();
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
                IdentificadorInternoBoleto = "19",
                DataVencimento = new DateTime(2014, 06, 30),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil)
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            //const int numeroLote = 1;
            //const int numeroRegistro = 1;

            //var escritor = new EscritorRemessaCnab240CefSicgb();

            //var linhasEscrever = escritor.EscreverDetalheSegmentoP(boleto, numeroLote, numeroRegistro);

            //var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //var data = DateTime.Now.ToString("d").Replace("/", "") ;

            //var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_DETALHESegmentoP", ".txt");

            //var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            //arquivo.WriteLine(linhasEscrever);

            //arquivo.Close();
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
                IdentificadorInternoBoleto = "19",
                DataVencimento = new DateTime(2014, 06, 30),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil)
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const int numeroLote = 1;
            const int numeroRegistroNoLote = 1;

            //var escritor = new EscritorRemessaCnab240CefSicgb();

            //var linhasEscrever = escritor.EscreverDetalheSegmentoQ(boleto, numeroLote, numeroRegistroNoLote);

            //var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //var data = DateTime.Now.ToString("d").Replace("/", "");

            //var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_DETALHESegmentoP", ".txt");

            //var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            //arquivo.WriteLine(linhasEscrever);

            //arquivo.Close();
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
                IdentificadorInternoBoleto = "19",
                DataVencimento = new DateTime(2014, 06, 30),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil)
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

            //var escritor = new EscritorRemessaCnab240CefSicgb();

            //var linhasEscrever = escritor.EscreverTrailerDeLote(
            //    qtdTotalCobrancaSimples,
            //    vlTotalCobrancaSimples,
            //    qtdTotalCobrancaCaucionada,
            //    vlTotalCobrancaCaucionada,
            //    qtdTotalCobrancaDescontada,
            //    vlTotalCobrancaDescontada,
            //    numeroLote,
            //    numeroRegistro);

            //var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //var data = DateTime.Now.ToString("d").Replace("/", "");

            //var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_TRAILERdeLOTE", ".txt");

            //var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            //arquivo.WriteLine(linhasEscrever);

            //arquivo.Close();
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
                IdentificadorInternoBoleto = "19",
                DataVencimento = new DateTime(2014, 06, 30),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil)
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const int qtdLotes = 1;
            const int qtdRegistros = 1;

            //var escritor = new EscritorRemessaCnab240CefSicgb();

            //var linhasEscrever = escritor.EscreverTrailer(qtdLotes, qtdRegistros);

            //var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //var data = DateTime.Now.ToString("d").Replace("/", "");

            //var nomeArquivo = string.Format("{0}{1}{2}{3}{4}{5}{6}", banco.CodigoBanco, "-", banco.NomeBanco, "_", data, @"_TRAILER", ".txt");

            //var arquivo = new System.IO.StreamWriter(path + @"\" + nomeArquivo, true);
            //arquivo.WriteLine(linhasEscrever);

            //arquivo.Close();
        }
    }
}
