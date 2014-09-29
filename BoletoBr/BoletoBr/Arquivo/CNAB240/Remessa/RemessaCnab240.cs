using System.Collections.Generic;

namespace BoletoBr.Arquivo.CNAB240.Remessa
{
    public class RemessaCnab240
    {
        public RemessaCnab240()
        {
            Lotes = new List<LoteRemessaCnab240>();
        }
        public HeaderRemessaCnab240 Header { get; set; }
        public List<LoteRemessaCnab240> Lotes { get; set; } 
        public TrailerRemessaCnab240 Trailer { get; set; }
    }
}
