using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;
using BoletoBr.Interfaces;

namespace BoletoBr.Dominio.CodigoLiquidacao
{
    public class CodigoLiquidacao : AbstractCodigoLiquidacao, ICodigoLiquidacao
    {

        #region Variaveis

        private readonly ICodigoLiquidacao _iCodigoLiquidacao = null;

        #endregion

        #region Propriedades da interface

        public override IBanco Banco
        {
            get
            {
                return _iCodigoLiquidacao.Banco;
            }
        }

        public override int Enumerado
        {
            get
            {
                return _iCodigoLiquidacao.Enumerado;
            }
        }

        public override string Codigo
        {
            get
            {
                return _iCodigoLiquidacao.Codigo;
            }
        }

        public override string Descricao
        {
            get
            {
                return _iCodigoLiquidacao.Descricao;
            }
        }

        public override string Recurso
        {
            get
            {
                return _iCodigoLiquidacao.Recurso;
            }
        }

        #endregion

    }
}
