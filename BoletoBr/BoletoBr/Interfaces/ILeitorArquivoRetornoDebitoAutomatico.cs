using BoletoBr.Arquivo.DebitoAutomatico.Retorno;
using BoletoBr.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Interfaces
{
    interface ILeitorArquivoRetornoDebitoAutomatico
    {
        RetornoDebitoAutomatico ProcessarRetorno();
        void ValidaArquivoRetorno();
        TrailerRetornoDebitoAutomatico ObterTrailer(string linha);
        DetalheRetornoRegistroX ObterDetalheRegistroX(string linha);
        DetalheRetornoRegistroT ObterDetalheRegistroT(string linha);
        DetalheRetornoRegistroJ ObterDetalheRegistroJ(string linha);
        DetalheRetornoRegistroH ObterDetalheRegistroH(string linha);
        DetalheRetornoRegistroF ObterDetalheRegistroF(string linha);
        DetalheRetornoRegistroB ObterDetalheRegistroB(string linha);
        HeaderRetornoDebitoAutomatico ObterHeader(string linha);
    }
}
