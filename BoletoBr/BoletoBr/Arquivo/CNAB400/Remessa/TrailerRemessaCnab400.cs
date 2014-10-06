
namespace BoletoBr.Arquivo.CNAB400.Remessa
{
    public class TrailerRemessaCnab400
    {
        public TrailerRemessaCnab400(int numeroSequencialRegistro)
        {
            this.NumeroSequencialRegistro = numeroSequencialRegistro;
        }

        public int TipoDeRegistro { get; set; }
        public int NumeroSequencialRegistro { get; set; }
    }
}
