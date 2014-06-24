using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Amazonia
{
    public class CarteiraCobrancaBancoAmazoniaCnr : CarteiraCobranca 
    {
        public CarteiraCobrancaBancoAmazoniaCnr()
        {
        this.Codigo = "CNR";
        this.Tipo = "";
        this.Descricao = "CNR Cobrança Não Registrada Banco da Amazônia";
        }
    }
}
