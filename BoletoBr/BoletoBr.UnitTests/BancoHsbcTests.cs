using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests
{
    [TestClass]
    public class BancoHsbcTests
    {
        /* 
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
        public void TesteCalculoNossoNumeroBoletoRealCNR()
        {
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");

            var contaBancariaCedente = new ContaBancaria("5036819", "", "00008", "7");

            var cedente = new Cedente("4295579", 0, "03.863.763/0001-54", "Adtur C. H. T. Ltd(CTC Travel)", contaBancariaCedente, null);

            var sacado = new Sacado("Time Share Soluções Ltda", "99.999.999/0001-99", new Endereco()
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
            boleto.NumeroDocumento = "850368";
            boleto.ValorBoleto = 330;
            boleto.SequencialNossoNumero = "850368";
            boleto.DataVencimento = new DateTime(2014, 7, 10);

            banco.FormataNossoNumero(boleto);

            Assert.AreEqual("0000000850368047", boleto.NossoNumeroFormatado);
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
        
        [TestMethod]
        public void TesteCalculoNossoNumero()
        {
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");

            var contaBancariaCedente = new ContaBancaria("2698", "", "00323", "46");

            var cedente = new Cedente("11111", 0, "67.581.019/0001-40", "Razão Social Teste", contaBancariaCedente, null);

            var sacado = new Sacado("Fulano de Tal", "00000000000", new Endereco()
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
            boleto.ValorBoleto = 200;
            boleto.SequencialNossoNumero = "22222";
            boleto.NossoNumeroFormatado = "40156";
            boleto.DataVencimento = new DateTime(2012, 11, 15);

            banco.FormataNossoNumero(boleto);

            Assert.AreEqual("11111222225", boleto.NossoNumeroFormatado);
        }
    }
}