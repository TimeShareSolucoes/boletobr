using System;
using System.IO;
using System.Net.Mime;
using BoletoBr.Arquivo;
using BoletoBr.Bancos.Bradesco;
using BoletoBr.Dominio;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancos
{
    [TestClass]
    public class BancoBradescoTests
    {
        #region Carteira 02

        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void FomatarNumeroDocumentoCarteira02Bradesco()
        {
            var boleto = new Boleto();
            var bradesco = new BancoBradesco();

            boleto.NumeroDocumento = "123";
            boleto.TipoArquivo = TipoArquivo.Cnab400;
            bradesco.FormataNumeroDocumento(boleto);

            Assert.AreEqual("0000000123", boleto.NumeroDocumento);

            boleto.NumeroDocumento = "0";
            bradesco.FormataNumeroDocumento(boleto);
            Assert.Fail();
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteira02Bradesco()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("0534", "7", "2801", "0");
            var cedente = new Cedente("4879962", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca {Codigo = "02", BancoEmiteBoleto = false};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "55617",
                ValorBoleto = Convert.ToDecimal(501.81),
                IdentificadorInternoBoleto = "55617",
                DataVencimento = new DateTime(2016, 11, 15),
                Moeda = "9"
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual("02/00000055617-0", boleto.NossoNumeroFormatado);
            Assert.AreEqual("0", boleto.DigitoNossoNumero);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteira02Bradesco()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("0534", "7", "2801", "0");
            var cedente = new Cedente("4879962", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca {Codigo = "02", BancoEmiteBoleto = false};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "55617",
                ValorBoleto = Convert.ToDecimal(501.81),
                IdentificadorInternoBoleto = "55617",
                DataVencimento = new DateTime(2016, 11, 15),
                Moeda = "9"
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "23790.53404 20000.005569 17000.280101 6 69790000050181";
            Assert.AreEqual(linhaDigitavel.Length, boleto.LinhaDigitavelBoleto.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteira02Bradesco()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("0534", "7", "2801", "0");
            var cedente = new Cedente("4879962", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca {Codigo = "02", BancoEmiteBoleto = false};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "55617",
                ValorBoleto = Convert.ToDecimal(501.81),
                IdentificadorInternoBoleto = "55617",
                DataVencimento = new DateTime(2016, 11, 15),
                Moeda = "9"
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "23796697900000501810534020000005561700028010";
            Assert.AreEqual(codigoBarras.Length, boleto.CodigoBarraBoleto.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }
        
        [TestMethod]
        public void FormatarExibicaoCodigoCedenteCarteira02Bradesco()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("0534", "7", "2801", "0");
            var cedente = new Cedente("4879962", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca {Codigo = "02", BancoEmiteBoleto = false};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "55617",
                ValorBoleto = Convert.ToDecimal(501.81),
                IdentificadorInternoBoleto = "55617",
                DataVencimento = new DateTime(2016, 11, 15),
                Moeda = "9"
            };

            banco.FormatarBoleto(boleto);

            const string codigoCedenteFormatado = "0534-7/0002801-0";
            Assert.AreEqual(codigoCedenteFormatado, boleto.CedenteBoleto.CodigoCedenteFormatado);
        }

        #endregion

        #region Carteira 04

        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void FormatarNumeroDocumentoCarteira04Bradesco()
        {
            var boleto = new Boleto();
            var bradesco = new BancoBradesco();

            boleto.NumeroDocumento = "123";
            boleto.TipoArquivo = TipoArquivo.Cnab400;
            bradesco.FormataNumeroDocumento(boleto);

            Assert.AreEqual("0000000123", boleto.NumeroDocumento);

            boleto.NumeroDocumento = "0";
            bradesco.FormataNumeroDocumento(boleto);
            Assert.Fail();
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteira04Bradesco()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("1923", "2", "31718", "7");
            var cedente = new Cedente("4872529", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca { Codigo = "04", BancoEmiteBoleto = false };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "57582",
                ValorBoleto = Convert.ToDecimal(472.50),
                IdentificadorInternoBoleto = "57582",
                DataVencimento = new DateTime(2016, 11, 10),
                Moeda = "9"
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual("04/00000057582-2", boleto.NossoNumeroFormatado);
            Assert.AreEqual("2", boleto.DigitoNossoNumero);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteira04Bradesco()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("1923", "2", "31718", "7");
            var cedente = new Cedente("4872529", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca { Codigo = "04", BancoEmiteBoleto = false };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "57582",
                ValorBoleto = Convert.ToDecimal(472.50),
                IdentificadorInternoBoleto = "57582",
                DataVencimento = new DateTime(2016, 11, 10),
                Moeda = "9"
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "23791.92301 40000.005757 82003.171806 9 69740000047250";
            Assert.AreEqual(linhaDigitavel.Length, boleto.LinhaDigitavelBoleto.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteira04Bradesco()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("1923", "2", "31718", "7");
            var cedente = new Cedente("4872529", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca { Codigo = "04", BancoEmiteBoleto = false };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "57582",
                ValorBoleto = Convert.ToDecimal(472.50),
                IdentificadorInternoBoleto = "57582",
                DataVencimento = new DateTime(2016, 11, 10),
                Moeda = "9"
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "23799697400000472501923040000005758200317180";
            Assert.AreEqual(codigoBarras.Length, boleto.CodigoBarraBoleto.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void FormatarExibicaoCodigoCedenteCarteira04Bradesco()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("1923", "2", "31718", "7");
            var cedente = new Cedente("4872529", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca { Codigo = "04", BancoEmiteBoleto = false };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "57582",
                ValorBoleto = Convert.ToDecimal(472.50),
                IdentificadorInternoBoleto = "57582",
                DataVencimento = new DateTime(2016, 11, 10),
                Moeda = "9"
            };

            banco.FormatarBoleto(boleto);

            const string codigoCedenteFormatado = "1923-2/0031718-7";
            Assert.AreEqual(codigoCedenteFormatado, boleto.CedenteBoleto.CodigoCedenteFormatado);
        }

        #endregion

        #region Carteira 09

        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void FomatarNumeroDocumentoCarteira09Bradesco()
        {
            var boleto = new Boleto();
            var bradesco = new BancoBradesco();

            boleto.NumeroDocumento = "123";
            boleto.TipoArquivo = TipoArquivo.Cnab400;
            bradesco.FormataNumeroDocumento(boleto);

            Assert.AreEqual("0000000123", boleto.NumeroDocumento);

            boleto.NumeroDocumento = "0";
            bradesco.FormataNumeroDocumento(boleto);
            Assert.Fail();
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteira09Bradesco()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("1923", "2", "31718", "7");
            var cedente = new Cedente("4872529", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca {Codigo = "09", BancoEmiteBoleto = false};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "57582",
                ValorBoleto = Convert.ToDecimal(472.50),
                IdentificadorInternoBoleto = "57582",
                DataVencimento = new DateTime(2016, 11, 10),
                Moeda = "9"
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual("09/00000057582-0", boleto.NossoNumeroFormatado);
            Assert.AreEqual("0", boleto.DigitoNossoNumero);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteira09Bradesco()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("1923", "2", "31718", "7");
            var cedente = new Cedente("4872529", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca {Codigo = "09", BancoEmiteBoleto = false};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "57582",
                ValorBoleto = Convert.ToDecimal(472.50),
                IdentificadorInternoBoleto = "57582",
                DataVencimento = new DateTime(2016, 11, 10),
                Moeda = "9"
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "23791.92301 90000.005752 82003.171806 6 69740000047250";
            Assert.AreEqual(linhaDigitavel.Length, boleto.LinhaDigitavelBoleto.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteira09Bradesco()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("1923", "2", "31718", "7");
            var cedente = new Cedente("4872529", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca {Codigo = "09", BancoEmiteBoleto = false};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "57582",
                ValorBoleto = Convert.ToDecimal(472.50),
                IdentificadorInternoBoleto = "57582",
                DataVencimento = new DateTime(2016, 11, 10),
                Moeda = "9"
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "23796697400000472501923090000005758200317180";
            Assert.AreEqual(codigoBarras.Length, boleto.CodigoBarraBoleto.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void FormatarExibicaoCodigoCedenteCarteira09Bradesco()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("1923", "2", "31718", "7");
            var cedente = new Cedente("4872529", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca {Codigo = "09", BancoEmiteBoleto = false};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "57582",
                ValorBoleto = Convert.ToDecimal(472.50),
                IdentificadorInternoBoleto = "57582",
                DataVencimento = new DateTime(2016, 11, 10),
                Moeda = "9"
            };

            banco.FormatarBoleto(boleto);

            const string codigoCedenteFormatado = "1923-2/0031718-7";
            Assert.AreEqual(codigoCedenteFormatado, boleto.CedenteBoleto.CodigoCedenteFormatado);
        }

        #endregion

        #region Carteira 06

        [TestMethod]
        public void FormataNumeroDocumentoCarteira06Bradesco()
        {
            var boleto = new Boleto();
            var bradesco = new BancoBradesco();

            const string numeroDocumento = "123";

            const string valorEsperadoCnab240 = "000000000000123";
            const string valorEsperadoCnab400 = "0000000123";

            boleto.NumeroDocumento = numeroDocumento;
            boleto.TipoArquivo = TipoArquivo.Cnab400;
            bradesco.FormataNumeroDocumento(boleto);

            var numeroDocumentoFormatado = boleto.NumeroDocumento;

            Assert.AreEqual(valorEsperadoCnab400, numeroDocumentoFormatado);
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteira06Bradesco()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("2374", "4", "0165199", "4");
            var cedente = new Cedente("9999999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca {Codigo = "06"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "41636135093",
                ValorBoleto = Convert.ToDecimal(221.40),
                IdentificadorInternoBoleto = "41636135093",
                DataVencimento = new DateTime(2014, 07, 10),
                Moeda = "9"
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual("06/41636135093-P", boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteira06Bradesco()
        {
            /* 
             * Teste baseado em um boleto apresentado na documentação oficial do Banco da Amazônia versão 1
             * Manual de Cobrança Registrada Simples
             */
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("1234", "8", "12345", "6");
            var cedente = new Cedente("99999", "1", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "06"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "3242",
                ValorBoleto = Convert.ToDecimal(275),
                IdentificadorInternoBoleto = "3242",
                DataVencimento = new DateTime(2014, 08, 04)
            };

            banco.FormatarBoleto(boleto);

            const string valorEsperado = "23761.23408 60000.000327 42001.234501 7 61450000027500";
            Assert.AreEqual(valorEsperado.Length, boleto.LinhaDigitavelBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteira06Bradesco()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("237", "2");
            var contaBancariaCedente = new ContaBancaria("2374", "4", "0165199", "4");
            var cedente = new Cedente("9999999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca {Codigo = "06"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "41636135093",
                ValorBoleto = Convert.ToDecimal(221.40),
                IdentificadorInternoBoleto = "41636135093",
                DataVencimento = new DateTime(2014, 07, 10),
                Moeda = "9"
            };

            banco.FormatarBoleto(boleto);

            const string valorEsperado = "23797612000000221402374064163613509301651990";
            Assert.AreEqual(valorEsperado.Length, boleto.CodigoBarraBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.CodigoBarraBoleto);
        }

        #endregion
    }
}
