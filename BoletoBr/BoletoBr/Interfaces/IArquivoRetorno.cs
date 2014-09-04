using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;
using BoletoBr.Interfaces;

namespace BoletoBr.Dominio
{
    public interface IArquivoRetorno
    {
        IBanco Banco { get; }
        TipoArquivo TipoArquivo { get; }
    }
}
