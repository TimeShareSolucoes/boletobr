using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.BRB
{
    public class BancoBRB : IBanco
    {
        public BancoBRB()
        {
            CodigoBanco = "070";
            DigitoBanco = "1";
            NomeBanco = "BRB";
            LocalDePagamento = "Pagável em qualquer banco até o vencimento.";
            MoedaBanco = "9";
        }

        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        public Image LogotipoBancoParaExibicao { get; set; }
        public string LocalDePagamento { get; private set; }
        public string MoedaBanco { get; private set; }

        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            try
            {
                //Verifica se o tamanho para o NossoNumero são 6 dígitos
                if (Convert.ToInt32(boleto.IdentificadorInternoBoleto).ToString().Length > 6)
                    throw new NotImplementedException("A quantidade de dígitos do nosso número para a carteira " +
                                                      boleto.CarteiraCobranca.Codigo + ", são 6 números.");

                //Verifica se data do processamento é valida
                if (boleto.DataProcessamento == DateTime.MinValue)
                    boleto.DataProcessamento = DateTime.Now;

                //Verifica se data do documento é valida
                if (boleto.DataDocumento == DateTime.MinValue)
                    boleto.DataDocumento = DateTime.Now;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao validar boleto (s).", ex);
            }
        }

        public void FormataMoeda(Boleto boleto)
        {
            try
            {
                boleto.Moeda = MoedaBanco;

                if (String.IsNullOrEmpty(boleto.Moeda))
                    throw new Exception("Espécie/Moeda para o boleto não foi informada.");

                if ((boleto.Moeda == "9") || (boleto.Moeda == "REAL") || (boleto.Moeda == "R$"))
                    boleto.Moeda = "R$";
                else
                    boleto.Moeda = "1";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("<BoletoBr>" +
                                                  "{0}Mensagem: Falha ao formatar moeda para o Banco: " +
                                                  CodigoBanco + " - " + NomeBanco, Environment.NewLine), ex);
            }
        }

        public void FormatarBoleto(Boleto boleto)
        {
            //Atribui o local de pagamento
            boleto.LocalPagamento = LocalDePagamento;

            boleto.ValidaDadosEssenciaisDoBoleto();

            FormataNumeroDocumento(boleto);
            FormataNossoNumero(boleto);
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataMoeda(boleto);

            ValidaBoletoComNormasBanco(boleto);

            boleto.CedenteBoleto.CodigoCedenteFormatado = String.Format("{0} - {1} - {2}",
                "000",
                boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(3, '0'),
                boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(6, '0') +
                boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            try
            {
                // Código de Barras
                //banco & moeda & fator & valor & carteira & nossonumero & dac_nossonumero & agencia & conta & dac_conta & "000"
                var valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");

                var BBB = CodigoBanco;
                var M = MoedaBanco;

                var FFFF = "0000";
                if (boleto.DataVencimento > new DateTime(1997, 10, 7, 0, 0, 0))
                    FFFF = Common.FatorVencimento(boleto.DataVencimento).BoletoBrToStringSafe();

                var VVVVVVVVVV = valorBoleto.PadLeft(10, '0');
                var CCCCCCCCCCCCCCCCCCCCCCCCC =
                    "000" + boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(3, '0') +
                    boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(6, '0') +
                    boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta +
                    boleto.NossoNumeroFormatado;

                var D = Common.Mod11Peso2a9(BBB + M + FFFF + VVVVVVVVVV + CCCCCCCCCCCCCCCCCCCCCCCCC);

                boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}", BBB, M, D, FFFF, VVVVVVVVVV,
                    CCCCCCCCCCCCCCCCCCCCCCCCC);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("<BoletoBr>" +
                                                  "{0}Mensagem: Falha ao formatar código de barras.",
                    Environment.NewLine), ex);
            }
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {
            try
            {
                var chave = "000" + boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(3, '0') +
                            boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(6, '0') +
                            boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta +
                            boleto.NossoNumeroFormatado;

                //1) GRUPO 1 (BBBMCCCCCD)
                var BBB = CodigoBanco;
                var M = MoedaBanco;
                var CCCCC = chave.ExtrairValorDaLinha(1, 5);
                var D1 = Common.Mod10(BBB + M + CCCCC);
                var BBBMCCCCCD = BBB + M + CCCCC + D1;

                //2) GRUPO 2 (CCCCCCCCCCD)
                var CCCCCCCCCC1 = chave.ExtrairValorDaLinha(6, 15);
                var D2 = Common.Mod10(CCCCCCCCCC1);
                var CCCCCCCCCCD1 = CCCCCCCCCC1 + D2;

                //3) GRUPO 3 (CCCCCCCCCCD)
                var CCCCCCCCCC2 = chave.ExtrairValorDaLinha(16, 25);
                var D3 = Common.Mod10(CCCCCCCCCC2);
                var CCCCCCCCCCD2 = CCCCCCCCCC2 + D3;

                var D4 = boleto.CodigoBarraBoleto.ExtrairValorDaLinha(6, 6);
                var FFFF = "0000";
                if (boleto.DataVencimento > new DateTime(1997, 10, 7, 0, 0, 0))
                    FFFF = Common.FatorVencimento(boleto.DataVencimento).BoletoBrToStringSafe();

                var valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
                var VVVVVVVVVV = valorBoleto.PadLeft(10, '0');

                boleto.LinhaDigitavelBoleto = string.Format("{0}.{1} {2}.{3} {4}.{5} {6} {7}",
                    BBBMCCCCCD.ExtrairValorDaLinha(1, 5),
                    BBBMCCCCCD.ExtrairValorDaLinha(6, 10),
                    CCCCCCCCCCD1.ExtrairValorDaLinha(1, 5),
                    CCCCCCCCCCD1.ExtrairValorDaLinha(6, 11),
                    CCCCCCCCCCD2.ExtrairValorDaLinha(1, 5),
                    CCCCCCCCCCD2.ExtrairValorDaLinha(6, 11),
                    D4,
                    FFFF + VVVVVVVVVV
                    );
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("<BoletoBr>" +
                                                  "{0}Mensagem: Falha ao formatar linha digitável." +
                                                  "{0}Carteira: " + boleto.CarteiraCobranca.Codigo +
                                                  "{0}Documento: " + boleto.NumeroDocumento, Environment.NewLine), ex);
            }
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            try
            {
                if (String.IsNullOrEmpty(boleto.IdentificadorInternoBoleto) ||
                    String.IsNullOrEmpty(boleto.IdentificadorInternoBoleto.TrimStart('0')))
                    throw new Exception("Sequencial Nosso Número não foi informado.");

                if (boleto.CarteiraCobranca.Tipo != "1" && boleto.CarteiraCobranca.Tipo != "2")
                    throw new Exception(
                        "Informe o tipo da carteira. 1 - COBRANÇA DIRETA SEM REGISTRO 2 - COBRANÇA DIRETA COM REGISTRO");

                if (String.IsNullOrEmpty(boleto.CedenteBoleto.ContaBancariaCedente.Agencia))
                    throw new Exception("Informe a Agencia da conta do cedente.");

                if (String.IsNullOrEmpty(boleto.CedenteBoleto.ContaBancariaCedente.Conta))
                    throw new Exception("Informe a Conta da conta do cedente.");

                if (String.IsNullOrEmpty(boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta))
                    throw new Exception("Informe o Dígito da conta do cedente.");

                var chave = "000" + boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(3, '0') +
                            boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(6, '0') +
                            boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta +
                            boleto.CarteiraCobranca.Tipo +
                            boleto.IdentificadorInternoBoleto.PadLeft(6, '0') +
                            CodigoBanco.PadLeft(3, '0');

                /*Calcular primeiro digito*/
                var D1 = Common.Mod10(chave);
                chave += D1;

                /*Calcular segundo digito*/
                var D2 = Common.Mod11Base7BRB(chave);
                chave += D2;

                boleto.SetNossoNumeroFormatado(string.Format("{0}{1}{2}{3}{4}", boleto.CarteiraCobranca.Tipo,
                    boleto.IdentificadorInternoBoleto.PadLeft(6, '0'), CodigoBanco.PadLeft(3, '0'), D1, D2));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("<BoletoBr>" +
                                                  "{0}Mensagem: Falha ao formatar nosso número." +
                                                  "{0}Carteira: " + boleto.CarteiraCobranca.Codigo +
                                                  "{0}Numeração Sequencial: " + boleto.NossoNumeroFormatado), ex);
            }
        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            boleto.NumeroDocumento = boleto.NumeroDocumento.PadLeft(6, '0');
        }

        public ICodigoOcorrencia ObtemCodigoOcorrenciaByInt(int numeroOcorrencia)
        {
            throw new NotImplementedException();
        }

        public ICodigoOcorrencia ObtemCodigoOcorrencia(EnumCodigoOcorrenciaRemessa ocorrencia,
            double valorOcorrencia,
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
                            Descricao = "Cancelamento de abatimento"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeVencimento:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 06,
                            Descricao = "Alteração do vencimento"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDoControleDoParticipante:
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
                            Descricao = "Protestar"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.NaoProtestar:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 10,
                            Descricao = "Não protestar"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.ProtestoParaFinsFalimentares:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 11,
                            Descricao = "Protesto para fins falimentares"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.SustarProtesto:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 18,
                            Descricao = "Sustar o protesto"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.ExclusaoDeSacadorAvalista:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 30,
                            Descricao = "Exclusão de sacador avalista"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeOutrosDados:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 31,
                            Descricao = "Alteração de outros dados"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.BaixaPorTerSidoPagoDiretamenteAoCedente:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 34,
                            Descricao = "Baixa por ter sido pago diretamente ao cedente"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDeInstrucao:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 35,
                            Descricao = "Cancelamento de instrução"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDoVencimentoESustarProtesto:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 37,
                            Descricao = "Alteração do vencimento e sustar protesto"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.CedenteNaoConcordaComAlegacaoDoSacado:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 38,
                            Descricao = "Cedente não concorda com alegação do sacado"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.CedenteSolicitaDispensaDeJuros:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 47,
                            Descricao = "Cedente solicita dispensa de juros"
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
            //21- Duplicata Mercantil; 
            //22- Nota Promissória; 
            //25- Recibo; 
            //31- Duplicata Prestação ou 
            //39- Outros

            switch (especie)
            {
                case EnumEspecieDocumento.DuplicataMercantil:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 21,
                        Descricao = "Duplicata Mercantil",
                        Sigla = "DM"
                    };
                }
                case EnumEspecieDocumento.NotaPromissoria:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 22,
                        Descricao = "Nota Promissória",
                        Sigla = "NP"
                    };
                }
                case EnumEspecieDocumento.Recibo:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 25,
                        Descricao = "Recibo",
                        Sigla = "RC"
                    };
                }
                case EnumEspecieDocumento.DuplicataPrestacao:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 31,
                        Descricao = "Duplicata prestação",
                        Sigla = "DP"
                    };
                }
                case EnumEspecieDocumento.Diversos:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 39,
                        Descricao = "Outros",
                        Sigla = "DV"
                    };
                }
            }
            throw new Exception(
                String.Format("Não foi possível obter espécie. Banco: {0} Código Espécie: {1}",
                    CodigoBanco, especie.ToString()));
        }

        public IInstrucao ObtemInstrucaoPadronizada(EnumTipoInstrucao tipoInstrucao, double valorInstrucao,
            DateTime dataInstrucao,
            int diasInstrucao)
        {
            switch (tipoInstrucao)
            {
                case EnumTipoInstrucao.JurosdeMora:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 01,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = string.Empty
                    };
                }
                case EnumTipoInstrucao.MultaDeVPorCentoSobreValorTitulo:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 03,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = string.Empty
                    };
                }
                case EnumTipoInstrucao.MultaDeVPorCentoAposNDiasCorridos:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 04,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = string.Empty
                    };
                }
                case EnumTipoInstrucao.MultaDeVPorCentoSobreValorTituloMaisEncargos:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 05,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = string.Empty
                    };
                }
                case EnumTipoInstrucao.MultaDeVPorCentoAposNDiasCorridosValorTituloMaisEncargos:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 06,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = string.Empty
                    };
                }
                case EnumTipoInstrucao.NaoCobrarJurosDeMora:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 08,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = string.Empty
                    };
                }
                case EnumTipoInstrucao.ProtestarAposNDiasCorridos:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 09,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = string.Empty
                    };
                }
                case EnumTipoInstrucao.NaoReceberAposOVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 13,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = string.Empty
                    };
                }
                case EnumTipoInstrucao.CobrarJurosMaisVariacaoIDTR50:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 18,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = string.Empty
                    };
                }
                case EnumTipoInstrucao.NaoReceberAposNDiasCorridos:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 94,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = string.Empty
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
                throw new ApplicationException("Arquivo informado é inválido.");

            /* Identifica o layout: 400 */
            if (linhasArquivo.First().Length == 400)
            {
                var leitor = new LeitorRetornoCnab400BRB(linhasArquivo);
                var retornoProcessado = leitor.ProcessarRetorno();

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
