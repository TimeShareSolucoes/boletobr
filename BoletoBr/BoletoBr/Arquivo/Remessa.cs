using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Dominio
{
    /// <summary>
    /// Contém informações que são pertinentes a um boleto, mas para geração da Remessa. Não são necessárias para Impressão do Boleto
    /// Autor: sidneiklein Data: 09/08/2013
    /// </summary>
    public class Remessa
    {
        public enum EnumTipoAmbiemte
        {
            Homologacao,
            Producao
        }
        //
        #region Atributos e Propriedades

        /// <summary>
        /// Variável que define se a Remessa é para Testes ou Produção
        /// </summary>
        public EnumTipoAmbiemte Ambiente { get; set; }

        /// <summary>
        /// Código de Ocorrência Utilizado na geração da Remessa.
        /// |Identificado no Banrisul        como "CODIGO OCORRENCIA" by sidneiklein|
        /// |Identificado no Banco do Brasil como "COMANDO"           by sidneiklein|
        /// </summary>
        public string CodigoOcorrencia { get; set; }

        /// <summary>
        /// Tipo Documento Utilizado na geração da remessa. |Identificado no Banrisul by sidneiklein|
        /// Tipo Cobranca Utilizado na geração da remessa.  |Identificado no Sicredi by sidneiklein|
        /// </summary>
        public string TipoDocumento { get; set; }

        #endregion
    }
}
