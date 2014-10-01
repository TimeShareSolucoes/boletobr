using System;

namespace BoletoBr.Arquivo.CNAB240.Remessa
{
    public class HeaderLoteRemessaCnab240
    {
        public int CodigoBanco { get; set; }
        public int LoteServico { get; set; }
        public int TipoRegistro { get; set; }
        public string TipoOperacao { get; set; }
        public int TipoServico { get; set; }
        public string VersaoLayoutLote { get; set; }
        public string TipoInscricaoEmpresa { get; set; }
        public string NumeroInscricaoEmpresa { get; set; }
        public string Convenio { get; set; }
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string CodigoCedente { get; set; }
        public string CodigoModeloPersonalizado { get; set; }
        public string NomeEmpresa { get; set; }
        public string Mensagem1 { get; set; }
        public string Mensagem2 { get; set; }
        public string NumeroRemessaRetorno { get; set; }
        public DateTime DataGravacaoRemessaRetorno { get; set; }
        public DateTime DataCredito { get; set; }
    }
}
