using System;
using BoletoBr.Interfaces;

namespace BoletoBr.Arquivo.Generico.Retorno
{
    public class RetornoDetalheGenerico
    {
        #region CNAB240

        #region Segmento T

        public string NossoNumero { get; set; }
        public string Carteira { get; set; }
        public string NumeroDocumento { get; set; }
        public string InscricaoSacado { get; set; }
        public string NomeSacado { get; set; }
        public string CodigoMovimento { get; set; }
        public string CodigoOcorrencia { get; set; }
        public string TipoLiquidacao { get; set; }
        public decimal ValorDocumento { get; set; }
        public decimal ValorTarifaCustas { get; set; }
        public DateTime DataVencimento { get; set; }

        #endregion

        #region Segmento U

        public DateTime DataPagamento { get; set; }
        // Juros/Multa/Encargos
        public decimal ValorAcrescimos { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorAbatimento { get; set; }
        public decimal ValorIof { get; set; }
        public decimal ValorPago { get; set; }
        //public decimal ValorLiquido { get; set; }
        public decimal ValorOutrasDespesas { get; set; }
        public decimal ValorOutrosCreditos { get; set; }
        public DateTime? DataLiquidacao { get; set; }
        public DateTime? DataCredito { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public decimal ValorOcorrencia { get; set; }
        public DateTime DataDebitoTarifaCustas { get; set; }

        #endregion

        #endregion

        #region CNAB400

        public string StatusDaParcela { get; set; }
        public int PercentualMulta { get; set; }
        public decimal ValorPorDiaAtraso { get; set; }
        public DateTime DataLimiteParaDesconto { get; set; }
        public string TipoCobranca { get; set; }
        public string Especie { get; set; }
        public int PercentualDesconto { get; set; }
        public decimal ValorJurosDesconto { get; set; }
        public int PercentualIof { get; set; }
        public decimal ValorIofDesconto { get; set; }
        public decimal ValorRecebido { get; set; }
        public decimal ValorOutrosRecebimentos { get; set; }
        public decimal ValorAbatimentoNaoAproveitadoPeloSacado { get; set; }
        public decimal ValorLancamento { get; set; }
        public decimal ValorJuros { get; set; }
        public decimal ValorMulta { get; set; }
        public string SeuNumero { get; set; }

        #endregion

        public string MensagemOcorrenciaRetornoBancario { get; set; }

        public ICodigoOcorrencia Ocorrencia { get; set; }

        #region Propriedades de uso livre
        /// <summary>
        /// Pode ser usado livremente.
        /// Foi pensado para armazenar informações do tipo:
        /// -> REGISTRO VÁLIDO
        /// -> NÃO ENCONTRADO
        /// -> PRONTO PARA PROCESSAR
        /// -> etc.
        /// </summary>
        public string StatusProcessamentoRegistro { get; set; }
        #endregion

        public bool Pago
        {
            get
            {
                if (ValorRecebido > 0 && DataCredito.HasValue && DataCredito.Value > DateTime.MinValue)
                    return true;

                return false;
            }
        }
    }
}
