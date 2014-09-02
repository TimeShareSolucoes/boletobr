using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Dominio;

namespace BoletoBr.Bancos.Cef
{
    public class EscritorRemessaCnab240CefSicgb : IEscritorArquivoRemessa
    {
        #region Especificações Remessa CAIXA

        /*
         * O controle entre um grupo de segmentos para um mesmo título será peos campos 'Código do Movimento' e 'Número do Registro'.
         * O segmento 'P' é obrigatório;
         * O segmento 'Q' é obrigatório somente para o Código de Movimento '01' (Entrada de Títulos)
         * 
         * Código de Movimento Remessa = 31 (Alteração de Outros Dados)
         * Campos Alteráveis:
         * - Espécie do Título
         * - Aceite
         * - Data de Emissão do Título
         * - Juros
         * - Desconto
         * - Valor do IOF
         * - Abatimento
         * - Código/Prazo Protesto
         * - Código/Prazo Devolução
         * - Dados do Sacado
         * - Dados do Avalista
         * - Multa
         * - Mensagens
         * Obs.: Campos numéricos -> Quando estes campos não precisarem ser alterados devem ser preenchidos com brancos, excepcionalmente,
         * para caracterizar a falta de informação.
         * 
         * Mensagens nos Bloquetos:
         * Desc.: zz.zzz.zzz.zz9,99 até dd/mm/aaaa
         * Abatimento: zz.zzz.zzz.zz9,99
         * Juros: zz.zzz.zzz.zz9,99 ao dia
         * Multa: zz.zzz.zzz.zz9,99 após dd/mm/aaaa
         * Protestar com z9 dias
         * Não receber após z9 dias do vencimento
         */

        #endregion

        private readonly string _sequencial;

        public EscritorRemessaCnab240CefSicgb(string sequencial)
        {
            _sequencial = sequencial;
        }

        public List<string> EscreverArquivo(List<Boleto> boletosEscrever)
        {
            throw new NotImplementedException();
        }

        public void ValidarArquivoRemessa()
        {
        }

