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
        HeaderRetornoCnab400 ObterHeader();
        List<DetalheRetornoCnab400> ObterRegistrosDetalhe();
        TrailerRetornoCnab400 ObterTrailer();
    }
}
