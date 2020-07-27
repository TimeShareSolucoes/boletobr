using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Arquivo.DebitoAutomatico.Remessa
{
    public class RemessaDebitoAutomatico
    {
        public HeaderRemessaDebitoAutomatico Header { get; set; }
        public List<DetalheRemessaRegistroC> RegistrosDetalheC { get; set; }
        public List<DetalheRemessaRegistroD> RegistrosDetalheD { get; set; }
        public List<DetalheRemessaRegistroI> RegistrosDetalheI { get; set; }
        public List<DetalheRemessaRegistroJ> RegistrosDetalheJ { get; set; }
        public List<DetalheRemessaRegistroL> RegistrosDetalheL { get; set; }
        public List<DetalheRemessaRegistroE> RegistrosDetalheE { get; set; }
        public TrailerRemessaDebitoAutomatico Trailer { get; set; }
    }
}
