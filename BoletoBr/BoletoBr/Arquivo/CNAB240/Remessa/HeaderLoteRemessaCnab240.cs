using System;

namespace BoletoBr.Arquivo.CNAB240.Remessa
{
    public class HeaderLoteRemessaCnab240
    {
        public HeaderLoteRemessaCnab240(Boleto boleto, int sequencialRemessa)
        {
            this.CodigoBanco = boleto.BancoBoleto.CodigoBanco;
            this.NumeroInscricao = boleto.CedenteBoleto.CpfCnpj;
            this.Agencia = boleto.CedenteBoleto.ContaBancariaCedente.Agencia;
            this.DigitoAgencia = boleto.CedenteBoleto.ContaBancariaCedente.DigitoAgencia;
            this.CodigoCedente = boleto.CedenteBoleto.CodigoCedente;
            this.Convenio = boleto.CedenteBoleto.Convenio;
            this.NomeEmpresa = boleto.CedenteBoleto.Nome;
            this.NumeroRemessaRetorno = sequencialRemessa.ToString();
        }

        public string CodigoBanco { get; set; }
        public int LoteServico { get; set; }
        public int TipoRegistro { get; set; }
        public string TipoOperacao { get; set; }
        public int TipoServico { get; set; }
        public string VersaoLayoutLote { get; set; }
        public string TipoInscricao { get; set; }
        public string NumeroInscricao { get; set; }
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
