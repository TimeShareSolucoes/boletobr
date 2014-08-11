using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests
{
    [TestClass]
    public class CommonTests
    {
        [TestMethod]
        public void TestPreenchimentoCadeia()
        {
            string texto = "TEST";

            string resultado = Common.CompletarCadeia(texto, '-', 7);

            Assert.AreEqual("TEST---", resultado);
        }


        [TestMethod]
        public void TestFormatarCep()
        {
            var cep = "75690000";
            var cepFormatado = cep.SetMascara("#####-###");

            Assert.AreEqual("75690-000", cepFormatado);
        }

        [TestMethod]
        public void TestCalculoDigitoAutoConferenciaBoletoCarteiraCnrHsbcMod11Peso2a9()
        {
            var banco = new Bancos.Hsbc.BancoHsbc();

            /*
             * Valor Base Cálculo obtido na documentação Oficial do HSBC.
             * Manual de Emissão Empresa
             * Cobrança Não Registrada
             */
            const string valorBaseCalculo = "3999392300001200008351202000023910476118682";

            var dacCalculado =
                Common.Mod11Peso2a9(valorBaseCalculo);

            Assert.AreEqual(4, dacCalculado);
        }

        [TestMethod]
        public void TestCalculoDigitoAutoConferenciaBoletoCarteiraCnrHsbcMod11Peso2a9BaseadoEmBoletoReal()
        {
            var banco = new Bancos.Hsbc.BancoHsbc();

            /*
             * Baseado na linha digitável de um boleto real
             */
            const string codBanco = "3999";
            const string fatorVencimento = "5518";
            const string valorDocumento10Dig = "0000020000";
            const string codigoBeneficiario7Dig = "4295579";
            const string codDocumento13Dig = "0000000040156";
            const string dataVencFormatoJuliano = "3202";
            const string codProdutoCnr = "2";
            const string valorBaseCalculo = codBanco + fatorVencimento + valorDocumento10Dig + codigoBeneficiario7Dig + codDocumento13Dig + dataVencFormatoJuliano + codProdutoCnr;

            Assert.AreEqual(43, valorBaseCalculo.Length);

            var dacCalculado =
                Common.Mod11Peso2a9(valorBaseCalculo);

            Assert.AreEqual(1, dacCalculado);
        }
    }
}
