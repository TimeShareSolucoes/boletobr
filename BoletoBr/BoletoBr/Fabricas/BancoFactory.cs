using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Fabricas
{
    public static class BancoFactory
    {
        public static IBanco ObterBanco(string codigoBanco, string digitoBanco)
        {
            if (codigoBanco == "399" && digitoBanco == "9")
                return new Bancos.Hsbc.BancoHsbc();

            throw new NotImplementedException("Banco " + codigoBanco + "-" + digitoBanco +
                                              " ainda não foi implementado.");
        }
    }
}
