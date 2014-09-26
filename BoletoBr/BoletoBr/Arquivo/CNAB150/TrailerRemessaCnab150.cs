namespace BoletoBr.Arquivo.CNAB150
{
    public class TrailerRemessaCnab150
    {
        public string CodigoDoRegistro { get; set; }
        public int TotalDeRegistros { get; set; }
        public int ValorTotalDosRegistros { get; set; }
        public string Filler { get; set; }
        public int Zeros { get; set; }
    }
}
