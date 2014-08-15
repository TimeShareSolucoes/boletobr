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
            this.Header.CodigoDoBanco = retornoCnab240.Header.CodigoBanco.ToString();
            this.Header.Convenio = retornoCnab240.Header.Convenio.ToString();
            this.Header.CodigoAgencia = retornoCnab240.Header.CodigoAgencia.ToString();
            this.Header.DvAgencia = retornoCnab240.Header.DvAgenciaConta.ToString();
            this.Header.NumeroConta = retornoCnab240.Header.ContaCorrente.ToString();
            this.Header.DvConta = retornoCnab240.Header.DvContaCorrente.ToString();
            this.Header.NomeEmpresa = retornoCnab240.Header.NomeDoBeneficiario.ToString();
            this.Header.NomeDoBanco = retornoCnab240.Header.NomeDoBanco.ToString();
                
            foreach (var loteAtual in retornoCnab240.Lotes)
            {
                foreach (var d in loteAtual.RegistrosDetalheSegmentos)
                {
                    var detalheGenericoAdd = new RetornoDetalheGenerico();
                    // Segmento T
                    detalheGenericoAdd.NossoNumero = d.RegistrosDetalheSegmentoT.NossoNumero;
                    detalheGenericoAdd.Carteira = d.RegistrosDetalheSegmentoT.CodigoCarteira.ToString();
                    detalheGenericoAdd.NumeroDocumento = d.RegistrosDetalheSegmentoT.NumeroDocumento.ToString();
                    detalheGenericoAdd.ValorTitulo = d.RegistrosDetalheSegmentoT.ValorTitulo / 100;
                    detalheGenericoAdd.ValorTarifaCustas = d.RegistrosDetalheSegmentoT.ValorTarifas / 100;
                    detalheGenericoAdd.DataVencimento = Convert.ToDateTime(d.RegistrosDetalheSegmentoT.DataVencimento.ToString("d"));
                    detalheGenericoAdd.InscricaoSacado = d.RegistrosDetalheSegmentoT.NumeroInscricaoSacado.ToString();
                    detalheGenericoAdd.NomeSacado = d.RegistrosDetalheSegmentoT.NomeSacado;
                    detalheGenericoAdd.CodigoMovimento = d.RegistrosDetalheSegmentoT.CodigoMovimento.ToString();
                    detalheGenericoAdd.CodigoOcorrencia = d.RegistrosDetalheSegmentoT.MotivoOcorrencia.ToString();
                    //detalheGenericoAdd.TipoLiquidacao = detalheTipoT.TipoLiquidacao;

                    // Segmento U
                    detalheGenericoAdd.DataPagamento = Convert.ToDateTime(d.RegistrosDetalheSegmentoU.DataPagamento.ToString("d"));
                    detalheGenericoAdd.ValorAcrescimos = d.RegistrosDetalheSegmentoU.JurosMultaEncargos / 100;
                    detalheGenericoAdd.ValorDesconto = d.RegistrosDetalheSegmentoU.ValorDescontoConcedido / 100;
                    detalheGenericoAdd.ValorAbatimento = d.RegistrosDetalheSegmentoU.ValorAbatimentoConcedido / 100;
                    detalheGenericoAdd.ValorIof = d.RegistrosDetalheSegmentoU.ValorIofRecolhido / 100;
                    detalheGenericoAdd.ValorPago = d.RegistrosDetalheSegmentoU.ValorPagoPeloSacado / 100;
                    detalheGenericoAdd.ValorLiquido = d.RegistrosDetalheSegmentoU.ValorLiquidoASerCreditado / 100;
                    detalheGenericoAdd.ValorOutrasDespesas = d.RegistrosDetalheSegmentoU.ValorOutrasDespesas / 100;
                    detalheGenericoAdd.ValorOutrosCreditos = d.RegistrosDetalheSegmentoU.ValorOutrosCreditos / 100;
                    detalheGenericoAdd.DataLiquidacao = Convert.ToDateTime(d.RegistrosDetalheSegmentoU.DataLiquidacao.ToString("d"));
                    detalheGenericoAdd.DataCredito = Convert.ToDateTime(d.RegistrosDetalheSegmentoU.DataCredito.ToString("d"));
                    detalheGenericoAdd.CodigoOcorrencia = d.RegistrosDetalheSegmentoU.CodigoOcorrenciaPagador;
                    detalheGenericoAdd.DataOcorrencia = d.RegistrosDetalheSegmentoU.DataOcorrenciaPagador;
                    detalheGenericoAdd.ValorOcorrencia = d.RegistrosDetalheSegmentoU.ValorOcorrenciaPagador / 100;
                    detalheGenericoAdd.DataDebitoTarifaCustas = d.RegistrosDetalheSegmentoU.DataDebitoTarifa;
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
                detalheGenericoAdd.ValorTitulo = registroAtual.ValorDoTituloParcela/100;
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
            this.Trailer.QtdRegistrosArquivo = retornoCnab400.Trailer.QtdRegistrosBaixados.ToString();
        }
        public RetornoHeaderGenerico Header { get; set; }
        public List<RetornoDetalheGenerico> RegistrosDetalhe { get; set; } 
        public RetornoTrailerGenerico Trailer { get; set; }
        public RetornoCnab240 RetornoCnab240Especifico { get; set; }
        public RetornoCnab400 RetornoCnab400Especifico { get; set; }
    }
}
