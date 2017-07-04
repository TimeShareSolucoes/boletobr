using System;

namespace BoletoBr
{
    public class CarteiraCobranca
    {
        public CarteiraCobranca()
        {
            /* Informar True quando a Empresa enviar somente a Remessa para o banco e a emissão e distribuição do boleto 
             for por responsabilidade do banco */
            this.BancoEmiteBoleto = false;
        }

        public string Codigo { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        /// <summary>
        /// Variação de Carteira usada para Banco do Brasil
        /// </summary>
        public string Variacao { get; set; }
        public bool BancoEmiteBoleto { get; set; }

        public string CodigoCompleto
        {
            get
            {
                string resultado;
                if (!String.IsNullOrEmpty(this.Variacao))
                    resultado = this.Codigo + "-" + this.Variacao;
                else
                    resultado = this.Codigo;

                return resultado;
            }
        }
    }
}
