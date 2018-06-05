using System;
using BoletoBr.Arquivo;
using BoletoBr.Bancos.Banrisul;
using BoletoBr.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.TestsBancos
{
    [TestClass]
    public class BancoBanrisulTests
    {
        [TestMethod]
        public void FomatarNumeroNossoNumeroBanrisul()
        {
            var NossoNumeroTeste = "00009274";
            var dv = Common.DigitoVerificadorBanrisulNC(NossoNumeroTeste);
            Assert.IsTrue(dv == 22);
        }
        [TestMethod]
        public void FomatarNumeroCodigoBarrasBanrisul()
        {
            var NossoNumeroTeste = "21110290001502283256340";
            var dv = Common.DigitoVerificadorBanrisulNC(NossoNumeroTeste);
            Assert.IsTrue(dv == 59);
        }
        [TestMethod]
        public void FomatarNumeroComAdicaoMod10Banrisul()
        {
            var NossoNumeroTeste = "00009194";
            var dv = Common.DigitoVerificadorBanrisulNC(NossoNumeroTeste);
            Assert.IsTrue(dv == 38);
        }
        [TestMethod]
        public void TesteCodigoBarras()
        {
            var dataAtual = DateTime.Now;
            var boleto = new Boleto()
            {
                CarteiraCobranca = new CarteiraCobranca() { BancoEmiteBoleto = false},
                CedenteBoleto = new Cedente("9000150",1,"70070070077","teste de codigo de barras", new ContaBancaria("1102","12345"),new Endereco() ),
                IdentificadorInternoBoleto = "22832563",
                ValorBoleto = 550,
                DataVencimento = dataAtual
            };
            var bancoBanrisul = new BancoBanrisul();

            bancoBanrisul.FormataCodigoBarra(boleto);
            bancoBanrisul.FormataLinhaDigitavel(boleto);
            Assert.IsTrue(boleto.CodigoBarraBoleto == $@"04198{Common.FatorVencimento(boleto.DataVencimento).ToString().PadLeft(4, '0')}00000550002111029000150228325634059");
            Assert.IsTrue(boleto.LinhaDigitavelBoleto == $@"04192.11107 29000.150226 83256.340593 8 {Common.FatorVencimento(boleto.DataVencimento).ToString().PadLeft(4, '0')}0000055000");
        }

    }
}
