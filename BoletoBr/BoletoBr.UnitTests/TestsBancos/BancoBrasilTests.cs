using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo;
using BoletoBr.Dominio;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancos
{
    [TestClass]
    public class BancoBrasilTests
    {
        #region Carteira 11-019

        #region Convenio 4 dígitos



        #endregion

        #region Convenio 6 dígitos



        #endregion

        #region Convenio 7 dígitos

        [TestMethod]
        public void FomatarNumeroDocumentoCarteira11019BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("165", "1", "32473", "6");
            var cedente = new Cedente("", "2749885", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "11", Variacao = "019"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "16282",
                ValorBoleto = Convert.ToDecimal(792.44),
                IdentificadorInternoBoleto = "16282",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NumeroDocumento.Length, 10);
            Assert.AreEqual("0000016282", boleto.NumeroDocumento);
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteira11019BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("165", "1", "32473", "6");
            var cedente = new Cedente("", "2749885", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "11", Variacao = "019"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "16282",
                ValorBoleto = Convert.ToDecimal(792.44),
                IdentificadorInternoBoleto = "16282",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NossoNumeroFormatado.Length, 17);
            Assert.AreEqual("27498850000016282", boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteira11019BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("165", "1", "32473", "6");
            var cedente = new Cedente("", "2749885", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "11", Variacao = "019"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "16282",
                ValorBoleto = Convert.ToDecimal(792.44),
                IdentificadorInternoBoleto = "16282",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "00190.00009 02749.885006 00016.282113 9 69790000079244";
            Assert.AreEqual(boleto.LinhaDigitavelBoleto.Length, linhaDigitavel.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteira11019BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("165", "1", "32473", "6");
            var cedente = new Cedente("", "2749885", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "11", Variacao = "019"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "16282",
                ValorBoleto = Convert.ToDecimal(792.44),
                IdentificadorInternoBoleto = "16282",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "00199697900000792440000002749885000001628211";
            Assert.AreEqual(boleto.CodigoBarraBoleto.Length, codigoBarras.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void FormatarExibicaoCodigoCedenteCarteira11019BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("165", "1", "32473", "6");
            var cedente = new Cedente("", "2749885", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "11", Variacao = "019" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "16282",
                ValorBoleto = Convert.ToDecimal(792.44),
                IdentificadorInternoBoleto = "16282",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);
            
            Assert.AreEqual("0165-1/00032473-6", boleto.CedenteBoleto.CodigoCedenteFormatado);
        }

        #endregion

        #endregion

        #region Carteira 17-019

        #region Convenio 4 dígitos


        #endregion

        #region Convenio 6 dígitos


        #endregion

        #region Convenio 7 dígitos

        [TestMethod]
        public void FomatarNumeroDocumentoCarteira17019BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("3391", "X", "16911", "0");
            var cedente = new Cedente("", "2714999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "17", Variacao = "019"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "165445",
                ValorBoleto = Convert.ToDecimal(1041.50),
                IdentificadorInternoBoleto = "165445",
                DataVencimento = new DateTime(2016, 11, 02)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NumeroDocumento.Length, 10);
            Assert.AreEqual("0000165445", boleto.NumeroDocumento);
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteira17019BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("3391", "X", "16911", "0");
            var cedente = new Cedente("", "2714999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "17", Variacao = "019"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "165445",
                ValorBoleto = Convert.ToDecimal(1041.50),
                IdentificadorInternoBoleto = "165445",
                DataVencimento = new DateTime(2016, 11, 02)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NossoNumeroFormatado.Length, 17);
            Assert.AreEqual("27149990000165445", boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteira17019BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("3391", "X", "16911", "0");
            var cedente = new Cedente("", "2714999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "17", Variacao = "019" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "165445",
                ValorBoleto = Convert.ToDecimal(1041.50),
                IdentificadorInternoBoleto = "165445",
                DataVencimento = new DateTime(2016, 11, 02)
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "00190.00009 02714.999006 00165.445172 2 69660000104150";
            Assert.AreEqual(boleto.LinhaDigitavelBoleto.Length, linhaDigitavel.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteira17019BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("3391", "X", "16911", "0");
            var cedente = new Cedente("", "2714999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "17", Variacao = "019" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "165445",
                ValorBoleto = Convert.ToDecimal(1041.50),
                IdentificadorInternoBoleto = "165445",
                DataVencimento = new DateTime(2016, 11, 02)
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "00192696600001041500000002714999000016544517";
            Assert.AreEqual(boleto.CodigoBarraBoleto.Length, codigoBarras.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void FormatarExibicaoCodigoCedenteCarteira17019BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("3391", "X", "16911", "0");
            var cedente = new Cedente("", "2714999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "17", Variacao = "019"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "165445",
                ValorBoleto = Convert.ToDecimal(1041.50),
                IdentificadorInternoBoleto = "165445",
                DataVencimento = new DateTime(2016, 11, 02)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual("3391-X/00016911-0", boleto.CedenteBoleto.CodigoCedenteFormatado);
        }

        #endregion

        #endregion

        #region Carteira 17-035

        #region Convenio 4 dígitos


        #endregion

        #region Convenio 6 dígitos


        #endregion

        #region Convenio 7 dígitos

        [TestMethod]
        public void FomatarNumeroDocumentoCarteira17035BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("3391", "X", "16954", "4");
            var cedente = new Cedente("", "2791531", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "17", Variacao = "035" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "11257",
                ValorBoleto = Convert.ToDecimal(1041.50),
                IdentificadorInternoBoleto = "11257",
                DataVencimento = new DateTime(2016, 11, 01)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NumeroDocumento.Length, 10);
            Assert.AreEqual("0000011257", boleto.NumeroDocumento);
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteira17035BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("3391", "X", "16954", "4");
            var cedente = new Cedente("", "2791531", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "17", Variacao = "035" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "11257",
                ValorBoleto = Convert.ToDecimal(1041.50),
                IdentificadorInternoBoleto = "11257",
                DataVencimento = new DateTime(2016, 11, 01)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NossoNumeroFormatado.Length, 17);
            Assert.AreEqual("27915310000011257", boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteira17035BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("3391", "X", "16954", "4");
            var cedente = new Cedente("", "2791531", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "17", Variacao = "035" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "11257",
                ValorBoleto = Convert.ToDecimal(1041.50),
                IdentificadorInternoBoleto = "11257",
                DataVencimento = new DateTime(2016, 11, 01)
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "00190.00009 02791.531003 00011.257177 7 69650000104150";
            Assert.AreEqual(boleto.LinhaDigitavelBoleto.Length, linhaDigitavel.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteira17035BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("3391", "X", "16954", "4");
            var cedente = new Cedente("", "2791531", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "17", Variacao = "035" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "11257",
                ValorBoleto = Convert.ToDecimal(1041.50),
                IdentificadorInternoBoleto = "11257",
                DataVencimento = new DateTime(2016, 11, 01)
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "00197696500001041500000002791531000001125717";
            Assert.AreEqual(boleto.CodigoBarraBoleto.Length, codigoBarras.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void FormatarExibicaoCodigoCedenteCarteira17035BbConvenio7Digitos()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("001", "9");
            var contaBancariaCedente = new ContaBancaria("3391", "X", "16954", "4");
            var cedente = new Cedente("", "2791531", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "17", Variacao = "035" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "11257",
                ValorBoleto = Convert.ToDecimal(1041.50),
                IdentificadorInternoBoleto = "11257",
                DataVencimento = new DateTime(2016, 11, 01)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual("3391-X/00016954-4", boleto.CedenteBoleto.CodigoCedenteFormatado);
        }

        #endregion

        #endregion
    }
}
