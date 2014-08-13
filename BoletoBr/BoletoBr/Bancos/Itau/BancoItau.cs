using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Dominio;

namespace BoletoBr.Bancos.Itau
{
    public class BancoItau : IBanco
    {

        private int _dacBoleto = 0;
        private int _dacNossoNumero = 0;

        public BancoItau()
        {
            this.CodigoBanco = "341";
            this.DigitoBanco = "7";
            this.NomeBanco = "Itaú";
            this.LocalDePagamento = "Pagável em qualquer banco até o vencimento.";
            this.MoedaBanco = "0";

            /* Adiciona carteiras de cobrança */
            _carteirasCobranca = new List<CarteiraCobranca>();
            _carteirasCobranca.Add(new CarteiraCobranca107());
            _carteirasCobranca.Add(new CarteiraCobranca109());
            _carteirasCobranca.Add(new CarteiraCobranca121());
            _carteirasCobranca.Add(new CarteiraCobranca122());
            _carteirasCobranca.Add(new CarteiraCobranca126());
            _carteirasCobranca.Add(new CarteiraCobranca131());
            _carteirasCobranca.Add(new CarteiraCobranca142());
            _carteirasCobranca.Add(new CarteiraCobranca143());
            _carteirasCobranca.Add(new CarteiraCobranca146());
            _carteirasCobranca.Add(new CarteiraCobranca150());
            _carteirasCobranca.Add(new CarteiraCobranca169());
            _carteirasCobranca.Add(new CarteiraCobranca175());
            _carteirasCobranca.Add(new CarteiraCobranca176());
            _carteirasCobranca.Add(new CarteiraCobranca178());
            _carteirasCobranca.Add(new CarteiraCobranca196());
            _carteirasCobranca.Add(new CarteiraCobranca198());
        }

        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        public Image LogotipoBancoParaExibicao { get; set; }

        private readonly List<CarteiraCobranca> _carteirasCobranca;

        public string LocalDePagamento { get; private set; }
        public string MoedaBanco { get; private set; }

        public List<CarteiraCobranca> GetCarteirasCobranca()
        {
            return _carteirasCobranca;
        }

        public CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira)
        {
            return GetCarteirasCobranca().Find(fd => fd.Codigo == codigoCarteira);
        }

        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            try
            {
                //Carteiras v�lidas
                string[] cv = new string[] { "107", "109", "121", "122", "126", "131", "142", "143", "146", "150", "169", "175", "176", "178", "196", "198" };
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
                    throw new NotImplementedException("Carteira n�o implementada: " + boleto.CarteiraCobranca.Codigo +
                                                      carteirasImplementadas);
                }
                //Verifica se o tamanho para o NossoNumero s�o 8 d�gitos
                if (Convert.ToInt32(boleto.NossoNumeroFormatado).ToString().Length > 8)
                    throw new NotImplementedException("A quantidade de d�gitos do nosso n�mero para a carteira " +
                                                      boleto.CarteiraCobranca.Codigo + ", s�o 8 n�meros.");
                else if (Convert.ToInt32(boleto.NossoNumeroFormatado).ToString().Length < 8)
                    boleto.SetNossoNumeroFormatado(boleto.SequencialNossoNumero.PadLeft(8, '0'));

                //� obrigat�rio o preenchimento do n�mero do documento
                if (boleto.CarteiraCobranca.Codigo == "106" || boleto.CarteiraCobranca.Codigo == "107" || boleto.CarteiraCobranca.Codigo == "122" ||
                    boleto.CarteiraCobranca.Codigo == "142" || boleto.CarteiraCobranca.Codigo == "143" || boleto.CarteiraCobranca.Codigo == "195" ||
                    boleto.CarteiraCobranca.Codigo == "196" || boleto.CarteiraCobranca.Codigo == "198")
                {
                    if (Convert.ToInt32(boleto.NumeroDocumento) == 0)
                        throw new NotImplementedException("O n�mero do documento n�o pode ser igual a zero.");
                }

