using System;
using BoletoBr.Arquivo;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancos
{
    [TestClass]
    public class BancoSantanderTests
    {
        #region Carteira 101

        [TestMethod]
        public void FomatarNumeroDocumentoCarteira101Santander()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("033", "7");
            var contaBancariaCedente = new ContaBancaria("0020", "5", "13003695", "9");
            var cedente = new Cedente("6432794", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

            var sacado = new Sacado("Sacado Fulano de Tal", "999.999.999-99", new Endereco()
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
                NumeroDocumento = "14369",
                ValorBoleto = Convert.ToDecimal(337.00),
                IdentificadorInternoBoleto = "14369",
                DataVencimento = new DateTime(2016, 11, 10),
                CodigoDeTransmissao = "00200643279401300369"
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NumeroDocumento.Length, 10);
            Assert.AreEqual("0000014369", boleto.NumeroDocumento);
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteira101Santander()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("033", "7");
            var contaBancariaCedente = new ContaBancaria("0020", "5", "13003695", "9");
            var cedente = new Cedente("6432794", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

            var sacado = new Sacado("Sacado Fulano de Tal", "999.999.999-99", new Endereco()
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
                NumeroDocumento = "14369",
                ValorBoleto = Convert.ToDecimal(337.00),
                IdentificadorInternoBoleto = "14369",
                DataVencimento = new DateTime(2016, 11, 10),
                CodigoDeTransmissao = "00200643279401300369"
            };

            banco.FormatarBoleto(boleto);

            const string nossoNumeroFormatado = "000000014369-3";
            Assert.AreEqual(nossoNumeroFormatado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(nossoNumeroFormatado, boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteira101Santander()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("033", "7");
            var contaBancariaCedente = new ContaBancaria("0020", "5", "13003695", "9");
            var cedente = new Cedente("6432794", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

            var sacado = new Sacado("Sacado Fulano de Tal", "999.999.999-99", new Endereco()
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
                NumeroDocumento = "14369",
                ValorBoleto = Convert.ToDecimal(337.00),
                IdentificadorInternoBoleto = "14369",
                DataVencimento = new DateTime(2016, 11, 10),
                CodigoDeTransmissao = "00200643279401300369"
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "03399.64322 79400.000000 14369.301016 1 69740000033700";
            Assert.AreEqual(boleto.LinhaDigitavelBoleto.Length, linhaDigitavel.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteira101Santander()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("033", "7");
            var contaBancariaCedente = new ContaBancaria("0020", "5", "13003695", "9");
            var cedente = new Cedente("6432794", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

            var sacado = new Sacado("Sacado Fulano de Tal", "999.999.999-99", new Endereco()
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
                NumeroDocumento = "14369",
                ValorBoleto = Convert.ToDecimal(337.00),
                IdentificadorInternoBoleto = "14369",
                DataVencimento = new DateTime(2016, 11, 10),
                CodigoDeTransmissao = "00200643279401300369"
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "03391697400000337009643279400000001436930101";
            Assert.AreEqual(boleto.CodigoBarraBoleto.Length, codigoBarras.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void FormatarExibicaoCodigoCedenteCarteira101Santander()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("033", "7");
            var contaBancariaCedente = new ContaBancaria("0020", "5", "13003695", "9");
            var cedente = new Cedente("6432794", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
                null);

            var sacado = new Sacado("Sacado Fulano de Tal", "999.999.999-99", new Endereco()
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

            var carteira = new CarteiraCobranca {Codigo = "101"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "14369",
                ValorBoleto = Convert.ToDecimal(337.00),
                IdentificadorInternoBoleto = "14369",
                DataVencimento = new DateTime(2016, 11, 10),
                CodigoDeTransmissao = "00200643279401300369"
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual("0020/6432794", boleto.CedenteBoleto.CodigoCedenteFormatado);
        }

        #endregion

        #region Carteira 102

        [TestMethod]
        public void FomatarNumeroDocumentoCarteira102Santander()
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

            var carteira = new CarteiraCobranca { Codigo = "102" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "001N002",
                ValorBoleto = Convert.ToDecimal(80.55),
                IdentificadorInternoBoleto = "000000000027",
                DataVencimento = new DateTime(2012, 11, 26)
            };

            banco.FormataNumeroDocumento(boleto);

            Assert.AreEqual(boleto.NumeroDocumento.Length, 10);
            Assert.AreEqual("000001N002", boleto.NumeroDocumento);
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteira102Santander()
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

            var carteira = new CarteiraCobranca { Codigo = "102" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "0000000014070",
                ValorBoleto = Convert.ToDecimal(252.00),
                IdentificadorInternoBoleto = "14070",
                DataVencimento = new DateTime(2014, 07, 07)
            };

            banco.FormataNossoNumero(boleto);

            const string valorEsperado = "0000000140708";
            Assert.AreEqual(valorEsperado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(valorEsperado, boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteira102Santander()
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

            var carteira = new CarteiraCobranca { Codigo = "102" };

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
        public void CalcularLinhaDigitavelCarteira102Santander()
        {
            /* 
             * Teste baseado em um boleto apresentado na documentação oficial do Banco Santander
             */
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("033", "7");
            var contaBancariaCedente = new ContaBancaria("4165", "", "", "");
            var cedente = new Cedente("6545424", "1", 0, "99.999.999/9999-99", "Razao Social X", contaBancariaCedente,
                null);

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

        #endregion
    }
}
