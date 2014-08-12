using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Cef
{
    public class EscritorRemessaCnab240CefSicgb : IEscritorArquivoRemessa
    {
        private readonly string _sequencial;

        public EscritorRemessaCnab240CefSicgb(string sequencial)
        {
            _sequencial = sequencial;
        }

        public List<string> EscreverArquivo(List<Boleto> boletosEscrever)
        {
            throw new NotImplementedException();
        }
    }
}
