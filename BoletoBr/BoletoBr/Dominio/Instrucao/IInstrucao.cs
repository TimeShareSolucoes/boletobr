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
        int Codigo { get; set; }
        int QtdDias { get; set; }
        double Valor { get; set; }
        string TextoInstrucao { get; }
    }
}
