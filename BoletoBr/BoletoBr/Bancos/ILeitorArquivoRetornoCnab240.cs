using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.CNAB240;
using BoletoBr.Dominio;

namespace BoletoBr.Bancos
{
    interface ILeitorArquivoRetornoCnab240
    {
        RetornoCnab240 ProcessarRetorno();
        void ValidaArquivoRetorno();
        HeaderRetornoCnab240 ObterHeader(string linha);
        HeaderLoteRetornoCnab240 ObterHeaderLote(string linha);
        DetalheSegmentoTRetornoCnab240 ObterRegistrosDetalheT(string linha);
        DetalheSegmentoURetornoCnab240 ObterRegistrosDetalheU(string linha);
        TrailerLoteRetornoCnab240 ObterTrailerLote(string linha);
        TrailerRetornoCnab240 ObterTrailer(string linha);
    }
}
