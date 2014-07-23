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
        public int CodigoDoDocumento { get; set; }
        public int CodigoDePostagem { get; set; }
        public DateTime DataDeCredito { get; set; }
        public int Moeda { get; set; }
        public int Carteira { get; set; }
        public int CodigoDeOcorrencia { get; set; }
        public DateTime DataDaOcorrencia { get; set; }

        /// <summary>
        /// Número da parcela e total de parcelas, sendo 3 dítidos para cada campo.
        /// PPP/TTT
        /// </summary>
        public int SeuNumero { get; set; }

        public int MotivoDaOcorrencia { get; set; }
        public DateTime DataDeVencimento { get; set; }
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

        #region Banco HSBC

        public void LerDetalheRetornoCnab400Hsbc(string registro)
        {
            try
            {
                int dataCredito = Convert.ToInt32(registro.Substring(83, 6));
                int dataOcorrencia = Convert.ToInt32(registro.Substring(111, 6));
                int dataVencimento = Convert.ToInt32(registro.Substring(147, 6));

                CodigoDoRegistro = Convert.ToInt32(registro.Substring(0, 1));
                CodigoDeInscricao = Convert.ToInt32(registro.Substring(2, 2));
                CodigoDoBeneficiario = Convert.ToInt32(registro.Substring(4, 14));
                CodigoAgenciaCedente = Convert.ToInt32(registro.Substring(18, 5));
                SubConta = Convert.ToInt32(registro.Substring(23, 2));
                ContaCorrente = Convert.ToInt32(registro.Substring(25, 11));
                // 36 - 37 -> 2 brancos
                CodigoDoDocumento = Convert.ToInt32(registro.Substring(38, 16));
                // 54 -> 1 branco
                CodigoDePostagem = Convert.ToInt32(registro.Substring(55, 1));
                // 56 - 62 -> 7 brancos
                CodigoDoDocumento = Convert.ToInt32(registro.Substring(63, 16));
                // 79 - 82 -> 4 brancos
                DataDeCredito = Convert.ToDateTime(dataCredito.ToString("##-##-##"));
                Moeda = Convert.ToInt32(registro.Substring(89, 1));
                // 90 - 107 -> 18 brancos
                Carteira = Convert.ToInt32(registro.Substring(108, 1));
                CodigoDeOcorrencia = Convert.ToInt32(registro.Substring(109, 2));
                DataDaOcorrencia = Convert.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                SeuNumero = Convert.ToInt32(registro.Substring(117, 6));
                MotivoDaOcorrencia = Convert.ToInt32(registro.Substring(123, 9));
                // 132 - 146 -> 15 brancos
                DataDeVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-##"));
                ValorDoTituloParcela = Convert.ToInt64(registro.Substring(153, 11));
                BancoCobrador = Convert.ToInt32(registro.Substring(166, 3));
                AgenciaCobradora = Convert.ToInt32(registro.Substring(169, 5));
                Especie = Convert.ToInt32(registro.Substring(174, 2));
                Iof = Convert.ToInt64(registro.Substring(176, 9));
                // 187 - 240 -> 54 brancos
                Desconto = Convert.ToInt64(registro.Substring(241, 11));
                ValorPago = Convert.ToInt64(registro.Substring(254, 11));
                JurosDeMora = Convert.ToInt64(registro.Substring(267, 11));
                Constante = Convert.ToInt32(registro.Substring(280, 1));
                QuantidadeMoeda = Convert.ToInt32(registro.Substring(281, 11));
                CotacaoMoeda = Convert.ToInt64(registro.Substring(294, 10));
                StatusDaParcela = Convert.ToInt32(registro.Substring(309, 1));
                IdentificadorLancamentoConta = Convert.ToInt32(registro.Substring(310, 6));
                // 316 - 341 - 26 brancos
                TipoLiquidacao = Convert.ToInt32(registro.Substring(342, 1));
                OrigemDaTarifa = Convert.ToInt32(registro.Substring(343, 1));
                // 344 - 394 - 51 brancos
                NumeroSequencial = Convert.ToInt32(registro.Substring(395, 6));
            }
            catch (Exception)
            {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 400.");
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
