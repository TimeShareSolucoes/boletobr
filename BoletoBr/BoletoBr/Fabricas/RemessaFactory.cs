using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;

namespace BoletoBr.Fabricas
{
    /// <summary>
    /// Auxilia na criação de objetos de remessa
    /// utilizaSegmentoR => utilizado para indformar ao banco instruções de multa
    /// </summary>
    public class RemessaFactory
    {
        public RemessaCnab240 GerarRemessa(HeaderRemessaCnab240 header, HeaderLoteRemessaCnab240 headerLote,
            List<Boleto> boletos, TrailerLoteRemessaCnab240 trailerLote,
            TrailerRemessaCnab240 trailer, bool utilizaSegmentoR = false)
        {
            var objReturn = new RemessaCnab240();

            objReturn.Header = header;
            objReturn.Lotes = new List<LoteRemessaCnab240>();

            var ultimoLoteAdicionado = objReturn.AdicionarLote(headerLote, trailerLote);

            // Usado para identificar com número único e sequencial cada boleto (registro) dentro do lote.
            var contador = 1;
            var reg1 = 1;
            var reg2 = 2;
            var reg3 = utilizaSegmentoR ? 3 : 0;

            foreach (var boletoAddRemessa in boletos)
            {
                objReturn.AdicionarBoletoAoLote(ultimoLoteAdicionado, boletoAddRemessa, contador, reg1, reg2, reg3);
                contador++;
                reg1 += utilizaSegmentoR ? 3 :2;
                reg2 += utilizaSegmentoR ? 3 : 2;
                reg3 += utilizaSegmentoR ? 3 : 0;
            }

            objReturn.Trailer = trailer;

            return objReturn;
        }

        public RemessaCnab400 GerarRemessa(HeaderRemessaCnab400 header, List<Boleto> boletos,
            List<DetalheRemessaCnab400> detalhes, TrailerRemessaCnab400 trailer)
        {
            //var contadorSequencialRegistro = 1;
            var objReturn = new RemessaCnab400();
            objReturn.RegistrosDetalhe = new List<DetalheRemessaCnab400>();

            objReturn.Header = header;

            foreach (var detalhe in detalhes)
            {
                objReturn.RegistrosDetalhe.Add(detalhe);
                //objReturn.AdicionarBoleto(boletoAddRemessa, contadorSequencialRegistro);
                //contadorSequencialRegistro++;
            }

            objReturn.Trailer = trailer;

            return objReturn;
        }
    }
}
