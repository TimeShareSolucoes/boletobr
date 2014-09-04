using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;
using BoletoBr.Dominio;
using BoletoBr.Interfaces;

namespace BoletoBr.Arquivo
{
    public interface IArquivoRemessa
    {
        /// <summary>
        /// Método que fará a verificação se a classe está devidamente implementada para a geração da Remessa
        /// </summary>
        bool ValidarArquivoRemessa(string numeroConvenio, IBanco banco, Cedente cedente, List<Boleto> boletos, int numeroArquivoRemessa, out string mensagem);

        /// <summary>
        /// Gera arquivo de remessa
        /// </summary>
        void GerarArquivoRemessa(string numeroConvenio, IBanco banco, Cedente cedente, List<Boleto> boletos, Stream arquivo, int numeroArquivoRemessa);

        List<Boleto> Boletos { get; }
        Cedente Cedente { get; }
        IBanco Banco { get; }
        string NumeroConvenio { get; set; }
        int NumeroArquivoRemessa { get; set; }
        TipoArquivo TipoArquivo { get; }
    }
}
