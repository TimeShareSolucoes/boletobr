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
                header = header.PreencherValorNaLinha(53, 57, boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(5, '0'));// Agência Mantenedora da Conta
                header = header.PreencherValorNaLinha(58, 58, boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);// Dígito Verificador da Agência                              //Verificar depois                
                header = header.PreencherValorNaLinha(59, 64, boleto.CedenteBoleto.CodigoCedente); // Codigo Convênio
                header = header.PreencherValorNaLinha(65, 71, "0");// Uso Exclusivo CAIXA
                header = header.PreencherValorNaLinha(72, 72, "0");// Uso Exclusivo CAIXA
                header = header.PreencherValorNaLinha(73, 102, boleto.CedenteBoleto.Nome.PadRight(30, ' '));// Nome da Empresa 
                header = header.PreencherValorNaLinha(103, 132, boleto.BancoBoleto.NomeBanco);// Nome do Banco                                                                    //Verificar depois
                header = header.PreencherValorNaLinha(133, 142, "");// Uso Exclusivo FEBRABAN / CNAB 
                 header = header.PreencherValorNaLinha(143, 143, "");// Código Remessa/ Retorno                                                                                   // Não informado
                header = header.PreencherValorNaLinha(144, 151,  DateTime.Now.ToString("d").Replace("/", ""));// Data de Geração do Arquivo
                header = header.PreencherValorNaLinha(152, 157, DateTime.Now.ToString("t").Replace("/", ""));// Hora de Geração do Arquivo                                      //Verificar depois
                header = header.PreencherValorNaLinha(158, 163, boleto.CedenteBoleto.NumeroSequencial);// Número Seqüencial do Arquivo
                header = header.PreencherValorNaLinha(164, 166, "050");// Nº da Versão do Layout do Arquivo
                header = header.PreencherValorNaLinha(167, 171, "0");// Densidade de Gravação do Arquivo
                header = header.PreencherValorNaLinha(172, 191, "");// Para uso reservado do Banco
                header = header.PreencherValorNaLinha(192, 211, "");// Para uso reservado da Empresa
                 header = header.PreencherValorNaLinha(212, 215, "");// Versão Aplicativo CAIXA
                header = header.PreencherValorNaLinha(216, 240, "");// Uso Exclusivo FEBRABAN / CNAB 
                
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
                headerlote = headerlote.PreencherValorNaLinha(9, 9, "".PadRight(7, ' '));//  Tipo de Operação            //Verificar depois
                 headerlote = headerlote.PreencherValorNaLinha(10, 11, "");// Tipo de Serviço
                headerlote = headerlote.PreencherValorNaLinha(12, 13, "00");// Uso Exclusivo FREBRABAN/CNAB
                headerlote = headerlote.PreencherValorNaLinha(14, 16, "030");// Nº da versão do Layout do Lote
                headerlote = headerlote.PreencherValorNaLinha(17, 17, " ");// Uso Exclusivo FREBRABAN/CNAB
                headerlote = headerlote.PreencherValorNaLinha(18, 18, boleto.CedenteBoleto.CpfCnpj.Length == 11 ? "01" : "02");// Tipo de Inscrição da Empresa
                headerlote = headerlote.PreencherValorNaLinha(19, 33, boleto.CedenteBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", ""));// Nº de Inscrição da Empresa (CPF/CNPJ)
                headerlote = headerlote.PreencherValorNaLinha(34, 39,  boleto.CedenteBoleto.CodigoCedente);// Código do Cedente no Banco
                headerlote = headerlote.PreencherValorNaLinha(40, 53, "0");// Uso Exclusivo da CAIXA
                headerlote = headerlote.PreencherValorNaLinha(54, 58, boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(5, '0'));// Agência Mantenedora da Conta 
                headerlote = headerlote.PreencherValorNaLinha(59, 59, boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);// Dígito Verificador da Conta
                headerlote = headerlote.PreencherValorNaLinha(60, 65, boleto.BancoBoleto.CodigoBanco);// Código do Convênio no Banco
                 headerlote = headerlote.PreencherValorNaLinha(66, 72, "");// Código do Modelo Personalizado
                headerlote = headerlote.PreencherValorNaLinha(73, 73, "0");// Uso Exclusivo da CAIXA
                headerlote = headerlote.PreencherValorNaLinha(74, 103, boleto.CedenteBoleto.Nome.PadRight(30, ' '));// Nome da Empresa
                headerlote = headerlote.PreencherValorNaLinha(104, 143, "".PadRight(40,' '));// Mensagem 1
                headerlote = headerlote.PreencherValorNaLinha(144, 183, "".PadRight(40,' '));// Mensagem 2
                 headerlote = headerlote.PreencherValorNaLinha(184, 191, "");// Número Remessa/Retorno
                headerlote = headerlote.PreencherValorNaLinha(192, 199, DateTime.Now.ToString("d").Replace("/", ""));// Data de Gravação Remessa/Retorno
                 headerlote = headerlote.PreencherValorNaLinha(200, 207, "");// Data do Crédito
                headerlote = headerlote.PreencherValorNaLinha(208, 240, "".PadRight(40, ' '));// Uso Exclusivo PREBRABAN/CNAB

                return headerlote;                           
            }
            catch (Exception e)
            {
                throw new Exception(String.Concat("Falha na geração do HEADER do arquivo de REMESSA.",
                             Environment.NewLine, e));
            }
       }
        
        public string EscreverDetalhe(Boleto boleto, int numeroRegistro)
        {
            #region Variável
            string carteiraCob = boleto.CarteiraCobranca.Codigo.PadLeft(3, ' ');
     
            #endregion

            string detalhe = new string(' ', 240);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 3, "104");// Código do Banco na Compensação
                 detalhe = detalhe.PreencherValorNaLinha(4, 7, "");// Lote De Serviço
                detalhe = detalhe.PreencherValorNaLinha(8, 8, "3");// Tipo de Registro
                detalhe = detalhe.PreencherValorNaLinha(9, 13, boleto.CedenteBoleto.NumeroSequencial);// Nº Sequencial do Registro no Lote       // Verificar depois
                detalhe = detalhe.PreencherValorNaLinha(14, 14, "P");// Cód. Segmento do Registro Detalhe
                detalhe = detalhe.PreencherValorNaLinha(15, 15, "");// Uso Exclusivo FEBRABAN/CNAB
                 detalhe = detalhe.PreencherValorNaLinha(16, 17, "");// Código de Movimento Remessa
                detalhe = detalhe.PreencherValorNaLinha(18, 22, boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(5, ' '));// Agência Mantenedora da Conta
                detalhe = detalhe.PreencherValorNaLinha(23, 23, boleto.CedenteBoleto.ContaBancariaCedente.DigitoAgencia);// Dígito Verificador da Agência
                detalhe = detalhe.PreencherValorNaLinha(24, 29, boleto.BancoBoleto.CodigoBanco);// Código do Convênio no Banco        // Verificar depois         
                detalhe = detalhe.PreencherValorNaLinha(30, 37, "0");// Uso Exclusivo CAIXA
                detalhe = detalhe.PreencherValorNaLinha(38, 40, "0");// Uso Exclusivo CAIXA
                detalhe = detalhe.PreencherValorNaLinha(41, 42, boleto.TipoModalidade);// Modalidade da Carteira
                detalhe = detalhe.PreencherValorNaLinha(43, 57, boleto.NossoNumeroFormatado.PadLeft(15, ' '));// Identificaçao do Título no Banco
                            /*Código da Carteira*/
                // Modalidade de Carteira D - Direta
                if (carteiraCob == "58")           // Verificar e corrigir depois
                    detalhe = detalhe.PreencherValorNaLinha(58, 58, "D");
                // Modalidade de Carteira S - Sem Registro
                if (carteiraCob == "" || carteiraCob == "" || carteiraCob == "")         // Verificar e corrigir depois
                    detalhe = detalhe.PreencherValorNaLinha(58, 58, "S");
                // Modalidade de Carteira E - Escritural 
                if (carteiraCob == "" || carteiraCob == "" || carteiraCob == "" || carteiraCob == "")        // Verificar e corrigir depois
                    detalhe = detalhe.PreencherValorNaLinha(58, 58, "E"); 
                             
                 detalhe = detalhe.PreencherValorNaLinha(59, 59, "");// Forma de Cadastr. do Título no Banco
                detalhe = detalhe.PreencherValorNaLinha(60, 60, "2");// Tipo de Documento
                 detalhe = detalhe.PreencherValorNaLinha(61, 61, "");// Identificação da Emissão do Bloqueto
                 detalhe = detalhe.PreencherValorNaLinha(62, 62, "");// Identificação da Entrega do Bloqueto
                detalhe = detalhe.PreencherValorNaLinha(63, 73, boleto.NumeroDocumento.PadLeft(11, ' '));// Número do Documento de Cobrança
                detalhe = detalhe.PreencherValorNaLinha(74, 77, "");// Uso Exclusivo CAIXA
                detalhe = detalhe.PreencherValorNaLinha(78, 85, boleto.DataVencimento.ToString("d").Replace("/", ""));// Data de Vencimento do Título
                detalhe = detalhe.PreencherValorNaLinha(86, 100, "");// Valor Nominal do Título
                detalhe = detalhe.PreencherValorNaLinha(101, 105, "0");// Agência Encarregada da Cobrança
                detalhe = detalhe.PreencherValorNaLinha(106, 106, "0");// Dígito Verificador da Agência
                detalhe = detalhe.PreencherValorNaLinha(107, 108, boleto.Especie.Especie.PadLeft(2, ' '));// Espécia do Título       // Verificar depois
                detalhe = detalhe.PreencherValorNaLinha(109, 109, boleto.Aceite);// Identific. de Título Aceito/Não Aceito
                detalhe = detalhe.PreencherValorNaLinha(110, 117, DateTime.Now.ToString("d").Replace("/", ""));// Data da Emissão de Título
                detalhe = detalhe.PreencherValorNaLinha(118, 118, "");// Código do Juros de Mora
                detalhe = detalhe.PreencherValorNaLinha(119, 126, boleto.DataJurosMora.ToString("d").Replace("/", ""));// Data do Juros de Mora        // Verificar depois
                detalhe = detalhe.PreencherValorNaLinha(127, 141, string.Format("{0:0.##}", boleto.JurosMora).PadLeft(13,'0'));// Juros de Mora por Dia/Taxa       // Verificar depois
                detalhe = detalhe.PreencherValorNaLinha(142, 142, "");// Código do Desconto 1
                detalhe = detalhe.PreencherValorNaLinha(143, 150, boleto.DataDesconto.ToString("d").Replace("/", ""));// Data do Desconto 1          // Verificar depois
                detalhe = detalhe.PreencherValorNaLinha(151, 165, string.Format("{0:0.##}", boleto.ValorDesconto).PadLeft(13,'0'));// Valor/Percentual a ser Concedido      // Verificar depois
                detalhe = detalhe.PreencherValorNaLinha(166, 180, string.Format("{0:0.##}", boleto.Iof).PadLeft(13, '0'));// Valor do IOF a ser Recolhido 
                detalhe = detalhe.PreencherValorNaLinha(181, 195, string.Format("{0:0.##}", boleto.ValorAbatimento).PadLeft(13, '0'));// Valor do Abatimento
                detalhe = detalhe.PreencherValorNaLinha(196, 220, boleto.NumeroDocumento.PadLeft(25, ' '));// Identificação do Título na Empresa
                 detalhe = detalhe.PreencherValorNaLinha(221, 221, "");// Código para Protesto
                 detalhe = detalhe.PreencherValorNaLinha(222, 223, "");// Número de Dias para Protesto
                 detalhe = detalhe.PreencherValorNaLinha(224, 224, "");// Código para Baixa/Devolução
                 detalhe = detalhe.PreencherValorNaLinha(225, 227, "");// Número de Dias para Baixa/Devolução 
                 detalhe = detalhe.PreencherValorNaLinha(228, 229, "");// Código da Moeda
                detalhe = detalhe.PreencherValorNaLinha(230, 239, "0");// Uso Exclusivo CAIXA
                detalhe = detalhe.PreencherValorNaLinha(240, 240, "");// Uso Exclusivo FREBRABAN/CNAB

                return detalhe;
            }
            catch(Exception e)
            
           {
        throw new Exception(String.Concat("Falha na geração do DETALHE do arquivo de REMESSA.",
            Environment.NewLine, e));
        
            }

        }

        public string EscreverDetalheSegQ(Boleto boleto, int numeroRegistro)
        {
            string detalheQ = new string(' ', 240);
            try
      {         detalheQ = detalheQ.PreencherValorNaLinha(1, 3, "104");
                detalheQ = detalheQ.PreencherValorNaLinha(4, 7, "");
                detalheQ = detalheQ.PreencherValorNaLinha(8, 8, "3");
                detalheQ = detalheQ.PreencherValorNaLinha(9, 13, "");
                detalheQ = detalheQ.PreencherValorNaLinha(14, 14, "Q");
                detalheQ = detalheQ.PreencherValorNaLinha(15, 15, "");
                
        }
     
        
        
        
        
                                                                                                             
        /* public string EscreverTrailer( int numeroRegistro )
        {
            string trailer = new string  (' ', 240);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 3, "104");
                trailer = trailer.PreencherValorNaLinha(4, 7, "9999");
                trailer = trailer.PreencherValorNaLinha(8, 8, "9");
                trailer = trailer.PreencherValorNaLinha(9, 17, "");
                trailer = trailer.PreencherValorNaLinha(18, 23, "");
                trailer = trailer.PreencherValorNaLinha(24, 29, "");
                trailer = trailer.PreencherValorNaLinha(30, 35, "");
                trailer = trailer.PreencherValorNaLinha(36, 240, "");

                return trailer;
            }
          
       }*/
        
        
       
        
        
      

    }
}
