using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BoletoBr
{
    public class HeaderRetornoCnab400
    {
        public int CodigoDoRegistro { get; set; }
        public int CodigoDeRetorno { get; set; }
        public string LiteralRetorno { get; set; }
        public string CodigoDoServico { get; set; }
        public string LiteralServico { get; set; }
        public int CodigoAgenciaCedente { get; set; }
        public string Constante { get; set; }
        public long ContaCorrente { get; set; }
        public string TipoRetorno { get; set; }
        public string NomeDoBeneficiario { get; set; }
        public string CodigoDoBanco { get; set; }
        public string NomeDoBanco { get; set; }
        public int DataGravacao { get; set; }
        public string Densidade { get; set; }
        public string LiteralDensidade { get; set; }
        public int CodigoDoBeneficiario { get; set; }
        public string NomeAgencia { get; set; }
        public int CodigoFormulario { get; set; }
        public string Volser { get; set; }
        public string NumeroSequencial { get; set; }

        #region Banco HSBC

        /// <summary>
        /// Faz a leitura do Header para arquivos do formato CNAB 400
        /// Bancos:
        /// HSBC
        /// </summary>
        /// <param name="registro"></param>
        public void LerHeaderRetornoCnab400Hsbc(string registro)
        {
            try
            {
                int dataGravacao = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 95, 100));

                CodigoDoRegistro = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 1, 1));
                CodigoDeRetorno = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 2, 2));
                LiteralRetorno = MetodosExtensao.ExtrairValorDaLinha(registro, 3, 9);
                CodigoDoServico = MetodosExtensao.ExtrairValorDaLinha(registro, 10, 11);
                LiteralServico = MetodosExtensao.ExtrairValorDaLinha(registro, 12, 26);
                CodigoAgenciaCedente = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 27, 31));
                Constante = MetodosExtensao.ExtrairValorDaLinha(registro, 32, 33);
                ContaCorrente = Convert.ToInt64(MetodosExtensao.ExtrairValorDaLinha(registro, 34, 44));
                TipoRetorno = MetodosExtensao.ExtrairValorDaLinha(registro, 45, 45);
                // 46 -> 1 branco
                NomeDoBeneficiario = MetodosExtensao.ExtrairValorDaLinha(registro, 47, 76);
                CodigoDoBanco = MetodosExtensao.ExtrairValorDaLinha(registro, 77, 79);
                NomeDoBanco = MetodosExtensao.ExtrairValorDaLinha(registro, 80, 94);
                DataGravacao = dataGravacao;
                Densidade = MetodosExtensao.ExtrairValorDaLinha(registro, 101, 105);
                LiteralDensidade = MetodosExtensao.ExtrairValorDaLinha(registro, 106, 108);
                CodigoDoBeneficiario = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 109, 118));
                NomeAgencia = MetodosExtensao.ExtrairValorDaLinha(registro, 119, 138);
                CodigoFormulario = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 139, 142));
                // 143 - 388 -> 246 brancos
                Volser = MetodosExtensao.ExtrairValorDaLinha(registro, 389, 394);
                NumeroSequencial = MetodosExtensao.ExtrairValorDaLinha(registro, 395, 400);
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

