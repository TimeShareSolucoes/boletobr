using System;
using BoletoBr.Arquivo;
using BoletoBr.Dominio;
using BoletoBr.Enums;
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

        [TestMethod]
        public void formatanumerodoc()
        {
            var tamanhomaximo = 15;
            var valor1 = 1234;
            var valor2 = "XxXX";

            var lenghtValor1 = valor1.ToString().Length;

            //var numeroDocCalculado = valor1 + valor2.PadRight(tamanhomaximo - lenghtValor1, '0');
            var numeroDocCalculado = valor1.ToString().PadLeft(tamanhomaximo - lenghtValor1, '0') + valor2;

            Assert.AreEqual(numeroDocCalculado.Length, tamanhomaximo);

        }

        #region Carteira RG - REGISTRADA

        [TestMethod]
        public void TesteCalculoNossoNumeroCarteiraRgDocumentacaoCef()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

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

            var carteira = new CarteiraCobranca();

            carteira.Codigo = "RG";

            var boleto = new Boleto(carteira, cedente, sacado, remessa);
            boleto.NumeroDocumento = "19";
            boleto.ValorBoleto = 1000;
            boleto.IdentificadorInternoBoleto = "19";
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
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

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

            var carteira = new CarteiraCobranca();

            carteira.Codigo = "RG";

            var boleto = new Boleto(carteira, cedente, sacado, remessa);
            boleto.NumeroDocumento = "222333777777777";
            boleto.ValorBoleto = Convert.ToDecimal(321.12);
            boleto.IdentificadorInternoBoleto = "222333777777777";
            boleto.DataVencimento = new DateTime(2006, 08, 23);

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const string valorEsperado = "10410.05503 77222.133348 77777.777713 1 32420000032112";
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
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

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

            var carteira = new CarteiraCobranca();

            carteira.Codigo = "RG";

            var boleto = new Boleto(carteira, cedente, sacado, remessa);
            boleto.NumeroDocumento = "222333777777777";
            boleto.ValorBoleto = Convert.ToDecimal(321.12);
            boleto.IdentificadorInternoBoleto = "222333777777777";
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
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

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

            var carteira = new CarteiraCobranca();

            carteira.Codigo = "SR";

            var boleto = new Boleto(carteira, cedente, sacado, remessa);
            boleto.NumeroDocumento = "19";
            boleto.ValorBoleto = 1000;
            boleto.IdentificadorInternoBoleto = "19";
            boleto.DataVencimento = new DateTime(2014, 06, 30);

            banco.FormataNossoNumero(boleto);

            //Providenciar boleto real para efetuar o teste da Carteira SEM REGISTRO
            Assert.AreEqual("24000000000019", boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void TesteLinhaDigitavelCarteiraSrDocumentacaoCef()
        {
            /* 
             * Teste baseado em um boleto apresentado na documentação oficial do Banco da Amazônia versão 1
             * Manual de Cobrança Registrada Simples
             */
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

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

            var carteira = new CarteiraCobranca();

            carteira.Codigo = "SR";

            var boleto = new Boleto(carteira, cedente, sacado, remessa);
            boleto.NumeroDocumento = "123";
            boleto.ValorBoleto = Convert.ToDecimal(15.56);
            boleto.IdentificadorInternoBoleto = "123";
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
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("1839", "", "", "");

            var cedente = new Cedente("256803", "", 9, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente, null);

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

            carteira.Codigo = "SR";

            var boleto = new Boleto(carteira, cedente, sacado, remessa);
            boleto.NumeroDocumento = "090972714";
            boleto.ValorBoleto = Convert.ToDecimal(119.90);
            boleto.IdentificadorInternoBoleto = "46286";
            boleto.DataVencimento = new DateTime(2014, 09, 15);

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            const string valorEsperadoCodBar = "10493618700000119902568039000200040000462868"; // Código de Barras
            const string valorEsperadoLinhaDig = "10492.56801 39000.200046 00004.628681 3 61870000011990"; // Linha Digitável
            Assert.AreEqual(valorEsperadoCodBar.Length, boleto.CodigoBarraBoleto.Length);
            Assert.AreEqual(valorEsperadoCodBar, boleto.CodigoBarraBoleto);
            Assert.AreEqual(valorEsperadoLinhaDig.Length, boleto.LinhaDigitavelBoleto.Length);
            Assert.AreEqual(valorEsperadoLinhaDig, boleto.LinhaDigitavelBoleto);
        }

        #endregion Carteira SR - SEM REGISTRO
    }
}
