using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsCommon
{
    [TestClass]
    public class CommonTests
    {
        //[TestMethod]
        //public void TestPreenchimentoCadeia()
        //{
        //    const string texto = "75690";
        //    var resultado = Common.CompletarCadeia(true, false, texto, '0', 3);

        //    Assert.AreEqual("75690000", resultado);
        //}

        [TestMethod]
        public void TestFormatarCep()
        {
            const string cep = "75690000";
            var cepFormatado = cep.BoletoBrSetMascara("#####-###");

            Assert.AreEqual("75690-000", cepFormatado);
        }
    }
}
