using System;
using BoletoBr.Arquivo;
using BoletoBr.Dominio;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.Tests.Bancos.ITAU
{
    [TestClass]
    public class BancoItauTests
    {
        [TestMethod]
        public void TesteCalculoNossoNumeroCarteira198BoletoDocumentacaoItau()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");

            var contaBancariaCedente = new ContaBancaria("0057", "", "72192", "");

            var cedente = new Cedente("9999999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

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

            carteira.Codigo = "198";

            var boleto = new Boleto(carteira, cedente, sacado, remessa);
            boleto.NumeroDocumento = "1234567";
            boleto.ValorBoleto = 12345;
            boleto.SequencialNossoNumero = "1234567";
            boleto.DataVencimento = new DateTime(2002, 05, 01);

            banco.ValidaBoletoComNormasBanco(boleto);
            banco.FormataNossoNumero(boleto);

            Assert.AreEqual("198/01234567-8", boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void TesteLinhaDigitavelCarteira198BoletoDocumentacaoItau()
        {
            /* 
             * Teste baseado em um boleto apresentado na documentação oficial do Banco Itaú
             */
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");

            var contaBancariaCedente = new ContaBancaria("0057", "", "72192", "");

            var cedente = new Cedente("9999999", "1", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente, null);

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

            carteira.Codigo = "19";

            var boleto = new Boleto(carteira, cedente, sacado, remessa);
            boleto.NumeroDocumento = "1234567";
            boleto.ValorBoleto = Convert.ToDecimal(123.45);
            boleto.SequencialNossoNumero = "1234567";
            boleto.DataVencimento = new DateTime(2014, 05, 01);

            banco.ValidaBoletoComNormasBanco(boleto);
            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const string valorEsperado = "34191.10121 34567.880058 71234.570001 6 60500000012345";
            Assert.AreEqual(valorEsperado.Length, boleto.LinhaDigitavelBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void TesteCalculoCodigoBarrasCarteira198BoletoDocumentacaoItau()
        {
            /* 
             * Teste baseado em um boleto apresentado na documentação oficial do Banco Itaú
             */
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");

            var contaBancariaCedente = new ContaBancaria("4343", "", "29550", "9");

            var cedente = new Cedente("12345", "1", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente, null);

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

            carteira.Codigo = "175";

            var boleto = new Boleto(carteira, cedente, sacado, remessa);
            boleto.NumeroDocumento = "71194120";
            boleto.ValorBoleto = Convert.ToDecimal(1419.04);
            boleto.SequencialNossoNumero = "71194120";
            boleto.DataVencimento = new DateTime(2014, 05, 01);

            banco.ValidaBoletoComNormasBanco(boleto);
            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);

            const string valorEsperado = "34195605000000123451101234567880057123457000";
            Assert.AreEqual(valorEsperado.Length, boleto.CodigoBarraBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.CodigoBarraBoleto);
        }
    }
}
