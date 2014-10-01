using System.Collections.Generic;

namespace BoletoBr.Arquivo.CNAB240.Remessa
{
    public class RemessaCnab240
    {
        public RemessaCnab240()
        {
            Lotes = new List<LoteRemessaCnab240>();
        }
        public HeaderRemessaCnab240 Header { get; set; }
        public List<LoteRemessaCnab240> Lotes { get; set; }
        public TrailerRemessaCnab240 Trailer { get; set; }

        public LoteRemessaCnab240 AdicionarLote(HeaderLoteRemessaCnab240 headerLote, TrailerLoteRemessaCnab240 trailerLote)
        {
            var loteAdd = new LoteRemessaCnab240();
            loteAdd.HeaderLote = headerLote;
            loteAdd.TrailerLote = trailerLote;

            this.Lotes.Add(loteAdd);
            return loteAdd;
        }

        public DetalheRemessaCnab240 AdicionarBoletoAoLote(LoteRemessaCnab240 loteContainer, Boleto boletoAdicionar)
        {
            var detalheRemessaAdd = new DetalheRemessaCnab240();
            detalheRemessaAdd.SegmentoP = new DetalheSegmentoPRemessaCnab240(boletoAdicionar);
            detalheRemessaAdd.SegmentoQ = new DetalheSegmentoQRemessaCnab240(boletoAdicionar);

            loteContainer.RegistrosDetalheSegmentos.Add(detalheRemessaAdd);

            return detalheRemessaAdd;
        }
    }
}
