using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Enums;

namespace BoletoBr
{
    public class DetalheRetornoCnab400
    {
        #region Propriedades Comuns

        public int CodigoDoRegistro { get; set; }
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
        public decimal Abatimento { get; set; }
        public decimal ValorMulta { get; set; }
        public int Constante { get; set; }
        public decimal QuantidadeMoeda { get; set; }
        public decimal CotacaoMoeda { get; set; }
        public int StatusDaParcela { get; set; }
        public int IdentificadorLancamentoConta { get; set; }
        public int TipoLiquidacao { get; set; }
        public int OrigemDaTarifa { get; set; }
        public int NumeroSequencial { get; set; }

        #endregion

        #region HSBC

        public int CodigoDeInscricao { get; set; }

        #endregion


        #region Caixa Econômica Federal

        public TipoInscricao TipoInscricao { get; set; }
        public int NumeroInscricao { get; set; }
        public int IdEmissao { get; set; }
        public int IdPostagem { get; set; }
        public string UsoExclusivo { get; set; }
        public int ModalidadeNossoNumero { get; set; }
        public int NossoNumero { get; set; }
        public int CodigoRejeicao { get; set; }
        public string NumeroDocumento { get; set; }
        /// <summary>
        /// Valor da Tarifa / Despesa de Cobrança
        /// </summary>
        public decimal ValorTarifa { get; set; }
        /// <summary>
        /// Código do canal de liquidação ou da baixa do título
        /// </summary>
        public int CodigoBaixaTitulo { get; set; }
        /// <summary>
        /// Código que identifica a forma de pagamento
        /// </summary>
        public int CodigoFormaPagamento { get; set; }
        /// <summary>
        /// Informação de float negociado
        /// </summary>
        public int FloatNegociado { get; set; }
        /// <summary>
        /// Data do débito da tarifa
        /// </summary>
        public int DataDebito { get; set; }

        #endregion
    }
}
