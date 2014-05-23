using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests
{
    [TestClass]
    public class BancoHsbcTests
    {
        [TestMethod]
        public void CarteiraNaoRegistradaTeste1()
        {
            var banco = Fabricas.BancoFactory.ObterBanco("399", "9");

            var contaBancariaCedente = new ContaBancaria("2698", "", "00323", "46");

            var cedente = new Cedente("4295579", 0, "67.581.019/0001-40", "Razão Social Teste", contaBancariaCedente, null);

            var sacado = new Sacado("Fulano de Tal", "00000000000", new Endereco()
            {
                TipoLogradouro = "R",
                Logradouro = "1",
                Bairro = "Bairro X",
                Cidade = "Cidade X",
                SiglaUf = "XX",
                Cep = "12345-000",
                Complemento = "Comp X",
                Numero = "9"
            });

            var boleto = new Boleto(cedente, sacado, banco.GetCarteiraCobrancaPorCodigo("CNR"));

            
        }
    }
}
