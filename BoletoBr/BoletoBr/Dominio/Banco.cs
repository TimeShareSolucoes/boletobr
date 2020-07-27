using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Dominio
{
    public class Banco
    {
        public int IdBanco { get; set; }

        private string _codBanco;
        public virtual string CodBanco { get => _codBanco; set => _codBanco = value.PadLeft(3, '0'); }
        public string NomeBanco { get; set; }
        public string NomeEmpresaNoBanco { get; set; }
         
        public int NumeroSequencialNsa { get; set; }

        public string CodigoConvenio { get; set; }
    }
}
