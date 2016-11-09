using BoletoBr.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Banrisul
{
    public class BancoBanrisul //: IBanco
    {
        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        public Image LogotipoBancoParaExibicao { get; set; }
        public string LocalDePagamento { get; private set; }
        public string MoedaBanco { get; private set; }

        public BancoBanrisul()
        {
            CodigoBanco = "041";
            DigitoBanco = "8";
            NomeBanco = "Banrisul";
            LocalDePagamento = "Pagável em qualquer banco até o vencimento";
            MoedaBanco = "9";
        }

        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            //if (!(boleto.CarteiraCobranca.Codigo == "1/01"))
            //    throw new NotImplementedException("Carteira não implementada. Carteira dísponivel 1/01");

            //if (boleto.CarteiraCobranca.Codigo == "1/01")
            //{
            //    if (boleto.CedenteBoleto.ContaBancariaCedente.Agencia.BoletoBrToStringSafe().Trim().Length > 4)
            //        throw new ValidacaoBoletoException("A agencia deve ter no máximo 4 dígitos.");

            //    var codigoCliente = boleto.CedenteBoleto.CodigoCedente + boleto.CedenteBoleto.DigitoCedente;
            //    if (codigoCliente.BoletoBrToStringSafe().Trim().Length > 10)
            //        throw new ValidacaoBoletoException("O código do cedente deve ter no máximo 10 dígitos.");

            //    if (boleto.IdentificadorInternoBoleto.BoletoBrToStringSafe().Trim().Length > 7)
            //        throw new ValidacaoBoletoException("Identificador interno do boleto deve ter no máximo 7 dígitos.");
            //}
        }

        public void FormataMoeda(Boleto boleto)
        {
            boleto.Moeda = MoedaBanco;

            if (String.IsNullOrEmpty(boleto.Moeda))
                throw new Exception("Espécie/Moeda para o boleto não foi informada.");

            if ((boleto.Moeda == "9") || (boleto.Moeda == "REAL") || (boleto.Moeda == "R$"))
                boleto.Moeda = "R$";
        }
    }
}
