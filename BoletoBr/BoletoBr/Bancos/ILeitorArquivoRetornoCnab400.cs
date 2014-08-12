using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.CNAB400.Retorno;
using BoletoBr.Dominio;

namespace BoletoBr.Bancos
{
    interface ILeitorArquivoRetornoCnab400
    {
        RetornoCnab400 ProcessarRetorno(TipoArquivo tipoArquivo);
        void ValidaArquivoRetorno();
        HeaderRetornoCnab400 ObterHeader(string linha);
        DetalheRetornoCnab400 ObterRegistrosDetalhe(string linha);
        TrailerRetornoCnab400 ObterTrailer(string linha);
    }
}
