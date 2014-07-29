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
        #region Propriedades

        public int CodigoBanco { get; set; }
        public string LoteServico { get; set; }
        public int CodigoRegistro { get; set; }
        public int NumeroRegistro { get; set; }
        public string CodigoSegmento { get; set; }
        public int CodigoMovimento { get; set; }
        public int Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public int CodigoCedente { get; set; }
        /// <summary>
        /// Número do banco de sacados
        /// </summary>
        public int NumeroBanco { get; set; }
        public int ModalidadeNossoNumero { get; set; }
        public string NossoNumero { get; set; }
        public int CodigoCarteira { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorTitulo { get; set; }
        public int BancoCobradorRecebedor { get; set; }
        public int AgenciaCobradoraRecebedora { get; set; }
        public int DvAgenciaCobradoraRecebedora { get; set; }
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

        #endregion
    }
}