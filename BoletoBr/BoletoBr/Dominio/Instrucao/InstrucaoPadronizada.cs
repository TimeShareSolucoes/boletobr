using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;
using BoletoBr.Interfaces;

namespace BoletoBr.Dominio.Instrucao
{
    public class InstrucaoPadronizada : IInstrucao
    {
        public int Codigo { get; set; }
        public int QtdDias { get; set; }
        public double Valor { get; set; }
        public string TextoInstrucao { get; set; }
    }
}
