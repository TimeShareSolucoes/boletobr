using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Bradesco
{
    public class CarteiraCobrancaBradesco02 : BoletoBr.CarteiraCobranca
    {
        public CarteiraCobrancaBradesco02()
        {
            this.Codigo = "02";
            this.Tipo = "";
            this.Descricao = "Carteira 02";
        }
    }
}
