using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Bradesco
{
    public class EscritorRemessaCnab400Bradesco : IEscritorArquivoRemessa
    {
        public List<string> EscreverArquivo(List<Boleto> boletosEscrever)
        {
            throw new NotImplementedException();
        }

        public void ValidarArquivoRemessa(Cedente cedente, List<Boleto> boletos, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        public string EscreverHeader(Boleto boleto, int numeroRemessa, int numeroRegistro)
        {
            var header = new string(' ', 400);
            try
            {
                header = header.PreencherValorNaLinha(1, 1, "0");
                header = header.PreencherValorNaLinha(2, 2, "1");
                header = header.PreencherValorNaLinha(3, 9, "REMESSA".PadRight(7, ' '));
                header = header.PreencherValorNaLinha(10, 11, "01");
                header = header.PreencherValorNaLinha(12, 26, "COBRANCA".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(27, 46, boleto.CedenteBoleto.CodigoCedente.PadLeft(20, '0'));
                header = header.PreencherValorNaLinha(47, 76, boleto.CedenteBoleto.Nome.ToUpper().PadRight(30, ' '));
                header = header.PreencherValorNaLinha(77, 79, "237");
                header = header.PreencherValorNaLinha(80, 94, "BRADESCO");
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("d").Replace("/", ""));
                header = header.PreencherValorNaLinha(101, 108, string.Empty.PadRight(8, ' '));
                header = header.PreencherValorNaLinha(109, 110, "MX");
                header = header.PreencherValorNaLinha(111, 117, numeroRemessa.ToString().PadLeft(7, '0'));
                header = header.PreencherValorNaLinha(118, 394, string.Empty.PadRight(8, ' '));
                header = header.PreencherValorNaLinha(395, 400, "000001");

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

            string codigoCedente = "0" + boleto.CarteiraCobranca.Codigo.PadLeft(2, '0') +
                                   boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(5, '0') +
                                   boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(7, '0') +
                                   boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta;
            string carteiraCob = boleto.CarteiraCobranca.Codigo.PadLeft(3, ' ');
            string enderecoSacado = boleto.SacadoBoleto.EnderecoSacado.TipoLogradouro.Substring(0, 3) + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Logradouro.Substring(0, 8) + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Numero + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Complemento.Substring(0, 7) + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Bairro.Substring(0, 6) + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Cidade.Substring(0, 6);
            #endregion

            var detalhe = new string(' ', 400);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, "1");
                // Débito Autómático em C/C
                detalhe = detalhe.PreencherValorNaLinha(2, 6, string.Empty.PadLeft(5, '0'));
                detalhe = detalhe.PreencherValorNaLinha(7, 7, "0");
                detalhe = detalhe.PreencherValorNaLinha(8, 12, string.Empty.PadLeft(5, '0'));
                detalhe = detalhe.PreencherValorNaLinha(13, 19, string.Empty.PadLeft(7, '0'));
                detalhe = detalhe.PreencherValorNaLinha(20, 20, "0");

                #region POSIÇÃO: 21-37 - IDENTIFICAÇÃO DA EMPRESA CEDENTE NO BANCO

                detalhe = detalhe.PreencherValorNaLinha(21, 21, "0");
                detalhe = detalhe.PreencherValorNaLinha(22, 24, boleto.CarteiraCobranca.Codigo.PadLeft(3, '0'));
                detalhe = detalhe.PreencherValorNaLinha(25, 29, boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(5, '0'));
                detalhe = detalhe.PreencherValorNaLinha(30, 36, boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(7, '0'));
                detalhe = detalhe.PreencherValorNaLinha(37, 37, boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(38, 62, boleto.NossoNumeroFormatado.PadLeft(25, '0'));
                detalhe = detalhe.PreencherValorNaLinha(63, 65, "237");
                detalhe = detalhe.PreencherValorNaLinha(66, 66, "0"); // Sem cobrança de multa
                detalhe = detalhe.PreencherValorNaLinha(67, 70, "0000"); // Percentual de multa
                detalhe = detalhe.PreencherValorNaLinha(71, 82, boleto.NossoNumeroFormatado.PadLeft(11, '0'));
                detalhe = detalhe.PreencherValorNaLinha(83, 92, boleto.ValorDesconto.ToString().Replace(".", "").Replace(",", "").PadLeft(10, '0'));
                detalhe = detalhe.PreencherValorNaLinha(93, 93, "2");
                detalhe = detalhe.PreencherValorNaLinha(94, 94, "N");
                detalhe = detalhe.PreencherValorNaLinha(95, 104, string.Empty.PadLeft(10, ' '));
                detalhe = detalhe.PreencherValorNaLinha(105, 105, " ");
                detalhe = detalhe.PreencherValorNaLinha(106, 106, "2");
                detalhe = detalhe.PreencherValorNaLinha(107, 108, string.Empty.PadLeft(2, ' '));
                detalhe = detalhe.PreencherValorNaLinha(109, 110, boleto.CodigoOcorrenciaRemessa.Codigo.ToString().PadLeft(2, '0'));
                detalhe = detalhe.PreencherValorNaLinha(111, 120, boleto.NumeroDocumento.PadLeft(10, ' '));
                detalhe = detalhe.PreencherValorNaLinha(121, 126, boleto.DataVencimento.ToString("d").Replace("/", ""));
                detalhe = detalhe.PreencherValorNaLinha(127, 139, boleto.ValorBoleto.ToString().Replace(".", "").Replace(",", "").PadLeft(13, '0'));

                if (boleto.CodigoOcorrenciaRemessa.Codigo.Equals("01"))
                {
                    detalhe = detalhe.PreencherValorNaLinha(140, 142, string.Empty.PadLeft(3, '0'));
                    detalhe = detalhe.PreencherValorNaLinha(143, 147, string.Empty.PadLeft(5, '0'));
                }
                else
                {
                    detalhe = detalhe.PreencherValorNaLinha(140, 142, boleto.BancoBoleto.CodigoBanco.PadLeft(3, '0'));
                    detalhe = detalhe.PreencherValorNaLinha(143, 147, boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0') + boleto.CedenteBoleto.ContaBancariaCedente.DigitoAgencia);
                }

                detalhe = detalhe.PreencherValorNaLinha(148, 149, boleto.Especie.Sigla.Equals("DM") ? "01" : boleto.Especie.Codigo.ToString());
                detalhe = detalhe.PreencherValorNaLinha(150, 150, boleto.Aceite.Equals("N") ? "N" : "A");
                detalhe = detalhe.PreencherValorNaLinha(151, 156, boleto.DataDocumento.ToString("d").Replace("/", ""));

                #region INSTRUÇÕES REMESSA

                if (boleto.InstrucoesDoBoleto.Count > 2)
                    throw new Exception(string.Format("<BoletoBr>{0}Não são aceitas mais que 2 instruções padronizadas para remessa de boletos no banco Bradesco.", Environment.NewLine));

                var primeiraInstrucao = boleto.InstrucoesDoBoleto.FirstOrDefault();
                var segundaInstrucao = boleto.InstrucoesDoBoleto.LastOrDefault();

                if (primeiraInstrucao != null)
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, primeiraInstrucao.ToString());
                else
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, "00");
                if (segundaInstrucao != null)
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, segundaInstrucao.ToString());
                else
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, "00");

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(161, 173, boleto.JurosMora.ToString().Replace(".", "").Replace(",", "").PadLeft(13, '0')); // Valor de Mora Por Dia de Atraso
                detalhe = detalhe.PreencherValorNaLinha(174, 179, boleto.DataDesconto.ToString("d").Replace("/", "")); // Data Limite para Concesão de Desconto
                detalhe = detalhe.PreencherValorNaLinha(180, 192, boleto.ValorDesconto.ToString().Replace(".", "").Replace(",", "").PadLeft(13, '0')); // Valor do Desconto a ser Concedido
                detalhe = detalhe.PreencherValorNaLinha(193, 205, boleto.Iof.ToString().Replace(".", "").Replace(",", "").PadLeft(13, '0')); // Valor do I.O.F. recolhido p/ notas seguro
                detalhe = detalhe.PreencherValorNaLinha(206, 218, boleto.ValorAbatimento.ToString().Replace(".", "").Replace(",", "").PadLeft(13, '0')); // Valor do Abatimento a ser concedido
                detalhe = detalhe.PreencherValorNaLinha(219, 220, boleto.SacadoBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11 ? "01" : "02"); // Identificação do tipo de inscrição/sacado
                detalhe = detalhe.PreencherValorNaLinha(221, 234, boleto.SacadoBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0')); // Nro de Inscrição do Sacado (CPF/CNPJ)
                detalhe = detalhe.PreencherValorNaLinha(235, 274, boleto.SacadoBoleto.Nome.ToUpper().PadRight(40, ' ')); // Nome do Sacado
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado.ToUpper()); // Rua, Número, e Complemento do Sacado
                detalhe = detalhe.PreencherValorNaLinha(315, 326, string.Empty.PadLeft(12, ' ')); // 1ª Mensagem
                detalhe = detalhe.PreencherValorNaLinha(327, 334, boleto.SacadoBoleto.EnderecoSacado.Cep.PadLeft(8, ' ')); // Cep do Sacado
                detalhe = detalhe.PreencherValorNaLinha(335, 394, boleto.SacadoBoleto.NomeAvalista.ToUpper().PadRight(60, ' ')); // Cidado do Sacado
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
