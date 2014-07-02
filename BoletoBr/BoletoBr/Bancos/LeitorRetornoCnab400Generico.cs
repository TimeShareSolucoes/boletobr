using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos
{
    public class LeitorRetornoCnab400Generico : ILeitorArquivoRetornoCnab400
    {
        private readonly List<string> _linhasArquivo;

        public LeitorRetornoCnab400Generico(List<string> linhasArquivo)
        {
            _linhasArquivo = linhasArquivo;
        }

        public RetornoCnab400 ProcessarRetorno()
        {
            /* Validações */
            #region Validações
            if (_linhasArquivo == null)
                throw new Exception("Dados do arquivo de retorno estão nulos. Impossível processar.");

            if (_linhasArquivo.Count <= 0)
                throw new Exception("Dados do arquivo de retorno estão nulos. Impossível processar.");

            if (_linhasArquivo.Count < 3)
                throw new Exception("Dados do arquivo de retorno não contém o mínimo de 3 linhas. Impossível processar.");
            #endregion

            var objRetornar = new RetornoCnab400();

            /* Processar Header */
            objRetornar.Header = ObterHeader();

            return objRetornar;
        }

        public HeaderRetornoCnab400 ObterHeader()
        {
            var objRetornar = new HeaderRetornoCnab400();
            /* Obter 1ª linha */
            var linha = _linhasArquivo[0];

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).ToInt();
            objRetornar.CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).ToInt();

            return objRetornar;
        }

        public List<DetalheRetornoCnab400> ObterRegistrosDetalhe()
        {
            return null;
        }

        public TrailerRetornoCnab400 ObterTrailer()
        {
            return null;
        }
    }
}
