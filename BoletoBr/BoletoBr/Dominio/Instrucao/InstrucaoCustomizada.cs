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
        public InstrucaoCustomizada(string mensagemInstrucao)
        {
            this.TextoInstrucao = mensagemInstrucao;
        }

        public InstrucaoCustomizada(int codigo, string mensagem, int qtdDias)
        {
            this.Codigo = codigo;
            this.TextoInstrucao = mensagem;
            this.QtdDias = qtdDias;
        }

        public InstrucaoCustomizada(int codigo, string mensagem, double valor)
        {
            this.Codigo = codigo;
            this.TextoInstrucao = mensagem;
            this.Valor = valor;
        }
        public int Codigo { get; set; }
        public int QtdDias { get; set; }
        public double Valor { get; set; }
        public string TextoInstrucao { get; private set; }
    }
}
