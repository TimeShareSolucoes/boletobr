using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;
using BoletoBr.Dominio.CodigoMovimento;

namespace BoletoBr.Dominio
{
    public class CodigoOcorrencia : ICodigoOcorrencia
    {
        public int Codigo { get; set; }
        public int QtdDias { get; set; }
        public double Valor { get; set; }
        public string Descricao { get; set; }
    }
}
