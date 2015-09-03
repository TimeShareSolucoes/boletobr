using System;
using BoletoBr.Arquivo;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancos
{
    [TestClass]
    public class BancoSantanderTests
    {
        [TestMethod]
        public void BSantanderCalculoCodigoDeBarras()
        {
            /* 
            * Teste baseado em um boleto apresentado na documentação oficial do Banco Santander
            */
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

            var carteira = new CarteiraCobranca {Codigo = "102"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "001N002",
                ValorBoleto = Convert.ToDecimal(80.55),
                IdentificadorInternoBoleto = "000000000027",
                DataVencimento = new DateTime(2012, 11, 26)
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);

            const string valorEsperado = "03391552900000080559052765400000000002720102";
            Assert.AreEqual(valorEsperado.Length, boleto.CodigoBarraBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void BSantanderCalculoCodigoDeBarras2()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("033", "7");

            var contaBancariaCedente = new ContaBancaria("0020", "5", "13003695", "9");

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
                NumeroDocumento = "19457",
                ValorBoleto = Convert.ToDecimal(337.00),
                IdentificadorInternoBoleto = "19457",
                DataVencimento = new DateTime(2015, 08, 05)
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);

            const string valorEsperado = "03396651100000337009052765400000001945730101";
            Assert.AreEqual(valorEsperado.Length, boleto.CodigoBarraBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void BSantanderCalculoLinhaDigitavel()
        {
           /* 
            * Teste baseado em um boleto apresentado na documentação oficial do Banco Santander
            */
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("033", "7");

            var contaBancariaCedente = new ContaBancaria("4165", "", "", "");

            var cedente = new Cedente("6545424", "1", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca {Codigo = "102"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "0000006152007",
                ValorBoleto = Convert.ToDecimal(252.00),
                IdentificadorInternoBoleto = "000000615200",
                DataVencimento = new DateTime(2014, 07, 07)
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const string valorEsperado = "03399.65451 42400.000065 15200.701025 1 61170000025200";
            Assert.AreEqual(valorEsperado.Length, boleto.LinhaDigitavelBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void BSantanderGeracaoDvNossoNumero()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("033", "7");

            var contaBancariaCedente = new ContaBancaria("4165", "", "", "");

            var cedente = new Cedente("6545424", "1", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca {Codigo = "102"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                //NumeroDocumento = "0000006152007",
                NumeroDocumento = "0000000014070",
                ValorBoleto = Convert.ToDecimal(252.00),
                IdentificadorInternoBoleto = "14070",
                DataVencimento = new DateTime(2014, 07, 07)
            };

            banco.FormataNossoNumero(boleto);

            const string valorEsperado = "0000000140709";
            Assert.AreEqual(valorEsperado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(valorEsperado, boleto.NossoNumeroFormatado);
        }
    }
}
