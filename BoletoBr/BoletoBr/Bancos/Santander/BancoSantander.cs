using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Dominio.EspecieDocumento;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;

namespace BoletoBr.Bancos.Santander
{
    public class BancoSantander : IBanco
    {
        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        public Image LogotipoBancoParaExibicao { get; set; }
        public string LocalDePagamento { get; private set; }
        public string MoedaBanco { get; private set; }

        public List<CarteiraCobranca> GetCarteirasCobranca()
        {
            throw new NotImplementedException();
        }

        public CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira)
        {
            throw new NotImplementedException();
        }

        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            throw new NotImplementedException();
        }

        public void FormataMoeda(Boleto boleto)
        {
            throw new NotImplementedException();
        }

        public void FormatarBoleto(Boleto boleto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        ///   *******
        /// 
        ///	O Código de barra para cobrança contém 44 posições dispostas da seguinte forma:
        ///    01 a 03 -  3 - 033 fixo - Código do banco
        ///    04 a 04 -  1 - 9 fixo - Código da moeda (R$)
        ///    05 a 05 -  1 - Dígito verificador do Código de barras
        ///    06 a 09 -  4 - Fator de vencimento
        ///    10 a 19 - 10 - Valor
        ///    20 a 20 -  1 - Fixo 9
        ///    21 a 27 -  7 - Código do cedente padrão satander
        ///    28 a 40 - 13 - Nosso número
        ///    41 - 41 - 1 -  IOS  - Seguradoras(Se 7% informar 7. Limitado  a 9%) Demais clientes usar 0 
        ///    42 - 44 - 3 - Tipo de modalidade da carteira 101, 102, 201
        /// 
        ///   *******
        /// 
        /// </summary>
        public void FormataCodigoBarra(Boleto boleto)
        {
            string codigoBanco = this.CodigoBanco.PadLeft(3, '0'); //3
            string codigoMoeda = boleto.Moeda; //1
            string fatorVencimento = Common.FatorVencimento(boleto.DataVencimento).ToString(); //4
            string valorNominal = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "").PadLeft(10, '0'); //10
            string fixo = "9"; //1
            string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0'); //7
            string nossoNumero = boleto.SequencialNossoNumero + Mod11Santander(boleto.SequencialNossoNumero, 9); //13
            //string IOS = boleto.PercentualIOS.ToString(); //1
            string tipoCarteira = boleto.CarteiraCobranca.Codigo; //3;
            boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                codigoBanco, codigoMoeda, fatorVencimento, valorNominal, fixo, codigoCedente, nossoNumero, /*IOS,*/
                tipoCarteira);

            string calculoDV = Mod10Mod11Santander(boleto.CodigoBarraBoleto, 9).ToString();

            boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                codigoBanco, codigoMoeda, calculoDV, fatorVencimento, valorNominal, fixo, codigoCedente, nossoNumero,
                /*IOS,*/ tipoCarteira);
        }

        /// <summary>
        /// 
        ///   *******
        /// 
        ///	A Linha Digitavel para cobrança contém 44 posições dispostas da seguinte forma:
        ///   1º Grupo - 
        ///    01 a 03 -  3 - 033 fixo - Código do banco
        ///    04 a 04 -  1 - 9 fixo - Código da moeda (R$) outra moedas 8
        ///    05 a 05 –  1 - Fixo 9
        ///    06 a 09 -  4 - Código cedente padrão santander
        ///    10 a 10 -  1 - Código DV do primeiro grupo
        ///   2º Grupo -
        ///    11 a 13 –  3 - Restante do código cedente
        ///    14 a 20 -  7 - 7 primeiros campos do nosso número
        ///    21 a 21 - 13 - Código DV do segundo grupo
        ///   3º Grupo -  
        ///    22 - 27 - 6 -  Restante do nosso número
        ///    28 - 28 - 1 - IOS  - Seguradoras(Se 7% informar 7. Limitado  a 9%) Demais clientes usar 0 
        ///    29 - 31 - 3 - Tipo de carteira
        ///    32 - 32 - 1 - Código DV do terceiro grupo
        ///   4º Grupo -
        ///    33 - 33 - 1 - Composto pelo DV do código de barras
        ///   5º Grupo -
        ///    34 - 36 - 4 - Fator de vencimento
        ///    37 - 47 - 10 - Valor do título
        ///   *******
        /// 
        /// </summary>
        public void FormataLinhaDigitavel(Boleto boleto)
        {
            string nossoNumero = boleto.SequencialNossoNumero +
                                 Mod11Santander(boleto.SequencialNossoNumero, 9); //13
            string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0');
            string fatorVencimento = Common.FatorVencimento(boleto.DataVencimento).ToString();
            //string IOS = boleto.PercentualIOS.ToString(); //1

            #region Grupo1

            string codigoBanco = this.CodigoBanco.PadLeft(3, '0'); //3
            string codigoModeda = boleto.Moeda.ToString(); //1
            string fixo = "9"; //1
            string codigoCedente1 = codigoCedente.Substring(0, 4); //4
            string calculoDV1 =
                Common.Mod10(string.Format("{0}{1}{2}{3}", codigoBanco, codigoModeda, fixo, codigoCedente1)).ToString(); //1
            string grupo1 = string.Format("{0}{1}{2}.{3}{4}", codigoBanco, codigoModeda, fixo, codigoCedente1,
                calculoDV1);

            #endregion

            #region Grupo2

            string codigoCedente2 = codigoCedente.Substring(4, 3); //3
            string nossoNumero1 = nossoNumero.Substring(0, 7); //7
            string calculoDV2 = Common.Mod10(string.Format("{0}{1}", codigoCedente2, nossoNumero1)).ToString();
            string grupo2 = string.Format("{0}{1}{2}", codigoCedente2, nossoNumero1, calculoDV2);
            grupo2 = " " + grupo2.Substring(0, 5) + "." + grupo2.Substring(5, 6);

            #endregion

            #region Grupo3

            string nossoNumero2 = nossoNumero.Substring(7, 6); //6

            string tipoCarteira = boleto.CarteiraCobranca.Codigo; //3
            string calculoDV3 = Common.Mod10(string.Format("{0}{1}{2}", nossoNumero2, /*IOS,*/ tipoCarteira)).ToString(); //1
            string grupo3 = string.Format("{0}{1}{2}{3}", nossoNumero2, /*IOS,*/ tipoCarteira, calculoDV3);
            grupo3 = " " + grupo3.Substring(0, 5) + "." + grupo3.Substring(5, 6) + " ";

            #endregion

            #region Grupo4

            string DVcodigoBanco = this.CodigoBanco.PadLeft(3, '0'); //3
            string DVcodigoMoeda = MoedaBanco; //1
            string DVvalorNominal = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "").PadLeft(10, '0'); //10
            string DVfixo = "9"; //1
            string DVcodigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0'); //7
            string DVnossoNumero = boleto.SequencialNossoNumero + Mod11Santander(boleto.SequencialNossoNumero, 9);
            string DVtipoCarteira = boleto.CarteiraCobranca.Codigo; //3;

            string calculoDVcodigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                DVcodigoBanco, DVcodigoMoeda, fatorVencimento, DVvalorNominal, DVfixo, DVcodigoCedente, DVnossoNumero
                /*IOS,*/, DVtipoCarteira);

            string grupo4 = Mod10Mod11Santander(calculoDVcodigo, 9).ToString() + " ";

            #endregion

            #region Grupo5

            //4
            string valorNominal = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "").PadLeft(10, '0'); //10

            string grupo5 = string.Format("{0}{1}", fatorVencimento, valorNominal);
            //grupo5 = grupo5.Substring(0, 4) + " " + grupo5.Substring(4, 1)+" "+grupo5.Substring(5,9);

            #endregion

            boleto.LinhaDigitavelBoleto = string.Format("{0}{1}{2}{3}{4}", grupo1, grupo2, grupo3, grupo4, grupo5);

            //Usado somente no Santander
            boleto.CedenteBoleto.ContaBancariaCedente.Conta = boleto.CedenteBoleto.CodigoCedente;
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            boleto.SetNossoNumeroFormatado(boleto.SequencialNossoNumero);

            boleto.SetNossoNumeroFormatado(string.Format("{0}-{1}", boleto.NossoNumeroFormatado, Mod11Santander(boleto.NossoNumeroFormatado, 9)));
        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public IInstrucao ObtemInstrucaoPadronizada(EnumTipoInstrucao tipoInstrucao, double valorInstrucao)
        {
            throw new NotImplementedException();
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

        public void ValidaBoleto(Boleto boleto)
        {
            //throw new NotImplementedException("Função não implementada.");
            if (!((boleto.CarteiraCobranca.Codigo == "102") || (boleto.CarteiraCobranca.Codigo == "101") || (boleto.CarteiraCobranca.Codigo == "201")))
                throw new NotImplementedException("Carteira não implementada.");

            //Banco 353  - Utilizar somente 08 posições do Nosso Numero (07 posições + DV), zerando os 05 primeiros dígitos
            if (this.CodigoBanco == "353")
            {
                if (boleto.NossoNumeroFormatado.Length != 7)
                    throw new NotImplementedException("Nosso Número deve ter 7 posições para o banco 353.");
            }

            //Banco 008  - Utilizar somente 09 posições do Nosso Numero (08 posições + DV), zerando os 04 primeiros dígitos
            if (this.CodigoBanco == "008")
            {
                if (boleto.NossoNumeroFormatado.Length != 8)
                    throw new NotImplementedException("Nosso Número deve ter 7 posições para o banco 008.");
            }

            if (this.CodigoBanco == "033")
            {
                if (boleto.NossoNumeroFormatado.Length == 7 && boleto.CarteiraCobranca.Codigo.Equals("101"))
                    boleto.SetNossoNumeroFormatado(boleto.NossoNumeroFormatado);

                if (boleto.NossoNumeroFormatado.Length != 12)
                    throw new NotSupportedException("Nosso Número deve ter 12 posições para o banco 033.");
            }
            if (boleto.CedenteBoleto.CodigoCedente.ToString().Length > 7)
                throw new NotImplementedException("Código cedente deve ter 7 posições.");

            boleto.LocalPagamento += "Grupo Santander - GC";

            if (EspecieDocumento.ValidaSigla(boleto.Especie) == "")
                boleto.Especie = new EspecieDocumentoSantander("2");

            //if (boleto.PercentualIOS > 10 & (this.CodigoBanco == "008" || this.CodigoBanco == "033" || this.CodigoBanco == "353"))
                //throw new Exception("O percentual do IOS é limitado a 9% para o Banco Santander");

            FormatarBoleto(boleto);
        }

        private static int Mod11Santander(string seq, int lim)
        {
            int ndig = 0;
            int nresto = 0;
            int total = 0;
            int multiplicador = 5;

            while (seq.Length > 0)
            {
                int valorPosicao = Convert.ToInt32(seq.Substring(0, 1));
                total += valorPosicao*multiplicador;
                multiplicador--;

                if (multiplicador == 1)
                {
                    multiplicador = 9;
                }

                seq = seq.Remove(0, 1);
            }

            nresto = total - ((total/11)*11);

            if (nresto == 0 || nresto == 1)
                ndig = 0;
            else if (nresto == 10)
                ndig = 1;
            else
                ndig = (11 - nresto);

            return ndig;
        }

        private static int Mod10Mod11Santander(string seq, int lim)
        {
            int ndig = 0;
            int nresto = 0;
            int total = 0;
            int multiplicador = 2;

            char[] posicaoSeq = seq.ToCharArray();
            Array.Reverse(posicaoSeq);
            string sequencia = new string(posicaoSeq);

            while (sequencia.Length > 0)
            {
                int valorPosicao = Convert.ToInt32(sequencia.Substring(0, 1));
                total += valorPosicao*multiplicador;
                multiplicador++;

                if (multiplicador == 10)
                {
                    multiplicador = 2;
                }

                sequencia = sequencia.Remove(0, 1);
            }

            nresto = (total*10)%11; //nresto = (((total * 10) / 11) % 10); Jefhtavares em 19/03/14


            if (nresto == 0 || nresto == 1 || nresto == 10)
                ndig = 1;
            else
                ndig = nresto;

            return ndig;
        }

        protected static int Mult10Mod11Santander(string seq, int lim, int flag)
        {
            int mult = 0;
            int total = 0;
            int pos = 1;
            int ndig = 0;
            int nresto = 0;
            string num = string.Empty;

            mult = 1 + (seq.Length%(lim - 1));

            if (mult == 1)
                mult = lim;

            while (pos <= seq.Length)
            {
                num = Common.Mid(seq, pos, 1);
                total += Convert.ToInt32(num)*mult;

                mult -= 1;
                if (mult == 1)
                    mult = lim;

                pos += 1;
            }

            nresto = ((total*10)%11);

            if (flag == 1)
                return nresto;
            else
            {
                if (nresto == 0 || nresto == 1)
                    ndig = 0;
                else if (nresto == 10)
                    ndig = 1;
                else
                    ndig = (11 - nresto);

                return ndig;
            }
        }

        /// <summary>
        /// Verifica o tipo de ocorrência para o arquivo remessa
        /// </summary>
        public string Ocorrencia(string codigo)
        {
            switch (codigo)
            {
                case "01":
                    return "01-Título não existe";
                case "02":
                    return "02-Entrada Confirmada";
                case "03":
                    return "03-Entrada Rejeitada";
                case "06":
                    return "06-Liquidação";
                case "07":
                    return "07-Liquidação por conta";
                case "08":
                    return "08-Liquidação por saldo";
                case "09":
                    return "09-Baixa Automatica";
                case "10":
                    return "10-Baixa conf. instrução ou protesto";
                case "11":
                    return "11-Em Ser";
                case "12":
                    return "12-Abatimento Concedido";
                case "13":
                    return "13-Abatimento Cancelado";
                case "14":
                    return "14-Prorrogação de Vencimento";
                case "15":
                    return "15-Enviado para Cartório";
                case "16":
                    return "16-Título já baixado/liquidado";
                case "17":
                    return "17-Liquidado em cartório";
                case "21":
                    return "21-Entrada em cartório";
                case "22":
                    return "22-Retirado de cartório";
                case "24":
                    return "24-Custas de cartório";
                case "25":
                    return "25-Protestar Título";
                case "26":
                    return "26-Sustar protesto";
                default:
                    return "";
            }
        }
    }
}
