using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Interfaces;

namespace BoletoBr.Dominio
{
    public class EspecieDocumento : IEspecieDocumento
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public string Sigla { get; set; }

        public EspecieDocumento(int codigo)
        {
            this.Codigo = codigo;
        }

        public EspecieDocumento(int codigo, string descricao)
        {
            this.Codigo = codigo;
            this.Descricao = descricao;
        }

        public EspecieDocumento(int codigo, string descricao, string sigla)
        {
            this.Codigo = codigo;
            this.Descricao = descricao;
            this.Sigla = sigla;
        }

        public static string ValidaSigla(IEspecieDocumento especie)
        {
            try
            {
                return especie.Sigla;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ValidaCodigo(IEspecieDocumento especie)
        {
            try
            {
                return especie.Codigo.ToString();
            }
            catch
            {
                return "0";
            }
        }
    }
}
