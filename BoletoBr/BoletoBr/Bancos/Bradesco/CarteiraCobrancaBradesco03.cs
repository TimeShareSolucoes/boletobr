using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Bradesco
{
    public class CarteiraCobrancaBradesco03 : BoletoBr.CarteiraCobranca
    {
        public CarteiraCobrancaBradesco03()
        {
            this.Codigo = "03";
            this.Tipo = "";
            this.Descricao = "Carteira 03";
        }
    }
}
