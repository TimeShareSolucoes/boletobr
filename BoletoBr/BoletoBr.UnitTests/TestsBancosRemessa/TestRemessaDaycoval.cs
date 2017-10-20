using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoletoBr.Arquivo;
using BoletoBr.Enums;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Bancos.Daycoval;
using BoletoBr.Dominio;

namespace BoletoBr.UnitTests.TestsBancosRemessa
{
    [TestClass]
    public class TestRemessaDaycoval
    {
        [TestMethod]
        public void TestGeracaoArquivoRemessa()
        {
            var dadosRemessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro,
                "2");

            var banco = Fabricas.BancoFactory.ObterBanco("707", "2");
            var contaBancariaCedente = new ContaBancaria("1001", "1", "2002", "1");
            var cedente = new Cedente("", "190658906300", 0, "99.999.999/9999-99", "Razao Social X",
                contaBancariaCedente,
                null);

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

            var carteira = new CarteiraCobranca {Codigo = "3"};
            var boleto = new Boleto(carteira, cedente, sacado, dadosRemessa)
            {
                NumeroDocumento = "1001",
                ValorBoleto = Convert.ToDecimal(255),
                IdentificadorInternoBoleto = "1001",
                DataVencimento = new DateTime(2017, 10, 20),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil),
                BancoBoleto = banco,
                CodigoOcorrenciaRemessa = new CodigoOcorrencia(01)
            };

            banco.FormatarBoleto(boleto);

            var remessa = new RemessaCnab400();
            remessa.Header = new HeaderRemessaCnab400(boleto, 1, 1);
            var detalheIndividual = new DetalheRemessaCnab400(boleto, 1);
            remessa.RegistrosDetalhe = new List<DetalheRemessaCnab400>
            {
                detalheIndividual
            };

            remessa.Trailer = new TrailerRemessaCnab400(1, 1);

            var escritor = new EscritorRemessaCnab400Daycoval(remessa);
            var linhasEscrever = escritor.EscreverTexto(remessa);
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var data = String.Format("{0}_{1}", DateTime.Now.ToString("ddMMyyyy"), DateTime.Now.ToString("HHmmss"));
            var nomeArquivo = string.Format("{0}{1}{2}{3}", banco.CodigoBanco, @"_REMESSA_", data, ".txt");

            StringBuilder sb = new StringBuilder();
            foreach (var linha in linhasEscrever)
            {
                sb.AppendLine(linha);
            }

