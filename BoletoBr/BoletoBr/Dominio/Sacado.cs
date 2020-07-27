using System;

namespace BoletoBr
{
    public class Sacado
    {
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public Endereco EnderecoSacado { get; set; }
        public string NomeAvalista { get; set; }
        public string CpfCnpjAvalista { get; set; }
        public string CpfCnpjFormatado
        {
            get
            {
                var valorBaseTratado = CpfCnpj;
                valorBaseTratado = valorBaseTratado.Replace(".", "").Replace("-", "").Replace("/", "");

                if (valorBaseTratado.Length > 11)
                    return valorBaseTratado.BoletoBrSetMascara("##.###.###/####-##");

                return valorBaseTratado.BoletoBrSetMascara("###.###.###-##");
            }
        }

        public Sacado(string nome, string cpfCnpj, Endereco endereco)
        {
            this.Nome = nome;
            this.CpfCnpj = cpfCnpj;
            this.EnderecoSacado = endereco;
        }

        public string IdentificacaoClienteBanco { get; set; }
    }
}
