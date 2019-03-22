using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Bancos.Safra;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Banestes
{
    public class BancoBanestes : IBanco
    {
        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        public Image LogotipoBancoParaExibicao { get; set; }
        public string LocalDePagamento { get; }
        public string MoedaBanco { get; }
        private int D1 { get; set; }


        private int _digitoAutoConferenciaBoleto;
        public BancoBanestes()
        {
            CodigoBanco = "021";
            DigitoBanco = "3";
            NomeBanco = "Banestes";
            LocalDePagamento = "PAGÁVEL PREFERENCIALMENTE NA REDE BANESTES";
            MoedaBanco = "R$";
        }

        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {

            if (boleto.CarteiraCobranca.Codigo != "11")
                throw new ValidacaoBoletoException(
                    "Carteira não implementada. Carteira implementada 11");/*01 - Desconto |11 - Simples |13 - Caucionada*/
        }

        public void FormataMoeda(Boleto boleto)
        {
            boleto.Moeda = MoedaBanco;

            if (String.IsNullOrEmpty(boleto.Moeda))
                throw new Exception("Espécie/Moeda para o boleto não foi informada.");

            if ((boleto.Moeda == "9") || (boleto.Moeda == "REAL") || (boleto.Moeda == "R$"))
                boleto.Moeda = "R$";
            else
                boleto.Moeda = "0";
        }

        public void FormatarBoleto(Boleto boleto)
        {
            //Atribui o local de pagamento
            boleto.LocalPagamento = this.LocalDePagamento;

            boleto.ValidaDadosEssenciaisDoBoleto();

            FormataNumeroDocumento(boleto);
            FormataNossoNumero(boleto);
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataMoeda(boleto);

            ValidaBoletoComNormasBanco(boleto);

            boleto.CedenteBoleto.CodigoCedenteFormatado = String.Format("{0}-{1}/{2}-{3}",
                boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                boleto.CedenteBoleto.ContaBancariaCedente.DigitoAgencia,
                boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);
        }
        private int CalculaDVNossoNumero(string nossoNumero, short peso = 9)
        {
            int S = 0;
            int P = 0;
            int N = 0;
            int d = 0;

            for (int i = 0; i < nossoNumero.Length; i++)
            {
                N = Convert.ToInt32(nossoNumero.Substring(i, 1));

                P = N * peso--;

                S += P;
            }

            int R = S % 11;

            if (R == 0 || R == 1)
                d = 0;

            if (R > 1)
                d = 11 - R;

            return d;
        }
        private int CalculaD1(string chave)
        {
            int D1 = 0;
            short peso = 2;
            int K = 0;
            int S = 0;

            for (int i = 0; i < chave.Length; i++)
            {
                int N = Convert.ToInt32(chave.Substring(i, 1));

                int P = N * peso;

                if (P > 9)
                    K = P - 9;

                if (P < 10)
                    K = P;

                S += K;

                if (peso == 2)
                    peso = 1;
                else
                    peso = 2;
            }

            int resto = S % 10;

            if (resto == 0)
                D1 = 0;
            else if (resto > 0)
                D1 = 10 - resto;

            return D1;
        }

        private int CalculaD2(string chave)
        {
            int D2 = 0;
            short peso = 7;
            int P = 0;
            int S = 0;
            
            string chaveD1 = string.Concat(chave, D1);

            for (int i = 0; i < chaveD1.Length; i++)
            {
                int N = Convert.ToInt32(chaveD1.Substring(i, 1));

                P = N * peso--;

                S += P;

                if (peso == 1)
                    peso = 7;
            }

            int resto = S % 11;

            if (resto == 0)
                D2 = 0;

            if (resto == 1)
            {
                D1++;
                if (D1 == 10)
                    D1 = 0;
                return CalculaD2(chave);
            }

            if (resto > 1)
                D2 = 11 - resto;

            return D2;
        }

        public string FormataCampoLivre(Boleto boleto)
        {
            try
            {
                boleto.CedenteBoleto.ContaBancariaCedente.Conta = boleto.CedenteBoleto.ContaBancariaCedente.Conta
                    .Replace(".", "").Replace("-", "");

                if (boleto.CedenteBoleto.ContaBancariaCedente.Conta.BoletoBrToInt() < 1)
                    throw new Exception("Conta Corrente inválida");
                if (boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta.BoletoBrToStringSafe().BoletoBrToInt() > 9)
                    throw new Exception("Digito Conta Corrente inválida");
                if (string.IsNullOrEmpty(boleto.CarteiraCobranca.Codigo))
                    throw new Exception("Carteira não informada");

                string NNNNNNNN = boleto.NossoNumeroFormatado.Substring(0, 8).PadLeft(8, '0');
                string CCCCCCCCCCD = $@"{boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(10, '0')}{boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta.PadLeft(1, '0')}";
                string R = "4"; /*Tipo Cobrança*/ /*4-Com Registro*/

                string NNNNNNNNCCCCCCCCCCCR021 = string.Concat(NNNNNNNN, CCCCCCCCCCD, R, "021");

                D1 = CalculaD1(NNNNNNNNCCCCCCCCCCCR021);
                int D2 = CalculaD2(NNNNNNNNCCCCCCCCCCCR021);

                return string.Concat(NNNNNNNNCCCCCCCCCCCR021, D1, D2);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Erro ao tentar gerar a chave ASBACE. {0}", e.Message));
            }
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            try
            {
                //0219DFFFFVVVVVVVVVVCCCCCCCCCCCCCCCCCCCCCCCCC

                var FFFF = Common.FatorVencimento(boleto.DataVencimento);
                var VVVVVVVVVV =  boleto.ValorBoleto.ToString("N").Replace(".", "").Replace(",", "").PadLeft(10, '0');


                string chave = string.Format("0219{0}{1}{2}", FFFF, VVVVVVVVVV, FormataCampoLivre(boleto));

                _digitoAutoConferenciaBoleto = Common.Mod11(chave, 9);

                boleto.CodigoBarraBoleto = string.Format("0219{0}{1}{2}{3}", _digitoAutoConferenciaBoleto, FFFF, VVVVVVVVVV, FormataCampoLivre(boleto));
                
                

                boleto.CodigoBarraBoleto = Common.Left(boleto.CodigoBarraBoleto, 4) + _digitoAutoConferenciaBoleto +
                                           Common.Right(boleto.CodigoBarraBoleto, 39); 
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar código de barras.", ex);
            }
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {
            try
            {
                int dv = 0;

                #region Parte 1

                var parte1 = string.Concat("0219", FormataCampoLivre(boleto).Substring(0, 5));

                dv = Common.Mod10(parte1);

                parte1 = string.Concat(parte1, dv);

                #endregion

                #region Parte 2

                var parte2 = FormataCampoLivre(boleto).Substring(5, 10);

                dv = Common.Mod10(parte2);

                parte2 = string.Concat(parte2, dv);

                #endregion

                #region Parte 3

                var parte3 = FormataCampoLivre(boleto).Substring(15, 10);

                dv = Common.Mod10(parte3);

                parte3 = string.Concat(parte3, dv);

                #endregion

                #region Parte 5

                var parte5 = string.Concat(Common.FatorVencimento(boleto.DataVencimento), boleto.ValorBoleto.ToString("N").Replace(".", "").Replace(",", "").PadLeft(10, '0'));

                #endregion
                
                boleto.LinhaDigitavelBoleto = string.Format("{0}.{1} {2}.{3} {4}.{5} {6} {7}",
                    parte1.Substring(0, 5),
                    parte1.Substring(5, 5),
                    parte2.Substring(0, 5),
                    parte2.Substring(5, 6),
                    parte3.Substring(0, 5),
                    parte3.Substring(5, 6),
                    _digitoAutoConferenciaBoleto,
                    parte5);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar linha digitável.", ex);
            }
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            try
            {
                var identificador = boleto.IdentificadorInternoBoleto.Trim().Replace(".", "").Replace("-", "");

                if (string.IsNullOrEmpty(identificador))
                    throw new Exception("Nosso Número não informado");

                if (identificador.Length > 8)
                    throw new Exception("Tamanho máximo para o Nosso Número são de 8 caracteres");

                identificador = identificador.PadLeft(8, '0');

                int D1 = CalculaDVNossoNumero(identificador);

                int D2 = CalculaDVNossoNumero(string.Concat(identificador, D1), 10);
                boleto.SetNossoNumeroFormatado(string.Format("{0}.{1}{2}", identificador, D1, D2));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar nosso número", ex);
            }
        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            if (String.IsNullOrEmpty(boleto.NumeroDocumento) || String.IsNullOrEmpty(boleto.NumeroDocumento.TrimStart('0')))
                throw new Exception("Número do Documento não foi informado.");

            boleto.NumeroDocumento = boleto.NumeroDocumento.PadLeft(10, '0');
        }

        public ICodigoOcorrencia ObtemCodigoOcorrenciaByInt(int numeroOcorrencia)
        {
            switch (numeroOcorrencia)
            {
                case 02:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 02,
                        Descricao = "ENTRADA CONFIRMADA"
                    };
                case 03:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 03,
                        Descricao = "ENTRADA REJEITADA"
                    };
                case 04:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 04,
                        Descricao = "TRANSFERÊNCIA DE CARTEIRA (ENTRADA)"
                    };
                case 05:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 05,
                        Descricao = "TRANSFERÊNCIA DE CARTEIRA (BAIXA)"
                    };
                case 06:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 06,
                        Descricao = "LIQUIDAÇÃO NORMAL"
                    };
                case 07:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 07,
                        Descricao = "CONFIRMAÇÃO DO RECEBIMENTO DO CANCELAMENTO DO DESCONTO"
                    };
                case 09:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 09,
                        Descricao = "BAIXADO AUTOMATICAMENTE"
                    };
                case 11:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 11,
                        Descricao = "TÍTULOS EM CARTEIRA (EM SER)"
                    };
                case 12:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 12,
                        Descricao = "ABATIMENTO CONCEDIDO"
                    };
                case 13:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 13,
                        Descricao = "ABATIMENTO CANCELADO"
                    };
                case 14:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 14,
                        Descricao = "VENCIMENTO ALTERADO"
                    };
                case 17:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 17,
                        Descricao = "LIQUIDAÇÃO APÓS BAIXA"
                    };
                case 19:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 19,
                        Descricao = "CONFIRMAÇÃO DE INSTRUÇÃO DE PROTESTO"
                    };
                case 20:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 20,
                        Descricao = "CONFIRMAÇÃO DE SUSTAR PROTESTO"
                    };
                case 21:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 21,
                        Descricao = "SOLICITAÇÃO 2VIA PROTESTO"
                    };
                case 22:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 22,
                        Descricao = "2VIA PROTESTO EMITIDA CARTÓRIO"
                    };
                case 23:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 23,
                        Descricao = "TÍTULO ENVIADO A CARTÓRIO"
                    };
                case 24:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 24,
                        Descricao = "RETIRADA DE CARTÓRIO E MANUTENÇÃO EM CARTEIRA"
                    };
                 case 25:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 25,
                        Descricao = "PROTESTADO E BAIXADO"
                    };
                case 26:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 26,
                        Descricao = "INSTRUÇÃO REJEITADA"
                    };
                case 27:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 27,
                        Descricao = "CONFIRMAÇÃO PEDIDO DE ALTERAÇÃO"
                    };
                case 28:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 28,
                        Descricao = "DÉBITO TARIFAS/CUSTAS"
                    };
                case 30:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 30,
                        Descricao = "ALTERAÇÃO DE DADOS REJEITADA"
                    };
                case 40:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 40,
                        Descricao = "CONFIRMAÇÃO DE ALTERAÇÃO DO NUMERO DO TITULO"
                    };
                case 42:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 42,
                        Descricao = "CONFIRMAÇÃO DE ALTERAÇÃO DOS DADOS DO SACADO"
                    };
                case 43:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 43,
                        Descricao = "CONFIRMAÇÃO DA ALTERAÇÃO DOS DADOS DO SACADOR AVALISTA"
                    };
                case 51:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 51,
                        Descricao = "TITULO RECONHECIDO PELO SACADO"
                    };
                case 52:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 52,
                        Descricao = "TITULO NÃO RECONHECIDO PELO SACADO"
                    };
                case 53:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 53,
                        Descricao = "TITULO RECUSADO PELA CIP"
                    };
                case 98:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 98,
                        Descricao = "INSTRUÇÃO DE PROTESTO PROCESSADA"
                    };
                case 99:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 99,
                        Descricao = "REMESSA REJEITADA"
                    };
                default:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = numeroOcorrencia,
                        Descricao = "Código de ocorrência não encontrado, n° ".ToUpper() + numeroOcorrencia
                    };
            }
        }

        public ICodigoOcorrencia ObtemCodigoOcorrencia(EnumCodigoOcorrenciaRemessa ocorrencia, double valorOcorrencia,
            DateTime dataOcorrencia)
        {
            switch (ocorrencia)
            {
                case EnumCodigoOcorrenciaRemessa.Registro:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 01,
                            Descricao = "Remessa"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.Baixa:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 02,
                            Descricao = "Pedido de baixa"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.ConcessaoDeAbatimento:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 04,
                            Descricao = "Concessão de abatimento"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDeAbatimento:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 05,
                            Descricao = "Cancelamento de abatimento concedido"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeVencimento:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 06,
                            Descricao = "Alteração de vencimento"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDoUsoDaEmpresa:
                {
                    return new CodigoOcorrencia((int)ocorrencia)
                    {
                        Codigo = 07,
                        Descricao = "Alteração do uso da empresa"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoSeuNumero:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 08,
                            Descricao = "Alteração de seu número"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.Protesto:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 09,
                            Descricao = "Pedido de Protesto"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.NaoProtestar:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 10,
                            Descricao = "Não Protestar"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.NaoCobrarJurosDeMora:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 11,
                            Descricao = "Não Cobrar Juros de Mora"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.ConcessaoDeDesconto:
                {
                    return new CodigoOcorrencia((int)ocorrencia)
                    {
                        Codigo = 12,
                        Descricao = "Concessão de desconto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDeDesconto:
                {
                    return new CodigoOcorrencia((int)ocorrencia)
                    {
                        Codigo = 13,
                        Descricao = "Cancelamento de desconto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDoValorDeDesconto:
                {
                    return new CodigoOcorrencia((int)ocorrencia)
                    {
                        Codigo = 14,
                        Descricao = "Alteração do valor de desconto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeValorPercentualDeMulta:
                {
                    return new CodigoOcorrencia((int)ocorrencia)
                    {
                        Codigo = 15,
                        Descricao = "Alteração Multa"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.DispensarCobrancaDeMulta:
                {
                    return new CodigoOcorrencia((int)ocorrencia)
                    {
                        Codigo = 16,
                        Descricao = "Dispensar cobrança multa"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeJurosDeMora:
                {
                    return new CodigoOcorrencia((int)ocorrencia)
                    {
                        Codigo = 17,
                        Descricao = "Alteração juros de mora"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.SustarProtesto:
                {
                    return new CodigoOcorrencia((int)ocorrencia)
                    {
                        Codigo = 18,
                        Descricao = "Sustar Protesto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDoValorDeAbatimento:
                {
                    return new CodigoOcorrencia((int)ocorrencia)
                    {
                        Codigo = 19,
                        Descricao = "Alteração valor abatimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.SustarProtestoEBaixarTitulo:
                {
                    return new CodigoOcorrencia((int)ocorrencia)
                    {
                        Codigo = 20,
                        Descricao = "Sustar protesto e baixar titulo"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlterarDadosSacado:
                {
                    return new CodigoOcorrencia((int)ocorrencia)
                    {
                        Codigo = 23,
                        Descricao = "Alterar dados sacado"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlterarDadosSacadorAvalista:
                {
                    return new CodigoOcorrencia((int)ocorrencia)
                    {
                        Codigo = 24,
                        Descricao = "Incluir/Alterar dados do sacador avalista"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.BaixaPorTerSidoPagoDiretamenteAoCedente:
                {
                    return new CodigoOcorrencia((int)ocorrencia)
                    {
                        Codigo = 34,
                        Descricao = "Pedido de baixa"
                    };
                }
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter Código de Comando/Movimento/Ocorrência. Banco: {0} Código: {1}",
                    CodigoBanco, ocorrencia.ToString()));
        }

        public ICodigoOcorrencia ObtemCodigoOcorrencia(EnumCodigoOcorrenciaRetorno ocorrenciaRetorno)
        {
            throw new NotImplementedException();
        }

        public IEspecieDocumento ObtemEspecieDocumento(EnumEspecieDocumento especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento.DuplicataMercantil:
                {
                    return new EspecieDocumento((int)especie)
                    {
                        Codigo = 01,
                        Descricao = "DUPLICATA MERCANTIL",
                        Sigla = "DM"
                    };
                }
                case EnumEspecieDocumento.NotaPromissoria:
                {
                    return new EspecieDocumento((int)especie)
                    {
                        Codigo = 02,
                        Descricao = "NOTA PROMISSÓRIA",
                        Sigla = "NP"
                    };
                }
                case EnumEspecieDocumento.NotaDeSeguro:
                {
                    return new EspecieDocumento((int)especie)
                    {
                        Codigo = 03,
                        Descricao = "NOTA DE SEGURO",
                        Sigla = "NS"
                    };
                }
                case EnumEspecieDocumento.CobrancaSeriada:
                {
                    return new EspecieDocumento((int)especie)
                    {
                        Codigo = 05,
                        Descricao = "COBRANÇA SERIADA",
                        Sigla = "CS"
                    };
                }
                case EnumEspecieDocumento.Recibo:
                {
                    return new EspecieDocumento((int)especie)
                    {
                        Codigo = 05,
                        Descricao = "RECIBO",
                        Sigla = "RC"
                    };
                }
                case EnumEspecieDocumento.LetraCambio:
                {
                    return new EspecieDocumento((int)especie)
                    {
                        Codigo = 10,
                        Descricao = "LETRA DE CÂMBIO",
                        Sigla = "LC"
                    };
                }
                case EnumEspecieDocumento.DuplicataServico:
                {
                    return new EspecieDocumento((int)especie)
                    {
                        Codigo = 11,
                        Descricao = "DUPLICATA DE SERVIÇOS",
                        Sigla = "DS"
                    };
                }
                case EnumEspecieDocumento.Outros:
                {
                    return new EspecieDocumento((int)especie)
                    {
                        Codigo = 99,
                        Descricao = "OUTROS CASOS",
                        Sigla = "OC"
                    };
                }
            }

            throw new Exception(
                String.Format("Não foi possível obter instrução padronizada. Banco: {0} Código Espécie: {1}",
                    CodigoBanco, especie.ToString()));
        }

        public IInstrucao ObtemInstrucaoPadronizada(EnumTipoInstrucao tipoInstrucao, double valorInstrucao, DateTime dataInstrucao,
            int diasInstrucao)
        {
            switch (tipoInstrucao)
            {
                case EnumTipoInstrucao.NaoReceberPrincipalSemJurosdeMora:
                    {
                        return new InstrucaoPadronizada
                        {
                            Codigo = 01,
                            QtdDias = diasInstrucao,
                            Valor = valorInstrucao,
                            TextoInstrucao = "NÃO RECEBER PRINCIPAL, SEM JUROS DE MORA"
                        };
                    }
                case EnumTipoInstrucao.DevolverSenaoPagoAte15DiasAposVencimento:
                    {
                        return new InstrucaoPadronizada
                        {
                            Codigo = 02,
                            QtdDias = diasInstrucao,
                            Valor = valorInstrucao,
                            TextoInstrucao = "DEVOLVER, SE NÃO PAGO, ATÉ 15 DIAS APÓS O VENCIMENTO"
                        };
                    }
                case EnumTipoInstrucao.DevolverSenaoPagoAte30DiasAposVencimento:
                    {
                        return new InstrucaoPadronizada
                        {
                            Codigo = 03,
                            QtdDias = diasInstrucao,
                            Valor = valorInstrucao,
                            TextoInstrucao = "DEVOLVER, SE NÃO PAGO, ATÉ 30 DIAS APÓS O VENCIMENTO"
                        };
                    }
                case EnumTipoInstrucao.NaoProtestar:
                    {
                        return new InstrucaoPadronizada
                        {
                            Codigo = 07,
                            QtdDias = diasInstrucao,
                            Valor = valorInstrucao,
                            TextoInstrucao = "NÃO PROTESTAR"
                        };
                    }
                case EnumTipoInstrucao.NaoCobrarJurosDeMora:
                    {
                        return new InstrucaoPadronizada
                        {
                            Codigo = 08,
                            QtdDias = diasInstrucao,
                            Valor = valorInstrucao,
                            TextoInstrucao = "NÃO COBRAR JUROS DE MORA"
                        };
                    }
                case EnumTipoInstrucao.MultaVencimento:
                    {
                        return new InstrucaoPadronizada
                        {
                            Codigo = 16,
                            QtdDias = diasInstrucao,
                            Valor = valorInstrucao,
                            TextoInstrucao = "MULTA"
                        };
                    }
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter instrução padronizada. Banco: {0} Código Instrução: {1} Qtd Dias/Valor: {2}",
                    CodigoBanco, tipoInstrucao.ToString(), valorInstrucao));
        }

        public RetornoGenerico LerArquivoRetorno(List<string> linhasArquivo)
        {
            if (linhasArquivo == null || linhasArquivo.Any() == false)
                throw new ApplicationException("Arquivo informado é inválido/Não existem títulos no retorno.");

            /* Identifica o layout: 400 */
            if (linhasArquivo.First().Length == 400)
            {
                var leitor = new LeitorRetornoCnab400Banestes(linhasArquivo);
                var retornoProcessado = leitor.ProcessarRetorno(TipoArquivo.Cnab400);

                var objRetornar = new RetornoGenerico(retornoProcessado);
                return objRetornar;
            }

            throw new Exception("Arquivo de RETORNO com " + linhasArquivo.First().Length + " posições, não é suportado.");
        }

        public RemessaCnab240 GerarArquivoRemessaCnab240(List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }

        public RemessaCnab400 GerarArquivoRemessaCnab400(List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }

        public int CodigoJurosMora(CodigoJurosMora codigoJurosMora)
        {
            return 0;
        }

        public int CodigoProteso(bool protestar = true)
        {
            return 0;
        }
    }
}
