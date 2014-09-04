using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;
using BoletoBr.Interfaces;

namespace BoletoBr.Dominio.CodigoRejeicao
{
    public class CodigoRejeicao : AbstractCodigoRejeicao, ICodigoRejeicao
    {

        #region Variaveis

        private readonly ICodigoRejeicao _iCodigoRejeicao = null;

        #endregion

        #region Propriedades da interface

        public override IBanco Banco
        {
            get { return _iCodigoRejeicao.Banco; }
        }

        public override int Codigo
        {
            get { return _iCodigoRejeicao.Codigo; }
        }

        public override string Descricao
        {
            get { return _iCodigoRejeicao.Descricao; }
        }

        #endregion

    }
}
