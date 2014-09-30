using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Arquivo.CNAB240.Remessa
{
    public class LoteRemessaCnab240
    {
        public LoteRemessaCnab240()
        {
            RegistrosDetalheSegmentos = new List<DetalheRemessaCnab240>();
        }
        public HeaderLoteRemessaCnab240 HeaderLote { get; set; }
        public List<DetalheRemessaCnab240> RegistrosDetalheSegmentos { get; set; }
        public TrailerLoteRemessaCnab240 TrailerLote { get; set; }
    }
}
