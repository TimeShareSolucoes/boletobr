using BoletoBr.Arquivo.DebitoAutomatico.Remessa; 
using System.Collections.Generic; 

namespace BoletoBr.Interfaces
{
   public interface IEscritorArquivoRemessaDebitoAutomatico
    {
        List<string> EscreverTexto(RemessaDebitoAutomatico remessaEscrever); 
    }
}
