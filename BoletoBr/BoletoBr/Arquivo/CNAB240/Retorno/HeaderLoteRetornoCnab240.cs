using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Arquivo.CNAB240
{
    public class HeaderLoteRetornoCnab240
    {
        public int CodigoBanco { get; set; }
        public string LoteServico { get; set; }
        public int CodigoRegistro { get; set; }
        public string TipoOperacao { get; set; }
        public int TipoServico { get; set; }
        public int VersaoLayoutLote { get; set; }
        public int TipoInscricaoEmpresa { get; set; }
        public long NumeroInscricaoEmpresa { get; set; }
        public string Convenio { get; set; }
        public int CodigoAgencia { get; set; }
        public string DvCodigoAgencia { get; set; }
        public long ContaCorrente { get; set; }
        public string DvContaCorrente { get; set; }
        public int CodigoCedente { get; set; }
        public int CodigoModeloPersonalizado { get; set; }
        public string NomeDoBeneficiario { get; set; }
        public string Mensagem1 { get; set; }
        public string Mensagem2 { get; set; }
        public long NumeroRemessaRetorno { get; set; }
        public int DataGeracaoGravacao { get; set; }
        public int DataDeCredito { get; set; }

        #region Banco do Brasil

        public long ConvenioNumeroCobranca { get; set; }
        public int CedenteCobranca { get; set; }
        public int CarteiraCobranca { get; set; }
        public int VariacaoCarteiraCobranca { get; set; }

        #endregion

        #region Bradesco

        /// <summary>
        /// NOTA: G012 do Layout do Banco Bradesco CNAB 240
        /// -> Dígito Verificador da Agência e Conta
        /// Código adotado pelo responsável pela conta corrente, para verificação da autenticidade do número da conta corrente.
        /// Para bancos que se utilizam de duas posições para o dígito verificador do número da conta corrente, preencher este campo com a 1ª posição deste dígito.
        /// Exemplo: 
        /// C/C = 45981-36
        /// Neste caso o DV da Ag/Conta = 6
        /// </summary>
        public string DvAgenciaConta { get; set; }

        #endregion
    }
}
