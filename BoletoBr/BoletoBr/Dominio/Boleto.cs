using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using BoletoBr.Arquivo;
using BoletoBr.Bancos;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;
using BoletoBr.Interfaces;

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
        public string NossoNumero { get; set; }
        public string DigitoNossoNumero { get; set; }
        public string SequencialNossoNumero { get; set; }
        /// <summary>
        /// Deve ser gerado pelo componente
        /// </summary>
        public string NossoNumeroFormatado { get; private set; }
        public string NumeroDocumento { get; set; }
        public string DigitoNumeroDocumento { get; set; }
        public string NumeroDocumentoFormatado { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataDocumento { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public int QtdParcelas { get; set; }
        public int NumeroParcela { get; set; }

        public string NumeroParcelaFormatado
        {
            get { return String.Format("{0} / {1}", NumeroParcela.ToString().PadLeft(3, '0'), QtdParcelas.ToString().PadLeft(3, '0')); }
        }

        public decimal ValorBoleto { get; set; }
        public decimal? ValorCobrado { get; set; }
        public string LocalPagamento { get; set; }
        public decimal? QuantidadeMoeda { get; set; }
        public string ValorMoeda { get; set; }
        public string Aceite { get; set; }
        public IEspecieDocumento Especie { get; set; }
        public string Moeda { get; set; }
        //public decimal? ValorAbatimentoDesconto { get; set; }
        public decimal? ValorAbatimento { get; set; }
        public decimal? ValorDesconto { get; set; }
        public decimal? ValorDescontoDia { get; set; }
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
        public string TipoModalidade { get; set; }
        public string CodigoBarraBoleto { get; set; }
        public string LinhaDigitavelBoleto { get; set; }      
        public TipoArquivo TipoArquivo { get; set; }
        public string CodigoDoProduto { get; set; }
        public int QtdDias { get; set; }
        public List<IInstrucao> InstrucoesDoBoleto { get; set; }
        public Remessa Remessa { get; set; }
        /// <summary>
        /// Código que identifica a finalizade do boleto no arquivo magnético de REMESSA/RETORNO
        /// Propriedade conhecida como:
        /// Comando
        /// Código de Movimento
        /// Código de Oocorrência
        /// </summary>
        public ICodigoOcorrencia CodigoOcorrenciaRemessa { get; set; }
        public ICodigoOcorrencia CodigoOcorrenciaRetorno { get; set; }

        #region Banco Santander

        public int PercentualIOS { get; set; }
        public string CodigoDeTransmissao { get; set; }
        // É usado no Banco Santander como Data do Segundo Desconto, pág. 8 do manual técnico do Santander
        public DateTime DataLimite { get; set; }

        #endregion

        public void AdicionarCodigoEspecie(IEspecieDocumento codigoEspecieDocumento)
        {
            return;
        }

        public void AdicionarOcorrenciaRemessa(ICodigoOcorrencia ocorrencia)
        {
            return;
        }

        public void AdicionarInstrucao(EnumTipoInstrucao tipoInstrucao, double valor, DateTime data, int dias)
        {
            this.InstrucoesDoBoleto.Add(this.BancoBoleto.ObtemInstrucaoPadronizada(tipoInstrucao, valor, data, dias));
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

        public Boleto(CarteiraCobranca carteiraCobranca, Cedente cedente, Sacado sacado, Remessa remessa)
        {
            Inicializa();

            this.CarteiraCobranca = carteiraCobranca;
            this.CedenteBoleto = cedente;
            this.SacadoBoleto = sacado;
            this.Remessa = remessa;
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
