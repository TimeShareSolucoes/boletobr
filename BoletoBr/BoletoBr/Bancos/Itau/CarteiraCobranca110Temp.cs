using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Itau
{
    public class CarteiraCobranca110Temp : CarteiraCobranca
    {
        /*
         * Carteira incluida somente para TESTES do banco Itau
         * Seguindo documentação técnica do banco
         */
        public CarteiraCobranca110Temp()
        {
            this.Codigo = "110";
            this.Tipo = "";
            this.Descricao = "Carteira 110";
        }
    }
}
