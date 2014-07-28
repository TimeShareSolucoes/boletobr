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
    }
}
