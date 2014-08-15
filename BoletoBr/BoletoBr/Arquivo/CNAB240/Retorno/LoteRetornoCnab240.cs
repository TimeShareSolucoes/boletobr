using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.CNAB240.Retorno;
using BoletoBr.Dominio;

namespace BoletoBr.Arquivo.CNAB240
{
    public class LoteRetornoCnab240
    {
        public LoteRetornoCnab240()
        {
            this.RegistrosDetalheSegmentos = new List<DetalheSegmentoRetornoCnab240>();
        }
        public HeaderLoteRetornoCnab240 HeaderLote { get; set; }
        public List<DetalheSegmentoRetornoCnab240> RegistrosDetalheSegmentos { get; set; }
        public TrailerLoteRetornoCnab240 TrailerLote { get; set; }
    }
}
