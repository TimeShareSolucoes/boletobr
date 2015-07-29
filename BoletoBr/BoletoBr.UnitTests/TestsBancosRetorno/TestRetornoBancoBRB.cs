using System;
using BoletoBr.Bancos.BRB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancosRetorno
{
    [TestClass]
    public class TestRetornoBancoBRB
    {
        #region CNAB 400

        [TestMethod]
        public void TestHeaderArquivoRetornoCnab400BancoBRB()
        {
            LeitorRetornoCnab400BRB leitor = new LeitorRetornoCnab400BRB(null);
        }

        [TestMethod]
        public void TestRegistroArquivoRetornoCnab400BancoBRB()
        {
            LeitorRetornoCnab400BRB leitor = new LeitorRetornoCnab400BRB(null);
        }

        [TestMethod]
        public void TestTrailerArquivoRetornoCnab400BancoBRB()
        {
            LeitorRetornoCnab400BRB leitor = new LeitorRetornoCnab400BRB(null);
        }

        #endregion
    }
}
