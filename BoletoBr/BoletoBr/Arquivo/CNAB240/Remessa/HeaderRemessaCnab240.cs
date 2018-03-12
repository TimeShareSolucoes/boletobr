
using System;

namespace BoletoBr.Arquivo.CNAB240.Remessa
{
    public class HeaderRemessaCnab240
    {
        public HeaderRemessaCnab240(Boleto boleto, int sequencialArquivo)
        {
            this.CodigoBanco = boleto.BancoBoleto.CodigoBanco;
            this.NumeroInscricao = boleto.CedenteBoleto.CpfCnpj;
            this.AgenciaMantenedora = boleto.CedenteBoleto.ContaBancariaCedente.Agencia;
            this.DigitoAgenciaMantenedora = boleto.CedenteBoleto.ContaBancariaCedente.DigitoAgencia;
            this.CodigoCedente = boleto.CedenteBoleto.CodigoCedente;
            this.DigitoCedente = boleto.CedenteBoleto.DigitoCedente;
            this.NomeEmpresa = boleto.CedenteBoleto.Nome;
            this.SequencialNsa = sequencialArquivo;

            this.NumeroContaCorrente = boleto.CedenteBoleto.ContaBancariaCedente.Conta;
            this.DigitoContaCorrente = boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta;
        }

        public string DigitoContaCorrente { get; set; }

        public string NumeroContaCorrente { get; set; }

        public string CodigoBanco { get; set; }
        public int LoteServico { get; set; }
        public int TipoRegistro { get; set; }
        public int TipoInscricao { get; set; }
        public string NumeroInscricao { get; set; }
        public string AgenciaMantenedora { get; set; }
        public string DigitoAgenciaMantenedora { get; set; }
        public string CodigoCedente { get; set; }
        public int DigitoCedente { get; set; }
        public string NomeEmpresa { get; set; }
        public string NomeBanco { get; set; }
        public string CodigoRemessa { get; set; }
        public DateTime DataGeracao { get; set; }
        public DateTime HoraGeracao { get; set; }
        public int SequencialNsa { get; set; }
        public string VersaoLayout { get; set; }
        public string Densidade { get; set; }
        public string ReservadoBanco { get; set; }
        public string ReservadoEmpresa { get; set; }
        public string VersaoAplicativo { get; set; }
    }
}
