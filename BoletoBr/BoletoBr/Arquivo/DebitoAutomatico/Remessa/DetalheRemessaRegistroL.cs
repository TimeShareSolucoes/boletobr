using System; 

namespace BoletoBr.Arquivo.DebitoAutomatico.Remessa
{
    /// <summary>
    /// Registro “L” - Cronograma de Faturamento de Contas / Tributos
    /// Gerado pela Empresa para o Banco.
    /// Este registro será gerado pela Empresa para o banco, uma vez por mês, ou quando houver datas de vencimento diferentes
    /// do mês anterior.
    /// A geração deste registro é opcional. 
    /// </summary>
    public class DetalheRemessaRegistroL
    {
        public DetalheRemessaRegistroL(DateTime dataFaturamentoContas, DateTime dataVencimento, DateTime dataRemessaArquivo, DateTime dataRemessaContas)
        {
            DataFaturamentoContas = dataFaturamentoContas;
            DataVencimento = dataVencimento;
            DataRemessaArquivo = dataRemessaArquivo;
            DataRemessaContas = dataRemessaContas;
        }
         
        /// <summary>
        /// "L"
        /// </summary>
        public string CodigoRegistro => "L";

        /// <summary>
        /// Formato AAAAMMDD. 
        /// </summary>
        public DateTime DataFaturamentoContas { get; set; }

        /// <summary>
        /// Formato AAAAMMDD. 
        /// </summary>
        public DateTime DataVencimento { get; set; }

        /// <summary>
        /// Formato AAAAMMDD. 
        /// </summary>
        public DateTime DataRemessaArquivo { get; set; }

        /// <summary>
        /// A Data de envio das contas ao Assinante / Consumidor,
        /// deverá ser sempre anterior, a data do envio do arquivo com os débitos ao Banco Formato AAAAMMDD.
        /// </summary>
        public DateTime DataRemessaContas { get; set; }

    }
}
