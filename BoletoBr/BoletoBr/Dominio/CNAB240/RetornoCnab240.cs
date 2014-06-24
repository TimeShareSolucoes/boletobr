using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public class RetornoCnab240
    {
        public HeaderRetornoCnab240 Header { get; set; }
        public List<DetalheRetornoCnab240> RegistrosDetalhe { get; set; }
        public TrailerRetornoCnab240 Trailer { get; set; }
    }
}