            Assert.AreEqual(linhasEscrever[0],
                "01REMESSA01COBRANÇA       190658906300        Razao Social X                707BANCO DAYCOVAL 201017                                                                                                                                                                                                                                                                                                      000001");
            Assert.AreEqual(linhasEscrever[1],
                "10299999999999999190658906300                         0000100100000000                                     30100001001  20101700000000255007070000001N201017000000000000000000000000000000000000000000000000000000000000000299999999999999SACADO FULANO DE TAL                    1,9,COMP X                              Bairro X    12345000Cidade X       XXRazao Social X                          000000002");
            Assert.AreEqual(linhasEscrever[2],
                "9                                                                                                                                                                                                                                                                                                                                                                                                         000003");
        }

        [TestMethod]
        public void TestGerarHeaderArquivoRemessaDaycovalCnab400()
        {
            var dadosRemessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro,
                "2");

            var banco = Fabricas.BancoFactory.ObterBanco("707", "2");
            var contaBancariaCedente = new ContaBancaria("1001", "1", "2002", "1");
            var cedente = new Cedente("", "190658906300", 0, "99.999.999/9999-99", "Razao Social X",
                contaBancariaCedente,
                null);

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

            var carteira = new CarteiraCobranca {Codigo = "3"};
            var boleto = new Boleto(carteira, cedente, sacado, dadosRemessa)
            {
                NumeroDocumento = "1001",
                ValorBoleto = Convert.ToDecimal(255),
                IdentificadorInternoBoleto = "1001",
                DataVencimento = new DateTime(2017, 10, 20),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil),
                BancoBoleto = banco,
                CodigoOcorrenciaRemessa = new CodigoOcorrencia(01)
            };

            banco.FormatarBoleto(boleto);

            var remessa = new RemessaCnab400();
            remessa.Header = new HeaderRemessaCnab400(boleto, 1, 1);
            var detalheIndividual = new DetalheRemessaCnab400(boleto, 1);
            remessa.RegistrosDetalhe = new List<DetalheRemessaCnab400>
            {
                detalheIndividual
            };

            remessa.Trailer = new TrailerRemessaCnab400(1, 1);

            var escritor = new EscritorRemessaCnab400Daycoval(remessa);
            var linhasEscrever = escritor.EscreverTexto(remessa);
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var data = String.Format("{0}_{1}", DateTime.Now.ToString("ddMMyyyy"), DateTime.Now.ToString("HHmmss"));
            var nomeArquivo = string.Format("{0}{1}{2}{3}", banco.CodigoBanco, @"_REMESSA_", data, ".txt");

            StringBuilder sb = new StringBuilder();
            foreach (var linha in linhasEscrever)
            {
                sb.AppendLine(linha);
            }

            Assert.AreEqual(linhasEscrever[0],
                "01REMESSA01COBRANÇA       190658906300        Razao Social X                707BANCO DAYCOVAL 201017                                                                                                                                                                                                                                                                                                      000001");
        }

        [TestMethod]
        public void TestGerarDetalheArquivoRemessaDaycovalCnab400()
        {
            var dadosRemessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro,
                "2");

            var banco = Fabricas.BancoFactory.ObterBanco("707", "2");
            var contaBancariaCedente = new ContaBancaria("1001", "1", "2002", "1");
            var cedente = new Cedente("", "190658906300", 0, "99.999.999/9999-99", "Razao Social X",
                contaBancariaCedente,
                null);

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

            var carteira = new CarteiraCobranca {Codigo = "3"};
            var boleto = new Boleto(carteira, cedente, sacado, dadosRemessa)
            {
                NumeroDocumento = "1001",
                ValorBoleto = Convert.ToDecimal(255),
                IdentificadorInternoBoleto = "1001",
                DataVencimento = new DateTime(2017, 10, 20),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil),
                BancoBoleto = banco,
                CodigoOcorrenciaRemessa = new CodigoOcorrencia(01)
            };

            banco.FormatarBoleto(boleto);

            var remessa = new RemessaCnab400();
            remessa.Header = new HeaderRemessaCnab400(boleto, 1, 1);
            var detalheIndividual = new DetalheRemessaCnab400(boleto, 1);
            remessa.RegistrosDetalhe = new List<DetalheRemessaCnab400>
            {
                detalheIndividual
            };

            remessa.Trailer = new TrailerRemessaCnab400(1, 1);

            var escritor = new EscritorRemessaCnab400Daycoval(remessa);
            var linhasEscrever = escritor.EscreverTexto(remessa);
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var data = String.Format("{0}_{1}", DateTime.Now.ToString("ddMMyyyy"), DateTime.Now.ToString("HHmmss"));
            var nomeArquivo = string.Format("{0}{1}{2}{3}", banco.CodigoBanco, @"_REMESSA_", data, ".txt");

            StringBuilder sb = new StringBuilder();
            foreach (var linha in linhasEscrever)
            {
                sb.AppendLine(linha);
            }

            Assert.AreEqual(linhasEscrever[1],
                "10299999999999999190658906300                         0000100100000000                                     30100001001  20101700000000255007070000001N201017000000000000000000000000000000000000000000000000000000000000000299999999999999SACADO FULANO DE TAL                    1,9,COMP X                              Bairro X    12345000Cidade X       XXRazao Social X                          000000002");
        }

        [TestMethod]
        public void TestGerarTrailerArquivoRemessaDaycovalCnab400()
        {
            var dadosRemessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro,
                "2");

            var banco = Fabricas.BancoFactory.ObterBanco("707", "2");
            var contaBancariaCedente = new ContaBancaria("1001", "1", "2002", "1");
            var cedente = new Cedente("", "190658906300", 0, "99.999.999/9999-99", "Razao Social X",
                contaBancariaCedente,
                null);

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

            var carteira = new CarteiraCobranca {Codigo = "3"};
            var boleto = new Boleto(carteira, cedente, sacado, dadosRemessa)
            {
                NumeroDocumento = "1001",
                ValorBoleto = Convert.ToDecimal(255),
                IdentificadorInternoBoleto = "1001",
                DataVencimento = new DateTime(2017, 10, 20),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil),
                BancoBoleto = banco,
                CodigoOcorrenciaRemessa = new CodigoOcorrencia(01)
            };

            banco.FormatarBoleto(boleto);

            var remessa = new RemessaCnab400();
            remessa.Header = new HeaderRemessaCnab400(boleto, 1, 1);
            var detalheIndividual = new DetalheRemessaCnab400(boleto, 1);
            remessa.RegistrosDetalhe = new List<DetalheRemessaCnab400>
            {
                detalheIndividual
            };

            remessa.Trailer = new TrailerRemessaCnab400(1, 1);

            var escritor = new EscritorRemessaCnab400Daycoval(remessa);
            var linhasEscrever = escritor.EscreverTexto(remessa);
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var data = String.Format("{0}_{1}", DateTime.Now.ToString("ddMMyyyy"), DateTime.Now.ToString("HHmmss"));
            var nomeArquivo = string.Format("{0}{1}{2}{3}", banco.CodigoBanco, @"_REMESSA_", data, ".txt");

            StringBuilder sb = new StringBuilder();
            foreach (var linha in linhasEscrever)
            {
                sb.AppendLine(linha);
            }

            Assert.AreEqual(linhasEscrever[2],
                "9                                                                                                                                                                                                                                                                                                                                                                                                         000003");
        }
    }
}
