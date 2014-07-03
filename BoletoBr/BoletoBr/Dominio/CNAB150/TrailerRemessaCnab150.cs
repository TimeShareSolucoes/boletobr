using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public class TrailerRemessaCnab150
    {
        public string CodigoDoRegistro { get; set; }
        public int TotalDeRegistros { get; set; }
        public int ValorTotalDosRegistros { get; set; }
        public string Filler { get; set; }
        public int Zeros { get; set; }
    }
}
