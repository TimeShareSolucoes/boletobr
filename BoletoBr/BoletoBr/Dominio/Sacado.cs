namespace BoletoBr
{
    public class Sacado
    {
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public Endereco EnderecoSacado { get; set; }

        public Sacado(string nome, string cpfCnpj, Endereco endereco)
        {
            this.Nome = nome;
            this.CpfCnpj = cpfCnpj;
            this.EnderecoSacado = endereco;
        }
    }
}
