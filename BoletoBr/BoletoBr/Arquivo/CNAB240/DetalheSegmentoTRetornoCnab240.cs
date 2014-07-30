using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Dominio
{
    public class DetalheSegmentoTRetornoCnab240
    {
        public int CodigoBanco { get; set; }
        public string LoteServico { get; set; }
        public int CodigoRegistro { get; set; }
        public int NumeroRegistro { get; set; }
        public string CodigoSegmento { get; set; }
        public int CodigoMovimento { get; set; }
        public int Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public int ContaCorrente { get; set; }
        public string DigitoContaCorrente { get; set; }
        public int CodigoCedente { get; set; }

        /// <summary>
        /// Número do banco de sacados
        /// </summary>
        public int NumeroBanco { get; set; }

        public int ModalidadeNossoNumero { get; set; }
        public string NossoNumero { get; set; }
        public int CodigoCarteira { get; set; }
        public string NumeroDocumento { get; set; }
        public int DataVencimento { get; set; }
        public decimal ValorTitulo { get; set; }
        public int BancoCobradorRecebedor { get; set; }
        public int AgenciaCobradoraRecebedora { get; set; }
        public string DvAgenciaCobradoraRecebedora { get; set; }
        public string IdentificacaoTituloNaEmpresa { get; set; }
        public int Moeda { get; set; }
        public int TipoInscricaoSacado { get; set; }
        public long NumeroInscricaoSacado { get; set; }
        public string NomeSacado { get; set; }
        public decimal ValorTarifas { get; set; }

        /// <summary>
        /// Identificação para rejeições, tarifas, custas, liquidação e baixas
        /// </summary>
        public string MotivoOcorrencia { get; set; }

        public string UsoFebraban { get; set; }

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
        public string DvAgenciaConta { get; set; }

        /// <summary>
        /// NOTA: C030 do Layouto do Banco Bradesco CNAB 240
        /// -> Número do Contrato da Operação de Crédito
        /// Número adotado pela empresa beneficiária para identificação do número do contrato.
        /// NÃO TRATADO PELO BANCO
        /// OBS.: Preencher com zeros
        /// </summary>
        public long NumeroContrato { get; set; }
        #endregion
    }
} 