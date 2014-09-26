using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Bradesco
{
    public class EscritorRemessaCnab400Bradesco : IEscritorArquivoRemessa
    {
        public List<string> EscreverArquivo(List<Boleto> boletosEscrever)
        {
            return null;
        }

        public void ValidarArquivoRemessa(Cedente cedente, List<Boleto> boletos, int numeroArquivoRemessa)
        {
            if (cedente == null)
                throw new Exception("O Cedente/Beneficiário é obrigatório!");

            if (boletos == null || boletos.Count.Equals(0))
                throw new Exception("Deverá existir ao menos 1 boleto para geração da remessa!");

            if (numeroArquivoRemessa == 0)
                throw new Exception("O número sequencial da remessa não foi informado!");

            foreach (var boleto in boletos)
            {
                if (boleto.Remessa == null)
                    throw new Exception("Para o boleto " + boleto.NumeroDocumento + ", informe as diretrizes de remessa!");
            }
        }

        public string EscreverHeader(Boleto boleto, int numeroRemessa, int numeroRegistro)
        {
            if (boleto == null)
                throw new Exception("Não há boleto para geração do HEADER");

            if (numeroRemessa == 0)
                throw new Exception("Sequencial da remessa não foi informado na geração do HEADER.");

            if (numeroRegistro == 0)
                throw new Exception("Sequencial do registro não foi informado na geração do HEADER.");

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
                header = header.PreencherValorNaLinha(80, 94, "BRADESCO".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("ddMMyy"));
                header = header.PreencherValorNaLinha(101, 108, string.Empty.PadRight(8, ' '));
                header = header.PreencherValorNaLinha(109, 110, "MX");
                header = header.PreencherValorNaLinha(111, 117, numeroRemessa.ToString(CultureInfo.InvariantCulture).PadLeft(7, '0'));
                header = header.PreencherValorNaLinha(118, 394, string.Empty.PadRight(277, ' '));
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
            if (boleto == null)
                throw new Exception("Não há boleto para geração do DETALHE");

            if (numeroRegistro == 0)
                throw new Exception("Sequencial do registro não foi informado na geração do DETALHE.");

            #region Variáveis

            //string codigoCedente = "0" + boleto.CarteiraCobranca.Codigo.PadLeft(2, '0') +
            //                       boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(5, '0') +
            //                       boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(7, '0') +
            //                       boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta;
            var enderecoSacado = string.Empty;
            enderecoSacado += boleto.SacadoBoleto.EnderecoSacado.TipoLogradouro;
            enderecoSacado += " ";
            enderecoSacado += boleto.SacadoBoleto.EnderecoSacado.Logradouro;
            enderecoSacado += " ";
            enderecoSacado += boleto.SacadoBoleto.EnderecoSacado.Numero;
            enderecoSacado += " ";
            enderecoSacado += boleto.SacadoBoleto.EnderecoSacado.Complemento;
            enderecoSacado += " ";
            enderecoSacado += boleto.SacadoBoleto.EnderecoSacado.Bairro;
            enderecoSacado += " ";
            enderecoSacado += boleto.SacadoBoleto.EnderecoSacado.Cidade;

            #endregion

            if (enderecoSacado.Length > 40)
                throw new Exception("Endereço do sacado excedeu o limite permitido.");

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

                const string doc = "DOC";
                var seuNumero = doc + boleto.NossoNumeroFormatado.PadRight(25 - doc.Length, ' ');

                detalhe = detalhe.PreencherValorNaLinha(38, 62, seuNumero);
                detalhe = detalhe.PreencherValorNaLinha(63, 65, "237");
                detalhe = detalhe.PreencherValorNaLinha(66, 66, "0"); // Sem cobrança de multa
                detalhe = detalhe.PreencherValorNaLinha(67, 70, "0000"); // Percentual de multa
                detalhe = detalhe.PreencherValorNaLinha(71, 81, boleto.NumeroDocumento);
                detalhe = detalhe.PreencherValorNaLinha(82, 82, boleto.DigitoNossoNumero);

                #region VALOR DESCONTO POR DIA

                var valorDescontoDia = string.Empty;

                if (boleto.ValorDescontoDia.ToString().Contains('.') && boleto.ValorDescontoDia.ToString().Contains(','))
                {
                    valorDescontoDia = boleto.ValorDescontoDia.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(83, 92, valorDescontoDia.PadLeft(10, '0'));
                }
                if (boleto.ValorDesconto.ToString().Contains('.'))
                {
                    valorDescontoDia = boleto.ValorDescontoDia.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(83, 92, valorDescontoDia.PadLeft(10, '0'));
                }
                if (boleto.ValorDesconto.ToString().Contains(','))
                {
                    valorDescontoDia = boleto.ValorDescontoDia.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(83, 92, valorDescontoDia.PadLeft(10, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(83, 92, valorDescontoDia.PadLeft(10, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(93, 93, "2");
                detalhe = detalhe.PreencherValorNaLinha(94, 94, "N");
                detalhe = detalhe.PreencherValorNaLinha(95, 104, string.Empty.PadLeft(10, ' '));
                detalhe = detalhe.PreencherValorNaLinha(105, 105, " ");
                detalhe = detalhe.PreencherValorNaLinha(106, 106, "2");
                detalhe = detalhe.PreencherValorNaLinha(107, 108, string.Empty.PadLeft(2, ' '));
                detalhe = detalhe.PreencherValorNaLinha(109, 110, boleto.CodigoOcorrenciaRemessa.Codigo.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'));
                detalhe = detalhe.PreencherValorNaLinha(111, 120, boleto.NumeroDocumento.Replace("0", "").PadLeft(10, '0'));
                detalhe = detalhe.PreencherValorNaLinha(121, 126, boleto.DataVencimento.ToString("ddMMyy"));

                #region VALOR BOLETO

                string valorBoleto;

                if (boleto.ValorBoleto.ToString("f").Contains('.') && boleto.ValorBoleto.ToString("f").Contains(','))
                {
                    valorBoleto = boleto.ValorBoleto.ToString("f").Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(127, 139, valorBoleto.PadLeft(13, '0'));
                }
                if (boleto.ValorBoleto.ToString("f").Contains('.'))
                {
                    valorBoleto = boleto.ValorBoleto.ToString("f").Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(127, 139, valorBoleto.PadLeft(13, '0'));
                }
                if (boleto.ValorBoleto.ToString("f").Contains(','))
                {
                    valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(127, 139, valorBoleto.PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(127, 139, boleto.ValorBoleto.ToString("f").Replace(".", "").Replace(",", "").PadLeft(13, '0'));

                #endregion

                if (boleto.CodigoOcorrenciaRemessa.Codigo.Equals(01))
                {
                    detalhe = detalhe.PreencherValorNaLinha(140, 142, string.Empty.PadLeft(3, '0'));
                    detalhe = detalhe.PreencherValorNaLinha(143, 147, string.Empty.PadLeft(5, '0'));
                }
                else
                {
                    detalhe = detalhe.PreencherValorNaLinha(140, 142, boleto.BancoBoleto.CodigoBanco.PadLeft(3, '0'));
                    detalhe = detalhe.PreencherValorNaLinha(143, 147, boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0') + boleto.CedenteBoleto.ContaBancariaCedente.DigitoAgencia);
                }

                detalhe = detalhe.PreencherValorNaLinha(148, 149, boleto.Especie.Sigla.Equals("DM") ? "01" : boleto.Especie.Codigo.ToString(CultureInfo.InvariantCulture));
                detalhe = detalhe.PreencherValorNaLinha(150, 150, boleto.Aceite.Equals("A") ? "A" : "N");
                detalhe = detalhe.PreencherValorNaLinha(151, 156, boleto.DataDocumento.ToString("ddMMyy"));

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

                #region VALOR JUROS

                var jurosBoleto = string.Empty;

                if (boleto.JurosMora.ToString().Contains('.') && boleto.JurosMora.ToString().Contains(','))
                {
                    jurosBoleto = boleto.JurosMora.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosBoleto.PadLeft(13, '0'));
                }
                if (boleto.JurosMora.ToString().Contains('.'))
                {
                    jurosBoleto = boleto.JurosMora.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosBoleto.PadLeft(13, '0'));
                }
                if (boleto.JurosMora.ToString().Contains(','))
                {
                    jurosBoleto = boleto.JurosMora.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosBoleto.PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosBoleto.PadLeft(13, '0')); // Valor de Mora Por Dia de Atraso

                #endregion

                if (boleto.DataDesconto == DateTime.MinValue)
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, string.Empty.PadLeft(6, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, boleto.DataDesconto.ToString("ddMMyy")); // Data Limite para Concesão de Desconto

                #region VALOR DESCONTO

                string descontoBoleto;

                if (boleto.ValorDesconto.ToString().Contains('.') && boleto.ValorDesconto.ToString().Contains(','))
                {
                    descontoBoleto = boleto.ValorDesconto.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(180, 192, descontoBoleto.PadLeft(13, '0'));
                }
                if (boleto.ValorDesconto.ToString().Contains('.'))
                {
                    descontoBoleto = boleto.ValorDesconto.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(180, 192, descontoBoleto.PadLeft(13, '0'));
                }
                if (boleto.ValorDesconto.ToString().Contains(','))
                {
                    descontoBoleto = boleto.ValorDesconto.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(180, 192, descontoBoleto.PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(180, 192, boleto.ValorDesconto.ToString().PadLeft(13, '0')); // Valor do Desconto a ser Concedido

                #endregion

                #region VALOR IOF

                string iofBoleto;

                if (boleto.Iof.ToString(CultureInfo.InvariantCulture).Contains('.') && boleto.Iof.ToString(CultureInfo.InvariantCulture).Contains(','))
                {
                    iofBoleto = boleto.Iof.ToString(CultureInfo.InvariantCulture).Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, iofBoleto.PadLeft(13, '0'));
                }
                if (boleto.Iof.ToString(CultureInfo.InvariantCulture).Contains('.'))
                {
                    iofBoleto = boleto.Iof.ToString(CultureInfo.InvariantCulture).Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, iofBoleto.PadLeft(13, '0'));
                }
                if (boleto.Iof.ToString(CultureInfo.InvariantCulture).Contains(','))
                {
                    iofBoleto = boleto.Iof.ToString(CultureInfo.InvariantCulture).Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, iofBoleto.PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(193, 205, boleto.Iof.ToString(CultureInfo.InvariantCulture).PadLeft(13, '0')); // Valor do I.O.F. recolhido p/ notas seguro

                #endregion

                #region VALOR ABATIMENTO

                string abatimentoBoleto;

                if (boleto.ValorAbatimento.ToString().Contains('.') && boleto.ValorAbatimento.ToString().Contains(','))
                {
                    abatimentoBoleto = boleto.ValorAbatimento.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(206, 218, abatimentoBoleto.PadLeft(13, '0'));
                }
                if (boleto.ValorAbatimento.ToString().Contains('.'))
                {
                    abatimentoBoleto = boleto.ValorAbatimento.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(206, 218, abatimentoBoleto.PadLeft(13, '0'));
                }
                if (boleto.ValorAbatimento.ToString().Contains(','))
                {
                    abatimentoBoleto = boleto.ValorAbatimento.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(206, 218, abatimentoBoleto.PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(206, 218, boleto.ValorAbatimento.ToString().PadLeft(13, '0')); // Valor do Abatimento a ser concedido

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(219, 220, boleto.SacadoBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11 ? "01" : "02"); // Identificação do tipo de inscrição/sacado
                detalhe = detalhe.PreencherValorNaLinha(221, 234, boleto.SacadoBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0')); // Nro de Inscrição do Sacado (CPF/CNPJ)
                detalhe = detalhe.PreencherValorNaLinha(235, 274, boleto.SacadoBoleto.Nome.ToUpper().PadRight(40, ' ')); // Nome do Sacado
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado.ToUpper().PadRight(40, ' ')); // Rua, Número, e Complemento do Sacado

                #region 1ª Mensagem

                /* POSIÇÃO: 315 a 326 - 1ª Mensagem
                 * Campo livre para uso da empresa.
                 * A mensagem enviada nesse campo será impressa somente no boleto enão será confirmada no arquivo retorno.
                 */
                detalhe = detalhe.PreencherValorNaLinha(315, 326, string.Empty.PadLeft(12, ' ')); // 1ª Mensagem

                #endregion

                var cep = boleto.SacadoBoleto.EnderecoSacado.Cep;

                if (cep.Contains(".") && cep.Contains("-"))
                    cep = cep.Replace(".", "").Replace("-", "");
                if (cep.Contains("."))
                    cep = cep.Replace(".", "");
                if (cep.Contains("-"))
                    cep = cep.Replace("-", "");

                detalhe = detalhe.PreencherValorNaLinha(327, 334, cep.PadLeft(8, ' ')); // Cep do Sacado

                #region 2ª Mensagem / Sacador Avalista

                /* Sacador / Avalista ou 2ª Mensagem
                 * CNPJ/CPF do Sacador Avalista ( o critério para preenchimento, deve ser o mesmo tanto para o
                 * CNPJ como para o CPF ou seja, iniciando da direita para a esquerda:
                 * - 2 posições para o controle;
                 * - 4 posições para filial; e
                 * - 9 posições para o CNPJ/CPF.
                 * Obs.: No caso de CPF, o campo filial deverá ser preenchido com zeros.
                 * COMPOSIÇÃO DAS POSIÇÕES 335-394
                 * 15 Numéricos
                 * 02 Brancos
                 * 43 Alfanumérico
                 */

                string str = string.Empty;

                if (String.IsNullOrEmpty(boleto.SacadoBoleto.NomeAvalista))
                    detalhe = detalhe.PreencherValorNaLinha(335, 394, str.PadLeft(60, ' '));
                else
                {
                    if (boleto.SacadoBoleto.CpfCnpjAvalista.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11)
                    {
                        str = (boleto.SacadoBoleto.NomeAvalista.ToUpper() +
                               string.Empty.PadLeft(2, ' ') +
                               boleto.SacadoBoleto.CpfCnpjAvalista.Replace(".", "")
                                   .Replace("/", "")
                                   .Replace("-", "")
                                   .Substring(0, 9) +
                               string.Empty.PadLeft(4, '0') +
                               boleto.SacadoBoleto.CpfCnpjAvalista.Replace(".", "")
                                   .Replace("/", "")
                                   .Replace("-", "")
                                   .Substring(9, 2));
                    }
                    else
                    {
                        str = (boleto.SacadoBoleto.NomeAvalista.ToUpper() +
                               string.Empty.PadLeft(2, ' ') +
                               boleto.SacadoBoleto.CpfCnpjAvalista.Replace(".", "")
                                   .Replace("/", "")
                                   .Replace("-", "")
                                   .PadLeft(15, '0'));
                    }

                    detalhe = detalhe.PreencherValorNaLinha(335, 394, str.PadLeft(60, ' '));
                }

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(395, 400, numeroRegistro.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0')); // Nro Sequencial do Registro no Arquivo

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
            if (numeroRegistro == 0)
                throw new Exception("Sequencial do registro não foi informado na geração do TRAILER.");

            var trailer = new string(' ', 400);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, "9");
                trailer = trailer.PreencherValorNaLinha(2, 394, string.Empty.PadRight(393, ' '));
                // Contagem total de linhas do arquivo no formato '000000' - 6 dígitos
                trailer = trailer.PreencherValorNaLinha(395, 400, numeroRegistro.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));

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
