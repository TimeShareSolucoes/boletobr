using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Amazonia
{
    public class CarteiraCobrancaBancoAmazoniaCsb : CarteiraCobranca
    {
        public CarteiraCobrancaBancoAmazoniaCsb()
        {
            this.Codigo = "CSB";
            this.Tipo = "";
            this.Descricao = "CSB Cobrança Simples HSBC";
        }
    }
}
