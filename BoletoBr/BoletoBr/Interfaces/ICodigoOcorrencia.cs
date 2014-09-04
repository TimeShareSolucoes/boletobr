using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.CodigoMovimento
{
    public interface ICodigoOcorrencia
    {
        int Codigo { get; set; }
        int QtdDias { get; set; }
        double Valor { get; set; }
        string Descricao { get; }
    }
}
