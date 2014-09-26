
namespace BoletoBr.Arquivo.CNAB400.Remessa
{
    public class HeaderRemessaCnab400
    {
        public int TipoDeRegistro { get; set; }
        public int Operacao { get; set; }
        public string LiteralDeRemessa { get; set; }
        public int CodigoDoServico { get; set; }
        public string LiteralDeServico { get; set; }
        public int Agencia { get; set; }
        public int Zeros { get; set; }
        public int Conta { get; set; }
        public int DigitoAutoConferencia { get; set; }
        public string NomeDaEmpresa { get; set; }
        public int CodigoBanco { get; set; }
        public string NomeDoBanco { get; set; }
        public int DataDeGeracao { get; set; }
        public int NumeroSequencial { get; set; }
    }
}
