using System.Collections.Generic;
using BoletoBr.Arquivo.CNAB240.Remessa;

namespace BoletoBr.Interfaces
{
    public interface IEscritorArquivoRemessaCnab240
    {
        List<string> EscreverArquivo(List<Boleto> boletosEscrever);
        void ValidarArquivoRemessa(Cedente cedente, List<Boleto> boletos, int numeroArquivoRemessa);
        RemessaCnab240 ProcessarRemessaCnab240();
        void ValidaArquivoRemessa();
        HeaderRemessaCnab240 EscreverHeader(string linha);
        HeaderLoteRemessaCnab240 EscreverHeaderLote(string linha);
        DetalheSegmentoPRemessaCnab240 EscreverDetalheSegmentoP(string linha);
        DetalheSegmentoQRemessaCnab240 EscreverDetalheSegmentoQ(string linha);
        TrailerLoteRemessaCnab240 EscreverTrailerLote(string linha);
        TrailerRemessaCnab240 EscreverTrailer(string linha);
    }
}
