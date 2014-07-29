using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos
{
    interface ILeitorArquivoRetornoCnab400
    {
        RetornoCnab400 ProcessarRetorno();
        HeaderRetornoCnab400 ObterHeader(string linha);
        DetalheRetornoCnab400 ObterRegistrosDetalhe(string linha);
        TrailerRetornoCnab400 ObterTrailer(string linha);
    }
}
