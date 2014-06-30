using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Cef
{
    public class BancoCef : IBanco
    {
        /* 
        * TipoDocumento 1 - SICGB - Com registro
        * TipoDocumento 2 - SICGB - Sem registro
        */

        // Identificador de Tipo de Cobrança
        private const string IdentificadorTipoCobrancaCarteiraRg = "1";
        private const string IdentificadorTipoCobrancaCarteiraSr = "2";
        // Identificador de Emissão do Boleto (4 - Beneficiário)
        private const string IdentificadorEmissaoCedente = "4";

        private string _dacBoleto = String.Empty;

        private bool protestar = false;
        private bool baixaDevolver = false;
        private bool desconto = false;
        private int diasProtesto = 0;
        private int diasDevolucao = 0;
        private int diasDesconto = 0;

        public BancoCef()
        {
            this.CodigoBanco = "104";
            this.DigitoBanco = "0";
            this.NomeBanco = "Caixa Econômica Federal";

            /* Adiciona carteiras de cobrança */
            _carteirasCobrancaCef = new List<CarteiraCobranca>();
            _carteirasCobrancaCef.Add(new CarteiraCobrancaCefRg());
            _carteirasCobrancaCef.Add(new CarteiraCobrancaCefSr());
        }

        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }

        private readonly List<CarteiraCobranca> _carteirasCobrancaCef;

        public List<CarteiraCobranca> GetCarteirasCobranca()
        {
            return _carteirasCobrancaCef;
        }

        public CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira)
        {
            return GetCarteirasCobranca().Find(fd => fd.Codigo == codigoCarteira);
        }

        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            if (boleto.CarteiraCobranca.Codigo.Equals("SR"))
            {
                if ((boleto.NossoNumeroFormatado.Length != 10) && (boleto.NossoNumeroFormatado.Length != 14) &&
                    (boleto.NossoNumeroFormatado.Length != 17))
                {
                    throw new Exception(
                        "Nosso N�mero inv�lido, Para Caixa Econ�mica - Carteira SR o Nosso N�mero deve conter 10, 14 ou 17 posi��es.");
                }
            }
            else if (boleto.CarteiraCobranca.Codigo.Equals("RG"))
            {
                if (boleto.NossoNumeroFormatado.Length != 17)
                    throw new Exception(
                        "Nosso n�mero inv�lido. Para Caixa Econ�mica - SIGCB carteira r�pida, o nosso n�mero deve conter 17 caracteres.");
            }
            else if (boleto.CarteiraCobranca.Codigo.Equals("CS"))
            {
                foreach (char ch in boleto.NossoNumeroFormatado)
                {
                    if (!ch.Equals('0'))
                        throw new Exception(
                            "Nosso N�mero inv�lido, Para Caixa Econ�mica - SIGCB carteira simples, o Nosso N�mero deve estar zerado.");
                }
            }
            else
            {
                if (boleto.NossoNumeroFormatado.Length != 10)
                {
                    throw new Exception(
                        "Nosso N�mero inv�lido, Para Caixa Econ�mica carteira indefinida, o Nosso N�mero deve conter 10 caracteres.");
                }

                if (!boleto.CedenteBoleto.CodigoCedente.Equals(0))
                {
                    string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(6, '0');
                    string dvCodigoCedente = Common.Mod10(codigoCedente).ToString(); //Base9 

                    if (boleto.CedenteBoleto.DigitoCedente.Equals(-1))
                        boleto.CedenteBoleto.DigitoCedente = Convert.ToInt32(dvCodigoCedente);
                }
                else
                {
                    throw new Exception("Informe o c�digo do cedente.");
                }
            }

            if (boleto.CedenteBoleto.DigitoCedente == -1)
                boleto.CedenteBoleto.DigitoCedente = Common.Mod11Base9(boleto.CedenteBoleto.CodigoCedente);

            if (boleto.DataDocumento == DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;

            if (boleto.CedenteBoleto.CodigoCedente.Length > 6)
                throw new Exception("O c�digo do cedente deve conter apenas 6 d�gitos");

            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento = "PREFERENCIALMENTE NAS CASAS LOT�RICAS E AG�NCIAS DA CAIXA";

            /* 
             * Na Carteira Simples n�o � necess�rio gerar a impress�o do boleto,
             * logo n�o � necess�rio formatar linha digit�vel nem c�d de barras
             * J�ferson (jefhtavares) em 10/03/14
             */

            if (!boleto.CarteiraCobranca.Codigo.Equals("CS"))
            {
                FormataCodigoBarra(boleto);
                FormataLinhaDigitavel(boleto);
                FormataNossoNumero(boleto);
            }
        }

        public void FormatarBoleto(Boleto boleto)
        {
            boleto.ValidaDadosEssenciaisDoBoleto();

            ValidaBoletoComNormasBanco(boleto);

            //Atribui o local de pagamento
            boleto.LocalPagamento = "Preferencialmente nas casas lotéricas até o vencimento.";

            FormataNumeroDocumento(boleto);
            FormataNossoNumero(boleto);
            FormataLinhaDigitavel(boleto);
            FormataCodigoBarra(boleto);
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            // Posi��o 01-03
            string banco = CodigoBanco;

            //Posi��o 04
            string moeda = "9";

            //Posi��o 05 - No final ...   

            // Posi��o 06 - 09
            long fatorVencimento = Common.FatorVencimento(boleto.DataVencimento);

            // Posi��o 10 - 19     
            string valorDocumento = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorDocumento = valorDocumento.PadLeft(10, '0');

            // Inicio Campo livre
            string campoLivre = string.Empty;

            //ESSA IMPLEMENTA��O FOI FEITA PARA CARTEIAS SIGCB "SR" COM NOSSO NUMERO DE 14 e 17 POSI��ES
            if (boleto.CarteiraCobranca != null)
            {
                if ((boleto.CarteiraCobranca.Equals("SR")) || (boleto.CarteiraCobranca.Equals("RG")))
                {
                    //14 POSI�OES
                    if (boleto.NossoNumeroFormatado.Length == 14)
                    {
                        //Posi��o 20 - 24
                        string contaCedente = boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(5, '0');

                        // Posi��o 25 - 28
                        string agenciaCedente = boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0');

                        //Posi��o 29
                        const string codigoCarteira = "8";

                        //Posi��o 30
                        const string constante = "7";

                        //Posi��o 31 - 44
                        string nossoNumero = boleto.NossoNumeroFormatado;

                        campoLivre = string.Format("{0}{1}{2}{3}{4}", contaCedente, agenciaCedente, codigoCarteira,
                            constante, nossoNumero);
                    }
                    //17 POSI��ES
                    if (boleto.NossoNumeroFormatado.Length == 17)
                    {
                        //104 - Caixa Econ�mica Federal S.A. 
                        //Carteira SR - 24 (cobran�a sem registro) || Carteira RG - 14 (cobran�a com registro)
                        //Cobran�a sem registro, nosso n�mero com 17 d�gitos. 

                        //Posi��o 20 - 25
                        string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(6, '0');

                        // Posi��o 26
                        string dvCodigoCedente = Common.Mod11Base9(codigoCedente).ToString();

                        //Posi��o 27 - 29
                        //De acordo com documenta��o, posi��o 3 a 5 do nosso numero
                        string primeiraParteNossoNumero = boleto.NossoNumeroFormatado.Substring(2, 3);

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
                        string segundaParteNossoNumero = boleto.NossoNumeroFormatado.Substring(5, 3);

                        // Posi��o 34
                        const string segundaConstante = "4"; // 4 => emiss�o do boleto pelo cedente

                        //Posi��o 35 - 43
                        //De acordo com documenta�ao, posi��o 9 a 17 do nosso numero
                        string terceiraParteNossoNumero = boleto.NossoNumeroFormatado.Substring(8, 9);

                        //Posi��o 44
                        string ccc = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                            codigoCedente,
                            dvCodigoCedente,
                            primeiraParteNossoNumero,
                            primeiraConstante,
                            segundaParteNossoNumero,
                            segundaConstante,
                            terceiraParteNossoNumero);
                        string dvCampoLivre = Common.Mod11Base9(ccc).ToString();
                        campoLivre = string.Format("{0}{1}", ccc, dvCampoLivre);
                    }
                }
                else
                {
                    //Posi��o 20 - 25
                    string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(6, '0');

                    // Posi��o 26
                    string dvCodigoCedente = Common.Mod11Base9(codigoCedente).ToString();

                    //Posi��o 27 - 29
                    string primeiraParteNossoNumero = boleto.NossoNumeroFormatado.Substring(2, 3);

                    //104 - Caixa Econ�mica Federal S.A. 
                    //Carteira 01. 
                    //Cobran�a r�pida. 
                    //Cobran�a sem registro. 
                    //Cobran�a sem registro, nosso n�mero com 16 d�gitos. 
                    //Cobran�a simples 

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
                    string segundaParteNossoNumero = boleto.NossoNumeroFormatado.Substring(5, 3); //(3, 3);

                    // Posi��o 24
                    string segundaConstante = IdentificadorEmissaoCedente;

                    //Posi��o 35 - 43
                    string terceiraParteNossoNumero = boleto.NossoNumeroFormatado.Substring(8, 9);

                    //Posi��o 44
                    string ccc = string.Format("{0}{1}{2}{3}{4}{5}{6}", codigoCedente, dvCodigoCedente,
                        primeiraParteNossoNumero,
                        primeiraConstante, segundaParteNossoNumero, segundaConstante,
                        terceiraParteNossoNumero);

                    string dvCampoLivre = Common.Mod11Base9(ccc).ToString();

                    campoLivre = string.Format("{0}{1}", ccc, dvCampoLivre);
                }

                string xxxx = string.Format("{0}{1}{2}{3}{4}", banco, moeda, fatorVencimento, valorDocumento, campoLivre);

                string dvGeral = Common.Mod11(xxxx, 9).ToString();
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
            else
            {
                throw new Exception("Carteira de cobrança não especificada");
            }
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

            /// <summary>
            ///   IMPLEMENTA��O PARA NOSSO N�MERO COM 17 POSI��ES
            ///   Autor.: F�bio Marcos
            ///   E-Mail: fabiomarcos@click21.com.br
            ///   Data..: 01/03/2011
            /// </summary>
            if (boleto.NossoNumeroFormatado.Length == 17)
            {
                #region Campo 1

                //POSI��O 1 A 4 DO CODIGO DE BARRAS
                str1 = boleto.CodigoBarraBoleto.Substring(0, 4);
                //POSICAO 20 A 24 DO CODIGO DE BARRAS
                str2 = boleto.CodigoBarraBoleto.Substring(19, 5);
                //CALCULO DO DIGITO
                str3 = Common.Mod10(str1 + str2).ToString();

                Grupo1 = str1 + str2 + str3;
                Grupo1 = Grupo1.Substring(0, 5) + "." + Grupo1.Substring(5) + " ";

                #endregion Campo 1

                #region Campo 2

                //POSI��O 25 A 34 DO COD DE BARRAS
                str1 = boleto.CodigoBarraBoleto.Substring(24, 10);
                //DIGITO
                str2 = Common.Mod10(str1).ToString();

                Grupo2 = string.Format("{0}.{1}{2} ", str1.Substring(0, 5), str1.Substring(5, 5), str2);

                #endregion Campo 2

                #region Campo 3

                //POSI��O 35 A 44 DO CODIGO DE BARRAS
                str1 = boleto.CodigoBarraBoleto.Substring(34, 10);
                //DIGITO
                str2 = Common.Mod10(str1).ToString();

                Grupo3 = string.Format("{0}.{1}{2} ", str1.Substring(0, 5), str1.Substring(5, 5), str2);

                #endregion Campo 3

                #region Campo 4

                string D4 = _dacBoleto.ToString();

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
                string D1 = Common.Mod10(BBB + M + CCCCC).ToString();

                Grupo1 = string.Format("{0}{1}{2}.{3}{4} ",
                    BBB,
                    M,
                    CCCCC.Substring(0, 1),
                    CCCCC.Substring(1, 4), D1);


                #endregion Campo 1

                #region Campo 2

                string CCCCCCCCCC2 = boleto.CodigoBarraBoleto.Substring(24, 10);
                string D2 = Common.Mod10(CCCCCCCCCC2).ToString();

                Grupo2 = string.Format("{0}.{1}{2} ", CCCCCCCCCC2.Substring(0, 5), CCCCCCCCCC2.Substring(5, 5), D2);

                #endregion Campo 2

                #region Campo 3

                string CCCCCCCCCC3 = boleto.CodigoBarraBoleto.Substring(34, 10);
                string D3 = Common.Mod10(CCCCCCCCCC3).ToString();

                Grupo3 = string.Format("{0}.{1}{2} ", CCCCCCCCCC3.Substring(0, 5), CCCCCCCCCC3.Substring(5, 5), D3);


                #endregion Campo 3

                #region Campo 4

                string D4 = _dacBoleto.ToString();

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
            boleto.SetNossoNumeroFormatado(boleto.SequencialNossoNumero);

            //Atribui ao Nosso Número o Identificador de Cobrança + Identificador do Emissor
            if (boleto.CarteiraCobranca.Codigo.Equals("RG"))
            {
                boleto.SetNossoNumeroFormatado(
                    IdentificadorTipoCobrancaCarteiraRg +
                    IdentificadorEmissaoCedente +
                    boleto.NossoNumeroFormatado.PadLeft(15, '0'));
            }

            else if (boleto.CarteiraCobranca.Codigo.Equals("SR"))
                boleto.SetNossoNumeroFormatado(
                    IdentificadorTipoCobrancaCarteiraSr +
                    IdentificadorEmissaoCedente +
                    boleto.NossoNumeroFormatado.PadLeft(12, '0'));
            else
            {
                throw new Exception("Não há formatação para 'Nosso Número' informado");
            }
        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            boleto.NumeroDocumento = string.Format("{0}", boleto.NumeroDocumento);
        }
    }
}