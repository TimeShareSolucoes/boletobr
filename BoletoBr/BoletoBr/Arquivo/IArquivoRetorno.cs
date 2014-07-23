using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio
{
    public interface IArquivoRetorno
    {

        /// <summary>
        /// Ler arquivo de Retorno
        /// </summary>
        void LerArquivoRetorno(IBanco banco, Stream arquivo);

        IBanco Banco { get; }
        TipoArquivo TipoArquivo { get; }

        event EventHandler<LinhaDeArquivoLidaArgs> LinhaDeArquivoLida;
    }
}
