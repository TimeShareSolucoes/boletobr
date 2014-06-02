using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Bradesco
{
    public class CarteiraCobrancaBradesco09 : BoletoBr.CarteiraCobranca
    {
        public CarteiraCobrancaBradesco09()
        {
            this.Codigo = "09";
            this.Tipo = "";
            this.Descricao = "Carteira 09";
        }
    }
}
