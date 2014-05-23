using BoletoBr.CalculoModulo;

namespace BoletoBr.Bancos
{
    public class BancoAbstract
    {
        private readonly ICalculadoraModulo10 _calculoModulo10;

        #region Propriedades
        /// <summary>
        /// Código do Banco
        /// Ex: 341 - Itaú; 104 - Caixa
        /// </summary>
        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        #endregion

        public BancoAbstract(ICalculadoraModulo10 calculoModulo10)
        {
            _calculoModulo10 = calculoModulo10;
        }
    }
}
