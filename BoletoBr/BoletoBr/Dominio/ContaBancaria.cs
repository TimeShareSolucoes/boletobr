using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public class ContaBancaria
    {
        #region Propriedades
        public string Agencia { get; set; } 
        public string DigitoAgencia { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public string OperacaoConta { get; set; }
        #endregion

        #region Construtores

        protected ContaBancaria()
        {
            this.Agencia = String.Empty;
            this.DigitoAgencia = String.Empty;
            this.Conta = String.Empty;
            this.DigitoConta = String.Empty;
            this.OperacaoConta = String.Empty;
        }
        public ContaBancaria(string agencia, string conta)
        {
            this.Agencia = agencia;
            this.Conta = conta;
        }

        public ContaBancaria(string agencia, string digitoAgencia, string conta, string digitoConta)
        {
            this.Agencia = agencia;
            this.DigitoAgencia = digitoAgencia;
            this.Conta = conta;
            this.DigitoConta = digitoConta;
        }
        public ContaBancaria(string agencia, string digitoAgencia, string conta, string digitoConta, string operacaoConta)
        {
            this.Agencia = agencia;
            this.DigitoAgencia = digitoAgencia;
            this.Conta = conta;
            this.DigitoConta = digitoConta;
            this.OperacaoConta = operacaoConta;
        }

        #endregion
    }
}
