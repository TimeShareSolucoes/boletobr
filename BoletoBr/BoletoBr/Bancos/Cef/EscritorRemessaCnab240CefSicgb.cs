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
                header = header.PreencherValorNaLinha(18, 18, boleto.CedenteBoleto.CpfCnpj.Length == 11 ?"01" : "02");// Tipo de Inscrição da Empresa                                //Verificar depois 
                header = header.PreencherValorNaLinha(19, 32,   boleto.CedenteBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", ""));// Número de Inscrição da Empresa  //Verificar depois
                header = header.PreencherValorNaLinha(33, 52, "0");// Uso Exclusivo CAIXA
                header = header.PreencherValorNaLinha(53, 57, boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(5, '0'));// Agência Mantedora da Conta
                header = header.PreencherValorNaLinha(58, 58, boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);// Dígito Verificador da Agência                              //Verificar depois                
                header = header.PreencherValorNaLinha(59, 64, boleto.CedenteBoleto.CodigoCedente); // Codigo Convênio
                header = header.PreencherValorNaLinha(65, 71, "0");// Uso Exclusivo CAIXA
                header = header.PreencherValorNaLinha(72, 72, "0");// Uso Exclusivo CAIXA
                header = header.PreencherValorNaLinha(73, 102, boleto.CedenteBoleto.Nome.PadRight(30, ' '));// Nome da Empresa 
                header = header.PreencherValorNaLinha(103, 132, boleto.BancoBoleto.NomeBanco);// Nome do Banco                                                                    //Verificar depois
                header = header.PreencherValorNaLinha(133, 142, "Brancos");// Uso Exclusivo FEBRABAN / CNAB 
                header = header.PreencherValorNaLinha(143, 143, "");// Código Remessa/ Retorno                                                                                   // Não informado
                header = header.PreencherValorNaLinha(144, 151,  DateTime.Now.ToString("d").Replace("/", ""));// Data de Geração do Arquivo
                header = header.PreencherValorNaLinha(152, 157, DateTime.Now.ToString("t").Replace(":", ""));// Hora de Geração do Arquivo                                      //Verificar depois
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
        
        public string EscreverHeaderDeLote(Boleto boleto, int numeroRegistro)
        {
            string headerlote = new string (' ', 240);
            try
            {
                headerlote = headerlote.PreencherValorNaLinha(1, 3, "104");// Código do Banco na Compensação
                headerlote = headerlote.PreencherValorNaLinha(4, 7, "");// Lote de Serviço
                headerlote = headerlote.PreencherValorNaLinha(8, 8, "1");// Tipo de Registro
                headerlote = headerlote.PreencherValorNaLinha(9, 9, "REMESSA".PadRight(7, ' '));//  Tipo de Operação            //Verificar depois
                headerlote = headerlote.PreencherValorNaLinha(10, 11, "");// Tipo de Serviço
                headerlote = headerlote.PreencherValorNaLinha(12, 13, "00");// Uso Exclusivo FREBRABAN/CNAB
                headerlote = headerlote.PreencherValorNaLinha(14, 16, "030");// Nº da versão do Layout do Lote
                headerlote = headerlote.PreencherValorNaLinha(17, 17, "Brancos");// Uso Exclusivo FREBRABAN/CNAB
                headerlote = headerlote.PreencherValorNaLinha(18, 18, boleto.CedenteBoleto.CpfCnpj.Length == 11 ? "01" : "02");// Tipo de Inscrição da Empresa
                headerlote = headerlote.PreencherValorNaLinha(19, 33, boleto.CedenteBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", ""));// Nº de Inscrição da Empresa (CPF/CNPJ)
                headerlote = headerlote.PreencherValorNaLinha(34, 39, "" );// Código do Cedente no Banco
                headerlote = headerlote.PreencherValorNaLinha(40, 53, "0");// Uso Exclusivo da CAIXA
                headerlote = headerlote.PreencherValorNaLinha(54, 58, "");// Agência Mantenedora da Conta
                headerlote = headerlote.PreencherValorNaLinha(59, 59, "");// Dígito Verificador da Conta
                headerlote = headerlote.PreencherValorNaLinha(60, 65, "");// Código do Convênio no Banco
                headerlote = headerlote.PreencherValorNaLinha(66, 72, "");// Código do Modelo Personalizado
                headerlote = headerlote.PreencherValorNaLinha(73, 73, "0");// Uso Exclusivo da CAIXA
                headerlote = headerlote.PreencherValorNaLinha(74, 103, "");// Nome da Empresa
                headerlote = headerlote.PreencherValorNaLinha(104, 143, "");// Mensagem 1
                headerlote = headerlote.PreencherValorNaLinha(144, 183, "");// Mensagem 2
                headerlote = headerlote.PreencherValorNaLinha(184, 191, "");// Número Remessa/Retorno
                headerlote = headerlote.PreencherValorNaLinha(192, 199, "");// Data de Gravação Remessa/Retorno
                headerlote = headerlote.PreencherValorNaLinha(200, 207, "");// Data do Crédito
                headerlote = headerlote.PreencherValorNaLinha(208, 240, "Brancos");// Uso Exclusivo PREBRABAN/CNAB

                return headerlote;
            }
            catch (Exception e)
            {
                throw new Exception(String.Concat("Falha na geração do HEADER do arquivo de REMESSA.",
                             Environment.NewLine, e));
            }
       }
        
        
     
        
        
        
        
                                                                                                             
        /* public string EscreverTrailer( int numeroRegistro )
        {
            string trailer = new string  (' ', 240);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 3, "104");
                trailer = trailer.PreencherValorNaLinha(4, 7, "9999");
                trailer = trailer.PreencherValorNaLinha(8, 8, "9");
                trailer = trailer.PreencherValorNaLinha(9, 17, "Brancos");
                trailer = trailer.PreencherValorNaLinha(18, 23, "");
                trailer = trailer.PreencherValorNaLinha(24, 29, "");
                trailer = trailer.PreencherValorNaLinha(30, 35, "Brancos");
                trailer = trailer.PreencherValorNaLinha(36, 240, "Brancos");

                return trailer;
            }
          
       }*/
        
        
       
        
        
      

    }
}
