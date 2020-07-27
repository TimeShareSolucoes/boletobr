using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Arquivo.DebitoAutomatico.Retorno
{
    public class RetornoDebitoAutomatico
    {
        public RetornoDebitoAutomatico()
        {
            this.DetalheRetornoRegistroB = new List<DetalheRetornoRegistroB>();
            this.DetalheRetornoRegistroF = new List<DetalheRetornoRegistroF>();
            this.DetalheRetornoRegistroH = new List<DetalheRetornoRegistroH>();
            this.DetalheRetornoRegistroJ = new List<DetalheRetornoRegistroJ>();
            this.DetalheRetornoRegistroT = new List<DetalheRetornoRegistroT>();
            this.DetalheRetornoRegistroX = new List<DetalheRetornoRegistroX>();
        }

        public HeaderRetornoDebitoAutomatico Header { get; set; }
        public List<DetalheRetornoRegistroB> DetalheRetornoRegistroB { get; set; }
        public List<DetalheRetornoRegistroF> DetalheRetornoRegistroF { get; set; }
        public List<DetalheRetornoRegistroH> DetalheRetornoRegistroH { get; set; }
        public List<DetalheRetornoRegistroJ> DetalheRetornoRegistroJ { get; set; }
        public List<DetalheRetornoRegistroT> DetalheRetornoRegistroT { get; set; }
        public List<DetalheRetornoRegistroX> DetalheRetornoRegistroX { get; set; }
        public TrailerRetornoDebitoAutomatico Trailer { get; set; }
    }
}
