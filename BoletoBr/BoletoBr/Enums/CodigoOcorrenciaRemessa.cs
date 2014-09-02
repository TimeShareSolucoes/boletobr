using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Enums
{
    internal enum CodigoOcorrenciaRemessa
    {
        //Ocorrências para arquivo remessa
        RemRegistrar, //Registrar o título no banco
        RemBaixar, //Baixar o título no banco
        RemDebitarEmConta,
        RemConcederAbatimento,
        RemCancelarAbatimento,
        RemConcederDesconto,
        RemCancelarDesconto,
        RemAlterarVencimento,
        RemProtestar,
        RemCancelarInstrucaoProtesto,
        RemDispensarJuros,
        RemAlterarNomeEnderecoSacado,
        RemAlterarNumeroControle,
        RemOutrasOcorrencias,
    }
}
