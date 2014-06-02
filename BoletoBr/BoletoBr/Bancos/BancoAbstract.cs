using System.Collections.Generic;
using BoletoBr.CalculoModulo;

namespace BoletoBr.Bancos
{
    public class BancoAbstract
    {
        #region Propriedades
        /// <summary>
        /// Código do Banco
        /// Ex: 341 - Itaú; 104 - Caixa
        /// </summary>
        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        #endregion
        public virtual List<CarteiraCobranca> GetCarteirasCobranca()
        {
            return new List<CarteiraCobranca>();
        }
        public CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira)
        {
            return GetCarteirasCobranca().Find(fd => fd.Codigo == codigoCarteira);
        }

        /// <summary>
        /// Formata código de barras seguindo regras específicas do banco
        /// </summary>
        /// <param name="boleto"></param>
        protected virtual void FormataCodigoBarra(Boleto boleto)
        {

        }
        /// <summary>
        /// Formata a linha digitável do boleto, seguindo as regras específicas do banco.
        /// </summary>
        /// <param name="boleto"></param>
        protected virtual void FormataLinhaDigitavel(Boleto boleto)
        {

        }
        /// <summary>
        /// Formata o Nosso número do boleto, seguindo as regras específicas do banco.
        /// </summary>
        /// <param name="boleto"></param>
        protected virtual void FormataNossoNumero(Boleto boleto)
        {

        }
        /// <summary>
        /// Formata o número do documento, seguindo as regras específicas do banco.
        /// </summary>
        /// <param name="boleto"></param>
        protected virtual void FormataNumeroDocumento(Boleto boleto)
        {

        }
    }
}
