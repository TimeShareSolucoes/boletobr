using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public class RetornoCnab400
    {
        public RetornoCnab400()
        {
            this.RegistrosDetalhe = new List<DetalheRetornoCnab400>();
        }
        public HeaderRetornoCnab400 Header { get; set; }
        public List<DetalheRetornoCnab400> RegistrosDetalhe { get; set; }
        public TrailerRetornoCnab400 Trailer { get; set; }
    }
}
