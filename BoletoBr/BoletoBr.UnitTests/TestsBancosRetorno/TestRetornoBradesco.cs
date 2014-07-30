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
        public void TestHeaderArquivoRetornoCnab240BancoBradesco()
        {
            LeitorRetornoCnab240Bradesco leitor = new LeitorRetornoCnab240Bradesco(null);
                
            string valorTesteRegistro =
                "10400000         2106230130001700000000000000000000001839220301600000000RMEX CONSTRUTORA E INCORPORADOC ECON FEDERAL                          22507201400562700036204000000                    RETORNO-PRODUCAO                  000         ";

            var resultado = leitor.ObterHeader(valorTesteRegistro);

            Assert.AreEqual(resultado.NomeDoBeneficiario, "RMEX CONSTRUTORA E INCORPORADO");
        }

        #endregion
    }
}
