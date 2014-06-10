using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.BancoBrasil
{
    public class BancoBrasil : IBanco
    {
        #region Campos
        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }

        private string _digitoAutoConferenciaNossoNumero = String.Empty;
        private int _digitoAutoConferenciaBoleto = 0;
        #endregion
        public BancoBrasil()
        {
            this.CodigoBanco = "001";
            this.DigitoBanco = "9";
            this.NomeBanco = "Banco do Brasil";

            this._carteirasCobranca = new List<CarteiraCobranca>();
            this._carteirasCobranca.Add(new CarteiraCobrancaBancoBrasil17019());
            this._carteirasCobranca.Add(new CarteiraCobrancaBancoBrasil18019());
            this._carteirasCobranca.Add(new CarteiraCobrancaBancoBrasil31());
        }

        private readonly List<CarteiraCobranca> _carteirasCobranca;

        public List<CarteiraCobranca> GetCarteirasCobranca()
        {
            return _carteirasCobranca;
        }

        public CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira)
        {
            return this.GetCarteirasCobranca().FirstOrDefault(wh => wh.Codigo == codigoCarteira);
        }

        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            if (string.IsNullOrEmpty(boleto.CarteiraCobranca.Codigo))
                throw new NotImplementedException("Carteira não informada. Utilize a carteira 11, 16, 17, 18, 18-019, 18-027, 18-035, 18-140 ou 31.");

            //Verifica as carteiras implementadas
            if (!boleto.CarteiraCobranca.Codigo.Equals("11") &
                !boleto.CarteiraCobranca.Codigo.Equals("16") &
                !boleto.CarteiraCobranca.Codigo.Equals("17") &
                !boleto.CarteiraCobranca.Codigo.Equals("17-019") &
                !boleto.CarteiraCobranca.Codigo.Equals("18") &
                !boleto.CarteiraCobranca.Codigo.Equals("18-019") &
                !boleto.CarteiraCobranca.Codigo.Equals("18-027") &
                !boleto.CarteiraCobranca.Codigo.Equals("18-035") &
                !boleto.CarteiraCobranca.Codigo.Equals("18-140") &
                !boleto.CarteiraCobranca.Codigo.Equals("31"))

                throw new NotImplementedException("Carteira não informada. Utilize a carteira 11, 16, 17, 17-019, 18, 18-019, 18-027, 18-035, 18-140 ou 31.");

            //Verifica se o nosso número é válido
            if (boleto.NossoNumero.ToString() == string.Empty)
                throw new NotImplementedException("Nosso número inválido");


            #region Carteira 11
            //Carteira 18 com nosso número de 11 posições
            if (boleto.CarteiraCobranca.Codigo.Equals("11"))
            {
                if (!boleto.TipoModalidade.Equals("21"))
                {
                    if (boleto.NossoNumero.Length > 11)
                        throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 11 de posições para o nosso número", boleto.CarteiraCobranca.Codigo));

                    if (boleto.CedenteBoleto.Convenio.Length == 6)
                        boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(11, '0'));
                    else
                        boleto.NossoNumero = boleto.NossoNumero.PadLeft(11, '0');
                }
                else
                {
                    if (boleto.CedenteBoleto.Convenio.ToString().Length != 6)
                        throw new ValidacaoBoletoException(string.Format("Para a carteira {0} e o tipo da modalidade 21, o número do convênio são de 6 posições", boleto.CarteiraCobranca.Codigo));

                    boleto.NossoNumero = boleto.NossoNumero.PadLeft(17, '0');
                }
            }
            #endregion Carteira 11

            #region Carteira 16
            //Carteira 18 com nosso número de 11 posições
            if (boleto.CarteiraCobranca.Codigo.Equals("16"))
            {
                if (!boleto.TipoModalidade.Equals("21"))
                {
                    if (boleto.NossoNumero.Length > 11)
                        throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 11 de posições para o nosso número", boleto.CarteiraCobranca.Codigo));

                    if (boleto.CedenteBoleto.Convenio.ToString().Length == 6)
                        boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(11, '0'));
                    else
                        boleto.NossoNumero = boleto.NossoNumero.PadLeft(11, '0');
                }
                else
                {
                    if (boleto.CedenteBoleto.Convenio.ToString().Length != 6)
                        throw new ValidacaoBoletoException(string.Format("Para a carteira {0} e o tipo da modalidade 21, o número do convênio são de 6 posições", boleto.CarteiraCobranca.Codigo));

                    boleto.NossoNumero = boleto.NossoNumero.PadLeft(17, '0');
                }
            }
            #endregion Carteira 16

            #region Carteira 17
            //Carteira 17
            if (boleto.CarteiraCobranca.Codigo.Equals("17"))
            {
                switch (boleto.CedenteBoleto.Convenio.ToString().Length)
                {
                    //O BB manda como padrão 7 posições, mas é possível solicitar um convênio com 6 posições na carteira 17
                    case 6:
                        if (boleto.NossoNumero.Length > 12)
                            throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 12 de posições para o nosso número", boleto.CarteiraCobranca.Codigo));
                        boleto.NossoNumero = boleto.NossoNumero.PadLeft(12, '0');
                        break;
                    case 7:
                        if (boleto.NossoNumero.Length > 17)
                            throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 10 de posições para o nosso número", boleto.CarteiraCobranca.Codigo));
                        boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(10, '0'));
                        break;
                    default:
                        throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, o número do convênio deve ter 6 ou 7 posições", boleto.CarteiraCobranca.Codigo));
                }
            }
            #endregion Carteira 17

            #region Carteira 17-019
            //Carteira 17, com variação 019
            if (boleto.CarteiraCobranca.Codigo.Equals("17-019"))
            {
                /*
                 * Convênio de 7 posições
                 * Nosso Número com 17 posições
                 */
                if (boleto.CedenteBoleto.Convenio.ToString().Length == 7)
                {
                    if (boleto.NossoNumero.Length > 10)
                        throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 10 de posições para o nosso número", boleto.CarteiraCobranca.Codigo));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(10, '0'));
                }
                /*
                 * Convênio de 6 posições
                 * Nosso Número com 11 posições
                 */
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 6)
                {
                    //Nosso Número com 17 posições
                    if ((boleto.CedenteBoleto.CodigoCedente.ToString().Length + boleto.NossoNumero.Length) > 11)
                        throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 11 de posições para o nosso número. Onde o nosso número é formado por CCCCCCNNNNN-X: C -> número do convênio fornecido pelo Banco, N -> seqüencial atribuído pelo cliente e X -> dígito verificador do “Nosso-Número”.", boleto.CarteiraCobranca.Codigo));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(5, '0'));
                }
                /*
                  * Convênio de 4 posições
                  * Nosso Número com 11 posições
                  */
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 4)
                {
                    if (boleto.NossoNumero.Length > 7)
                        throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 7 de posições para o nosso número [{1}]", boleto.CarteiraCobranca.Codigo, boleto.NossoNumero));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(7, '0'));
                }
                else
                    boleto.NossoNumero = boleto.NossoNumero.PadLeft(11, '0');
            }
            #endregion Carteira 17-019

            #region Carteira 18
            //Carteira 18 com nosso número de 11 posições
            if (boleto.CarteiraCobranca.Codigo.Equals("18"))
                boleto.NossoNumero = boleto.NossoNumero.PadLeft(11, '0');
            #endregion Carteira 18

            #region Carteira 18-019
            //Carteira 18, com variação 019
            if (boleto.CarteiraCobranca.Codigo.Equals("18-019"))
            {
                /*
                 * Convênio de 7 posições
                 * Nosso Número com 17 posições
                 */
                if (boleto.CedenteBoleto.Convenio.ToString().Length == 7)
                {
                    if (boleto.NossoNumero.Length > 10)
                        throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 10 de posições para o nosso número", boleto.CarteiraCobranca.Codigo));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(10, '0'));
                }
                /*
                 * Convênio de 6 posições
                 * Nosso Número com 11 posições
                 */
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 6)
                {
                    //Modalidades de Cobrança Sem Registro – Carteira 16 e 18
                    //Nosso Número com 17 posições
                    if (!boleto.TipoModalidade.Equals("21"))
                    {
                        if ((boleto.CedenteBoleto.CodigoCedente.ToString().Length + boleto.NossoNumero.Length) > 11)
                            throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 11 de posições para o nosso número. Onde o nosso número é formado por CCCCCCNNNNN-X: C -> número do convênio fornecido pelo Banco, N -> seqüencial atribuído pelo cliente e X -> dígito verificador do “Nosso-Número”.", boleto.CarteiraCobranca.Codigo));

                        boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(5, '0'));
                    }
                    else
                    {
                        if (boleto.CedenteBoleto.Convenio.ToString().Length != 6)
                            throw new ValidacaoBoletoException(string.Format("Para a carteira {0} e o tipo da modalidade 21, o número do convênio são de 6 posições", boleto.CarteiraCobranca.Codigo));

                        boleto.NossoNumero = boleto.NossoNumero.PadLeft(17, '0');
                    }
                }
                /*
                  * Convênio de 4 posições
                  * Nosso Número com 11 posições
                  */
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 4)
                {
                    if (boleto.NossoNumero.Length > 7)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade máxima são de 7 de posições para o nosso número [{1}]", boleto.CarteiraCobranca.Codigo, boleto.NossoNumero));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(7, '0'));
                }
                else
                    boleto.NossoNumero = boleto.NossoNumero.PadLeft(11, '0');
            }
            #endregion Carteira 18-019


            //Para atender o cliente Fiemg foi adaptado no código na variação 18-027 as variações 18-035 e 18-140
            #region Carteira 18-027
            //Carteira 18, com variação 019
            if (boleto.CarteiraCobranca.Codigo.Equals("18-027"))
            {
                /*
                 * Convênio de 7 posições
                 * Nosso Número com 17 posições
                 */
                if (boleto.CedenteBoleto.Convenio.ToString().Length == 7)
                {
                    if (boleto.NossoNumero.Length > 10)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade máxima são de 10 de posições para o nosso número", boleto.CarteiraCobranca.Codigo));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(10, '0'));
                }
                /*
                 * Convênio de 6 posições
                 * Nosso Número com 11 posições
                 */
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 6)
                {
                    //Modalidades de Cobrança Sem Registro – Carteira 16 e 18
                    //Nosso Número com 17 posições
                    if (!boleto.TipoModalidade.Equals("21"))
                    {
                        if ((boleto.CedenteBoleto.CodigoCedente.ToString().Length + boleto.NossoNumero.Length) > 11)
                            throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade máxima são de 11 de posições para o nosso número. Onde o nosso número é formado por CCCCCCNNNNN-X: C -> número do convênio fornecido pelo Banco, N -> seqüencial atribuído pelo cliente e X -> dígito verificador do “Nosso-Número”.", boleto.CarteiraCobranca.Codigo));

                        boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(5, '0'));
                    }
                    else
                    {
                        if (boleto.CedenteBoleto.Convenio.ToString().Length != 6)
                            throw new NotImplementedException(string.Format("Para a carteira {0} e o tipo da modalidade 21, o número do convênio são de 6 posições", boleto.CarteiraCobranca.Codigo));

                        boleto.NossoNumero = boleto.NossoNumero.PadLeft(17, '0');
                    }
                }
                /*
                  * Convênio de 4 posições
                  * Nosso Número com 11 posições
                  */
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 4)
                {
                    if (boleto.NossoNumero.Length > 7)
                        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade máxima são de 7 de posições para o nosso número [{1}]", boleto.CarteiraCobranca.Codigo, boleto.NossoNumero));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(7, '0'));
                }
                else
                    boleto.NossoNumero = boleto.NossoNumero.PadLeft(11, '0');
            }
            #endregion Carteira 18-027

            #region Carteira 18-035
            //Carteira 18, com variação 019
            if (boleto.CarteiraCobranca.Codigo.Equals("18-035"))
            {
                /*
                 * Convênio de 7 posições
                 * Nosso Número com 17 posições
                 */
                if (boleto.CedenteBoleto.Convenio.ToString().Length == 7)
                {
                    if (boleto.NossoNumero.Length > 10)
                        throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 10 de posições para o nosso número", boleto.CarteiraCobranca.Codigo));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(10, '0'));
                }
                /*
                 * Convênio de 6 posições
                 * Nosso Número com 11 posições
                 */
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 6)
                {
                    //Modalidades de Cobrança Sem Registro – Carteira 16 e 18
                    //Nosso Número com 17 posições
                    if (!boleto.TipoModalidade.Equals("21"))
                    {
                        if ((boleto.CedenteBoleto.CodigoCedente.ToString().Length + boleto.NossoNumero.Length) > 11)
                            throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade máxima são de 11 de posições para o nosso número. Onde o nosso número é formado por CCCCCCNNNNN-X: C -> número do convênio fornecido pelo Banco, N -> seqüencial atribuído pelo cliente e X -> dígito verificador do “Nosso-Número”.", boleto.CarteiraCobranca.Codigo));

                        boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(5, '0'));
                    }
                    else
                    {
                        if (boleto.CedenteBoleto.Convenio.ToString().Length != 6)
                            throw new NotImplementedException(string.Format("Para a carteira {0} e o tipo da modalidade 21, o número do convênio são de 6 posições", boleto.CarteiraCobranca.Codigo));

                        boleto.NossoNumero = boleto.NossoNumero.PadLeft(17, '0');
                    }
                }
                /*
                  * Convênio de 4 posições
                  * Nosso Número com 11 posições
                  */
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 4)
                {
                    if (boleto.NossoNumero.Length > 7)
                        throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 7 de posições para o nosso número [{1}]", boleto.CarteiraCobranca.Codigo, boleto.NossoNumero));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(7, '0'));
                }
                else
                    boleto.NossoNumero = boleto.NossoNumero.PadLeft(11, '0');
            }
            #endregion Carteira 18-035

            #region Carteira 18-140
            //Carteira 18, com variação 019
            if (boleto.CarteiraCobranca.Codigo.Equals("18-140"))
            {
                /*
                 * Convênio de 7 posições
                 * Nosso Número com 17 posições
                 */
                if (boleto.CedenteBoleto.Convenio.ToString().Length == 7)
                {
                    if (boleto.NossoNumero.Length > 10)
                        throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 10 de posições para o nosso número", boleto.CarteiraCobranca.Codigo));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(10, '0'));
                }
                /*
                 * Convênio de 6 posições
                 * Nosso Número com 11 posições
                 */
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 6)
                {
                    //Modalidades de Cobrança Sem Registro – Carteira 16 e 18
                    //Nosso Número com 17 posições
                    if (!boleto.TipoModalidade.Equals("21"))
                    {
                        if ((boleto.CedenteBoleto.CodigoCedente.ToString().Length + boleto.NossoNumero.Length) > 11)
                            throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 11 de posições para o nosso número. Onde o nosso número é formado por CCCCCCNNNNN-X: C -> número do convênio fornecido pelo Banco, N -> seqüencial atribuído pelo cliente e X -> dígito verificador do “Nosso-Número”.", boleto.CarteiraCobranca.Codigo));

                        boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(5, '0'));
                    }
                    else
                    {
                        if (boleto.CedenteBoleto.Convenio.ToString().Length != 6)
                            throw new ValidacaoBoletoException(string.Format("Para a carteira {0} e o tipo da modalidade 21, o número do convênio são de 6 posições", boleto.CarteiraCobranca.Codigo));

                        boleto.NossoNumero = boleto.NossoNumero.PadLeft(17, '0');
                    }
                }
                /*
                  * Convênio de 4 posições
                  * Nosso Número com 11 posições
                  */
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 4)
                {
                    if (boleto.NossoNumero.Length > 7)
                        throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 7 de posições para o nosso número [{1}]", boleto.CarteiraCobranca.Codigo, boleto.NossoNumero));

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(7, '0'));
                }
                else
                    boleto.NossoNumero = boleto.NossoNumero.PadLeft(11, '0');
            }
            #endregion Carteira 18-140

            #region Carteira 31
            //Carteira 31
            if (boleto.CarteiraCobranca.Codigo.Equals("31"))
            {
                switch (boleto.CedenteBoleto.Convenio.ToString().Length)
                {
                    //O BB manda como padrão 7 posições, mas é possível solicitar um convênio com 6 posições na carteira 31
                    case 5:
                        if (boleto.NossoNumero.Length > 10)
                            throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 12 de posições para o nosso número", boleto.CarteiraCobranca));
                        boleto.NossoNumero = boleto.NossoNumero.PadLeft(10, '0');
                        break;
                    case 6:
                        if (boleto.NossoNumero.Length > 10)
                            throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 12 de posições para o nosso número", boleto.CarteiraCobranca));
                        boleto.NossoNumero = boleto.NossoNumero.PadLeft(10, '0');
                        break;
                    case 7:
                        if (boleto.NossoNumero.Length > 17)
                            throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, a quantidade máxima são de 10 de posições para o nosso número", boleto.CarteiraCobranca));
                        boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio, boleto.NossoNumero.PadLeft(10, '0'));
                        break;
                    default:
                        throw new ValidacaoBoletoException(string.Format("Para a carteira {0}, o número do convênio deve ter 6 ou 7 posições", boleto.CarteiraCobranca));
                }
            }
            #endregion Carteira 31


            #region Agência e Conta Corrente
            //Verificar se a Agencia esta correta
            if (boleto.CedenteBoleto.ContaBancariaCedente.Agencia.Length > 4)
                throw new NotImplementedException("A quantidade de dígitos da Agência " + boleto.CedenteBoleto.ContaBancariaCedente.Agencia + ", são de 4 números.");
            else if (boleto.CedenteBoleto.ContaBancariaCedente.Agencia.Length < 4)
                boleto.CedenteBoleto.ContaBancariaCedente.Agencia = boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0');

            //Verificar se a Conta esta correta
            if (boleto.CedenteBoleto.ContaBancariaCedente.Conta.Length > 8)
                throw new NotImplementedException("A quantidade de dígitos da Conta " + boleto.CedenteBoleto.ContaBancariaCedente.Conta + ", são de 8 números.");
            else if (boleto.CedenteBoleto.ContaBancariaCedente.Conta.Length < 8)
                boleto.CedenteBoleto.ContaBancariaCedente.Conta = boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(8, '0');
            #endregion Agência e Conta Corrente
        }
        public void FormatarBoleto(Boleto boleto)
        {
            boleto.LocalPagamento = "PAGÁVEL EM QUALQUER AGÊNCIA BANCÁRIA ATÉ O VENCIMENTO";

            if (boleto.DataProcessamento == DateTime.MinValue)
                boleto.DataProcessamento = DateTime.Now;

            if (boleto.DataDocumento == DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;

            this.FormataCodigoBarra(boleto);
            this.FormataLinhaDigitavel(boleto);
            this.FormataNossoNumero(boleto);
        }
        private string LimparCarteira(string carteira)
        {
            return carteira.Split('-')[0];
        }
        internal static string Mod11BancoBrasil(string value)
        {
            #region Trecho do manual DVMD11.doc
            /* 
            Multiplicar cada algarismo que compõe o número pelo seu respectivo multiplicador (PESO).
            Os multiplicadores(PESOS) variam de 9 a 2.
            O primeiro dígito da direita para a esquerda deverá ser multiplicado por 9, o segundo por 8 e assim sucessivamente.
            O resultados das multiplicações devem ser somados:
            72+35+24+27+4+9+8=179
            O total da soma deverá ser dividido por 11:
            179 / 11=16
            RESTO=3

            Se o resto da divisão for igual a 10 o D.V. será igual a X. 
            Se o resto da divisão for igual a 0 o D.V. será igual a 0.
            Se o resto for menor que 10, o D.V.  será igual ao resto.

            No exemplo acima, o dígito verificador será igual a 3
            */
            #endregion

            /* d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            string d;
            int s = 0, p = 9, b = 2;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                s += (int.Parse(value[i].ToString()) * p);
                if (p == b)
                    p = 9;
                else
                    p--;
            }

            int r = (s % 11);
            if (r == 10)
                d = "X";
            else if (r == 0)
                d = "0";
            else
                d = r.ToString();

            return d;
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = valorBoleto.PadLeft(10, '0');

            //Criada por AFK
            #region Carteira 11
            if (boleto.CarteiraCobranca.Codigo.Equals("11"))
            {
                if (boleto.CedenteBoleto.Convenio.ToString().Length == 6)
                {
                    if (boleto.TipoModalidade.Equals("21"))
                        boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                            this.CodigoBanco.PadLeft(3, '0'),
                            boleto.Moeda,
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoleto,
                            boleto.CedenteBoleto.Convenio,
                            boleto.NossoNumero,
                            "21");
                }
                else
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        this.CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                        boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                        boleto.CarteiraCobranca.Codigo);
                }
            }
            #endregion Carteira 11

            #region Carteira 16
            if (boleto.CarteiraCobranca.Codigo.Equals("16"))
            {
                if (boleto.CedenteBoleto.Convenio.ToString().Length == 6)
                {
                    if (boleto.TipoModalidade.Equals("21"))
                        boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                            this.CodigoBanco.PadLeft(3, '0'),
                            boleto.Moeda,
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoleto,
                            boleto.CedenteBoleto.Convenio,
                            boleto.NossoNumero,
                            "21");
                }
                else
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        this.CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                        boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                        boleto.CarteiraCobranca.Codigo);
                }
            }
            #endregion Carteira 16

            #region Carteira 17
            if (boleto.CarteiraCobranca.Codigo.Equals("17"))
            {
                if (boleto.CedenteBoleto.Convenio.ToString().Length == 7)
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                        this.CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento),
                        valorBoleto,
                        "000000",
                        boleto.NossoNumero,
                        LimparCarteira(boleto.CarteiraCobranca.Codigo).PadLeft(2, '0'));
                }
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 6)
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        this.CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento),
                        valorBoleto,
                        Common.Mid(boleto.NossoNumero, 1, 11),
                        boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                        boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                        boleto.CarteiraCobranca.Codigo);
                }
                else
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        this.CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                        boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                        boleto.CarteiraCobranca.Codigo);
                }
            }
            #endregion Carteira 17

            #region Carteira 17-019
            if (boleto.CarteiraCobranca.Codigo.Equals("17-019"))
            {
                if (boleto.CedenteBoleto.Convenio.ToString().Length == 7)
                {
                    #region Especificação Convênio 7 posições
                    /*
                    Posição     Tamanho     Picture     Conteúdo
                    01 a 03         03      9(3)            Código do Banco na Câmara de Compensação = ‘001’
                    04 a 04         01      9(1)            Código da Moeda = '9'
                    05 a 05         01      9(1)            DV do Código de Barras (Anexo 10)
                    06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
                    10 a 19         10      9(08)           V(2) Valor
                    20 a 25         06      9(6)            Zeros
                    26 a 42         17      9(17)           Nosso-Número, sem o DV
                    26 a 32         9       (7)             Número do Convênio fornecido pelo Banco (CCCCCCC)
                    33 a 42         9       (10)            Complemento do Nosso-Número, sem DV (NNNNNNNNNN)
                    43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobrança
                     */
                    #endregion Especificação Convênio 7 posições

                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                        this.CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento),
                        valorBoleto,
                        "000000",
                        boleto.NossoNumero,
                        LimparCarteira(boleto.CarteiraCobranca.Codigo).PadLeft(2, '0'));
                }
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 6)
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                            this.CodigoBanco.PadLeft(3, '0'),
                            boleto.Moeda,
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoleto,
                            boleto.NossoNumero,
                            boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                            boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                            LimparCarteira(boleto.CarteiraCobranca.Codigo));
                }
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 4)
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        this.CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                        boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                        LimparCarteira(boleto.CarteiraCobranca.Codigo));
                }
            }
            #endregion Carteira 17-019

            #region Carteira 18
            if (boleto.CarteiraCobranca.Codigo.Equals("18"))
            {
                boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                    this.CodigoBanco.PadLeft(3, '0'),
                    boleto.Moeda,
                    Common.FatorVencimento(boleto.DataVencimento),
                    valorBoleto,
                    boleto.NossoNumero,
                    boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                    boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                    boleto.CarteiraCobranca.Codigo);
            }
            #endregion Carteira 18

            #region Carteira 18-019
            if (boleto.CarteiraCobranca.Codigo.Equals("18-019"))
            {
                if (boleto.CedenteBoleto.Convenio.ToString().Length == 7)
                {
                    #region Especificação Convênio 7 posições
                    /*
                    Posição     Tamanho     Picture     Conteúdo
                    01 a 03         03      9(3)            Código do Banco na Câmara de Compensação = ‘001’
                    04 a 04         01      9(1)            Código da Moeda = '9'
                    05 a 05         01      9(1)            DV do Código de Barras (Anexo 10)
                    06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
                    10 a 19         10      9(08)           V(2) Valor
                    20 a 25         06      9(6)            Zeros
                    26 a 42         17      9(17)           Nosso-Número, sem o DV
                    26 a 32         9       (7)             Número do Convênio fornecido pelo Banco (CCCCCCC)
                    33 a 42         9       (10)            Complemento do Nosso-Número, sem DV (NNNNNNNNNN)
                    43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobrança
                     */
                    #endregion Especificação Convênio 7 posições

                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                        CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento),
                        valorBoleto,
                        "000000",
                        boleto.NossoNumero,
                        LimparCarteira(boleto.CarteiraCobranca.Codigo).PadLeft(2, '0'));
                }
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 6)
                {
                    if (boleto.TipoModalidade.Equals("21"))
                        boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                            this.CodigoBanco.PadLeft(3, '0'),
                            boleto.Moeda,
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoleto,
                            boleto.CedenteBoleto.Convenio,
                            boleto.NossoNumero,
                            "21");
                    else
                        boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                            this.CodigoBanco.PadLeft(3, '0'),
                            boleto.Moeda,
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoleto,
                            boleto.NossoNumero,
                            boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                            boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                            LimparCarteira(boleto.CarteiraCobranca.Codigo));
                }
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 4)
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        this.CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                        boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                        LimparCarteira(boleto.CarteiraCobranca.Codigo));
                }
            }
            #endregion Carteira 18-019

            //Para atender o cliente Fiemg foi adptado no código na variação 18-027 as variações 18-035 e 18-140
            #region Carteira 18-027
            if (boleto.CarteiraCobranca.Codigo.Equals("18-027"))
            {
                if (boleto.CedenteBoleto.Convenio.ToString().Length == 7)
                {
                    #region Especificação Convênio 7 posições
                    /*
                    Posição     Tamanho     Picture     Conteúdo
                    01 a 03         03      9(3)            Código do Banco na Câmara de Compensação = ‘001’
                    04 a 04         01      9(1)            Código da Moeda = '9'
                    05 a 05         01      9(1)            DV do Código de Barras (Anexo 10)
                    06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
                    10 a 19         10      9(08)           V(2) Valor
                    20 a 25         06      9(6)            Zeros
                    26 a 42         17      9(17)           Nosso-Número, sem o DV
                    26 a 32         9       (7)             Número do Convênio fornecido pelo Banco (CCCCCCC)
                    33 a 42         9       (10)            Complemento do Nosso-Número, sem DV (NNNNNNNNNN)
                    43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobrança
                     */
                    #endregion Especificação Convênio 7 posições

                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                        this.CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento).ToString("0000"),
                        valorBoleto,
                        "000000",
                        boleto.NossoNumero,
                        LimparCarteira(boleto.CarteiraCobranca.Codigo).PadLeft(2, '0'));
                }
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 6)
                {
                    if (boleto.TipoModalidade.Equals("21"))
                        boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                            this.CodigoBanco.PadLeft(3, '0'),
                            boleto.Moeda,
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoleto,
                            boleto.CedenteBoleto.Convenio,
                            boleto.NossoNumero,
                            "21");
                    else
                        boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                            this.CodigoBanco.PadLeft(3, '0'),
                            boleto.Moeda,
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoleto,
                            boleto.NossoNumero,
                            boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                            boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                            LimparCarteira(boleto.CarteiraCobranca.Codigo));
                }
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 4)
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        this.CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                        boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                        LimparCarteira(boleto.CarteiraCobranca.Codigo));
                }
            }
            #endregion Carteira 18-027

            #region Carteira 18-035
            if (boleto.CarteiraCobranca.Codigo.Equals("18-035"))
            {
                if (boleto.CedenteBoleto.Convenio.ToString().Length == 7)
                {
                    #region Especificação Convênio 7 posições
                    /*
                    Posição     Tamanho     Picture     Conteúdo
                    01 a 03         03      9(3)            Código do Banco na Câmara de Compensação = ‘001’
                    04 a 04         01      9(1)            Código da Moeda = '9'
                    05 a 05         01      9(1)            DV do Código de Barras (Anexo 10)
                    06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
                    10 a 19         10      9(08)           V(2) Valor
                    20 a 25         06      9(6)            Zeros
                    26 a 42         17      9(17)           Nosso-Número, sem o DV
                    26 a 32         9       (7)             Número do Convênio fornecido pelo Banco (CCCCCCC)
                    33 a 42         9       (10)            Complemento do Nosso-Número, sem DV (NNNNNNNNNN)
                    43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobrança
                     */
                    #endregion Especificação Convênio 7 posições

                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                        this.CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento).ToString("0000"),
                        valorBoleto,
                        "000000",
                        boleto.NossoNumero,
                        LimparCarteira(boleto.CarteiraCobranca.Codigo).PadLeft(2, '0'));
                }
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 6)
                {
                    if (boleto.TipoModalidade.Equals("21"))
                        boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                            this.CodigoBanco.PadLeft(3, '0'),
                            boleto.Moeda,
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoleto,
                            boleto.CedenteBoleto.Convenio,
                            boleto.NossoNumero,
                            "21");
                    else
                        boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                            this.CodigoBanco.PadLeft(3, '0'),
                            boleto.Moeda,
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoleto,
                            boleto.NossoNumero,
                            boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                            boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                            LimparCarteira(boleto.CarteiraCobranca.Codigo));
                }
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 4)
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        this.CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                        boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                        LimparCarteira(boleto.CarteiraCobranca.Codigo));
                }
            }
            #endregion Carteira 18-035

            #region Carteira 18-140
            if (boleto.CarteiraCobranca.Codigo.Equals("18-140"))
            {
                if (boleto.CedenteBoleto.Convenio.ToString().Length == 7)
                {
                    #region Especificação Convênio 7 posições
                    /*
                    Posição     Tamanho     Picture     Conteúdo
                    01 a 03         03      9(3)            Código do Banco na Câmara de Compensação = ‘001’
                    04 a 04         01      9(1)            Código da Moeda = '9'
                    05 a 05         01      9(1)            DV do Código de Barras (Anexo 10)
                    06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
                    10 a 19         10      9(08)           V(2) Valor
                    20 a 25         06      9(6)            Zeros
                    26 a 42         17      9(17)           Nosso-Número, sem o DV
                    26 a 32         9       (7)             Número do Convênio fornecido pelo Banco (CCCCCCC)
                    33 a 42         9       (10)            Complemento do Nosso-Número, sem DV (NNNNNNNNNN)
                    43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobrança
                     */
                    #endregion Especificação Convênio 7 posições

                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                        this.CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento).ToString("0000"),
                        valorBoleto,
                        "000000",
                        boleto.NossoNumero,
                        LimparCarteira(boleto.CarteiraCobranca.Codigo).PadLeft(2, '0'));
                }
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 6)
                {
                    if (boleto.TipoModalidade.Equals("21"))
                        boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                            this.CodigoBanco.PadLeft(3, '0'),
                            boleto.Moeda,
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoleto,
                            boleto.CedenteBoleto.Convenio,
                            boleto.NossoNumero,
                            "21");
                    else
                        boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                            this.CodigoBanco.PadLeft(3, '0'),
                            boleto.Moeda,
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoleto,
                            boleto.NossoNumero,
                            boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                            boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                            LimparCarteira(boleto.CarteiraCobranca.Codigo));
                }
                else if (boleto.CedenteBoleto.Convenio.ToString().Length == 4)
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        this.CodigoBanco.PadLeft(3, '0'),
                        boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento),
                        valorBoleto,
                        boleto.NossoNumero,
                        boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                        boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                        LimparCarteira(boleto.CarteiraCobranca.Codigo));
                }
            }
            #endregion Carteira 18-140

            #region Carteira 31
            if (boleto.CarteiraCobranca.Codigo.Equals("31"))
            {
                boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                    this.CodigoBanco.PadLeft(3, '0'),
                    boleto.Moeda,
                    Common.FatorVencimento(boleto.DataVencimento),
                    valorBoleto,
                    boleto.NossoNumero,
                    boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                    boleto.CedenteBoleto.ContaBancariaCedente.Conta,
                    boleto.CarteiraCobranca.Codigo);
            }
            #endregion Carteira 31

            _digitoAutoConferenciaBoleto = Common.Mod11(boleto.CodigoBarraBoleto, 9);

            boleto.CodigoBarraBoleto = Common.Left(boleto.CodigoBarraBoleto, 4) + _digitoAutoConferenciaBoleto + Common.Right(boleto.CodigoBarraBoleto, 39);
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {
            string cmplivre = string.Empty;
            string campo1 = string.Empty;
            string campo2 = string.Empty;
            string campo3 = string.Empty;
            string campo4 = string.Empty;
            string campo5 = string.Empty;
            long icampo5 = 0;
            int digitoMod = 0;

            /*
            Campos 1 (AAABC.CCCCX):
            A = Código do Banco na Câmara de Compensação “001”
            B = Código da moeda "9" (*)
            C = Posição 20 a 24 do código de barras
            X = DV que amarra o campo 1 (Módulo 10, contido no Anexo 7)
             */

            cmplivre = Common.Mid(boleto.CodigoBarraBoleto, 20, 25);

            campo1 = Common.Left(boleto.CodigoBarraBoleto, 4) + Common.Mid(cmplivre, 1, 5);
            digitoMod = Common.Mod10(campo1);
            campo1 = campo1 + digitoMod.ToString();
            campo1 = Common.Mid(campo1, 1, 5) + "." + Common.Mid(campo1, 6, 5);
            /*
            Campo 2 (DDDDD.DDDDDY)
            D = Posição 25 a 34 do código de barras
            Y = DV que amarra o campo 2 (Módulo 10, contido no Anexo 7)
             */
            campo2 = Common.Mid(cmplivre, 6, 10);
            digitoMod = Common.Mod10(campo2);
            campo2 = campo2 + digitoMod.ToString();
            campo2 = Common.Mid(campo2, 1, 5) + "." + Common.Mid(campo2, 6, 6);


            /*
            Campo 3 (EEEEE.EEEEEZ)
            E = Posição 35 a 44 do código de barras
            Z = DV que amarra o campo 3 (Módulo 10, contido no Anexo 7)
             */
            campo3 = Common.Mid(cmplivre, 16, 10);
            digitoMod = Common.Mod10(campo3);
            campo3 = campo3 + digitoMod;
            campo3 = Common.Mid(campo3, 1, 5) + "." + Common.Mid(campo3, 6, 6);

            /*
            Campo 4 (K)
            K = DV do Código de Barras (Módulo 11, contido no Anexo 10)
             */
            campo4 = Common.Mid(boleto.CodigoBarraBoleto, 5, 1);

            /*
            Campo 5 (UUUUVVVVVVVVVV)
            U = Fator de Vencimento ( Anexo 10)
            V = Valor do Título (*)
             */
            icampo5 = Convert.ToInt64(Common.Mid(boleto.CodigoBarraBoleto, 6, 14));

            if (icampo5 == 0)
                campo5 = "000";
            else
                campo5 = icampo5.ToString();

            boleto.LinhaDigitavelBoleto = campo1 + " " + campo2 + " " + campo3 + " " + campo4 + " " + campo5;
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            if (boleto.CedenteBoleto.Convenio.ToString().Length == 6) //somente monta o digito verificador no nosso numero se o convenio tiver 6 posições
            {
                switch (boleto.CarteiraCobranca.Codigo)
                {
                    case "18-019":
                        boleto.NossoNumero = string.Format("{0}/{1}-{2}", LimparCarteira(boleto.CarteiraCobranca.Codigo), boleto.NossoNumero, Mod11BancoBrasil(boleto.NossoNumero));
                        return;
                }
            }

            switch (boleto.CarteiraCobranca.Codigo)
            {
                case "17-019":
                case "18-019":
                    boleto.NossoNumero = string.Format("{0}/{1}", LimparCarteira(boleto.CarteiraCobranca.Codigo), boleto.NossoNumero);
                    return;
                case "31":
                    boleto.NossoNumero = string.Format("{0}{1}", boleto.CedenteBoleto.Convenio.PadLeft(7, '0'), boleto.NossoNumero);
                    return;
            }


            boleto.NossoNumero = string.Format("{0}", boleto.NossoNumero);
        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException();
        }
    }
}
