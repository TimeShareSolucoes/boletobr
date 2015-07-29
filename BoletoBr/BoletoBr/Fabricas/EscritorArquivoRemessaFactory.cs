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
                switch (remessaEscrever.Header.CodigoBanco.PadLeft(3, '0'))
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
        public static IEscritorArquivoRemessaCnab400 ObterEscritorRemessa(RemessaCnab400 remessaEscrever)
        {
            try
            {
                switch (remessaEscrever.Header.CodigoBanco.PadLeft(3, '0'))
                {
                        /* 001 - Banco do Brasil */
                    case "001":
                        return new Bancos.Brasil.EscritorRemessaCnab400BancoDoBrasil(remessaEscrever);
                        break;
                        /* 003 - Banco da Amazônia */
                    case "003":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco +
                                                          " ainda não foi implementado.");
                        break;
                    case "033":
                        return new Bancos.Santander.EscritorRemessaCnab400Santander(remessaEscrever);
                        /* 104 - Caixa */
                    case "104":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco +
                                                          " ainda não foi implementado.");
                        break;
                        /* 237 - Bradesco */
                    case "237":
                        return new Bancos.Bradesco.EscritorRemessaCnab400Bradesco(remessaEscrever);
                        break;
                        /* 341 - Itaú */
                    case "341":
                        return new Bancos.Itau.EscritorRemessaCnab400Itau(remessaEscrever);
                        break;
                        /* 399 - HSBC */
                    case "399":
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco +
                                                          " ainda não foi implementado.");
                        break;
                        /* 070 - BRB */
                    case "070":
                        return new Bancos.BRB.EscritorRemessaCnab400BRB(remessaEscrever);
                        break;
                    default:
                        throw new NotImplementedException("Banco " + remessaEscrever.Header.CodigoBanco +
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
