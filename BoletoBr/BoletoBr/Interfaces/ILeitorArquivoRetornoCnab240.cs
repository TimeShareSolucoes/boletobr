using BoletoBr.Arquivo.CNAB240.Retorno;

namespace BoletoBr.Interfaces
{
    interface ILeitorArquivoRetornoCnab240
    {
        RetornoCnab240 ProcessarRetorno();
        void ValidaArquivoRetorno();
        HeaderRetornoCnab240 ObterHeader(string linha);
        HeaderLoteRetornoCnab240 ObterHeaderLote(string linha);
        DetalheSegmentoTRetornoCnab240 ObterRegistrosDetalheT(string linha);
        DetalheSegmentoURetornoCnab240 ObterRegistrosDetalheU(string linha);
        DetalheSegmentoERetornoCnab240 ObterRegistrosDetalheE(string linha);
        TrailerLoteRetornoCnab240 ObterTrailerLote(string linha);
        TrailerRetornoCnab240 ObterTrailer(string linha);
    }
}
