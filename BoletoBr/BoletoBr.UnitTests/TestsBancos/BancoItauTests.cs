using System;
using BoletoBr.Arquivo;
using BoletoBr.Dominio;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancos
{
    [TestClass]
    public class BancoItauTests
    {
        #region Carteira 112

        [TestMethod]
        public void FomatarNumeroDocumentoCarteira112Itau()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");
            var contaBancariaCedente = new ContaBancaria("0507", "0", "23333", "6");
            var cedente = new Cedente("23333", 6, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "112" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "34598460",
                ValorBoleto = Convert.ToDecimal(501.81),
                IdentificadorInternoBoleto = "34598460",
                DataVencimento = new DateTime(2016, 08, 15)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NumeroDocumento.Length, 10);
            Assert.AreEqual("34598460-3", boleto.NumeroDocumento);
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteira112Itau()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");
            var contaBancariaCedente = new ContaBancaria("0507", "0", "23333", "6");
            var cedente = new Cedente("23333", 6, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "112" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "34598460",
                ValorBoleto = Convert.ToDecimal(501.81),
                IdentificadorInternoBoleto = "34598460",
                DataVencimento = new DateTime(2016, 08, 15)
            };

            banco.FormatarBoleto(boleto);

            const string nossoNumeroFormatado = "112/34598460-6";
            Assert.AreEqual(nossoNumeroFormatado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(nossoNumeroFormatado, boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteira112Itau()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");
            var contaBancariaCedente = new ContaBancaria("0507", "0", "23333", "6");
            var cedente = new Cedente("23333", 6, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "112" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "34598460",
                ValorBoleto = Convert.ToDecimal(501.81),
                IdentificadorInternoBoleto = "34598460",
                DataVencimento = new DateTime(2016, 08, 15)
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "34191.12341 59846.060503 72333.360005 3 68870000050181";
            Assert.AreEqual(boleto.LinhaDigitavelBoleto.Length, linhaDigitavel.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteira112Itau()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");
            var contaBancariaCedente = new ContaBancaria("0507", "0", "23333", "6");
            var cedente = new Cedente("23333", 6, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "112" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "34598460",
                ValorBoleto = Convert.ToDecimal(501.81),
                IdentificadorInternoBoleto = "34598460",
                DataVencimento = new DateTime(2016, 08, 15)
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "34193688700000501811123459846060507233336000";
            Assert.AreEqual(boleto.CodigoBarraBoleto.Length, codigoBarras.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void FormatarExibicaoCodigoCedenteCarteira112Itau()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");
            var contaBancariaCedente = new ContaBancaria("0507", "0", "23333", "6");
            var cedente = new Cedente("23333", 6, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "112"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "34598460",
                ValorBoleto = Convert.ToDecimal(501.81),
                IdentificadorInternoBoleto = "34598460",
                DataVencimento = new DateTime(2016, 08, 15)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual("0507/23333-6", boleto.CedenteBoleto.CodigoCedenteFormatado);
        }

        #endregion

        #region Carteira 109

        [TestMethod]
        public void FomatarNumeroDocumentoCarteira109Itau()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");
            var contaBancariaCedente = new ContaBancaria("4343", "5", "38383", "4");
            var cedente = new Cedente("38383", 4, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "109" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "90017",
                ValorBoleto = Convert.ToDecimal(268.06),
                IdentificadorInternoBoleto = "90017",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NumeroDocumento.Length, 10);
            Assert.AreEqual("00090017-5", boleto.NumeroDocumento);
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteira109Itau()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");
            var contaBancariaCedente = new ContaBancaria("4343", "5", "38383", "4");
            var cedente = new Cedente("38383", 4, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "109" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "90017",
                ValorBoleto = Convert.ToDecimal(268.06),
                IdentificadorInternoBoleto = "90017",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            const string nossoNumeroFormatado = "109/00090017-1";
            Assert.AreEqual(nossoNumeroFormatado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(nossoNumeroFormatado, boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteira109Itau()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");
            var contaBancariaCedente = new ContaBancaria("4343", "5", "38383", "4");
            var cedente = new Cedente("38383", 4, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "109" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "90017",
                ValorBoleto = Convert.ToDecimal(268.06),
                IdentificadorInternoBoleto = "90017",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "34191.09008 09001.714345 33838.340009 9 69690000026806";
            Assert.AreEqual(boleto.LinhaDigitavelBoleto.Length, linhaDigitavel.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteira109Itau()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");
            var contaBancariaCedente = new ContaBancaria("4343", "5", "38383", "4");
            var cedente = new Cedente("38383", 4, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "109" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "90017",
                ValorBoleto = Convert.ToDecimal(268.06),
                IdentificadorInternoBoleto = "90017",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "34199696900000268061090009001714343383834000";
            Assert.AreEqual(boleto.CodigoBarraBoleto.Length, codigoBarras.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void FormatarExibicaoCodigoCedenteCarteira109Itau()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");
            var contaBancariaCedente = new ContaBancaria("4343", "5", "38383", "4");
            var cedente = new Cedente("38383", 4, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "109" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "90017",
                ValorBoleto = Convert.ToDecimal(268.06),
                IdentificadorInternoBoleto = "90017",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual("4343/38383-4", boleto.CedenteBoleto.CodigoCedenteFormatado);
        }

        #endregion

        #region Carteira 103



        #endregion

        #region Carteira 173



        #endregion
    }
}
