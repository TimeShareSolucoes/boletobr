using System;
using BoletoBr.Arquivo;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsGeracaoRemessa
{
    [TestClass]
    public class TestsGeracaoRemessa
    {
        [TestMethod]
        public void TestGeracaoArquivoRemessa()
        {
            var remessa = new Remessa(Remessa.EnumTipoAmbiemte.Homologacao, EnumCodigoOcorrenciaRemessa.Registro, "2");

            var banco = Fabricas.BancoFactory.ObterBanco("104", "0");

            var contaBancariaCedente = new ContaBancaria("007", "8", "1234", "0");

            var cedente = new Cedente("999999", 0, "99.999.999/9999-99", "Razão Social X", contaBancariaCedente, null);

            var sacado = new Sacado("Sacado Fulano de Tal", "99.999.999/9999-99", new Endereco
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
                NumeroDocumento = "19",
                ValorBoleto = (decimal)1000.51,
                IdentificadorInternoBoleto = "19",
                DataVencimento = new DateTime(2014, 06, 30),
                Especie = banco.ObtemEspecieDocumento(EnumEspecieDocumento.DuplicataMercantil)
            };

            banco.FormatarBoleto(boleto);


        }
    }
}
