using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests
{
    [TestClass]
    public class MetodosExtensaoTests
    {
        [TestMethod]
        public void TestOkMetodoExtensaoToDateTimeFromDdMmAa()
        {
            var valorOrigem = "100614";
            DateTime valorEsperado = new DateTime(2014, 6, 10);
            var valorObtido = valorOrigem.ToDateTimeFromDdMmAa();
            Assert.AreEqual(valorObtido.GetValueOrDefault(), valorEsperado);

            var valorOrigem2 = "100699";
            DateTime valorEsperado2 = new DateTime(1999, 6, 10);
            var valorObtido2 = valorOrigem2.ToDateTimeFromDdMmAa();
            Assert.AreEqual(valorObtido2.GetValueOrDefault(), valorEsperado2);
        }

        [TestMethod]
        public void TestFailMetodoExtensaoToDateTimeFromDdMmAa()
        {
            var valorOrigem = "10062014";
            DateTime valorEsperado = new DateTime(2014, 6, 10);
            var valorObtido = valorOrigem.ToDateTimeFromDdMmAa();

            Assert.AreNotEqual(valorObtido.GetValueOrDefault(), valorEsperado);
        }

        [TestMethod]
        public void TestOkMetodoExtensaoToDateTimeFromDdMmAaaa()
        {
            var valorOrigem = "10062014";
            DateTime valorEsperado = new DateTime(2014, 6, 10);
            var valorObtido = valorOrigem.ToDateTimeFromDdMmAaaa();
            Assert.AreEqual(valorObtido.GetValueOrDefault(), valorEsperado);

            var valorOrigem2 = "10061999";
            DateTime valorEsperado2 = new DateTime(1999, 6, 10);
            var valorObtido2 = valorOrigem2.ToDateTimeFromDdMmAaaa();
            Assert.AreEqual(valorObtido2.GetValueOrDefault(), valorEsperado2);
        }

        [TestMethod]
        public void TestFailMetodoExtensaoToDateTimeFromDdMmAaaa()
        {
            var valorOrigem = "10062014";
            DateTime valorEsperado = new DateTime(2014, 6, 10);
            var valorObtido = valorOrigem.ToDateTimeFromDdMmAa();

            Assert.AreNotEqual(valorObtido.GetValueOrDefault(), valorEsperado);
        }
    }
}
