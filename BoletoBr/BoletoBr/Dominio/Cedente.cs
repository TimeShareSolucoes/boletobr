namespace BoletoBr
{
    public class Cedente
    {
        public Cedente(string codigoCedente, int digitoCedente, string cpfCnpj, string nome, ContaBancaria contaBancaria, Endereco enderecoCedente)
        {
            this.CodigoCedente = codigoCedente;
            this.DigitoCedente = digitoCedente;
            this.CpfCnpj = cpfCnpj;
            this.Nome = nome;
            this.EnderecoCedente = enderecoCedente;
            this.ContaBancariaCedente = contaBancaria;
        }
        public ContaBancaria ContaBancariaCedente { get; set; }
        public string CodigoCedente { get; set; }
        public string Convenio { get; set; }
        public int DigitoCedente { get; set; }
        public string CpfCnpj { get; set; }
        public string Nome { get; set; }
        public Endereco EnderecoCedente { get; set; }
    }
}
