using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.CNAB400.Retorno;
using BoletoBr.Dominio;

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
            objRetornar.RegistrosDetalhe = ObterRegistrosDetalhe();
            objRetornar.Trailer = ObterTrailer();

            return objRetornar;
        }

        public RetornoCnab400 ProcessarRetorno(TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
        }

        public void ValidaArquivoRetorno()
        {
            throw new NotImplementedException();
        }

        public HeaderRetornoCnab400 ObterHeader(string valor)
        {
            throw new NotImplementedException();
        }

        public DetalheRetornoCnab400 ObterRegistrosDetalhe(string valor)
        {
            throw new NotImplementedException();
        }

        public DetalheRateioRetornoCnab400 ObterRegistrosDetalheRateio(string linha)
        {
            throw new NotImplementedException();
        }

        public TrailerRetornoCnab400 ObterTrailer(string valor)
        {
            throw new NotImplementedException();
        }

        public HeaderRetornoCnab400 ObterHeader()
        {
            return null;
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
