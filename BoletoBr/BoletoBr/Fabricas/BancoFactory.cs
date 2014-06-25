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
            try
            {
                switch (codigoBanco)
                {
                    //001 - Banco do Brasil
                    case "001":
                        return new Bancos.BancoBrasil.BancoBrasil();
                        break;
                    //003 - Banco da Amazônia
                    case "003":
                        return new Bancos.Amazonia.BancoAmazonia();
                        break;
                    //104 - Caixa
                    case "104":
                        return new Bancos.Cef.BancoCef();
                        break;
                    //237 - Bradesco
                    case "237":
                        return new Bancos.Bradesco.BancoBradesco();
                        break;

                    //399 - HSBC
                    case "399":
                        return new Bancos.Hsbc.BancoHsbc();
                        break;

                    default:
                        throw new NotImplementedException("Banco " + codigoBanco + "-" + digitoBanco +
                                              " ainda não foi implementado.");
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("Erro durante a execução da transação.", ex);
            }
        }
    }
}
