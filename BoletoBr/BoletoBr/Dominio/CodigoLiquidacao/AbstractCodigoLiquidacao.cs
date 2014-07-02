using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.CodigoLiquidacao
{
    public abstract class AbstractCodigoLiquidacao : ICodigoLiquidacao
    {
        public virtual IBanco Banco { get; set; }

        public virtual int Enumerado { get; set; }

        public virtual string Codigo { get; set; }

        public virtual string Descricao { get; set; }

        public virtual string Recurso { get; set; }
    }
}
