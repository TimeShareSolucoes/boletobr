using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BoletoBr.View.Win.Dominio
{
    public class CarteiraBoleto
    {
        [PrimaryKey, AutoIncrement]
        public int IdCarteiraBoleto { get; set; }

        public string CodigoBanco { get; set; }
        public string NomeBanco { get; set; }

        public string NumeroAgencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string NumeroConta { get; set; }
        public string DigitoConta { get; set; }

        public string DescricaoCarteira { get; set; }
        public string NumeroCarteira { get; set; }
        public string ModeloBoleto { get; set; }
        public string TipoArquivoRetorno { get; set; }
        public string TipoArquivoRemessa { get; set; }
        public string CodigoCedente { get; set; }
        public string DigitoCodigoCedente { get; set; }
        public string NumeroConvenio { get; set; }
        public string CodigoTransmissao { get; set; }
        public bool BancoGeraBoleto { get; set; }

        public string NomeCedente { get; set; }
        public string CpfCnpjCedente { get; set; }
        public string EnderecoCedente { get; set; }
        public string ComplementoCedente { get; set; }
        public string NumeroCedente { get; set; }
        public string BairroCedente { get; set; }
        public string CidadeCedente { get; set; }
        public string UfCedente { get; set; }
        public string CepCedente { get; set; }

        public decimal ValorJuros { get; set; }
        public decimal ValorMulta { get; set; }
        public string Instrucao1 { get; set; }
        public string Instrucao2 { get; set; }
        public string Instrucao3 { get; set; }
        public string Instrucao4 { get; set; }
        public string Instrucao5 { get; set; }
        public string Instrucao6 { get; set; }
    }
}
