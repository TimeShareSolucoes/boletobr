using System;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Interfaces;

namespace BoletoBr.Fabricas
{
    public static class EscritorArquivoRemessaFactory
    {
        public static IEscritorArquivoRemessaCnab240 ObterEscritorRemessa(RemessaCnab240 remessaEscrever)
        {
            try
            {
                switch (remessaEscrever.Header.CodigoBanco.ToString("000"))
                {
                    /* 001 - Banco do Brasil */
                    case "001":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                        break;
                    /* 003 - Banco da Amazônia */
                    case "003":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                        break;
                    case "033":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                    /* 104 - Caixa */
                    case "104":
                        return new Bancos.Cef.EscritorRemessaCnab240CefSicgb(remessaEscrever);
                        break;
                    /* 237 - Bradesco */
                    case "237":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                        break;
                    /* 341 - Itaú */
                    case "341":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                        break;
                    /* 399 - HSBC */
                    case "399":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                        break;
                    default:
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro durante a execução da transação.", ex);
            }
        }
        public static IEscritorArquivoRemessaCnab240 ObterEscritorRemessa(RemessaCnab400 remessaEscrever)
        {
            try
            {
                switch (remessaEscrever.Header.CodigoBanco.ToString("000"))
                {
                    /* 001 - Banco do Brasil */
                    case "001":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                        break;
                    /* 003 - Banco da Amazônia */
                    case "003":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                        break;
                    case "033":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                    /* 104 - Caixa */
                    case "104":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                        break;
                    /* 237 - Bradesco */
                    case "237":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                        break;
                    /* 341 - Itaú */
                    case "341":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                        break;
                    /* 399 - HSBC */
                    case "399":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                        break;
                    default:
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco + " ainda não foi implementado.");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro durante a execução da transação.", ex);
            }
        }
    }
}
