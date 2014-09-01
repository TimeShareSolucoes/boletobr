namespace BoletoBr.Bancos.Hsbc
{
    public class CarteiraCobrancaHsbcCsb : CarteiraCobranca
    {
        public CarteiraCobrancaHsbcCsb()
        {
            this.Codigo = "CSB";
            this.Tipo = "";
            this.Descricao = "CSB Cobrança Simples HSBC";
        }
    }
}
