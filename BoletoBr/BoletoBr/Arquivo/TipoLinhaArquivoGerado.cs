using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Arquivo
{
    public enum TipoLinhaGerada
    {
        HeaderDeArquivo = 1,
        HeaderDeLote = 10,
        DetalheSegmentoQ = 30,
        DetalheSegmentoR = 50,
        DetalheSegmentoP = 70,
        TraillerDeLote = 90,
        TraillerDeArquivo = 99
    }

}
