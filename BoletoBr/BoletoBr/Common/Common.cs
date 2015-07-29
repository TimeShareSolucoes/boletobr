using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public static class Common
    {
        #region Cálculo de Módulo
        internal static int Mod10(string seq)
        {
            int digito;
            int soma = 0;
            int peso = 2;
            int resto;

            for (var i = seq.Length; i > 0; i--)
            {
                resto = (Convert.ToInt32(Common.Mid(seq, i, 1)) * peso);

                if (resto > 9)
                    resto = (resto / 10) + (resto % 10);

                soma += resto;

                if (peso == 2)
                    peso = 1;
                else
                    peso = peso + 1;
            }
            digito = ((10 - (soma % 10)) % 10);
            return digito;
        }

        public static int Mod11(string seq)
        {
            int digito;
            int soma = 0;
            int peso = 2;
            int baseCalculo = 9;

            for (int i = 0; i < seq.Length; i++)
            {
                soma = soma + (Convert.ToInt32(seq[i]) * peso);
                if (peso < baseCalculo)
                    peso = peso + 1;
                else
                    peso = 2;
            }

            digito = 11 - (soma % 11);
            if (digito > 9)
                digito = 0;
            return digito;
        }

        public static int Mod11Peso2a9(string seq)
        {
            int digito;
            int resto;
            int soma = 0;
            int peso = 2;
            int basecalculo = 9;
            string n;

            for (int i = seq.Length; i > 0; i--)
            {
                n = Common.Mid(seq, i, 1);

                soma = soma + (Convert.ToInt32(n) * peso);

                if (peso < basecalculo)
                    peso = peso + 1;
                else
                    peso = 2;
            }

            resto = ((soma * 10) % 11);

            if (resto == 0 || resto == 1 || resto == 10)
                digito = 1;
            else
                digito = resto;

            return digito;

        }

        public static int Mod11(string seq, int b)
        {
            int digito;
            int soma = 0;
            int peso = 2;


            for (int i = seq.Length; i > 0; i--)
            {
                soma = soma + (Convert.ToInt32(Common.Mid(seq, i, 1)) * peso);
                if (peso == b)
                    peso = 2;
                else
                    peso = peso + 1;
            }

            digito = 11 - (soma % 11);


            if ((digito > 9) || (digito == 0) || (digito == 1))
                digito = 1;

            return digito;
        }

        public static int Mod11Base9(string seq)
        {
            int digito, soma = 0, peso = 2, baseCalculo = 9;


            for (int i = seq.Length - 1; i >= 0; i--)
            {
                string aux = Convert.ToString(seq[i]);
                soma += (Convert.ToInt32(aux) * peso);
                if (peso >= baseCalculo)
                    peso = 2;
                else
                    peso = peso + 1;
            }

            if (soma < 11)
            {
                digito = 11 - soma;
                return digito;
            }
            else
            {
                digito = 11 - (soma % 11);
                if ((digito > 9) || (digito == 0))
                    digito = 0;

                return digito;
            }
        }

        internal static string Mod11Base7Bradesco(string value)
        {
            #region Trecho do manual DVMD11.doc
            /* 
            Posição 71 a 81: Nosso Número - poderá ser gerado a partir de 00000000001,
            00000000002 etc - 11 posições, devendo ser atribuído numero diferenciado para
            identificação de cada documento na Cobrança Bradesco.
            • posição 82 a 82: Dígito de auto-conferência do Nosso Número - 1 posição
            Obs.: Para o cálculo do dígito, será necessário acrescentar o número da
            carteira à esquerda antes do Nosso Número, e aplicar o módulo 11, com base
            7.
            Exemplo
            a) efetuar a multiplicação:
            CARTEIRA    NOSSO NÚMERO
               |              |
            --------    ------------     
               19       00000000002
                MULTIPLICAÇÃO
             * INCLUIR RESTANTE DO MANUAL
             *
               

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
            int s = 0, p = 7, b = 2;

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
                d = "P";
            else if (r == 0)
                d = "0";
            else
                d = r.ToString();

            return d;
        }

        public static int Mod11(string seq, int lim, int flag)
        {
            int mult = 0;
            int total = 0;
            int pos = 1;
            //int res = 0;
            int ndig = 0;
            int nresto = 0;
            string num = string.Empty;

            mult = 1 + (seq.Length % (lim - 1));

            if (mult == 1)
                mult = lim;


            while (pos <= seq.Length)
            {
                num = Mid(seq, pos, 1);
                total += Convert.ToInt32(num) * mult;

                mult -= 1;
                if (mult == 1)
                    mult = lim;

                pos += 1;
            }
            nresto = (total % 11);
            if (flag == 1)
                return nresto;
            else
            {   
                if (nresto == 0 || nresto == 1 || nresto == 10)
                    ndig = 1;
                else
                    ndig = (11 - nresto);

                return ndig;
            }
        }

        public static int Mult10Mod11(string seq, int lim, int flag)
        {
            int mult = 0;
            int total = 0;
            int pos = 1;
            int ndig = 0;
            int nresto = 0;
            string num = string.Empty;

            mult = 1 + (seq.Length % (lim - 1));

            if (mult == 1)
                mult = lim;

            while (pos <= seq.Length)
            {
                num = Common.Mid(seq, pos, 1);
                total += Convert.ToInt32(num) * mult;

                mult -= 1;
                if (mult == 1)
                    mult = lim;

                pos += 1;
            }

            nresto = ((total * 10) % 11);

            if (flag == 1)
                return nresto;
            else
            {
                if (nresto == 0 || nresto == 1 || nresto == 10)
                    ndig = 1;
                else
                    ndig = nresto;

                return ndig;
            }
        }

        /// <summary>
        /// Encontra multiplo de 10 igual ou superior a soma e subtrai multiplo da soma devolvendo o resultado
        /// </summary>
        /// <param name="soma"></param>
        /// <returns></returns>
        public static int Multiplo10(int soma)
        {
            //Variaveis
            int result = 0;
            //Encontrando multiplo de 10
            while (result < soma)
            {
                result = result + 10;
            }
            //Subtraindo
            result = result - soma;
            //Retornando
            return result;
        }

        public static string Mod11Base7Bradesco(string seq, int b)
        {
            #region Trecho do manual layout_cobranca_port.pdf do BRADESCO
            /* 
            Para o c�lculo do d�gito, ser� necess�rio acrescentar o n�mero da carteira � esquerda antes do Nosso N�mero, 
            e aplicar o m�dulo 11, com base 7.
            Multiplicar cada algarismo que comp�e o n�mero pelo seu respectivo multiplicador (PESO).
            Os multiplicadores(PESOS) variam de 2 a 7.
            O primeiro d�gito da direita para a esquerda dever� ser multiplicado por 2, o segundo por 3 e assim sucessivamente.
             
              Carteira   Nosso Numero
                ______   _________________________________________
                1    9   0   0   0   0   0   0   0   0   0   0   2
                x    x   x   x   x   x   x   x   x   x   x   x   x
                2    7   6   5   4   3   2   7   6   5   4   3   2
                =    =   =   =   =   =   =   =   =   =   =   =   =
                2 + 63 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 4 = 69

            O total da soma dever� ser dividido por 11: 69 / 11 = 6 tendo como resto = 3
            A diferen�a entre o divisor e o resto, ser� o d�gito de autoconfer�ncia: 11 - 3 = 8 (d�gito de auto-confer�ncia)
            
            Se o resto da divis�o for �1�, desprezar o c�lculo de subtra��o e considerar o d�gito como �P�. 
            Se o resto da divis�o for �0�, desprezar o c�lculo de subtra��o e considerar o d�gito como �0�.
            */
            #endregion

            /* Vari�veis
             * -------------
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int s = 0, p = 2;

            for (int i = seq.Length; i > 0; i--)
            {
                s = s + (Convert.ToInt32(Mid(seq, i, 1)) * p);
                if (p == b)
                    p = 2;
                else
                    p = p + 1;
            }

            int r = (s % 11);

            if (r == 0)
                return "0";
            else if (r == 1)
                return "P";
            else
                return (11 - r).ToString();
        }

        public static int Mod11Base7BRB(string seq)
        {
            int digito;
            int soma = 0;
            int peso = 7;

            for (int i = 0; i < seq.Length; i++)
            {
                soma = soma + (Convert.ToInt32(Mid(seq, i + 1, 1))*peso);

                if (peso == 2) peso = 7;
                else peso--;
            }

            digito = (soma%11);

            if (digito > 0)
            {
                if (digito == 1)
                {
                    /*
                    Se o resto fosse 1 (um), D2 teria de ser recalculado com um novo D1, da
                    seguinte forma:
                    Somaria 1 ao D1. Se o resultado fosse 10, o novo D1 seria 0 (zero).Caso
                    contrário seria o resultado da soma. Em qualquer situação o novo D1 seria mantido
                    na chave.
                     */

                    var D1 = seq.ExtrairValorDaLinha(seq.Length, seq.Length).BoletoBrToInt();
                    D1 += 1;
                    if (D1 == 10) D1 = 0;

                    var newSeq = seq.ExtrairValorDaLinha(1, seq.Length - 1) + D1;
                    digito = Mod11Base7BRB(newSeq);
                }
                else
                    digito = 11 - digito;
            }

            return digito;
        }

        #endregion Mod

        #region Funções para strings

        public static string Mid(string str, int Start)
        {
            try
            {
                if (str == null)
                    return (string)null;
                else
                    return Common.Mid(str, Start, str.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns a string containing a specified number of characters from a string.
        /// 
        /// Exemplo:
        /// ' Creates text string.
        /// Dim TestString As String = "Mid Function Demo"
        /// ' Returns "Mid".
        /// Dim FirstWord As String = Mid(TestString, 1, 3)
        /// ' Returns "Demo".
        /// Dim LastWord As String = Mid(TestString, 14, 4)
        /// ' Returns "Function Demo".
        /// Dim MidWords As String = Mid(TestString, 5)
        /// </summary>
        /// 
        /// <returns>
        /// Returns a string containing a specified number of characters from a string.
        /// </returns>
        /// <param name="str">Required. String expression from which characters are returned.</param><param name="Start">Required. Integer expression. Starting position of the characters to return. If <paramref name="Start"/> is greater than the number of characters in <paramref name="str"/>, the Mid function returns a zero-length string (""). <paramref name="Start"/> is one based.</param><param name="Length">Optional. Integer expression. Number of characters to return. If omitted or if there are fewer than <paramref name="Length"/> characters in the text (including the character at position <paramref name="Start"/>), all characters from the start position to the end of the string are returned.</param><exception cref="T:System.ArgumentException"><paramref name="Start"/> &lt;= 0 or <paramref name="Length"/> &lt; 0.</exception><filterpriority>1</filterpriority>
        public static string Mid(string str, int Start, int Length)
        {
            if (Start <= 0)
                throw new ArgumentException("Start precisa ser maior que 0");
            else if (Length < 0)
            {
                throw new ArgumentException("Length precisa ser maior que 0");
            }
            else
            {
                if (Length == 0 || str == null)
                    return "";
                int length = str.Length;
                if (Start > length)
                    return "";
                if (checked(Start + Length) > length)
                    return str.Substring(checked(Start - 1));
                else
                    return str.Substring(checked(Start - 1), Length);
            }
        }

        public static string Left(string str, int Length)
        {
            if (Length < 0)
            {
                throw new ArgumentException("Informe o Length");
            }
            else
            {
                if (Length == 0 || str == null)
                    return "";
                if (Length >= str.Length)
                    return str;
                else
                    return str.Substring(0, Length);
            }
        }

        public static string Right(string str, int Length)
        {
            if (Length < 0)
            {
                throw new ArgumentException("Informe o Length.");
            }
            else
            {
                if (Length == 0 || str == null)
                    return "";
                int length = str.Length;
                if (Length >= length)
                    return str;
                else
                    return str.Substring(checked(length - Length), Length);
            }
        }
        #endregion

        #region Apoio a geração de boletos/remessas
        internal enum DateInterval
        {
            Second,
            Minute,
            Hour,
            Day,
            Week,
            Month,
            Quarter,
            Year
        }
        internal static long DateDiff(DateInterval interval, System.DateTime startDate, System.DateTime endDate)
        {
            long lngDateDiffValue = 0;
            var TS = new System.TimeSpan(endDate.Ticks - startDate.Ticks);
            switch (interval)
            {
                case DateInterval.Day:
                    lngDateDiffValue = (long)TS.Days;
                    break;
                case DateInterval.Hour:
                    lngDateDiffValue = (long)TS.TotalHours;
                    break;
                case DateInterval.Minute:
                    lngDateDiffValue = (long)TS.TotalMinutes;
                    break;
                case DateInterval.Month:
                    lngDateDiffValue = (long)(TS.Days / 30);
                    break;
                case DateInterval.Quarter:
                    lngDateDiffValue = (long)((TS.Days / 30) / 3);
                    break;
                case DateInterval.Second:
                    lngDateDiffValue = (long)TS.TotalSeconds;
                    break;
                case DateInterval.Week:
                    lngDateDiffValue = (long)(TS.Days / 7);
                    break;
                case DateInterval.Year:
                    lngDateDiffValue = (long)(TS.Days / 365);
                    break;
            }
            return (lngDateDiffValue);
        }

        /// <summary>
        /// Calcula fator de vencimento, com base em uma data informada.
        /// </summary>
        /// <param name="dataParaCalculo">Data Base de Cálculo</param>
        /// <returns>Valor inteiro com o fator de vencimento</returns>
        /// <remarks>
        ///     Wellington(wcarvalho@novatela.com.br) 
        ///     Com base na proposta feita pela CENEGESC de acordo com o comunicado FEBRABAN de n° 082/2012 de 14/06/2012 segue regra para implantação.
        ///     No dia 21/02/2025 o fator vencimento chegará em 9999 assim atigindo o tempo de utilização, para contornar esse problema foi definido com uma nova regra
        ///     de utilizaçao criando um range de uso o range funcionara controlando a emissão dos boletos.
        ///     Exemplo:
        ///         Data Atual: 12/03/2014 = 6000
        ///         Para os boletos vencidos, anterior a data atual é de 3000 fatores cerca de =/- 8 anos. Os boletos que forem gerados acima dos 3000 não serão aceitos pelas instituições financeiras.
        ///         Para os boletos a vencer, posterior a data atual é de 5500 fatores cerca de +/- 15 anos. Os boletos que forem gerados acima dos 5500 não serão aceitos pelas instituições financeiras.
        ///     Quando o fator de vencimento atingir 9999 ele retorna para 1000
        ///     Exemplo:
        ///         21/02/2025 = 9999
        ///         22/02/2025 = 1000
        ///         23/02/2025 = 1001
        ///         ...
        ///         05/03/2025 = 1011
        /// </remarks>
        public static long FatorVencimento(DateTime dataParaCalculo)
        {
            var dateBase = new DateTime(1997, 10, 7, 0, 0, 0);

            //Verifica se a data esta dentro do range utilizavel
            var dataAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            long rangeUtilizavel = DateDiff(DateInterval.Day, dataAtual, dataParaCalculo);

            if (rangeUtilizavel > 5500 || rangeUtilizavel < -3000)
                throw new Exception("Data do vencimento fora do range de utilização proposto pela CENEGESC. Comunicado FEBRABAN de n° 082/2012 de 14/06/2012");

            while (dataParaCalculo > dateBase.AddDays(9999))
                dateBase = dataParaCalculo.AddDays(-(((DateDiff(DateInterval.Day, dateBase, dataParaCalculo) - 9999) - 1) + 1000));

            return DateDiff(DateInterval.Day, dateBase, dataParaCalculo);
        }
        #endregion

        #region Funções de uso geral

        public static string CompletarCadeia(bool completarDireita, bool completarEsquerda, string texto, char caractere, int tamanho)
        {
            string resultado = string.Empty;
            int i;
            bool conversao = Int32.TryParse(texto, out i);

            if (completarDireita == true)
                resultado = texto.ToUpper().PadRight(tamanho, caractere);
            if (completarEsquerda == true)
                resultado = texto.PadLeft(tamanho, caractere);
            if (completarDireita && completarEsquerda == false)
                if (true == conversao)
                resultado = texto.PadLeft(tamanho, caractere);
                else
                    resultado = texto.ToUpper().PadRight(tamanho, caractere);

            return resultado;
        }

        public static string CompletarCadeiaADireita(string texto, char caractere,  int tamanho)
        {
            return null;
        }
        public static string CompletarCadeiaAEsquerda(string texto, string caractere, int tamanho)
        {
            return null;
        }

        #endregion
    }
}
