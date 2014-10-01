using System.Collections.Generic;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;

namespace BoletoBr.Interfaces
{
    public interface IEscritorArquivoRemessaCnab400
    {
        List<string> EscreverTexto(RemessaCnab400 remessaEscrever);
        void ValidarRemessa(RemessaCnab400 remessaValidar);
    }
}
