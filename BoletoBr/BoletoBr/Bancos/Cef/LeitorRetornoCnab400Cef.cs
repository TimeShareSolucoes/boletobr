using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Cef
{
    public class LeitorRetornoCnab400Cef : ILeitorArquivoRetornoCnab400
    {
       private readonly List<string> _linhasArquivo;

       public LeitorRetornoCnab400Cef(List<string> linhasArquivo)
        {
            _linhasArquivo = linhasArquivo;
        }

        public RetornoCnab400 ProcessarRetorno()
        {
            /* Validações */
            #region Validações
            ValidaArquivoRetorno();
            #endregion

            var objRetornar = new RetornoCnab400();
            objRetornar.RegistrosDetalhe = new List<DetalheRetornoCnab400>();

            foreach (var linhaAtual in _linhasArquivo)
            {
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "0")
                {
                   objRetornar.Header = ObterHeader(linhaAtual);
                }
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "1")
                {
                    var objDetalhe = ObterRegistrosDetalhe(linhaAtual);
                    objRetornar.RegistrosDetalhe.Add(objDetalhe);
                }
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "9")
                {
                    objRetornar.Trailer = ObterTrailer(linhaAtual);
                }
            }

            return objRetornar;
        }

        void ValidaArquivoRetorno()
        {
            if (_linhasArquivo == null)
                throw new Exception("Dados do arquivo de retorno estão nulos. Impossível processar.");

            if (_linhasArquivo.Count <= 0)
                throw new Exception("Dados do arquivo de retorno não estão corretos. Impossível processar.");

            if (_linhasArquivo.Count < 3)
                throw new Exception("Dados do arquivo de retorno não contém o mínimo de 3 linhas. Impossível processar.");

            var qtdLinhasHeader =
                _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(1, 1) == "0");

            if (qtdLinhasHeader <= 0)
                throw new Exception("Não foi encontrado HEADER do arquivo de retorno.");

            if (qtdLinhasHeader > 1)
                throw new Exception("Não é permitido mais de um HEADER no arquivo de retorno.");

            var qtdLinhasDetalhe = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(1, 1) == "1");

            if (qtdLinhasDetalhe <= 0)
                throw new Exception("Não foi encontrado DETALHE do arquivo de retorno.");

            var qtdLinhasTrailer = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(1, 1) == "9");

            if (qtdLinhasTrailer <= 0)
                throw new Exception("Não foi encontrado TRAILER do arquivo de retorno.");

            if (qtdLinhasTrailer > 1)
                throw new Exception("Não é permitido mais de um TRAILER no arquivo de retorno.");
        }

        public HeaderRetornoCnab400 ObterHeader(string linhaObterInformacoes)
        {
            var objRetornar = new HeaderRetornoCnab400();

            var linha = linhaObterInformacoes;

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).ToInt();
            objRetornar.CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).ToInt();
            objRetornar.LiteralRetorno = linha.ExtrairValorDaLinha(3, 9);
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(10, 11);
            objRetornar.LiteralServico = linha.ExtrairValorDaLinha(12, 26);
            objRetornar.CodigoAgenciaCedente = linha.ExtrairValorDaLinha(27, 31).ToInt();
            objRetornar.Constante = linha.ExtrairValorDaLinha(32, 33);
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(34, 44).ToInt();
            objRetornar.TipoRetorno = linha.ExtrairValorDaLinha(45, 45);
            // Posição 46 branco
            objRetornar.NomeDoBeneficiario = linha.ExtrairValorDaLinha(47, 76);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(77, 79);
            objRetornar.NomeDoBanco = linha.ExtrairValorDaLinha(80, 94);
            objRetornar.DataGeracaoGravacao = Convert.ToInt32(linha.ExtrairValorDaLinha(95, 100));
            objRetornar.Densidade = linha.ExtrairValorDaLinha(101, 105);
            objRetornar.LiteralDensidade = linha.ExtrairValorDaLinha(106, 108);
            objRetornar.CodigoDoBeneficiario = linha.ExtrairValorDaLinha(109, 118).ToInt();
            objRetornar.NomeAgencia = linha.ExtrairValorDaLinha(119, 138);
            objRetornar.CodigoFormulario = linha.ExtrairValorDaLinha(139, 142).ToInt();
            // Posição 143 - 388 brancos
            objRetornar.Volser = linha.ExtrairValorDaLinha(389, 394);
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400);
        }

        public DetalheRetornoCnab400 ObterRegistrosDetalhe(string linhaProcessar)
        {
            throw new NotImplementedException();
        }

        public TrailerRetornoCnab400 ObterTrailer(string linhaObterInformacoes)
        {
            throw new NotImplementedException();
        }
    }
}
