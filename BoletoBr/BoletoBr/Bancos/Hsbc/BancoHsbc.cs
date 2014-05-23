using System;
using System.Collections.Generic;
using BoletoBr.CalculoModulo;

namespace BoletoBr.Bancos.Hsbc
{
    public class BancoHsbc : IBanco
    {
        public BancoHsbc()
        {
            CodigoBanco = "399";
            DigitoBanco = "9";
            NomeBanco = "HSBC";

            /* Adiciona carteiras de cobrança */
            _carteirasCobrancaHsbc.Add(new CarteiraCobrancaHsbcCnr());
            _carteirasCobrancaHsbc.Add(new CarteiraCobrancaHsbcCsb());
        }

        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        public ICalculadoraModulo10 CalculadoraModulo10 { get; set; }
        public ICalculadoraModulo11 CalculadoraModulo11 { get; set; }

        private readonly List<CarteiraCobranca> _carteirasCobrancaHsbc = new List<CarteiraCobranca>();
        public List<CarteiraCobranca> GetCarteirasCobranca()
        {
            return _carteirasCobrancaHsbc;
        }

        public CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira)
        {
            return this._carteirasCobrancaHsbc.Find(fd => fd.Codigo == codigoCarteira);
        }
        /// <summary>
        /// Valida se o boleto está preenchido com os campos mínimos requeridos.
        /// Dispara uma ApplicationException caso esteja faltando alguma informação.
        /// </summary>
        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            boleto.ValidaDadosEssenciaisDoBoleto();

        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            throw new System.NotImplementedException();
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {
            throw new System.NotImplementedException();
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            throw new System.NotImplementedException();
        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            throw new System.NotImplementedException();
        }
    }
}
