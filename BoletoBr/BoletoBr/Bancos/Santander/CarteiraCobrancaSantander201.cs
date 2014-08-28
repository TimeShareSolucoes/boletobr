using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Santander
{
    public class CarteiraCobrancaSantander201 : CarteiraCobranca
    {
        public CarteiraCobrancaSantander201()
        {
            this.Codigo = "201";
            this.Tipo = "";
            this.Descricao = "Penhor Rápida com Registro";
        }
    }
}
