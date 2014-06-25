using System;
using System.Diagnostics;
using BoletoBr.Bancos.Hsbc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests
{
    [TestClass]
    public class BancoHsbcTests
    {
        /* NOTA
         * /!\ Carteira CNR /!\
         * Agência: 4 dígitos
         * Cód. Cedente: 7 dígitos
         * Nosso Número: 13 dígitos
         * 
         * /!\ Carteira CSB /!\
         * Agência: 4 dígitos
         * Cód. Cedente: 7 dígitos
         * Nosso Número: 5 dígitos
         * Cód. Cliente: 5 dígitos
         */

        [TestMethod]
        public void TesteCalculoCodigoBarrasCarteiraCnrHsbcBoletoDocumentacaoHsbc()
        {
            /* 
             * Teste baseado em um boleto apresentado na documentação oficial do hsbc.
             * Cobrança Não Registrada
             * Manual de Emissão Empresa
             */
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");

            var contaBancariaCedente = new ContaBancaria("", "", "", "");

            var cedente = new Cedente("8351202", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente, null);

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

            var boleto = new Boleto(cedente, sacado, banco.GetCarteiraCobrancaPorCodigo("CNR"));
            boleto.NumeroDocumento = "0000239104761";
            boleto.Moeda = 9;
            boleto.ValorBoleto = 1200;
            boleto.DataFormatoJuliano = "1868";
            boleto.CodigoDoProduto = "2";
            boleto.SequencialNossoNumero = "0000239104761";
            boleto.DataVencimento = new DateTime(2008, 7, 4);

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);

            var valorEsperado = "39994392300001200008351202000023910476118682";
            Assert.AreEqual(valorEsperado.Length, boleto.CodigoBarraBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void TesteCalculoCodigoBarrasCarteiraCnrHsbcBoletoReal()
        {
            /* Teste baseado em um boleto real, ou seja, boleto válido. */
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");

            var contaBancariaCedente = new ContaBancaria("", "", "", "");

            var cedente = new Cedente("4295579", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente, null);

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

            var boleto = new Boleto(cedente, sacado, banco.GetCarteiraCobrancaPorCodigo("CNR"));
            boleto.NumeroDocumento = "40156";
            boleto.Moeda = 9;
            boleto.ValorBoleto = 200;
            boleto.DataFormatoJuliano = "3202";
            boleto.CodigoDoProduto = "2";
            boleto.SequencialNossoNumero = "40156";
            boleto.DataVencimento = new DateTime(2012, 11, 15);

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);

            var valorEsperado = "39991551800000200004295579000000004015632022";
            Assert.AreEqual(valorEsperado.Length, boleto.CodigoBarraBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void TestCalcularPrimeiroDigitoVerificadorNossoNumeroCarteiraCnrTipo4()
        {
            var codigoPagador = "239104761";
            var resultadoEsperado = "9";


            var banco = new Bancos.Hsbc.BancoHsbc();

            var resultadoObtido = banco.CalculaPrimeiroDigitoVerificadorCnrTipo4(codigoPagador);
            Assert.AreEqual(resultadoEsperado, resultadoObtido);
            
        }

        [TestMethod]
        public void TestCalcularSegundoDigitoVerificadorNossoNumeroCarteiraCnrTipo4()
        {
            var banco = new Bancos.Hsbc.BancoHsbc();

            var codigoPagador = "239104761";
            var primeiroDigitoVerificador = banco.CalculaPrimeiroDigitoVerificadorCnrTipo4(codigoPagador);

            string codigoBeneficiario = "8351202";
            DateTime dataVencimento = new DateTime(2008, 7, 4);

            string segundoDigitoVerificador =
                banco.CalculaSegundoDigitoVerificadorCnrTipo4(
                codigoPagador, primeiroDigitoVerificador, codigoBeneficiario, dataVencimento);

            Assert.AreEqual(segundoDigitoVerificador, "1");
        }
    }
}