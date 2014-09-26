using System;

namespace BoletoBr.Arquivo.CNAB150
{
    public class HeaderRetornoCnab150
    {
        public string CodigoDoRegistro { get; set; }
        public int CodigoDeRemessa { get; set; }
        public int CodigoDoConvenio { get; set; }
        public string NomeDaEmpresa { get; set; }
        public int CodigoDoBanco { get; set; }
        public string NomeDoBanco { get; set; }
        public DateTime DataGeracao { get; set; }
        public int NumeroSequencial { get; set; }
        public int VersaoDoLayout { get; set; }
        public string UsoDoBanco { get; set; }
        public int Zeros { get; set; }
    }
}