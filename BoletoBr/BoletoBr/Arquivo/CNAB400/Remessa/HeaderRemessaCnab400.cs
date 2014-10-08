
using System;


namespace BoletoBr.Arquivo.CNAB400.Remessa
{
    public class HeaderRemessaCnab400
    {
        public HeaderRemessaCnab400(Boleto boleto, int numeroSequencialRemessa, int numeroSequencialRegistro)
        {
            this.CodigoBanco = boleto.BancoBoleto.CodigoBanco;
            this.Agencia = boleto.CedenteBoleto.ContaBancariaCedente.Agencia;
            this.DvAgencia = boleto.CedenteBoleto.ContaBancariaCedente.DigitoAgencia;
            this.ContaCorrente = boleto.CedenteBoleto.ContaBancariaCedente.Conta;
            this.DvContaCorrente = boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta;
            this.CodigoEmpresa = boleto.CedenteBoleto.CodigoCedente;
            this.NomeEmpresa = boleto.CedenteBoleto.Nome;
            this.DataDeGravacao = DateTime.Now;
            this.NumeroSequencialRemessa = numeroSequencialRemessa;
            this.NumeroSequencialRegistro = numeroSequencialRegistro;

            #region #033|SANTANDER

            // Informação cedida pelo banco que identifica o arquivo remessa do cliente
            this.CodigoDeTransmissao = boleto.CedenteBoleto.CodigoCedente;

            #endregion
        }

        public string CodigoBanco { get; set; }
        public DateTime DataDeGravacao { get; set; }
        public int NumeroSequencialRegistro { get; set; }
        public int NumeroSequencialRemessa { get; set; }
        public string CodigoEmpresa { get; set; }
        public string NomeEmpresa { get; set; }

        #region #033|SANTANDER

        public string CodigoDeTransmissao { get; set; }

        #endregion

        #region #341|ITAÚ

        public string Agencia { get; set; }
        public string DvAgencia { get; set; }
        public string ContaCorrente { get; set; }
        public string DvContaCorrente { get; set; }

        #endregion
    }
}
