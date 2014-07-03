using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.Tests.Bancos.CEF
{
    [TestClass]
    public class BancoCefTests
    {
        /* 
         * Teste 1 - Nosso número
         * Teste 2 - Linha digitável
         * Teste 3 - Código de barras
         */

        #region Carteira RG - REGISTRADA

        [TestMethod]
        public void TesteCalculoNossoNumeroCarteiraRgDocumentacaoCef()
        {
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("007", "8", "", "");

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

            var boleto = new Boleto(cedente, sacado, banco.GetCarteiraCobrancaPorCodigo("RG"));
            boleto.NumeroDocumento = "19";
            boleto.ValorBoleto = 1000;
            boleto.SequencialNossoNumero = "19";
            boleto.DataVencimento = new DateTime(2014, 06, 30);

            banco.FormataNossoNumero(boleto);

            const string valorEsperado = "14000000000000019";
            Assert.AreEqual(valorEsperado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(valorEsperado, boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void TesteLinhaDigitavelCarteiraRgDocumentacaoCef()
        {
            /* 
             * Teste baseado em um boleto apresentado na documentação oficial do Banco da Amazônia versão 1
             * Manual de Cobrança Registrada Simples
             */
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("007", "8", "", "");

            var cedente = new Cedente("005507", "", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente, null);

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

            var boleto = new Boleto(cedente, sacado, banco.GetCarteiraCobrancaPorCodigo("RG"));
            boleto.NumeroDocumento = "222333777777777";
            boleto.ValorBoleto = Convert.ToDecimal(321.12);
            boleto.SequencialNossoNumero = "222333777777777";
            boleto.DataVencimento = new DateTime(2006, 08, 23);

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const string valorEsperado = "10490.05505 77222.133348 77777.777713 4 32420000032112";
            Assert.AreEqual(valorEsperado.Length, boleto.LinhaDigitavelBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void TesteCalculoCodigoBarrasCarteiraRgBoletoDocumentacaoCef()
        {
            /* 
             * Teste baseado em um boleto apresentado na documentação oficial do Banco da Amazônia versão 1
             * Manual de Cobrança Registrada Simples
             */
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("007", "8", "", "");

            var cedente = new Cedente("005507", "1", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente, null);

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

            var boleto = new Boleto(cedente, sacado, banco.GetCarteiraCobrancaPorCodigo("RG"));
            boleto.NumeroDocumento = "222333777777777";
            boleto.ValorBoleto = Convert.ToDecimal(321.12);
            boleto.SequencialNossoNumero = "222333777777777";
            boleto.DataVencimento = new DateTime(2006, 08, 23);

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);

            const string valorEsperado = "10494324200000321120055077222133347777777771";
            Assert.AreEqual(valorEsperado.Length, boleto.CodigoBarraBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.CodigoBarraBoleto);
        }

        #endregion Carteira RG - REGISTRADA

        #region Carteira SR - SEM REGISTRO

        [TestMethod]
        public void TesteCalculoNossoNumeroCarteiraSrDocumentacaoCef()
        {
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("007", "8", "", "");

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

            var boleto = new Boleto(cedente, sacado, banco.GetCarteiraCobrancaPorCodigo("SR"));
            boleto.NumeroDocumento = "19";
            boleto.ValorBoleto = 1000;
            boleto.SequencialNossoNumero = "19";
            boleto.DataVencimento = new DateTime(2014, 06, 30);

            banco.FormataNossoNumero(boleto);

            //Providenciar boleto rela para efetuar o teste da Carteira SEM REGISTRO
            Assert.AreEqual("24000000000019-", boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void TesteLinhaDigitavelCarteiraSrDocumentacaoCef()
        {
            /* 
             * Teste baseado em um boleto apresentado na documentação oficial do Banco da Amazônia versão 1
             * Manual de Cobrança Registrada Simples
             */
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("007", "8", "", "");

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

            var boleto = new Boleto(cedente, sacado, banco.GetCarteiraCobrancaPorCodigo("SR"));
            boleto.NumeroDocumento = "123";
            boleto.ValorBoleto = Convert.ToDecimal(15.56);
            boleto.SequencialNossoNumero = "123";
            boleto.DataVencimento = new DateTime(2008, 06, 27);

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const string valorEsperado = "00390.07802 00100.000009  00000.012385 8 39160000001556";
            Assert.AreEqual(valorEsperado.Length, boleto.LinhaDigitavelBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void TesteCalculoCodigoBarrasCarteiraSrBoletoDocumentacaoCef()
        {
            /* 
             * Teste baseado em um boleto apresentado na documentação oficial do Banco da Amazônia versão 1
             * Manual de Cobrança Registrada Simples
             */
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("007", "8", "", "");

            var cedente = new Cedente("8351202", "1", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente, null);

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

            var boleto = new Boleto(cedente, sacado, banco.GetCarteiraCobrancaPorCodigo("SR"));
            boleto.NumeroDocumento = "123";
            boleto.ValorBoleto = Convert.ToDecimal(15.56);
            boleto.SequencialNossoNumero = "123";
            boleto.DataVencimento = new DateTime(2008, 06, 27);

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const string valorEsperado = "00398391600000015560078000100000000000001238";
            Assert.AreEqual(valorEsperado.Length, boleto.CodigoBarraBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.CodigoBarraBoleto);
        }

        #endregion Carteira SR - SEM REGISTRO
    }
}
