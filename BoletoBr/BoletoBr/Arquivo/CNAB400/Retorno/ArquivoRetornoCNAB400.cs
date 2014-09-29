using System.Collections.Generic;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Dominio;
using BoletoBr.Interfaces;

namespace BoletoBr.Arquivo.CNAB400.Retorno
{
    public class ArquivoRetornoCnab400 : IArquivoRetorno
    {
        public IBanco Banco { get; set; }
        public TipoArquivo TipoArquivo { get; private set; }

        public List<RetornoDetalheGenerico> ListaDetalhe { get; set; }

        #region Construtores

        public ArquivoRetornoCnab400()
        {
            ListaDetalhe = new List<RetornoDetalheGenerico>();
            TipoArquivo = TipoArquivo.Cnab400;
        }

        #endregion
    }
}
