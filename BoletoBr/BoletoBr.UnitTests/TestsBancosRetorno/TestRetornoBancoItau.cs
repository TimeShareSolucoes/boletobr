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

        [TestMethod]
        public void TestArquivoRetornoOcorrencia()
        {
            var banco = Fabricas.BancoFactory.ObterBanco("341", "7");

            List<string> arquivo = new List<string>();
            arquivo.Add("02RETORNO01COBRANCA       434300295509        REZ E ROCHA S EM TI LTDA - ME 341BANCO ITAU S.A.15071501600BPI00138160715                                                                                                                                                                                                                                                                                   000001");
            arquivo.Add("10218183122000126434300295509                                 57417541            157574175411             I06140715          57417541            140715000000017680010436327  000000000000000000000000000000000000000000000000000000000000000000000000000000000000017680000000000000000000000000000   16071500000000000000000000000ETR CONST. E INCORPORA LTDA                                         B3000002");
            arquivo.Add("9201341          000000000000000000000000000000          000000000000000000000000000000                                                  000000000000000000000000000000          0000000500000001487691  16/07S001380000000100000000176800                                                                                                                                                                000003");

            var retorno = banco.LerArquivoRetorno(arquivo);

            Assert.AreNotEqual(retorno.RegistrosDetalhe[0].MensagemOcorrenciaRetornoBancario, "");
        }

        #endregion
    }
}
