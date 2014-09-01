namespace BoletoBr
{
    public class CarteiraCobranca
    {
        public string Codigo { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        /// <summary>
        /// true = Carteira COM Registro
        /// false = Carteira SEM Registro
        /// </summary>
        public bool TipoRegistro { get; set; }
    }
}
