using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos
{
    public class LeitorRetornoCnab400Hsbc : ILeitorArquivoRetornoCnab400
    {
        private readonly List<string> _linhasArquivo;

        public LeitorRetornoCnab400Hsbc(List<string> linhasArquivo)
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

        public HeaderRetornoCnab400 ObterHeader()
        {
            var objRetornar = new HeaderRetornoCnab400();
            /* Obter 1ª linha */
            var linha = _linhasArquivo[0];

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).ToInt();
            objRetornar.CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).ToInt();
            objRetornar.LiteralRetorno = linha.ExtrairValorDaLinha(3, 9);
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(10, 11).ToInt();
            objRetornar.LiteralServico = linha.ExtrairValorDaLinha(12, 26);
            objRetornar.CodigoAgenciaCedente = linha.ExtrairValorDaLinha(27, 31).ToInt();
            objRetornar.Constante = linha.ExtrairValorDaLinha(32, 33);
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(34, 44).ToInt();
            objRetornar.TipoRetorno = linha.ExtrairValorDaLinha(45, 45);
            // Brancos
            objRetornar.NomeDoBeneficiario = linha.ExtrairValorDaLinha(47, 76);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(77, 79).ToInt();
            objRetornar.NomeDoBanco = linha.ExtrairValorDaLinha(80, 94);
            objRetornar.DataGravacao = Convert.ToDateTime(linha.ExtrairValorDaLinha(95, 100));
            objRetornar.Densidade = linha.ExtrairValorDaLinha(101, 105);
            objRetornar.LiteralDensidade = linha.ExtrairValorDaLinha(106, 108);
            objRetornar.CodigoDoBeneficiario = linha.ExtrairValorDaLinha(109, 118).ToInt();
            objRetornar.NomeAgencia = linha.ExtrairValorDaLinha(119, 138);
            objRetornar.CodigoFormulario = linha.ExtrairValorDaLinha(139, 142).ToInt();
            // Brancos
            objRetornar.Volser = linha.ExtrairValorDaLinha(389, 394);
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400);

            return objRetornar;
        }

        public List<DetalheRetornoCnab400> ObterRegistrosDetalhe()
        {
            var objRetornar = new List<DetalheRetornoCnab400>();

            var linha = _linhasArquivo[0];

            return null;
        }

        public TrailerRetornoCnab400 ObterTrailer()
        {
            var objRetornar = new TrailerRetornoCnab400();
            /* Obter 1ª linha */
            var linha = _linhasArquivo[0];

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).ToInt();
            objRetornar.CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).ToInt();
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(3, 4).ToInt();
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(5, 7).ToInt();
            // Brancos
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).ToInt();
            
            return objRetornar;
        }
    }
}
