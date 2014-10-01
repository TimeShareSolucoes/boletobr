using System.Collections.Generic;
using BoletoBr.Arquivo.CNAB240.Remessa;

namespace BoletoBr.Interfaces
{
    public interface IEscritorArquivoRemessaCnab240
    {
        List<string> EscreverTexto(RemessaCnab240 remessaEscrever);
        void ValidarRemessa(RemessaCnab240 remessaValidar);
    }
}
