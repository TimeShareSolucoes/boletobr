using System;

namespace BoletoBr.Arquivo.Generico.Remessa
{
    public class RemessaDetalheGenerica
    {

        #region Segmento P

        public string NossoNumero { get; set; }
        public string Carteira { get; set; }
        public string NumeroDocumento { get; set; }
        public string CodigoOcorrencia { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorDocumento { get; set; }
        public decimal ValorJuros { get; set; }
        public decimal ValorDescontos { get; set; }
        public decimal ValorIof { get; set; }
        public decimal ValorAbatimento { get; set; }

        #endregion

        #region Segmento Q

        public string InscricaoSacado { get; set; }
        public string NomeSacado { get; set; }
        public string CidadeSacado { get; set; }
        public string UfSacado { get; set; }
        public string InscricaoAvalista { get; set; }
        public string NomeAvalista { get; set; }

        #endregion
    }
}
