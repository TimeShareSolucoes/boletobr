using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Interfaces
{
    public interface IEspecieDocumento
    {
        int Codigo { get; set; }
        string Descricao { get; set; }
        string Sigla { get; set; }
    }
}
