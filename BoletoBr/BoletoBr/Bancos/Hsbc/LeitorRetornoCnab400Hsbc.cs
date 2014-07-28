using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
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
            /* Obter 1ª linha */
            var linha = linhaObterInformacoes;

            if (linha.ExtrairValorDaLinha(1, 1) != "0")
                throw new Exception("Linha informada não possui características do  Registro Header do arquivo");

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
            objRetornar.DataGravacao = Convert.ToInt32(linha.ExtrairValorDaLinha(95, 100));
            objRetornar.Densidade = linha.ExtrairValorDaLinha(101, 105);
            objRetornar.LiteralDensidade = linha.ExtrairValorDaLinha(106, 108);
            objRetornar.CodigoDoBeneficiario = linha.ExtrairValorDaLinha(109, 118).ToInt();
            objRetornar.NomeAgencia = linha.ExtrairValorDaLinha(119, 138);
            objRetornar.CodigoFormulario = linha.ExtrairValorDaLinha(139, 142).ToInt();
            // Posição 143 - 388 brancos
            objRetornar.Volser = linha.ExtrairValorDaLinha(389, 394);
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400);

            return objRetornar;
        }

        public DetalheRetornoCnab400 ObterRegistrosDetalhe(string linhaProcessar)
        {
            var objRetornar = new DetalheRetornoCnab400();
            /* Obter 2ª linha */
            var linha = linhaProcessar;

            objRetornar.CodigoDoRegistro = MetodosExtensao.ExtrairValorDaLinha(linha, 1, 1).ToInt();
            objRetornar.CodigoDeInscricao = MetodosExtensao.ExtrairValorDaLinha(linha, 2, 3).ToInt();
            objRetornar.CodigoDoBeneficiario = MetodosExtensao.ExtrairValorDaLinha(linha, 4, 17).ToInt();
            objRetornar.CodigoAgenciaCedente = MetodosExtensao.ExtrairValorDaLinha(linha, 18, 22).ToInt();
            objRetornar.SubConta = MetodosExtensao.ExtrairValorDaLinha(linha, 23, 24).ToInt();
            objRetornar.ContaCorrente = MetodosExtensao.ExtrairValorDaLinha(linha, 25, 35).ToInt();
            // Posição 36-37 brancos
            objRetornar.CodigoDoDocumentoEmpresa = MetodosExtensao.ExtrairValorDaLinha(linha, 38, 53).ToInt();
            // Posição 54 branco
            objRetornar.CodigoDePostagem = MetodosExtensao.ExtrairValorDaLinha(linha, 55, 55).ToInt();
            // Posição 56-62 brancos
            objRetornar.CodigoDoDocumentoBanco = MetodosExtensao.ExtrairValorDaLinha(linha, 63, 78).ToInt();
            // Posição 79-82 brancos
            objRetornar.DataDeCredito = MetodosExtensao.ExtrairValorDaLinha(linha, 83, 88).ToInt();
            objRetornar.Moeda = MetodosExtensao.ExtrairValorDaLinha(linha, 89, 89).ToInt();
            // Posição 90-107 brancos
            objRetornar.Carteira = MetodosExtensao.ExtrairValorDaLinha(linha, 108, 108).ToInt();
            objRetornar.CodigoDeOcorrencia = MetodosExtensao.ExtrairValorDaLinha(linha, 109, 110).ToInt();
            objRetornar.DataDaOcorrencia = MetodosExtensao.ExtrairValorDaLinha(linha, 111, 116).ToInt();
            objRetornar.SeuNumero = MetodosExtensao.ExtrairValorDaLinha(linha, 117, 122).ToInt();
            objRetornar.MotivoDaOcorrencia = MetodosExtensao.ExtrairValorDaLinha(linha, 123, 131).ToInt();
            // Posição 132-146 brancos
            objRetornar.DataDeVencimento = MetodosExtensao.ExtrairValorDaLinha(linha, 147, 152).ToInt();
            objRetornar.ValorDoTituloParcela = MetodosExtensao.ExtrairValorDaLinha(linha, 153, 165).ToDecimal();
            objRetornar.BancoCobrador = MetodosExtensao.ExtrairValorDaLinha(linha, 166, 168).ToInt();
            objRetornar.AgenciaCobradora = MetodosExtensao.ExtrairValorDaLinha(linha, 169, 173).ToInt();
            objRetornar.Especie = MetodosExtensao.ExtrairValorDaLinha(linha, 174, 175).ToInt();
            objRetornar.Iof = MetodosExtensao.ExtrairValorDaLinha(linha, 176, 186).ToDecimal();
            // Posição 187-240 brancos
            objRetornar.Desconto = MetodosExtensao.ExtrairValorDaLinha(linha, 241, 253).ToDecimal();
            objRetornar.ValorPago = MetodosExtensao.ExtrairValorDaLinha(linha, 254, 266).ToDecimal();
            objRetornar.JurosDeMora = MetodosExtensao.ExtrairValorDaLinha(linha, 267, 279).ToDecimal();
            objRetornar.Constante = MetodosExtensao.ExtrairValorDaLinha(linha, 280, 280).ToInt();
            objRetornar.QuantidadeMoeda = MetodosExtensao.ExtrairValorDaLinha(linha, 281, 293).ToInt();
            objRetornar.CotacaoMoeda = MetodosExtensao.ExtrairValorDaLinha(linha, 294, 308).ToDecimal();
            objRetornar.StatusDaParcela = MetodosExtensao.ExtrairValorDaLinha(linha, 309, 309).ToInt();
            objRetornar.IdentificadorLancamentoConta = MetodosExtensao.ExtrairValorDaLinha(linha, 310, 315).ToInt();
            // Posição 316-341 brancos
            objRetornar.TipoLiquidacao = MetodosExtensao.ExtrairValorDaLinha(linha, 342, 342).ToInt();
            objRetornar.OrigemDaTarifa = MetodosExtensao.ExtrairValorDaLinha(linha, 343, 343).ToInt();
            // Posição 344-394 brancos
            objRetornar.NumeroSequencial = MetodosExtensao.ExtrairValorDaLinha(linha, 395, 400).ToInt();
            
            return objRetornar;
        }

        public TrailerRetornoCnab400 ObterTrailer(string linhaObterInformacoes)
        {
            var objRetornar = new TrailerRetornoCnab400();
            /* Obter última linha */
            var linha = linhaObterInformacoes;

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).ToInt();
            objRetornar.CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).ToInt();
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(3, 4);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(5, 7);
            // Brancos
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).ToInt();
            
            return objRetornar;
        }

        public static string PrimeiroCaracter(string retorno)
        {
            try
            {
                return retorno.ExtrairValorDaLinha(1, 1);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao desmembrar registro.", ex);
            }
        }
    }
}
