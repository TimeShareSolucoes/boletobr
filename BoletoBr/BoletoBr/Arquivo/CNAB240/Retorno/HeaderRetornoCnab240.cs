using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public class HeaderRetornoCnab240
    {
        /// <summary>
        /// Caixa -> Código do Banco na Compensação
        /// </summary>
        public int CodigoBanco { get; set; }
        public string LoteServico { get; set; }
        public int CodigoRegistro { get; set; }
        public int TipoInscricaoEmpresa { get; set; }
        public string NumeroInscricaoEmpresa { get; set; }
        public string Convenio { get; set; }
        public int CodigoAgencia { get; set; }
        public string DvCodigoAgencia { get; set; }
        public string ContaCorrente { get; set; }
        public string DvContaCorrente { get; set; }
        public int CodigoCedente { get; set; }
        public string NomeDoBeneficiario { get; set; }
        public string NomeDoBanco { get; set; }
        public int CodigoRemessaRetorno { get; set; }
        public DateTime? DataGeracaoGravacao { get; set; }

        public int HoraGeracaoGravacao { get; set; }
        public int NumeroSequencial { get; set; }
        public string VersaoLayout { get; set; }
        public string Densidade { get; set; }
        public string UsoBanco { get; set; }
        public string UsoEmpresa { get; set; }
        public string UsoFebraban { get; set; }

        #region Banco do Brasil

        public string ConvenioNumeroCobranca { get; set; }
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

        #region Caixa

        public string VersaoAplicativo { get; set; }

        #endregion
    }
}
