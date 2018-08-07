using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.View.Win.Dominio
{
    public class Banco
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }

        public string CodigoDescricao => Codigo + " - " + Descricao;

        public static List<Banco> GetBancos()
        {
            var listaBanco = new List<Banco>
            {
                new Banco() {Codigo = "001", Descricao = "Banco do Brasil"},
                new Banco() {Codigo = "003", Descricao = "Banco da Amazônia"},
                new Banco() {Codigo = "033", Descricao = "Banco Santander Banespa"},
                new Banco() {Codigo = "070", Descricao = "Banco de Brasília – BRB"},
                new Banco() {Codigo = "104", Descricao = "Caixa Econômica Federal"},
                new Banco() {Codigo = "237", Descricao = "Bradesco"},
                new Banco() {Codigo = "341", Descricao = "Itau"},
                new Banco() {Codigo = "399", Descricao = "HSBC"},
                new Banco() {Codigo = "422", Descricao = "Banco Safra"},
                new Banco() {Codigo = "756", Descricao = "Banco Cooperativo do Brasil"},
                new Banco() {Codigo = "707", Descricao = "Banco Daycoval"}
            };

            return listaBanco;
        }
    }
}
