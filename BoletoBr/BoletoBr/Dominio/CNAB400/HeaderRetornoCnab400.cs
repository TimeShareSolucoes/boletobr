using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BoletoBr
{
    public class HeaderRetornoCnab400
    {
        #region Propriedades

        public int CodigoDoRegistro { get; set; }
        public int CodigoDeRetorno { get; set; }
        public string LiteralRetorno { get; set; }
        public int CodigoDoServico { get; set; }
        public string LiteralServico { get; set; }
        public int CodigoAgenciaCedente { get; set; }
        public string Constante { get; set; }
        public int ContaCorrente { get; set; }
        public string TipoRetorno { get; set; }
        /* public string Brancos { get; set; } */
        public string NomeDoBeneficiario { get; set; }
        public int CodigoDoBanco { get; set; }
        public string NomeDoBanco { get; set; }
        public DateTime DataGravacao { get; set; }
        public string Densidade { get; set; }
        public string LiteralDensidade { get; set; }
        public int CodigoDoBeneficiario { get; set; }
        public string NomeAgencia { get; set; }
        public int CodigoFormulario { get; set; }
        public string Volser { get; set; }
        public string NumeroSequencial { get; set; }

        #endregion

        #region Banco HSBC

        public void LerHeaderRetornoCnab400Hsbc(string registro)
        {
            try
            {
                int dataGravacao = Convert.ToInt32(registro.Substring(95, 6));

                CodigoDoRegistro = Convert.ToInt32(registro.Substring(0, 1));
                CodigoDeRetorno = Convert.ToInt32(registro.Substring(2, 1));
                LiteralRetorno = registro.Substring(3, 7);
                CodigoDoServico = Convert.ToInt32(registro.Substring(10, 2));
                LiteralServico = registro.Substring(12, 15);
                CodigoAgenciaCedente = Convert.ToInt32(registro.Substring(27, 5));
                Constante = registro.Substring(32, 2);
                ContaCorrente = Convert.ToInt32(registro.Substring(34, 11));
                TipoRetorno = registro.Substring(45, 1);
                // 46 -> 1 branco
                NomeDoBeneficiario = registro.Substring(47, 30);
                CodigoDoBanco = Convert.ToInt32(registro.Substring(77, 3));
                NomeDoBanco = registro.Substring(80, 15);
                DataGravacao = Convert.ToDateTime(dataGravacao.ToString("##-##-##"));
                Densidade = registro.Substring(101, 5);
                LiteralDensidade = registro.Substring(106, 3);
                CodigoDoBeneficiario = Convert.ToInt32(registro.Substring(109, 10));
                NomeAgencia = registro.Substring(119, 20);
                CodigoFormulario = Convert.ToInt32(registro.Substring(139, 4));
                // 143 - 388 -> 246 brancos
                Volser = registro.Substring(389, 6);
                NumeroSequencial = registro.Substring(395, 6);
            }
            catch (Exception)
            {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 400.");
            }
        }

        public static string PrimeiroCaracter(string retorno)
        {
            try
            {
                return retorno.Substring(0, 1);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao desmembrar registro.", ex);
            }
        }
        #endregion
    }
}

