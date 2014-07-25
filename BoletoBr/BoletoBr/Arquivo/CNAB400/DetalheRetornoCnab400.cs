using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public class DetalheRetornoCnab400
    {
        public int CodigoDoRegistro { get; set; }
        public int CodigoDeInscricao { get; set; }
        public int CodigoDoBeneficiario { get; set; }
        public int CodigoAgenciaCedente { get; set; }
        public int SubConta { get; set; }
        public int ContaCorrente { get; set; }
        public int CodigoDoDocumentoEmpresa { get; set; }
        public int CodigoDePostagem { get; set; }
        public int CodigoDoDocumentoBanco { get; set; }
        public int DataDeCredito { get; set; }
        public int Moeda { get; set; }
        public int Carteira { get; set; }
        public int CodigoDeOcorrencia { get; set; }
        public int DataDaOcorrencia { get; set; }
        /// <summary>
        /// Número da parcela e total de parcelas, sendo 3 dítidos para cada campo.
        /// PPP/TTT
        /// </summary>
        public int SeuNumero { get; set; }
        public int MotivoDaOcorrencia { get; set; }
        public int DataDeVencimento { get; set; }
        public decimal ValorDoTituloParcela { get; set; }
        public int BancoCobrador { get; set; }
        public int AgenciaCobradora { get; set; }
        public int Especie { get; set; }
        public decimal Iof { get; set; }
        public decimal Desconto { get; set; }
        public decimal ValorPago { get; set; }
        public decimal JurosDeMora { get; set; }
        public int Constante { get; set; }
        public decimal QuantidadeMoeda { get; set; }
        public decimal CotacaoMoeda { get; set; }
        public int StatusDaParcela { get; set; }
        public int IdentificadorLancamentoConta { get; set; }
        public int TipoLiquidacao { get; set; }
        public int OrigemDaTarifa { get; set; }
        public int NumeroSequencial { get; set; }

        private readonly List<string> _linhasArquivo;

        public DetalheRetornoCnab400(List<string> linhasArquivo)
        {
            _linhasArquivo = linhasArquivo;
        }

        #region Banco HSBC

        /// <summary>
        /// Faz a leitura do registro Detalhe para arquivos do formato CNAB 400
        /// Bancos:
        /// HSBC
        /// </summary>
        /// <param name="registro"></param>
        public void LerDetalheRetornoCnab400(string registro)
        {   
            try
            {
                var contador = 1;

                var objRetornar = new List<DetalheRetornoCnab400>();
                /* Obter 1ª linha */
                var linha = _linhasArquivo[0];

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
            }
            catch (Exception)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.");
            }
        }

        public static string PrimeiroCaracter(string retorno)
        {
            try
            {
                return retorno.Substring(0, 1);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao desmembrar registro.", ex);
            }
        }

        #endregion
    }
}
