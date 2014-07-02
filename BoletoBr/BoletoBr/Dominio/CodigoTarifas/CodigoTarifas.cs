using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.CodigoTarifas
{
    public class CodigoTarifas : AbstractCodigoTarifas, ICodigoTarifas
    {

        #region Variaveis

        private readonly ICodigoTarifas _iCodigoTarifas = null;

        #endregion

        #region Propriedades da interface

        public override IBanco Banco
        {
            get { return _iCodigoTarifas.Banco; }
        }

        public override int Codigo
        {
            get { return _iCodigoTarifas.Codigo; }
        }

        public override string Descricao
        {
            get { return _iCodigoTarifas.Descricao; }
        }

        #endregion

    }
}
