using System;
using BoletoBr.Arquivo;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancos
{
    [TestClass]
    public class BancoBRBTests
    {
        #region Carteira 1

        [TestMethod]
        public void TesteCalculoNossoNumeroBoleto()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "1");

            var banco = Fabricas.BancoFactory.ObterBanco("070", "");

            var contaBancariaCedente = new ContaBancaria("201", "", "19316", "9");
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

            var carteira = new CarteiraCobranca
            {
                Codigo = "1",
                Tipo = "1"
            };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "37018",
                ValorBoleto = Convert.ToDecimal(1.00),
                IdentificadorInternoBoleto = "37018",
                DataVencimento = new DateTime(2008, 06, 27)
            };

            banco.FormataNossoNumero(boleto);

            Assert.AreEqual("103701807010", boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void TesteGerarNumeroBoletoSemFatorVencimento()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "1");

            var banco = Fabricas.BancoFactory.ObterBanco("070");

            var contaBancariaCedente = new ContaBancaria("058", "", "600200", "6");
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

            var carteira = new CarteiraCobranca
            {
                Codigo = "1",
                Tipo = "1"
            };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "1",
                ValorBoleto = Convert.ToDecimal(1.00),
                IdentificadorInternoBoleto = "1"
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);

            Assert.AreEqual("07096000000000001000000586002006100000107045", boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void TesteGerarNumeroBoletoComFatorVencimento()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "1");

            var banco = Fabricas.BancoFactory.ObterBanco("070");

            var contaBancariaCedente = new ContaBancaria("201", "", "19316", "9");
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

            var carteira = new CarteiraCobranca
            {
                Codigo = "1",
                Tipo = "1"
            };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "37018",
                ValorBoleto = Convert.ToDecimal(510.35),
                IdentificadorInternoBoleto = "37018",
                DataVencimento = new DateTime(2015, 07, 08)
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);

            Assert.AreEqual("07096648300000510350002010193169103701807010", boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void TesteGerarLinhaDigitavelBoletoSemFatorVencimento()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "1");

            var banco = Fabricas.BancoFactory.ObterBanco("070");

            var contaBancariaCedente = new ContaBancaria("058", "", "600200", "6");
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

            var carteira = new CarteiraCobranca
            {
                Codigo = "1",
                Tipo = "1"
            };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "1",
                ValorBoleto = Convert.ToDecimal(1.00),
                IdentificadorInternoBoleto = "1"
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            Assert.AreEqual("07090.00053 86002.006103 00001.070457 0 00000000000100", boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void TesteGerarLinhaDigitavelBoletoComFatorVencimento()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "1");

            var banco = Fabricas.BancoFactory.ObterBanco("070");

            var contaBancariaCedente = new ContaBancaria("201", "", "19316", "9");
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

            var carteira = new CarteiraCobranca
            {
                Codigo = "1",
                Tipo = "1"
            };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "37018",
                ValorBoleto = Convert.ToDecimal(510.35),
                IdentificadorInternoBoleto = "37018",
                DataVencimento = new DateTime(2015, 07, 08)
            };

            banco.FormataNossoNumero(boleto);
            banco.FormataCodigoBarra(boleto);
            banco.FormataLinhaDigitavel(boleto);

            Assert.AreEqual("07090.00202 10193.169108 37018.070104 6 64830000051035", boleto.LinhaDigitavelBoleto);
        }

        #endregion
    }
}
