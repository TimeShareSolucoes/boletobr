using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsCommon
{
    [TestClass]
    public class MetodosExtensaoTests
    {
        [TestMethod]
        public void TestOkMetodoExtensaoToDateTimeFromDdMmAa()
        {
            const string valorOrigem = "100614";
            var valorEsperado = new DateTime(2014, 6, 10);
            var valorObtido = valorOrigem.ToDateTimeFromDdMmAa();
            Assert.AreEqual(valorObtido.GetValueOrDefault(), valorEsperado);

            const string valorOrigem2 = "100699";
            var valorEsperado2 = new DateTime(1999, 6, 10);
            var valorObtido2 = valorOrigem2.ToDateTimeFromDdMmAa();
            Assert.AreEqual(valorObtido2.GetValueOrDefault(), valorEsperado2);
        }

        [TestMethod]
        public void TestFailMetodoExtensaoToDateTimeFromDdMmAa()
        {
            const string valorOrigem = "10062014";
            var valorEsperado = new DateTime(2014, 6, 10);
            var valorObtido = valorOrigem.ToDateTimeFromDdMmAa();

            Assert.AreNotEqual(valorObtido.GetValueOrDefault(), valorEsperado);
        }

        [TestMethod]
        public void TestOkMetodoExtensaoToDateTimeFromDdMmAaaa()
        {
            const string valorOrigem = "10062014";
            var valorEsperado = new DateTime(2014, 6, 10);
            var valorObtido = valorOrigem.ToDateTimeFromDdMmAaaa();
            Assert.AreEqual(valorObtido.GetValueOrDefault(), valorEsperado);

            const string valorOrigem2 = "10061999";
            var valorEsperado2 = new DateTime(1999, 6, 10);
            var valorObtido2 = valorOrigem2.ToDateTimeFromDdMmAaaa();
            Assert.AreEqual(valorObtido2.GetValueOrDefault(), valorEsperado2);
        }

        [TestMethod]
        public void TestFailMetodoExtensaoToDateTimeFromDdMmAaaa()
        {
            const string valorOrigem = "10062014";
            var valorEsperado = new DateTime(2014, 6, 10);
            var valorObtido = valorOrigem.ToDateTimeFromDdMmAa();

            Assert.AreNotEqual(valorObtido.GetValueOrDefault(), valorEsperado);
        }

        [TestMethod]
        public void TestPreencherValorLinha()
        {
            var linhaCnab400 = new string(' ', 400);

            linhaCnab400 = linhaCnab400.PreencherValorNaLinha(1, 1, "0");
            linhaCnab400 = linhaCnab400.PreencherValorNaLinha(2, 2, "1");
            linhaCnab400 = linhaCnab400.PreencherValorNaLinha(3, 9, "REMESSA");
            linhaCnab400 = linhaCnab400.PreencherValorNaLinha(10, 11, "01");

            Assert.IsNotNull(linhaCnab400);
        }
    }
}
