using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.CodigoLiquidacao
{
    public interface ICodigoLiquidacao
    {
        IBanco Banco { get; }
        int Enumerado { get; set; }
        string Codigo { get; set; }
        string Descricao { get; }
        string Recurso { get; }
    }
}
