using BoletoBr.Dominio;

namespace BoletoBr.Interfaces
{
    public interface IArquivoRetorno
    {
        IBanco Banco { get; }
        TipoArquivo TipoArquivo { get; }
    }
}
