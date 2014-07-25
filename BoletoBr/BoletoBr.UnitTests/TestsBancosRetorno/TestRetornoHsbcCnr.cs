using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoletoBr.Bancos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.Tests.BancosRetorno
{
    [TestClass]
    public class TestRetornoHsbcCnr
    {
        [TestMethod]
        public void TestHeaderArquivoRetornoBancoHsbcCarteiraCnr()
        {
            var resourceTxt = Properties.Resources.ModeloHeaderRetornoHsbcCnr;

            var linhas = new List<string>(resourceTxt.Split(new[] { Environment.NewLine }, StringSplitOptions.None));

            LeitorRetornoCnab400Hsbc leitor = new LeitorRetornoCnab400Hsbc(linhas);

            var resultado = leitor.ObterHeader();

            StringBuilder b = new StringBuilder();

            b.Append(resultado.CodigoDoRegistro)
                .Append(resultado.CodigoDeRetorno)
                .Append(resultado.LiteralRetorno)
                .Append(resultado.CodigoDoServico)
                .Append(resultado.LiteralServico)
                .Append(resultado.CodigoAgenciaCedente)
                .Append(resultado.Constante)
                .Append(resultado.ContaCorrente)
                .Append(" ")
                .Append(resultado.TipoRetorno)
                .Append(" ")
                .Append(resultado.NomeDoBeneficiario)
                .Append(resultado.CodigoDoBanco)
                .Append(resultado.NomeDoBanco)
                .Append(resultado.DataGravacao)
                .Append(resultado.Densidade)
                .Append(resultado.LiteralDensidade)
                .Append(resultado.CodigoDoBeneficiario)
                .Append(resultado.NomeAgencia)
                .Append(resultado.CodigoFormulario)
                .Append("                                                                                                                                                                                                                                                      ")
                .Append(resultado.Volser)
                .Append(resultado.NumeroSequencial);     

            var builder = b.ToString();

            var conteudoArquivoResource = resourceTxt;

            Assert.AreEqual(conteudoArquivoResource, builder);
        }

        [TestMethod]
        public void TestTrailerArquivoRetornoBancoHsbcCarteiraCnr()
        {
            var resourceTxt = Properties.Resources.ModeloTrailerRetornoHsbcCnr;

            var linhas = new List<string>(resourceTxt.Split(new[] { Environment.NewLine }, StringSplitOptions.None));

            LeitorRetornoCnab400Hsbc leitor = new LeitorRetornoCnab400Hsbc(linhas);

            var resultado = leitor.ObterTrailer();

            StringBuilder b = new StringBuilder();

            b.Append(resultado.CodigoDoRegistro)
                .Append(resultado.CodigoDeRetorno)
                .Append(resultado.CodigoDoServico)
                .Append(resultado.CodigoDoBanco)
                .Append("                                                                                                                                                                                                                                                                                                                                                                                                        ")
                .Append(resultado.NumeroSequencial);

            var builder = b.ToString();

            // Pega último caractere da cadeia.
            var conteudoArquivoResource = resourceTxt.Split().LastOrDefault();

            Assert.AreEqual(conteudoArquivoResource, builder);
        }
    }
}

