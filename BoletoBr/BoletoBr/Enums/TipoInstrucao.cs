using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Enums
{
    public enum EnumTipoInstrucao
    {
        Protestar,
        NaoProtestar,
        NaoReceberAposOVencimento,
        ProtestarAposNDiasCorridos,
        ProtestarAposNDiasUteis,
        NaoReceberAposNDiasCorridos,
        NaoReceberAposNDiasUteis,
        DevolverAposNDias,
        DevolverApos90Dias,
        MultaVencimento,
        JurosdeMora,
        DescontoPorDia,
        CobrarJurosApos7DiasVencimento,
        SemInstrucoes,

        #region Itaú

        ImportanciaPorDiaDeAtrasoAPartirDeDDMMAA,
        NoVencimentoPagavelEmQualquerAgencia,

        #endregion

        #region Santander

        NaoHaInstrucoes,
        BaixarAposQuinzeDiasDoVencto,
        BaixarAposTrintaDiasDoVencto,
        NaoBaixar,
        NaoCobrarJurosDeMora,

        #endregion

        #region HSBC

        MultaPercentualVencimento,
        MultaPorDiaVencimento,
        MultaPorDiaCorrido,
        MultaPorDiaUtil,
        JurosSoAposData,
        ConcederAbatimento,
        AposVencimentoMulta10PorCento,
        ConcederDescontoPagoAposVencimento,
        NaoReceberAntesDoVencimento,
        NaoReceberAntesdoVencimentoOu10DiasApos,
        AposVencimentoMulta20PorCentoMaisMora,
        AbatimentoDesconto,
        TituloSujeitoAProtestoAposVencimento,
        AposVencimentoMulta2PorCento,
        MultaDeVPorCentoAposNDiasCorridos,
        MultaDeVPorCentoAposNDiasUteis,

        // Instruções que não geram mensagens nos boletos
        ProtestarAposNDiasCorridosNGM,
        ProtestarAposNDiasUteisNGM,

        #endregion

        #region Bradesco

        CobrarEncargosApos5DiaVencimento,
        CobrarEncargosApos10DiaVencimento,
        CobrarEncargosApos15DiaVencimento,

        #endregion

        #region CAIXA

        ProtestoFinsFalimentares,

        #endregion

        #region Banco do Brasil

        // COMANDO 01
        ProtestarApos3DiasUteis,
        ProtestarApos4DiasUteis,
        ProtestarApos5DiasUteis,
        ConcederDescontoAte,

        // COMANDO 02
        Devolver,
        Baixar,
        EntregarAoSacado,

        #endregion

        #region BRB

        MultaDeVPorCentoSobreValorTitulo,
        MultaDeVPorCentoSobreValorTituloMaisEncargos,
        MultaDeVPorCentoAposNDiasCorridosValorTituloMaisEncargos,
        CobrarJurosMaisVariacaoIDTR50

        #endregion
    }
}
