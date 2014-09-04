using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;
using BoletoBr.Interfaces;

namespace BoletoBr
{
    public class RemessaCnab240
    {
        private readonly IBanco _bancoRemessa;

        public RemessaCnab240(IBanco bancoRemessa)
        {
            _bancoRemessa = bancoRemessa;
        }

        public void AdicionarBoleto(Boleto boletoAdicionar)
        {
            if (boletoAdicionar.BancoBoleto == null)
                throw new ValidacaoBoletoException("Boleto " + boletoAdicionar.NossoNumeroFormatado +
                                                   " não é válido para adição na remessa. Falta informar o banco do boleto.");

            if (boletoAdicionar.CarteiraCobranca == null)
                throw new ValidacaoBoletoException("Boleto " + boletoAdicionar.NossoNumeroFormatado +
                                                   " não é válido para adição na remessa. Falta informar a carteira de cobrança.");
        }

        public void AdicionarBoletos(List<Boleto> boletosAdicionar)
        {
            if (boletosAdicionar == null)
                return;

            boletosAdicionar.ForEach(AdicionarBoleto);
        }
        public HeaderRemessaCnab240 Header { get; set; }
        public List<DetalheRemessaCnab240> RegistrosDetalhe { get; set; }
        public TrailerRemessaCnab240 Trailer { get; set; }
    }
}
