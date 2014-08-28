using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var boleto = new Boleto(cedente, sacado, banco.GetCarteiraCobrancaPorCodigo("102"));
            boleto.NumeroDocumento = "001N002";
            boleto.ValorBoleto = Convert.ToDecimal(80.55);
            boleto.SequencialNossoNumero = "000000000027";
            boleto.DataVencimento = new DateTime(2012, 11, 26);

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);

            const string valorEsperado = "03391552900000080559052765400000000002720102";
            Assert.AreEqual(valorEsperado.Length, boleto.CodigoBarraBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void BSantanderCalculoLinhaDigitavel()
        {
            /* 
            * Teste baseado em um boleto apresentado na documentação oficial do Banco Santander
            */
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

            var boleto = new Boleto(cedente, sacado, banco.GetCarteiraCobrancaPorCodigo("102"));
            boleto.NumeroDocumento = "0000006152007";
            boleto.ValorBoleto = Convert.ToDecimal(252.00);
            boleto.SequencialNossoNumero = "0000006152007";
            boleto.DataVencimento = new DateTime(2014, 07, 07);

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const string valorEsperado = "03399.65451 42400.000065 15200.701025 1 61170000025200";
            Assert.AreEqual(valorEsperado.Length, boleto.LinhaDigitavelBoleto.Length);
            Assert.AreEqual(valorEsperado, boleto.LinhaDigitavelBoleto);
        }
    }
}
