using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoletoBr.Arquivo;
using BoletoBr.Enums;

namespace BoletoBr.UnitTests.TestsBancos
{
    [TestClass]
    public class BancoSafraTests
    {
        #region Carteira

        [TestMethod]
        public void FomatarNumeroDocumentoSafra()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("422", "7");
            var contaBancariaCedente = new ContaBancaria("12345", "0", "38383", "0");
            var cedente = new Cedente("38383", 4, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "1"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "94550200",
                ValorBoleto = Convert.ToDecimal(268.06),
                IdentificadorInternoBoleto = "94550200",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NumeroDocumento.Length, 8);
            Assert.AreEqual("94550200", boleto.NumeroDocumento);
        }

        [TestMethod]
        public void FormatarNossoNumeroSafra()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("422", "7");
            var contaBancariaCedente = new ContaBancaria("12345", "0", "38383", "0");
            var cedente = new Cedente("38383", 4, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "1"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "94550200",
                ValorBoleto = Convert.ToDecimal(268.06),
                IdentificadorInternoBoleto = "94550200",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            const string nossoNumeroFormatado = "945502001";
            Assert.AreEqual(nossoNumeroFormatado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(nossoNumeroFormatado, boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void FormatarNossoNumeroSafra2()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("422", "7");
            var contaBancariaCedente = new ContaBancaria("12345", "0", "38383", "0");
            var cedente = new Cedente("38383", 4, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "1"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "93199999",
                ValorBoleto = Convert.ToDecimal(268.06),
                IdentificadorInternoBoleto = "93199999",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            const string nossoNumeroFormatado = "931999995";
            Assert.AreEqual(nossoNumeroFormatado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(nossoNumeroFormatado, boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void FormatarNossoNumeroSafra3()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("422", "7");
            var contaBancariaCedente = new ContaBancaria("12345", "0", "38383", "0");
            var cedente = new Cedente("38383", 4, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "1"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "26173001",
                ValorBoleto = Convert.ToDecimal(268.06),
                IdentificadorInternoBoleto = "26173001",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            const string nossoNumeroFormatado = "261730011";
            Assert.AreEqual(nossoNumeroFormatado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(nossoNumeroFormatado, boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelSafra()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("422", "7");
            var contaBancariaCedente = new ContaBancaria("12345", "0", "38383", "0");
            var cedente = new Cedente("38383", 4, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "1"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "93199999",
                ValorBoleto = Convert.ToDecimal(268.06),
                IdentificadorInternoBoleto = "93199999",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "42267.12349 50000.383831 09319.999927 5 69690000026806";
            Assert.AreEqual(boleto.LinhaDigitavelBoleto.Length, linhaDigitavel.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasSafra()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("422", "7");
            var contaBancariaCedente = new ContaBancaria("12345", "0", "38383", "0");
            var cedente = new Cedente("38383", 4, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "1"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "93199999",
                ValorBoleto = Convert.ToDecimal(268.06),
                IdentificadorInternoBoleto = "93199999",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "42265696900000268067123450000383830931999992";
            Assert.AreEqual(boleto.CodigoBarraBoleto.Length, codigoBarras.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }

        #endregion
    }
}
