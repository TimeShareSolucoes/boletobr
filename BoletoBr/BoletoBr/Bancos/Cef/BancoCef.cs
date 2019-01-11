using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Cef
{
    public class BancoCef : IBanco
    {
        /* 
        * TipoDocumento 1 - SICGB - Com registro
        * TipoDocumento 2 - SICGB - Sem registro
        */

        // Identificador de Tipo de CobrançaII
        private const string IdentificadorTipoCobrancaCarteiraSicgbRg = "1";
        private const string IdentificadorTipoCobrancaCarteiraSicgbSr = "2";
        // Identificador de Emissão do Boleto (4 - Beneficiário)
        private const string IdentificadorEmissaoCedente = "4";

        private string _dacBoleto = String.Empty;

        private bool _protestar;
        private bool _baixaDevolver;
        private bool _desconto;
        private int _diasProtesto = 0;
        private int _diasDevolucao = 0;
        private int _diasDesconto = 0;

        public BancoCef()
        {
            CodigoBanco = "104";
            DigitoBanco = "0";
            NomeBanco = "Caixa Econômica Federal";
            LocalDePagamento = "Pagável preferencialmente nas agências da Caixa ou Lotéricas.";
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
            if (boleto.CarteiraCobranca == null)
            {
                throw new Exception("Carteira de cobrança não especificada");
            }

            if (!(boleto.CarteiraCobranca.Codigo.Equals("SR") || boleto.CarteiraCobranca.Codigo.Equals("RG")))
            {
                throw new Exception("Carteira cobrança com código: " + boleto.CarteiraCobranca.Codigo +
                                    " não é suportada.");
            }

            if (boleto.NossoNumeroFormatado.Length != 19)
            {
                throw new Exception("Nosso Número Formatado não pode ter tamanho diferente de 19 dígitos.");
            }

            if (String.IsNullOrEmpty(boleto.CedenteBoleto.CodigoCedente))
                throw new Exception("Código do cedente não foi informado.");

            if (boleto.CedenteBoleto.CodigoCedente.Length > 6)
                throw new Exception("O código do cedente deve no máximo 6 dígitos");
        }

        public void FormataMoeda(Boleto boleto)
        {
            boleto.Moeda = MoedaBanco;

            if (string.IsNullOrEmpty(boleto.Moeda))
                throw new Exception("Espécie/Moeda para o boleto não foi informada.");

            if ((boleto.Moeda == "9") || (boleto.Moeda == "REAL") || (boleto.Moeda == "R$"))
                boleto.Moeda = "R$";
            else
                boleto.Moeda = "0";
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

            /* Formata o código do cedente
             * Inserindo o dígito verificador
             */
            string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(6, '0');
            string dvCodigoCedente = Common.Mod11Base9(codigoCedente).ToString(CultureInfo.InvariantCulture); //Base9 

            if (boleto.CedenteBoleto.DigitoCedente.Equals(-1))
                boleto.CedenteBoleto.DigitoCedente = Convert.ToInt32(dvCodigoCedente);

            boleto.CedenteBoleto.CodigoCedenteFormatado = String.Format("{0}/{1}-{2}",
                boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'), 
                codigoCedente, 
                dvCodigoCedente);
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            if (boleto.CarteiraCobranca == null)
            {
                throw new Exception("Carteira de cobrança não especificada");
            }

            if (!(boleto.CarteiraCobranca.Codigo.Equals("SR") || boleto.CarteiraCobranca.Codigo.Equals("RG")))
            {
                throw new Exception("Carteira cobrança com código: " + boleto.CarteiraCobranca.Codigo +
                                    " não é suportada.");
            }

            if (boleto.NossoNumeroFormatado.Length != 19)
            {
                throw new Exception("Nosso Número Formatado não pode ter tamanho diferente de 19 dígitos.");
            }

            // Posi��o 01-03
            string banco = CodigoBanco;

            //Posi��o 04
            string moeda = MoedaBanco;

            //Posi��o 05 - No final ...   

            // Posi��o 06 - 09
            long fatorVencimento = Common.FatorVencimento(boleto.DataVencimento);

            // Posi��o 10 - 19     
            string valorDocumento = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorDocumento = valorDocumento.PadLeft(10, '0');

            // Inicio Campo livre

            /* Presumimos que o nosso número formatado tem:
             * 19 dígitos no total se incluir o dígito verificador.
             * Para o cálculo do código de barras, precisamos do nosso número, sem o dígito verificador.
             * Para obter esse valor, vamos remover os 2 últimos dígitos.
             */
            string nossoNumeroFormatadoSemDigito =
                boleto.NossoNumeroFormatado.Substring(0, 17);

            //104 - Caixa Econ�mica Federal S.A. 
            //Carteira SR - 24 (cobran�a sem registro) || Carteira RG - 14 (cobran�a com registro)
            //Cobran�a sem registro, nosso n�mero com 17 d�gitos. 

            //Posi��o 20 - 25
            string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(6, '0');

            // Posi��o 26
            string dvCodigoCedente = Common.Mod11Base9(codigoCedente).ToString(CultureInfo.InvariantCulture);

            //Posi��o 27 - 29
            //De acordo com documenta��o, posi��o 3 a 5 do nosso numero
            string primeiraParteNossoNumeroSemDigito = nossoNumeroFormatadoSemDigito.Substring(2, 3);

            //Posi��o 30
            string primeiraConstante;
            switch (boleto.CarteiraCobranca.Codigo)
            {
                case "SR":
                    primeiraConstante = "2";
                    break;
                case "RG":
                    primeiraConstante = "1";
                    break;
                default:
                    primeiraConstante = boleto.CarteiraCobranca.Codigo;
                    break;
            }

            // Posi��o 31 - 33
            //DE acordo com documenta��o, posi��o 6 a 8 do nosso numero
            string segundaParteNossoNumeroSemDigito = nossoNumeroFormatadoSemDigito.Substring(5, 3);

            // Posi��o 34
            string segundaConstante = "4"; // 4 => emiss�o do boleto pelo cedente
            if(boleto.CarteiraCobranca.BancoEmiteBoleto)
                segundaConstante = "1"; // 1=> emissão do boleto pelo banco
            //Posi��o 35 - 43
            //De acordo com documenta�ao, posi��o 9 a 17 do nosso numero
            string terceiraParteNossoNumeroSemDigito = nossoNumeroFormatadoSemDigito.Substring(8, 9);

            //Posi��o 44
            string ccc = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                codigoCedente,
                dvCodigoCedente,
                primeiraParteNossoNumeroSemDigito,
                primeiraConstante,
                segundaParteNossoNumeroSemDigito,
                segundaConstante,
                terceiraParteNossoNumeroSemDigito);
            string dvCampoLivre = Common.Mod11Base9(ccc).ToString(CultureInfo.InvariantCulture);
            string campoLivre = string.Format("{0}{1}", ccc, dvCampoLivre);

            string xxxx = string.Format("{0}{1}{2}{3}{4}", banco, moeda, fatorVencimento, valorDocumento, campoLivre);

            string dvGeral = Common.Mod11(xxxx, 9).ToString(CultureInfo.InvariantCulture);
            // Posi��o 5
            _dacBoleto = dvGeral;

            boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}",
                banco,
                moeda,
                dvGeral,
                fatorVencimento,
                valorDocumento,
                campoLivre
                );
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {
            string Grupo1 = string.Empty;
            string Grupo2 = string.Empty;
            string Grupo3 = string.Empty;
            string Grupo4 = string.Empty;
            string Grupo5 = string.Empty;

            string str1 = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;

            if (boleto.NossoNumeroFormatado.Length == 17)
            {
                #region Campo 1

                //POSI��O 1 A 4 DO CODIGO DE BARRAS
                str1 = boleto.CodigoBarraBoleto.Substring(0, 4);
                //POSICAO 20 A 24 DO CODIGO DE BARRAS
                str2 = boleto.CodigoBarraBoleto.Substring(19, 5);
                //CALCULO DO DIGITO
                str3 = Common.Mod10(str1 + str2).ToString(CultureInfo.InvariantCulture);

                Grupo1 = str1 + str2 + str3;
                Grupo1 = Grupo1.Substring(0, 5) + "." + Grupo1.Substring(5) + " ";

                #endregion Campo 1

                #region Campo 2

                //POSI��O 25 A 34 DO COD DE BARRAS
                str1 = boleto.CodigoBarraBoleto.Substring(24, 10);
                //DIGITO
                str2 = Common.Mod10(str1).ToString(CultureInfo.InvariantCulture);

                Grupo2 = string.Format("{0}.{1}{2} ", str1.Substring(0, 5), str1.Substring(5, 5), str2);

                #endregion Campo 2

                #region Campo 3

                //POSI��O 35 A 44 DO CODIGO DE BARRAS
                str1 = boleto.CodigoBarraBoleto.Substring(34, 10);
                //DIGITO
                str2 = Common.Mod10(str1).ToString(CultureInfo.InvariantCulture);

                Grupo3 = string.Format("{0}.{1}{2} ", str1.Substring(0, 5), str1.Substring(5, 5), str2);

                #endregion Campo 3

                #region Campo 4

                string D4 = _dacBoleto;

                Grupo4 = string.Format("{0} ", D4);

                #endregion Campo 4

                #region Campo 5

                //POSICAO 6 A 9 DO CODIGO DE BARRAS
                str1 = boleto.CodigoBarraBoleto.Substring(5, 4);

                //POSICAO 10 A 19 DO CODIGO DE BARRAS
                str2 = boleto.CodigoBarraBoleto.Substring(9, 10);

                Grupo5 = string.Format("{0}{1}", str1, str2);

                #endregion Campo 5
            }
            else
            {
                #region Campo 1

                string BBB = boleto.CodigoBarraBoleto.Substring(0, 3);
                string M = boleto.CodigoBarraBoleto.Substring(3, 1);
                string CCCCC = boleto.CodigoBarraBoleto.Substring(19, 5);
                string D1 = Common.Mod10(BBB + M + CCCCC).ToString(CultureInfo.InvariantCulture);

                Grupo1 = string.Format("{0}{1}{2}.{3}{4} ",
                    BBB,
                    M,
                    CCCCC.Substring(0, 1),
                    CCCCC.Substring(1, 4), D1);


                #endregion Campo 1

                #region Campo 2

                string CCCCCCCCCC2 = boleto.CodigoBarraBoleto.Substring(24, 10);
                string D2 = Common.Mod10(CCCCCCCCCC2).ToString(CultureInfo.InvariantCulture);

                Grupo2 = string.Format("{0}.{1}{2} ", CCCCCCCCCC2.Substring(0, 5), CCCCCCCCCC2.Substring(5, 5), D2);

                #endregion Campo 2

                #region Campo 3

                string CCCCCCCCCC3 = boleto.CodigoBarraBoleto.Substring(34, 10);
                string D3 = Common.Mod10(CCCCCCCCCC3).ToString(CultureInfo.InvariantCulture);

                Grupo3 = string.Format("{0}.{1}{2} ", CCCCCCCCCC3.Substring(0, 5), CCCCCCCCCC3.Substring(5, 5), D3);


                #endregion Campo 3

                #region Campo 4

                string D4 = _dacBoleto;

                Grupo4 = string.Format(" {0} ", D4);

                #endregion Campo 4

                #region Campo 5

                long FFFF = Common.FatorVencimento(boleto.DataVencimento);

                string VVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
                VVVVVVVVVV = VVVVVVVVVV.PadLeft(10, '0');

                if (Convert.ToInt64(VVVVVVVVVV) == 0)
                    VVVVVVVVVV = "000";

                Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

                #endregion Campo 5
            }

            //MONTA OS DADOS DA LINHA DIGIT�VEL DE ACORDO COM OS DADOS OBTIDOS ACIMA
            boleto.LinhaDigitavelBoleto = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            if (String.IsNullOrEmpty(boleto.IdentificadorInternoBoleto))
                throw new Exception("Sequencial nosso número não pode estar em branco.");

            if (boleto.IdentificadorInternoBoleto.Length > 15)
                throw new Exception("Sequencial nosso número não pode exceder 15 dígitos.");

            string dvNossoNumero;

            /* 
             * Informação reservada para arquivo de remessa
             * O tipo de modalidade são os 2 primeiros dígitos do Nosso Número
             */
            boleto.TipoModalidade = boleto.IdentificadorInternoBoleto.Length > 1 ? ("00" + boleto.IdentificadorInternoBoleto).Substring(0, 2) : boleto.IdentificadorInternoBoleto.Substring(0, 2);

            boleto.SetNossoNumeroFormatado(boleto.IdentificadorInternoBoleto.PadLeft(15, '0'));

            //Atribui ao Nosso Número o Identificador de Cobrança + Identificador do Emissor
            if (boleto.CarteiraCobranca.Codigo.Equals("RG"))
            {
                boleto.SetNossoNumeroFormatado(
                    IdentificadorTipoCobrancaCarteiraSicgbRg +
                    (boleto.CarteiraCobranca.BancoEmiteBoleto? "1" : IdentificadorEmissaoCedente) +
                    boleto.NossoNumeroFormatado);

                // Permite 0 (zero) no DV do Nosso Número
                dvNossoNumero = Common.Mod11Base9Caixa(boleto.NossoNumeroFormatado).ToString(CultureInfo.InvariantCulture);

                boleto.SetNossoNumeroFormatado(string.Format("{0}-{1}", boleto.NossoNumeroFormatado, dvNossoNumero));
            }
            else if (boleto.CarteiraCobranca.Codigo.Equals("SR"))
            {
                boleto.SetNossoNumeroFormatado(
                    IdentificadorTipoCobrancaCarteiraSicgbSr +
                    (boleto.CarteiraCobranca.BancoEmiteBoleto ? "1" : IdentificadorEmissaoCedente) +
                    boleto.NossoNumeroFormatado);

                // Permite 0 (zero) no DV do Nosso Número
                dvNossoNumero = Common.Mod11Base9Caixa(boleto.NossoNumeroFormatado).ToString(CultureInfo.InvariantCulture);

                boleto.SetNossoNumeroFormatado(string.Format("{0}-{1}", boleto.NossoNumeroFormatado, dvNossoNumero));
            }
            else
            {
                throw new Exception("Erro ao formatar nosso número");
            }
        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            if (String.IsNullOrEmpty(boleto.NumeroDocumento) ||
                String.IsNullOrEmpty(boleto.NumeroDocumento.TrimStart('0')))
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
                        Descricao = "Transferência de Carteira/Entrada"
                    };
                case 05:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 05,
                        Descricao = "Transferência de Carteira/Baixa"
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
                        Descricao = "CONFIRMAÇÃO DO RECEBIMENTO DA INSTRUÇÃO DE DESCONTO"
                    };
                case 08:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 08,
                        Descricao = "CONFIRMAÇÃO DO RECEBIMENTO DO CANCELAMENTO DO DESCONTO"
                    };
                case 09:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 09,
                        Descricao = "BAIXA SIMPLES"
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
                        Descricao = "CANCELAMENTO ABATIMENTO"
                    };
                case 14:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 14,
                        Descricao = "ALTERAÇÃO DE VENCIMENTO"
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
                    default:
                        return new CodigoOcorrencia(numeroOcorrencia)
                        {
                            Codigo = numeroOcorrencia,
                            Descricao = $@"ocorrencia - {numeroOcorrencia}"
                        };
            }
        }

        public ICodigoOcorrencia ObtemCodigoOcorrencia(EnumCodigoOcorrenciaRetorno ocorrenciaRetorno)
        {
            throw new NotImplementedException();
        }

        public IEspecieDocumento ObtemEspecieDocumento(EnumEspecieDocumento especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento.Cheque:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 01,
                        Descricao = "Cheque",
                        Sigla = "CH"
                    };
                }
                case EnumEspecieDocumento.DuplicataMercantil:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 02,
                        Descricao = "Duplicata mercantil",
                        Sigla = "DM"
                    };
                }
                case EnumEspecieDocumento.DuplicataMercantilIndicacao:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 03,
                        Descricao = "Duplicata Mercatil p/ Indicação",
                        Sigla = "DMI"
                    };
                }
                case EnumEspecieDocumento.DuplicataServico:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 04,
                        Descricao = "Duplicata de Serviço",
                        Sigla = "DS"
                    };
                }
                case EnumEspecieDocumento.DuplicataServicoIndicacao:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 05,
                        Descricao = "Duplicata de Serviço p/ Indicação",
                        Sigla = "DSI"
                    };
                }
                case EnumEspecieDocumento.DuplicataRural:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 06,
                        Descricao = "Duplicata Rural",
                        Sigla = "DR"
                    };
                }
                case EnumEspecieDocumento.LetraCambio:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 07,
                        Descricao = "Letra de Câmbio",
                        Sigla = "LC"
                    };
                }
                case EnumEspecieDocumento.NotaCreditoComercial:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 08,
                        Descricao = "Nota de Crédito Comercial",
                        Sigla = "NCC"
                    };
                }
                case EnumEspecieDocumento.NotaCreditoExportacao:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 09,
                        Descricao = "Nota de Crédito a Exportação",
                        Sigla = "NCE"
                    };
                }
                case EnumEspecieDocumento.NotaCreditoIndustrial:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 10,
                        Descricao = "Nota de Crédito Industrial",
                        Sigla = "NCI"
                    };
                }
                case EnumEspecieDocumento.NotaCreditoRural:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 11,
                        Descricao = "Nota de Crédito Rural",
                        Sigla = "NCR"
                    };
                }
                case EnumEspecieDocumento.NotaPromissoria:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 12,
                        Descricao = "Nota Promissória",
                        Sigla = "NP"
                    };
                }
                case EnumEspecieDocumento.NotaPromissoriaRural:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 13,
                        Descricao = "Nota Promissória Rural",
                        Sigla = "NPR"
                    };
                }
                case EnumEspecieDocumento.TriplicataMercantil:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 14,
                        Descricao = "Triplicata Mercantil",
                        Sigla = "TM"
                    };
                }
                case EnumEspecieDocumento.TriplicataServico:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 15,
                        Descricao = "Triplicata de Serviço",
                        Sigla = "TS"
                    };
                }
                case EnumEspecieDocumento.NotaDeSeguro:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 16,
                        Descricao = "Nota de Seguro",
                        Sigla = "NS"
                    };
                }
                case EnumEspecieDocumento.Recibo:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 17,
                        Descricao = "Recibo",
                        Sigla = "RC"
                    };
                }
                case EnumEspecieDocumento.Fatura:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 18,
                        Descricao = "Fatura",
                        Sigla = "FAT"
                    };
                }
                case EnumEspecieDocumento.NotaDebito:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 19,
                        Descricao = "Nota de Débito",
                        Sigla = "ND"
                    };
                }
                case EnumEspecieDocumento.ApoliceSeguro:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 20,
                        Descricao = "Apólice de Seguro",
                        Sigla = "AP"
                    };
                }
                case EnumEspecieDocumento.MensalidadeEscolar:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 21,
                        Descricao = "Mensalidade Escolar",
                        Sigla = "ME"
                    };
                }
                case EnumEspecieDocumento.ParcelaConsorcio:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 22,
                        Descricao = "Parcela de Consórcio",
                        Sigla = "PC"
                    };
                }
                case EnumEspecieDocumento.NotaFiscal:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 23,
                        Descricao = "Nota Fiscal",
                        Sigla = "NF"
                    };
                }
                case EnumEspecieDocumento.DocumentoDivida:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 24,
                        Descricao = "Documento de Dívida",
                        Sigla = "DD"
                    };
                }
                case EnumEspecieDocumento.CedulaProdutoRural:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 25,
                        Descricao = "cédula de Produto Rural",
                        Sigla = "CPR"
                    };
                }
                case EnumEspecieDocumento.Outros:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 99,
                        Descricao = "Outros",
                        Sigla = "OU"
                    };
                }
            }
            throw new Exception(
                String.Format("Não foi possível obter instrução padronizada. Banco: {0} Código Espécie: {1}",
                    CodigoBanco, especie.ToString()));
        }

        public IInstrucao ObtemInstrucaoPadronizada(EnumTipoInstrucao tipoInstrucao, double valorInstrucao,
            DateTime dataInstrucao, int diasInstrucao)
        {
            /*
            Protestar = 9,                      // Emite aviso ao sacado ap�s N dias do vencto, e envia ao cart�rio ap�s 5 dias �teis
            NaoProtestar = 10,                  // Inibe protesto, quando houver instru��o permanente na conta corrente
            ImportanciaporDiaDesconto = 30,
            ProtestoFinsFalimentares = 42,
            ProtestarAposNDiasCorridos = 81,
            ProtestarAposNDiasUteis = 82,
            NaoReceberAposNDias = 91,
            DevolverAposNDias = 92,
            JurosdeMora = 998,
            DescontoporDia = 999,
            Multa = 8
             */

            switch (tipoInstrucao)
            {
                case EnumTipoInstrucao.MultaVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 9,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Após vencimento cobrar Multa de " + valorInstrucao + "%"
                    };
                }

                case EnumTipoInstrucao.Protestar:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 9,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Protestar após " + valorInstrucao + " dias úteis."
                    };
                }
                case EnumTipoInstrucao.NaoProtestar:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 10,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não protestar."
                    };
                }
                case EnumTipoInstrucao.DescontoPorDia:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 30,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Importância por dia de desconto."
                    };
                }
                case EnumTipoInstrucao.ProtestoFinsFalimentares:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 42,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Protesto para fins falimentares"
                    };
                }
                case EnumTipoInstrucao.ProtestarAposNDiasCorridos:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 81,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Protestar após " + diasInstrucao + " dias corridos do vencimento."
                    };
                }
                case EnumTipoInstrucao.ProtestarAposNDiasUteis:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 82,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Protestar após " + diasInstrucao + " dias úteis do vencimento."
                    };
                }
                case EnumTipoInstrucao.NaoReceberAposOVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 91,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber após " + diasInstrucao + " dias do vencimento."
                    };
                }
                case EnumTipoInstrucao.DevolverAposNDias:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 92,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Devolver após " + diasInstrucao + " dias do vencimento."
                    };
                }
                case EnumTipoInstrucao.JurosdeMora:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 998,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Após vencimento cobrar juros de " + valorInstrucao + "%"
                    };
                }
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter instrução padronizada. Banco: {0} Código Instrução: {1} Qtd Dias/Valor: {2}",
                    CodigoBanco, tipoInstrucao, valorInstrucao));
        }

        public ICodigoOcorrencia ObtemCodigoOcorrencia(EnumCodigoOcorrenciaRemessa ocorrencia, double valorOcorrencia,
            DateTime dataOcorrencia)
        {
            switch (ocorrencia)
            {
                case EnumCodigoOcorrenciaRemessa.Registro:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 01,
                        Descricao = "Entrada de Título"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.Baixa:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 02,
                        Descricao = "Pedido de Baixa"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.ConcessaoDeAbatimento:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 04,
                        Descricao = "Concessão de Abatimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDeAbatimento:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 05,
                        Descricao = "Cancelamento de Abatimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeVencimento:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 06,
                        Descricao = "Alteração de Vencimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.ConcessaoDeDesconto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 07,
                        Descricao = "Concessão de Desconto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDeDesconto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 08,
                        Descricao = "Cancelamento de Desconto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.Protesto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 09,
                        Descricao = "Protestar"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.SustarProtestoEBaixarTitulo:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 10,
                        Descricao = "Sustar Protesto e Baixar Título"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.SustarProtestoEManterEmCarteira:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 11,
                        Descricao = "Sustar Protesto e Manter em Carteira"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeJurosDeMora:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 12,
                        Descricao = "Alteração de Juros de Mora"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.DispensarCobrancaDeJurosDeMora:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 13,
                        Descricao = "Dispensar cobrança de Juros de Mora"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeValorPercentualDeMulta:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 14,
                        Descricao = "Alteração de Valor/Percentual de Multa"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.DispensarCobrancaDeMulta:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 15,
                        Descricao = "Dispensar Cobrança de Multa"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDoValorDeDesconto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 16,
                        Descricao = "Alteração do Valor de Desconto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.NaoConcederDesconto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 17,
                        Descricao = "Não Conceder Desconto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDoValorDeAbatimento:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 18,
                        Descricao = "Alteração do Valor de Abatimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeOutrosDados:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 31,
                        Descricao = "Alteração de Outros Dados"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDosDadosDoRateioDeCredito:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 33,
                        Descricao = "Alteração dos Dados do Rateio de Crédito"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDoRateioDeCredito:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 34,
                        Descricao = "Pedido de Cancelamento dos Dados do Rateio de Crédito"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.InclusaoNoBancoDeSacados:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 36,
                        Descricao = "Inclusão no Banco de Sacados"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoNoBancoDeSacados:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 37,
                        Descricao = "Alteração no Banco de Sacados"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.ExclusaoNoBancoDeSacados:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 38,
                        Descricao = "Exclusão no Banco de Sacados"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.Servicos:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 40,
                        Descricao = "Serviços"
                    };
                }
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter Código de Comando/Movimento/Ocorrência. Banco: {0} Código: {1}",
                    CodigoBanco, ocorrencia.ToString()));
        }

        public RetornoGenerico LerArquivoRetorno(List<string> linhasArquivo)
        {
            if (linhasArquivo == null || linhasArquivo.Any() == false)
                throw new ApplicationException("Arquivo informado é inválido/Não existem títulos no retorno.");

            /* Identifica o layout: 240 ou 400 */
            if (linhasArquivo.First().Length == 240)
            {
                var leitor = new LeitorRetornoCnab240Cef(linhasArquivo);
                var retornoProcessado = leitor.ProcessarRetorno();

                var objRetornar = new RetornoGenerico(retornoProcessado);
                return objRetornar;
            }
            if (linhasArquivo.First().Length == 400)
            {
                var leitor = new LeitorRetornoCnab400Cef(linhasArquivo);
                var retornoProcessado = leitor.ProcessarRetorno();

                var objRetornar = new RetornoGenerico(retornoProcessado);
                return objRetornar;
            }

            throw new Exception("Arquivo de RETORNO com " + linhasArquivo.First().Length + " posições, não é suportado.");
        }

        public RemessaCnab240 GerarArquivoRemessaCnab240(List<Boleto> boletos)
        {
            //if (boletos == null || boletos.Count == 0)
            //    throw new Exception("Não há boletos para gerar a remessa.");

            //var escritor = new EscritorRemessaCnab240CefSicgb(remessa);

            //var obj = escritor.EscreverTexto(remessa);

            //return obj;

            throw new NotImplementedException();
        }

        public RemessaCnab400 GerarArquivoRemessaCnab400(List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }

        public int CodigoJurosMora(CodigoJurosMora codigoJurosMora)
        {
            switch (codigoJurosMora)
            {
                case Enums.CodigoJurosMora.Valor:
                    return 1;
                case Enums.CodigoJurosMora.Percentual:
                    return 2;
                case Enums.CodigoJurosMora.Isento:
                    return 3;
                default: return 0;
            }
        }

        public int CodigoProteso(bool protestar = true)
        {
            return 0;
        }
    }
}
