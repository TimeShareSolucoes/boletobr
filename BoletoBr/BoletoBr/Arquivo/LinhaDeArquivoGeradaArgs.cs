using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Arquivo
{
    public enum EnumTipodeLinha
    {
        HeaderDeArquivo = 1,
        HeaderDeLote = 10,
        DetalheSegmentoQ = 21,
        DetalheSegmentoR = 22,
        DetalheSegmentoP = 23,
        TraillerDeLote = 90,
        TraillerDeArquivo = 99
    }

    public class LinhaDeArquivoGeradaArgs : EventArgs
    {
        private readonly string _linha;
        private readonly Boleto _boleto;
        private readonly EnumTipodeLinha _tipoLinha;

        public LinhaDeArquivoGeradaArgs(Boleto boleto, string linha, EnumTipodeLinha tipoLinha)
        {
            try
            {
                _boleto = boleto;
                _linha = linha;
                _tipoLinha = tipoLinha;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto", ex);
            }
        }

        public string Linha
        {
            get { return _linha; }
        }

        public Boleto Boleto
        {
            get { return _boleto; }
        }

        public EnumTipodeLinha TipoLinha
        {
            get { return _tipoLinha; }
        }
    }
}
