using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Hsbc
{
    public class CarteiraCobrancaHsbcCnr : CarteiraCobranca
    {
        /// <summary>
        /// Dados utilizados para geração do boleto:
        /// AGENCIA: 4 dígitos
        /// COD_CLIENTE: 7 dígitos
        /// NUMERO_DOCUMENTO: (Letras);
        /// CONTA_CORRENTE: 7 dígitos (Letras) [Opcional]
        /// CONTA_CORRENTE_DV: 1 digito (letras ou Inteiro) [Opcional]
        /// CARTEIRA: 3 dígitos (Inteiro) ou constante "CNR" (Letras)
        /// NOSSO_NUMERO: 13 dígitos (Letras)
        /// </summary>
        public CarteiraCobrancaHsbcCnr()
        {
            this.Codigo = "CNR";
            this.Tipo = "";
            this.Descricao = "CNR Cobrança Não Registrada HSBC";
        }
    }
}
