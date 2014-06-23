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

        /// <summary>
        /// Deve ser gerado pelo componente
        /// </summary>
        public string NossoNumeroFormatado { get; private set; }
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
        /// <summary>
        /// Data de Vencimento no formato Juliano.
        /// A data de vencimento no formato juliano somente deve ser utilizada quando o cliente optar pelo uso do Tipo Identificador “4” no Código do Documento, com retorno dos três dígitos no arquivo magnético e no demonstrativo de liquidação (condição cadastral).  
        /// As três primeiras posições correspondem à data de vencimento informada pelo mês juliano. Exemplos:
        /// 001 = corresponde a 01 de janeiro.
        /// 042 = corresponde a 11 de fevereiro.   
        /// A última posição representa o ano. Os algarismos de 0 a 9 correspondem ao algarismo final do ano da data de vencimento.
        /// Exemplos:  
        /// 0=2010, 2020;  1=2011, 2021;  2=2012, 2022;  3=2013, 2023;  4=2014, 2024;
        /// 5=2015, 2025;  6=2006, 2016;  7=2007, 2017;  8=2008, 2018;  9=2009, 2019.  
        /// Nota: Se utilizado o Tipo Identificador “5”, a data de vencimento no formato juliano deverá ser preenchida com quatro zeros = 0000. 
        /// </summary>
        public string DataFormatoJuliano { get; set; }
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

        public void SetNossoNumeroFormatado(string valor)
        {
            this.NossoNumeroFormatado = valor;
        }
    }
}
