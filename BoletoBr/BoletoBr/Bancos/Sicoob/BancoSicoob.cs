using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;
using BoletoBr.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Sicoob
{
    public class BancoSicoob : IBanco
    {
        private int _dvCodigoBarras;

        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        public Image LogotipoBancoParaExibicao { get; set; }
        public string LocalDePagamento { get; private set; }
        public string MoedaBanco { get; private set; }

        public BancoSicoob()
        {
            CodigoBanco = "756";
            DigitoBanco = "0";
            NomeBanco = "SICOOB";
            LocalDePagamento = "Pagável em qualquer banco até o vencimento";
            MoedaBanco = "9";
        }

        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            if (!(boleto.CarteiraCobranca.Codigo == "1/01"))
                throw new NotImplementedException("Carteira não implementada. Carteira dísponivel 1/01");

            if (boleto.CarteiraCobranca.Codigo == "1/01")
            {
                if (boleto.CedenteBoleto.ContaBancariaCedente.Agencia.BoletoBrToStringSafe().Trim().Length > 4)
                    throw new ValidacaoBoletoException("A agencia deve ter no máximo 4 dígitos.");

                var codigoCliente = boleto.CedenteBoleto.CodigoCedente + boleto.CedenteBoleto.DigitoCedente;
                if (codigoCliente.BoletoBrToStringSafe().Trim().Length > 10)
                    throw new ValidacaoBoletoException("O código do cedente deve ter no máximo 10 dígitos.");

                if (boleto.IdentificadorInternoBoleto.BoletoBrToStringSafe().Trim().Length > 7)
                    throw new ValidacaoBoletoException("Identificador interno do boleto deve ter no máximo 7 dígitos.");
            }
        }

        public void FormataMoeda(Boleto boleto)
        {
            boleto.Moeda = MoedaBanco;

            if (String.IsNullOrEmpty(boleto.Moeda))
                throw new Exception("Espécie/Moeda para o boleto não foi informada.");

            if ((boleto.Moeda == "9") || (boleto.Moeda == "REAL") || (boleto.Moeda == "R$"))
                boleto.Moeda = "R$";
        }

        public void FormatarBoleto(Boleto boleto)
        {
            boleto.LocalPagamento = LocalDePagamento;

            boleto.ValidaDadosEssenciaisDoBoleto();

            FormataNumeroDocumento(boleto);
            FormataNossoNumero(boleto);
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataMoeda(boleto);

            ValidaBoletoComNormasBanco(boleto);

            boleto.CedenteBoleto.CodigoCedenteFormatado = String.Format("{0}/{1}{2}",
                boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'),
                boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0'),
                boleto.CedenteBoleto.DigitoCedente);
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            var codigoBanco = CodigoBanco.PadLeft(3, '0'); //3
            var codigoMoeda = MoedaBanco; //1
            var fatorVencimento = Common.FatorVencimento(boleto.DataVencimento).ToString(); //4
            var valorNominal = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "").PadLeft(10, '0'); //10

            var carteiraCobranca = boleto.CarteiraCobranca.Codigo.ExtrairValorDaLinha(1, 1); //1
            var codigoAgencia = boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'); //4
            var codigoModalidade = boleto.CarteiraCobranca.Codigo.ExtrairValorDaLinha(3, 4); //2
            var codigoCedente = (boleto.CedenteBoleto.CodigoCedente + boleto.CedenteBoleto.DigitoCedente).PadLeft(7, '0'); //7
            var nossoNumeroBoleto = boleto.NossoNumeroFormatado.Replace("-", "").PadLeft(8, '0'); //8
            var numeroParcela = "001"; //3;

            boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", codigoBanco, codigoMoeda,
                fatorVencimento, valorNominal, carteiraCobranca, codigoAgencia, codigoModalidade, codigoCedente,
                nossoNumeroBoleto, numeroParcela);

            _dvCodigoBarras = Common.Mod11Peso2a9(boleto.CodigoBarraBoleto);

            boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", codigoBanco, codigoMoeda,
                _dvCodigoBarras, fatorVencimento, valorNominal, carteiraCobranca, codigoAgencia, codigoModalidade, codigoCedente,
                nossoNumeroBoleto, numeroParcela);
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {
            var nossoNumero = boleto.NossoNumeroFormatado.Replace("-", "").PadLeft(8, '0');

            #region AAABC.DDDDE

            var codigoBanco = CodigoBanco.PadLeft(3, '0'); //3
            var codigoMoeda = MoedaBanco; //1
            var carteiraCobranca = boleto.CarteiraCobranca.Codigo.ExtrairValorDaLinha(1, 1); //1
            var codigoAgencia = boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'); //4

            var calculoDv1 = Common.Mod10(string.Format("{0}{1}{2}{3}", codigoBanco, codigoMoeda, carteiraCobranca,
                codigoAgencia)).ToString(CultureInfo.InvariantCulture); //1

            var grupo1 = string.Format("{0}{1}{2}.{3}{4}", codigoBanco, codigoMoeda, carteiraCobranca, codigoAgencia, calculoDv1);

            #endregion

            #region FFGGG.GGGGHI

            var codigoModalidade = boleto.CarteiraCobranca.Codigo.ExtrairValorDaLinha(3, 4); //2
            var codigoCedente = (boleto.CedenteBoleto.CodigoCedente + boleto.CedenteBoleto.DigitoCedente).PadLeft(7, '0'); //7
            var primeiroDigitoNossoNumero = nossoNumero.ExtrairValorDaLinha(1, 1);

            var calculoDv2 = Common.Mod10(string.Format("{0}{1}{2}", codigoModalidade, codigoCedente,
                primeiroDigitoNossoNumero)).ToString(CultureInfo.InvariantCulture); //1

            var grupo2 = string.Format("{0}{1}.{2}{3}{4}", codigoModalidade, codigoCedente.ExtrairValorDaLinha(1, 3),
                codigoCedente.ExtrairValorDaLinha(4, 7), primeiroDigitoNossoNumero, calculoDv2);

            #endregion

            #region HHHHH.HHJJJK

            var restanteNossoNumero = nossoNumero.ExtrairValorDaLinha(2, 8); //7
            var numeroParcela = "001"; //3

            var calculoDv3 = Common.Mod10(string.Format("{0}{1}", restanteNossoNumero, numeroParcela)).ToString(CultureInfo.InvariantCulture); //1

            var grupo3 = string.Format("{0}.{1}{2}{3}", restanteNossoNumero.ExtrairValorDaLinha(1, 5),
                restanteNossoNumero.ExtrairValorDaLinha(6, 7), numeroParcela, calculoDv3);

            #endregion

            #region MMMMNNNNNNNNNN

            var fatorVencimento = Common.FatorVencimento(boleto.DataVencimento).ToString(); //4
            var valorNominal = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "").PadLeft(10, '0'); //10

            var grupo4 = string.Format("{0}{1}", fatorVencimento, valorNominal);

            #endregion

            boleto.LinhaDigitavelBoleto = string.Format("{0} {1} {2} {3} {4}", grupo1, grupo2, grupo3, _dvCodigoBarras, grupo4);
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            if (String.IsNullOrEmpty(boleto.IdentificadorInternoBoleto) || String.IsNullOrEmpty(boleto.IdentificadorInternoBoleto.TrimStart('0')))
                throw new Exception("Sequencial Nosso Número não foi informado.");

            if (boleto.CarteiraCobranca.Codigo == "1/01")
            {
                var numeroAgencia = boleto.CedenteBoleto.ContaBancariaCedente.Agencia.BoletoBrToStringSafe().PadLeft(4, '0');
                var codigoCliente = (boleto.CedenteBoleto.CodigoCedente + boleto.CedenteBoleto.DigitoCedente).BoletoBrToStringSafe().PadLeft(10, '0');
                var identificadorInternoBoleto = boleto.IdentificadorInternoBoleto.BoletoBrToStringSafe().PadLeft(7, '0');

                var nossoNumeroComposto = numeroAgencia + codigoCliente + identificadorInternoBoleto;

                boleto.SetNossoNumeroFormatado(String.Format("{0}-{1}", identificadorInternoBoleto, Mod11Sicoob(nossoNumeroComposto)));
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
                        Descricao = "CONFIRMAÇÃO ENTRADA DE TITULO"
                    };
                case 05:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 05,
                        Descricao = "LIQUIDAÇÃO SEM REGISTRO"
                    };
                case 06:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 06,
                        Descricao = "LIQUIDAÇÃO NORMAL"
                    };
                case 09:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 09,
                        Descricao = "BAIXA SIMPLES"
                    };
                case 10:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 10,
                        Descricao = "BAIXA SOLICITADA"
                    };
                case 11:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 11,
                        Descricao = "TITULO EM SER"
                    };
                case 12:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 12,
                        Descricao = "ABATIMENTO CONCEDIDO"
                    };
                case 14:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 14,
                        Descricao = "ALTERAÇÃO DE VENCIMENTO"
                    };
                case 15:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 15,
                        Descricao = "LIQUIDAÇÃO EM CARTÓRIO"
                    };
                case 23:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 23,
                        Descricao = "ENCAMINHADO A PROTESTO"
                    };
                case 27:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 27,
                        Descricao = "CONFIRMAÇÃO DE ALTERAÇÃO DE DADOS"
                    };
                case 48:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 48,
                        Descricao = "CONFIRMAÇÃO DE INSTRUÇÃO DE TRANSFERÊNCIA DE CARTEIRA/MODALIDADE DE COBRANÇA"
                    };
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter Código de Comando/Movimento/Ocorrência. Banco: {0} Código: {1}",
                    CodigoBanco, numeroOcorrencia.ToString()));
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
                            Descricao = "Entrada de título"
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
                            Descricao = "Alteração de vencimento"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.AlteracaoSeuNumero:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 08,
                            Descricao = "Alteração seu número"
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
                case EnumCodigoOcorrenciaRemessa.SustarProtesto:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 10,
                            Descricao = "Pedido de Sustação de Protesto"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.DispensarCobrancaDeJurosDeMora:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 11,
                            Descricao = "Dispensar Juros"
                        };
                    }
                case EnumCodigoOcorrenciaRemessa.AlteracaoNomeEEnderecoSacado:
                    {
                        return new CodigoOcorrencia((int)ocorrencia)
                        {
                            Codigo = 12,
                            Descricao = "Alteração de Pagador"
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
                            Descricao = "Baixa - Pagamento Direto ao Beneficiário"
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
                            Descricao = "Duplicata mercantil",
                            Sigla = "DM"
                        };
                    }
                case EnumEspecieDocumento.NotaPromissoria:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 02,
                            Descricao = "Nota promissória",
                            Sigla = "NP"
                        };
                    }
                case EnumEspecieDocumento.NotaSeguro:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 03,
                            Descricao = "Nota de Seguro",
                            Sigla = "NS"
                        };
                    }
                case EnumEspecieDocumento.Recibo:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 05,
                            Descricao = "Recibo",
                            Sigla = "RE"
                        };
                    }
                case EnumEspecieDocumento.DuplicataRural:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 06,
                            Descricao = "Duplicata Rural",
                            Sigla = "DR"
                        };
                    }
                case EnumEspecieDocumento.LetraCambio:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 08,
                            Descricao = "Letra Câmbio",
                            Sigla = "LC"
                        };
                    }
                case EnumEspecieDocumento.Warrant:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 09,
                            Descricao = "Warrant",
                            Sigla = "WA"
                        };
                    }
                case EnumEspecieDocumento.Cheque:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 10,
                            Descricao = "Cheque",
                            Sigla = "CH"
                        };
                    }
                case EnumEspecieDocumento.DuplicataServico:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 12,
                            Descricao = "Duplicata de Serviço",
                            Sigla = "DS"
                        };
                    }
                case EnumEspecieDocumento.NotaDebito:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 13,
                            Descricao = "Nota de Débito",
                            Sigla = "ND"
                        };
                    }
                case EnumEspecieDocumento.TriplicataMercantil:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 14,
                            Descricao = "Triplicata Mercantil",
                            Sigla = "TM"
                        };
                    }
                case EnumEspecieDocumento.TriplicataServico:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 15,
                            Descricao = "Triplicata de Serviço",
                            Sigla = "NP"
                        };
                    }
                case EnumEspecieDocumento.Fatura:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 18,
                            Descricao = "Fatura",
                            Sigla = "FT"
                        };
                    }
                case EnumEspecieDocumento.ApoliceSeguro:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 20,
                            Descricao = "Apólice de Seguro",
                            Sigla = "AS"
                        };
                    }
                case EnumEspecieDocumento.MensalidadeEscolar:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 21,
                            Descricao = "Mensalidade Escolar",
                            Sigla = "ME"
                        };
                    }
                case EnumEspecieDocumento.ParcelaConsorcio:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 22,
                            Descricao = "Parcela de Consórcio",
                            Sigla = "PC"
                        };
                    }
                case EnumEspecieDocumento.Outros:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 99,
                            Descricao = "Outros",
                            Sigla = "OT"
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
                case EnumTipoInstrucao.SemInstrucoes:
                    {
                        return new InstrucaoPadronizada()
                        {
                            Codigo = 0,
                            QtdDias = diasInstrucao,
                            Valor = valorInstrucao,
                            TextoInstrucao = "AUSENCIA DE INSTRUCOES"
                        };
                    }
                case EnumTipoInstrucao.JurosdeMora:
                    {
                        return new InstrucaoPadronizada()
                        {
                            Codigo = 1,
                            QtdDias = diasInstrucao,
                            Valor = valorInstrucao,
                            TextoInstrucao = "COBRAR JUROS"
                        };
                    }
                case EnumTipoInstrucao.ProtestarApos3DiasUteis:
                    {
                        return new InstrucaoPadronizada()
                        {
                            Codigo = 3,
                            QtdDias = diasInstrucao,
                            Valor = valorInstrucao,
                            TextoInstrucao = "PROTESTAR APÓS " + diasInstrucao + " DIAS ÚTEIS DO VENCIMENTO"
                        };
                    }
                case EnumTipoInstrucao.ProtestarApos4DiasUteis:
                    {
                        return new InstrucaoPadronizada()
                        {
                            Codigo = 4,
                            QtdDias = diasInstrucao,
                            Valor = valorInstrucao,
                            TextoInstrucao = "PROTESTAR APÓS " + diasInstrucao + " DIAS ÚTEIS DO VENCIMENTO"
                        };
                    }
                case EnumTipoInstrucao.ProtestarApos5DiasUteis:
                    {
                        return new InstrucaoPadronizada()
                        {
                            Codigo = 5,
                            QtdDias = diasInstrucao,
                            Valor = valorInstrucao,
                            TextoInstrucao = "PROTESTAR APÓS " + diasInstrucao + " DIAS ÚTEIS DO VENCIMENTO"
                        };
                    }
                case EnumTipoInstrucao.NaoProtestar:
                    {
                        return new InstrucaoPadronizada()
                        {
                            Codigo = 7,
                            QtdDias = diasInstrucao,
                            Valor = valorInstrucao,
                            TextoInstrucao = "NÃO PROTESTAR"
                        };
                    }
                case EnumTipoInstrucao.ProtestarAposNDiasUteis:
                    {
                        return new InstrucaoPadronizada()
                        {
                            Codigo = 10,
                            QtdDias = diasInstrucao,
                            Valor = valorInstrucao,
                            TextoInstrucao = "PROTESTAR APÓS " + diasInstrucao + " DIAS ÚTEIS DO VENCIMENTO"
                        };
                    }
                case EnumTipoInstrucao.ConcederDescontoAte:
                    {
                        return new InstrucaoPadronizada()
                        {
                            Codigo = 22,
                            QtdDias = diasInstrucao,
                            Valor = valorInstrucao,
                            TextoInstrucao = "CONCEDER DESCONTO SO ATE A DATA ESTIPULADA"
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

            /* Identifica o layout: 240 ou 400 */
            if (linhasArquivo.First().Length == 400)
            {
                var leitor = new LeitorRetornoCnab400Sicoob(linhasArquivo);
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

        private static int Mod11Sicoob(string seq)
        {
            var sequencia = seq;
            var total = 0;
            var numSeq = 1;
            var constValor = 0;
            var nresto = 0;
            var ndig = 0;

            while (sequencia.Length > 0)
            {
                if (numSeq == 1)
                    constValor = 3;
                else if (numSeq == 2)
                    constValor = 1;
                else if (numSeq == 3)
                    constValor = 9;
                else if (numSeq == 4)
                {
                    constValor = 7;
                    numSeq = 0;
                }

                int valorPosicao = Convert.ToInt32(sequencia.Substring(0, 1));
                total += valorPosicao * constValor;

                numSeq++;
                sequencia = sequencia.Remove(0, 1);
            }

            nresto = total - ((total / 11) * 11);

            if (nresto == 0 || nresto == 1)
                ndig = 0;
            else
                ndig = (11 - nresto);

            return ndig;
        }
    }
}
