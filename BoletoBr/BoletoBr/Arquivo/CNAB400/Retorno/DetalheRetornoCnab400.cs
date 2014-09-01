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
        public string CodigoDoServico { get; set; }
        public int CodigoDoBeneficiario { get; set; }
        public int CodigoAgenciaCedente { get; set; }
        public int SubConta { get; set; }
        public string ContaCorrente { get; set; }
        public string ContaCobranca { get; set; }
        public string CodigoDoDocumentoEmpresa { get; set; }
        public int CodigoDePostagem { get; set; }
        public string CodigoDoDocumentoBanco { get; set; }
        public DateTime DataDeCredito { get; set; }
        public int Moeda { get; set; }
        public int NumeroCarteira { get; set; }
        public int CodigoDeOcorrencia { get; set; }
        public DateTime DataDaOcorrencia { get; set; }
        /// <summary>
        /// Número da parcela e total de parcelas, sendo 3 dítidos para cada campo.
        /// PPP/TTT
        /// </summary>
        public int SeuNumero { get; set; }
        public string NossoNumero { get; set; }
        public int MotivoDaOcorrencia { get; set; }
        public DateTime DataDeVencimento { get; set; }
        public decimal ValorDoTituloParcela { get; set; }
        public int BancoCobrador { get; set; }
        public int AgenciaCobradora { get; set; }
        public string Especie { get; set; }
        public decimal ValorIof { get; set; }
        public decimal ValorIofDesconto { get; set; }
        public decimal ValorOutrasDespesas { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorPrincipal { get; set; }
        public decimal ValorJurosDeMora { get; set; }
        public decimal ValorJurosDesconto { get; set; }
        public decimal ValorAbatimento { get; set; }
        public decimal ValorMulta { get; set; }
        public decimal ValorLiquidoRecebido { get; set; }
        public decimal ValorOutrosCreditos { get; set; }
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

        #region Banco do Brasil

        #region CBR 643

        #region Detalhe Tipo 7 - GERAL

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
        /// 0 - Caso não haja alteração de tipo de cobrança
        /// 1 - Simples
        /// 2 - Vinculada
        /// 4 - Descontada
        /// 7 - Cobrança Simples Carteira 17
        /// 8 - Vendor
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
        public DateTime DataLiquidacao { get; set; }
        /// <summary>
        /// Número do título dado pelo cedente
        /// </summary>
        public string TituloDadoCedente { get; set; }
        public string DvAgenciaCobradora { get; set; }
        public decimal ValorOutrosRecebimentos { get; set; }
        public decimal ValorAbatimentosNaoAproveitado { get; set; }
        public decimal ValorLancamento { get; set; }
        public string IndicativoDebitoCredito { get; set; }
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

        #region Detalhe Tipo 2 - Cobrança Partilhada

        // Informações Partilha 1
        public int BancoParaCredito1 { get; set; }
        public int CamaraCompensacao1 { get; set; }
        public int AgenciaParaCredito1 { get; set; }
        public string DvAgenciaParaCredito1 { get; set; }
        public long ContaParaCredito1 { get; set; }
        public string DvContaParaCredito1 { get; set; }
        public string NomeFavorecido1 { get; set; }
        public decimal ValorInformadoPartilha1 { get; set; }
        public decimal ValorEfetivamentePartilhado1 { get; set; }
        // Informações Partilha 2
        public int BancoParaCredito2 { get; set; }
        public int CamaraCompensacao2 { get; set; }
        public int AgenciaParaCredito2 { get; set; }
        public string DvAgenciaParaCredito2 { get; set; }
        public long ContaParaCredito2 { get; set; }
        public string DvContaParaCredito2 { get; set; }
        public string NomeFavorecido2 { get; set; }
        public decimal ValorInformadoPartilha2 { get; set; }
        public decimal ValorEfetivamentePartilhado2 { get; set; }
        // Informações Partilha 3
        public int BancoParaCredito3 { get; set; }
        public int CamaraCompensacao3 { get; set; }
        public int AgenciaParaCredito3 { get; set; }
        public string DvAgenciaParaCredito3 { get; set; }
        public long ContaParaCredito3 { get; set; }
        public string DvContaParaCredito3 { get; set; }
        public string NomeFavorecido3 { get; set; }
        public decimal ValorInformadoPartilha3 { get; set; }
        public decimal ValorEfetivamentePartilhado3 { get; set; }
        // Informações Partilha 4
        public int BancoParaCredito4 { get; set; }
        public int CamaraCompensacao4 { get; set; }
        public int AgenciaParaCredito4 { get; set; }
        public string DvAgenciaParaCredito4 { get; set; }
        public long ContaParaCredito4 { get; set; }
        public string DvContaParaCredito4 { get; set; }
        public string NomeFavorecido4 { get; set; }
        public decimal ValorInformadoPartilha4 { get; set; }
        public decimal ValorEfetivamentePartilhado4 { get; set; }
        // Tipos e Inscrições Partilha
        public int TipoInscricaoFavorecido1 { get; set; }
        public long NumeroInscricaoFavorecido1 { get; set; }
        public int TipoInscricaoFavorecido2 { get; set; }
        public long NumeroInscricaoFavorecido2 { get; set; }
        public int TipoInscricaoFavorecido3 { get; set; }
        public long NumeroInscricaoFavorecido3 { get; set; }
        public int TipoInscricaoFavorecido4 { get; set; }
        public long NumeroInscricaoFavorecido4 { get; set; }

        #endregion

        #region Detalhe Tipo 3 - Cobrança Vendor

        public string DvNossoNumero { get; set; }
        /// <summary>
        /// Número da operação BBVendor no aplicativo CIOPE
        /// </summary>
        public long NumeroOperacaoBBVendor { get; set; }
        /// <summary>
        /// Data da operação BBVendor (DDMMAAAA)
        /// </summary>
        public DateTime DataOperacaoBBVendor { get; set; }
        public int TaxaJurosVendedor { get; set; }
        public int TaxaJurosComprador { get; set; }
        /// <summary>
        /// Valor do título no vencimento
        /// </summary>
        public decimal ValorTituloVencimento { get; set; }
        /// <summary>
        /// Valor original da venda
        /// </summary>
        public decimal ValorOriginal { get; set; }
        public decimal ValorEncargosComprador { get; set; }
        public decimal ValorIofFinanciado { get; set; }
        public decimal ValorAcumuladoAbatimento { get; set; }
        public int IndicativoEpocaEqualizacao { get; set; }
        public int IndicativoNaturezaEqualizacao { get; set; }
        public decimal ValorEqualizacao { get; set; }
        public decimal ValorJurosProrrogacao { get; set; }
        public decimal ValorIofProrrogacao { get; set; }
        public decimal ValorIofPeriodoAtraso { get; set; }
        public string NomeComprador { get; set; }
        public int TipoInscricaoComprador { get; set; }
        public long NumeroInscricaoComprador { get; set; }
        public int TipoConversaoCnab240 { get; set; }
        public string NossoNumero17Posicoes { get; set; }
        public decimal NovoValorTitulo { get; set; }
        public decimal ValorEqualizacaoEstornada { get; set; }
        public decimal ValorNovaEqualizacao { get; set; }
        public decimal ValorDiferencaEqualizacao { get; set; }
        /// <summary>
        /// Indicativo de débito/crédito (corresponde ao valor indicado no campo 25)
        /// </summary>
        public int IndicativoDebitoCreditoCampo25 { get; set; }
        /// <summary>
        /// Indicativo da natureza da equalização (corresponde ao valor indicado no campo 25)
        /// </summary>
        public int IndicativoNaturezaEqualizacao1 { get; set; }
        /// <summary>
        /// Indicativo da natureza da equalização (corresponde ao valor indicado no campo 26)
        /// </summary>
        public int IndicativoNaturezaEqualizacao2 { get; set; }
        public decimal ValorIofNaoFinanciado { get; set; }
        public decimal ValorComissaoPermanencia { get; set; }

        #endregion

        #region Detalhe Tipo 5 - Bloqueto por e-mail

        /// <summary>
        /// Endereços de e-mail informado no arquivo-remessa
        /// </summary>
        public string EnderecosEmail { get; set; }

        #endregion

        #region Detalhe Tipo 5 - Dados do cheque utilizado para liquidação de título

        public int DataPagamento { get; set; }
        public decimal ValorCheque { get; set; }
        public int PrazoBloqueioCheque { get; set; }
        public int MotivoDevolucaoCheque { get; set; }
        public string TrilhaDoCheque { get; set; }
        public string TipoCaptura { get; set; }

        #endregion

        #region Detalhe Tipo 5 - Número do título do cedente com 15 posições (OPCIONAL)

        public string NumeroTituloCedente { get; set; }

        #endregion

        #endregion CBR 643

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
        public string CodigoFormaPagamento { get; set; }
        /// <summary>
        /// Informação de float negociado
        /// </summary>
        public int FloatNegociado { get; set; }
        /// <summary>
        /// Data do débito da tarifa
        /// </summary>
        public DateTime DataDebitoTarifa { get; set; }

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
        public string CodigoCarteira { get; set; }
        public string BandaMagneticaCheque { get; set; }
        public string IndicadorBoletoDDA { get; set; }
        public int CodigoInstrucaoCancelada { get; set; }
        /// <summary>
        /// Registros rejeitados ou alegação do sacado ou registro de mensagem informativa
        /// </summary>
        public string MensagemInformativa { get; set; }

        #endregion

        #region Santander

        public string CodigoOriginalRemessa { get; set; }
        public string CodigoOcorrencia1 { get; set; }
        public string CodigoOcorrencia2 { get; set; }
        public string CodigoOcorrencia3 { get; set; }
        public string IdentificadorComplemento { get; set; }
        public string UnidadeValorMoedaCorrente { get; set; }
        public decimal ValorTituloOutraUnidadeValor { get; set; }
        public decimal ValorIOCOutraUnidadeValor { get; set; }
        public decimal ValorDoDebitoCredito { get; set; }
        public int Versao { get; set; }
        #endregion
    }
}
