using System.Collections.Generic;
using BoletoBr.CalculoModulo;

namespace BoletoBr.Bancos
{
    public interface IBanco
    {
        string CodigoBanco { get; set; }
        string DigitoBanco { get; set; }
        string NomeBanco { get; set; }

        ICalculadoraModulo10 CalculadoraModulo10 { get; set; }
        ICalculadoraModulo11 CalculadoraModulo11 { get; set; }

        List<CarteiraCobranca> GetCarteirasCobranca();
        CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira);

        void ValidaBoletoComNormasBanco(Boleto boleto);
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
    }
}
