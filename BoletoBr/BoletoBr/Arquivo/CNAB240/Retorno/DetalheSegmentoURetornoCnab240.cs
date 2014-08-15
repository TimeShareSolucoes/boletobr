using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Dominio
{
    public class DetalheSegmentoURetornoCnab240
    {
        public int CodigoBanco { get; set; }
        public string LoteServico { get; set; }
        public int CodigoRegistro { get; set; }
        public int NumeroRegistro { get; set; }
        public string CodigoSegmento { get; set; }
        public int CodigoMovimento { get; set; }
        public decimal JurosMultaEncargos { get; set; }
        public decimal ValorDescontoConcedido { get; set; }
        public decimal ValorAbatimentoConcedido { get; set; }
        public decimal ValorIofRecolhido { get; set; }
        public decimal ValorPagoPeloSacado { get; set; }
        public decimal ValorLiquidoASerCreditado { get; set; }
        public decimal ValorOutrasDespesas { get; set; }
        public decimal ValorOutrosCreditos { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public DateTime DataPagamento { get; set; }
        public DateTime DataLiquidacao { get; set; }
        public DateTime DataCredito { get; set; }
        public DateTime DataDebitoTarifa { get; set; }
        public string CodigoSacadoNoBanco { get; set; }
        public int CodigoBancoCompensacao { get; set; }
        public string NossoNumeroBancoCompensacao { get; set; }

        #region Bradesco

        public string CodigoOcorrenciaPagador { get; set; }
        public DateTime DataOcorrenciaPagador { get; set; }
        public decimal ValorOcorrenciaPagador { get; set; }
        public string ComplementoOcorrenciaPagador { get; set; }

        #endregion

        #region Caixa

        #region Segmento U para códigos de movimento 35,36 e 37

        public int NumeroBancoDeSacados { get; set; }
        public string NomeBancoDeSacados { get; set; }
        public string IdAjusteVencimento { get; set; }
        public string IdAjusteEmissao { get; set; }
        public string IdModeloBloqueto { get; set; }
        public string IdViaEntregaDistribuicao { get; set; }
        public string IdEspecieTitulo { get; set; }
        public string IdAceite { get; set; }

        #endregion

        #endregion

    }
}
    

