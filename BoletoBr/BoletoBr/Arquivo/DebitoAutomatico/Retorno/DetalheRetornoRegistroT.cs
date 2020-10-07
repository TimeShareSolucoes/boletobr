

namespace BoletoBr.Arquivo.DebitoAutomatico.Retorno
{
    /// <summary>
    /// Registro “T” - Total de Clientes Debitados (registro não tratado pelo BB)
    /// Gerado pelo Banco para a Empresa.
    /// Este registro será enviado juntamente com o registro “F”, quando houver clientes não debitados.
    /// A geração deste registro é opcional. 
    /// </summary>
    public class DetalheRetornoRegistroT
    {
        /// <summary>
        /// "T"
        /// </summary>
        public string CodigoRegistro { get; set; }

        /// <summary>
        /// Somatório da quantidade de registros “E”, que foram efetivamente debitados.
        /// </summary>
        public int QuantidadeDebitado { get; set; }

        /// <summary>
        /// Valor total dos registros “E”, que foram efetivamente debitados.
        /// </summary>
        public decimal TotalDebitado { get; set; }

    }
}
