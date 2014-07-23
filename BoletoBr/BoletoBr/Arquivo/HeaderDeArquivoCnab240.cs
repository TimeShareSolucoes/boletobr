using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Arquivo
{
    public class HeaderDeArquivoCnab240
    {

        string _mensagemRemessa;
        string _numeroRemessa;
        string _dataRemessa;
        string _horaRemessa;

        public HeaderDeArquivoCnab240()
        {
        }

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

        public void LerHeaderDeArquivoCNAB240(string Registro)
        {
            try
            {
                if (Registro.Substring(7, 1) != "0")
                    throw new Exception("Registro inválido. O detalhe não possuí as características de Header de Arquivo.");

                _mensagemRemessa = Registro.Substring(171, 20).Trim();
                _numeroRemessa = Registro.Substring(157, 6).Trim().PadLeft(6, '0');
                _dataRemessa = Convert.ToDecimal(Registro.Substring(143, 8)).ToString("00/00/0000");
                _horaRemessa = Convert.ToDecimal(Registro.Substring(151, 6)).ToString("00:00:00");

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - Header de Arquivo.", ex);
            }
        }

        #endregion
    }
}
