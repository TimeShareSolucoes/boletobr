using System;

namespace BoletoBr.Arquivo.Generico.Retorno
{
    public class RetornoDetalheGenerico
    {
        #region CNAB 240

        #region Segmento T

        public string NossoNumero { get; set; }
        public string Carteira { get; set; }
        public string NumeroDocumento { get; set; }
        public string InscricaoSacado { get; set; }
        public string NomeSacado { get; set; }
        public string CodigoMovimento { get; set; }
        public string CodigoOcorrencia { get; set; }
        public string TipoLiquidacao { get; set; }
        public decimal ValorTitulo { get; set; }
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
        public decimal ValorLiquido { get; set; }
        public decimal ValorOutrasDespesas { get; set; }
        public decimal ValorOutrosCreditos { get; set; }
        public DateTime DataLiquidacao { get; set; }
        public DateTime DataCredito { get; set; }
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
        #endregion
    }
}
