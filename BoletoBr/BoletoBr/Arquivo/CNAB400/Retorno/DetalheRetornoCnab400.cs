using System;
using System.Collections.Generic;
using System.Configuration;
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
        public string NossoNumero { get; set; }
        public int MotivoDaOcorrencia { get; set; }
        public int DataDeVencimento { get; set; }
        public decimal ValorDoTituloParcela { get; set; }
        public int BancoCobrador { get; set; }
        public int AgenciaCobradora { get; set; }
        public string Especie { get; set; }
        public decimal ValorIof { get; set; }
        public decimal ValorOutrasDespesas { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorPrincipal { get; set; }
        public decimal ValorJurosDeMora { get; set; }
        public decimal ValorAbatimento { get; set; }
        public decimal ValorMulta { get; set; }
        public decimal ValorLiquidoRecebido { get; set; }
        public decimal OutrosCreditos { get; set; }
        public int Constante { get; set; }
        public decimal QuantidadeMoeda { get; set; }
        public decimal CotacaoMoeda { get; set; }
        public int StatusDaParcela { get; set; }
        public int IdentificadorLancamentoConta { get; set; }
        public int TipoLiquidacao { get; set; }
        public int OrigemDaTarifa { get; set; }
        public int NumeroSequencial { get; set; }

        #endregion

        #region Bradesco

        public string IdentificacaoEmpresaNoBanco { get; set; }
        public string IndicadorRateioCredito { get; set; }
        /// <summary>
        /// Despesas de cobrança para os códigos de ocorrência:
        /// 02 - Entrada Confirmada
        /// 28 - Débito de Tarifas
        /// </summary>
        public decimal ValorDespesas { get; set; }
        public decimal ValorJurosAtraso { get; set; }
        public string MotivoCodigoOcorrencia { get; set; }
        public int OrigemPagamento { get; set; }
        public string Cheque { get; set; }
        public string MotivoCodigoRejeicao { get; set; }
        public int NumeroCartorio { get; set; }
        public string NumeroProtocolo { get; set; }

        #endregion

        #region HSBC

        public int CodigoDeInscricao { get; set; }

        #endregion

        #region Banco do Brasil CBR 643

        public string DvAgenciaCedente { get; set; }
        public string DvContaCorrente { get; set; }
        public int NumeroConvenio { get; set; }
        public string NumeroControle { get; set; }
        /// <summary>
        /// Tipo de Cobrança
        /// 1 - Simples
        /// 2 - Vinculada
        /// 4 - Descontada
        /// 7 - Cobrança Simples Carteira 17
        /// 8 - Vendor
        /// </summary>
        public int TipoCobranca { get; set; }
        /// <summary>
        /// Tipo de Cobrança Específico para Comando 72
        /// Alteração de tipo de cobrança de títulos das carteiras 11 e 17
        /// </summary>
        public int TipoCobrancaCmd17 { get; set; }
        public int DiasCalculo { get; set; }
        public int NaturezaRecebimento { get; set; }
        public string PrefixoTitulo { get; set; }
        public int VariacaoCarteira { get; set; }
        public int ContaCaucao { get; set; }
        public int TaxaDesconto { get; set; }
        public int TaxaIof { get; set; }
        public int Comando { get; set; }
        public int DataLiquidacao { get; set; }
        /// <summary>
        /// Número do título dado pelo cedente
        /// </summary>
        public string TituloDadoCedente { get; set; }
        public string DvAgenciaCobradora { get; set; }
        public decimal ValorOutrosRecebimentos { get; set; }
        public decimal ValorAbatimentosNaoAproveitado { get; set; }
        public decimal ValorLancamento { get; set; }
        public int IndicativoDebitoCredito { get; set; }
        public int IndicadorValor { get; set; }
        public decimal ValorAjuste { get; set; }
        /// <summary>
        /// Posição: 333 a 333
        /// Para cobrança compartilhada usar:
        /// 2 - Convênio compartilhador
        /// 3 - Convênio compartilhado
        /// </summary>
        public string BrancosIndicadorConvenio { get; set; }
        /// <summary>
        /// Posição: 334 a 342
        /// Valor original do título pago pelo sacado
        /// </summary>
        public decimal BrancosValorPagoTitulo { get; set; }
        /// <summary>
        /// Posição: 343 a 349
        /// Se 333 igual a 2 - informado convênio compartilhado.
        /// Se 333 igual a 3 - informado convênio compartilhador.
        /// </summary>
        public long ZerosNumeroPrimeiroConvenio { get; set; }
        /// <summary>
        /// Posição: 350 a 358
        /// Se 333 igual a 2 - valor repassado para o primeiro convênio compartilhado.
        /// Se 333 igual a 3 - valor recebido no convênio compartilhador.
        /// </summary>
        public decimal ZerosValorPrimeiroConvenio { get; set; }
        /// <summary>
        /// Posição: 359 a 365
        /// Se 333 igual a 2 - número do segundo convênio compartilhado.
        /// Se 333 igual a 3 - zeros.
        /// </summary>
        public long ZerosNumeroSegundoConvenio { get; set; }
        /// <summary>
        /// Posição: 366 a 374
        /// Se 333 igual a 2 - valor repassado para o segundo convênio compartilhado.
        /// Se 333 igual a 3 - zeros
        /// </summary>
        public decimal ZerosValorSegundoConvenio { get; set; }
        /// <summary>
        /// Posição: 375 a 381
        /// Se 333 igual a 2 - número do terceiro convênio compartilhado.
        /// Se 333 igual a 3 - zeros
        /// </summary>
        public long ZerosNumeroTerceiroConvenio { get; set; }
        /// <summary>
        /// Posição: 382 a 390
        /// Se 333 igual a 2 - valor repassado para o terceiro convênio compartilhado.
        /// Se 333 igual a 3 - zeros
        /// </summary>
        public decimal ZerosValorTerceiroConvenio { get; set; }
        public int AutorizacaoLiquidacaoParcial { get; set; }
        /// <summary>
        /// Canal utilizado para pagamento/Meios de apresenção do título ao sacado.
        /// -> Comando 02 nas posições 109/110 (Confirmação de entrada de título)
        /// 00 - Não é sacado eletrônico no DDA
        /// 50 - Sacado eletrônico no DDA
        /// -> Comando 06, 07, 08, 15 ou 46 nas posições 109/110 (Liquidação de título)
        /// 01 - terminal de auto-atendimento
        /// 02 - internet
        /// 03 - central de atendimento (URA)
        /// 04 - gerenciador financeiro
        /// 05 - central de atendimento
        /// 06 - outro canal de auto-atendimento
        /// 07 - correspondente bancário
        /// 08 - guichê de caixa
        /// 09 - arquivo-eletrônico
        /// 10 - compensação
        /// 11 - outro canal eletrônico
        /// </summary>
        public int MeioApresentacaoTituloAoSacado { get; set; }

        #endregion

        #region Caixa Econômica Federal

        public int TipoInscricao { get; set; }
        public long NumeroInscricao { get; set; }
        public int IdEmissao { get; set; }
        public int IdPostagem { get; set; }
        /// <summary>
        /// Identificação do título na empresa
        /// </summary>
        public string UsoDaEmpresa { get; set; }
        public int ModalidadeNossoNumero { get; set; }
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
        public int DataDebitoTarifa { get; set; }

        #endregion

        #region Itaú

        public int CodigoLayout { get; set; }
        public int DacAgenciaConta { get; set; }
        public int DacNossoNumero { get; set; }
        public string LiteralMoeda { get; set; }
        public string Aceite { get; set; }
        public int DataEmissao { get; set; }
        public int TipoInscricaoSacado { get; set; }
        public long NumeroInscricaoSacado { get; set; }
        public string NomeSacado { get; set; }
        public string LogradouroSacado { get; set; }
        public string BairroSacado { get; set; }
        public string CepSacado { get; set; }
        public string CidadeSacado { get; set; }
        public string EstadoSacado { get; set; }
        public string SacadorAvalista { get; set; }
        public string LocalPagamento1 { get; set; }
        public string LocalPagamento2 { get; set; }
        public int TipoInscricaoSacadorAvalista { get; set; }
        public long NumeroInscricaoSacadorAvalista { get; set; }

        #endregion
    }
}
