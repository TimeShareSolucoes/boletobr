using System;

namespace BoletoBr.Arquivo.CNAB400.Remessa
{
    public class DetalheRemessaCnab400
    {
        public string NossoNumero { get; set; }
        public string Carteira { get; set; }
        public string CodigoOcorrencia { get; set; }
        public DateTime Vencimento { get; set; }
        public decimal ValorTitulo { get; set; }
        public string InscricaoPagador { get; set; }
        public string NomePagador { get; set; }
        public string CidadePagador { get; set; }
        public string UfPagador { get; set; }
    }
}
