using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancos
{
    [TestClass]
    public class BancoBanestesTests
    {

        [TestMethod]
        public void FomatarNumeroDocumentoBanestes()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("021", "0");
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

            var carteira = new CarteiraCobranca { Codigo = "11" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "1803029901",
                ValorBoleto = Convert.ToDecimal(268.06),
                IdentificadorInternoBoleto = "21487805",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NumeroDocumento.Length, 10);
            Assert.AreEqual("1803029901", boleto.NumeroDocumento);
        }

        [TestMethod]
        public void FormatarNossoNumeroBanestes()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("021", "0");
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

            var carteira = new CarteiraCobranca { Codigo = "11" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "1803029901",
                ValorBoleto = Convert.ToDecimal(268.06),
                IdentificadorInternoBoleto = "21487805",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            const string nossoNumeroFormatado = "21487805.81";
            Assert.AreEqual(nossoNumeroFormatado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(nossoNumeroFormatado, boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void FormatarNossoNumeroBanestes2()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("021", "0");
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

            var carteira = new CarteiraCobranca { Codigo = "11" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "1803029901",
                ValorBoleto = Convert.ToDecimal(268.06),
                IdentificadorInternoBoleto = "93199999",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            const string nossoNumeroFormatado = "93199999.53";
            Assert.AreEqual(nossoNumeroFormatado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(nossoNumeroFormatado, boleto.NossoNumeroFormatado);
        }

    

        [TestMethod]
        public void CalcularLinhaDigitavelBanestes()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("021", "0");
            var contaBancariaCedente = new ContaBancaria("123", "0", "1.222.333", "0");
            var cedente = new Cedente("1222333", 4, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "11" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "18.030299.01",
                ValorBoleto = Convert.ToDecimal(2952.95),
                IdentificadorInternoBoleto = "21487805",
                DataVencimento = new DateTime(2014, 1, 18)
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "02192.14871 80500.001229 23334.021815 7 59470000295295";
            Assert.AreEqual(boleto.LinhaDigitavelBoleto.Length, linhaDigitavel.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasBanestes()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("021", "0");
            var contaBancariaCedente = new ContaBancaria("123", "0", "1.222.333", "0");
            var cedente = new Cedente("1222333", 4, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "11" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "18.030299.01",
                ValorBoleto = Convert.ToDecimal(2952.95),
                IdentificadorInternoBoleto = "21487805",
                DataVencimento = new DateTime(2014, 1, 18)
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "02197594700002952952148780500001222333402181";
            Assert.AreEqual(boleto.CodigoBarraBoleto.Length, codigoBarras.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }
    }
}


