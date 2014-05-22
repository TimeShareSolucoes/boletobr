using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests
{
    [TestClass]
    public class CommonTests
    {
        [TestMethod]
        public void TestFormatarCep()
        {
            var cep = "75690000";
            var cepFormatado = cep.SetMascara("#####-###");

            Assert.AreEqual("75690-000", cepFormatado);
        }
    }
}
