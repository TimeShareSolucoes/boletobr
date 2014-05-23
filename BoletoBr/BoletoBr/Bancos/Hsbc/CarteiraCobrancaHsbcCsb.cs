namespace BoletoBr.Bancos.Hsbc
{
    public class CarteiraCobrancaHsbcCsb : BoletoBr.CarteiraCobranca
    {
        public CarteiraCobrancaHsbcCsb()
        {
            this.Codigo = "CSB";
            this.Tipo = "";
            this.Descricao = "CSB Cobrança Simples HSBC";
        }
    }
}
