using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Arquivo.CNAB240
{
    public class TrailerLoteRetornoCnab240
    {
        public int CodigoBanco { get; set; }
        public string LoteServico { get; set; }
        public int CodigoRegistro { get; set; }
        public long QtdRegistrosLote { get; set; }
        public long QtdTitulosCobrancaSimples { get; set; }
        public decimal ValorTitulosCobrancaSimples { get; set; }
        public long QtdTitulosCobrancaCaucionada { get; set; }
        public decimal ValorTitulosCobrancaCaucionada { get; set; }
        public long QtdTitulosCobrancaDescontada { get; set; }
        public decimal ValorTitulosCobrancaDescontada { get; set; }

        #region Bradesco

        public long QtdTitulosCobrancaVinculada { get; set; }
        public decimal ValorTitulosCobrancaVinculada { get; set; }
        public string NumeroAvisoLancamento { get; set; }

        #endregion
    }
}
