using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;
using BoletoBr.Bancos.Banrisul;
using BoletoBr.Bancos.Brasil;
using BoletoBr.Bancos.Daycoval;
using BoletoBr.Interfaces;

namespace BoletoBr.Fabricas
{
    public static class BancoFactory
    {
        public static IBanco ObterBanco(string codigoBanco, string digitoBanco = "")
        {
            try
            {
                switch (codigoBanco)
                {
                    /* 001 - Banco do Brasil */
                    case "001":
                        return new BancoBrasil();
                        break;
                    /* 003 - Banco da Amazônia */
                    case "003":
                        return new Bancos.Amazonia.BancoAmazonia();
                        break;
                    case "033":
                        return new Bancos.Santander.BancoSantander();
                    /* 104 - Caixa */
                    case "104":
                        return new Bancos.Cef.BancoCef();
                        break;
                    /* 237 - Bradesco */
                    case "237":
                        return new Bancos.Bradesco.BancoBradesco();
                        break;
                    /* 341 - Itaú */
                    case "341":
                        return new Bancos.Itau.BancoItau();
                        break;
                    /* 399 - HSBC */
                    case "399":
                        return new Bancos.Hsbc.BancoHsbc();
                        break;
                    /* 070 - BRB */
                    case "070":
                        return new Bancos.BRB.BancoBRB();
                        break;
                    case "756":
                        return new Bancos.Sicoob.BancoSicoob();
                        break;
                    /* 422 - Safra*/
                    case "422":
                        return new Bancos.Safra.BancoSafra();
                    /* 707 - BANCO DAYCOVAL */
                    case "707":
                        return new Bancos.Daycoval.BancoDaycoval();
                    /* 041 - BANCO BANRISUL */
                    case "041":
                        return new BancoBanrisul();
                    default:
                        throw new NotImplementedException("Banco " + codigoBanco +
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
