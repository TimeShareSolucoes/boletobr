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
        NaoReceberAposNDias,
        DevolverAposNDias,
        MultaVencimento,
        JurosdeMora,
        DescontoporDia,
        InstrucaoXItau,
    }
}
