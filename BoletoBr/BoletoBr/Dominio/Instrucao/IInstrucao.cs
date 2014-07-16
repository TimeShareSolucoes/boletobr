using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.Instrucao
{
    public interface IInstrucao
    {
        IBanco Banco { get; set; }
        int Codigo { get; set; }
        int QtdDias { get; set; }
        string TextoInstrucao { get; }
    }
}
