using System;
using System.Collections.Generic;
using System.Linq;
using BoletoBr.Dominio;
using BoletoBr.Enums;
using BoletoBr.Interfaces;

namespace BoletoBr.Arquivo.CNAB400.Remessa
{
    public class DetalheRemessaCnab400
    {
        public DetalheRemessaCnab400(Boleto boleto, int numeroSequencialRegistro)
        {
            this.CodigoBanco = boleto.BancoBoleto.CodigoBanco;
            this.CarteiraCobranca = boleto.CarteiraCobranca.Codigo;
            Variacao = boleto.CarteiraCobranca.Variacao;
            this.InscricaoCedente = boleto.CedenteBoleto.CpfCnpj;
            this.CodigoOcorrencia = boleto.CodigoOcorrenciaRemessa;
            this.Agencia = boleto.CedenteBoleto.ContaBancariaCedente.Agencia;
            this.DvAgencia = boleto.CedenteBoleto.ContaBancariaCedente.DigitoAgencia;
            this.ContaCorrente = boleto.CedenteBoleto.ContaBancariaCedente.Conta;
            this.DvContaCorrente = boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta;
            Convenio = boleto.CedenteBoleto.Convenio;
            this.CodigoCedente = boleto.CedenteBoleto.CodigoCedente;
            this.DVCedente = boleto.CedenteBoleto.DigitoCedente.BoletoBrToStringSafe();
            this.RazaoContaCorrente = boleto.CedenteBoleto.Nome;
            this.NossoNumero = boleto.IdentificadorInternoBoleto;
            this.DvNossoNumero = boleto.DigitoNossoNumero;
            this.NossoNumeroFormatado = boleto.NossoNumeroFormatado;
            this.NumeroDocumento = boleto.NumeroDocumento;
            this.DataVencimento = boleto.DataVencimento;
            this.ValorBoleto = boleto.ValorBoleto;
            this.Especie = boleto.Especie;
            this.Aceite = boleto.Aceite;
            this.DataEmissao = boleto.DataDocumento;
            this.ValorDesconto = Convert.ToDecimal(boleto.ValorDesconto).Equals(null)
                ? 0
                : Convert.ToDecimal(boleto.ValorDesconto);
            if (boleto.DataLimitDesconto != null && boleto.DataLimitDesconto.GetValueOrDefault() > DateTime.MinValue)
                this.DataLimiteConcessaoDesconto = boleto.DataLimitDesconto.GetValueOrDefault();
            this.ValorIof = Convert.ToDecimal(boleto.Iof).Equals(null) ? 0 : Convert.ToDecimal(boleto.Iof);
            this.ValorAbatimento = Convert.ToDecimal(boleto.ValorAbatimento).Equals(null)
                ? 0
                : Convert.ToDecimal(boleto.ValorAbatimento);
            this.InscricaoPagador = boleto.SacadoBoleto.CpfCnpj;
            this.NomePagador = boleto.SacadoBoleto.Nome;
            this.EnderecoPagador = boleto.SacadoBoleto.EnderecoSacado.LogradouroNumeroComplementoConcatenado;
            this.BairroPagador = boleto.SacadoBoleto.EnderecoSacado.Bairro;
            this.CidadePagador = boleto.SacadoBoleto.EnderecoSacado.Cidade;
            this.UfPagador = boleto.SacadoBoleto.EnderecoSacado.SiglaUf;
            this.CepPagador = boleto.SacadoBoleto.EnderecoSacado.Cep;
            this.NumeroSequencialRegistro = numeroSequencialRegistro;
            this.Moeda = boleto.Moeda;
            this.QuantidadeMoeda = Convert.ToDecimal(boleto.QuantidadeMoeda).Equals(null)
                ? 0
                : Convert.ToDecimal(boleto.QuantidadeMoeda);
            this.DataJurosMora = boleto.DataJurosMora;
            this.Instrucoes = boleto.InstrucoesDoBoleto;
            this.TipoCobrancaJuro = boleto.TipoCobrancaJuro;
            PercentualMulta = boleto.PercentualMulta.BoletoBrToStringSafe().BoletoBrToDecimal();
            ValorMoraDia = boleto.PercentualJurosMora.BoletoBrToStringSafe().BoletoBrToDecimal();
            TipoCarteiraCobranca = boleto.CarteiraCobranca.Tipo;
            BancoEmiteBoleto = boleto.CarteiraCobranca.BancoEmiteBoleto;
            /*Instruções Protesto*/
            this.NroDiasParaProtesto = boleto.CarteiraCobranca.QtdDiasProtesto;
            this.CodigoProtesto = NroDiasParaProtesto > 0 ? boleto.BancoBoleto.CodigoProteso() : boleto.BancoBoleto.CodigoProteso(false);
            /*Instrução de Baixa/Devolução*/
            this.PrazoBaixaDevolucao = boleto.PrazoBaixaDevolucao;
            //this.MensagemLinha1 = boleto.InstrucoesDoBoleto.ElementAt(0).TextoInstrucao.Equals(null) ? "" : boleto.InstrucoesDoBoleto.ElementAt(0).TextoInstrucao;
            //this.MensagemLinha2 = boleto.InstrucoesDoBoleto.ElementAt(1).TextoInstrucao.Equals(null) ? "" : boleto.InstrucoesDoBoleto.ElementAt(1).TextoInstrucao;
            //this.MensagemLinha3 = boleto.InstrucoesDoBoleto.ElementAt(2).TextoInstrucao.Equals(null) ? "" : boleto.InstrucoesDoBoleto.ElementAt(2).TextoInstrucao;
            //this.MensagemLinha4 = boleto.InstrucoesDoBoleto.ElementAt(3).TextoInstrucao.Equals(null) ? "" : boleto.InstrucoesDoBoleto.ElementAt(3).TextoInstrucao;
            //this.MensagemLinha5 = boleto.InstrucoesDoBoleto.ElementAt(4).TextoInstrucao.Equals(null) ? "" : boleto.InstrucoesDoBoleto.ElementAt(4).TextoInstrucao;
            //this.MensagemLinha6 = boleto.InstrucoesDoBoleto.ElementAt(5).TextoInstrucao.Equals(null) ? "" : boleto.InstrucoesDoBoleto.ElementAt(5).TextoInstrucao;

            this.Instrucao1 = boleto.Instrucao1;
            this.Instrucao2 = boleto.Instrucao2;

            #region #033|SANTADER
            // Informação cedida pelo banco que identifica o arquivo remessa do cliente
            this.CodigoDeTransmissao = boleto.CodigoDeTransmissao;
            this.DataDesconto = boleto.DataDesconto;
            #endregion
 
        }

