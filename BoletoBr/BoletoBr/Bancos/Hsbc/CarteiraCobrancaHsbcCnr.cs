using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Hsbc
{
    public class CarteiraCobrancaHsbcCnr : CarteiraCobranca
    {
        public CarteiraCobrancaHsbcCnr()
        {
            this.Codigo = "CNR";
            this.Tipo = "";
            this.Descricao = "CNR Cobrança Não Registrada HSBC";
        }
    }
}
