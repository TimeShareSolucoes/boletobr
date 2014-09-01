using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Enums
{
    internal enum TipoOcorrenciaRetorno
    {
        //Ocorrências para arquivo retorno
        RetRegistroConfirmado,
        RetRegistroRecusado,
        RetComandoRecusado,
        RetLiquidado,
        RetLiquidadoEmCartorio,
        RetLiquidadoParcialmente,
        RetLiquidadoSaldoRestante,
        RetLiquidadoSemRegistro,
        RetLiquidadoPorConta,
        RetBaixaSolicitada,
        RetBaixado,
        RetBaixadoPorDevolucao,
        RetBaixadoFrancoPagamento,
        RetBaixaPorProtesto,
        RetRecebimentoInstrucaoBaixar,
        RetBaixaOuLiquidacaoEstornada,
        RetTituloEmSer,
        RetRecebimentoInstrucaoConcederAbatimento,
        RetAbatimentoConcedido,
        RetRecebimentoInstrucaoCancelarAbatimento,
        RetAbatimentoCancelado,
        RetRecebimentoInstrucaoConcederDesconto,
        RetDescontoConcedido,
        RetRecebimentoInstrucaoCancelarDesconto,
        RetDescontoCancelado,
        RetRecebimentoInstrucaoAlterarDados,
        RetDadosAlterados,
        RetRecebimentoInstrucaoAlterarVencimento,
        RetVencimentoAlterado,
        RetAlteracaoDadosNovaEntrada,
        RetAlteracaoDadosBaixa,
        RetRecebimentoInstrucaoProtestar,
        RetProtestado,
        RetRecebimentoInstrucaoSustarProtesto,
        RetProtestoSustado,
        RetInstrucaoProtestoRejeitadaSustadaOuPendente,
        RetDebitoEmConta,
        RetRecebimentoInstrucaoAlterarNomeSacado,
        RetNomeSacadoAlterado,
        RetRecebimentoInstrucaoAlterarEnderecoSacado,
        RetEnderecoSacadoAlterado,
        RetEncaminhadoACartorio,
        RetRetiradoDeCartorio,
        RetRecebimentoInstrucaoDispensarJuros,
        RetJurosDispensados,
        RetManutencaoTituloVencido,
        RetRecebimentoInstrucaoAlterarTipoCobranca,
        RetTipoCobrancaAlterado,
        RetDespesasProtesto,
        RetDespesasSustacaoProtesto,
        RetDebitoCustasAntecipadas,
        RetCustasCartorioDistribuidor,
        RetCustasEdital,
        RetProtestoOuSustacaoEstornado,
        RetDebitoTarifas,
        RetAcertoDepositaria,
        RetOutrasOcorrencias
    }
}
