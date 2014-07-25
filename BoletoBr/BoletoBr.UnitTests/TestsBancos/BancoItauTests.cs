using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.Tests.Bancos.ITAU
{
    [TestClass]
    public class BancoItauTests
    {
        [TestMethod]
        public void TesteCalculoNossoNumeroCarteira198BoletoDocumentacaoItau()
        {
            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");

            var contaBancariaCedente = new ContaBancaria("0057", "", "", "");

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

            var boleto = new Boleto(cedente, sacado, banco.GetCarteiraCobrancaPorCodigo("110"));
            boleto.NumeroDocumento = "1234567";
            boleto.ValorBoleto = 12345;
            boleto.SequencialNossoNumero = "1234567";
            boleto.DataVencimento = new DateTime(2002, 05, 01);

            banco.FormataNossoNumero(boleto);

            Assert.AreEqual("110/01234567-0", boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void TesteLinhaDigitavelCarteira198BoletoDocumentacaoItau()
        {
            /* 
             * Teste baseado em um boleto apresentado na documentação oficial do Banco Itaú
             */
            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");

            var contaBancariaCedente = new ContaBancaria("0057", "", "01234", "5");

            var cedente = new Cedente("99999", "1", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente, null);

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

            var boleto = new Boleto(cedente, sacado, banco.GetCarteiraCobrancaPorCodigo("109"));
            boleto.NumeroDocumento = "1234567";
            boleto.ValorBoleto = Convert.ToDecimal(123.45);
            boleto.SequencialNossoNumero = "1234567";
            boleto.DataVencimento = new DateTime(2014, 05, 01);

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
            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");

            var contaBancariaCedente = new ContaBancaria("0057", "", "12345", "");

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

            var boleto = new Boleto(cedente, sacado, banco.GetCarteiraCobrancaPorCodigo("110"));
            boleto.NumeroDocumento = "1234567";
            boleto.ValorBoleto = Convert.ToDecimal(123.45);
            boleto.SequencialNossoNumero = "1234567";
            boleto.DataVencimento = new DateTime(2014, 05, 01);

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);

            const string valorEsperado = "34195605000000123451101234567880057123457000";
            Assert.AreEqual(valorEsperado.Length, boleto.CodigoBarraBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.CodigoBarraBoleto);
        }
    }
}
