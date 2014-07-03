namespace BoletoBr.Bancos.Hsbc
{
    public class CarteiraCobrancaHsbcCsb : CarteiraCobranca
    {
        /// <summary>
        /// Dados utilizados para geração do boleto:
        /// AGENCIA: 4 digitos
        /// COD_CLIENTE: 5 dígitos
        /// NUMERO_DOCUMENTO: (Letras);
        /// CONTA_CORRENTE: 7 dígitos (Letras) [Opcional]
        /// CONTA_CORRENTE_DV: 1 digito (Letras ou Inteiro) [Opcional]
        /// CARTEIRA: 3 dígitos (Inteiro) ou constante "COB" ou "CSB" (Letras)
        /// NOSSO_NUMERO: 5 dígitos
        /// </summary>
        public CarteiraCobrancaHsbcCsb()
        {
            this.Codigo = "CSB";
            this.Tipo = "";
            this.Descricao = "CSB Cobrança Simples HSBC";
        }
    }
}
