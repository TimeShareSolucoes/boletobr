using System;
using BoletoBr.Dominio;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.Tests.Bancos.HSBC
{
    [TestClass]
    public class HsbcCarteiraCnrTipoIdentificador4
    {
        [TestMethod]
        public void TesteCalculoNossoNumeroBoletoReal()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");

            var contaBancariaCedente = new ContaBancaria("", "", "", "");

            var cedente = new Cedente("4295579", 0, "99.999.999/9999-99", "Razão Social Teste Cálculo Nosso Número Ltda", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca();

            carteira.Codigo = "CNR";

            var boleto = new Boleto(carteira, cedente, sacado, remessa);
            boleto.NumeroDocumento = "40156";
            boleto.ValorBoleto = 200;
            boleto.SequencialNossoNumero = "40156";
            boleto.DataVencimento = new DateTime(2012, 11, 15);

            banco.FormataNossoNumero(boleto);

            Assert.AreEqual("0000000040156044", boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void TesteLinhaDigitavelBoletoReal()
        {
            /* Teste baseado em um boleto real, ou seja, boleto válido. */
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");

            var contaBancariaCedente = new ContaBancaria("", "", "", "");

            var cedente = new Cedente("4295579", 0, "99.999.999/0001-99", "Razao Social Teste Linha Digitável", contaBancariaCedente, null);

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

            var carteira = new CarteiraCobranca();

            carteira.Codigo = "CNR";

            var boleto = new Boleto(carteira, cedente, sacado, remessa);
            boleto.NumeroDocumento = "40156";
            boleto.Moeda = "9";
            boleto.ValorBoleto = 200;
            boleto.CodigoDoProduto = "2";
            boleto.SequencialNossoNumero = "40156";
            boleto.DataVencimento = new DateTime(2012, 11, 15);

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            var valorEsperado = "39994.29552 79000.000004 40156.320224 1 55180000020000";
            Assert.AreEqual(valorEsperado.Length, boleto.LinhaDigitavelBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.LinhaDigitavelBoleto);
        }
    }
}
