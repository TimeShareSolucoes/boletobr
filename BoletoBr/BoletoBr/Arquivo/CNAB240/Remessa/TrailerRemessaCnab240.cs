
namespace BoletoBr.Arquivo.CNAB240.Remessa
{
    public class TrailerRemessaCnab240
    {
        public TrailerRemessaCnab240(int qtdLotes, int qtdRegistros)
        {
            this.QtdLotesArquivo = qtdLotes;
            this.QtdRegistrosArquivo = qtdRegistros;
        }

        public int CodigoBanco { get; set; }
        public int LoteServico { get; set; }
        public int TipoRegistro { get; set; }
        public int QtdLotesArquivo { get; set; }
        public int QtdRegistrosArquivo { get; set; }
    }
}
