using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;
using BoletoBr.Interfaces;

namespace BoletoBr.Dominio.CodigoTarifas
{
    public abstract class AbstractCodigoTarifas : ICodigoTarifas
    {
        # region Propriedades

        public virtual IBanco Banco { get; set; }

        public virtual int Codigo { get; set; }

        public virtual string Descricao { get; set; }

        # endregion
    }
}
