using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;
using BoletoBr.Interfaces;

namespace BoletoBr.Dominio.CNAB400
{
    public class ArquivoRetornoCNAB400 : IArquivoRetorno
    {
        public IBanco Banco { get; private set; }
        public TipoArquivo TipoArquivo { get; private set; }

        public List<DetalheRetornoGenericoCnab400> ListaDetalhe { get; set; }

        #region Construtores

        public ArquivoRetornoCNAB400()
        {
            ListaDetalhe = new List<DetalheRetornoGenericoCnab400>();
            this.TipoArquivo = TipoArquivo.Cnab400;
        }

        #endregion
    }
}