                //Formato o n�mero do documento 
                if (Convert.ToInt32(boleto.NumeroDocumento) > 0)
                    boleto.NumeroDocumento = boleto.NumeroDocumento.PadLeft(7, '0');

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

                // Usando Métodod e Geração do DAC do Nosso Número
                GerarDacNossoNumero(boleto);

                // Calcula o DAC da Conta Corrente
                boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta =
                    Common.Mod10(boleto.CedenteBoleto.ContaBancariaCedente.Agencia + boleto.CedenteBoleto.ContaBancariaCedente.Conta).ToString();

                //Verifica se o nosso n�mero � v�lido
                if (Convert.ToInt64(boleto.NossoNumeroFormatado) == 0)
                    throw new NotImplementedException("Nosso n�mero inv�lido");

                //Verifica se data do processamento � valida
                //if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
                if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                    boleto.DataProcessamento = DateTime.Now;

                //Verifica se data do documento � valida
                //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
                if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                    boleto.DataDocumento = DateTime.Now;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao validar boletos.", e);
            }
        }

        public void GerarDacNossoNumero(Boleto boleto)
        {
            // Calcula o DAC do Nosso N�mero a maioria das carteiras
            // agencia/conta/carteira/nosso numero
            if (boleto.CarteiraCobranca.Codigo != "126" && boleto.CarteiraCobranca.Codigo != "131"
                && boleto.CarteiraCobranca.Codigo != "145" && boleto.CarteiraCobranca.Codigo != "150"
                && boleto.CarteiraCobranca.Codigo != "168")
                _dacNossoNumero = Common.Mod10(boleto.CedenteBoleto.ContaBancariaCedente.Agencia + boleto.CedenteBoleto.ContaBancariaCedente.Conta +
                          boleto.CarteiraCobranca.Codigo + boleto.NossoNumeroFormatado);
            else
                // Excess�o 126 - 131 - 146 - 150 - 168
                // carteira/nosso numero
                _dacNossoNumero = Common.Mod10(boleto.CarteiraCobranca + boleto.NossoNumeroFormatado);
        }

        public void FormataMoeda(Boleto boleto)
        {
            boleto.Moeda = this.MoedaBanco;

            if (string.IsNullOrEmpty(boleto.Moeda))
                throw new Exception("Espécie/Moeda para o boleto não foi informada.");

            if ((boleto.Moeda == "0") || (boleto.Moeda == "REAL") || (boleto.Moeda == "R$"))
                boleto.Moeda = "R$";
            else
                boleto.Moeda = "1";
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
                
                /* Carteira 110 incluída somente para testes */
                if ((boleto.CarteiraCobranca.Codigo == "109") || (boleto.CarteiraCobranca.Codigo == "198") ||
                    (boleto.CarteiraCobranca.Codigo == "121") || (boleto.CarteiraCobranca.Codigo == "175") ||
                    (boleto.CarteiraCobranca.Codigo == "176") || (boleto.CarteiraCobranca.Codigo == "178"))
                    
                    boleto.CodigoBarraBoleto =
                        string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}000", CodigoBanco, MoedaBanco,
                                      Common.FatorVencimento(boleto.DataVencimento), valorBoleto, boleto.CarteiraCobranca.Codigo,
                                      boleto.SequencialNossoNumero, _dacNossoNumero, boleto.CedenteBoleto.ContaBancariaCedente.Agencia,
                                      boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(5, '0'), boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);
                         
                else if ((boleto.CarteiraCobranca.Codigo == "107") || (boleto.CarteiraCobranca.Codigo == "122") ||
                    (boleto.CarteiraCobranca.Codigo == "142") || (boleto.CarteiraCobranca.Codigo == "143") ||
                    (boleto.CarteiraCobranca.Codigo == "196"))
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}0", CodigoBanco, boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento), boleto.ValorBoleto, boleto.CarteiraCobranca.Codigo,
                        boleto.NossoNumeroFormatado, numeroDocumento, codigoCedente,
                        Common.Mod10(boleto.CarteiraCobranca.Codigo + boleto.NossoNumeroFormatado + numeroDocumento + codigoCedente));
                }

                _dacBoleto = Common.Mod11(boleto.CodigoBarraBoleto, 9, 0);

                boleto.CodigoBarraBoleto = Common.Left(boleto.CodigoBarraBoleto, 4) +
                    _dacBoleto + Common.Right(boleto.CodigoBarraBoleto, 39);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar c�digo de barras.", ex);
            }
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {
            try
            {
                string numeroDocumento = boleto.NumeroDocumento.PadLeft(7, '0');
                string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(5, '0');

                string AAA = CodigoBanco;
                string B = MoedaBanco;
                string CCC = boleto.CarteiraCobranca.Codigo;
                string DD = boleto.NossoNumeroFormatado.Replace("/", "").Replace("-", "").Substring(0, 2);
                string X = Common.Mod10(AAA + B + CCC + DD).ToString();
                string LD = string.Empty; /* Linha Digit�vel */

                string DDDDDD = boleto.NossoNumeroFormatado.Replace("/", "").Replace("-", "").Substring(2, 6);

                string K = string.Format(" {0} ", _dacBoleto);

                string UUUU = Common.FatorVencimento(boleto.DataVencimento).ToString();
                string VVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");

                string C1 = string.Empty;
                string C2 = string.Empty;
                string C3 = string.Empty;
                string C5 = string.Empty;

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

                    string EEEE = numeroDocumento.Substring(0, 4);
                    string Y = Common.Mod10(DDDDDD + EEEE).ToString();

                    C2 = string.Format("{0}.", DDDDDD.Substring(0, 5));
                    C2 += string.Format("{0}{1}{2} ", DDDDDD.Substring(5, 1), EEEE, Y);

                    #endregion DDDDD.DEEEEY

                    #region EEEFF.FFFGHZ

                    string EEE = numeroDocumento.Substring(4, 3);
                    string FFFFF = codigoCedente;
                    string G = Common.Mod10(boleto.CarteiraCobranca.Codigo + boleto.SequencialNossoNumero.PadLeft(8, '0') + numeroDocumento + codigoCedente).ToString();
                    string H = "0";
                    string Z = Common.Mod10(EEE + FFFFF + G + H).ToString();
                    C3 = string.Format("{0}{1}.{2}{3}{4}{5}", EEE, FFFFF.Substring(0, 2), FFFFF.Substring(2, 3), G, H, Z);

                    #endregion EEEFF.FFFGHZ
                }
                else if (boleto.CarteiraCobranca.Codigo == "126" || boleto.CarteiraCobranca.Codigo == "131" || boleto.CarteiraCobranca.Codigo == "146" ||
                         boleto.CarteiraCobranca.Codigo == "150" || boleto.CarteiraCobranca.Codigo == "168")
                {
                    throw new NotImplementedException("Fun��o n�o implementada.");
                }

                boleto.LinhaDigitavelBoleto = C1 + C2 + C3 + K + C5;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar linha digit�vel.", ex);
            }
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            boleto.SetNossoNumeroFormatado(boleto.SequencialNossoNumero.PadLeft(8, '0'));

            try
            {
                boleto.SetNossoNumeroFormatado(string.Format("{0}/{1}-{2}", boleto.CarteiraCobranca.Codigo, boleto.NossoNumeroFormatado, _dacNossoNumero));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar nosso n�mero", ex);
            }
        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            try
            {
                boleto.NumeroDocumento = string.Format("{0}-{1}", boleto.NumeroDocumento, Common.Mod10(boleto.NumeroDocumento));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar n�mero do documento.", ex);
            }
        }

        public RetornoGenerico LerArquivoRetorno(List<string> linhasArquivo)
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
