using System;
using System.Collections.Generic;
using System.Drawing;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Enums;

namespace BoletoBr.Interfaces
{
    public interface IBanco
    {
        string CodigoBanco { get; set; }
        string DigitoBanco { get; set; }
        string NomeBanco { get; set; }
        Image LogotipoBancoParaExibicao { get; set; }
        string LocalDePagamento { get; }
        string MoedaBanco { get; }
        //List<CarteiraCobranca> GetCarteirasCobranca();
        //CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira);
        void ValidaBoletoComNormasBanco(Boleto boleto);

        /// <summary>
        /// Formata o código/sigla da Espécie/Moeda do boleto.
        /// </summary>
        /// <param name="boleto"></param>
        void FormataMoeda(Boleto boleto);

        /// <summary>
        /// Efetua os cálculos de linha digitável do boleto.
        /// </summary>
        /// <param name="boleto"></param>
        void FormatarBoleto(Boleto boleto);

        /// <summary>
        /// Formata código de barras seguindo regras específicas do banco
        /// </summary>
        /// <param name="boleto"></param>
        void FormataCodigoBarra(Boleto boleto);

        /// <summary>
        /// Formata a linha digitável do boleto, seguindo as regras específicas do banco.
        /// </summary>
        /// <param name="boleto"></param>
        void FormataLinhaDigitavel(Boleto boleto);

        /// <summary>
        /// Formata o Nosso número do boleto, seguindo as regras específicas do banco.
        /// </summary>
        /// <param name="boleto"></param>
        void FormataNossoNumero(Boleto boleto);

        /// <summary>
        /// Formata o número do documento, seguindo as regras específicas do banco.
        /// </summary>
        /// <param name="boleto"></param>
        void FormataNumeroDocumento(Boleto boleto);

        /// <summary>
        /// Retorna o código do comando/movimento/ocorrência a ser usada na geração do arquivo de remessa.
        /// </summary>
        /// <param name="ocorrenciaRemessa"></param>
        /// <param name="valorOcorrencia"></param>
        /// <param name="dataOcorrencia"></param>
        /// <returns></returns>
        ICodigoOcorrencia ObtemCodigoOcorrencia(EnumCodigoOcorrenciaRemessa ocorrenciaRemessa, double valorOcorrencia, DateTime dataOcorrencia);

        /// <summary>
        /// Retorna a espécie de documento a ser usada na geração do arquivo de remessa.
        /// </summary>
        /// <param name="especie"></param>
        IEspecieDocumento ObtemEspecieDocumento(EnumEspecieDocumento especie);

        /// <summary>
        /// Retorna a instrução a ser usada na geração do arquivo de remessa.
        /// </summary>
        /// <param name="tipoInstrucao">Tipo de instrução padronizada no componente</param>
        /// <param name="valorInstrucao">Valor monetário da instrução.</param>
        /// <param name="dataInstrucao">Usado quando é solicitada data no formato dd/mm/yyyy na instrução</param>
        /// <param name="diasInstrucao">Quantidade de dias para execução da instrução</param>
        IInstrucao ObtemInstrucaoPadronizada(EnumTipoInstrucao tipoInstrucao, double valorInstrucao, DateTime dataInstrucao, int diasInstrucao);

        /// <summary>
        /// Faz a leitura do arquivo de retorno.
        /// </summary>
        /// <param name="linhasArquivo">Linhas do arquivo que será processado</param>
        RetornoGenerico LerArquivoRetorno(List<string> linhasArquivo);
        RemessaCnab240 GerarArquivoRemessaCnab240(List<Boleto> boletos);
        RemessaCnab400 GerarArquivoRemessaCnab400(List<Boleto> boletos);
    }
}
