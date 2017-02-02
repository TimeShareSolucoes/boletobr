using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.View.Win.Dominio
{
    public class SacadoBoleto
    {
        public SacadoBoleto(string nome, string cpfcnpj, string logradouro, string complemento, string numero,
            string bairro, string cidade, string uf, string cep)
        {
            Nome = nome;
            CpfCnpj = cpfcnpj;
            Logradouro = logradouro;
            Complemento = complemento;
            Numero = numero;
            Bairro = bairro;
            Cidade = cidade;
            Uf = uf;
            Cep = cep;
        }

        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
    }
}
