using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Cef
{
    public class BancoCef : IBanco
    {
        public BancoCef()
        {
            this.CodigoBanco = "104";
            this.DigitoBanco = "0";
            this.NomeBanco = "Caixa Econômica Federal";

            _carteirasCobranca = new List<CarteiraCobranca>();
        }

        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }

        private List<CarteiraCobranca> _carteirasCobranca;
        public List<CarteiraCobranca> GetCarteirasCobranca()
        {
            return _carteirasCobranca;
        }

        public CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira)
        {
            throw new NotImplementedException();
        }

        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            throw new NotImplementedException();
        }

        public void FormatarBoleto(Boleto boleto)
        {
            throw new NotImplementedException();
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            throw new NotImplementedException();
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {
            throw new NotImplementedException();
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            throw new NotImplementedException();
        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException();
        }
    }
}
