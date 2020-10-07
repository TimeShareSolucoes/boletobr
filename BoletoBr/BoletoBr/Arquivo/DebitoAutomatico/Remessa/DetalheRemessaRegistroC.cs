using BoletoBr.Arquivo.DebitoAutomatico.Retorno; 

namespace BoletoBr.Arquivo.DebitoAutomatico.Remessa
{
    /// <summary>
    /// Registro “C” - Ocorrências no Cadastramento do Débito Automático
    //  Gerado pela Empresa para o Banco, somente para cada “cadastramento” (registro “B”), enviado pelo Banco, que for
    //  recusado pela Empresa.EM HIPÓTESE ALGUMA DEVE SER GERADO PARA OS REGISTROS ACEITOS.
    /// </summary>
    public class DetalheRemessaRegistroC
    {
        public DetalheRemessaRegistroC(DetalheRetornoRegistroB retorno, string ocorrencia1, string ocorrencia2)
        {
            
            this.IdentificacaoClienteEmpresa = retorno.IdentificacaoClienteEmpresa;
            this.AgenciaDebito = retorno.AgenciaDebito;
            this.IdentificacaoClienteBanco = retorno.IdentificacaoClienteBanco;
            this.Ocorrencia1 = ocorrencia1;
            this.Ocorrencia2 = ocorrencia2;
            this.CodigoMovimento = retorno.CodigoMovimento;
        }

        /// <summary>
        /// "C"
        /// </summary>
        public string CodigoRegistro => "C";

        /// <summary>
        /// O conteúdo deverá ser idêntico ao anteriormente enviado
        /// pelo Banco, no registro tipo “B”
        /// </summary>
        public string IdentificacaoClienteEmpresa { get; set; }

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
        /// Mensagem explicativa da “recusa”, pela Empresa. Por
        ///        Exemplo :
        /// Identificação do cliente não localizada / inexistente
        /// Restrição de cadastramento pela empresa
        /// Cliente cadastrado em outro Banco com data posterior
        /// Operadora invalida
        /// Cliente desativado no cadastro da empresa
        /// </summary>
        public string Ocorrencia1 { get; set; }

        /// <summary>
        /// Complemento da mensagem explicativa da “recusa”
        /// </summary>
        public string Ocorrencia2 { get; set; }

        /// <summary>
        /// O conteúdo deverá ser idêntico ao anteriormente enviado
        /// pelo Banco, no registro tipo “B”
        /// </summary>
        public int CodigoMovimento { get; set; }
    }
}
