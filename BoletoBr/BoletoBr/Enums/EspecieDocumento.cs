using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Enums
{
    public enum EnumEspecieDocumento
    {
        Cheque,
        DuplicataMercantil,
        DuplicataMercantilIndicacao,
        DuplicataServico,
        DuplicataServicoIndicacao,
        DuplicataRural, //DR � DUPLICATA RURAL
        LetraCambio, //LC � LETRA DE CAMBIO
        NotaCreditoComercial, //NCC � NOTA DE CR�DITO COMERCIAL
        NotaCreditoExportacao, //NCE � NOTA DE CR�DITO A EXPORTA��O
        NotaCreditoIndustrial, //NCI � NOTA DE CR�DITO INDUSTRIAL
        NotaCreditoRural, //NCR � NOTA DE CR�DITO RURAL
        NotaPromissoria, //NP � NOTA PROMISS�RIA
        NotaPromissoriaRural, //NPR �NOTA PROMISS�RIA RURAL
        TriplicataMercantil, //TM � TRIPLICATA MERCANTIL
        TriplicataServico, //TS �  TRIPLICATA DE SERVI�O
        NotaSeguro, //NS � NOTA DE SEGURO
        Recibo, //RC � RECIBO
        Fatura, //FAT � FATURA
        NotaDebito, //ND �  NOTA DE D�BITO
        ApoliceSeguro, //AP �  AP�LICE DE SEGURO
        MensalidadeEscolar, //ME � MENSALIDADE ESCOLAR
        ParcelaConsorcio, //PC �  PARCELA DE CONS�RCIO
        Outros, //OUTROS
        NotaFiscal, //NF-Nota Fiscal
        DocumentoDivida, //DD-Documento de Dívida
        LetraCambio353, //07	LC - LETRA DE C�MBIO (SOMENTE PARA BANCO 353)
        LetraCambio008, //30	LC - LETRA DE C�MBIO (SOMENTE PARA BANCO 008)
        NotaPromissoariaDireta,
        Contrato,
        Cosseguros,
        EncargosCondominais,
        ContaPrestacaoServicos,
        Diversos,
        CobrancaSeriada,
        NotaDeSeguro,
        CedulaProdutoRural
    }
}
