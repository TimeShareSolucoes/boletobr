using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.EspecieDocumento
{
    public abstract class AbstractEspecieDocumento : IEspecieDocumento
    {

        public virtual IBanco Banco { get; set; }

        public virtual string Codigo { get; set; }

        public virtual string Sigla { get; set; }

        public virtual string Especie { get; set; }

    }
}
