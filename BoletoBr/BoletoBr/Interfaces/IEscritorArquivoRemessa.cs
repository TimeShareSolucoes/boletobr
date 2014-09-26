using System.Collections.Generic;

namespace BoletoBr.Interfaces
{
    interface IEscritorArquivoRemessa
    {
        List<string> EscreverArquivo(List<Boleto> boletosEscrever);
        void ValidarArquivoRemessa(Cedente cedente, List<Boleto> boletos, int numeroArquivoRemessa);
    }
}
