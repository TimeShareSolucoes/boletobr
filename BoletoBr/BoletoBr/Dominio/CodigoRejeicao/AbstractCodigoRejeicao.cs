using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.CodigoRejeicao
{
    public abstract class AbstractCodigoRejeicao : ICodigoRejeicao
    {

        #region Variaveis

        #endregion

        # region Propriedades

        public virtual IBanco Banco { get; set; }

        public virtual int Codigo { get; set; }

        public virtual string Descricao { get; set; }

        # endregion

    }
}
