using System.Collections.Generic;
using BoletoBr.Arquivo.CNAB400.Remessa;

namespace BoletoBr.Interfaces
{
    public interface IEscritorArquivoRemessaCnab400
    {
        List<string> EscreverArquivo(List<Boleto> boletosEscrever);
        void ValidarArquivoRemessa(Cedente cedente, List<Boleto> boletos, int numeroArquivoRemessa);
        RemessaCnab400 ProcessarRemessaCnab400();
        void ValidaArquivoRemessa();
        HeaderRemessaCnab400 EscreverHeader(string linha);
        DetalheRemessaCnab400 EscreverDetalhe(string linha);
        TrailerRemessaCnab400 EscreverTrailer(string linha);
    }
}
