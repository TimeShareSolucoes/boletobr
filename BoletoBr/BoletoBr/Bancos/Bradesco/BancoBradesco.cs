using System;
using System.Collections.Generic;
using BoletoBr.CalculoModulo;

namespace BoletoBr.Bancos.Bradesco
{
    public class BancoBradesco : IBanco
    {
        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }

        public BancoBradesco()
        {
            CodigoBanco = "237";
            DigitoBanco = "2";
            NomeBanco = "Bradesco";

            _carteirasCobranca = new List<CarteiraCobranca>();
            _carteirasCobranca.Add(new CarteiraCobrancaBradesco09());
        }

        public string CalcularDigitoNossoNumero(Boleto boleto)
        {
            return Common.Mod11Bradesco(boleto.CarteiraCobranca.Codigo + boleto.NossoNumeroFormatado, 7);
        }
        private int _digitoAutoConferenciaBoleto;
        private string _digitoAutoConferenciaNossoNumero;

        private readonly List<CarteiraCobranca> _carteirasCobranca;

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
            if (boleto.CarteiraCobranca.Codigo != "02" && boleto.CarteiraCobranca.Codigo != "03" && boleto.CarteiraCobranca.Codigo != "06" && boleto.CarteiraCobranca.Codigo != "09" && boleto.CarteiraCobranca.Codigo != "19")
                throw new ValidacaoBoletoException("Carteira não implementada. Carteiras implementadas 02, 03, 06, 09, 19.");

            //O valor � obrigat�rio para a carteira 03
            if (boleto.CarteiraCobranca.Codigo == "03")
            {
                if (boleto.ValorBoleto == 0)
                    throw new ValidacaoBoletoException("Para a carteira 03, o valor do boleto n�o pode ser igual a zero");
            }

            //O valor � obrigat�rio para a carteira 09
            if (boleto.CarteiraCobranca.Codigo == "09")
            {
                if (boleto.ValorBoleto == 0)
                    throw new ValidacaoBoletoException("Para a carteira 09, o valor do boleto não pode ser igual a zero");
            }

            //Verifica se o nosso n�mero � v�lido
            if (boleto.NossoNumeroFormatado.Length > 11)
                throw new ValidacaoBoletoException("A quantidade de d�gitos do nosso n�mero, s�o 11 n�meros.");
            else if (boleto.NossoNumeroFormatado.Length < 11)
                boleto.SetNossoNumeroFormatado(boleto.NossoNumeroFormatado.PadLeft(11, '0'));

