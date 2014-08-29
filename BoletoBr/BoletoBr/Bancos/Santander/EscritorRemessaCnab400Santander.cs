using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Santander
{
    public class EscritorRemessaCnab400Santander : IEscritorArquivoRemessa
    {
        public List<string> EscreverArquivo(List<Boleto> boletosEscrever)
        {
            throw new NotImplementedException();
        }

        public void ValidarArquivoRemessa()
        {
            throw new NotImplementedException();
        }

        public string EscreverHeader(Boleto boleto, int numeroRegistro)
        {
            return null;
        }

        public string EscreverDetalhe(Boleto boleto, int numeroRegistro)
        {
            return null;
        }

        public string EscreverTrailer(int numeroRegistro)
        {
            return null;
        }
    }
}
