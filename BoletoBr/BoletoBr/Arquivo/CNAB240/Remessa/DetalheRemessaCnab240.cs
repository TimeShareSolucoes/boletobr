
using System.Collections.Generic;

namespace BoletoBr.Arquivo.CNAB240.Remessa
{
    public class DetalheRemessaCnab240
    {
        #region Construtores

        public DetalheRemessaCnab240(Boleto boleto, int numeroRegistroNoLote)
        {
            SegmentoP = new DetalheSegmentoPRemessaCnab240(boleto, numeroRegistroNoLote);
            SegmentoQ = new DetalheSegmentoQRemessaCnab240(boleto, numeroRegistroNoLote);
            SegmentoR = new DetalheSegmentoRRemessaCnab240(boleto, numeroRegistroNoLote);
            SegmentoS = new DetalheSegmentoSRemessaCnab240();
            SegmentoY = new DetalheSegmentoYRemessaCnab240();
            SegmentoY08 = new DetalheSegmentoY08RemessaCnab240();
        }

        public DetalheRemessaCnab240(DetalheSegmentoPRemessaCnab240 segmentoP)
        {
            SegmentoP = segmentoP;
        }

        public DetalheRemessaCnab240(DetalheSegmentoQRemessaCnab240 segmentoQ)
        {
            SegmentoQ = segmentoQ;
        }

        public DetalheRemessaCnab240(DetalheSegmentoRRemessaCnab240 segmentoR)
        {
            SegmentoR = segmentoR;
        }

        public DetalheRemessaCnab240(DetalheSegmentoSRemessaCnab240 segmentoS)
        {
            SegmentoS = segmentoS;
        }

        public DetalheRemessaCnab240(DetalheSegmentoYRemessaCnab240 segmentoY)
        {
            SegmentoY = segmentoY;
        }
        public DetalheRemessaCnab240(DetalheSegmentoY08RemessaCnab240 segmentoY08)
        {
            SegmentoY08 = segmentoY08;
        }

        public DetalheRemessaCnab240(DetalheSegmentoPRemessaCnab240 segmentoP, DetalheSegmentoQRemessaCnab240 segmentoQ)
        {
            SegmentoP = segmentoP;
            SegmentoQ = segmentoQ;
        }

        #endregion

        #region Propriedades

        public DetalheSegmentoPRemessaCnab240 SegmentoP { get; set; }

        public DetalheSegmentoQRemessaCnab240 SegmentoQ { get; set; }

        public DetalheSegmentoRRemessaCnab240 SegmentoR { get; set; }

        public DetalheSegmentoSRemessaCnab240 SegmentoS { get; set; }

        public DetalheSegmentoYRemessaCnab240 SegmentoY { get; set; }

        public DetalheSegmentoY08RemessaCnab240 SegmentoY08 { get; set; }

        #endregion
    }
}
