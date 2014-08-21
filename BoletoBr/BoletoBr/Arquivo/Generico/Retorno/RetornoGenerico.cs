using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace BoletoBr.Arquivo.Generico.Retorno
{
    public class RetornoGenerico
    {
        public void Inicializa()
        {
            this.Header = new RetornoHeaderGenerico();
            this.RegistrosDetalhe = new List<RetornoDetalheGenerico>();
            this.Trailer = new RetornoTrailerGenerico();
        }
        public RetornoGenerico(RetornoCnab240 retornoCnab240)
        {
            Inicializa();
            this.RetornoCnab240Especifico = retornoCnab240;
            /* Transformar de CNAB240 para formato genérico */
                
            foreach (var loteAtual in retornoCnab240.Lotes)
            {
                foreach (var d in loteAtual.RegistrosDetalheSegmentos)
                {
                    var detalheGenericoAdd = new RetornoDetalheGenerico();

                    // Segmento T
                    detalheGenericoAdd.NossoNumero = d.SegmentoT.NossoNumero;
                    detalheGenericoAdd.Carteira = d.SegmentoT.CodigoCarteira.ToString();
                    detalheGenericoAdd.NumeroDocumento = d.SegmentoT.NumeroDocumento.ToString();
                    var valorDoc = d.SegmentoT.ValorTitulo;
                    detalheGenericoAdd.ValorDocumento = valorDoc;
                    //detalheGenericoAdd.ValorDocumento = Math.Round(d.SegmentoT.ValorTitulo, 2);
                    //detalheGenericoAdd.ValorTarifaCustas = d.SegmentoT.ValorTarifas / 100;
                    //detalheGenericoAdd.CodigoMovimento = d.SegmentoT.CodigoMovimento.ToString();
                    //detalheGenericoAdd.CodigoOcorrencia = d.SegmentoT.MotivoOcorrencia.ToString();

                    // Segmento U
                    detalheGenericoAdd.DataCredito = d.SegmentoU.DataCredito;
                    //detalheGenericoAdd.DataLiquidacao = Convert.ToDateTime(d.SegmentoU.DataLiquidacao.ToString());
                    //detalheGenericoAdd.DataCredito = Convert.ToDateTime(d.SegmentoU.DataCredito.ToString());

                    #region Valores no detalhe

                    var valorAcres = d.SegmentoU.JurosMultaEncargos;
                    var valorDesc = d.SegmentoU.ValorDescontoConcedido + d.SegmentoU.ValorAbatimentoConcedido;
                    var valorPago = d.SegmentoU.ValorPagoPeloSacado;

                    detalheGenericoAdd.ValorAcrescimos = valorAcres;
                    detalheGenericoAdd.ValorDesconto = valorDesc;
                    detalheGenericoAdd.ValorPago = valorPago;
                    //detalheGenericoAdd.ValorLiquido = Math.Round(d.SegmentoU.ValorLiquidoASerCreditado, 2);

                    #endregion
                    
                    //detalheGenericoAdd.ValorIof = d.SegmentoU.ValorIofRecolhido / 100;
                    //detalheGenericoAdd.ValorOutrasDespesas = d.SegmentoU.ValorOutrasDespesas / 100;
                    //detalheGenericoAdd.ValorOutrosCreditos = d.SegmentoU.ValorOutrosCreditos / 100;
                    //detalheGenericoAdd.CodigoOcorrencia = d.SegmentoU.CodigoOcorrenciaPagador;
                    //detalheGenericoAdd.DataOcorrencia = d.SegmentoU.DataOcorrenciaPagador;
                    //detalheGenericoAdd.ValorOcorrencia = d.SegmentoU.ValorOcorrenciaPagador / 100;
                    //detalheGenericoAdd.DataDebitoTarifaCustas = Convert.ToDateTime(d.SegmentoU.DataDebitoTarifa.ToString());
                    this.RegistrosDetalhe.Add(detalheGenericoAdd);
                }
            }
            this.Trailer.QtdRegistrosArquivo = retornoCnab240.Trailer.QtdRegistrosArquivo.ToString();
        }

        public RetornoGenerico(RetornoCnab400 retornoCnab400)
        {
            Inicializa();
            this.RetornoCnab400Especifico = retornoCnab400;
            /* Transformar de CNAB400 para formato genérico */
            this.Header.CodigoDoBanco = retornoCnab400.Header.CodigoDoBanco.ToString();
            this.Header.Convenio = retornoCnab400.Header.NumeroConvenio.ToString();
            this.Header.CodigoAgencia = retornoCnab400.Header.CodigoAgenciaCedente.ToString();
            this.Header.DvAgencia = retornoCnab400.Header.DvAgenciaCedente.ToString();
            this.Header.NumeroConta = retornoCnab400.Header.ContaCorrente.ToString();
            this.Header.DvConta = retornoCnab400.Header.DvContaCorrente.ToString();
            this.Header.NomeEmpresa = retornoCnab400.Header.NomeDoBeneficiario.ToString();
            this.Header.NomeDoBanco = retornoCnab400.Header.NomeDoBanco.ToString();

            foreach (var registroAtual in retornoCnab400.RegistrosDetalhe)
            {
                var detalheGenericoAdd = new RetornoDetalheGenerico();
                detalheGenericoAdd.NossoNumero = registroAtual.NossoNumero;
                detalheGenericoAdd.TipoCobranca = registroAtual.TipoCobranca.ToString();
                detalheGenericoAdd.Carteira = registroAtual.CodigoCarteira.ToString();
                detalheGenericoAdd.PercentualDesconto = registroAtual.TaxaDesconto/100;
                detalheGenericoAdd.PercentualIof = registroAtual.TaxaIof/100;
                detalheGenericoAdd.Especie = registroAtual.Especie;
                detalheGenericoAdd.DataCredito = Convert.ToDateTime(registroAtual.DataDeCredito.ToString("d"));
                detalheGenericoAdd.NumeroDocumento = registroAtual.NumeroDocumento.ToString();
                detalheGenericoAdd.ValorDocumento = registroAtual.ValorDoTituloParcela/100;
                detalheGenericoAdd.ValorTarifaCustas = registroAtual.ValorTarifa/100;
                detalheGenericoAdd.ValorOutrasDespesas = registroAtual.ValorOutrasDespesas/100;
                detalheGenericoAdd.ValorJurosDesconto = registroAtual.ValorJurosDesconto/100;
                detalheGenericoAdd.ValorIofDesconto = registroAtual.ValorIofDesconto/100;
                detalheGenericoAdd.ValorAbatimento = registroAtual.ValorAbatimento/100;
                detalheGenericoAdd.ValorDesconto = (registroAtual.ValorDoTituloParcela/100) -
                                                   (registroAtual.ValorLiquidoRecebido);
                detalheGenericoAdd.ValorRecebido = registroAtual.ValorLiquidoRecebido;
                detalheGenericoAdd.ValorAcrescimos = registroAtual.ValorJurosDeMora/100;
                detalheGenericoAdd.ValorOutrosRecebimentos = registroAtual.ValorOutrosRecebimentos/100;
                detalheGenericoAdd.ValorAbatimentoNaoAproveitadoPeloSacado =
                    registroAtual.ValorAbatimentosNaoAproveitado/100;
                detalheGenericoAdd.ValorLancamento = registroAtual.ValorLancamento/100;
                detalheGenericoAdd.DataLiquidacao = Convert.ToDateTime(registroAtual.DataLiquidacao.ToString("d"));
                detalheGenericoAdd.InscricaoSacado = registroAtual.NumeroInscricaoSacado.ToString();
                detalheGenericoAdd.NomeSacado = registroAtual.NomeSacado;
                detalheGenericoAdd.CodigoMovimento = registroAtual.MotivoCodigoRejeicao.ToString();
                detalheGenericoAdd.CodigoOcorrencia = registroAtual.MotivoCodigoOcorrencia.ToString();

                this.RegistrosDetalhe.Add(detalheGenericoAdd);
            }
        }
        public RetornoHeaderGenerico Header { get; set; }
        public List<RetornoDetalheGenerico> RegistrosDetalhe { get; set; } 
        public RetornoTrailerGenerico Trailer { get; set; }
        public RetornoCnab240 RetornoCnab240Especifico { get; set; }
        public RetornoCnab400 RetornoCnab400Especifico { get; set; }
    }
}
