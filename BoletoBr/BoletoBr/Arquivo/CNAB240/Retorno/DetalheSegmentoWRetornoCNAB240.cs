using System;

namespace BoletoBr.Arquivo.CNAB240.Retorno
{
    public class DetalheSegmentoWRetornoCnab240
    {
        public DetalheSegmentoWRetornoCnab240()
        {
            CodigoErro = 0;
        }

        public int CodigoErro { get; set; }

        public void LerDetalheSegmentoWRetornoCnab240(string registro)
        {
            try
            {
                if (registro.Substring(13, 1) != "W")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento W.");

                if (!registro.Substring(28, 3).Trim().Equals(""))
                    CodigoErro = Convert.ToInt32(registro.Substring(28, 3));

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO W.", ex);
            }
        }
    }
}
