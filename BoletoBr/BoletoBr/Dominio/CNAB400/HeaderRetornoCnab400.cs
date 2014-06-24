using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr
{
    public class HeaderRetornoCnab400
    {
        
        /// <summary>
        /// Numérico igual a "0" (zero)
        /// </summary>
        public int CodigoDeRetorno { get; set; }
        /// <summary>
        /// Numérico igual a "2"
        /// </summary>
        public int TipoDeRegistro { get; set; }
        /// <summary>
        /// Alfabético igual a "RETORNO"
        /// </summary>
        public string LiteralDeRemessa { get; set; }
        /// <summary>
        /// Numérico igual a "01"
        /// </summary>
        public int CodigoDoServico { get; set; }
        /// <summary>
        /// Alfabético igual a "COBRANÇA"
        /// </summary>
        public string LiteralDeServico { get; set; }
        public int Zeros { get; set; }
        public int Agencia { get; set; }
        public int SubConta { get; set; }
        public int Conta { get; set; }
        public int BancoTamanho2 { get; set; }
        public string NomeCliente { get; set; }
        public int CodigoBanco { get; set; }
        public string NomeDoBanco { get; set; }
        public int DataDeGeracao { get; set; }
        /// <summary>
        /// Numérico igual a "01600"
        /// </summary>
        public int Densidade { get; set; }
        /// <summary>
        /// Alfabético igual a "01600"
        /// </summary>
        public string LiteralDensidade { get; set; }
        public int BancoTamanho11 { get; set; }
        public int DataDoCredito { get; set; }
        public int BancoTamanho263 { get; set; }
        public int SequencialDoArquivo { get; set; }
        public int BancoTamanho1 { get; set; }
        /// <summary>
        /// Numérico igual a "000001"
        /// </summary>
        public int NumeroSequencial { get; set; }
    }
}
