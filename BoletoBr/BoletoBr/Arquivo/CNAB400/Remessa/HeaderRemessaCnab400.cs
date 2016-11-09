
using System;


namespace BoletoBr.Arquivo.CNAB400.Remessa
{
    public class HeaderRemessaCnab400
    {
        public HeaderRemessaCnab400(Boleto boleto, int numeroSequencialRemessa, int numeroSequencialRegistro,
            DateTime? dataHora = null)
        {
            this.CodigoBanco = boleto.BancoBoleto.CodigoBanco;
            this.Agencia = boleto.CedenteBoleto.ContaBancariaCedente.Agencia;
            this.DvAgencia = boleto.CedenteBoleto.ContaBancariaCedente.DigitoAgencia;
            this.ContaCorrente = boleto.CedenteBoleto.ContaBancariaCedente.Conta;
            this.DvContaCorrente = boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta;
            this.CodigoEmpresa = boleto.CedenteBoleto.CodigoCedente;
            this.DigitoCedenteEmpresa = boleto.CedenteBoleto.DigitoCedente.BoletoBrToStringSafe();
            this.NomeEmpresa = boleto.CedenteBoleto.Nome;
            this.NumeroSequencialRemessa = numeroSequencialRemessa;
            this.NumeroSequencialRegistro = numeroSequencialRegistro;
            this.Convenio = boleto.CedenteBoleto.Convenio;

            if (dataHora == null)
                this.DataDeGravacao = DateTime.Now;
            else
                this.DataDeGravacao = (DateTime) dataHora;

            #region #033|SANTANDER

            // Informação cedida pelo banco que identifica o arquivo remessa do cliente
            this.CodigoDeTransmissao = boleto.CodigoDeTransmissao;

            #endregion
        }

        public string CodigoBanco { get; set; }
        public DateTime DataDeGravacao { get; set; }
        public int NumeroSequencialRegistro { get; set; }
        public int NumeroSequencialRemessa { get; set; }
        public string CodigoEmpresa { get; set; }
        public string DigitoCedenteEmpresa { get; set; }
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

        #region #001|BANCO DO BRASIL

        public string Convenio { get; set; }

        #endregion
    }
}
