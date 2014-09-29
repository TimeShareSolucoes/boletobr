
using System;

namespace BoletoBr.Arquivo.CNAB240.Remessa
{
    public class HeaderRemessaCnab240
    {
        public int CodigoBanco { get; set; }
        public int LoteServico { get; set; }
        public int TipoRegistro { get; set; }
        public int TipoInscricao { get; set; }
        public string NumeroInscricao { get; set; }
        public string AgenciaMantenedora { get; set; }
        public string DigitoAgenciaMantenedora { get; set; }
        public string CodigoCedente { get; set; }
        public string NomeEmpresa { get; set; }
        public string NomeBanco { get; set; }
        public string CodigoRemessa { get; set; }
        public DateTime DataGeracao { get; set; }
        public DateTime HoraGeracao { get; set; }
        public string SequenciaNsa { get; set; }
        public string VersaoLayout { get; set; }
        public string Densidade { get; set; }
        public string ReservadoBanco { get; set; }
        public string ReservadoEmpresa { get; set; }
        public string VersaoAplicativo { get; set; }
    }
}