        #region #INFORMAÇÕES DE USO EXCLUSIVO NA REMESSA

        // Usado para identificar código da carteira no arquivo de remessa do Banco Santander
        public string CarteiraCobranca { get; set; }
        public string Variacao { get; set; }
        public string TipoCarteiraCobranca { get; set; }
        public string InscricaoCedente { get; set; }
        public string CodigoDeTransmissao { get; set; }
        public string NossoNumeroFormatado { get; set; }
        public int NroDiasParaProtesto { get; set; }
        public int PrazoBaixaDevolucao { get; set; }
        public int CodigoProtesto { get; set; }
        public string Moeda { get; set; }
        public decimal QuantidadeMoeda { get; set; }

        #endregion

        #region Mensagens Boleto

        public string MensagemLinha1 { get; set; }
        public string MensagemLinha2 { get; set; }
        public string MensagemLinha3 { get; set; }
        public string MensagemLinha4 { get; set; }
        public string MensagemLinha5 { get; set; }
        public string MensagemLinha6 { get; set; }

        #endregion

        public int TipoRegistro { get; set; }
        public string Agencia { get; set; }
        public string DvAgencia { get; set; }
        public string RazaoContaCorrente { get; set; }
        public string ContaCorrente { get; set; }
        public string DvContaCorrente { get; set; }
        public string Convenio { get; set; }
        public string CodigoCedente { get; set; }
        public string DVCedente { get; set; }
        public string UsoEmpresa { get; set; }
        public string CodigoBanco { get; set; }
        public int CampoMulta { get; set; }
        public decimal PercentualMulta { get; set; }
        public string NossoNumero { get; set; }
        public string DvNossoNumero { get; set; }
        public decimal ValorDescontoDia { get; set; }
        public int IdEmissao { get; set; }
        public string IdDebitoAutomatico { get; set; }
        public string IdRateioCredito { get; set; }
        public int IdEnderecamentoDebitoAutomatico { get; set; }
        public ICodigoOcorrencia CodigoOcorrencia { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorBoleto { get; set; }
        public string BancoEncarregadoCobranca { get; set; }
        public string AgenciaDepositaria { get; set; }
        public IEspecieDocumento Especie { get; set; }
        public string Aceite { get; set; }
        public DateTime DataEmissao { get; set; }
        public List<IInstrucao> Instrucoes { get; set; }
        public string Instrucao1 { get; set; }
        public string Instrucao2 { get; set; }
        public decimal ValorCobradoDiaAtraso { get; set; }
        public DateTime DataLimiteConcessaoDesconto { get; set; }
        public DateTime DataLimiteConcessaoDesconto2 { get; set; }
        public DateTime DataLimiteConcessaoDesconto3 { get; set; }
        /// <summary>
        /// Propriedade duplicada
        /// </summary>
        [Obsolete]
        public DateTime DataDesconto { get; set; }
        public DateTime DataJurosMora { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorDesconto2 { get; set; }
        public decimal ValorDesconto3 { get; set; }
        public decimal ValorIof { get; set; }
        public decimal ValorAbatimento { get; set; }
        public decimal ValorJuros { get; set; }
        public decimal ValorMoraDia { get; set; }
        public int IdTipoInscricaoPagador { get; set; }
        public string InscricaoPagador { get; set; }
        public string NomePagador { get; set; }
        public string EnderecoPagador { get; set; }
        public string BairroPagador { get; set; }
        public string CidadePagador { get; set; }
        public string UfPagador { get; set; }
        public string CepPagador { get; set; }
        public string Mensagem1 { get; set; }
        public string NomeAvalistaOuMensagem2 { get; set; }
        public int NumeroSequencialRegistro { get; set; }
        public TipoCobrancaJuro TipoCobrancaJuro { get; set; }
        public bool BancoEmiteBoleto { get; set; }
    }
}
