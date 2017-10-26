using System;

namespace BoletoBr.Arquivo.CNAB240.Retorno
{
    public class TrailerLoteRetornoCnab240
    {
        public int CodigoBanco { get; set; }
        public string LoteServico { get; set; }
        public int CodigoRegistro { get; set; }
        public int TipoRegistro { get; set; }
        public int TipoIncricaoEmpresa { get; set; }
        public string NumeroIncricaoEmpresa { get; set; }
        public string ConvenioBanco { get; set; }
        public string AgenciaConta { get; set; }
        public string DigitoAgencia { get; set; }
        public string NumeroContaCorrente { get; set; }
        public string DigitoContaCorrente { get; set; }
        public string DigitoAgenciaConta { get; set; }
        public decimal ValorVinculadoDiaAnterior { get; set; }
        public decimal ValorLimiteConta { get; set; }
        public decimal ValorVinculadoDia { get; set; }
        public DateTime DataSaldoFinal { get; set; }
        public decimal SomaValoresaDebito { get; set; }
        public decimal SomaValoresaCredito { get; set; }
        public long QtdRegistrosLote { get; set; }
        public long QtdTitulosCobrancaSimples { get; set; }
        public decimal ValorTitulosCobrancaSimples { get; set; }
        public long QtdTitulosCobrancaCaucionada { get; set; }
        public decimal ValorTitulosCobrancaCaucionada { get; set; }
        public long QtdTitulosCobrancaDescontada { get; set; }
        public decimal ValorTitulosCobrancaDescontada { get; set; }

        #region Bradesco

        public long QtdTitulosCobrancaVinculada { get; set; }
        public decimal ValorTitulosCobrancaVinculada { get; set; }
        public string NumeroAvisoLancamento { get; set; }

        #endregion
    }
}
