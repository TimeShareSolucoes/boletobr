using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Cef
{
    public class CarteiraCobrancaCefSr : CarteiraCobranca
    {
        public CarteiraCobrancaCefSr()
        {
            this.Codigo = "SR";
            this.Tipo = "";
            this.Descricao = "Cobrança Sem Registro";
        }
    }
}
