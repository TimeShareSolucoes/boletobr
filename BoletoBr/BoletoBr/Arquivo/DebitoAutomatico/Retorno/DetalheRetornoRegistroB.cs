using System; 

namespace BoletoBr.Arquivo.DebitoAutomatico.Retorno
{
    /// <summary>
    /// Registro “B” - Cadastramento de Débito Automático
    /// Gerado pelo Banco para a Empresa, para cada inclusão ou exclusão, de optante pelo débito automático.
    /// </summary>
    public class DetalheRetornoRegistroB
    {

        /// <summary>
        /// "B"
        /// </summary>
        public string CodigoRegistro { get; set; }

        /// <summary>
        /// Esta identificação deverá :
        /// Conter um processo de validação(DV),
        /// Ser única para cada cliente da empresa,
        /// Ser a mesma em todos os débitos consecutivos de um
        /// mesmo cliente.
        /// Esta informação será validada pelo Banco, conforme regra
        /// definida pela empresa, no momento do cadastramento.
        /// </summary>
        public string IdentificacaoClienteEmpresa { get; set; }

        /// <summary>
        /// Identificação da Agência no Banco onde será efetuado o Débito Automático
        /// </summary>
        public string AgenciaDebito { get; set; }

        /// <summary>
        /// Identificação utilizada pelo Banco para efetuar o débito 
        /// </summary>
        public string IdentificacaoClienteBanco { get; set; }

        /// <summary>
        /// B05-Data da Opção/Exclusão
        ///Conterá :
        /// Data de Exclusão, se Código de Movimento igual a 1, Data de Inclusão, se Código de Movimento igual a 2. Formato AAAAMMDD
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// 1 = Exclusão de optante pelo débito automático
        /// 2 = Inclusão de optante pelo débito automático
        /// </summary>
        public int CodigoMovimento { get; set; }

        public string CodigoMovimentoTratado
        {
            get
            {
                switch (CodigoMovimento)
                {
                    case 1: return "Exclusão";
                    case 2: return "Inclusão";
                    default:
                        return "Código inválido";
                }
            }
        }

        /// <summary>
        /// Campo gerado para controle, não é lido pelo arquivo
        /// </summary>
        public bool Aceito { get; set; }
        
        /// <summary>
        /// Campo gerado para controle, não é lido pelo arquivo
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// Campo gerado para controle, não é lido pelo arquivo
        /// </summary>
        public string Observacao { get; set; }
    }
}
