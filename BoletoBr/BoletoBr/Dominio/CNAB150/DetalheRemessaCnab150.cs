using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public class DetalheRemessaCnab150
    {
        public string CodigoDoRegistro { get; set; }
        public string IdentificacaoEmpresa { get; set; }
        public int DataDoPagamento { get; set; }
        public int DataDoCredito { get; set; }
        public int CodigoDeBarras { get; set; }
        public decimal ValorRecebido { get; set; }
        public decimal ValorDaTarifa { get; set; }
        public string NumeroSequencial { get; set; }
        public string CodigoAgenciaArrecadadora { get; set; }
        public int FormaDeArrecadacao { get; set; }
        public string Filler { get; set; }
        public int Zeros { get; set; }
    }
}
