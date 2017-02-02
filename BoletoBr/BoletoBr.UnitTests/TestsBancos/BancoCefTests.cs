using System;
using System.Runtime.InteropServices;
using BoletoBr.Arquivo;
using BoletoBr.Dominio;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancos
{
    [TestClass]
    public class BancoCefTests
    {
        #region Carteira RG - REGISTRADA

        [TestMethod]
        public void FomatarNumeroDocumentoCarteiraRGCaixa()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");
            var contaBancariaCedente = new ContaBancaria("0799", "4", "1679", "9");
            var cedente = new Cedente("657224", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "RG"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "528",
                ValorBoleto = Convert.ToDecimal(491.65),
                IdentificadorInternoBoleto = "528",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NumeroDocumento.Length, 10);
            Assert.AreEqual("0000000528", boleto.NumeroDocumento);
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteiraRGCaixa()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");
            var contaBancariaCedente = new ContaBancaria("0799", "4", "1679", "9");
            var cedente = new Cedente("657224", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "RG"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "528",
                ValorBoleto = Convert.ToDecimal(491.65),
                IdentificadorInternoBoleto = "528",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            const string nossoNumeroFormatado = "14000000000000528-3";
            Assert.AreEqual(nossoNumeroFormatado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(nossoNumeroFormatado, boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteiraRGCaixa()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");
            var contaBancariaCedente = new ContaBancaria("0799", "4", "1679", "9");
            var cedente = new Cedente("657224", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "RG" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "528",
                ValorBoleto = Convert.ToDecimal(491.65),
                IdentificadorInternoBoleto = "528",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "10496.57222 43000.100040 00000.052829  4 69690000049165";
            Assert.AreEqual(boleto.LinhaDigitavelBoleto.Length, linhaDigitavel.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteiraRGCaixa()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");
            var contaBancariaCedente = new ContaBancaria("0799", "4", "1679", "9");
            var cedente = new Cedente("657224", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "RG" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "528",
                ValorBoleto = Convert.ToDecimal(491.65),
                IdentificadorInternoBoleto = "528",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "10494696900000491656572243000100040000005282";
            Assert.AreEqual(boleto.CodigoBarraBoleto.Length, codigoBarras.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void FormatarExibicaoCodigoCedenteCarteiraRGCaixa()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");
            var contaBancariaCedente = new ContaBancaria("0799", "4", "1679", "9");
            var cedente = new Cedente("657224", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "RG"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "528",
                ValorBoleto = Convert.ToDecimal(491.65),
                IdentificadorInternoBoleto = "528",
                DataVencimento = new DateTime(2016, 11, 05)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual("0799/657224-3", boleto.CedenteBoleto.CodigoCedenteFormatado);
        }

        #endregion Carteira RG - REGISTRADA

        #region Carteira SR - SEM REGISTRO

        [TestMethod]
        public void FomatarNumeroDocumentoCarteiraSRCaixa()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");
            var contaBancariaCedente = new ContaBancaria("0799", "4", "001024", "3");
            var cedente = new Cedente("245", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "SR" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "83",
                ValorBoleto = Convert.ToDecimal(478.01),
                IdentificadorInternoBoleto = "83",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual(boleto.NumeroDocumento.Length, 10);
            Assert.AreEqual("0000000083", boleto.NumeroDocumento);
        }

        [TestMethod]
        public void FormatarNossoNumeroCarteiraSRCaixa()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");
            var contaBancariaCedente = new ContaBancaria("0799", "4", "001024", "3");
            var cedente = new Cedente("245", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "SR" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "83",
                ValorBoleto = Convert.ToDecimal(478.01),
                IdentificadorInternoBoleto = "83",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);

            const string nossoNumeroFormatado = "24000000000000083-8";
            Assert.AreEqual(nossoNumeroFormatado.Length, boleto.NossoNumeroFormatado.Length);
            Assert.AreEqual(nossoNumeroFormatado, boleto.NossoNumeroFormatado);
        }

        [TestMethod]
        public void CalcularLinhaDigitavelCarteiraSRCaixa()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");
            var contaBancariaCedente = new ContaBancaria("0799", "4", "001024", "3");
            var cedente = new Cedente("245", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "SR" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "83",
                ValorBoleto = Convert.ToDecimal(478.01),
                IdentificadorInternoBoleto = "83",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);

            const string linhaDigitavel = "10490.00241 53000.200047 00000.008342  1 69790000047801";
            Assert.AreEqual(boleto.LinhaDigitavelBoleto.Length, linhaDigitavel.Length);
            Assert.AreEqual(linhaDigitavel, boleto.LinhaDigitavelBoleto);
        }

        [TestMethod]
        public void CalcularCodigoBarrasCarteiraSRCaixa()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");
            var contaBancariaCedente = new ContaBancaria("0799", "4", "001024", "3");
            var cedente = new Cedente("245", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca { Codigo = "SR" };

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "83",
                ValorBoleto = Convert.ToDecimal(478.01),
                IdentificadorInternoBoleto = "83",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);

            const string codigoBarras = "10491697900000478010002453000200040000000834";
            Assert.AreEqual(boleto.CodigoBarraBoleto.Length, codigoBarras.Length);
            Assert.AreEqual(codigoBarras, boleto.CodigoBarraBoleto);
        }

        [TestMethod]
        public void FormatarExibicaoCodigoCedenteCarteiraSRCaixa()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");
            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");
            var contaBancariaCedente = new ContaBancaria("0799", "4", "001024", "3");
            var cedente = new Cedente("245", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente,
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

            var carteira = new CarteiraCobranca {Codigo = "SR"};

            var boleto = new Boleto(carteira, cedente, sacado, remessa)
            {
                NumeroDocumento = "83",
                ValorBoleto = Convert.ToDecimal(478.01),
                IdentificadorInternoBoleto = "83",
                DataVencimento = new DateTime(2016, 11, 15)
            };

            banco.FormatarBoleto(boleto);

            Assert.AreEqual("0799/000245-3", boleto.CedenteBoleto.CodigoCedenteFormatado);
        }

        #endregion Carteira SR - SEM REGISTRO
    }
}
