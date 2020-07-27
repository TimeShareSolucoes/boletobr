 

namespace BoletoBr.Arquivo.DebitoAutomatico.Retorno
{
    public class TrailerRetornoDebitoAutomatico
    { 

        /// <summary>
        /// "Z"
        /// </summary>
        public string CodigoRegistro { get; set; }

        /// <summary>
        /// No somatório dos registros, deverão ser também incluídos, os registros Header e Trailler.
        /// </summary>
        public int QuantidadeRegistros { get; set; }

        /// <summary>
        /// Este campo deverá ser o somatório do campo E06 (quando
        /// for o arquivo remessa - Empresa), ou do campo F06
        /// (quando for arquivo retorno - Banco), independente do
        /// tratamento de casas decimais(ou código de moeda). 
        /// </summary>
        public decimal TotalRegistros { get; set; }

    }
}
