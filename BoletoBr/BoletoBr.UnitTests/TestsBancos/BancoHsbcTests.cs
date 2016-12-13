using System;
using BoletoBr.Arquivo;
using BoletoBr.Bancos.Hsbc;
using BoletoBr.Dominio;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancos
{
    [TestClass]
    public class BancoHsbcTests
    {
        #region Carteira CSB

        [TestMethod]
        public void FomatarNumeroDocumentoCarteiraCsbHsbc()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");
            var contaBancariaCedente = new ContaBancaria("0134", "0", "05168", "42");
            var cedente = new Cedente("27599", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

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

            var carteira = new CarteiraCobranca {Codigo = "CSB"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "117",
                ValorBoleto = Convert.ToDecimal(34977.29),
                IdentificadorInternoBoleto = "117",
                DataVencimento = new DateTime(2016, 11, 10)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NumeroDocumento.Length, 13);
            Assert.AreEqual("0000000000117", boleto.NumeroDocumento);
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteiraCsbHsbc()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");
            var contaBancariaCedente = new ContaBancaria("0134", "0", "05168", "42");
            var cedente = new Cedente("27599", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

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

            var carteira = new CarteiraCobranca {Codigo = "CSB"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "117",
                ValorBoleto = Convert.ToDecimal(34977.29),
                IdentificadorInternoBoleto = "117",
                DataVencimento = new DateTime(2016, 11, 10)
            };

            banco.FormatarBoleto(boleto);

            const string nossoNumeroFormatado = "27599001170";
            Assert.AreEqual(nossoNumeroFormatado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(nossoNumeroFormatado, boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteiraCsbHsbc()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");
            var contaBancariaCedente = new ContaBancaria("0134", "0", "05168", "42");
            var cedente = new Cedente("27599", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

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

            var carteira = new CarteiraCobranca {Codigo = "CSB"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "117",
                ValorBoleto = Convert.ToDecimal(34977.29),
                IdentificadorInternoBoleto = "117",
                DataVencimento = new DateTime(2016, 11, 10)
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "39992.75997 00117.001347 05168.420015 1 69740003497729";
            Assert.AreEqual(boleto.LinhaDigitavelBoleto.Length, linhaDigitavel.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteiraCsbHsbc()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");
            var contaBancariaCedente = new ContaBancaria("0134", "0", "05168", "42");
            var cedente = new Cedente("27599", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

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

            var carteira = new CarteiraCobranca {Codigo = "CSB"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "117",
                ValorBoleto = Convert.ToDecimal(34977.29),
                IdentificadorInternoBoleto = "117",
                DataVencimento = new DateTime(2016, 11, 10)
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "39991697400034977292759900117001340516842001";
            Assert.AreEqual(boleto.CodigoBarraBoleto.Length, codigoBarras.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void FormatarExibicaoCodigoCedenteCarteiraCsbHsbc()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");
            var contaBancariaCedente = new ContaBancaria("0134", "0", "05168", "42");
            var cedente = new Cedente("27599", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

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

            var carteira = new CarteiraCobranca {Codigo = "CSB"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "117",
                ValorBoleto = Convert.ToDecimal(34977.29),
                IdentificadorInternoBoleto = "117",
                DataVencimento = new DateTime(2016, 11, 10)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual("0134/0516842", boleto.CedenteBoleto.CodigoCedenteFormatado);
        }

        #endregion

        #region Carteira CNR

        [TestMethod]
        public void FomatarNumeroDocumentoCarteiraCnrHsbc()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");
            var contaBancariaCedente = new ContaBancaria("1740", "0", "00135", "97");
            var cedente = new Cedente("4295579", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

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

            var carteira = new CarteiraCobranca {Codigo = "CNR"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "130741",
                ValorBoleto = Convert.ToDecimal(185.33),
                IdentificadorInternoBoleto = "130741",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NumeroDocumento.Length, 13);
            Assert.AreEqual("0000000130741", boleto.NumeroDocumento);
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteiraCnrHsbc()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");
            var contaBancariaCedente = new ContaBancaria("1740", "0", "00135", "97");
            var cedente = new Cedente("4295579", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

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

            var carteira = new CarteiraCobranca {Codigo = "CNR"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "130741",
                ValorBoleto = Convert.ToDecimal(185.33),
                IdentificadorInternoBoleto = "130741",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);

            const string nossoNumeroFormatado = "0000000130741042";
            Assert.AreEqual(nossoNumeroFormatado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(nossoNumeroFormatado, boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteiraCnrHsbc()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");
            var contaBancariaCedente = new ContaBancaria("1740", "0", "00135", "97");
            var cedente = new Cedente("4295579", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

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

            var carteira = new CarteiraCobranca {Codigo = "CNR"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "130741",
                ValorBoleto = Convert.ToDecimal(185.33),
                IdentificadorInternoBoleto = "130741",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "39994.29552 79000.000012 30741.320623 1 69790000018533";
            Assert.AreEqual(boleto.LinhaDigitavelBoleto.Length, linhaDigitavel.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteiraCnrHsbc()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");
            var contaBancariaCedente = new ContaBancaria("1740", "0", "00135", "97");
            var cedente = new Cedente("4295579", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

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

            var carteira = new CarteiraCobranca {Codigo = "CNR"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "130741",
                ValorBoleto = Convert.ToDecimal(185.33),
                IdentificadorInternoBoleto = "130741",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "39991697900000185334295579000000013074132062";
            Assert.AreEqual(boleto.CodigoBarraBoleto.Length, codigoBarras.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void FormatarExibicaoCodigoCedenteCarteiraCnrHsbc()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");
            var contaBancariaCedente = new ContaBancaria("1740", "0", "00135", "97");
            var cedente = new Cedente("4295579", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

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

            var carteira = new CarteiraCobranca {Codigo = "CNR"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "130741",
                ValorBoleto = Convert.ToDecimal(185.33),
                IdentificadorInternoBoleto = "130741",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual("1740/0013597", boleto.CedenteBoleto.CodigoCedenteFormatado);
        }

        #endregion

        #region Digitos

        [TestMethod]
        public void TestCalcularPrimeiroDigitoVerificadorNossoNumeroCarteiraCnrTipo4()
        {
            const string codigoPagador = "239104761";
            const string resultadoEsperado = "9";
            
            var banco = new BancoHsbc();

            var resultadoObtido = banco.CalculaPrimeiroDigitoVerificadorCnrTipo4(codigoPagador);
            Assert.AreEqual(resultadoEsperado, resultadoObtido);
        }

        [TestMethod]
        public void TestCalcularSegundoDigitoVerificadorNossoNumeroCarteiraCnrTipo4()
        {
            var banco = new BancoHsbc();

            const string codigoPagador = "239104761";
            var primeiroDigitoVerificador = banco.CalculaPrimeiroDigitoVerificadorCnrTipo4(codigoPagador);

            const string codigoBeneficiario = "8351202";
            var dataVencimento = new DateTime(2008, 7, 4);

            var segundoDigitoVerificador =
                banco.CalculaSegundoDigitoVerificadorCnrTipo4(
                    codigoPagador, primeiroDigitoVerificador, codigoBeneficiario, dataVencimento);

            Assert.AreEqual(segundoDigitoVerificador, "1");
        }

        [TestMethod]
        public void TestCalculoDigitoAutoConferenciaBoletoCarteiraCnrHsbcMod11Peso2a9()
        {
            /*
             * Valor Base Cálculo obtido na documentação Oficial do HSBC.
             * Manual de Emissão Empresa
             * Cobrança Não Registrada
             */
            const string valorBaseCalculo = "3999392300001200008351202000023910476118682";

            var dacCalculado = Common.Mod11Peso2a9(valorBaseCalculo);

            Assert.AreEqual(4, dacCalculado);
        }

        [TestMethod]
        public void TestCalculoDigitoAutoConferenciaBoletoCarteiraCnrHsbcMod11Peso2a9BaseadoEmBoletoReal()
        {
            /*
             * Baseado na linha digitável de um boleto real
             */
            const string codBanco = "3999";
            const string fatorVencimento = "5518";
            const string valorDocumento10Dig = "0000020000";
            const string codigoBeneficiario7Dig = "4295579";
            const string codDocumento13Dig = "0000000040156";
            const string dataVencFormatoJuliano = "3202";
            const string codProdutoCnr = "2";
            const string valorBaseCalculo =
                codBanco + fatorVencimento + valorDocumento10Dig + codigoBeneficiario7Dig + codDocumento13Dig +
                dataVencFormatoJuliano + codProdutoCnr;

            Assert.AreEqual(43, valorBaseCalculo.Length);

            var dacCalculado =
                Common.Mod11Peso2a9(valorBaseCalculo);

            Assert.AreEqual(1, dacCalculado);
        }

        #endregion
    }
}