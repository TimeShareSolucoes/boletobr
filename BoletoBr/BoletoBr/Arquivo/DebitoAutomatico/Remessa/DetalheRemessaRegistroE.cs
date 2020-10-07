using BoletoBr.Arquivo.DebitoAutomatico.Retorno;
using System; 

namespace BoletoBr.Arquivo.DebitoAutomatico.Remessa
{
    /// <summary>
    /// Registro “E” - Débito em Conta Corrente
    /// </summary>
    public class DetalheRemessaRegistroE
    {
        public enum TipoCodigoMoeda
        {
            UFIR,
            REAL
        }

        public enum FormaCobranca
        {
            FIDIC,
            NaoCumulativaCOFINS
        }
        public DetalheRemessaRegistroE(DetalheRetornoRegistroB retorno,
                                        DateTime dataVencimento,
                                        decimal valorBoleto,
                                        TipoCodigoMoeda tipoCodigoMoeda,
                                        FormaCobranca formaCobranca,
                                        int codMovimento,
                                        decimal valorTributo = 0,
                                        string usoEmpresa = ""
                                        )
        {
            this.IdentificacaoClienteEmpresa = retorno.IdentificacaoClienteEmpresa;
            this.AgenciaDebito = retorno.AgenciaDebito;
            this.IdentificacaoClienteBanco = retorno.IdentificacaoClienteBanco;

            this.DataVencimento = dataVencimento;
            this.ValorDebito = valorBoleto;
            this.ValorTotalTributos = valorTributo;
            this.CodigoMoeda = tipoCodigoMoeda == TipoCodigoMoeda.REAL ? "03" : "01";
            this.TipoCobranca = formaCobranca == FormaCobranca.FIDIC ? "X" : "Y";
            this.CodigoMovimento = codMovimento;
            this.UsoEmpresa = usoEmpresa;

        }

        /// <summary>
        /// "E"
        /// </summary>
        public string CodigoRegistro => "E";

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
        /// Data em que deverá ser efetuado o débito na conta corrente.
        /// Ser for informado um dia não útil, o débito será efetuado no
        /// primeiro dia útil subseqüente.
        /// Formato AAAAMMDD.
        /// </summary>
        public DateTime DataVencimento { get; set; }

        /// <summary>
        /// Valor a ser debitado na conta corrente.
        /// Quando for igual a “zero”, será utilizado para efeito de
        /// “manutenção” da autorização no cadastro de cliente.
        /// </summary>
        public decimal ValorDebito { get; set; }

        /// <summary>
        /// “01” = UFIR, neste caso, ler o valor do débito com 5
        /// decimais “03” = REAL, neste caso, ler o valor do débito com 2
        /// decimais
        /// </summary>
        public string CodigoMoeda { get; set; }

        /// <summary>
        /// Valor total dos tributos – Lei n. 10.833 
        /// </summary>
        public decimal ValorTotalTributos { get; set; }

        /// <summary>
        /// Esta informação não será tratada pelo Banco (posições 070 a 118).
        /// Será retornada para a empresa, com o mesmo conteúdo enviado.
        /// </summary>
        public string UsoEmpresa { get; set; }

        /// <summary>
        /// Se X na ultima posição = FIDC
        /// Se Y na ultima posição = Lei n. 10.833
        /// Obs.: Quando do Y, o valor a ser debitado na conta do
        /// cliente devera ser obtido pela diferença do campo E06 com
        /// o valor constante do E08.
        /// </summary>
        public string TipoCobranca { get; set; }

        /// <summary>
        /// 0 = Débito Normal
        /// 1 = Cancelamento(exclusão) de lançamento enviado
        /// anteriormente para o Banco.O cancelamento só será
        /// efetuado, desde que o débito ainda não tenha sido efetivado.
        /// </summary>
        public int CodigoMovimento { get; set; }
    }
}