            //Verificar se a Agencia esta correta
            if (boleto.CedenteBoleto.ContaBancariaCedente.Agencia.Length > 4)
                throw new ValidacaoBoletoException("A quantidade de d�gitos da Ag�ncia " + boleto.CedenteBoleto.ContaBancariaCedente.Agencia + ", s�o de 4 n�meros.");
            else if (boleto.CedenteBoleto.ContaBancariaCedente.Agencia.Length < 4)
                boleto.CedenteBoleto.ContaBancariaCedente.Agencia = boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4,'0');

            //Verificar se a Conta esta correta
            if (boleto.CedenteBoleto.ContaBancariaCedente.Conta.Length > 7)
                throw new ValidacaoBoletoException("A quantidade de d�gitos da Conta " + boleto.CedenteBoleto.ContaBancariaCedente.Conta + ", s�o de 07 n�meros.");
            else if (boleto.CedenteBoleto.ContaBancariaCedente.Conta.Length < 7)
                boleto.CedenteBoleto.ContaBancariaCedente.Conta =
                    boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(7, '0');

            //Verifica se data do processamento � valida
            //if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;


            //Verifica se data do documento � valida
            //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;
        }

        public void FormatarBoleto(Boleto boleto)
        {
            boleto.ValidaDadosEssenciaisDoBoleto();
            ValidaBoletoComNormasBanco(boleto);

            boleto.QuantidadeMoeda = 0;

            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento = "PAGÁVEL PREFERENCIALMENTE NAS AGÊNCIAS DO BRADESCO";

            // Calcula o DAC do Nosso Número
            _digitoAutoConferenciaNossoNumero = CalcularDigitoNossoNumero(boleto);
            boleto.DigitoNossoNumero = _digitoAutoConferenciaNossoNumero;

            FormataNossoNumero(boleto);
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
        }
        /// <summary>
        /// 
        ///   *******
        /// 
        ///	O c�digo de barra para cobran�a cont�m 44 posi��es dispostas da seguinte forma:
        ///    01 a 03 - 3 - Identifica��o  do  Banco
        ///    04 a 04 - 1 - C�digo da Moeda
        ///    05 a 05 � 1 - D�gito verificador do C�digo de Barras
        ///    06 a 09 - 4 - Fator de vencimento
        ///    10 a 19 - 10 - Valor
        ///    20 a 44 � 25 - Campo Livre
        /// 
        ///   *******
        /// 
        /// </summary>
        public void FormataCodigoBarra(Boleto boleto)
        {
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = valorBoleto.PadLeft(10, '0');

            if (boleto.CarteiraCobranca.Codigo == "02" || boleto.CarteiraCobranca.Codigo == "03" || boleto.CarteiraCobranca.Codigo == "09" || boleto.CarteiraCobranca.Codigo == "19")
            {
                boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}", this.CodigoBanco, boleto.Moeda,
                Common.FatorVencimento(boleto.DataVencimento), valorBoleto, FormataCampoLivre(boleto));
            }
            else if (boleto.CarteiraCobranca.Codigo == "06")
            {
                if (boleto.ValorBoleto == 0)
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}0000{2}{3}", this.CodigoBanco, boleto.Moeda,
                        valorBoleto, FormataCampoLivre(boleto));
                }
                else
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}", this.CodigoBanco, boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento), valorBoleto, FormataCampoLivre(boleto));
                }

            }
            else
            {
                throw new NotImplementedException("Carteira ainda n�o implementada.");
            }


            _digitoAutoConferenciaBoleto = Common.Mod11(boleto.CodigoBarraBoleto, 9);

            boleto.CodigoBarraBoleto = Common.Left(boleto.CodigoBarraBoleto, 4) + _digitoAutoConferenciaBoleto + Common.Right(boleto.CodigoBarraBoleto, 39);
        }

        ///<summary>
        /// Campo Livre
        ///    20 a 23 -  4 - Ag�ncia Cedente (Sem o digito verificador,completar com zeros a esquerda quandonecess�rio)
        ///    24 a 25 -  2 - Carteira
        ///    26 a 36 - 11 - N�mero do Nosso N�mero(Sem o digito verificador)
        ///    37 a 43 -  7 - Conta do Cedente (Sem o digito verificador,completar com zeros a esquerda quando necess�rio)
        ///    44 a 44	- 1 - Zero            
        ///</summary>
        public string FormataCampoLivre(Boleto boleto)
        {

            string FormataCampoLivre = string.Format("{0}{1}{2}{3}{4}", boleto.CedenteBoleto.ContaBancariaCedente.Agencia, boleto.CarteiraCobranca.Codigo,
                                            boleto.NossoNumeroFormatado, boleto.CedenteBoleto.ContaBancariaCedente.Conta, "0");

            return FormataCampoLivre;
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {

            //BBBMC.CCCCD1 CCCCC.CCCCCD2 CCCCC.CCCCCD3 D4 FFFFVVVVVVVVVV


            #region Campo 1

            string Grupo1 = string.Empty;

            string BBB = boleto.CodigoBarraBoleto.Substring(0, 3);
            string M = boleto.CodigoBarraBoleto.Substring(3, 1);
            string CCCCC = boleto.CodigoBarraBoleto.Substring(19, 5);
            string D1 = Common.Mod10(BBB + M + CCCCC).ToString();

            Grupo1 = string.Format("{0}{1}{2}.{3}{4} ", BBB, M, CCCCC.Substring(0, 1), CCCCC.Substring(1, 4), D1);


            #endregion Campo 1

            #region Campo 2

            string Grupo2 = string.Empty;

            string CCCCCCCCCC2 = boleto.CodigoBarraBoleto.Substring(24, 10);
            string D2 = Common.Mod10(CCCCCCCCCC2).ToString();

            Grupo2 = string.Format("{0}.{1}{2} ", CCCCCCCCCC2.Substring(0, 5), CCCCCCCCCC2.Substring(5, 5), D2);

            #endregion Campo 2

            #region Campo 3

            string Grupo3 = string.Empty;

            string CCCCCCCCCC3 = boleto.CodigoBarraBoleto.Substring(34, 10);
            string D3 = Common.Mod10(CCCCCCCCCC3).ToString();

            Grupo3 = string.Format("{0}.{1}{2} ", CCCCCCCCCC3.Substring(0, 5), CCCCCCCCCC3.Substring(5, 5), D3);


            #endregion Campo 3

            #region Campo 4

            string Grupo4 = string.Empty;

            string D4 = _digitoAutoConferenciaBoleto.ToString();

            Grupo4 = string.Format("{0} ", D4);

            #endregion Campo 4

            #region Campo 5

            string Grupo5 = string.Empty;

            //string FFFF = boleto.CodigoBarra.Codigo.Substring(5, 4);//FatorVencimento(boleto).ToString() ;
            string FFFF = Common.FatorVencimento(boleto.DataVencimento).ToString();

            //if (boleto.Carteira == "06" && !Utils.DataValida(boleto.DataVencimento))
            //    FFFF = "0000";

            string VVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            VVVVVVVVVV = VVVVVVVVVV.PadLeft(10, '0');

            //if (Utils.ToInt64(VVVVVVVVVV) == 0)
            //    VVVVVVVVVV = "000";

            Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

            #endregion Campo 5

            boleto.LinhaDigitavelBoleto = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;

        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função ainda não implementada.");
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            boleto.SetNossoNumeroFormatado(
                string.Format("{0}/{1}-{2}",
                    boleto.CarteiraCobranca.Codigo,
                    boleto.NossoNumeroFormatado,
                    boleto.DigitoNossoNumero));
        }
    }
}
