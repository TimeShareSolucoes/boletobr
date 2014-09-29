using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;

namespace BoletoBr.Arquivo.Generico.Remessa
{
    public class RemessaGenerica
    {
        public void Inicializa()
        {
            Header = new RemessaHeaderGenerica();
            RegistrosDetalhe = new List<RemessaDetalheGenerica>();
            Trailer = new RemessaTrailerGenerica();
        }
        public RemessaGenerica(RemessaCnab240 remessaCnab240)
        {
            Inicializa();
            RemessaCnab240Especifico = remessaCnab240;
            /* Transformar de CNAB240 para formato genérico */

            foreach (var loteAtual in remessaCnab240.Lotes)
            {
                foreach (var d in loteAtual.RegistrosDetalheSegmentos)
                {
                    var detalheGenericoAdd = new RemessaDetalheGenerica
                    {
                        NossoNumero = d.SegmentoP.NossoNumero,
                        Carteira = d.SegmentoP.Carteira,
                        NumeroDocumento = d.SegmentoP.NumeroDocumento
                    };

                    // Segmento P
                    var valorDoc = d.SegmentoP.ValorBoleto;
                    detalheGenericoAdd.ValorDocumento = valorDoc;

                    #region Valores no detalhe

                    var valorJuros = d.SegmentoP.ValorJuros;
                    var valorDesc = d.SegmentoP.ValorDesconto1 + d.SegmentoP.ValorAbatimento;
                    //var valorPago = d.SegmentoU.ValorPagoPeloSacado;

                    detalheGenericoAdd.ValorJuros = valorJuros;
                    detalheGenericoAdd.ValorDescontos = valorDesc;

                    #endregion
                    
                    //detalheGenericoAdd.ValorIof = d.SegmentoU.ValorIofRecolhido / 100;
                    //detalheGenericoAdd.ValorOutrasDespesas = d.SegmentoU.ValorOutrasDespesas / 100;
                    //detalheGenericoAdd.ValorOutrosCreditos = d.SegmentoU.ValorOutrosCreditos / 100;
                    //detalheGenericoAdd.CodigoOcorrencia = d.SegmentoU.CodigoOcorrenciaPagador;
                    //detalheGenericoAdd.DataOcorrencia = d.SegmentoU.DataOcorrenciaPagador;
                    //detalheGenericoAdd.ValorOcorrencia = d.SegmentoU.ValorOcorrenciaPagador / 100;
                    //detalheGenericoAdd.DataDebitoTarifaCustas = Convert.ToDateTime(d.SegmentoU.DataDebitoTarifa.ToString());
                    RegistrosDetalhe.Add(detalheGenericoAdd);
                }
            }
            Trailer.QtdRegistrosArquivo = remessaCnab240.Trailer.QtdRegistrosArquivo.ToString();
        }

        public RemessaGenerica(RemessaCnab400 remessaCnab400)
        {
            Inicializa();
            RemessaCnab400Especifico = remessaCnab400;
            /* Transformar de CNAB400 para formato genérico */
            Header.CodigoDoBanco = remessaCnab400.Header.CodigoBanco.ToString();
            Header.CodigoAgencia = remessaCnab400.Header.Agencia.ToString();
            Header.NomeEmpresa = remessaCnab400.Header.NomeDaEmpresa;
            Header.NomeDoBanco = remessaCnab400.Header.NomeDoBanco;

            foreach (var registroAtual in remessaCnab400.RegistrosDetalhe)
            {
                var detalheGenericoAdd = new RemessaDetalheGenerica
                {
                    NossoNumero = registroAtual.NossoNumero,
                    Carteira = registroAtual.Carteira,
                    ValorDocumento = registroAtual.ValorTitulo/100,
                    DataVencimento = Convert.ToDateTime(registroAtual.Vencimento.ToString("ddMMyy")),
                    InscricaoSacado = registroAtual.InscricaoPagador,
                    NomeSacado = registroAtual.NomePagador,
                    CidadeSacado = registroAtual.CidadePagador,
                    UfSacado = registroAtual.UfPagador,
                    CodigoOcorrencia = registroAtual.CodigoOcorrencia
                };

                RegistrosDetalhe.Add(detalheGenericoAdd);
            }
        }
        public RemessaHeaderGenerica Header { get; set; }
        public List<RemessaDetalheGenerica> RegistrosDetalhe { get; set; } 
        public RemessaTrailerGenerica Trailer { get; set; }
        public RemessaCnab240 RemessaCnab240Especifico { get; set; }
        public RemessaCnab400 RemessaCnab400Especifico { get; set; }
    }
}
