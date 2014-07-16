using System;

namespace BoletoBr
{
    public class Cedente
    {
        public ContaBancaria ContaBancariaCedente { get; set; }
        public string CodigoCedente { get; set; }
        public string Convenio { get; set; }
        public int DigitoCedente { get; set; }
        public string CpfCnpj { get; set; }
        public string Nome { get; set; }
        public Endereco EnderecoCedente { get; set; }
        public string CpfCnpjFormatado
        {
            get
            {
                var valorBaseTratado = CpfCnpj;
                valorBaseTratado = valorBaseTratado.Replace(".", "").Replace("-", "").Replace("/", "");

                if (valorBaseTratado.Length > 11)
                    return Convert.ToUInt64(valorBaseTratado).ToString(@"00\.000\.000\/0000\-00");
                
                return Convert.ToUInt64(valorBaseTratado).ToString(@"000\.000\.000\-00");
            }

        }

        public Cedente(string codigoCedente, int digitoCedente, string cpfCnpj, string nome, ContaBancaria contaBancaria, Endereco enderecoCedente)
        {
            this.CodigoCedente = codigoCedente;
            this.DigitoCedente = digitoCedente;
            this.CpfCnpj = cpfCnpj;
            this.Nome = nome;
            this.EnderecoCedente = enderecoCedente;
            this.ContaBancariaCedente = contaBancaria;
        }

        public Cedente(string codigoCedente, string convenio, int digitoCedente, string cpfCnpj, string nome, ContaBancaria contaBancaria, Endereco enderecoCedente)
        {
            this.CodigoCedente = codigoCedente;
            this.Convenio = convenio;
            this.DigitoCedente = digitoCedente;
            this.CpfCnpj = cpfCnpj;
            this.Nome = nome;
            this.EnderecoCedente = enderecoCedente;
            this.ContaBancariaCedente = contaBancaria;
        }
    }
}
