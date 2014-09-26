using System.Collections.Generic;

namespace BoletoBr.Arquivo.CNAB240.Retorno
{
    public class RetornoCnab240
    {
        public RetornoCnab240()
        {
            Lotes = new List<LoteRetornoCnab240>();
        }
        public HeaderRetornoCnab240 Header { get; set; }
        public List<LoteRetornoCnab240> Lotes { get; set; } 
        public TrailerRetornoCnab240 Trailer { get; set; }
    }
}
