using System.Collections.Generic;

namespace BoletoBr.Arquivo.Generico.Retorno
{
    public class RetornoGenerico
    {
        public RetornoGenerico(RetornoCnab240 retornoCnab240)
        {
            /* Transformar de CNAB240 para formato genérico */

        }

        public RetornoGenerico(RetornoCnab400 retornoCnab400)
        {
            /* Transformar de CNAB400 para formato genérico */

        }
        public RetornoHeaderGenerico Header { get; set; }
        public List<RetornoDetalheGenerico> RegistrosDetalhe { get; set; } 
        public RetornoTrailerGenerico Trailer { get; set; }
        public RetornoCnab240 RetornoCnab240Especifico { get; set; }
        public RetornoCnab400 RetornoCnab400Especifico { get; set; }
    }
}
