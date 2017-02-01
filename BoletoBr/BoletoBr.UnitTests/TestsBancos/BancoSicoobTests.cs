using System;
using BoletoBr.Arquivo;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancos
{
    [TestClass]
    public class BancoSicoobTests
    {
        #region Carteira 1/01

        [TestMethod]
        public void FomatarNumeroDocumentoCarteira101Sicoob()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("756", "0");
            var contaBancariaCedente = new ContaBancaria("5004", "0", "107228", "5");
            var cedente = new Cedente("28796", 2, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "1/01"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "106",
                ValorBoleto = Convert.ToDecimal(465.82),
                IdentificadorInternoBoleto = "106",
                DataVencimento = new DateTime(2016, 11, 30)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NumeroDocumento.Length, 10);
            Assert.AreEqual("0000000106", boleto.NumeroDocumento);
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteira101Sicoob()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("756", "0");
            var contaBancariaCedente = new ContaBancaria("5004", "0", "107228", "5");
            var cedente = new Cedente("28796", 2, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "1/01"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "106",
                ValorBoleto = Convert.ToDecimal(465.82),
                IdentificadorInternoBoleto = "106",
                DataVencimento = new DateTime(2016, 11, 30)
            };

            banco.FormatarBoleto(boleto);

            const string nossoNumeroFormatado = "0000106-1";
            Assert.AreEqual(nossoNumeroFormatado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(nossoNumeroFormatado, boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteira101Sicoob()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("756", "0");
            var contaBancariaCedente = new ContaBancaria("5004", "0", "107228", "5");
            var cedente = new Cedente("28796", 2, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "1/01"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "106",
                ValorBoleto = Convert.ToDecimal(465.82),
                IdentificadorInternoBoleto = "106",
                DataVencimento = new DateTime(2016, 11, 30)
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "75691.50043 01028.796207 00010.610012 1 69940000046582";
            Assert.AreEqual(boleto.LinhaDigitavelBoleto.Length, linhaDigitavel.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteira101Sicoob()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("756", "0");
            var contaBancariaCedente = new ContaBancaria("5004", "0", "107228", "5");
            var cedente = new Cedente("28796", 2, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "1/01"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "106",
                ValorBoleto = Convert.ToDecimal(465.82),
                IdentificadorInternoBoleto = "106",
                DataVencimento = new DateTime(2016, 11, 30)
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "75691699400000465821500401028796200001061001";
            Assert.AreEqual(boleto.CodigoBarraBoleto.Length, codigoBarras.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
			Assert.AreEqual(boleto.CedenteBoleto.CodigoCedenteFormatado, "5004/00287962");
        }

        [TestMethod]
        public void FormatarExibicaoCodigoCedenteCarteira101Sicoob()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("756", "0");
            var contaBancariaCedente = new ContaBancaria("5004", "0", "107228", "5");
            var cedente = new Cedente("28796", 2, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "1/01"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "106",
                ValorBoleto = Convert.ToDecimal(465.82),
                IdentificadorInternoBoleto = "106",
                DataVencimento = new DateTime(2016, 11, 30)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual("5004/01072285", boleto.CedenteBoleto.CodigoCedenteFormatado);
        }

        #endregion
    }
}
