using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public class ValidacaoBoletoException : Exception
    {
        public ValidacaoBoletoException() { }

        public ValidacaoBoletoException(string mensagem) : base(mensagem) { }

        public ValidacaoBoletoException(string mensagem, Exception inner) : base(mensagem, inner) { }
    }
}
