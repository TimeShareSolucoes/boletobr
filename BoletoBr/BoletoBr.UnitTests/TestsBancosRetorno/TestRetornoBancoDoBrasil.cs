using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos.Brasil;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancosRetorno
{
    [TestClass]
    public class TestRetornoBancoDoBrasil
    {
        #region CNAB240

        [TestMethod]
        public void TestHeaderArquivoRetornoCnab240BancoDoBrasil()
        {

        }

        #endregion

        #region CNAB400

        [TestMethod]
        public void TestHeaderArquivoRetornoCnab400BancoDoBrasil()
        {
            LeitorRetornoCnab400BancoDoBrasil leitor = new LeitorRetornoCnab400BancoDoBrasil(null);

            string valorTesteRegistro =
                "02RETORNO01COBRANCA       00201300369513003695MARIA AUGUSTA SOARES PASCHOAL 033SANTANDER      13081500000000006432794                                                                                                                                                                                                                                                                                  370000001";

            var resultado = leitor.ObterHeader(valorTesteRegistro);

            Assert.AreEqual(resultado.LiteralRetorno, "RETORNO");
        }

        [TestMethod]
        public void TestArquivoRetornoCnab400BancoDoBrasil()
        {
            var linhas = new List<string>
            {
                "02RETORNO01COBRANCA       01651000324736000000HOT BEACH SUITES OLIMPIA - EMP001BANCO DO BRASIL2808150000059                      000000060447579594  2749885                                                                                                                                                                                                                                              000001",
                "70000000000000000016510003247362749885DOC27498850000005356     2749885465588530110002800AI 01900000000000 11022808150000005356                    250915000000003400000104278010000000000350000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000035010000000000000          0000000000000000000000000000000000000000000000001000000002",
                "9201001          000000020000000006807500000002          000000000000000000000000000000          000000000000000000000000000000          000000000000000000000000000000                                                  000000000000000000000000000000                                                                                                                                                   000003"
            };

            var banco = Fabricas.BancoFactory.ObterBanco("001");
            var objReturn = banco.LerArquivoRetorno(linhas);

            Assert.AreEqual(objReturn.RegistrosDetalhe[0].NumeroDocumento, "0000005356");
        }

        #endregion
    }
}
