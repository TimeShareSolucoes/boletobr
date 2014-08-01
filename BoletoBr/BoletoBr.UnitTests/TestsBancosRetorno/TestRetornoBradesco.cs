using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos.Bradesco;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancosRetorno
{
    [TestClass]
    public class TestRetornoBradesco
    {
        #region CNAB 240

        [TestMethod]
        public void TestHeaderArquivoRetornoCnab240BancoBradesco() { }

        [TestMethod]
        public void TestRegistroArquivoRetornoCnab240BancoBradesco() { }

        [TestMethod]
        public void TestTrailerArquivoRetornoCnab240BancoBradesco() { }

        #endregion

        #region CNAB 400

        [TestMethod]
        public void TestHeaderArquivoRetornoCnab400BancoBradesco()
        {
            LeitorRetornoCnab400Bradesco leitor = new LeitorRetornoCnab400Bradesco(null);
                
            string valorTesteRegistro =
                "02RETORNO01COBRANCA       00000000000004603020RMEX CONSTRUTORA E INCORPORADO237BRADESCO       0907140160000000131                                                                                                                                                                                                                                                                          100714         000001";

            var resultado = leitor.ObterHeader(valorTesteRegistro);

            Assert.AreEqual(resultado.CodigoDoBanco, "237");
        }

        [TestMethod]
        public void TestRegistroArquivoRetornoCnab400BancoBradesco()
        {
            LeitorRetornoCnab400Bradesco leitor = new LeitorRetornoCnab400Bradesco(null);

            string valorTesteRegistro =
                "10210623013000170000000901923000105100001217010               000000002300000684440000000000000000000000000906090714000121701000000000230000068444100714000000002902410400162  000000000000000000000000000000000000000000000000000000000000000000000000000000000000002902400000000000000000000000000   100714             00000000000000                                                                  000002";

            var resultado = leitor.ObterRegistrosDetalhe(valorTesteRegistro);

            Assert.AreEqual(resultado.NumeroSequencial, 2);
        }

        [TestMethod]
        public void TestTrailerArquivoRetornoCnab400BancoBradesco()
        {
            LeitorRetornoCnab400Bradesco leitor = new LeitorRetornoCnab400Bradesco(null);

            string valorTesteRegistro =
                "9201237          000008850000003044113700000131          00000000000000000000000122998000030000001229980000000000000000000000000000000000000000000000000000000000000000000000000000000000000                                                                                                                                                                              00000000000000000000000         000006";

            var resultado = leitor.ObterTrailer(valorTesteRegistro);

            Assert.AreEqual(resultado.NumeroSequencial, 6);
        }

        #endregion
    }
}
