using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.CNAB400
{
    public class ArquivoRetornoCNAB400 : IArquivoRetorno
    {
        public IBanco Banco { get; private set; }
        public TipoArquivo TipoArquivo { get; private set; }

        public event EventHandler<LinhaDeArquivoLidaArgs> LinhaDeArquivoLida;

        public List<DetalheRetorno> ListaDetalhe { get; set; }

        #region Construtores

        public ArquivoRetornoCNAB400()
        {
            ListaDetalhe = new List<DetalheRetorno>();
            this.TipoArquivo = TipoArquivo.Cnab400;
        }

        #endregion

        #region M�todos de inst�ncia

        public void LerArquivoRetorno(IBanco banco, Stream arquivo)
        {
            try
            {
                StreamReader stream = new StreamReader(arquivo, System.Text.Encoding.UTF8);
                string linha = "";

                // Lendo o arquivo
                linha = stream.ReadLine();

                // Pr�xima linha (DETALHE)
                linha = stream.ReadLine();

                while (DetalheRetorno.PrimeiroCaracter(linha) == "1")
                {
                    //DetalheRetorno detalhe = banco.LerDetalheRetornoCNAB400(linha);
                    //ListaDetalhe.Add(detalhe);
                    //OnLinhaLida(detalhe, linha);
                    linha = stream.ReadLine();
                }

                stream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler arquivo.", ex);
            }
        }

        #endregion
    }
}
