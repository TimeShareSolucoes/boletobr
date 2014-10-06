
using System;


namespace BoletoBr.Arquivo.CNAB400.Remessa
{
    public class HeaderRemessaCnab400
    {
        public HeaderRemessaCnab400(Boleto boleto, int numeroSequencialRemessa, int numeroSequencialRegistro)
        {
            this.CodigoBanco = boleto.BancoBoleto.CodigoBanco;
            this.CodigoEmpresa = boleto.CedenteBoleto.CodigoCedente;
            this.NomeEmpresa = boleto.CedenteBoleto.Nome;
            this.DataDeGravacao = DateTime.Now;
            this.NumeroSequencialRemessa = numeroSequencialRemessa;
            this.NumeroSequencialRegistro = numeroSequencialRegistro;
        }

        public string CodigoBanco { get; set; }
        public DateTime DataDeGravacao { get; set; }
        public int NumeroSequencialRegistro { get; set; }
        public int NumeroSequencialRemessa { get; set; }
        public string CodigoEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
    }
}
