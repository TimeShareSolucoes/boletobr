using System.Collections.Generic;

namespace BoletoBr.Arquivo.CNAB400.Remessa
{
    public class RemessaCnab400
    {
        public void AdicionarBoleto(Boleto boletoAdicionar)
        {
            if (boletoAdicionar.BancoBoleto == null)
                throw new ValidacaoBoletoException("Boleto " + boletoAdicionar.NossoNumeroFormatado +
                                                   " não é válido para adição na remessa. Falta informar o banco do boleto.");

            if (boletoAdicionar.CarteiraCobranca == null)
                throw new ValidacaoBoletoException("Boleto " + boletoAdicionar.NossoNumeroFormatado +
                                                   " não é válido para adição na remessa. Falta informar a carteira de cobrança.");

            var detalheRemessaAdicionar = new DetalheRemessaCnab400();

            detalheRemessaAdicionar.CodigoBanco = boletoAdicionar.BancoBoleto.CodigoBanco;
            detalheRemessaAdicionar.Agencia = boletoAdicionar.CedenteBoleto.ContaBancariaCedente.Agencia;
            detalheRemessaAdicionar.DvAgencia = boletoAdicionar.CedenteBoleto.ContaBancariaCedente.DigitoAgencia;
            detalheRemessaAdicionar.ContaCorrente = boletoAdicionar.CedenteBoleto.ContaBancariaCedente.Conta;
            detalheRemessaAdicionar.DvContaCorrente = boletoAdicionar.CedenteBoleto.ContaBancariaCedente.Conta;

            detalheRemessaAdicionar.CodigoCedente = boletoAdicionar.CedenteBoleto.CodigoCedente;
            detalheRemessaAdicionar.NossoNumero = boletoAdicionar.NossoNumeroFormatado;
            detalheRemessaAdicionar.DvNossoNumero = boletoAdicionar.DigitoNossoNumero;
            detalheRemessaAdicionar.NumeroDocumento = boletoAdicionar.NumeroDocumento;
            detalheRemessaAdicionar.DataVencimento = boletoAdicionar.DataVencimento;
            detalheRemessaAdicionar.DataEmissao = boletoAdicionar.DataDocumento;

            detalheRemessaAdicionar.CodigoOcorrencia = boletoAdicionar.CodigoOcorrenciaRemessa;
            detalheRemessaAdicionar.Especie = boletoAdicionar.Especie;
            detalheRemessaAdicionar.Aceite = boletoAdicionar.Aceite;

            detalheRemessaAdicionar.ValorBoleto = boletoAdicionar.ValorBoleto;
            detalheRemessaAdicionar.ValorDesconto = boletoAdicionar.ValorDesconto;
            detalheRemessaAdicionar.ValorIof = boletoAdicionar.Iof;
            detalheRemessaAdicionar.ValorAbatimento = boletoAdicionar.ValorAbatimento;

            detalheRemessaAdicionar.InscricaoPagador = boletoAdicionar.SacadoBoleto.CpfCnpj;
            detalheRemessaAdicionar.NomePagador = boletoAdicionar.SacadoBoleto.Nome;
            detalheRemessaAdicionar.EnderecoPagador =
                boletoAdicionar.SacadoBoleto.EnderecoSacado.LogradouroNumeroComplementoConcatenado;
            detalheRemessaAdicionar.CidadePagador = boletoAdicionar.SacadoBoleto.EnderecoSacado.Cidade;
            detalheRemessaAdicionar.UfPagador = boletoAdicionar.SacadoBoleto.EnderecoSacado.SiglaUf;
            detalheRemessaAdicionar.CepPagador = boletoAdicionar.SacadoBoleto.EnderecoSacado.Cep;
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
