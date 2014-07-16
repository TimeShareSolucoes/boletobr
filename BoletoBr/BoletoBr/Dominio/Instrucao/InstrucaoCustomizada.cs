using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.Instrucao
{
    public class InstrucaoCustomizada : IInstrucao
    {
        public InstrucaoCustomizada(IBanco banco, string instrucao)
        {
            this.Banco = banco;
            this.TextoInstrucao = instrucao;
        }
        public IBanco Banco { get; set; }
        public int Codigo { get; set; }
        public int QtdDias { get; set; }
        public string TextoInstrucao { get; private set; }
    }
}
