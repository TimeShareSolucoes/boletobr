using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BoletoBr.Arquivo.CNAB400.Retorno;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Fabricas;

namespace BoletoBr.Bancos.Itau
{
    public class EscritorRemessaCnab400Itau : IEscritorArquivoRemessa
    {
        public List<string> EscreverArquivo(List<Boleto> boletosEscrever)
        {
            throw new NotImplementedException();
        }

        public void ValidarArquivoRemessa() { }

        public string EscreverHeader(Boleto boleto, int numeroRegistro)
        {
            var header = new string(' ', 400);
            try
            {
                header = header.PreencherValorNaLinha(1, 1, "0");
                header = header.PreencherValorNaLinha(2, 2, "1");
                header = header.PreencherValorNaLinha(3, 9, "REMESSA".PadRight(7, ' '));
                header = header.PreencherValorNaLinha(10, 11, "01");
                header = header.PreencherValorNaLinha(12, 26, "COBRANCA".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(27, 30,
                    boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'));
                header = header.PreencherValorNaLinha(31, 32, "00");
                header = header.PreencherValorNaLinha(33, 37,
                    boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(5, '0'));
                header = header.PreencherValorNaLinha(38, 38, boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);
                header = header.PreencherValorNaLinha(39, 46, string.Empty.PadRight(8, ' '));
                header = header.PreencherValorNaLinha(47, 76, boleto.CedenteBoleto.Nome.PadRight(30, ' '));
                header = header.PreencherValorNaLinha(77, 79, "341");
                header = header.PreencherValorNaLinha(80, 94, "BANCO ITAU SA");
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("d").Replace("/", ""));
                header = header.PreencherValorNaLinha(101, 394, string.Empty.PadRight(294, ' '));
                header = header.PreencherValorNaLinha(395, 400, numeroRegistro.ToString().PadLeft(6, '0'));

                return header;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("BoletoBr{0}Falha na geração do HEADER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverDetalhe(Boleto boleto, int numeroRegistro)
        {
            #region Variáveis
            string carteiraCob = boleto.CarteiraCobranca.Codigo.PadLeft(3, ' ');
            string enderecoSacado = boleto.SacadoBoleto.EnderecoSacado.TipoLogradouro + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Logradouro + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Numero + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Complemento.PadRight(40, ' ');
            #endregion

            var detalhe = new string(' ', 400);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, "1"); // Identificação do Registro Transação
                detalhe = detalhe.PreencherValorNaLinha(2, 3, boleto.CedenteBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11 ? "01" : "02"); // Tipo de Inscrição da Empresa
                detalhe = detalhe.PreencherValorNaLinha(4, 17, boleto.CedenteBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "")); // Nro de Inscrição da Empresa (CPF/CNPJ)
                detalhe = detalhe.PreencherValorNaLinha(18, 21, boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0')); // Agência Mantenedora da Conta
                detalhe = detalhe.PreencherValorNaLinha(22, 23, string.Empty.PadRight(2, '0')); // Complemento de Registro
                detalhe = detalhe.PreencherValorNaLinha(24, 28, boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(5, '0')); // Nro da Conta Corrente da Empresa
                detalhe = detalhe.PreencherValorNaLinha(29, 29, boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta); // Dígito de Auto Conferência Ag/Conta Empresa
                detalhe = detalhe.PreencherValorNaLinha(30, 33, string.Empty.PadRight(4, ' ')); // Complemento de Registro
                //detalhe = boleto.InstrucoesDoBoleto; // Cód. Instrução/Alegação a ser cancelada
                detalhe = detalhe.PreencherValorNaLinha(38, 62, boleto.NumeroDocumento.PadLeft(25, '0')); // Identificação do Título na Empresa
                detalhe = detalhe.PreencherValorNaLinha(63, 70, boleto.NossoNumeroFormatado.PadLeft(8, '0')); // Identificação do Título no Banco
                // Se Moeda = REAL, preenche com zeros
                if (boleto.BancoBoleto.MoedaBanco == "0")
                {
                    const string qtdMoeda = "0";
                    qtdMoeda.PadLeft(13, '0');
                }
                    // Caso contrário, preenche com a quantidade
                else
                    String.Format("{0:0.#####}", boleto.QuantidadeMoeda);
                detalhe = detalhe.PreencherValorNaLinha(71, 83, boleto.QuantidadeMoeda.ToString().PadLeft(13, '0')); // Quantidade de Moeda Variável
                detalhe = detalhe.PreencherValorNaLinha(84, 86, boleto.CarteiraCobranca.Codigo.PadLeft(3, '0')); // Número da Carteira no Banco
                detalhe = detalhe.PreencherValorNaLinha(87, 107, string.Empty.PadRight(21, ' ')); // Identificação da Operação no Banco
                /* Código da Carteira */
                // Modalidade de Carteira D - Direta
                if (carteiraCob == "108")
                    detalhe = detalhe.PreencherValorNaLinha(108, 108, "D");
                // Modalidade de Carteira S - Sem Registro
                if (carteiraCob == "103" || carteiraCob == "173" || carteiraCob == "196")
                    detalhe = detalhe.PreencherValorNaLinha(108, 108, "S");
                // Modalidade de Carteira E - Escritural
                if (carteiraCob == "104" || carteiraCob == "112" || carteiraCob == "138" || carteiraCob == "147")
                    detalhe = detalhe.PreencherValorNaLinha(108, 108, "E");
                detalhe = detalhe.PreencherValorNaLinha(109, 110, boleto.CodigoOcorrencia.Codigo.PadLeft(2, '0')); // Identificação da Ocorrência
                detalhe = detalhe.PreencherValorNaLinha(111, 120, boleto.NumeroDocumento.PadLeft(10, '0')); // Nro do Documento de Cobrança
                detalhe = detalhe.PreencherValorNaLinha(121, 126, boleto.DataVencimento.ToString("d").Replace("/", "")); // Data de Vencimento do Título
                detalhe = detalhe.PreencherValorNaLinha(127, 139, boleto.ValorBoleto.ToString().Replace(".", "").Replace(",", "").PadLeft(11, '0')); // Valor Nominal do Título
                detalhe = detalhe.PreencherValorNaLinha(140, 142, boleto.BancoBoleto.CodigoBanco.PadLeft(3, '0')); // Nro do Banco na Câmara de Compensação
                detalhe = detalhe.PreencherValorNaLinha(143, 147, string.Empty.PadLeft(5, '0')); // Agência onde o título será cobrado
                // Espécie do documento padronizado para DM - Duplicata Mercantil
                detalhe = detalhe.PreencherValorNaLinha(148, 149, boleto.Especie.Sigla.Equals("DM") ? "01" : boleto.Especie.Codigo.ToString()); // Espécie do Título
                detalhe = detalhe.PreencherValorNaLinha(150, 150, boleto.Aceite.Equals("A") ? "A" : "N"); // Identificação de Título Aceitou ou Não Aceito
                detalhe = detalhe.PreencherValorNaLinha(151, 156, boleto.DataDocumento.ToString("d").Replace("/", "")); // Data da Emissão do Título

                #region INSTRUÇÕES REMESSA

                if (boleto.InstrucoesDoBoleto.Count > 2)
                    throw new Exception(string.Format("<BoletoBr>{0}Não são aceitas mais que 2 instruções padronizadas para remessa de boletos no banco Itaú.", Environment.NewLine));

                var primeiraInstrucao = boleto.InstrucoesDoBoleto.FirstOrDefault();
                var segundaInstrucao = boleto.InstrucoesDoBoleto.LastOrDefault();

                if (primeiraInstrucao != null)
                {
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, primeiraInstrucao.ToString());
                        // Instrução de Cobrança 1
                }
                if (segundaInstrucao != null)
                {
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, segundaInstrucao.ToString());
                        // Instrução de Cobrança 2
                }

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(161, 173, boleto.JurosMora.ToString().Replace(".", "").Replace(",", "").PadLeft(11, '0')); // Valor de Mora Por Dia de Atraso
                detalhe = detalhe.PreencherValorNaLinha(174, 179, boleto.DataDesconto.ToString("d").Replace("/", "")); // Data Limite para Concesão de Desconto
                detalhe = detalhe.PreencherValorNaLinha(180, 192, boleto.ValorDesconto.ToString().Replace(".", "").Replace(",", "").PadLeft(11, '0')); // Valor do Desconto a ser Concedido
                detalhe = detalhe.PreencherValorNaLinha(193, 205, boleto.Iof.ToString().Replace(".", "").Replace(",", "").PadLeft(11, '0')); // Valor do I.O.F. recolhido p/ notas seguro
                detalhe = detalhe.PreencherValorNaLinha(206, 218, boleto.ValorAbatimento.ToString().Replace(".", "").Replace(",", "").PadLeft(11, '0')); // Valor do Abatimento a ser concedido
                detalhe = detalhe.PreencherValorNaLinha(219, 220, boleto.SacadoBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11 ? "01" : "02"); // Identificação do tipo de inscrição/sacado
                detalhe = detalhe.PreencherValorNaLinha(221, 234, boleto.SacadoBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "")); // Nro de Inscrição do Sacado (CPF/CNPJ)
                detalhe = detalhe.PreencherValorNaLinha(235, 264, boleto.SacadoBoleto.Nome.PadRight(30, ' ')); // Nome do Sacado
                detalhe = detalhe.PreencherValorNaLinha(265, 274, string.Empty.PadRight(10, '0')); // Complemento de Registro
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado); // Rua, Número, e Complemento do Sacado
                detalhe = detalhe.PreencherValorNaLinha(315, 326, boleto.SacadoBoleto.EnderecoSacado.Bairro.PadRight(12, ' ')); // Bairro do Sacado
                detalhe = detalhe.PreencherValorNaLinha(327, 334, boleto.SacadoBoleto.EnderecoSacado.Cep.PadLeft(8, ' ')); // Cep do Sacado
                detalhe = detalhe.PreencherValorNaLinha(335, 349, boleto.SacadoBoleto.EnderecoSacado.Cidade.PadRight(15, ' ')); // Cidado do Sacado
                detalhe = detalhe.PreencherValorNaLinha(350, 351, boleto.SacadoBoleto.EnderecoSacado.SiglaUf.PadRight(2, ' ')); // UF do Sacado
                detalhe = detalhe.PreencherValorNaLinha(352, 381, boleto.SacadoBoleto.NomeAvalista.PadRight(30, ' ')); // Nome do Sacador ou Avalista
                detalhe = detalhe.PreencherValorNaLinha(382, 385, string.Empty.PadRight(4, '0')); // Complemento do Registro
                detalhe = detalhe.PreencherValorNaLinha(386, 391, boleto.DataJurosMora.ToString("d").Replace("/", "")); // Data de Mora
                detalhe = boleto.QtdDias.ToString().PadLeft(2, '0'); // Quantidade de Dias Posição 392 a 393
                detalhe = detalhe.PreencherValorNaLinha(394, 394, string.Empty.PadRight(1, '0')); // Complemento do Registro
                detalhe = detalhe.PreencherValorNaLinha(395, 400, numeroRegistro.ToString().PadLeft(6, ' ')); // Nro Sequencial do Registro no Arquivo

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do DETALHE do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverTrailer(int numeroRegistro)
        {
            var trailer = new string(' ', 400);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, "9");
                trailer = trailer.PreencherValorNaLinha(2, 394, string.Empty.PadRight(393, ' '));
                // Contagem total de linhas do arquivo no formato '000000' - 6 dígitos
                trailer = trailer.PreencherValorNaLinha(395, 400, numeroRegistro.ToString().PadLeft(6, '0'));

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do TRAILER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }
    }
}
