using System;

namespace BoletoBr.Arquivo.CNAB240.Remessa
{
    public class HeaderDeArquivoCnab240
    {

        string _mensagemRemessa;
        string _numeroRemessa;
        string _dataRemessa;
        string _horaRemessa;

        #region Propriedades

        public string MensagemRemessa
        {
            get { return _mensagemRemessa; }
            set { _mensagemRemessa = value; }
        }

        public string NumeroRemessa
        {
            get { return _numeroRemessa; }
            set { _numeroRemessa = value; }
        }
        public string DataRemessa
        {
            get { return _dataRemessa; }
            set { _dataRemessa = value; }
        }
        public string HoraRemessa
        {
            get { return _horaRemessa; }
            set { _horaRemessa = value; }
        }

        #endregion

        #region Métodos de Instância

        public void LerHeaderDeArquivoCnab240(string registro)
        {
            try
            {
                if (registro.Substring(7, 1) != "0")
                    throw new Exception("Registro inválido. O detalhe não possuí as características de Header de Arquivo.");

                _mensagemRemessa = registro.Substring(171, 20).Trim();
                _numeroRemessa = registro.Substring(157, 6).Trim().PadLeft(6, '0');
                _dataRemessa = Convert.ToDecimal(registro.Substring(143, 8)).ToString("00/00/0000");
                _horaRemessa = Convert.ToDecimal(registro.Substring(151, 6)).ToString("00:00:00");

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - Header de Arquivo.", ex);
            }
        }

        #endregion
    }
}
