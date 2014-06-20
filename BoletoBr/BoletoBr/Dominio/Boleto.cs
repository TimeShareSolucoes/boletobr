using System;
using System.ComponentModel;
using BoletoBr.Bancos;

namespace BoletoBr
{
    public class Boleto
    {
        public IBanco BancoBoleto { get; set; }
        public Cedente CedenteBoleto { get; set; }
        public Sacado SacadoBoleto { get; set; }
        public CarteiraCobranca CarteiraCobranca { get; set; }
        public string SequencialNossoNumero { get; set; }
        public string NossoNumeroFormatado { get; set; }
        public string DigitoNossoNumero { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataDocumento { get; set; }
        public DateTime DataProcessamento { get; set; }
        public int NumeroParcela { get; set; }
        public decimal ValorBoleto { get; set; }
        public decimal ValorCobrado { get; set; }
        public string LocalPagamento { get; set; }
        public int QuantidadeMoeda { get; set; }
        public string ValorMoeda { get; set; }
        public string Aceite { get; set; }
        public string NumeroDocumento { get; set; }
        public string Especie { get; set; }
        public int Moeda { get; set; }
        public string UsoBanco { get; set; }
        public decimal ValorDesconto { get; set; }
        public bool JurosPermanente { get; set; }
        public decimal PercentualJurosMora { get; set; }
        public decimal JurosMora { get; set; }
        public decimal Iof { get; set; }
        public decimal Abatimento { get; set; }
        public decimal PercentualMulta { get; set; }
        public decimal ValorMulta { get; set; }
        public decimal OutrosAcrescimos { get; set; }
        public decimal OutrosDescontos { get; set; }
        public DateTime DataJurosMora { get; set; }
        public DateTime DataMulta { get; set; }
        public DateTime DataDesconto { get; set; }
        public DateTime DataOutrosAcrescimos { get; set; }
        public DateTime DataOutrosDescontos { get; set; }
        public short PercentualIos { get; set; }
        public string TipoModalidade { get; set; }
        public string CodigoBarraBoleto { get; set; }
        public string LinhaDigitavelBoleto { get; set; }

        // Conforme layout HSBC.
        // Data de Vencimento em formato Juliano.
        public string DataFormatJuliano { get; set; }
        public string CodigoDoProduto { get; set; }

        public Boleto()
        {
            Inicializa();
        }

        private void Inicializa()
        {
            this.LocalPagamento = "Até o vencimento, preferencialmente no ";
            this.QuantidadeMoeda = 1;
            this.ValorMoeda = "";
            this.Aceite = "N";
            this.Especie = "R$";
            this.Moeda = 9;
        }

        public Boleto(Cedente cedente, Sacado sacado, CarteiraCobranca carteiraCobranca) : base()
        {
            Inicializa();
            this.CedenteBoleto = cedente;
            this.SacadoBoleto = sacado;
            this.CarteiraCobranca = carteiraCobranca;
        }

        public void ValidaDadosEssenciaisDoBoleto()
        {
            if (this.CarteiraCobranca == null)
                throw new ApplicationException("Informe a carteira de cobrança.");

            if (String.IsNullOrEmpty(this.NossoNumeroFormatado))
                throw new ApplicationException("Nosso número não foi informado.");
        }
    }
}
