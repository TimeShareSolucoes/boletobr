using System;
using System.Collections.Generic;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos
{
    public class EscritorArquivoRemessaGenericoCnab240 : IEscritorArquivoRemessaCnab240
    {
        public List<string> EscreverArquivo(List<Boleto> boletosEscrever)
        {
            foreach (var boleto in boletosEscrever)
            {
                #region #Validações

                if (boleto.ValorBoleto <= 0)
                    throw new Exception("O valor do boleto não pode ser menor que 0 (zero) ou zerado");

                if (boleto.NumeroDocumento.TrimStart('0') == String.Empty)
                    throw new Exception("Não há número do documento informado no boleto para o sacado " + boleto.SacadoBoleto.Nome + "(" + boleto.SacadoBoleto.CpfCnpj + ")");
                
                if (boleto.CedenteBoleto == null)
                    throw new Exception("Não há cedente informado para o boleto " + boleto.NumeroDocumento);

                if (boleto.SacadoBoleto == null)
                    throw  new Exception("Não há sacado informado para o boleto " + boleto.NumeroDocumento);

                if (boleto.CarteiraCobranca == null)
                    throw new Exception("Não há carteira de cobrança informada para o boleto " + boleto.NumeroDocumento);

                if (boleto.BancoBoleto == null)
                    throw new Exception("Não há banco informado para o boleto " + boleto.NumeroDocumento);

                #endregion
            }

            throw new NotImplementedException();
        }

        public void ValidarArquivoRemessa(Cedente cedente, List<Boleto> boletos, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        public RemessaCnab240 ProcessarRemessaCnab240()
        {
            throw new NotImplementedException();
        }

        public void ValidaArquivoRemessa()
        {
            throw new NotImplementedException();
        }

        public RemessaCnab240 ProcessarRemessaCnab400()
        {
            throw new NotImplementedException();
        }

        public void ValidaArquivoRetorno()
        {
            throw new NotImplementedException();
        }

        public HeaderRemessaCnab240 EscreverHeader(string linha)
        {
            throw new NotImplementedException();
        }

        public HeaderLoteRemessaCnab240 EscreverHeaderLote(string linha)
        {
            throw new NotImplementedException();
        }

        public DetalheSegmentoPRemessaCnab240 EscreverDetalheSegmentoP(string linha)
        {
            throw new NotImplementedException();
        }

        public DetalheSegmentoQRemessaCnab240 EscreverDetalheSegmentoQ(string linha)
        {
            throw new NotImplementedException();
        }

        public TrailerLoteRemessaCnab240 EscreverTrailerLote(string linha)
        {
            throw new NotImplementedException();
        }

        public TrailerRemessaCnab240 EscreverTrailer(string linha)
        {
            throw new NotImplementedException();
        }
    }
}
