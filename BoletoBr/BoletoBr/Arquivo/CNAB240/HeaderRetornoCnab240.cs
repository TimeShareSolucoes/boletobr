using System;
using System.Collections.Generic;
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
        public long NumeroInscricaoEmpresa { get; set; }
        public int CodigoAgencia { get; set; }
        public string DvCodigoAgencia { get; set; }
        public int CodigoCedente { get; set; }
        public string NomeDoBeneficiario { get; set; }
        public string NomeDoBanco { get; set; }
        public int CodigoRemessaRetorno { get; set; }
        public int DataGeracaoGravacao { get; set; }
        public int HoraGeracaoGravacao { get; set; }
        public int NumeroSequencial { get; set; }
        public int VersaoLayout { get; set; }
        public string Densidade { get; set; }
        public string UsoBanco { get; set; }
        public string UsoEmpresa { get; set; }
        public string VersaoAplicativo { get; set; }
    }
}
