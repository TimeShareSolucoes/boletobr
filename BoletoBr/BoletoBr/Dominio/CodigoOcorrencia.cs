using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;
using BoletoBr.Interfaces;

namespace BoletoBr.Dominio
{
    public class CodigoOcorrencia : ICodigoOcorrencia
    {
        public int Codigo { get; set; }
        public int QtdDias { get; set; }
        public double Valor { get; set; }
        public string Descricao { get; set; }

        public CodigoOcorrencia(int codigo)
        {
            this.Codigo = codigo;
        }

        public static string ValidaCodigo(ICodigoOcorrencia ocorrencia)
        {
            try
            {
                return ocorrencia.Codigo.ToString();
            }
            catch
            {
                return "0";
            }
        }
    }
}
