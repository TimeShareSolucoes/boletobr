using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BoletoBr.Bancos;
using BoletoBr.Bancos.Cef;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.Tests.BancosRetorno
{
    [TestClass]
    public class TestRetornoCef
    {
        #region CNAB 240

        [TestMethod]
        public void TestHeaderArquivoRetornoCnab240BancoCef()
        {
            LeitorRetornoCnab240Cef leitor = new LeitorRetornoCnab240Cef(null);

            string valorTesteRegistro =
                "10400000         2106230130001700000000000000000000001839220301600000000RMEX CONSTRUTORA E INCORPORADOC ECON FEDERAL                          22507201400562700036204000000                    RETORNO-PRODUCAO                  000         ";
            
            var resultado = leitor.ObterHeader(valorTesteRegistro);

            Assert.AreEqual(resultado.NomeDoBeneficiario, "RMEX CONSTRUTORA E INCORPORADO");
        }

        [TestMethod]
        public void TestHeaderLoteArquivoRetornoCnab240BancoCef()
        {
            LeitorRetornoCnab240Cef leitor = new LeitorRetornoCnab240Cef(null);

            string valorTesteRegistro =
                "10400011T0100030 20106230130001700000000000000000000001839220301600000000RMEX CONSTRUTORA E INCORPORADO                                                                                00000362250720140000000000                          00   ";

            var resultado = leitor.ObterHeaderLote(valorTesteRegistro);

            Assert.AreEqual(resultado.TipoOperacao, "T");
        }

        [TestMethod]
        public void TestDetalheSegmentoTArquivoRetornoCnab240BancoCef()
        {
            LeitorRetornoCnab240Cef leitor = new LeitorRetornoCnab240Cef(null);

            string valorTesteRegistro =
                "1040001300001T 060000002030160000000   110000000588426997100002055010    1007201400000000003818100004222000002055010              090000000000000000NATANNI SANTANA PINHEIRO                          000000000000320030100                     ";

            var resultado = leitor.ObterRegistrosDetalheT(valorTesteRegistro);

            Assert.AreEqual(resultado.CodigoSegmento, "T");
        }

        [TestMethod]
        public void TestDetalheSegmentoUArquivoRetornoCnab240BancoCef()
        {
            LeitorRetornoCnab240Cef leitor = new LeitorRetornoCnab240Cef(null);

            string valorTesteRegistro =
                "1040001300790U 09000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000250720140000000000000000000000000000000000000000000000000000000000000000000000000000000000000000       ";

            var resultado = leitor.ObterRegistrosDetalheU(valorTesteRegistro);

            Assert.AreEqual(resultado.CodigoSegmento, "U");
        }

        [TestMethod]
        public void TestTrailerLoteArquivoRetornoCnab240BancoCef()
        {
            LeitorRetornoCnab240Cef leitor = new LeitorRetornoCnab240Cef(null);

            string valorTesteRegistro =
                "10400015         00079600000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000                                                                                                                             ";

            var resultado = leitor.ObterTrailerLote(valorTesteRegistro);

            Assert.AreEqual(resultado.QtdRegistrosLote, 796);
        }

        [TestMethod]
        public void TestTrailerArquivoRetornoCnab240BancoCef()
        {
            LeitorRetornoCnab240Cef leitor = new LeitorRetornoCnab240Cef(null);

            string valorTesteRegistro =
                "10499999         000001000798                                                                                                                                                                                                                ";

            var resultado = leitor.ObterTrailer(valorTesteRegistro);

            Assert.AreEqual(resultado.LoteServico, "9999");
        }

        #endregion

        #region CNAB 400

        [TestMethod]
        public void TestHeaderArquivoRetornoCnab400BancoCef()
        {
            LeitorRetornoCnab400Cef leitor = new LeitorRetornoCnab400Cef(null);

            string valorTesteRegistro =
                "10400000         2106230130001700000000000000000000001839220301600000000RMEX CONSTRUTORA E INCORPORADOC ECON FEDERAL                          22507201400562700036204000000                    RETORNO-PRODUCAO                  000         ";

            var resultado = leitor.ObterHeader(valorTesteRegistro);

            Assert.AreEqual(resultado.NomeDoBeneficiario, "RMEX CONSTRUTORA E INCORPORADO");
        }

        [TestMethod]
        public void TestDetalheArquivoRetornoBancoCef()
        {
            LeitorRetornoCnab400Hsbc leitor = new LeitorRetornoCnab400Hsbc(null);


            // Linha detalhe de outro banco.
            string valorTesteRegistro =
                "";
            
            var resultado = leitor.ObterRegistrosDetalhe(valorTesteRegistro);

            Assert.AreEqual(resultado.ValorDoTituloParcela, 10.60);
        }

        [TestMethod]
        public void TestTrailerArquivoRetornoBancoCef()
        {
            LeitorRetornoCnab400Hsbc leitor = new LeitorRetornoCnab400Hsbc(null);

            string dadosTesteTrailer = 
                "9201399                                                                                                                                                                                                                                                                                                                                                                                                        1";

            var resultado = leitor.ObterTrailer(dadosTesteTrailer);

            Assert.AreEqual(resultado.CodigoDeRetorno, 2);
            Assert.AreEqual(resultado.CodigoDoBanco, "399");
            Assert.AreEqual(resultado.CodigoDoRegistro, 9);
            Assert.AreEqual(resultado.CodigoDoServico, "01");
            Assert.AreEqual(resultado.NumeroSequencial, 1);
        }

        #endregion
    }
}

