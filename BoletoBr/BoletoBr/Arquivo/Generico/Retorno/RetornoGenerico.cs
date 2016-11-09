using System;
using System.Collections.Generic;
using System.Globalization;
using BoletoBr.Arquivo.CNAB240.Retorno;
using BoletoBr.Fabricas;

namespace BoletoBr.Arquivo.Generico.Retorno
{
    public class RetornoGenerico
    {
        public void Inicializa()
        {
            Header = new RetornoHeaderGenerico();
            RegistrosDetalhe = new List<RetornoDetalheGenerico>();
            Trailer = new RetornoTrailerGenerico();
        }

        public RetornoGenerico(RetornoCnab240 retornoCnab240)
        {
            Inicializa();
            RetornoCnab240Especifico = retornoCnab240;
            /* Transformar de CNAB240 para formato genérico */

            foreach (var loteAtual in retornoCnab240.Lotes)
            {
                foreach (var d in loteAtual.RegistrosDetalheSegmentos)
                {
                    var detalheGenericoAdd = new RetornoDetalheGenerico
                    {
                        NossoNumero = d.SegmentoT.NossoNumero,
                        Carteira = d.SegmentoT.CodigoCarteira.ToString(CultureInfo.InvariantCulture),
                        NumeroDocumento = d.SegmentoT.NumeroDocumento
                    };

                    // Segmento T
                    var valorDoc = d.SegmentoT.ValorTitulo;
                    detalheGenericoAdd.ValorDocumento = valorDoc;
                    detalheGenericoAdd.DataVencimento = Convert.ToDateTime(d.SegmentoT.DataVencimento);
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
                    //var valorPago = d.SegmentoU.ValorPagoPeloSacado;
                    var valorRecebido = d.SegmentoU.ValorLiquidoASerCreditado;

                    detalheGenericoAdd.ValorAcrescimos = valorAcres;
                    detalheGenericoAdd.ValorDesconto = valorDesc;
                    //detalheGenericoAdd.ValorPago = valorPago;
                    detalheGenericoAdd.ValorRecebido = valorRecebido;

                    #endregion

                    //detalheGenericoAdd.ValorIof = d.SegmentoU.ValorIofRecolhido / 100;
                    //detalheGenericoAdd.ValorOutrasDespesas = d.SegmentoU.ValorOutrasDespesas / 100;
                    //detalheGenericoAdd.ValorOutrosCreditos = d.SegmentoU.ValorOutrosCreditos / 100;
                    //detalheGenericoAdd.CodigoOcorrencia = d.SegmentoU.CodigoOcorrenciaPagador;
                    //detalheGenericoAdd.DataOcorrencia = d.SegmentoU.DataOcorrenciaPagador;
                    //detalheGenericoAdd.ValorOcorrencia = d.SegmentoU.ValorOcorrenciaPagador / 100;
                    //detalheGenericoAdd.DataDebitoTarifaCustas = Convert.ToDateTime(d.SegmentoU.DataDebitoTarifa.ToString());

                    //DATA LIQUIDAÇÃO E DATA OCORRENCIA
                    if (detalheGenericoAdd.Pago && detalheGenericoAdd.DataLiquidacao == DateTime.MinValue)
                        detalheGenericoAdd.DataLiquidacao = d.SegmentoU.DataOcorrencia;

                    RegistrosDetalhe.Add(detalheGenericoAdd);
                }
            }

            Trailer.QtdRegistrosArquivo = retornoCnab240.Trailer.QtdRegistrosArquivo.ToString(CultureInfo.InvariantCulture);
        }

