 
namespace BoletoBr.Arquivo.DebitoAutomatico.Retorno
{
    public class DetalheRetornoRegistroX
    {
        /// <summary>
        /// "X"
        /// </summary>
        public string CodigoRegistro { get; set; }
        public string CodigoAgencia { get; set; }

        /// <summary>
        /// Nome Abreviado. 
        /// </summary>
        public string NomeAgencia { get; set; }

        /// <summary>
        /// Logradouro. 
        /// </summary>
        public string EnderecoAgencia { get; set; }
        public string Numero { get; set; }
        public string CodigoCep { get; set; }
        public string SufixoCep { get; set; }
        public string NomeCidade { get; set; }

        /// <summary>
        /// Sigla da Unidade Federativa (UF) 
        /// </summary>
        public string SiglaEstado { get; set; }

        /// <summary>
        /// “A” = Ativa “B” = em regime de encerramento 
        /// </summary>
        public string SituacaoAgencia { get; set; }
        public string SituacaoAgenciaTratado
        {
            get
            {
                switch (SituacaoAgencia)
                {
                    case "A": return "Ativo";
                    case "B": return "Em regime de encerramento";
                    default:
                        return "Código inválido";
                }
            }
        }

    }
}
