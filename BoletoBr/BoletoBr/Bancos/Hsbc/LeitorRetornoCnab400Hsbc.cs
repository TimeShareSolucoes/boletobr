using System;
using System.Collections.Generic;
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
            if (_linhasArquivo == null)
                throw new Exception("Dados do arquivo de retorno estão nulos. Impossível processar.");

            if (_linhasArquivo.Count <= 0)
                throw new Exception("Dados do arquivo de retorno não estão corretos. Impossível processar.");

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

        public List<DetalheRetornoCnab400> ObterRegistrosDetalhe()
        {
            var contador = 1;

            var objRetornar = new List<DetalheRetornoCnab400>();
            /* Obter 2ª linha */
            var linha = _linhasArquivo[1];

            if (linha.ExtrairValorDaLinha(1, 1) != "1")
                throw new Exception("Linha informada não possui características do  Registro Detalhe do arquivo");

            foreach (var infoLinha in objRetornar)
            {
                infoLinha.CodigoDoRegistro = MetodosExtensao.ExtrairValorDaLinha(linha, 1, 1).ToInt();
                infoLinha.CodigoDeInscricao = MetodosExtensao.ExtrairValorDaLinha(linha, 2, 3).ToInt();
                infoLinha.CodigoDoBeneficiario = MetodosExtensao.ExtrairValorDaLinha(linha, 4, 17).ToInt();
                infoLinha.CodigoAgenciaCedente = MetodosExtensao.ExtrairValorDaLinha(linha, 18, 22).ToInt();
                infoLinha.SubConta = MetodosExtensao.ExtrairValorDaLinha(linha, 23, 24).ToInt();
                infoLinha.ContaCorrente = MetodosExtensao.ExtrairValorDaLinha(linha, 25, 35).ToInt();
                // Posição 36-37 brancos
                infoLinha.CodigoDoDocumentoEmpresa = MetodosExtensao.ExtrairValorDaLinha(linha, 38, 53).ToInt();
                // Posição 54 branco
                infoLinha.CodigoDePostagem = MetodosExtensao.ExtrairValorDaLinha(linha, 55, 55).ToInt();
                // Posição 56-62 brancos
                infoLinha.CodigoDoDocumentoBanco = MetodosExtensao.ExtrairValorDaLinha(linha, 63, 78).ToInt();
                // Posição 79-82 brancos
                infoLinha.DataDeCredito = MetodosExtensao.ExtrairValorDaLinha(linha, 83, 88).ToInt();
                infoLinha.Moeda = MetodosExtensao.ExtrairValorDaLinha(linha, 89, 89).ToInt();
                // Posição 90-107 brancos
                infoLinha.Carteira = MetodosExtensao.ExtrairValorDaLinha(linha, 108, 108).ToInt();
                infoLinha.CodigoDeOcorrencia = MetodosExtensao.ExtrairValorDaLinha(linha, 109, 110).ToInt();
                infoLinha.DataDaOcorrencia = MetodosExtensao.ExtrairValorDaLinha(linha, 111, 116).ToInt();
                infoLinha.SeuNumero = MetodosExtensao.ExtrairValorDaLinha(linha, 117, 122).ToInt();
                infoLinha.MotivoDaOcorrencia = MetodosExtensao.ExtrairValorDaLinha(linha, 123, 131).ToInt();
                // Posição 132-146 brancos
                infoLinha.DataDeVencimento = MetodosExtensao.ExtrairValorDaLinha(linha, 147, 152).ToInt();
                infoLinha.ValorDoTituloParcela = MetodosExtensao.ExtrairValorDaLinha(linha, 153, 165).ToDecimal();
                infoLinha.BancoCobrador = MetodosExtensao.ExtrairValorDaLinha(linha, 166, 168).ToInt();
                infoLinha.AgenciaCobradora = MetodosExtensao.ExtrairValorDaLinha(linha, 169, 173).ToInt();
                infoLinha.Especie = MetodosExtensao.ExtrairValorDaLinha(linha, 174, 175).ToInt();
                infoLinha.Iof = MetodosExtensao.ExtrairValorDaLinha(linha, 176, 186).ToDecimal();
                // Posição 187-240 brancos
                infoLinha.Desconto = MetodosExtensao.ExtrairValorDaLinha(linha, 241, 253).ToInt();
                infoLinha.ValorPago = MetodosExtensao.ExtrairValorDaLinha(linha, 254, 266).ToInt();
                infoLinha.JurosDeMora = MetodosExtensao.ExtrairValorDaLinha(linha, 267, 279).ToInt();
                infoLinha.Constante = MetodosExtensao.ExtrairValorDaLinha(linha, 280, 280).ToInt();
                infoLinha.QuantidadeMoeda = MetodosExtensao.ExtrairValorDaLinha(linha, 281, 293).ToInt();
                infoLinha.CotacaoMoeda = MetodosExtensao.ExtrairValorDaLinha(linha, 294, 308).ToInt();
                infoLinha.StatusDaParcela = MetodosExtensao.ExtrairValorDaLinha(linha, 309, 309).ToInt();
                infoLinha.IdentificadorLancamentoConta = MetodosExtensao.ExtrairValorDaLinha(linha, 310, 315).ToInt();
                // Posição 316-341 brancos
                infoLinha.TipoLiquidacao = MetodosExtensao.ExtrairValorDaLinha(linha, 342, 342).ToInt();
                infoLinha.OrigemDaTarifa = MetodosExtensao.ExtrairValorDaLinha(linha, 343, 343).ToInt();
                // Posição 344-394 brancos
                infoLinha.NumeroSequencial = MetodosExtensao.ExtrairValorDaLinha(linha, 395, 400).ToInt();

                contador++;
            }

            return objRetornar;
        }

        public TrailerRetornoCnab400 ObterTrailer()
        {
            var objRetornar = new TrailerRetornoCnab400();
            /* Obter última linha */
            var linha = _linhasArquivo.LastOrDefault();

            if (linha.ExtrairValorDaLinha(1, 1) != "9")
                throw new Exception("Linha informada não possui características do  Registro Trailer do arquivo");

            objRetornar.CodigoDoRegistro = MetodosExtensao.ExtrairValorDaLinha(linha, 1, 1).ToInt();
            objRetornar.CodigoDeRetorno = MetodosExtensao.ExtrairValorDaLinha(linha, 2, 2).ToInt();
            objRetornar.CodigoDoServico = MetodosExtensao.ExtrairValorDaLinha(linha, 3, 4);
            objRetornar.CodigoDoBanco = MetodosExtensao.ExtrairValorDaLinha(linha, 5, 7);
            // Brancos
            objRetornar.NumeroSequencial = MetodosExtensao.ExtrairValorDaLinha(linha, 395, 400).ToInt();
            
            return objRetornar;
        }
    }
}
