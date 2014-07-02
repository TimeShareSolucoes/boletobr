using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Dominio
{
    internal class DetalheSegmentoURetornoCnab240
    {
        public DetalheSegmentoURetornoCnab240(string registro)
        {
            ListaDetalhe = new List<DetalheSegmentoURetornoCnab240>();
            Registro = registro;
        }

        #region Propriedades

        public decimal Servico_Codigo_Movimento_Retorno { get; set; }

        public decimal JurosMultaEncargos { get; set; }

        public decimal ValorDescontoConcedido { get; set; }

        public decimal ValorAbatimentoConcedido { get; set; }

        public decimal ValorIOFRecolhido { get; set; }

        public decimal ValorPagoPeloSacado { get; set; }

        public decimal ValorLiquidoASerCreditado { get; set; }

        public decimal ValorOutrasDespesas { get; set; }

        public decimal ValorOutrosCreditos { get; set; }

        public DateTime DataOcorrencia { get; set; }

        public DateTime DataCredito { get; set; }

        public string CodigoOcorrenciaSacado { get; set; }

        public DateTime DataOcorrenciaSacado { get; set; }

        public decimal ValorOcorrenciaSacado { get; set; }

        public List<DetalheSegmentoURetornoCnab240> ListaDetalhe { get; set; }

        public string Registro { get; private set; }

        #endregion

        public void LerDetalheSegmentoURetornoCnab240(string registro)
        {
            try
            {
                Registro = Registro;

                if (registro.Substring(13, 1) != "U")
                    throw new Exception("Registro inv�lido. O detalhe n�o possu� as caracter�sticas do segmento U.");

                int dataOcorrenciaSacado = 0;
                if (registro.Substring(153, 4) != "    ")
                    dataOcorrenciaSacado = Convert.ToInt32(registro.Substring(157, 8));

                decimal jurosMultaEncargos = Convert.ToInt64(registro.Substring(17, 15));
                JurosMultaEncargos = jurosMultaEncargos/100;
                decimal valorDescontoConcedido = Convert.ToInt64(registro.Substring(32, 15));
                ValorDescontoConcedido = valorDescontoConcedido/100;
                decimal valorAbatimentoConcedido = Convert.ToInt64(registro.Substring(47, 15));
                ValorAbatimentoConcedido = valorAbatimentoConcedido/100;
                decimal valorIOFRecolhido = Convert.ToInt64(registro.Substring(62, 15));
                ValorIOFRecolhido = valorIOFRecolhido/100;
                decimal valorPagoPeloSacado = Convert.ToInt64(registro.Substring(77, 15));
                ValorPagoPeloSacado = valorPagoPeloSacado/100;
                decimal valorLiquidoASerCreditado = Convert.ToInt64(registro.Substring(92, 15));
                ValorLiquidoASerCreditado = valorLiquidoASerCreditado/100;
                decimal valorOutrasDespesas = Convert.ToInt64(registro.Substring(107, 15));
                ValorOutrasDespesas = valorOutrasDespesas/100;
                decimal valorOutrosCreditos = Convert.ToInt64(registro.Substring(122, 15));
                ValorOutrosCreditos = valorOutrosCreditos/100;
                int dataOcorrencia = Convert.ToInt32(registro.Substring(137, 8));
                DataOcorrencia = Convert.ToDateTime(dataOcorrencia.ToString("##-##-####"));
                int dataCredito = Convert.ToInt32(registro.Substring(145, 8));
                if (dataCredito != 0)
                    DataCredito = Convert.ToDateTime(dataCredito.ToString("##-##-####"));
                CodigoOcorrenciaSacado = registro.Substring(153, 4);
                if (dataOcorrenciaSacado != 0)
                    DataOcorrenciaSacado = Convert.ToDateTime(dataOcorrenciaSacado.ToString("##-##-####"));
                decimal valorOcorrenciaSacado = Convert.ToInt64(registro.Substring(165, 15));
                ValorOcorrenciaSacado = valorOcorrenciaSacado/100;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }
        }
    }
}
    

