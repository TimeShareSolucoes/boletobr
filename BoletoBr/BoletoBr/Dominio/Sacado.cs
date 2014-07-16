using System;

namespace BoletoBr
{
    public class Sacado
    {
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public Endereco EnderecoSacado { get; set; }
        public string CpfCnpjFormatado
        {
            get
            {
                var valorBaseTratado = CpfCnpj;
                valorBaseTratado = valorBaseTratado.Replace(".", "").Replace("-", "").Replace("/", "");

                if (valorBaseTratado.Length > 11)
                    return MetodosExtensao.SetMascara(valorBaseTratado, "##.###.###/####-##");

                return MetodosExtensao.SetMascara(valorBaseTratado, "###.###.###-##");
            }
        }

        public Sacado(string nome, string cpfCnpj, Endereco endereco)
        {
            this.Nome = nome;
            this.CpfCnpj = cpfCnpj;
            this.EnderecoSacado = endereco;
        }
    }
}
