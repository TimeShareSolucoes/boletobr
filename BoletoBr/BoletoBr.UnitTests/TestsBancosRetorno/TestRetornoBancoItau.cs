using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos.Itau;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancosRetorno
{
    [TestClass]
    public class TestRetornoBancoItau
    {
        #region CNAB400

        [TestMethod]
        public void TestHeaderArquivoRetornoCnab400Itau()
        {
            LeitorRetornoCnab400Itau leitor = new LeitorRetornoCnab400Itau(null);

            string valorTesteRegistro =
                "02RETORNO01COBRANCA";

            var resultado = leitor.ObterHeader(valorTesteRegistro);

            Assert.AreEqual(resultado.LiteralRetorno, "RETORNO");
        }

        public void TestDetalheArquivoRetornoCnab400Itau()
        {
            LeitorRetornoCnab400Itau leitor = new LeitorRetornoCnab400Itau(null);

            string valorTesteRegistro =
                "10210623013000170000000901923000105100001217010               000000002300000684440000000000000000000000000906090714000121701000000000230000068444100714000000002902410400162  000000000000000000000000000000000000000000000000000000000000000000000000000000000000002902400000000000000000000000000   100714             00000000000000                                                                  000002";

            var resultado = leitor.ObterRegistrosDetalhe(valorTesteRegistro);

            Assert.AreEqual(resultado.DataDeVencimento, "090714");
        }

        public void TestTrailerArquivoRetornoCnab400Itau()
        {
            LeitorRetornoCnab400Itau leitor = new LeitorRetornoCnab400Itau(null);

            string valorTesteRegistro =
                "10210623013000170000000901923000105100001217010               000000002300000684440000000000000000000000000906090714000121701000000000230000068444100714000000002902410400162  000000000000000000000000000000000000000000000000000000000000000000000000000000000000002902400000000000000000000000000   100714             00000000000000                                                                  000002";

            var resultado = leitor.ObterRegistrosDetalhe(valorTesteRegistro);

            Assert.AreEqual(resultado.DataDeVencimento, "090714");
        }

        #endregion
    }
}
