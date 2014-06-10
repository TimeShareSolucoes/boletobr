using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr
{
    public class RemessaCnab400
    {
        private readonly IBanco _bancoRemessa;
        private List<Boleto> _listaBoletos;

        public RemessaCnab400(IBanco bancoRemessa)
        {
            _bancoRemessa = bancoRemessa;
        }

        public void AdicionarBoleto(Boleto boletoAdicionar)
        {
            if (boletoAdicionar.BancoBoleto == null)
                throw new ValidacaoBoletoException("Boleto " + boletoAdicionar.NossoNumero +
                                                   " não é válido para adição na remessa. Falta informar o banco do boleto.");

            if (boletoAdicionar.CarteiraCobranca == null)
                throw new ValidacaoBoletoException("Boleto " + boletoAdicionar.NossoNumero +
                                                   " não é válido para adição na remessa. Falta informar a carteira de cobrança.");
        }

        public void AdicionarBoletos(List<Boleto> boletosAdicionar)
        {
            if (boletosAdicionar == null)
                return;

            boletosAdicionar.ForEach(AdicionarBoleto);
        }

        public HeaderRemessaCnab400 Header { get; set; }
        public List<DetalheRemessaCnab400> RegistrosDetalhe { get; set; }
        public TrailerRemessaCnab400 Trailer { get; set; }
    }
}
