using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Cef
{
    public class EscritorRemessaCnab240CefSicgb : IEscritorArquivoRemessa
    {
        private readonly string _sequencial;

        public EscritorRemessaCnab240CefSicgb(string sequencial)
        {
            _sequencial = sequencial;
        }

        public List<string> EscreverArquivo(List<Boleto> boletosEscrever)
        {
            throw new NotImplementedException();
        }

        public void ValidarArquivoRemessa(){}
        
        public string EscreverHeader(Boleto boleto, int numeroRegistro)
        {
            string header = new string (' ', 240);
            try
            {
                header = header.PreencherValorNaLinha(1, 3, "104");// Código do Banco na Compensação 
                header = header.PreencherValorNaLinha(4, 7, "0000");// Lote de Serviço
                header = header.PreencherValorNaLinha(8, 8, "0");// Tipo de Registro
                header = header.PreencherValorNaLinha(9, 17, "");// Uso Exclusivo FEBRABAN / CNAB
                header = header.PreencherValorNaLinha(18, 18, "");// Tipo de Inscrição da Empresa 
                header = header.PreencherValorNaLinha(19, 32,   boleto.CedenteBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", ""));// Número de Inscrição da Empresa  
                header = header.PreencherValorNaLinha(33, 52, "0");// Uso Exclusivo CAIXA
                header = header.PreencherValorNaLinha(53, 57, "");// Agência Mantedora da Conta
                header = header.PreencherValorNaLinha(58, 58, "");// Dígito Verificador da Agência
                header = header.PreencherValorNaLinha(59, 64, boleto.CedenteBoleto.CodigoCedente); // Codigo Convênio
                header = header.PreencherValorNaLinha(65, 71, "0");// Uso Exclusivo CAIXA
                header = header.PreencherValorNaLinha(72, 72, "0");// Uso Exclusivo CAIXA
                header = header.PreencherValorNaLinha(73, 102, "");// Nome da Empresa 
                header = header.PreencherValorNaLinha(103, 132, boleto.BancoBoleto.NomeBanco);// Nome do Banco
                header = header.PreencherValorNaLinha(133, 142, "Brancos");// Uso Exclusivo FEBRABAN / CNAB 
                header = header.PreencherValorNaLinha(143, 143, "");// Código Remessa/ Retorno
                header = header.PreencherValorNaLinha(144, 151, "");// Data de Geração do Arquivo
                header = header.PreencherValorNaLinha(152, 157, "");// Hora de Geração do Arquivo
                header = header.PreencherValorNaLinha(158, 163, boleto.CedenteBoleto.NumeroSequencial);// Número Seqüencial do Arquivo
                header = header.PreencherValorNaLinha(164, 166, "050");// Nº da Versão do Layout do Arquivo
                header = header.PreencherValorNaLinha(167, 171, "0");// Densidade de Gravação do Arquivo
                header = header.PreencherValorNaLinha(172, 191, "");// Para uso reservado do Banco
                header = header.PreencherValorNaLinha(192, 211, "");// Para uso reservado da Empresa
                header = header.PreencherValorNaLinha(212, 215, "");// Versão Aplicativo CAIXA
                header = header.PreencherValorNaLinha(216, 240, "Brancos");// Uso Exclusivo FEBRABAN / CNAB 

                return header;
            }
            catch (Exception e)
            {
                throw new Exception(String.Concat("Falha na geração do HEADER do arquivo de REMESSA.",
                    Environment.NewLine, e));
            }
        }
      /* public string EscreverDetalhe(Boleto boleto, int numeroRegistro)
        {   
          string 
        }*/

    }
}
