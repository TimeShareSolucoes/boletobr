using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Itau
{
    public enum CodigoOcorrenciaRemessaItau
    {
        Remessa = 01,
        PedidoDeBaixa = 02,
        ConcessaoDeAbatimento = 04,
        CancelamentoDeAbatimento = 05,
        AlteracaoDoVencimento = 06,
        AlteracaoDoUsoDaEmpresa = 07,
        AlteracaoDoSeuNumero = 08,
        Protestar = 09,
        NaoProtestar = 10,
        ProtestoParaFinsFalimentares = 11,
        SustarOProtesto = 18,
        ExclusaoDeSacadorAvalista = 30,
        AlteracaoDeOutrosDados = 31,
        BaixaPorTerSidoPagoDiretamenteAoCedente = 34,
        CancelamentoDeInstrucao = 35,
        AlteracaoDoVencimentoESustarProtesto = 37,
        CedenteNaoConcordaComAlegacaoDoSacado = 38,
        CedenteSolicitaDispensaDeJuros = 47,
    }

    public enum CodigoOcorrenciaRetornoItau
    {
        EntradaConfirmada = 02,
        EntradaRejeitada = 03,
        AlteracaoNovaEntradaOuAlteracaoExclusao = 04,
        // ...
        // NOTA 17, pág. 20 do Layout Técnico do Itaú CNAB 400
    }
    public class CodigoOcorrenciaItau
    {

    }
}
