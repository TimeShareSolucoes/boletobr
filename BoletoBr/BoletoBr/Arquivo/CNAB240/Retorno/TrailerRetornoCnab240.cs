namespace BoletoBr.Arquivo.CNAB240.Retorno
{
    public class TrailerRetornoCnab240
    {
        public int CodigoBanco { get; set; }
        public string LoteServico { get; set; }
        public int CodigoRegistro { get; set; }
        public int QtdLotesArquivo { get; set; }
        public int QtdRegistrosArquivo { get; set; }

        #region Bradesco

        public int QtdContasConc { get; set; }

        #endregion
    }
}
