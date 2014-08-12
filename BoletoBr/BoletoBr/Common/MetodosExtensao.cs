using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public static class MetodosExtensao
    {
        /// <summary>
        /// Aplica máscara a um texto
        /// </summary>
        /// <param name="textoformatar">Texto a ser formatado</param>
        /// <param name="mascara">Máscara utilizando cerquilha.</param>
        /// <returns>Texto com máscara aplicada</returns>
        public static string BoletoBrSetMascara(this string textoformatar, string mascara)
        {
            string novoValor = string.Empty;
            int posicao = 0;
            for (int i = 0; mascara.Length > i; i++)
            {
                if (mascara[i] == '#')
                {
                    if (textoformatar.Length > posicao)
                    {
                        novoValor = novoValor + textoformatar[posicao];
                        posicao++;
                    }
                    else
                        break;
                }
                else
                {
                    if (textoformatar.Length > posicao)
                        novoValor = novoValor + mascara[i];
                    else
                        break;
                }
            }
            return novoValor;
        }

        public static bool BoletoBrSomenteNumeros(this string texto)
        {
            try
            {
                for (int i = 0; i < texto.Length; i++)
                {
                    if (char.IsNumber(texto, i) == false) // se não é string
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao verificar se o texto: " + texto + " contém somente números.", ex);
            }
        }

        public static string BoletoBrRemoveAcentos(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();

            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static Nullable<T> BoletoBrToNullable<T>(this string s) where T : struct
        {
            Nullable<T> result = new Nullable<T>();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(s);
                }
            }
            catch { }
            return result;
        }

        public static long BoletoBrToLong(this string s)
        {
            long result = new long();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(long));
                    result = (long)conv.ConvertFrom(s);
                }
                else result = 0;
            }
            catch { }
            return result;
        }

        public static string BoletoBrToStringSafe(this object obj)
        {
            if (obj == null)
                return String.Empty;

            var resultado = obj.ToString();

            // remove caracteres de escapebind
            resultado = resultado.Replace("\r", "");
            resultado = resultado.Replace("\n", "");

            return resultado;
        }

        public static int BoletoBrToInt(this string s)
        {
            if (String.IsNullOrEmpty(s))
                return 0;

            int valorRetornar = 0;
            int.TryParse(s, out valorRetornar);
            return valorRetornar;
        }

        public static int? BoletoBrToNullableInt(this string s)
        {
            if (String.IsNullOrEmpty(s))
                return new int?();

            int valorretornar = 0;
            int.TryParse(s, out valorretornar);
            return valorretornar;
        }

        public static decimal BoletoBrToDecimal(this string s)
        {
            decimal result = new decimal();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(decimal));
                    result = (decimal)conv.ConvertFrom(s);
                }
                else result = 0;
            }
            catch { }
            return result;
        }

        public static double BoletoBrToDouble(this string s)
        {
            double result = new double();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(double));
                    result = (double)conv.ConvertFrom(s);
                }
                else result = 0;
            }
            catch { }
            return result;
        }

        public static DateTime BoletoBrToDateTime(this string s)
        {
            DateTime result = new DateTime();

            if (String.IsNullOrEmpty(s))
                return result;

            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(DateTime));
                    result = (DateTime)conv.ConvertFrom(s);
                }
                else result = new DateTime();
            }
            catch { }
            return result;
        }

        public static bool BoletoBrToBool(this string s)
        {
            if (s == null)
                return false;

            if (s == "true")
                return true;

            if (s.ToLower() == "sim")
                return true;

            return false;
        }

        public static bool? BoletoBrToBoolNullable(this string s)
        {
            if (String.IsNullOrEmpty(s))
                return new bool?();

            return s.BoletoBrToBool();
        }

        public static string BoletoBrToBoolString(this bool b)
        {
            if (b == true)
                return "Sim";
            else
                return "Não";
        }

        /// <summary>
        /// Retorna uma instância do objeto nula
        /// </summary>
        /// <param name="obj">Objeto alvo do método de extensão</param>
        /// <returns>Retorna própria instância do objeto ou uma nova instância com dados default</returns>
        public static T BoletoBrToBind<T>(this T obj) where T : class, new()
        {
            if (obj != null)
                return obj;

            return new T();
        }

        /// <summary>
        /// Evita que uma string seja repassada como NULA
        /// </summary>
        /// <param name="texto"></param>
        /// <returns>String.Empty caso o texto seja nulo</returns>
        public static string BoletoBrToBind(this string texto)
        {
            if (texto == null)
                return "";
            return texto;
        }


        /// <summary>
        /// Retorna uma instância do objeto nula
        /// </summary>
        /// <param name="obj">Objeto alvo do método de extensão</param>
        /// <returns>Retorna própria instância do objeto ou uma nova instância com dados default</returns>
        public static IList<T> BoletoBrToBind<T>(this IList<T> obj) where T : class, new()
        {
            if (obj != null)
                return obj;

            return new List<T>();
        }

        /// <summary>
        /// Método de Extensão: Remove um caractere de uma string.
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="caractereRemover">Caractere a se remover</param>
        /// <returns>Retorna a String</returns>
        public static string BoletoBrDeletarCaractere(this string s, string caractereRemover)
        {
            var stringRemover = s;

            while (stringRemover.IndexOf(caractereRemover) > 0)
            {
                var indice = s.IndexOf(caractereRemover);
                stringRemover = stringRemover.Remove(indice, 1);
            }

            return stringRemover;
        }

        public static string BoletoBrCopiarTextoAteCaractere(this string s, string caractereEncontrar)
        {
            var stringBase = s;

            var indiceCaractere = stringBase.IndexOf(caractereEncontrar);

            if (indiceCaractere <= 0)
                return String.Empty;

            var texto = s.Substring(0, indiceCaractere);

            return texto;
        }

        /// <summary>
        /// Remove todos os caracteres encontrados na lista de caracteres
        /// </summary>
        /// <param name="s"></param>
        /// <param name="caracteres"></param>
        /// <returns></returns>
        public static string BoletoBrDeletarCaracteres(this string s, string[] caracteresRemover)
        {
            var stringVerificar = s;

            foreach (var caractere in caracteresRemover)
            {
                while (stringVerificar.IndexOf(caractere) > 0)
                {
                    var indice = stringVerificar.IndexOf(caractere);
                    stringVerificar = stringVerificar.Remove(indice, 1);
                }
            }

            return stringVerificar;
        }

        public static string ExtrairValorDaLinha(this string conteudoLinha, int de, int ate)
        {
            int inicio = de - 1;
            return conteudoLinha.Substring(inicio, ate - inicio);
        }

        public static DateTime? ToDateTimeFromDdMmAa(this string s)
        {
            DateTime result;

            if (String.IsNullOrEmpty(s))
                return null;

            if (DateTime.TryParseExact(s, "ddMMyy", null, DateTimeStyles.None, out result))
            {
                return result;
            }
            return null;
        }

        public static DateTime? ToDateTimeFromDdMmAaaa(this string s)
        {
            DateTime result;

            if (String.IsNullOrEmpty(s))
                return null;

            if (DateTime.TryParseExact(s, "ddMMyyyy", null, DateTimeStyles.None, out result))
            {
                return result;
            }
            return null;
        }
    }
}
