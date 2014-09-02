using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Enums
{
    internal enum CodigoOcorrenciaRemessa
    {

        #region CÓDIGO DE MOVIMENTO REMESSA - CAIXA

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
        
        EntradaDeTitulo,
        PedidoDeBaixa,
        ConcessaoDeAbatimento,
        CancelamentoDeAbatimento,
        AlteracaoDeVencimento,
        ConcessaoDeDesconto,
        CancelamentoDeDesconto,
        Protestar,
        SustarProtestoEBaixarTitulo,
        SustarProtestoEManterEmCarteira,
        AlteracaoDeJurosDeMora,
        DispensarCobrancaDeJurosDeMora,
        AlteracaoDeValorPercentualDeMulta,
        DispensarCobrancaDeMulta,
        AlteracaoDoValorDeDesconto,
        NaoConcederDesconto,
        AlteracaoDoValorDeAbatimento,
        AlteracaoDeOutrosDados,
        AlteracaoDosDadosDoRateioDeCredito,
        PedidoDeCancelamentoDosDAdosDoRateioDeCredito,
        InclusaoNoBancoDeSacados,
        AlteracaoNoBancoDeSacados,
        ExclusaoNoBancoDeSacados,
        Servicos

        #endregion

    }
}
