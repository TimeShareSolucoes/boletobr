
namespace BoletoBr.Arquivo.CNAB400.Remessa
{
    public class TrailerRemessaCnab400
    {
        public TrailerRemessaCnab400(decimal valorTotalTitulos, int numeroSequencialRegistro)
        {
            this.NumeroSequencialRegistro = numeroSequencialRegistro;
            this.ValorTotalTitulos = valorTotalTitulos;
            #region #033|SANTANDER

            // O número sequencial do registro dentro do arquivo de remessa é igual ao total de linhas do arquivo.
            this.TotalLinhasArquivo = numeroSequencialRegistro;
            
            #endregion
        }

        public int TipoDeRegistro { get; set; }
        public int NumeroSequencialRegistro { get; set; }

        #region #033|SANTANDER

        public int TotalLinhasArquivo { get; set; }
        public decimal ValorTotalTitulos { get; set; }
        #endregion

    }
}
