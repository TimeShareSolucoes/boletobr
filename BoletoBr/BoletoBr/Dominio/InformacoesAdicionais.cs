using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Dominio
{
    /// <summary>
    /// Informações Adicionais para uso livre no boleto
    /// </summary>
    public class InformacoesAdicionais
    {
        public string ProdutoServico { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnit { get; set; }
        public decimal ValorTotal { get; set; }
        public string ValorTotalExtenso { get; set; }
        public string InfoAdicionais { get; set; }
    }
}