        public string EscreverHeader(Boleto boleto, int numeroRegistro)
        {
            var remessa = new Remessa()
            {
                Ambiente = Remessa.EnumTipoAmbiemte.Homologacao,
                CodigoOcorrencia = "01",
                TipoDocumento = ""

            };

            var header = new string(' ', 240);
            try
            {
                header = header.PreencherValorNaLinha(1, 3, "104");
                header = header.PreencherValorNaLinha(4, 7, "0000"); 
                header = header.PreencherValorNaLinha(8, 8, "0");
                header = header.PreencherValorNaLinha(9, 17, string.Empty.PadLeft(9, ' '));
                header = header.PreencherValorNaLinha(18, 18, boleto.CedenteBoleto.CpfCnpj.Length == 11 ? "1" : "2");                          
                header = header.PreencherValorNaLinha(19, 32, boleto.CedenteBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                header = header.PreencherValorNaLinha(33, 52, string.Empty.PadLeft(20, '0'));
                header = header.PreencherValorNaLinha(53, 57, boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(5, '0')); 
                header = header.PreencherValorNaLinha(58, 58, boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);                                      
                header = header.PreencherValorNaLinha(59, 64, boleto.CedenteBoleto.CodigoCedente.PadLeft(6, '0'));
                header = header.PreencherValorNaLinha(65, 71, string.Empty.PadLeft(7, '0'));
                header = header.PreencherValorNaLinha(72, 72, "0"); 
                header = header.PreencherValorNaLinha(73, 102, boleto.CedenteBoleto.Nome.PadRight(30, ' '));
                header = header.PreencherValorNaLinha(103, 132, "CAIXA ECONOMICA FEDERAL".PadRight(30, ' '));                                                         
                header = header.PreencherValorNaLinha(133, 142, string.Empty.PadLeft(10, ' '));

                #region CÓDIGO REMESSA / RETORNO

                /* Código adotado pela FEBRABAN para qualificar o envio ou devoução de arquivo entre Empresa Cliente e o Banco prestador de serviços.
                 * Informar:
                 * 1 - Remessa (Cliente -> Banco)
                 * 2 - Retorno (Banco -> Cliente)
                 * 3 - Remessa Processada (Banco -> Cliente - Pré-Crítica)
                 * 4 - Remessa Processada Parcial (Banco -> Cliente - Pré-Crítica)
                 * 5 - Remessa Rejeitada (Banco -> Cliente - Pré-Crítica)
                 */

                #endregion

                // Código Remessa/Retorno padronizado para "1" no envio do arquivo
                header = header.PreencherValorNaLinha(143, 143, "1");                                                                             
                header = header.PreencherValorNaLinha(144, 151, DateTime.Now.ToString("d").Replace("/", ""));
                header = header.PreencherValorNaLinha(152, 157, DateTime.Now.ToString("T").Replace(":", ""));                                
                header = header.PreencherValorNaLinha(158, 163, numeroRegistro.ToString().PadLeft(6, '0'));
                header = header.PreencherValorNaLinha(164, 166, "050");
                header = header.PreencherValorNaLinha(167, 171, string.Empty.PadLeft(5, '0')); 
                header = header.PreencherValorNaLinha(172, 191, string.Empty.PadRight(20, ' '));
                header = header.PreencherValorNaLinha(192, 211, "REMESSA-TESTE".PadRight(20, ' '));
                header = header.PreencherValorNaLinha(212, 215, string.Empty.PadRight(4, ' '));
                header = header.PreencherValorNaLinha(216, 240, string.Empty.PadRight(25, ' '));

                return header;
            }
            catch (Exception e)
            {
                throw new Exception(String.Concat("Falha na geração do HEADER do arquivo de REMESSA.",
                    Environment.NewLine, e));
            }
        }

        public string EscreverHeaderDeLote(Boleto boleto, int numeroLote, int numeroRegistro)
        {
            #region NOTAS EXPLICATIVAS

            #region 04.1 - G028 - TIPO DE OPERAÇÃO

            /* Tipo de Operação
             * Código adotado pela FEBRABAN para identificar a transação que será realizada com os registros detalhe do lote.
             * Domínio:
             * R = Arquivo Remessa
             * T = Arquivo Retorno
             */

            #endregion

            #region 05.1 - G025 - TIPO DE SERVIÇO

            /* Tipo de Serviço
             * Código adotado pela FEBRABAN para indicar o tipo de serviço / produto (processo) contido no arquivo / lote.
             * Domínio:
             * 01 = Cobrança Registrada
             * 02 = Cobrança Sem Registro / Serviços
             * 03 = Desconto de Títulos
             * 04 = Caução de Títulos
             */

            #endregion

            #endregion

            var headerlote = new string(' ', 240);
            try
            {
                headerlote = headerlote.PreencherValorNaLinha(1, 3, "104"); // Código do Banco na Compensação
                headerlote = headerlote.PreencherValorNaLinha(4, 7, numeroLote.ToString().PadLeft(4, '0')); // Lote de Serviço
                headerlote = headerlote.PreencherValorNaLinha(8, 8, "1"); // Tipo de Registro
                headerlote = headerlote.PreencherValorNaLinha(9, 9, "R");         
                headerlote = headerlote.PreencherValorNaLinha(10, 11, boleto.CarteiraCobranca.Codigo.Equals("RG") ? "01" : "02"); // Tipo de Serviço
                headerlote = headerlote.PreencherValorNaLinha(12, 13, "00"); // Uso Exclusivo FREBRABAN/CNAB
                headerlote = headerlote.PreencherValorNaLinha(14, 16, "030"); // Nº da versão do Layout do Lote
                headerlote = headerlote.PreencherValorNaLinha(17, 17, " "); // Uso Exclusivo FREBRABAN/CNAB
                headerlote = headerlote.PreencherValorNaLinha(18, 18, boleto.CedenteBoleto.CpfCnpj.Length == 11 ? "01" : "02");
                headerlote = headerlote.PreencherValorNaLinha(19, 33, boleto.CedenteBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(15, '0'));
                headerlote = headerlote.PreencherValorNaLinha(34, 39, boleto.CedenteBoleto.CodigoCedente.PadLeft(6, '0'));
                headerlote = headerlote.PreencherValorNaLinha(40, 53, string.Empty.PadLeft(14, '0'));
                headerlote = headerlote.PreencherValorNaLinha(54, 58, boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(5, '0'));
                headerlote = headerlote.PreencherValorNaLinha(59, 59, boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);
                headerlote = headerlote.PreencherValorNaLinha(60, 65, boleto.CedenteBoleto.CodigoCedente.PadLeft(6, '0'));
                headerlote = headerlote.PreencherValorNaLinha(66, 72, string.Empty.PadLeft(7, '0')); // Código do Modelo Personalizado
                headerlote = headerlote.PreencherValorNaLinha(73, 73, "0"); // Uso Exclusivo da CAIXA
                headerlote = headerlote.PreencherValorNaLinha(74, 103, boleto.CedenteBoleto.Nome.PadRight(30, ' '));
                headerlote = headerlote.PreencherValorNaLinha(104, 143, string.Empty.PadRight(40, ' ')); // Mensagem 1
                headerlote = headerlote.PreencherValorNaLinha(144, 183, string.Empty.PadRight(40, ' ')); // Mensagem 2

                // TODO: Analisar como vai controlar o número da remessa
                headerlote = headerlote.PreencherValorNaLinha(184, 191, ""); // Número Remessa/Retorno



                headerlote = headerlote.PreencherValorNaLinha(192, 199, DateTime.Now.ToString("d").Replace("/", ""));
                    // Data de Gravação Remessa/Retorno
                headerlote = headerlote.PreencherValorNaLinha(200, 207, ""); // Data do Crédito
                headerlote = headerlote.PreencherValorNaLinha(208, 240, "".PadRight(40, ' '));
                    // Uso Exclusivo PREBRABAN/CNAB

                return headerlote;
            }
            catch (Exception e)
            {
                throw new Exception(
                    String.Format("<BoletoBr>{0}Falha na geração do HEADER DE LOTE do arquivo de REMESSA.",
                        Environment.NewLine), e);
            }
        }

        public string EscreverDetalheSegmentoP(Boleto boleto, int numeroLote, int numeroRegistro)
        {
            #region Variável

            string carteiraCob = boleto.CarteiraCobranca.Codigo.PadLeft(3, ' ');
            boleto.TipoModalidade = boleto.NossoNumeroFormatado.Replace("0", "").Substring(0, 2);

            #endregion

            var detalhe = new string(' ', 240);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 3, "104"); // Código do Banco na Compensação
                detalhe = detalhe.PreencherValorNaLinha(4, 7, numeroLote.ToString().PadLeft(4, '0')); // Lote De Serviço
                detalhe = detalhe.PreencherValorNaLinha(8, 8, "3"); // Tipo de Registro
                detalhe = detalhe.PreencherValorNaLinha(9, 13, numeroRegistro.ToString().PadLeft(5, '0'));
                    // Nº Sequencial do Registro no Lote   
                detalhe = detalhe.PreencherValorNaLinha(14, 14, "P"); // Cód. Segmento do Registro Detalhe
                detalhe = detalhe.PreencherValorNaLinha(15, 15, string.Empty.PadLeft(1, ' '));
                    // Uso Exclusivo FEBRABAN/CNAB
                // Padronizado para 01 - Entrada de Título
                detalhe = detalhe.PreencherValorNaLinha(16, 17, "01"); // Código de Movimento Remessa
                detalhe = detalhe.PreencherValorNaLinha(18, 22,
                    boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(5, ' ')); // Agência Mantenedora da Conta
                detalhe = detalhe.PreencherValorNaLinha(23, 23, boleto.CedenteBoleto.ContaBancariaCedente.DigitoAgencia);
                    // Dígito Verificador da Agência
                detalhe = detalhe.PreencherValorNaLinha(24, 29, boleto.CedenteBoleto.CodigoCedente.PadLeft(6, '0'));
                    // Código do Convênio no Banco             
                detalhe = detalhe.PreencherValorNaLinha(30, 37, "0"); // Uso Exclusivo CAIXA
                detalhe = detalhe.PreencherValorNaLinha(38, 40, "0"); // Uso Exclusivo CAIXA
                detalhe = detalhe.PreencherValorNaLinha(41, 42, boleto.TipoModalidade); // Modalidade da Carteira
                detalhe = detalhe.PreencherValorNaLinha(43, 57, boleto.NossoNumeroFormatado.PadLeft(15, '0'));
                    // Identificaçao do Título no Banco
                detalhe = detalhe.PreencherValorNaLinha(58, 58, "1"); // Forma de Cadastr. do Título no Banco
                // 1 - Cobrança Registrada ou 2 - Cobrança Sem Registro
                detalhe = detalhe.PreencherValorNaLinha(59, 59,
                    boleto.CarteiraCobranca.TipoRegistro.Equals(true) ? "1" : "2");
                    // Forma de Cadastr. do Título no Banco
                // Fixo 2 - Escritural
                detalhe = detalhe.PreencherValorNaLinha(60, 60, "2"); // Tipo de Documento
                detalhe = detalhe.PreencherValorNaLinha(61, 61, "2"); // Identificação da Emissão do Bloqueto
                detalhe = detalhe.PreencherValorNaLinha(62, 62, "3"); // Identificação da Entrega do Bloqueto
                detalhe = detalhe.PreencherValorNaLinha(63, 73, boleto.NumeroDocumento.PadLeft(11, '0'));
                    // Número do Documento de Cobrança
                detalhe = detalhe.PreencherValorNaLinha(74, 77, string.Empty.PadLeft(4, ' ')); // Uso Exclusivo CAIXA
                detalhe = detalhe.PreencherValorNaLinha(78, 85, boleto.DataVencimento.ToString("d").Replace("/", ""));
                    // Data de Vencimento do Título
                detalhe = detalhe.PreencherValorNaLinha(86, 100,
                    boleto.ValorBoleto.ToString().Replace(".", "").Replace(",", "")); // Valor Nominal do Título
                detalhe = detalhe.PreencherValorNaLinha(101, 105, string.Empty.PadLeft(5, '0'));
                    // Agência Encarregada da Cobrança
                detalhe = detalhe.PreencherValorNaLinha(106, 106, "0"); // Dígito Verificador da Agência
                detalhe = detalhe.PreencherValorNaLinha(107, 108,
                    boleto.Especie.Sigla.Equals("DM") ? "02" : boleto.Especie.Codigo.ToString()); // Espécia do Título 
                detalhe = detalhe.PreencherValorNaLinha(109, 109, boleto.Aceite.Equals("N") ? "N" : "S");
                    // Identific. de Título Aceito/Não Aceito
                detalhe = detalhe.PreencherValorNaLinha(110, 117, DateTime.Now.ToString("d").Replace("/", ""));
                    // Data da Emissão de Título
                /* Modalidade de cobrança de juros de mora
                 * 1 - Valor por dia
                 * 2 - Taxa Mensal
                 * 3 - Isento
                 */
                detalhe = detalhe.PreencherValorNaLinha(118, 118, "3"); // Código do Juros de Mora
                detalhe = detalhe.PreencherValorNaLinha(119, 126, boleto.DataJurosMora.ToString("d").Replace("/", ""));
                    // Data do Juros de Mora
                detalhe = detalhe.PreencherValorNaLinha(127, 141,
                    string.Format("{0:0.##}", boleto.JurosMora).PadLeft(13, '0')); // Juros de Mora por Dia/Taxa
                /* Código do Desconto
                 * 0 - Sem desconto
                 * 1 - Valor fixo até a data informada
                 * 2 - Percentual até a data informada
                 * Obs.: Para os códigos '1' e '2' será obrigatório a informação da data.
                 */
                detalhe = detalhe.PreencherValorNaLinha(142, 142, ""); // Código do Desconto 1
                detalhe = detalhe.PreencherValorNaLinha(143, 150, boleto.DataDesconto.ToString("d").Replace("/", ""));
                    // Data do Desconto 1
                detalhe = detalhe.PreencherValorNaLinha(151, 165,
                    string.Format("{0:0.##}", boleto.ValorDesconto).PadLeft(13, '0'));
                    // Valor/Percentual a ser Concedido
                detalhe = detalhe.PreencherValorNaLinha(166, 180, string.Format("{0:0.##}", boleto.Iof).PadLeft(13, '0'));
                    // Valor do IOF a ser Recolhido 
                detalhe = detalhe.PreencherValorNaLinha(181, 195,
                    string.Format("{0:0.##}", boleto.ValorAbatimento).PadLeft(13, '0')); // Valor do Abatimento
                detalhe = detalhe.PreencherValorNaLinha(196, 220, boleto.NumeroDocumento.PadLeft(25, '0'));
                    // Identificação do Título na Empresa

                #region CÓDIGO PROTESTO

                /* Código para Protesto
                 * Código adotado pela FEBRABAN para identificar o tipo de prazo a ser considerado para protesto.
                 * 1 - Protestar
                 * 3 - Não Protestar
                 * 9 - Cancelamento Protesto Automático
                 * (Somente válido p/ Código Movimento Remessa = '31' - Alteração de Outros Dados)
                 */

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(221, 221, "3"); // Código para Protesto
                detalhe = detalhe.PreencherValorNaLinha(222, 223, "5"); // Número de Dias para Protesto
                detalhe = detalhe.PreencherValorNaLinha(224, 224, "1"); // Código para Baixa/Devolução
                detalhe = detalhe.PreencherValorNaLinha(225, 227, "20"); // Número de Dias para Baixa/Devolução 
                detalhe = detalhe.PreencherValorNaLinha(228, 229, "09"); // Código da Moeda
                detalhe = detalhe.PreencherValorNaLinha(230, 239, "0"); // Uso Exclusivo CAIXA
                detalhe = detalhe.PreencherValorNaLinha(240, 240, string.Empty.PadLeft(1, ' '));
                    // Uso Exclusivo FREBRABAN/CNAB

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("<BoletoBr>{0}Falha na geração do DETALHE - Segmento T do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverDetalheSegQ(Boleto boleto, int numeroLote, int numeroRegistro)
        {
           string enderecoSacado = boleto.SacadoBoleto.EnderecoSacado.TipoLogradouro + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Logradouro + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Numero + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Complemento.PadRight(40, ' ');

            var detalhe = new string(' ', 240);

            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 3, "104");
                detalhe = detalhe.PreencherValorNaLinha(4, 7, numeroLote.ToString().PadLeft(4, '0'));
                detalhe = detalhe.PreencherValorNaLinha(8, 8, "3");
                detalhe = detalhe.PreencherValorNaLinha(9, 13, numeroRegistro.ToString().PadLeft(5, '0'));
                detalhe = detalhe.PreencherValorNaLinha(14, 14, "Q");
                detalhe = detalhe.PreencherValorNaLinha(15, 15, string.Empty.PadLeft(1, ' '));
                detalhe = detalhe.PreencherValorNaLinha(16, 17, "01");
                detalhe = detalhe.PreencherValorNaLinha(18, 18, boleto.SacadoBoleto.CpfCnpj.Length == 11 ? "01" : "02");
                detalhe = detalhe.PreencherValorNaLinha(19, 33, boleto.SacadoBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(15, '0'));
                detalhe = detalhe.PreencherValorNaLinha(34, 73, boleto.SacadoBoleto.Nome.PadRight(40, ' '));
                detalhe = detalhe.PreencherValorNaLinha(74, 128, enderecoSacado);
                detalhe = detalhe.PreencherValorNaLinha(129, 136, boleto.SacadoBoleto.EnderecoSacado.Cep.PadLeft(8, '0'));
                detalhe = detalhe.PreencherValorNaLinha(137, 151, boleto.SacadoBoleto.EnderecoSacado.Cidade.PadRight(15, ' '));
                detalhe = detalhe.PreencherValorNaLinha(152, 153, boleto.SacadoBoleto.EnderecoSacado.SiglaUf);
                detalhe = detalhe.PreencherValorNaLinha(154, 154, boleto.SacadoBoleto.CpfCnpjAvalista.Length == 11 ? "01" : "02");
                detalhe = detalhe.PreencherValorNaLinha(155, 169, boleto.SacadoBoleto.CpfCnpjAvalista.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(15, '0'));
                detalhe = detalhe.PreencherValorNaLinha(170, 209, boleto.SacadoBoleto.NomeAvalista.PadRight(40, ' '));
                detalhe = detalhe.PreencherValorNaLinha(210, 212, string.Empty.PadLeft(3, ' '));
                detalhe = detalhe.PreencherValorNaLinha(213, 232, string.Empty.PadLeft(20, ' '));
                detalhe = detalhe.PreencherValorNaLinha(233, 240, string.Empty.PadLeft(8, ' '));

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("<BoletoBr>{0}Falha na geração do DETALHE - Segmento Q do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }

        }

        public string EscreverTrailer(int qtdLotes, int qtdRegistros)
        {
            var trailer = new string(' ', 240);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 3, "104");
                trailer = trailer.PreencherValorNaLinha(4, 7, "9999");
                trailer = trailer.PreencherValorNaLinha(8, 8, "9");
                trailer = trailer.PreencherValorNaLinha(9, 17, string.Empty.PadLeft(9, ' '));
                trailer = trailer.PreencherValorNaLinha(18, 23, qtdLotes.ToString().PadLeft(6, ' '));
                trailer = trailer.PreencherValorNaLinha(24, 29, qtdRegistros.ToString().PadLeft(6, ' '));
                trailer = trailer.PreencherValorNaLinha(30, 35, string.Empty.PadLeft(6, ' '));
                trailer = trailer.PreencherValorNaLinha(36, 240, string.Empty.PadLeft(205, ' '));

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("<BoletoBr>{0}Falha na geração do TRAILER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }
    }
}
