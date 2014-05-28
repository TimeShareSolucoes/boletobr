using System;
using System.Collections.Generic;
using BoletoBr.CalculoModulo;
using BoletoBr.Bancos;

namespace BoletoBr.Bancos.Hsbc
{
    public class BancoHsbc : BancoAbstract, IBanco
    {
        public BancoHsbc()
        {
            CodigoBanco = "399";
            DigitoBanco = "9";
            NomeBanco = "HSBC";

            /* Adiciona carteiras de cobrança */
            _carteirasCobrancaHsbc.Add(new CarteiraCobrancaHsbcCnr());
            _carteirasCobrancaHsbc.Add(new CarteiraCobrancaHsbcCsb());
        }

        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }

        private int _digitoAutoConferenciaCodigoBarras;
        public ICalculadoraModulo10 CalculadoraModulo10 { get; set; }
        public ICalculadoraModulo11 CalculadoraModulo11 { get; set; }

        private readonly List<CarteiraCobranca> _carteirasCobrancaHsbc = new List<CarteiraCobranca>();
        public List<CarteiraCobranca> GetCarteirasCobranca()
        {
            return _carteirasCobrancaHsbc;
        }

        public CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira)
        {
            return this._carteirasCobrancaHsbc.Find(fd => fd.Codigo == codigoCarteira);
        }
        /// <summary>
        /// Valida se o boleto está preenchido com os campos mínimos requeridos.
        /// Dispara uma ApplicationException caso esteja faltando alguma informação.
        /// </summary>
        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            boleto.ValidaDadosEssenciaisDoBoleto();
        }

        public void FormatarBoleto(Boleto boleto)
        {
            FormataNumeroDocumento(boleto);
            FormataNossoNumero(boleto);
            FormataLinhaDigitavel(boleto);
            FormataCodigoBarra(boleto);
        }

        protected override void FormataCodigoBarra(Boleto boleto)
        {
            try
            {
                /* Preenche com 0´s a esquerda
                 * 10 caracteres
                 */
                string valorBoletoTexto = 
                    boleto.ValorBoleto.ToString("f")
                    .Replace(",", "")
                    .Replace(".", "")
                    .PadLeft(10, '0');

                string numeroDocumentoFormatado =
                    boleto.NumeroDocumento.PadLeft(7, '0');

                string codigoBarraSemDigitoVerificador = null;

                if (boleto.CarteiraCobranca.Codigo == "CSB")
                {
                    codigoBarraSemDigitoVerificador =
                        String.Format("{0}{1}{2}{3}{4}{5}{6}001",
                            this.CodigoBanco,
                            boleto.Moeda,
                            9999,
                            valorBoletoTexto,
                            boleto.NossoNumero + boleto.DigitoNossoNumero,
                            boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'),
                            boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(7, '0')
                            );
                }
                if (boleto.CarteiraCobranca.Codigo == "CNR")
                {
                    codigoBarraSemDigitoVerificador =
                        String.Format("{0}{1}{2}{3}{4}{5}{6}2",
                            this.CodigoBanco,
                            boleto.Moeda,
                            9999,
                            valorBoletoTexto,
                            boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0'),
                            boleto.NossoNumero.PadLeft(13, '0'),
                            (boleto.DataVencimento.DayOfYear +
                             boleto.DataVencimento.ToString("yy").Substring(1, 1)).PadLeft(4, '0')
                            );
                }

                /* 
                 * 1. Calcula dígito de auto conferência
                 * 2. Insere no meio do código de barras
                 * 3. Atribui ao boleto
                 */
                string codigoBarraComDigitoVerificador = null;

                _digitoAutoConferenciaCodigoBarras = Common.Mod11(codigoBarraSemDigitoVerificador, 9, 0);

                codigoBarraComDigitoVerificador =
                    Common.Left(codigoBarraSemDigitoVerificador, 4) +
                    _digitoAutoConferenciaCodigoBarras +
                    Common.Right(codigoBarraSemDigitoVerificador, 39);


                boleto.CodigoBarraBoleto = codigoBarraComDigitoVerificador;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao formatar código de barras.", ex);
            }
        }

        protected override void FormataLinhaDigitavel(Boleto boleto)
        {
            string nossoNumeroLinhaDigitavel = boleto.NossoNumero.PadLeft(13, '0');
            string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0');
            string digitoAutoConferenciaNossoNumero = Common.Mod11(boleto.NossoNumero, 7).ToString();

            string C1 = string.Empty;
            string C2 = string.Empty;
            string C3 = string.Empty;
            string C5 = string.Empty;

            string AAA;
            string B;
            string CCCCC;
            string X;


            string DDDDDD; 
            string DD;
            string EEEE; 
            string EEEEEEEE;
            string Y;
            string FFFFFFF; 
            string FFFFF;
            string GGGGG;
            string Z;

            if (boleto.CarteiraCobranca.Codigo == "CSB")
            {
                #region AAABC.CCCCX

                AAA = this.CodigoBanco.PadLeft(3, '0');
                B = boleto.Moeda.ToString();
                CCCCC = boleto.NossoNumero.Substring(0, 5);
                X = Common.Mod10(AAA + B + CCCCC).ToString();

                C1 = String.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
                C1 += String.Format("{0}{1} ", CCCCC.Substring(1, 4), X);
                #endregion

                #region DDDDD.DEEEEY

                DDDDDD = boleto.NossoNumero.Substring(5, 5) + digitoAutoConferenciaNossoNumero;
                EEEE = boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0');
                Y = Common.Mod10(DDDDDD + EEEE).ToString();

                C2 = String.Format("{0}.", DDDDDD.Substring(0, 5));
                C2 += string.Format("{0}{1}{2} ", DDDDDD.Substring(5, 1), EEEE, Y);

                #endregion

                #region FFFFF.FF001Z

                FFFFFFF = boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(7, '0');
                Z = Common.Mod10(FFFFFFF + "001").ToString();

                C3 = String.Format("{0}.", FFFFFFF.Substring(0, 5));
                C3 += String.Format("{0}001{1}", FFFFFFF.Substring(5, 2), Z);

                #endregion
            }
            if (boleto.CarteiraCobranca.Codigo == "CNR")
            {
                #region AAABC.CCCCX

                AAA = this.CodigoBanco.PadLeft(3, '0');
                B = boleto.Moeda.ToString();
                CCCCC = boleto.CedenteBoleto.CodigoCedente.Substring(0, 5);
                X = Common.Mod10(AAA + B + CCCCC).ToString();

                C1 = string.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
                C1 += string.Format("{0}{1} ", CCCCC.Substring(1, 4), X);

                #endregion AAABC.CCDDX

                #region DDEEE.EEEEEY

                DD = boleto.CedenteBoleto.CodigoCedente.Substring(5, 2);
                EEEEEEEE = nossoNumeroLinhaDigitavel.Substring(0, 8);
                Y = Common.Mod10(DD + EEEEEEEE).ToString();

                C2 = string.Format("{0}{1}.", DD, EEEEEEEE.Substring(0, 3));
                C2 += string.Format("{0}{1} ", EEEEEEEE.Substring(3, 5), Y);

                #endregion DDEEE.EEEEEY

                #region FFFFF.GGGGGZ

                FFFFF = nossoNumeroLinhaDigitavel.Substring(8, 5);
                GGGGG = (boleto.DataVencimento.DayOfYear + boleto.DataVencimento.ToString("yy").Substring(1, 1)).PadLeft(4, '0') + "2";

                Z = Common.Mod10(FFFFF + GGGGG).ToString();

                C3 = string.Format("{0}.", FFFFF);
                C3 += string.Format("{0}{1}", GGGGG, Z);

                #endregion FFFFF.GGGGGZ
            }

            string W = String.Format(" {0} ", _digitoAutoConferenciaCodigoBarras);

            #region HHHHIIIIIIIIII

            string HHHH = Common.FatorVencimento(boleto.DataVencimento).ToString();
            string IIIIIIIIII = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");

            IIIIIIIIII = IIIIIIIIII.PadLeft(10, '0');
                C5 = HHHH + IIIIIIIIII;

                #endregion HHHHHHHHHHHHHH

            boleto.LinhaDigitavelBoleto = C1+C2+C3+W+C5;
        }

        protected override void FormataNossoNumero(Boleto boleto)
        {
            try
            {
                /* Padroniza nosso número para 10 dígitos */
                string nossoNumeroFormatar = boleto.NossoNumero.PadLeft(10, '0');
                string digitoAutoConferenciaNossoNumero = Common.Mod11(nossoNumeroFormatar, 7).ToString();

                if (boleto.CarteiraCobranca.Codigo == "CSB")
                {
                    boleto.NossoNumero = String.Format("{0}{1}", nossoNumeroFormatar, digitoAutoConferenciaNossoNumero);
                    return;
                }
                if (boleto.CarteiraCobranca.Codigo == "CNR")
                {
                    string campo0 = nossoNumeroFormatar;
                    string campo1 = Common.Mod11Base9(nossoNumeroFormatar).ToString();
                    string campo2 =
                        Common.Mod11Base9(
                            (
                                long.Parse(nossoNumeroFormatar + Common.Mod11Base9(nossoNumeroFormatar) + "4") +
                                long.Parse(boleto.CedenteBoleto.CodigoCedente.ToString()) +
                                long.Parse(boleto.DataVencimento.ToString("ddMMyy"))
                                )
                                .ToString()
                            )
                            .ToString();

                        boleto.NossoNumero =
                            String.Format("{0}{1}4{2}",
                                campo0,
                                campo1,
                                campo2);
                        return;
                }

                throw new NotImplementedException("Modelo de carteira de cobrança: " + boleto.CarteiraCobranca.Codigo +
                                                  " não está implementado.");
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao tentar formatar nosso número.", ex);
            }
        }

        protected override void FormataNumeroDocumento(Boleto boleto)
        {
            boleto.NumeroDocumento = boleto.NumeroDocumento.PadLeft(7, '0');
        }
    }
}