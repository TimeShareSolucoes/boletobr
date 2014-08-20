using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Arquivo.CNAB400.Retorno
{
    /// <summary>
    /// BANCO BRADESCO
    /// Registro de Transação - TIPO 3 - Rateio de Crédito
    /// Layout para Cobrança com Registro com Emissão de Papeletas pelo Banco ou pela Empresa
    /// </summary>
    public class DetalheRateioRetornoCnab400
    {
        public int CodigoDoRegistro { get; set; }
        public string IdentificacaoEmpresaNoBanco { get; set; }
        public string NossoNumero { get; set; }
        public int CodigoCalculoRateio { get; set; }
        public int TipoValorInformado { get; set; }
        public int CodigoBancoPrimeiroBeneficiario { get; set; }
        public int CodigoAgenciaPrimeiroBeneficiario { get; set; }
        public string DvAgenciaPrimeiroBeneficiario { get; set; }
        public long ContaCorrentePrimeiroBeneficiario { get; set; }
        public string DvContaCorrentePrimeiroBeneficiario { get; set; }
        public decimal ValorRateioPrimeiroBeneficiario { get; set; }
        public string NomePrimeiroBeneficiario { get; set; }
        public string ParcelaPrimeiroBeneficiario { get; set; }
        public int FloatingPrimeiroBeneficiario { get; set; }
        public DateTime DataCreditoPrimeiroBeneficiario { get; set; }
        public int MotivoOcorrenciaPrimeiroBeneficiario { get; set; }
        public int CodigoBancoSegundoBeneficiario { get; set; }
        public int CodigoAgenciaSegundoBeneficiario { get; set; }
        public string DvAgenciaSegundoBeneficiario { get; set; }
        public long ContaCorrenteSegundoBeneficiario { get; set; }
        public string DvContaCorrenteSegundoBeneficiario { get; set; }
        public decimal ValorRateioSegundoBeneficiario { get; set; }
        public string NomeSegundoBeneficiario { get; set; }
        public string ParcelaSegundoBeneficiario { get; set; }
        public int FloatingSegundoBeneficiario { get; set; }
        public DateTime DataCreditoSegundoBeneficiario { get; set; }
        public int MotivoOcorrenciaSegundoBeneficiario { get; set; }
        public int CodigoBancoTerceiroBeneficiario { get; set; }
        public int CodigoAgenciaTerceiroBeneficiario { get; set; }
        public string DvAgenciaTerceiroBeneficiario { get; set; }
        public long ContaCorrenteTerceiroBeneficiario { get; set; }
        public string DvContaCorrenteTerceiroBeneficiario { get; set; }
        public decimal ValorRateioTerceiroBeneficiario { get; set; }
        public string NomeTerceiroBeneficiario { get; set; }
        public string ParcelaTerceiroBeneficiario { get; set; }
        public int FloatingTerceiroBeneficiario { get; set; }
        public DateTime DataCreditoTerceiroBeneficiario { get; set; }
        public int MotivoOcorrenciaTerceiroBeneficiario { get; set; }
        public int NumeroSequencial { get; set; }
    }
}
