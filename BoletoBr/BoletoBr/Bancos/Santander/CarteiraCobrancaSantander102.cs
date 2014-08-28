using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Santander
{
    public class CarteiraCobrancaSantander102 : CarteiraCobranca
    {
        public CarteiraCobrancaSantander102()
        {
            this.Codigo = "102";
            this.Tipo = "";
            this.Descricao = "Cobrança Simples SEM Registro";
        }
    }
}
