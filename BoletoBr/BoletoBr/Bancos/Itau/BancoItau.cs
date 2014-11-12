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
                string[] cv = new string[] { "107", "109", "121", "122", "126", "131", "142", "143", "145", "146", "150", "168", "169", "175", "176", "178", "196", "198" };
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

                //Verifica se o tamanho para o NossoNumero s�o 8 d�gitos
                //if (boleto.NossoNumeroFormatado.ToString().Length > 8)
                //    throw new NotImplementedException("A quantidade de dígitos do nosso número para a carteira " +
                //                                      boleto.CarteiraCobranca.Codigo + ", s�o 8 n�meros.");
                //else if (boleto.NossoNumeroFormatado.ToString().Length < 8)
                //    boleto.SetNossoNumeroFormatado(boleto.SequencialNossoNumero.PadLeft(8, '0'));

                //� obrigat�rio o preenchimento do n�mero do documento
                if (boleto.CarteiraCobranca.Codigo == "106" || boleto.CarteiraCobranca.Codigo == "107" || boleto.CarteiraCobranca.Codigo == "122" ||
                    boleto.CarteiraCobranca.Codigo == "142" || boleto.CarteiraCobranca.Codigo == "143" || boleto.CarteiraCobranca.Codigo == "195" ||
                    boleto.CarteiraCobranca.Codigo == "196" || boleto.CarteiraCobranca.Codigo == "198")
                {
                    if (String.IsNullOrEmpty(boleto.NumeroDocumento.Replace("-", "").TrimStart('0')))
                        throw new NotImplementedException("O número do documento não pode ser igual a zero.");
                }

                //Formato o n�mero do documento 
                if (!String.IsNullOrEmpty(boleto.NumeroDocumento.Replace("-", "")) && boleto.NumeroDocumento.Replace("-", "").Length < 7)
                    boleto.NumeroDocumento = boleto.NumeroDocumento.Replace("-", "").PadLeft(7, '0');

                // Calcula o DAC do Nosso N�mero a maioria das carteiras
                // agencia/conta/carteira/nosso numero
                //if (boleto.CarteiraCobranca.Codigo != "126" && boleto.CarteiraCobranca.Codigo != "131"
                //    && boleto.CarteiraCobranca.Codigo != "145" && boleto.CarteiraCobranca.Codigo != "150"
                //    && boleto.CarteiraCobranca.Codigo != "168")
                //    _dacNossoNumero = Common.Mod10(boleto.CedenteBoleto.ContaBancariaCedente.Agencia + boleto.CedenteBoleto.ContaBancariaCedente.Conta +
                //              boleto.CarteiraCobranca.Codigo + boleto.NossoNumeroFormatado);
                //else
                //    // Excess�o 126 - 131 - 146 - 150 - 168
                //    // carteira/nosso numero
                //    _dacNossoNumero = Common.Mod10(boleto.CarteiraCobranca + boleto.NossoNumeroFormatado);

                // Calcula o DAC da Conta Corrente
                boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta =
                    Common.Mod10(boleto.CedenteBoleto.ContaBancariaCedente.Agencia + boleto.CedenteBoleto.ContaBancariaCedente.Conta).ToString();

                //Verifica se o nosso n�mero � v�lido
                if (Convert.ToInt64(boleto.NossoNumeroFormatado.Replace("/", "").Replace("-", "")) == 0)
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
            var sequencialNN = boleto.SequencialNossoNumero.Replace("-", "");

            if (boleto.CarteiraCobranca.Codigo == "104" || /* Escritural */
                boleto.CarteiraCobranca.Codigo == "112" || /* Escritural */
                boleto.CarteiraCobranca.Codigo == "126" ||
                boleto.CarteiraCobranca.Codigo == "131" ||
                boleto.CarteiraCobranca.Codigo == "138" || /* Escritural */
                boleto.CarteiraCobranca.Codigo == "145" ||
                boleto.CarteiraCobranca.Codigo == "147" || /* Escritural */
                boleto.CarteiraCobranca.Codigo == "150" ||
                boleto.CarteiraCobranca.Codigo == "168")

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
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            try
            {
                /*
                 * C�digo de Barras
                 * banco & moeda & fator & valor & carteira & nossonumero & dac_nossonumero & agencia & conta & dac_conta & "000"
                 */
                string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
                valorBoleto = valorBoleto.PadLeft(10, '0');

                string numeroDocumento = boleto.NumeroDocumento.PadLeft(7, '0');
                string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(5, '0');
                
                if ((boleto.CarteiraCobranca.Codigo == "109") || (boleto.CarteiraCobranca.Codigo == "198") ||
                    (boleto.CarteiraCobranca.Codigo == "121") || (boleto.CarteiraCobranca.Codigo == "175") ||
                    (boleto.CarteiraCobranca.Codigo == "176") || (boleto.CarteiraCobranca.Codigo == "178"))
                    
                    boleto.CodigoBarraBoleto =
                        string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}000", CodigoBanco, MoedaBanco,
                                      Common.FatorVencimento(boleto.DataVencimento), valorBoleto, boleto.CarteiraCobranca.Codigo,
                                      boleto.SequencialNossoNumero.Replace("-", ""), _dacNossoNumero, boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                                      boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(5, '0'), boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);
                         
                else if ((boleto.CarteiraCobranca.Codigo == "107") || (boleto.CarteiraCobranca.Codigo == "122") ||
                    (boleto.CarteiraCobranca.Codigo == "142") || (boleto.CarteiraCobranca.Codigo == "143") ||
                    (boleto.CarteiraCobranca.Codigo == "196"))
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}0", CodigoBanco, MoedaBanco,
                        Common.FatorVencimento(boleto.DataVencimento), boleto.ValorBoleto, boleto.CarteiraCobranca.Codigo,
                        boleto.SequencialNossoNumero.Replace("-", ""), numeroDocumento, codigoCedente,
                        Common.Mod10(boleto.CarteiraCobranca.Codigo + boleto.SequencialNossoNumero + numeroDocumento + codigoCedente));
                }

                _dacBoleto = Common.Mod11(boleto.CodigoBarraBoleto, 9, 0);

                boleto.CodigoBarraBoleto = Common.Left(boleto.CodigoBarraBoleto, 4) +
                    _dacBoleto + Common.Right(boleto.CodigoBarraBoleto, 39);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("<BoletoBr>" +
                    "{0}Mensagem: Falha ao formatar código de barras.", Environment.NewLine), ex);
            }
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {
            try
            {
                var numeroDocumento = boleto.NumeroDocumento.Replace("-", "").PadLeft(7, '0');
                var codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(5, '0');

                // ReSharper disable once InconsistentNaming
                var AAA = CodigoBanco;
                // ReSharper disable once InconsistentNaming
                var B = MoedaBanco;
                // ReSharper disable once InconsistentNaming
                var CCC = boleto.CarteiraCobranca.Codigo;
                // ReSharper disable once InconsistentNaming
                var DD = boleto.NossoNumeroFormatado.Replace("/", "").Replace("-", "").Substring(0, 2);
                // ReSharper disable once InconsistentNaming
                var X = Common.Mod10(AAA + B + CCC + DD).ToString();
                // ReSharper disable once InconsistentNaming
                var LD = string.Empty; /* Linha Digit�vel */

                // ReSharper disable once InconsistentNaming
                var DDDDDD = boleto.NossoNumeroFormatado.Replace("/", "").Replace("-", "").Substring(2, 6);

                // ReSharper disable once InconsistentNaming
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

                if (boleto.CarteiraCobranca.Codigo == "109" || boleto.CarteiraCobranca.Codigo == "121" || boleto.CarteiraCobranca.Codigo == "175" || boleto.CarteiraCobranca.Codigo == "176" || boleto.CarteiraCobranca.Codigo == "178")
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
                    string GGGGGG = boleto.CedenteBoleto.ContaBancariaCedente.Conta + boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta;
                    string HHH = "000";
                    string Z = Common.Mod10(F + GGGGGG + HHH).ToString();

                    C3 = string.Format("{0}{1}.{2}{3}{4}", F, GGGGGG.Substring(0, 4), GGGGGG.Substring(4, 2), HHH, Z);

                    #endregion FGGGG.GGHHHZ
                }
                else if (boleto.CarteiraCobranca.Codigo == "107" || boleto.CarteiraCobranca.Codigo == "122" || boleto.CarteiraCobranca.Codigo == "142"
                         || boleto.CarteiraCobranca.Codigo == "143" || boleto.CarteiraCobranca.Codigo == "196" || boleto.CarteiraCobranca.Codigo == "198")
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
                    var G = Common.Mod10(boleto.CarteiraCobranca.Codigo + boleto.SequencialNossoNumero.PadLeft(8, '0') + numeroDocumento + codigoCedente).ToString();
                    var H = "0";
                    var Z = Common.Mod10(EEE + FFFFF + G + H).ToString();
                    C3 = string.Format("{0}{1}.{2}{3}{4}{5}", EEE, FFFFF.Substring(0, 2), FFFFF.Substring(2, 3), G, H, Z);

                    #endregion EEEFF.FFFGHZ
                }
                else if (boleto.CarteiraCobranca.Codigo == "126" || boleto.CarteiraCobranca.Codigo == "131" || boleto.CarteiraCobranca.Codigo == "146" ||
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
            if (String.IsNullOrEmpty(boleto.SequencialNossoNumero) ||
                String.IsNullOrEmpty(boleto.SequencialNossoNumero.TrimStart('0')))
                throw new Exception("Sequencial Nosso Número não foi informado.");

            var sequencialNN = boleto.SequencialNossoNumero.Substring(0, 8);

            // Usando Método e Geração do DAC do Nosso Número
            GerarDacNossoNumero(boleto);
            try
            {
                boleto.SetNossoNumeroFormatado(string.Format("{0}/{1}-{2}", boleto.CarteiraCobranca.Codigo,
                    sequencialNN, _dacNossoNumero));
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

            if (boleto.NumeroDocumento.Length > 9)
                numeroDoDocumento = boleto.NumeroDocumento.Substring(0, 9);
            else
                numeroDoDocumento = boleto.NumeroDocumento.PadLeft(9, '0');

            digitoNumeroDoDocumento = Common.Mod10(numeroDoDocumento).ToString();
            numeroDoDocumentoFormatado = String.Format("{0}-{1}", numeroDoDocumento, digitoNumeroDoDocumento);

            boleto.NumeroDocumento = numeroDoDocumentoFormatado;
            //boleto.DigitoNumeroDocumento = digitoNumeroDoDocumento;
            //boleto.NumeroDocumentoFormatado = numeroDoDocumentoFormatado;
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
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 01,
                            Descricao = "Duplicata Mercantil",
                            Sigla = "DM"
                        };
                    }
                case EnumEspecieDocumento.NotaPromissoria:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 02,
                            Descricao = "Nota Promissória",
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
                case EnumEspecieDocumento.MensalidadeEscolar:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 04,
                            Descricao = "Mensalidade Escolar",
                            Sigla = "ME"
                        };
                    }
                case EnumEspecieDocumento.Recibo:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 05,
                            Descricao = "Recibo",
                            Sigla = "RC"
                        };
                    }
                case EnumEspecieDocumento.Contrato:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 06,
                            Descricao = "Contrato",
                            Sigla = "CT"
                        };
                    }
                case EnumEspecieDocumento.Cosseguros:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 07,
                            Descricao = "Cosseguros",
                            Sigla = "CO"
                        };
                    }
                case EnumEspecieDocumento.DuplicataServico:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 08,
                            Descricao = "Duplicata de Serviço",
                            Sigla = "DS"
                        };
                    }
                case EnumEspecieDocumento.LetraCambio:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 09,
                            Descricao = "Letra de Câmbio",
                            Sigla = "LC"
                        };
                    }
                case EnumEspecieDocumento.NotaDebito:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 13,
                            Descricao = "Nota de Débitos",
                            Sigla = "ND"
                        };
                    }
                case EnumEspecieDocumento.DocumentoDivida:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 15,
                            Descricao = "Documento de Dívida",
                            Sigla = "DD"
                        };
                    }
                case EnumEspecieDocumento.EncargosCondominais:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 16,
                            Descricao = "Encargos Condominiais",
                            Sigla = "EC"
                        };
                    }
                case EnumEspecieDocumento.ContaPrestacaoServicos:
                    {
                        return new EspecieDocumento((int)especie)
                        {
                            Codigo = 17,
                            Descricao = "Conta de Prestação de Serviços",
                            Sigla = "PS"
                        };
                    }
                case EnumEspecieDocumento.Diversos:
                    {
                        return new EspecieDocumento((int)especie)
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

        public IInstrucao ObtemInstrucaoPadronizada(EnumTipoInstrucao tipoInstrucao, double valorInstrucao, DateTime dataInstrucao, int diasInstrucao)
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
            throw new Exception(String.Format("Não foi possível obter instrução padronizada. Banco: {0} Código Instrução: {1} Qtd Dias/Valor: {2}",
                CodigoBanco, tipoInstrucao.ToString(), valorInstrucao));
        }

        public ICodigoOcorrencia ObtemCodigoOcorrencia(EnumCodigoOcorrenciaRemessa ocorrencia, double valorOcorrencia, DateTime dataOcorrencia)
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
                throw new ApplicationException("Arquivo informado é inválido.");

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
    }
}
