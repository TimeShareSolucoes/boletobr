using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Dominio
{
    public enum EnumTipodeLinhaLida
    {
        HeaderDeArquivo = 1,
        HeaderDeLote = 10,
        Detalhe = 20,
      //DetalheSegmentoQ = 21,
        DetalheSegmentoW = 22,
        DetalheSegmentoE = 23,
      //DetalheSegmentoR = 24,
        DetalheSegmentoT = 25,
      //DetalheSegmentoY = 26,
        DetalheSegmentoU = 27,
        TraillerDeLote = 90,
        TraillerDeArquivo = 99,
        
        
    }

    public class LinhaDeArquivoLidaArgs : EventArgs
    {
        private readonly string _linha;
        private readonly object _detalhe;
        private readonly EnumTipodeLinhaLida _tipoLinha;

        public LinhaDeArquivoLidaArgs(object detalhe, string linha)
        {
            try
            {
                _linha = linha;
                _detalhe = detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto", ex);
            }
        }

        public LinhaDeArquivoLidaArgs(object detalhe, string linha, EnumTipodeLinhaLida tipoLinha)
        {
            try
            {
                _linha = linha;
                _detalhe = detalhe;
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

        public object Detalhe
        {
            get { return _detalhe; }
        }

        public EnumTipodeLinhaLida TipoLinha
        {
            get { return _tipoLinha; }
        }
    }
}
