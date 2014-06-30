using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Cef
{
    class CarteiraCobrancaCefRg : CarteiraCobranca
    {
        public CarteiraCobrancaCefRg()
        {
            this.Codigo = "RG";
            this.Tipo = "";
            this.Descricao = "Cobrança Registrada";
        }
    }
}
