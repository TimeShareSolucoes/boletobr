using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Dominio;

namespace BoletoBr.Arquivo.CNAB240
{
    public class LoteRetornoCnab240
    {
        public LoteRetornoCnab240()
        {
            this.RegistrosDetalheSegmentoT = new List<DetalheSegmentoTRetornoCnab240>();
            this.RegistrosDetalheSegmentoU = new List<DetalheSegmentoURetornoCnab240>();
        }
        public HeaderLoteRetornoCnab240 HeaderLote { get; set; }
        public List<DetalheSegmentoTRetornoCnab240> RegistrosDetalheSegmentoT { get; set; }
        public List<DetalheSegmentoURetornoCnab240> RegistrosDetalheSegmentoU { get; set; }
        public TrailerLoteRetornoCnab240 TrailerLote { get; set; }
    }
}
