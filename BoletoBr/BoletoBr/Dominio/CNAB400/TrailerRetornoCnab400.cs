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
        public int CodigoDoServico { get; set; }
        public int CodigoDoBanco { get; set; }
        public int NumeroSequencial { get; set; }

        #region Banco HSBC

        public void LerTrailerRetornoCnab400Hsbc(string registro)
        {
            try
            {
                int dataGravacao = Convert.ToInt32(registro.Substring(95, 6));

                CodigoDoRegistro = Convert.ToInt32(registro.Substring(0, 1));
                CodigoDeRetorno = Convert.ToInt32(registro.Substring(2, 1));
                CodigoDoServico = Convert.ToInt32(registro.Substring(3, 2));
                CodigoDoBanco = Convert.ToInt32(registro.Substring(5, 3));
                // 8 - 394 -> 387 brancos
                NumeroSequencial = Convert.ToInt32(registro.Substring(395, 6));
            }
            catch (Exception)
            {
                throw new Exception("Erro ao ler trailer do arquivo de RETORNO / CNAB 400.");
            }
        }

        #endregion
    }
}
