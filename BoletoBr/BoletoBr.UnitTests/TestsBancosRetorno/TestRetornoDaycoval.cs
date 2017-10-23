using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos.Daycoval;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancosRetorno
{
    [TestClass]
    public class TestRetornoDaycoval
    {
        #region CNAB 400

        [TestMethod]
        public void TestHeaderArquivoRetornoCnab400BancoDaycoval()
        {
            LeitorRetornoCnab400Daycoval leitor = new LeitorRetornoCnab400Daycoval(null);

            string valorTesteRegistro =
                "02RETORNO01COBRANCA       00058906300011365703TESTE                      LTD707BANCO DAYCOVAL 05101701600BPI00003                                                                                                                                                                                                                                                                                         000001";

            var resultado = leitor.ObterHeader(valorTesteRegistro);

            Assert.AreEqual(resultado.CodigoDoBanco, "707");
        }

        [TestMethod]
        public void TestRegistroArquivoRetornoCnab400BancoDaycoval()
        {
            LeitorRetornoCnab400Daycoval leitor = new LeitorRetornoCnab400Daycoval(null);

            string valorTesteRegistro =
                "102218604220001070005890630001136570300684720                 74721953            112747219535565125765    10604101700684720  74721953            021017000000000300003300200010000000000000                          000000000000000000000000000000000000000000000000300000000000000000000000000000                                                                                    100000000051017000000002";

            var resultado = leitor.ObterRegistrosDetalhe(valorTesteRegistro);

            Assert.AreEqual(resultado.NumeroSequencial, 2);
        }

        [TestMethod]
        public void TestTrailerArquivoRetornoCnab400BancoDaycoval()
        {
            LeitorRetornoCnab400Daycoval leitor = new LeitorRetornoCnab400Daycoval(null);

            string valorTesteRegistro =
                "920170700000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000                                                                                                                                                                                                                                                                                                 000003";

            var resultado = leitor.ObterTrailer(valorTesteRegistro);

            Assert.AreEqual(resultado.NumeroSequencial, 3);
        }

        #endregion
    }
}
