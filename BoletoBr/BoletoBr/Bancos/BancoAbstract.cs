using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public class BancoAbstract
    {
        private readonly ICalculaModulo10 _calculoModulo10;

        #region Propriedades
        /// <summary>
        /// Código do Banco
        /// Ex: 341 - Itaú; 104 - Caixa
        /// </summary>
        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        #endregion

        public BancoAbstract(ICalculaModulo10 calculoModulo10)
        {
            _calculoModulo10 = calculoModulo10;
        }
    }
}
