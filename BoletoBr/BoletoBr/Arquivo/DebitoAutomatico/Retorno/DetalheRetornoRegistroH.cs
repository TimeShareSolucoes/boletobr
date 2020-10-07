

namespace BoletoBr.Arquivo.DebitoAutomatico.Retorno
{
    /// <summary>
    ///  Registro “H” - Ocorrência de Alteração da Identificação do Cliente na Empresa
    ///  Gerado pelo Banco para a Empresa, somente para cada “alteração” (registro “D”), enviada pela Empresa, que for recusada
    ///  pelo Banco.EM HIPÓTESE ALGUMA SERÁ GERADO PARA OS REGISTROS ACEITOS.
    /// </summary>
    public class DetalheRetornoRegistroH
    { 
        /// <summary>
        /// "H"
        /// </summary>
        public string CodigoRegistro { get; set; }

        /// <summary>
        /// O conteúdo deverá ser idêntico ao anteriormente enviado pela Empresa, no registro tipo “D” 
        /// </summary>
        public string IdentificacaoClienteEmpresaAnterior { get; set; }

        /// <summary>
        /// O conteúdo deverá ser idêntico ao anteriormente enviado pela Empresa, no registro tipo “D” 
        /// </summary>
        public string IdentificacaoClienteEmpresaAtual { get; set; }

        /// <summary>
        /// O conteúdo deverá ser idêntico ao anteriormente enviado pela Empresa, no registro tipo “D” 
        /// </summary>
        public string AgenciaDebito { get; set; }

        /// <summary>
        /// O conteúdo deverá ser idêntico ao anteriormente enviado pela Empresa, no registro tipo “D” 
        /// </summary>
        public string IdentificacaoClienteBanco { get; set; }

        /// <summary>
        ///Mensagem explicativa do não processamento 
        /// </summary>
        public string Ocorrencia { get; set; }

        /// <summary>
        /// O conteúdo deverá ser idêntico ao anteriormente enviado pela Empresa, no registro tipo “D” 
        /// </summary>
        public int CodigoMovimento { get; set; }
    }
}
