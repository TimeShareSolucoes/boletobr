using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public class TrailerRetornoCnab400
    {
        public int TipoDeRegistro { get; set; }
        public int CodigoArquivo { get; set; }
        public int CodigoDoServico { get; set; }
        public int CodigoBanco { get; set; }
        public int BancoTamanho10 { get; set; }
        public int QuantidaDeTitulos { get; set; }
        public int ValorDosTitulos { get; set; }
        public int BancoTamanho355 { get; set; }
        public int NumeroSequencial { get; set; }
    }
}
