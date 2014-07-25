using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public class TrailerRetornoCnab400
    {
        public int CodigoDoRegistro { get; set; }
        public int CodigoDeRetorno { get; set; }
        public string CodigoDoServico { get; set; }
        public string CodigoDoBanco { get; set; }
        public int NumeroSequencial { get; set; }

        #region Banco HSBC

        /// <summary>
        /// Faz a leitura do Trailer para arquivos do formato CNAB 400
        /// Bancos:
        /// HSBC
        /// </summary>
        /// <param name="registro"></param>
        public void LerTrailerRetornoCnab400Hsbc(string registro)
        {
            try
            {
                CodigoDoRegistro = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 1, 1));
                CodigoDeRetorno = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 2, 2));
                CodigoDoServico = MetodosExtensao.ExtrairValorDaLinha(registro, 3, 4);
                CodigoDoBanco = MetodosExtensao.ExtrairValorDaLinha(registro, 5, 7);
                // 8 - 394 -> 387 brancos
                NumeroSequencial = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 395, 400));
            }
            catch (Exception)
            {
                throw new Exception("Erro ao ler trailer do arquivo de RETORNO / CNAB 400.");
            }
        }

        #endregion
    }
}
