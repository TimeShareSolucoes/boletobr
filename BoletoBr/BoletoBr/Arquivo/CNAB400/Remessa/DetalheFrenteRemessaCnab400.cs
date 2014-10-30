using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Arquivo.CNAB400.Remessa
{
    public class DetalheFrenteRemessaCnab400
    {
        public DetalheFrenteRemessaCnab400(Boleto boleto, string codigoFlash, int nroUltimaLinha, int numeroSequencialRegistro)
        {
            this.CodigoFlash = codigoFlash;
            this.NroLinha1 = nroUltimaLinha;
            this.ConteudoLinha1 = boleto.InstrucoesDoBoleto.GetRange(0, 1).ToString();
            this.NroLinha2 = nroUltimaLinha + 1;
            this.ConteudoLinha2 = boleto.InstrucoesDoBoleto.GetRange(1, 1).ToString();
            this.NroLinha3 = nroUltimaLinha + 2;
            this.ConteudoLinha3 = boleto.InstrucoesDoBoleto.GetRange(2, 1).ToString();
            this.NumeroSequencialRegistro = numeroSequencialRegistro;
        }
        public int TipoRegistro { get; set; }
        public string CodigoFlash { get; set; }
        public int NroLinha1 { get; set; }
        public string ConteudoLinha1 { get; set; }
        public int NroLinha2 { get; set; }
        public string ConteudoLinha2 { get; set; }
        public int NroLinha3 { get; set; }
        public string ConteudoLinha3 { get; set; }
        public string DestinoBoleto { get; set; }
        public int NumeroSequencialRegistro { get; set; }
    }
}
