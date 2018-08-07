using System;

namespace BoletoBr.Arquivo.CNAB240.Remessa
{
    public class DetalheSegmentoRRemessaCnab240
    {
        public DetalheSegmentoRRemessaCnab240(Boleto boleto, int numeroRegistroNoLote)
        {
            this.NumeroRegistro = numeroRegistroNoLote;
            this.ValorMulta = boleto.ValorMulta.GetValueOrDefault();
            this.PercentualMulta = boleto.PercentualMulta.GetValueOrDefault();
            this.DataAplicarMulta = boleto.DataMulta > DateTime.MinValue ? boleto.DataMulta : boleto.DataVencimento.AddDays(1);
        }
        public int LoteServico { get; set; }
        public int NumeroRegistro { get; set; }
        public decimal ValorMulta { get; set; }
        public decimal PercentualMulta { get; set; }
        public DateTime DataAplicarMulta { get; set; }
    }
}
