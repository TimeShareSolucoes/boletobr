using BoletoBr.Arquivo.DebitoAutomatico.Retorno; 

namespace BoletoBr.Arquivo.DebitoAutomatico.Remessa
{
    /// <summary>
    /// Gerado pela Empresa para o Banco, obrigatoriamente, nas seguintes situações :
    /// Necessidade de alteração, por parte da Empresa, da “Identificação do Cliente na Empresa”. Em cada registro será
    /// informado o par : Identificação Anterior / Identificação Atual(DE/PARA), e
    /// Nas situações onde a empresa “necessitar” excluir o cliente da modalidade de débito automático.
    /// </summary>
    public class DetalheRemessaRegistroD
    {
        public DetalheRemessaRegistroD(DetalheRetornoRegistroB retorno, string ocorrencia, string novoIdentificadorClienteEmpresa, int codMovimento)
        {
             

            this.IdentificacaoClienteEmpresaAnterior = retorno.IdentificacaoClienteEmpresa;
            this.AgenciaDebito = retorno.AgenciaDebito;
            this.IdentificacaoClienteBanco = retorno.IdentificacaoClienteBanco;

            this.IdentificacaoClienteEmpresaAtual = novoIdentificadorClienteEmpresa;           
            this.Ocorrencia = ocorrencia;
            this.CodigoMovimento = codMovimento;
        }

        /// <summary>
        /// "D"
        /// </summary>
        public string CodigoRegistro => "D";

        /// <summary>
        /// Identificação do Cliente na Empresa - Anterior
        /// </summary>
        public string IdentificacaoClienteEmpresaAnterior { get; set; }

        /// <summary>
        /// Identificação do Cliente na Empresa - Atual
        /// </summary>
        public string IdentificacaoClienteEmpresaAtual { get; set; }

        /// <summary>
        /// O conteúdo deverá ser idêntico ao anteriormente enviado
        /// pelo Banco, no registro tipo “B”
        /// </summary>
        public string AgenciaDebito { get; set; }

        /// <summary>
        /// O conteúdo deverá ser idêntico ao anteriormente enviado
        /// pelo Banco, no registro tipo “B”
        /// </summary>
        public string IdentificacaoClienteBanco { get; set; }

        /// <summary>
        ///Mensagem explicativa do movimento enviado pela
        ///Empresa, quando o Código do Movimento for igual a 1. Por
        ///exemplo:
        ///Exclusão por alteração cadastral do cliente,
        ///Exclusão - transferido para débito em outro banco,
        ///Exclusão por insuficiência de fundos,
        ///Exclusão por solicitação do cliente.
        /// </summary>
        public string Ocorrencia { get; set; }

        /// <summary>
        /// 0 = Alteração da Identificação do Cliente na Empresa
        /// 1 = Exclusão de optante do Débito Automático, solicitado
        /// pela Empresa, conforme clausulas contratuais do convênio
        /// </summary>

        public int CodigoMovimento { get; set; }
    }
}
