using System;
using BoletoBr.Dominio;
using BoletoBr.Enums;
using BoletoBr.Interfaces;

namespace BoletoBr.Arquivo.CNAB400.Remessa
{
    public class DetalheRemessaCnab400
    {
        public int TipoRegistro { get; set; }
        public string Agencia { get; set; }
        public string DvAgencia { get; set; }
        public string RazaoContaCorrente { get; set; }
        public string ContaCorrente { get; set; }
        public string DvContaCorrente { get; set; }
        public string CodigoCedente { get; set; }
        public string UsoEmpresa { get; set; }
        public string CodigoBanco { get; set; }
        public int CampoMulta { get; set; }
        public int PercentualMulta { get; set; }
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
        public string Instrucao1 { get; set; }
        public string Instrucao2 { get; set; }
        public decimal ValorCobradoDiaAtraso { get; set; }
        public DateTime DataLimiteConcessaoDesconto { get; set; }
        public decimal? ValorDesconto { get; set; }
        public decimal? ValorIof { get; set; }
        public decimal? ValorAbatimento { get; set; }
        public int IdTipoInscricaoPagador { get; set; }
        public string InscricaoPagador { get; set; }
        public string NomePagador { get; set; }
        public string EnderecoPagador { get; set; }
        public string CidadePagador { get; set; }
        public string UfPagador { get; set; }
        public string CepPagador { get; set; }
        public string Mensagem1 { get; set; }
        public string NomeAvalistaOuMensagem2 { get; set; }
        public int NumeroSequencialRegistro { get; set; }
    }
}
