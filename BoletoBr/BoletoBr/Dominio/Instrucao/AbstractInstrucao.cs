using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.Instrucao
{
    public abstract class AbstractInstrucao : IInstrucao
    {

        public virtual IBanco Banco { get; set; }

        public virtual int Codigo { get; set; }

        public virtual string Descricao { get; set; }

        public virtual int QuantidadeDias { get; set; }

        public virtual void Valida()
        {
            throw new NotImplementedException("Função não implementada");
        }
    }
}
