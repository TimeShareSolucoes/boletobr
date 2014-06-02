using System.Collections.Generic;
using BoletoBr.CalculoModulo;

namespace BoletoBr.Bancos
{
    public interface IBanco
    {
        string CodigoBanco { get; set; }
        string DigitoBanco { get; set; }
        string NomeBanco { get; set; }
        List<CarteiraCobranca> GetCarteirasCobranca();
        CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira);
        void ValidaBoletoComNormasBanco(Boleto boleto);
        /// <summary>
        /// Efetua os cálculos de linha digitável do boleto.
        /// </summary>
        /// <param name="boleto"></param>
        void FormatarBoleto(Boleto boleto);
    }
}
