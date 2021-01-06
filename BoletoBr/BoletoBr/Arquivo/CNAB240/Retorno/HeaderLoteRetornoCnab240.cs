using System;

namespace BoletoBr.Arquivo.CNAB240.Retorno
{
    public class HeaderLoteRetornoCnab240
    {
        public int CodigoBanco { get; set; }
        public string LoteServico { get; set; }
        public int CodigoRegistro { get; set; }
        public string TipoOperacao { get; set; }
        public int TipoServico { get; set; }
        public int VersaoLayoutLote { get; set; }
        public int TipoInscricaoEmpresa { get; set; }
        public string NumeroInscricaoEmpresa { get; set; }
        public string Convenio { get; set; }
        public int CodigoAgencia { get; set; }
        public string DvCodigoAgencia { get; set; }
        public string ContaCorrente { get; set; }
        public string DvContaCorrente { get; set; }
        public int CodigoCedente { get; set; }
        public int CodigoModeloPersonalizado { get; set; }
        public string NomeDoBeneficiario { get; set; }
        public string Mensagem1 { get; set; }
        public string Mensagem2 { get; set; }
        public string NumeroRemessaRetorno { get; set; }
        public DateTime DataGeracaoGravacao { get; set; }
        public DateTime DataDeCredito { get; set; }
        public string ComplementoRegistro { get; set; }

        #region Banco do Brasil

        public string ConvenioNumeroCobranca { get; set; }
        public int CedenteCobranca { get; set; }
        public int CarteiraCobranca { get; set; }
        public int VariacaoCarteiraCobranca { get; set; }

        #endregion

        #region Bradesco

        /// <summary>
        /// NOTA: G012 do Layout do Banco Bradesco CNAB 240
        /// -> Dígito Verificador da Agência e Conta
        /// Código adotado pelo responsável pela conta corrente, para verificação da autenticidade do número da conta corrente.
        /// Para bancos que se utilizam de duas posições para o dígito verificador do número da conta corrente, preencher este campo com a 1ª posição deste dígito.
        /// Exemplo: 
        /// C/C = 45981-36
        /// Neste caso o DV da Ag/Conta = 6
        /// </summary>
        public int FormaLancamento { get; set; }
        public string DvAgenciaConta { get; set; }
        public DateTime DataSaldoInicial { get; set; }
        public decimal ValorSaldoInicial { get; set; }
        public string SituacaoSaldoInicial { get; set; }
        public string PosicaoSaldoInicial { get; set; }
        public string MoedaReferenciadaExtrato { get; set; }
        public int NumeroSequenciaExtrato { get; set; }

        #endregion
    }
}
