using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests
{
    [TestClass]
    public class BancoAmazoniaTests
    {
        /* 
         * 1 - Nosso número
         * 2 - Linha digitável
         * 3 - Código de barras
         */

        [TestMethod]
        public void TesteCalculoNossoNumeroBoletoDocumentacaoBasa()
        {
            var banco = Fabricas.BancoFactory.ObterBanco("003", "5");

            var contaBancariaCedente = new ContaBancaria("007", "8", "", "");

            var cedente = new Cedente("9999999", 0, "99.999.999/9999-99", "Razão Social Teste Cálculo Nosso Número Ltda", contaBancariaCedente, null);

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
            boleto.NumeroDocumento = "12345";
            boleto.ValorBoleto = 15.56;
            boleto.SequencialNossoNumero = "12345";
            boleto.DataVencimento = new DateTime(2008, 06, 27);
            

            banco.FormataNossoNumero(boleto);

            Assert.AreEqual("0000000000000123", boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void TesteLinhaDigitavelBoletoDocumentacaoBasa()
        {
            /* 
             * Teste baseado em um boleto apresentado na documentação oficial do Banco da Amazônia versão 1
             * Manual de Cobrança Registrada Simples
             */
            var banco = Fabricas.BancoFactory.ObterBanco("003", "5");

            var contaBancariaCedente = new ContaBancaria("007", "8", "", "");

            var cedente = new Cedente("99999", "01234567890123456789", 0, "99.999.999/0001-99", "Razao Social X", contaBancariaCedente, null);

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
            boleto.ValorBoleto = 15.56;
            boleto.DataVencimento = new DateTime(2008, 7, 4);
            boleto.SetNossoNumeroFormatado("0123456789000000");

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            var valorEsperado = "00390078020010000000900000012385839160000001556";
            Assert.AreEqual(valorEsperado.Length, boleto.LinhaDigitavelBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void TesteCalculoCodigoBarrasCarteiraCnrBoletoDocumentacaoBasa()
        {
            /* 
             * Teste baseado em um boleto apresentado na documentação oficial do Banco da Amazônia versão 1
             * Manual de Cobrança Registrada Simples
             */
            var banco = Fabricas.BancoFactory.ObterBanco("003", "5");

            var contaBancariaCedente = new ContaBancaria("", "", "", "");

            var cedente = new Cedente("8351202", 0, "99.999.999/0001-99", "Razao Social X", contaBancariaCedente, null);

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
            banco.FormataLinhaDigitavel(boleto);

            var valorEsperado = "39998351210200002391704761186826439230000120000";
            Assert.AreEqual(valorEsperado.Length, boleto.LinhaDigitavelBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.LinhaDigitavelBoleto);
        }
    }
}
