using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public class TrailerRetornoCnab240
    {
        public int CodigoBanco { get; set; }
        public string LoteServico { get; set; }
        public int CodigoRegistro { get; set; }
        public int QtdLotesArquivo { get; set; }
        public int QtdRegistrosArquivo { get; set; }

        #region Bradesco

        public int QtdContasConc { get; set; }

        #endregion
    }
}
