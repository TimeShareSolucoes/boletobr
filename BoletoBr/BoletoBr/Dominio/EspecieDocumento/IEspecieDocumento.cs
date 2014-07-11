using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.EspecieDocumento
{
    public interface IEspecieDocumento
    {
        IBanco Banco { get; set; }
        string Codigo { get; set; }
        string Sigla { get; set; }
        string Especie { get; set; }
    }
}
