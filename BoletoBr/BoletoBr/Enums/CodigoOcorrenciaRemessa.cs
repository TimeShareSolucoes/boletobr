using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Enums
{
    public enum CodigoOcorrenciaRemessa
    {
        #region CÓDIGO DE MOVIMENTO REMESSA - COMUNS

        Registro,
        Baixa,
        Protesto,
        NaoProtestar,
        AlteracaoDeOutrosDados,
        CancelamentoDoRateioDeCredito,
        SustarProtesto,
        ProtestoParaFinsFalimentares,
        ConcessaoDeDescontoComData,

        #endregion

        #region CÓDIGO DE MOVIMENTO REMESSA - 001|BANCO DO BRASIL

        PedidoDeDebitoEmConta,


        #endregion

        #region CÓDIGO DE MOVIMENTO REMESSA - 033|SANTANDER

        AlteracaoDaIdentificacaoDotituloNaEmpresa,
        AlteracaoSeuNumero,

        #endregion

        #region CÓDIGO DE MOVIMENTO REMESSA - 104|CAIXA

        /* Código de Movimento Remessa
        01| Entrada de Título
        02| Pedido de Baixa
        04| Concessão de Abatimento
        05| Cancelamento de Abatimento
        06| Alteração de Vencimento
        07| Concessão de Desconto
        08| Cancelamento de Desconto
        09| Protestar (transferir de Devolução para Protesto)
        10| Sustar Protesto e Baixar Título
        11| Sustar Protesto e Manter em Carteira
        12| Alteração de Juros de Mora
        13| Dispensar Cobrança de Juros de Mora
        14| Alteração de Valor/Percentual de Multa
        15| Dispensar Cobrança de Multa
        16| Alteração do Valor de Desconto
        17| Não Conceder Desconto
        18| Alteração do Valor do Abatimento
        31| Alteração de Outros Dados
        33| Alteração dos Dados do Rateio de Crédito
        34| Pedido de Cancelamento dos Dados do Rateio de Crédito
        36| Inclusão no Banco de Sacados
        37| Alteração no Banco de Sacados
        38| Exclusão no Banco de Sacados
        40| Serviços
        */ 
        
        ConcessaoDeAbatimento,
        CancelamentoDeAbatimento,
        AlteracaoDeVencimento,
        ConcessaoDeDesconto,
        CancelamentoDeDesconto,
        SustarProtestoEBaixarTitulo,
        SustarProtestoEManterEmCarteira,
        AlteracaoDeJurosDeMora,
        DispensarCobrancaDeJurosDeMora,
        AlteracaoDeValorPercentualDeMulta,
        DispensarCobrancaDeMulta,
        AlteracaoDoValorDeDesconto,
        NaoConcederDesconto,
        AlteracaoDoValorDeAbatimento,
        AlteracaoDosDadosDoRateioDeCredito,
        InclusaoNoBancoDeSacados,
        AlteracaoNoBancoDeSacados,
        ExclusaoNoBancoDeSacados,
        Servicos,

        #endregion

        #region CÓDIGO DE MOVIMENTO REMESSA - 237|BRADESCO

        AlteracaoDoControleDoParticipante,
        TransferenciaCessaoCredito,
        TransferenciaEntreCarteiras,
        DevTransferenciaEntreCarteiras,
        DesagendamentoDoDebitoAutomatico,
        AcertoNosDadosDoRateioDeCredito,

        #endregion

        #region CÓDIGO DE MOVIMENTO REMESSA - 341|ITAÚ
        
        ExclusaoDeSacadorAvalista,
        BaixaPorTerSidoPagoDiretamenteAoCedente,
        CancelamentoDeInstrucao,
        AlteracaoDoVencimentoESustarProtesto,
        CedenteNaoConcordaComAlegacaoDoSacado,
        CedenteSolicitaDispensaDeJuros,

        #endregion

        #region CÓDIGO DE MOVIMENTO REMESSA - 399|HSBC

        AlteracaoDeDiasParaEnvioACartorio,
        InclusaoDePagadorNoBoleto,
        ExclusaoDePagadorNoBoleto,
        Reemissao,
        EntradaDeTitulosComParcelasFaltantes,
        TransferenciaParaDesconto,
        NaoCobrarJurosDeMora,
        CancelamentoDescontoFixo,
        CancelamentoDescontoDiario,
        AlteracaoDeVencimentoComData

        #endregion
    }
}
