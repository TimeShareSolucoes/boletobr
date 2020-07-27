using System; 

namespace BoletoBr.Arquivo.DebitoAutomatico.Remessa
{
    /// <summary>
    /// Registro “J” - Confirmação de Processamento de Arquivos (solicite cadastramento do registro à sua agência)
    /// Gerado tanto da Empresa para o Banco, como do Banco para a Empresa.
    /// Cada registro “J”, será correspondente a 01 (um) arquivo processado.
    /// A geração deste registro é opcional.  
    /// </summary>
    public class DetalheRemessaRegistroJ
    {

        public DetalheRemessaRegistroJ(int numeroSequencial,DateTime dataGeracao, int totalRegistroProcessado,decimal valorTotalProcessado, DateTime dataProcessamento)
        {
            NumeroSequencial = numeroSequencial;
            DataGeracao = dataGeracao;
            TotalRegistroProcessado = totalRegistroProcessado;
            ValorTotalProcessado = valorTotalProcessado;
            DataProcessamento = dataProcessamento;
        }

        /// <summary>
        /// "J"
        /// </summary>
        public string CodigoRegistro => "J";

        /// <summary>
        /// O conteúdo deverá ser idêntico ao campo A08 (registro“A”), do arquivo processado.
        /// </summary>
        public int NumeroSequencial { get; set; }

        /// <summary>
        /// O conteúdo deverá ser idêntico ao campo A07 (registro“A”), do arquivo processado.
        /// Formato AAAAMMDD.
        /// </summary>
        public DateTime DataGeracao { get; set; }

        /// <summary>
        /// O conteúdo deverá ser idêntico ao campo Z02 (registro“Z”), do arquivo processado.
        /// </summary>
        public int TotalRegistroProcessado { get; set; }
        /// <summary>
        /// O conteúdo deverá ser idêntico ao campo Z03 (registro“Z”), do arquivo processado.
        /// </summary>
        public decimal ValorTotalProcessado { get; set; }

        /// <summary>
        /// Data de processamento do arquivo pelo Banco ou pela Empresa.
        /// Formato AAAAMMDD.
        /// </summary>
        public DateTime DataProcessamento { get; set; }

    }
}
