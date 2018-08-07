using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Itau
{
    public class BancoItau : IBanco
    {
        private int _dacBoleto = 0;
        private int _dacNossoNumero = 0;

        public BancoItau()
        {
            CodigoBanco = "341";
            DigitoBanco = "7";
            NomeBanco = "Itaú";
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
                //Carteiras v�lidas
                string[] cv = new string[]
                {
                    "107", "109", "112", "121", "122", "126", "131", "142", "143", "145", "146", "150", "168", "169", "175", "176",
                    "178", "196", "198"
                };
                bool valida = false;

                foreach (string c in cv)
                    if ((boleto.CarteiraCobranca.Codigo) == c.ToString(CultureInfo.InvariantCulture))
                        valida = true;

                if (!valida)
                {
                    var carteirasImplementadas = new StringBuilder(100);

                    carteirasImplementadas.Append(". Carteiras implementadas: ");
                    foreach (string c in cv)
                    {
                        carteirasImplementadas.AppendFormat(" {0}", c);
                    }
                    throw new NotImplementedException("Carteira não implementada: " + boleto.CarteiraCobranca.Codigo +
                                                      carteirasImplementadas);
                }

                //� obrigat�rio o preenchimento do n�mero do documento
                if (boleto.CarteiraCobranca.Codigo == "106" || boleto.CarteiraCobranca.Codigo == "107" ||
                    boleto.CarteiraCobranca.Codigo == "109" || boleto.CarteiraCobranca.Codigo == "112" ||
                    boleto.CarteiraCobranca.Codigo == "122" ||
                    boleto.CarteiraCobranca.Codigo == "142" || boleto.CarteiraCobranca.Codigo == "143" ||
                    boleto.CarteiraCobranca.Codigo == "195" ||
                    boleto.CarteiraCobranca.Codigo == "196" || boleto.CarteiraCobranca.Codigo == "198")
                {
                    if (String.IsNullOrEmpty(boleto.NumeroDocumento.Replace("-", "").TrimStart('0')))
                        throw new NotImplementedException("O número do documento não pode ser igual a zero.");
                }

                //Formato o n�mero do documento 
                if (!String.IsNullOrEmpty(boleto.NumeroDocumento.Replace("-", "")) &&
                    boleto.NumeroDocumento.Replace("-", "").Length < 8)
                    boleto.NumeroDocumento = boleto.NumeroDocumento.Replace("-", "").PadLeft(8, '0');

                // Calcula o DAC da Conta Corrente
                boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta =
                    Common.Mod10(boleto.CedenteBoleto.ContaBancariaCedente.Agencia +
                                 boleto.CedenteBoleto.ContaBancariaCedente.Conta).ToString();

                //Verifica se o nosso n�mero � v�lido
                if (Convert.ToInt64(boleto.IdentificadorInternoBoleto.Replace("/", "").Replace("-", "")) == 0)
                    throw new NotImplementedException("Nosso número inválido");

                //Verifica se data do processamento � valida
                //if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
                if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                    boleto.DataProcessamento = DateTime.Now;

                //Verifica se data do documento � valida
                //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
                if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                    boleto.DataDocumento = DateTime.Now;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao validar boleto (s).", ex);
            }
        }

        public void GerarDacNossoNumero(Boleto boleto)
        {
            /*
             * Para todas as carteiras de cobrança do Banco Itaú o DAC do "Nosso Número" é calculado a partir dos campos:
             * Agência, Conta do Cedente (sem DAC), Número da carteira e "Nosso Número",
             * EXCETO
             * As carteiras escriturais e na modalidade direta as carteiras 126, 131, 145, 150 e 168,
             * cujo DAC do "Nosso Número" é composto apenas dos campos:
             * Carteira e Nosso Número, mas todos calculados através do Módulo 10.
             */
            var sequencialNN = boleto.IdentificadorInternoBoleto.Replace("-", "");
            if (sequencialNN.Length < 8) sequencialNN = sequencialNN.PadLeft(8, '0');

            if (boleto.CarteiraCobranca.Codigo == "104" || /* Escritural */
                boleto.CarteiraCobranca.Codigo == "126" ||
                boleto.CarteiraCobranca.Codigo == "131" ||
                boleto.CarteiraCobranca.Codigo == "138" || /* Escritural */
                boleto.CarteiraCobranca.Codigo == "145" ||
                boleto.CarteiraCobranca.Codigo == "147" || /* Escritural */
                boleto.CarteiraCobranca.Codigo == "150" ||
                boleto.CarteiraCobranca.Codigo == "168" ||
                boleto.CarteiraCobranca.Codigo == "112")
                _dacNossoNumero = Common.Mod10(boleto.CarteiraCobranca.Codigo + sequencialNN);
            else
                _dacNossoNumero = Common.Mod10(
                    boleto.CedenteBoleto.ContaBancariaCedente.Agencia +
                    boleto.CedenteBoleto.ContaBancariaCedente.Conta +
                    boleto.CarteiraCobranca.Codigo +
                    sequencialNN);
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

            boleto.CedenteBoleto.CodigoCedenteFormatado = String.Format("{0}/{1}-{2}",
                boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'),
                boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(5, '0'),
                boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            try
            {
                /*
                 * Código de Barras
                 * banco & moeda & fator & valor & carteira & nossonumero & dac_nossonumero & agencia & conta & dac_conta & "000"
                 */
                string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
                valorBoleto = valorBoleto.PadLeft(10, '0');

                string numeroDocumento = boleto.NumeroDocumento.PadLeft(7, '0');
                string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(5, '0');

                if ((boleto.CarteiraCobranca.Codigo == "109") || (boleto.CarteiraCobranca.Codigo == "112") ||
                    (boleto.CarteiraCobranca.Codigo == "198") ||
                    (boleto.CarteiraCobranca.Codigo == "121") || (boleto.CarteiraCobranca.Codigo == "175") ||
                    (boleto.CarteiraCobranca.Codigo == "176") || (boleto.CarteiraCobranca.Codigo == "178"))
                {
                    boleto.CodigoBarraBoleto =
                        string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}000", CodigoBanco, MoedaBanco,
                            Common.FatorVencimento(boleto.DataVencimento), valorBoleto, boleto.CarteiraCobranca.Codigo,
                            boleto.IdentificadorInternoBoleto.PadLeft(8, '0'), _dacNossoNumero,
                            boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                            boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(5, '0'),
                            boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);
                }
                else if ((boleto.CarteiraCobranca.Codigo == "107") || (boleto.CarteiraCobranca.Codigo == "122") ||
                         (boleto.CarteiraCobranca.Codigo == "142") || (boleto.CarteiraCobranca.Codigo == "143") ||
                         (boleto.CarteiraCobranca.Codigo == "196"))
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}0", CodigoBanco, MoedaBanco,
                        Common.FatorVencimento(boleto.DataVencimento), boleto.ValorBoleto,
                        boleto.CarteiraCobranca.Codigo,
                        boleto.IdentificadorInternoBoleto.PadLeft(8, '0'), numeroDocumento, codigoCedente,
                        Common.Mod10(boleto.CarteiraCobranca.Codigo + boleto.IdentificadorInternoBoleto +
                                     numeroDocumento + codigoCedente));
                }

                _dacBoleto = Common.Mod11(boleto.CodigoBarraBoleto, 9, 0);

                boleto.CodigoBarraBoleto = Common.Left(boleto.CodigoBarraBoleto, 4) +
                                           _dacBoleto + Common.Right(boleto.CodigoBarraBoleto, 39);
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
                var numeroDocumento = boleto.NumeroDocumento.Replace("-", "").PadLeft(7, '0');
                var codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(5, '0');

                var AAA = CodigoBanco;
                var B = MoedaBanco;
                var CCC = boleto.CarteiraCobranca.Codigo;
                
                // PEGAR O NÚMERO DOCUMENTO JÁ FORMATADO COM ZEROS
                var DD = boleto.NumeroDocumento.Replace("/", "").Replace("-", "").Substring(0, 2);
                var X = Common.Mod10(AAA + B + CCC + DD).ToString();
                var LD = string.Empty; /* Linha Digitável */

                // PEGAR O NÚMERO DOCUMENTO JÁ FORMATADO COM ZEROS
                var DDDDDD = boleto.NumeroDocumento.Replace("/", "").Replace("-", "").Substring(2, 6);

                var K = string.Format(" {0} ", _dacBoleto);

                var UUUU = Common.FatorVencimento(boleto.DataVencimento).ToString();
                var VVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");

                var C1 = string.Empty;
                var C2 = string.Empty;
                var C3 = string.Empty;
                var C5 = string.Empty;

                #region AAABC.CCDDX

                C1 = string.Format("{0}{1}{2}.", AAA, B, CCC.Substring(0, 1));
                C1 += string.Format("{0}{1}{2} ", CCC.Substring(1, 2), DD, X);

                #endregion AAABC.CCDDX

                #region UUUUVVVVVVVVVV

                VVVVVVVVVV = VVVVVVVVVV.PadLeft(10, '0');
                C5 = UUUU + VVVVVVVVVV;

                #endregion UUUUVVVVVVVVVV

                if (boleto.CarteiraCobranca.Codigo == "109" || boleto.CarteiraCobranca.Codigo == "112" ||
                    boleto.CarteiraCobranca.Codigo == "121" || boleto.CarteiraCobranca.Codigo == "175" || 
                    boleto.CarteiraCobranca.Codigo == "176" || boleto.CarteiraCobranca.Codigo == "178")
                {
                    #region Definições Carteiras 109 - 121 - 175 - 176 - 178

                    /* AAABC.CCDDX.DDDDD.DEFFFY.FGGGG.GGHHHZ.K.UUUUVVVVVVVVVV
                     * ------------------------------------------------------
                     * Campo 1
                     * AAABC.CCDDX
                     * AAA - C�digo do Banco
                     * B   - Moeda
                     * CCC - Carteira
                     * DD  - 2 primeiros n�meros Nosso N�mero
                     * X   - DAC Campo 1 (AAABC.CCDD) Mod10
                     * 
                     * Campo 2
                     * DDDDD.DEFFFY
                     * DDDDD.D - Restante Nosso N�mero
                     * E       - DAC (Ag�ncia/Conta/Carteira/Nosso N�mero)
                     * FFF     - Tr�s primeiros da ag�ncia
                     * Y       - DAC Campo 2 (DDDDD.DEFFF) Mod10
                     * 
                     * Campo 3
                     * FGGGG.GGHHHZ
                     * F       - Restante da Ag�ncia
                     * GGGG.GG - N�mero Conta Corrente + DAC
                     * HHH     - Zeros (N�o utilizado)
                     * Z       - DAC Campo 3
                     * 
                     * Campo 4
                     * K       - DAC C�digo de Barras
                     * 
                     * Campo 5
                     * UUUUVVVVVVVVVV
                     * UUUU       - Fator Vencimento
                     * VVVVVVVVVV - Valor do T�tulo 
                     */

                    #endregion Defini��es

                    #region DDDDD.DEFFFY

                    string E = _dacNossoNumero.ToString();
                    string FFF = boleto.CedenteBoleto.ContaBancariaCedente.Agencia.Substring(0, 3);
                    string Y = Common.Mod10(DDDDDD + E + FFF).ToString();

                    C2 = string.Format("{0}.", DDDDDD.Substring(0, 5));
                    C2 += string.Format("{0}{1}{2}{3} ", DDDDDD.Substring(5, 1), E, FFF, Y);

                    #endregion DDDDD.DEFFFY

                    #region FGGGG.GGHHHZ

                    string F = boleto.CedenteBoleto.ContaBancariaCedente.Agencia.Substring(3, 1);
                    string GGGGGG = boleto.CedenteBoleto.ContaBancariaCedente.Conta +
                                    boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta;
                    string HHH = "000";
                    string Z = Common.Mod10(F + GGGGGG + HHH).ToString();

                    C3 = string.Format("{0}{1}.{2}{3}{4}", F, GGGGGG.Substring(0, 4), GGGGGG.Substring(4, 2), HHH, Z);

                    #endregion FGGGG.GGHHHZ
                }
                else if (boleto.CarteiraCobranca.Codigo == "107" || boleto.CarteiraCobranca.Codigo == "122" ||
                         boleto.CarteiraCobranca.Codigo == "142"
                         || boleto.CarteiraCobranca.Codigo == "143" || boleto.CarteiraCobranca.Codigo == "196" ||
                         boleto.CarteiraCobranca.Codigo == "198")
                {

                    #region Definições Carteiras 107 - 122 - 142 - 143 - 196 - 198

                    /* AAABC.CCDDX.DDDDD.DEEEEY.EEEFF.FFFGHZ.K.UUUUVVVVVVVVVV
                    * ------------------------------------------------------
                    * Campo 1 - AAABC.CCDDX
                    * AAA - C�digo do Banco
                    * B   - Moeda
                    * CCC - Carteira
                    * DD  - 2 primeiros n�meros Nosso N�mero
                    * X   - DAC Campo 1 (AAABC.CCDD) Mod10
                    * 
                    * Campo 2 - DDDDD.DEEEEY
                    * DDDDD.D - Restante Nosso N�mero
                    * EEEE    - 4 primeiros numeros do n�mero do documento
                    * Y       - DAC Campo 2 (DDDDD.DEEEEY) Mod10
                    * 
                    * Campo 3 - EEEFF.FFFGHZ
                    * EEE     - Restante do n�mero do documento
                    * FFFFF   - C�digo do Cliente
                    * G       - DAC (Carteira/Nosso Numero(sem DAC)/Numero Documento/Codigo Cliente)
                    * H       - zero
                    * Z       - DAC Campo 3
                    * 
                    * Campo 4 - K
                    * K       - DAC C�digo de Barras
                    * 
                    * Campo 5 - UUUUVVVVVVVVVV
                    * UUUU       - Fator Vencimento
                    * VVVVVVVVVV - Valor do T�tulo 
                      */

                    #endregion Defini��es

                    #region DDDDD.DEEEEY

                    var EEEE = numeroDocumento.Substring(0, 4);
                    var Y = Common.Mod10(DDDDDD + EEEE).ToString();

                    C2 = string.Format("{0}.", DDDDDD.Substring(0, 5));
                    C2 += string.Format("{0}{1}{2} ", DDDDDD.Substring(5, 1), EEEE, Y);

                    #endregion DDDDD.DEEEEY

                    #region EEEFF.FFFGHZ

                    var EEE = numeroDocumento.Substring(4, 3);
                    var FFFFF = codigoCedente;
                    var G =
                        Common.Mod10(boleto.CarteiraCobranca.Codigo +
                                     boleto.IdentificadorInternoBoleto.PadLeft(8, '0') + numeroDocumento +
                                     codigoCedente).ToString();
                    var H = "0";
                    var Z = Common.Mod10(EEE + FFFFF + G + H).ToString();
                    C3 = string.Format("{0}{1}.{2}{3}{4}{5}", EEE, FFFFF.Substring(0, 2), FFFFF.Substring(2, 3), G,
                        H, Z);

                    #endregion EEEFF.FFFGHZ
                }
                else if (boleto.CarteiraCobranca.Codigo == "126" || boleto.CarteiraCobranca.Codigo == "131" ||
                         boleto.CarteiraCobranca.Codigo == "146" ||
                         boleto.CarteiraCobranca.Codigo == "150" || boleto.CarteiraCobranca.Codigo == "168")
                {
                    throw new NotImplementedException("Função não implementada.");
                }

                boleto.LinhaDigitavelBoleto = C1 + C2 + C3 + K + C5;
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
            if (String.IsNullOrEmpty(boleto.IdentificadorInternoBoleto) ||
                String.IsNullOrEmpty(boleto.IdentificadorInternoBoleto.TrimStart('0')))
                throw new Exception("Sequencial Nosso Número não foi informado.");

            // Usando Método e Geração do DAC do Nosso Número
            GerarDacNossoNumero(boleto);

            try
            {
                var identificadorBoleto = boleto.IdentificadorInternoBoleto;
                if (identificadorBoleto.Length < 8) identificadorBoleto = identificadorBoleto.PadLeft(8, '0');

                boleto.SetNossoNumeroFormatado(string.Format("{0}/{1}-{2}", boleto.CarteiraCobranca.Codigo,
                    identificadorBoleto, _dacNossoNumero));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("<BoletoBr>" +
                                                  "{0}Mensagem: Falha ao formatar nosso número." +
                                                  "{0}Carteira: " + boleto.CarteiraCobranca.Codigo +
                                                  "{0}Numeração Sequencial: " + boleto.NossoNumeroFormatado + " - " +
                                                  "DAC: " + _dacNossoNumero, Environment.NewLine), ex);
            }
        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            string numeroDoDocumento;
            string digitoNumeroDoDocumento;
            string numeroDoDocumentoFormatado;

            if (String.IsNullOrEmpty(boleto.NumeroDocumento))
                throw new Exception("O número do documento não foi informado.");

            if (boleto.NumeroDocumento.Length > 8)
                numeroDoDocumento = boleto.NumeroDocumento.Substring(0, 8);
            else
                numeroDoDocumento = boleto.NumeroDocumento.PadLeft(8, '0');

            digitoNumeroDoDocumento = Common.Mod10(numeroDoDocumento).ToString();
            numeroDoDocumentoFormatado = String.Format("{0}-{1}", numeroDoDocumento, digitoNumeroDoDocumento);

            boleto.NumeroDocumento = numeroDoDocumentoFormatado;
        }

        public ICodigoOcorrencia ObtemCodigoOcorrenciaByInt(int numeroOcorrencia)
        {
            switch (numeroOcorrencia)
            {
                case 02:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 02,
                        Descricao = "ENTRADA CONFIRMADA"/* COM POSSIBILIDADE DE MENSAGEM*/
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
                        Descricao = "ALTERAÇÃO DE DADOS - NOVA ENTRADA OU ALTERAÇÃO/EXCLUSÃO DE DADOS ACATADA"
                    };
                case 05:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 05,
                        Descricao = "ALTERAÇÃO DE DADOS – BAIXA"
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
                        Descricao = "LIQUIDAÇÃO PARCIAL – COBRANÇA INTELIGENTE (B2B)"
                    };
                case 08:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 08,
                        Descricao = "LIQUIDAÇÃO PARCIAL – COBRANÇA INTELIGENTE (B2B)"
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
                        Descricao = "BAIXA POR TER SIDO LIQUIDADO"
                    };
                case 11:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 11,
                        Descricao = "EM SER (SÓ NO RETORNO MENSAL)"
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
                case 15:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 15,
                        Descricao = "BAIXAS REJEITADAS"
                    };
                case 16:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 16,
                        Descricao = "INSTRUÇÕES REJEITADAS"
                    };
                case 17:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 17,
                        Descricao = "ALTERAÇÃO/EXCLUSÃO DE DADOS REJEITADOS"
                    };
                case 18:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 18,
                        Descricao = "COBRANÇA CONTRATUAL - INSTRUÇÕES/ALTERAÇÕES REJEITADAS/PENDENTES"
                    };
                case 19:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 19,
                        Descricao = "CONFIRMA RECEBIMENTO DE INSTRUÇÃO DE PROTESTO"
                    };
                case 20:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 20,
                        Descricao = "CONFIRMA RECEBIMENTO DE INSTRUÇÃO DE SUSTAÇÃO DE PROTESTO /TARIFA"
                    };
                case 21:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 21,
                        Descricao = "CONFIRMA RECEBIMENTO DE INSTRUÇÃO DE NÃO PROTESTAR"
                    };
                case 23:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 23,
                        Descricao = "TÍTULO ENVIADO A CARTÓRIO/TARIFA"
                    };
                case 24:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 24,
                        Descricao = "INSTRUÇÃO DE PROTESTO REJEITADA / SUSTADA / PENDENTE"
                    };
                case 25:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 25,
                        Descricao = "ALEGAÇÕES DO SACADO"
                    };
                case 26:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 26,
                        Descricao = "TARIFA DE AVISO DE COBRANÇA"
                    };
                case 27:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 27,
                        Descricao = "TARIFA DE EXTRATO POSIÇÃO (B40X)"
                    };
                case 28:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 28,
                        Descricao = "TARIFA DE RELAÇÃO DAS LIQUIDAÇÕES"
                    };
                case 29:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 29,
                        Descricao = "TARIFA DE MANUTENÇÃO DE TÍTULOS VENCIDOS"
                    };
                case 30:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 30,
                        Descricao = "DÉBITO MENSAL DE TARIFAS (PARA ENTRADAS E BAIXAS)"
                    };
                case 32:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 32,
                        Descricao = "BAIXA POR TER SIDO PROTESTADO"
                    };
                case 33:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 33,
                        Descricao = "CUSTAS DE PROTESTO"
                    };
                case 34:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 34,
                        Descricao = "CUSTAS DE SUSTAÇÃO"
                    };
                case 35:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 35,
                        Descricao = "CUSTAS DE CARTÓRIO DISTRIBUIDOR"
                    };
                case 36:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 36,
                        Descricao = "CUSTAS DE EDITAL"
                    };
                case 37:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 37,
                        Descricao = "TARIFA DE EMISSÃO DE BOLETO/TARIFA DE ENVIO DE DUPLICATA"
                    };
                case 38:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 38,
                        Descricao = "TARIFA DE INSTRUÇÃO"
                    };
                case 39:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 39,
                        Descricao = "TARIFA DE OCORRÊNCIAS"
                    };
                case 40:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 40,
                        Descricao = "TARIFA MENSAL DE EMISSÃO DE BOLETO/TARIFA MENSAL DE ENVIO DE DUPLICATA"
                    };
                case 41:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 41,
                        Descricao = "DÉBITO MENSAL DE TARIFAS – EXTRATO DE POSIÇÃO (B4EP/B4OX)"
                    };
                case 42:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 42,
                        Descricao = "DÉBITO MENSAL DE TARIFAS – OUTRAS INSTRUÇÕES"
                    };
                case 43:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 43,
                        Descricao = "DÉBITO MENSAL DE TARIFAS – MANUTENÇÃO DE TÍTULOS VENCIDOS"
                    };
                case 44:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 44,
                        Descricao = "DÉBITO MENSAL DE TARIFAS – OUTRAS OCORRÊNCIAS"
                    };
                case 45:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 45,
                        Descricao = "DÉBITO MENSAL DE TARIFAS – PROTESTO"
                    };
                case 46:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 46,
                        Descricao = "DÉBITO MENSAL DE TARIFAS – SUSTAÇÃO DE PROTESTO"
                    };
                case 47:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 47,
                        Descricao = "BAIXA COM TRANSFERÊNCIA PARA DESCONTO"
                    };
                case 48:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 48,
                        Descricao = "CUSTAS DE SUSTAÇÃO JUDICIAL"
                    };
                case 51:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 51,
                        Descricao = "TARIFA MENSAL REF A ENTRADAS BANCOS CORRESPONDENTES NA CARTEIRA"
                    };
                case 52:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 52,
                        Descricao = "TARIFA MENSAL BAIXAS NA CARTEIRA"
                    };
                case 53:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 53,
                        Descricao = "TARIFA MENSAL BAIXAS EM BANCOS CORRESPONDENTES NA CARTEIRA"
                    };
                case 54:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 54,
                        Descricao = "TARIFA MENSAL DE LIQUIDAÇÕES NA CARTEIRA"
                    };
                case 55:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 55,
                        Descricao = "TARIFA MENSAL DE LIQUIDAÇÕES EM BANCOS CORRESPONDENTES NA CARTEIRA"
                    };
                case 56:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 56,
                        Descricao = "CUSTAS DE IRREGULARIDADE"
                    };
                case 57:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 57,
                        Descricao = "INSTRUÇÃO CANCELADA"
                    };
                case 59:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 59,
                        Descricao = "BAIXA POR CRÉDITO EM C/C ATRAVÉS DO SISPAG"
                    };
                case 60:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 60,
                        Descricao = "ENTRADA REJEITADA CARNÊ"
                    };
                case 61:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 61,
                        Descricao = "TARIFA EMISSÃO AVISO DE MOVIMENTAÇÃO DE TÍTULOS (2154)"
                    };
                case 62:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 62,
                        Descricao = "DÉBITO MENSAL DE TARIFA - AVISO DE MOVIMENTAÇÃO DE TÍTULOS (2154)"
                    };
                case 63:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 63,
                        Descricao = "TÍTULO SUSTADO JUDICIALMENTE"
                    };
                case 64:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 64,
                        Descricao = "ENTRADA CONFIRMADA COM RATEIO DE CRÉDITO"
                    };
                case 69:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 69,
                        Descricao = "CHEQUE DEVOLVIDO"
                    };
                case 71:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 71,
                        Descricao = "ENTRADA REGISTRADA, AGUARDANDO AVALIAÇÃO"
                    };
                case 72:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 72,
                        Descricao = "BAIXA POR CRÉDITO EM C/C ATRAVÉS DO SISPAG SEM TÍTULO CORRESPONDENTE"
                    };
                case 73:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 73,
                        Descricao =
                            "CONFIRMAÇÃO DE ENTRADA NA COBRANÇA SIMPLES – ENTRADA NÃO ACEITA NA COBRANÇA CONTRATUAL"
                    };
                case 76:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 76,
                        Descricao = "CHEQUE COMPENSADO"
                    };
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter Código de Comando/Movimento/Ocorrência. Banco: {0} Código: {1}",
                    CodigoBanco, numeroOcorrencia.ToString()));
        }

        public ICodigoOcorrencia ObtemCodigoOcorrencia(EnumCodigoOcorrenciaRetorno ocorrenciaRetorno)
        {
            throw new NotImplementedException();
        }

        public IEspecieDocumento ObtemEspecieDocumento(EnumEspecieDocumento especie)
        {
            #region Código Espécie

            // 01 - DM - DUPLICATA MERCANTIL    
            // 02 - NP - NOTA PROMISSORIA   
            // 03 - NS - NOTA DE SEGURO
            // 04 - ME - MENSALIDADE ESCOLAR
            // 05 - RC - RECIBO
            // 06 - CT - CONTRATO
            // 07 - CO - COSSEGUROS
            // 08 - DS - DUPLICATA DE SERVICO
            // 09 - LC - LETRA DE CÂMBIO
            // 13 - ND - NOTA DE DÉBITOS
            // 15 - DD - DOCUMENTO DE DÍVIDA
            // 16 - EC - ENCARGOS CONDOMINIAIS
            // 17 - PS - CONTA DE PRESTAÇÃO DE SERVIÇOS
            // 99 - DV - DIVERSOS

            #endregion

            switch (especie)
            {
                case EnumEspecieDocumento.DuplicataMercantil:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 01,
                        Descricao = "Duplicata Mercantil",
                        Sigla = "DM"
                    };
                }
                case EnumEspecieDocumento.NotaPromissoria:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 02,
                        Descricao = "Nota Promissória",
                        Sigla = "NP"
                    };
                }
                case EnumEspecieDocumento.NotaSeguro:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 03,
                        Descricao = "Nota de Seguro",
                        Sigla = "NS"
                    };
                }
                case EnumEspecieDocumento.MensalidadeEscolar:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 04,
                        Descricao = "Mensalidade Escolar",
                        Sigla = "ME"
                    };
                }
                case EnumEspecieDocumento.Recibo:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 05,
                        Descricao = "Recibo",
                        Sigla = "RC"
                    };
                }
                case EnumEspecieDocumento.Contrato:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 06,
                        Descricao = "Contrato",
                        Sigla = "CT"
                    };
                }
                case EnumEspecieDocumento.Cosseguros:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 07,
                        Descricao = "Cosseguros",
                        Sigla = "CO"
                    };
                }
                case EnumEspecieDocumento.DuplicataServico:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 08,
                        Descricao = "Duplicata de Serviço",
                        Sigla = "DS"
                    };
                }
                case EnumEspecieDocumento.LetraCambio:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 09,
                        Descricao = "Letra de Câmbio",
                        Sigla = "LC"
                    };
                }
                case EnumEspecieDocumento.NotaDebito:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 13,
                        Descricao = "Nota de Débitos",
                        Sigla = "ND"
                    };
                }
                case EnumEspecieDocumento.DocumentoDivida:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 15,
                        Descricao = "Documento de Dívida",
                        Sigla = "DD"
                    };
                }
                case EnumEspecieDocumento.EncargosCondominais:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 16,
                        Descricao = "Encargos Condominiais",
                        Sigla = "EC"
                    };
                }
                case EnumEspecieDocumento.ContaPrestacaoServicos:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 17,
                        Descricao = "Conta de Prestação de Serviços",
                        Sigla = "PS"
                    };
                }
                case EnumEspecieDocumento.Diversos:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 99,
                        Descricao = "Diversos",
                        Sigla = "DV"
                    };
                }
            }
            throw new Exception(
                String.Format("Não foi possível obter espécie. Banco: {0} Código Espécie: {1}",
                    CodigoBanco, especie.ToString()));
        }

        public IInstrucao ObtemInstrucaoPadronizada(EnumTipoInstrucao tipoInstrucao, double valorInstrucao,
            DateTime dataInstrucao, int diasInstrucao)
        {
            switch (tipoInstrucao)
            {
                case EnumTipoInstrucao.Protestar:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 9,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Protestar após " + valorInstrucao + " dias úteis."
                    };
                }
                case EnumTipoInstrucao.NaoProtestar:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 10,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não protestar."
                    };
                }
                case EnumTipoInstrucao.DevolverApos90Dias:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 18,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Devolver após 90 dias do vencimento."
                    };
                }
                case EnumTipoInstrucao.ProtestarAposNDiasCorridos:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 34,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Protestar após " + diasInstrucao + " dias corridos do vencimento."
                    };
                }
                case EnumTipoInstrucao.ProtestarAposNDiasUteis:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 35,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Protestar após " + diasInstrucao + " dias úteis do vencimento."
                    };
                }
                case EnumTipoInstrucao.NaoReceberAposOVencimento:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 39,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber após o vencimento."
                    };
                }
                case EnumTipoInstrucao.ImportanciaPorDiaDeAtrasoAPartirDeDDMMAA:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 44,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Importância por dia de atraso a partir de " + dataInstrucao.ToString("ddmmyy")
                    };
                }
                case EnumTipoInstrucao.NoVencimentoPagavelEmQualquerAgencia:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 90,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "No vencimento pagável em qualquer agência bancária."
                    };
                }
                case EnumTipoInstrucao.NaoReceberAposNDiasCorridos:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 91,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber após " + diasInstrucao + " dias do vencimento."
                    };
                }
                case EnumTipoInstrucao.DevolverAposNDias:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 92,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Devolver após " + diasInstrucao + " dias do vencimento."
                    };
                }
                case EnumTipoInstrucao.MultaVencimento:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 997,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Cobrar multa após o vencimento."
                    };
                }
                case EnumTipoInstrucao.JurosdeMora:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 998,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Cobrar juros após o vencimento."
                    };
                }
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter instrução padronizada. Banco: {0} Código Instrução: {1} Qtd Dias/Valor: {2}",
                    CodigoBanco, tipoInstrucao.ToString(), valorInstrucao));
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
                        Descricao = "Remessa"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.Baixa:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 02,
                        Descricao = "Pedido de baixa"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.ConcessaoDeAbatimento:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 04,
                        Descricao = "Concessão de abatimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDeAbatimento:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 05,
                        Descricao = "Cancelamento de abatimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeVencimento:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 06,
                        Descricao = "Alteração do vencimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDoControleDoParticipante:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 07,
                        Descricao = "Alteração do uso da empresa"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoSeuNumero:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 08,
                        Descricao = "Alteração de seu número"
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
                case EnumCodigoOcorrenciaRemessa.NaoProtestar:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 10,
                        Descricao = "Não protestar"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.ProtestoParaFinsFalimentares:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 11,
                        Descricao = "Protesto para fins falimentares"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.SustarProtesto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 18,
                        Descricao = "Sustar o protesto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.ExclusaoDeSacadorAvalista:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 30,
                        Descricao = "Exclusão de sacador avalista"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeOutrosDados:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 31,
                        Descricao = "Alteração de outros dados"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.BaixaPorTerSidoPagoDiretamenteAoCedente:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 34,
                        Descricao = "Baixa por ter sido pago diretamente ao cedente"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDeInstrucao:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 35,
                        Descricao = "Cancelamento de instrução"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDoVencimentoESustarProtesto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 37,
                        Descricao = "Alteração do vencimento e sustar protesto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CedenteNaoConcordaComAlegacaoDoSacado:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 38,
                        Descricao = "Cedente não concorda com alegação do sacado"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CedenteSolicitaDispensaDeJuros:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
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

        public RetornoGenerico LerArquivoRetorno(List<string> linhasArquivo)
        {
            if (linhasArquivo == null || linhasArquivo.Any() == false)
                throw new ApplicationException("Arquivo informado é inválido/Não existem títulos no retorno.");

            /* Identifica o layout: 240 ou 400 */
            if (linhasArquivo.First().Length == 240)
            {
                //var leitor = new LeitorRetornoCnab240Itau(linhasArquivo);
                //var retornoProcessado = leitor.ProcessarRetorno();

                //var objRetornar = new RetornoGenerico(retornoProcessado);
                //return objRetornar;
            }
            if (linhasArquivo.First().Length == 400)
            {
                var leitor = new LeitorRetornoCnab400Itau(linhasArquivo);
                var retornoProcessado = leitor.ProcessarRetorno();

                var objRetornar = new RetornoGenerico(retornoProcessado);
                return objRetornar;
            }

            throw new Exception("Arquivo de RETORNO com " + linhasArquivo.First().Length + " posições, não é suportado.");
        }

        public RemessaCnab240 GerarArquivoRemessaCnab240(RemessaCnab240 remessaCnab240, List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }

        public RemessaCnab400 GerarArquivoRemessaCnab400(RemessaCnab400 remessaCnab400, List<Boleto> boletos)
        {
            throw new NotImplementedException();
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
