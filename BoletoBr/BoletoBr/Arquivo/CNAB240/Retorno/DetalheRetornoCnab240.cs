using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo;
using BoletoBr.Dominio;

namespace BoletoBr
{
    public class DetalheRetornoCnab240
    {
        #region Construtores

        public DetalheRetornoCnab240()
        {
            HeaderArquivo = new HeaderDeArquivoCnab240();
            SegmentoW = new DetalheSegmentoWRetornoCnab240();
            SegmentoU = new DetalheSegmentoURetornoCnab240();
            SegmentoT = new DetalheSegmentoTRetornoCnab240();
        }

        public DetalheRetornoCnab240(DetalheSegmentoTRetornoCnab240 segmentoT)
        {
            SegmentoT = segmentoT;
        }

        public DetalheRetornoCnab240(DetalheSegmentoURetornoCnab240 segmentoU)
        {
            SegmentoU = segmentoU;
        }

        public DetalheRetornoCnab240(DetalheSegmentoWRetornoCnab240 segmentoW)
        {
            SegmentoW = segmentoW;
        }

        public DetalheRetornoCnab240(DetalheSegmentoERetornoCnab240 segmentoE)
        {
            SegmentoE = segmentoE;
        }

        public DetalheRetornoCnab240(DetalheSegmentoTRetornoCnab240 segmentoT, DetalheSegmentoURetornoCnab240 segmentoU)
		{
            SegmentoT = segmentoT;
            SegmentoU = segmentoU;
        }

        #endregion

        #region Propriedades

        public DetalheSegmentoTRetornoCnab240 SegmentoT { get; set; }

        public DetalheSegmentoURetornoCnab240 SegmentoU { get; set; }

        public DetalheSegmentoWRetornoCnab240 SegmentoW { get; set; }

        public DetalheSegmentoERetornoCnab240 SegmentoE { get; set; }

        public HeaderDeArquivoCnab240 HeaderArquivo { get; set; }

        #endregion
    }
}
