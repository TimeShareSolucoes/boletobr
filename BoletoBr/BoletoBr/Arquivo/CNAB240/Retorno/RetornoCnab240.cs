using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.CNAB240;
using BoletoBr.Arquivo.CNAB240.Retorno;
using BoletoBr.Dominio;

namespace BoletoBr
{
    public class RetornoCnab240
    {
        public RetornoCnab240()
        {
            this.Lotes = new List<LoteRetornoCnab240>();
        }
        public HeaderRetornoCnab240 Header { get; set; }
        public List<LoteRetornoCnab240> Lotes { get; set; } 
        public TrailerRetornoCnab240 Trailer { get; set; }
    }
}
