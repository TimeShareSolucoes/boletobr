using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using BoletoBr.Bancos;
using BoletoBr.Dominio;
using BoletoBr.Dominio.EspecieDocumento;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;

namespace BoletoBr
{
    public class Boleto
    {
        #region Propriedades

        public IBanco BancoBoleto { get; set; }

        public string CodigoEDigitoBancoBind
        {
            get
            {
                if (this.BancoBoleto != null)
                    return BancoBoleto.CodigoBanco + "-" + BancoBoleto.DigitoBanco;

                return "";
            }
        }

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
        public DateTime? DataProcessamento { get; set; }
        public int QtdParcelas { get; set; }
        public int NumeroParcela { get; set; }

        public string NumeroParcelaFormatado
        {
            get { return String.Format("{0} / {1}", NumeroParcela, QtdParcelas); }
        }

        public decimal ValorBoleto { get; set; }
        public decimal? ValorCobrado { get; set; }
        public string LocalPagamento { get; set; }
        public decimal? QuantidadeMoeda { get; set; }
        public string ValorMoeda { get; set; }
        public string Aceite { get; set; }
        public string NumeroDocumento { get; set; }
        public IEspecieDocumento Especie { get; set; }
        public string Moeda { get; set; }
        public string UsoBanco { get; set; }
        //public decimal? ValorAbatimentoDesconto { get; set; }
        public decimal? ValorAbatimento { get; set; }
        public decimal? ValorDesconto { get; set; }
        public bool JurosPermanente { get; set; }
        public decimal? PercentualJurosMora { get; set; }
        public decimal? JurosMora { get; set; }
        public decimal? Iof { get; set; }
        public decimal? PercentualMulta { get; set; }
        public decimal? ValorMulta { get; set; }
        public decimal? OutrosAcrescimos { get; set; }
        public decimal? OutrosDescontos { get; set; }
        public DateTime DataJurosMora { get; set; }
        public DateTime DataMulta { get; set; }
        public DateTime DataDesconto { get; set; }
        public DateTime DataOutrosAcrescimos { get; set; }
        public DateTime DataOutrosDescontos { get; set; }
        public short PercentualIos { get; set; }
        public string TipoModalidade { get; set; }
        public string CodigoBarraBoleto { get; set; }
        public string LinhaDigitavelBoleto { get; set; }

        /// <summary> DATA DE VENCIMENTO NO FORMATO JULIANO
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
        /// <summary>
        /// Utilizado no Banco Santander
        /// </summary>
        public int PercentualIOS { get; set; }
        public TipoArquivo TipoArquivo { get; set; }
        public string CodigoDoProduto { get; set; }
        public List<IInstrucao> InstrucoesDoBoleto { get; set; }

        public void AdicionarInstrucao(EnumTipoInstrucao tipoInstrucao, double valor)
        {
            this.InstrucoesDoBoleto.Add(this.BancoBoleto.ObtemInstrucaoPadronizada(tipoInstrucao, valor));
        }

        public string InstrucoesBoletoConcatenadas
        {
            get
            {
                if (this.InstrucoesDoBoleto == null || this.InstrucoesDoBoleto.Count <= 0)
                    return "";

                string textoConcatenado = "";

                foreach (var instrucao in this.InstrucoesDoBoleto)
                {
                    textoConcatenado += instrucao.TextoInstrucao + Environment.NewLine;
                }

                return textoConcatenado;
            }
        }
        /// <summary>
        /// Propriedade: Instruçõe do Boleto Rodapé
        /// Objetivo: Criado para atender exigência do Banco BASA. Onde, deve constar:
        /// ARRECADAÇÃO BASA – CONVÊNIO XXXX – AGÊNCIA ZZZ-Z
        /// </summary>
        public string InstrucoesDoBoletoRodape { get; set; }
        public Remessa Remessa { get; set; }

        #endregion

        public Boleto()
        {
            Inicializa();
        }

        private void Inicializa()
        {
            this.QuantidadeMoeda = null;
            this.Aceite = "";
            this.ValorMoeda = "";
            this.InstrucoesDoBoleto = new List<IInstrucao>();
        }

        public Boleto(Cedente cedente, Sacado sacado, CarteiraCobranca carteiraCobranca)
        {
            Inicializa();

            this.CedenteBoleto = cedente;
            this.SacadoBoleto = sacado;
            this.CarteiraCobranca = carteiraCobranca;
        }

        /// <summary>
        /// Dados que já tem que ser informados na criação do boleto
        /// </summary>
        public void ValidaDadosEssenciaisDoBoleto()
        {
            if (this.CarteiraCobranca == null)
                throw new ApplicationException("Informe a carteira de cobrança.");

            if (String.IsNullOrEmpty(this.NumeroDocumento))
                throw new ApplicationException("Número do documento não consta no boleto.");

            if (String.IsNullOrEmpty(this.SequencialNossoNumero))
                throw new ApplicationException("Nosso número não foi informado.");
        }

        public void SetNossoNumeroFormatado(string valor)
        {
            this.NossoNumeroFormatado = valor;
        }
    }
}
