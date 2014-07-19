using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.Instrucao
{
    public class InstrucaoPadronizada : IInstrucao
    {
        public InstrucaoPadronizada(IBanco banco, int codigo, int qtdDias, string textoInstrucao)
        {
            this.Banco = banco;
            this.Codigo = codigo;
            this.QtdDias = qtdDias;
            this.TextoInstrucao = textoInstrucao;
        }
        public IBanco Banco { get; set; }
        public int Codigo { get; set; }
        public int QtdDias { get; set; }
        public string TextoInstrucao { get; private set; }
    }
}
