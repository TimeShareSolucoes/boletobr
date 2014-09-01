using System.Collections.Generic;

namespace BoletoBr.Arquivo.CNAB240.Retorno
{
    public class LoteRetornoCnab240
    {
        public LoteRetornoCnab240()
        {
            this.RegistrosDetalheSegmentos = new List<DetalheRetornoCnab240>();
        }
        public HeaderLoteRetornoCnab240 HeaderLote { get; set; }
        public List<DetalheRetornoCnab240> RegistrosDetalheSegmentos { get; set; }
        public TrailerLoteRetornoCnab240 TrailerLote { get; set; }
    }
}
