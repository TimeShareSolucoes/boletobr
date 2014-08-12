using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos
{
    interface IEscritorArquivoRemessa
    {
        List<string> EscreverArquivo(List<Boleto> boletosEscrever);
    }
}
