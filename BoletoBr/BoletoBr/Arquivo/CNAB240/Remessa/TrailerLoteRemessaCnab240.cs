
namespace BoletoBr.Arquivo.CNAB240.Remessa
{
    public class TrailerLoteRemessaCnab240
    {
        public int CodigoBanco { get; set; }
        public int LoteServico { get; set; }
        public int TipoRegistro { get; set; }
        public int QtdRegistrosLote { get; set; }
        public int QtdTitulosCobrancaSimples { get; set; }
        public decimal ValorTitulosCobrancaSimples { get; set; }
        public int QtdTitulosCobrancaCaucionada { get; set; }
        public decimal ValorTitulosCobrancaCaucionada { get; set; }
        public int QtdTitulosCobrancaDescontada { get; set; }
        public decimal ValorTitulosCobrancaDescontada { get; set; }
    }
}