        public RetornoGenerico(RetornoCnab400 retornoCnab400)
        {
            Inicializa();
            RetornoCnab400Especifico = retornoCnab400;

            /* Transformar de CNAB400 para formato genérico */
            Header.CodigoDoBanco = retornoCnab400.Header.CodigoDoBanco;
            Header.Convenio = retornoCnab400.Header.NumeroConvenio.ToString(CultureInfo.InvariantCulture);
            Header.CodigoAgencia = retornoCnab400.Header.CodigoAgenciaCedente.ToString(CultureInfo.InvariantCulture);
            Header.DvAgencia = retornoCnab400.Header.DvAgenciaCedente;
            Header.NumeroConta = retornoCnab400.Header.ContaCorrente;
            Header.DvConta = retornoCnab400.Header.DvContaCorrente;
            Header.NomeEmpresa = retornoCnab400.Header.NomeDoBeneficiario;
            Header.NomeDoBanco = retornoCnab400.Header.NomeDoBanco;

            foreach (var registroAtual in retornoCnab400.RegistrosDetalhe)
            {
                var banco = BancoFactory.ObterBanco(Header.CodigoDoBanco);
                var ocorrencia = banco.ObtemCodigoOcorrenciaByInt(registroAtual.CodigoDeOcorrencia);

                var detalheGenericoAdd = new RetornoDetalheGenerico
                {
                    NossoNumero = registroAtual.NossoNumero,
                    TipoCobranca = registroAtual.TipoCobranca.ToString(CultureInfo.InvariantCulture),
                    Carteira = registroAtual.CodigoCarteira,
                    PercentualDesconto = registroAtual.TaxaDesconto,
                    PercentualIof = registroAtual.TaxaIof,
                    Especie = registroAtual.Especie,
                    DataCredito = registroAtual.DataDeCredito,
                    DataVencimento = registroAtual.DataDeVencimento,
                    DataLiquidacao = registroAtual.DataLiquidacao,
                    NumeroDocumento = registroAtual.NumeroDocumento,
                    SeuNumero = registroAtual.SeuNumero,
                    ValorDocumento = registroAtual.ValorDoTituloParcela,
                    ValorTarifaCustas = registroAtual.ValorTarifa,
                    ValorOutrasDespesas = registroAtual.ValorOutrasDespesas,
                    ValorJurosDesconto = registroAtual.ValorJurosDesconto,
                    ValorIofDesconto = registroAtual.ValorIofDesconto,
                    ValorAbatimento = registroAtual.ValorAbatimento,
                    ValorDesconto = registroAtual.ValorDesconto,
                    ValorRecebido = registroAtual.ValorLiquidoRecebido + registroAtual.ValorDoDebitoCredito,
                    ValorAcrescimos = registroAtual.ValorJurosDeMora + registroAtual.ValorMulta + registroAtual.ValorTarifa,
                    ValorJuros = registroAtual.ValorJurosDeMora,
                    ValorMulta = registroAtual.ValorMulta,
                    ValorOutrosRecebimentos = registroAtual.ValorOutrosRecebimentos,
                    ValorAbatimentoNaoAproveitadoPeloSacado = registroAtual.ValorAbatimentosNaoAproveitado,
                    ValorLancamento = registroAtual.ValorLancamento,
                    InscricaoSacado = registroAtual.NumeroInscricaoSacado.ToString(CultureInfo.InvariantCulture),
                    NomeSacado = registroAtual.NomeSacado,
                    CodigoOcorrencia = String.IsNullOrEmpty(registroAtual.MotivoCodigoOcorrencia) ? "00" : registroAtual.MotivoCodigoOcorrencia,
                    MensagemOcorrenciaRetornoBancario = ocorrencia.Descricao,
                    Ocorrencia = ocorrencia
                };

                //DATA LIQUIDAÇÃO E DATA OCORRENCIA
                if (detalheGenericoAdd.Pago && detalheGenericoAdd.DataLiquidacao == DateTime.MinValue)
                    detalheGenericoAdd.DataLiquidacao = registroAtual.DataDaOcorrencia;

                RegistrosDetalhe.Add(detalheGenericoAdd);
            }
        }
        
        public RetornoHeaderGenerico Header { get; set; }
        public List<RetornoDetalheGenerico> RegistrosDetalhe { get; set; } 
        public RetornoTrailerGenerico Trailer { get; set; }
        public RetornoCnab240 RetornoCnab240Especifico { get; set; }
        public RetornoCnab400 RetornoCnab400Especifico { get; set; }
    }
}
