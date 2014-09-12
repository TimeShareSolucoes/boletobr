using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public class Endereco
    {
        /// <summary>
        /// Ex: Rua, Av, Travessa
        /// </summary>
        /// <remarks>
        /// A abreviação é interessante, devido a limitação de caracteres em arquivos de remessa.
        /// </remarks>
        public string TipoLogradouro { get; set; }
        /// <summary>
        /// Nome do logradouro
        /// </summary>
        /// <remarks>
        /// http://www.significados.com.br/logradouro/
        /// </remarks>
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }

        public string LogradouroNumeroComplementoConcatenado
        {
            get
            {
                var textoRetornar = "";
                textoRetornar += Logradouro;
                if (String.IsNullOrEmpty(textoRetornar) == false)
                {
                    textoRetornar += ", ";
                }
                textoRetornar += Numero;
                if (String.IsNullOrEmpty(textoRetornar) == false)
                {
                    textoRetornar += ", ";
                }
                textoRetornar += Complemento;

                return textoRetornar;
            }
        }
        /// <summary>
        /// Sigla do UF
        /// Ex:
        /// GO, SP, MG, PR etc.
        /// </summary>
        public string SiglaUf { get; set; }
        public string Cep { get; set; }
        public string CepFormatado
        {
            get
            {
                var valorATratar = Cep;
                valorATratar = valorATratar.Replace(".", "").Replace("-", "");
                return valorATratar.BoletoBrSetMascara("##.###-###");
            }
        }

        public string CepSemFormatacao
        {
            get { return Cep.BoletoBrToBind().Replace(".", "").Replace("-", ""); }
        }
    }
}
