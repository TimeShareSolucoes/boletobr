using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.CodigoRejeicao
{
    public interface ICodigoRejeicao
    {
        IBanco Banco { get; }
        int Codigo { get; set; }
        string Descricao { get; }
    }
}
