using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Santander
{
    public class EscritorRemessaCnab400Santander : IEscritorArquivoRemessa
    {
        public List<string> EscreverArquivo(List<Boleto> boletosEscrever)
        {
            throw new NotImplementedException();
        }

        public void ValidarArquivoRemessa(Cedente cedente, List<Boleto> boletos, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        public string EscreverHeader(Boleto boleto, int numeroRegistro)
        {
            var header = new string(' ', 400);
            try
            {
                header = header.PreencherValorNaLinha(1, 1, "0");
                header = header.PreencherValorNaLinha(2, 2, "1");
                header = header.PreencherValorNaLinha(3, 9, "REMESSA");
                header = header.PreencherValorNaLinha(10, 11, "01");
                header = header.PreencherValorNaLinha(12, 26, "COBRANCA".PadRight(15, ' '));
                // Código de Transmissão fornecido pelo Banco
                if (String.IsNullOrEmpty(boleto.CodigoDeTransmissao))
                    header = header.PreencherValorNaLinha(27, 46, string.Empty.PadLeft(20, '0'));
                else
                    header = header.PreencherValorNaLinha(27, 46, boleto.CodigoDeTransmissao.PadLeft(20, '0')); 
                header = header.PreencherValorNaLinha(47, 76, boleto.CedenteBoleto.Nome.PadRight(30, ' '));
                header = header.PreencherValorNaLinha(77, 79, "033");
                header = header.PreencherValorNaLinha(80, 94, "SANTANDER".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("ddMMyy").Replace("/", ""));
                header = header.PreencherValorNaLinha(101, 116, string.Empty.PadLeft(16, '0'));
                // Mensagem 1 a Mensagem 6
                header = header.PreencherValorNaLinha(117, 391, string.Empty.PadRight(275, ' '));
                header = header.PreencherValorNaLinha(392, 394, "000");
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

            int identificadorMulta = 0;
            if (boleto.ValorMulta > 0)
                identificadorMulta = 4;

            /* Códigos de Carteira
             * 1 - Eletrônica com registro
             * 3 - Caucionada eletrônica
             * 4 - Cobrança sem registro
             * 5 - Rápida com registro
             * 6 - Caucionada rápida
             * 7 - Descontada eletrônica
             */

            var codigoCarteira = string.Empty;

            if (boleto.CarteiraCobranca.Codigo == "101")
                codigoCarteira = "5";
            if (boleto.CarteiraCobranca.Codigo == "102")
                codigoCarteira = "4";

            string enderecoSacado = boleto.SacadoBoleto.EnderecoSacado.TipoLogradouro + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Logradouro + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Numero + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Complemento;

            string complemento = 
                boleto.CedenteBoleto.ContaBancariaCedente.Conta.Remove(0, boleto.CedenteBoleto.ContaBancariaCedente.Conta.Length - 1) + 
                boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta;


            #endregion

            if (enderecoSacado.Length > 40)
                throw new Exception("Endereço do sacado excedeu o limite permitido.");

            var detalhe = new string(' ', 400);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, "1");
                detalhe = detalhe.PreencherValorNaLinha(2, 3, boleto.CedenteBoleto.CpfCnpj.Length == 11 ? "01" : "02");
                detalhe = detalhe.PreencherValorNaLinha(4, 17, boleto.CedenteBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                // Código de Transmissão fornecido pelo Banco
                //detalhe = detalhe.PreencherValorNaLinha(18, 37, boleto.CodigoDeTransmissao.PadLeft(20, '0'));

                /* Identificador do Complemento
                 * O identificador "i" e o complemento da conta cobrança só seraõ disponibilizados no arquivo retorno quando a
                 * conta movimento for diferente da conta cobrança.
                 */
                detalhe = detalhe.PreencherValorNaLinha(18, 21, boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'));
                detalhe = detalhe.PreencherValorNaLinha(18, 21, boleto.CedenteBoleto.CodigoCedente.PadLeft(10, '0').Substring(0, 8));
                detalhe = detalhe.PreencherValorNaLinha(18, 21, boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(10, '0').Substring(0, 8));

                const string doc = "DOC";
                var seuNumero = doc + boleto.NossoNumeroFormatado.PadRight(25 - doc.Length, ' ');

                detalhe = detalhe.PreencherValorNaLinha(38, 62, seuNumero);
                // NossoNumero com DV, pegar os 8 primeiros dígitos, da direita para esquerda
                detalhe = detalhe.PreencherValorNaLinha(63, 70, boleto.NossoNumeroFormatado.Replace("-", "").Substring(5, 8));

                if (boleto.DataLimite == DateTime.MinValue)
                    detalhe = detalhe.PreencherValorNaLinha(71, 76, string.Empty.PadLeft(6, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(71, 76, boleto.DataLimite.ToString("ddMMyy"));

                detalhe = detalhe.PreencherValorNaLinha(77, 77, string.Empty.PadLeft(1, ' '));
                detalhe = detalhe.PreencherValorNaLinha(78, 78, identificadorMulta.ToString());

                if (String.IsNullOrEmpty(boleto.PercentualMulta.ToString()))
                    detalhe = detalhe.PreencherValorNaLinha(79, 82, string.Empty.PadLeft(4, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(79, 82, boleto.PercentualMulta.ToString().PadLeft(4, '0'));

                detalhe = detalhe.PreencherValorNaLinha(83, 84, "00");
                detalhe = detalhe.PreencherValorNaLinha(85, 97, "0000000000000" /*Vl do título em outra unidade (consultar banco)*/);
                detalhe = detalhe.PreencherValorNaLinha(98, 101, string.Empty.PadLeft(4, ' '));

                if (boleto.DataMulta == DateTime.MinValue)
                    detalhe = detalhe.PreencherValorNaLinha(71, 76, string.Empty.PadLeft(6, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(102, 107, boleto.DataMulta.ToString("ddMMyy"));

                detalhe = detalhe.PreencherValorNaLinha(108, 108, codigoCarteira);
                detalhe = detalhe.PreencherValorNaLinha(109, 110, "01" /* Código da Ocorrência*/);
                detalhe = detalhe.PreencherValorNaLinha(111, 120, boleto.NumeroDocumento.PadRight(10, ' '));
                detalhe = detalhe.PreencherValorNaLinha(121, 126, boleto.DataVencimento.ToString("ddMMyy"));

                #region VALOR TOTAL

                var valorBoleto = string.Empty;

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

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(140, 142, "033");
                if (codigoCarteira == "5")
                    detalhe = detalhe.PreencherValorNaLinha(143, 147, boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0') + boleto.CedenteBoleto.ContaBancariaCedente.DigitoAgencia);
                else
                    detalhe = detalhe.PreencherValorNaLinha(143, 147, string.Empty.PadLeft(5, '0'));
                detalhe = detalhe.PreencherValorNaLinha(148, 149, boleto.Especie.Sigla.Equals("DM") ? "01" : boleto.Especie.Codigo.ToString(CultureInfo.InvariantCulture));
                detalhe = detalhe.PreencherValorNaLinha(150, 150, "N");
                detalhe = detalhe.PreencherValorNaLinha(151, 156, boleto.DataDocumento.ToString("ddMMyy"));

                #region INSTRUÇÕES REMESSA

                if (boleto.InstrucoesDoBoleto.Count > 2)
                    throw new Exception(string.Format("<BoletoBr>{0}Não são aceitas mais que 2 instruções padronizadas para remessa de boletos no banco Santander.", Environment.NewLine));

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

                #region JUROS

                var jurosBoleto = string.Empty;

                if (boleto.JurosMora.ToString().Contains('.') && boleto.JurosMora.ToString().Contains(','))
                {
                    jurosBoleto = boleto.JurosMora.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosBoleto.PadLeft(13, '0'));
                }
                else if (boleto.JurosMora.ToString().Contains('.'))
                {
                    jurosBoleto = boleto.JurosMora.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosBoleto.PadLeft(13, '0'));
                }
                else if (boleto.JurosMora.ToString().Contains(','))
                {
                    jurosBoleto = boleto.JurosMora.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosBoleto.PadLeft(13, '0'));
                }
                else
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, boleto.JurosMora.ToString().PadLeft(13, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(174, 179, boleto.DataDesconto.ToString("ddMMyy").Replace("/", ""));

                #region DESCONTO

                var descontoBoleto = string.Empty;

                if (boleto.ValorDesconto.ToString().Contains('.') && boleto.ValorDesconto.ToString().Contains(','))
                {
                    descontoBoleto = boleto.ValorDesconto.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(180, 192, descontoBoleto.PadLeft(13, '0'));
                }
                else if (boleto.ValorDesconto.ToString().Contains('.'))
                {
                    descontoBoleto = boleto.ValorDesconto.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(180, 192, descontoBoleto.PadLeft(13, '0'));
                }
                else if (boleto.ValorDesconto.ToString().Contains(','))
                {
                    descontoBoleto = boleto.ValorDesconto.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(180, 192, descontoBoleto.PadLeft(13, '0'));
                }
                else
                    detalhe = detalhe.PreencherValorNaLinha(180, 192, boleto.ValorDesconto.ToString().PadLeft(13, '0'));

                #endregion

                #region IOF

                var iofBoleto = string.Empty;

                if (boleto.Iof.ToString().Contains('.') && boleto.Iof.ToString().Contains(','))
                {
                    iofBoleto = boleto.Iof.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, iofBoleto.PadLeft(13, '0'));
                }
                else if (boleto.Iof.ToString().Contains('.'))
                {
                    iofBoleto = boleto.Iof.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, iofBoleto.PadLeft(13, '0'));
                }
                else if (boleto.Iof.ToString().Contains(','))
                {
                    iofBoleto = boleto.Iof.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, iofBoleto.PadLeft(13, '0'));
                }
                else
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, boleto.Iof.ToString().PadLeft(13, '0'));

                #endregion

                #region ABATIMENTO

                var abatimentoBoleto = string.Empty;

                if (boleto.ValorAbatimento.ToString().Contains('.') && boleto.ValorAbatimento.ToString().Contains(','))
                {
                    abatimentoBoleto = boleto.ValorAbatimento.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(206, 218, abatimentoBoleto.PadLeft(13, '0'));
                }
                else if (boleto.ValorAbatimento.ToString().Contains('.'))
                {
                    abatimentoBoleto = boleto.ValorAbatimento.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(206, 218, abatimentoBoleto.PadLeft(13, '0'));
                }
                else if (boleto.ValorAbatimento.ToString().Contains(','))
                {
                    abatimentoBoleto = boleto.ValorAbatimento.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(206, 218, abatimentoBoleto.PadLeft(13, '0'));
                }
                else
                    detalhe = detalhe.PreencherValorNaLinha(206, 218, boleto.ValorAbatimento.ToString().PadLeft(13, '0'));

                #endregion
                
                detalhe = detalhe.PreencherValorNaLinha(219, 220, boleto.SacadoBoleto.CpfCnpj.Length == 11 ? "01" : "02");
                detalhe = detalhe.PreencherValorNaLinha(221, 234, boleto.SacadoBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(235, 274, boleto.SacadoBoleto.Nome.PadRight(40, ' '));
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado.PadRight(40, ' '));
                detalhe = detalhe.PreencherValorNaLinha(315, 326, boleto.SacadoBoleto.EnderecoSacado.Bairro.PadRight(12, ' '));

                #region CEP

                var Cep = boleto.SacadoBoleto.EnderecoSacado.Cep;

                if (Cep.Contains(".") && Cep.Contains("-"))
                    Cep = Cep.Replace(".", "").Replace("-", "");
                if (Cep.Contains("."))
                    Cep = Cep.Replace(".", "");
                if (Cep.Contains("-"))
                    Cep = Cep.Replace("-", "");

                detalhe = detalhe.PreencherValorNaLinha(327, 334, Cep.PadLeft(8, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(335, 349, boleto.SacadoBoleto.EnderecoSacado.Cidade.PadRight(15, ' '));
                detalhe = detalhe.PreencherValorNaLinha(350, 351, boleto.SacadoBoleto.EnderecoSacado.SiglaUf.PadRight(2, ' '));
                
                if (String.IsNullOrEmpty(boleto.SacadoBoleto.NomeAvalista))
                    detalhe = detalhe.PreencherValorNaLinha(352, 381, string.Empty.PadRight(30, ' '));
                else
                    detalhe = detalhe.PreencherValorNaLinha(352, 381, boleto.SacadoBoleto.NomeAvalista.PadRight(30, ' '));

                detalhe = detalhe.PreencherValorNaLinha(382, 382, " ");
                detalhe = detalhe.PreencherValorNaLinha(383, 383, "i".ToUpper()); // Identificador do Complemento
                detalhe = detalhe.PreencherValorNaLinha(384, 385, complemento);
                detalhe = detalhe.PreencherValorNaLinha(386, 391, string.Empty.PadLeft(6, ' '));
                detalhe = detalhe.PreencherValorNaLinha(392, 393, boleto.QtdDias.ToString().PadLeft(2, '0'));
                detalhe = detalhe.PreencherValorNaLinha(394, 394, "0");
                detalhe = detalhe.PreencherValorNaLinha(395, 400, numeroRegistro.ToString().PadLeft(6, '0'));

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do DETALHE do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverTrailer(int numeroRegistro, decimal valorTitulos)
        {
            var trailer = new string(' ', 400);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, "9");
                trailer = trailer.PreencherValorNaLinha(2, 7, numeroRegistro.ToString().PadLeft(6, '0') /*Quantidade total de linhas no arquivo*/);

                #region VALOR TOTAL DOS TÍTULOS

                var vlTitulos = string.Empty;

                if (valorTitulos.ToString("f").Contains('.') && valorTitulos.ToString("f").Contains(','))
                {
                    vlTitulos = valorTitulos.ToString("f").Replace(".", "").Replace(",", "");
                    trailer = trailer.PreencherValorNaLinha(206, 218, vlTitulos.PadLeft(13, '0'));
                }
                else if (valorTitulos.ToString("f").Contains('.'))
                {
                    vlTitulos = valorTitulos.ToString("f").Replace(".", "");
                    trailer = trailer.PreencherValorNaLinha(206, 218, vlTitulos.PadLeft(13, '0'));
                }
                else if (valorTitulos.ToString("f").Contains(','))
                {
                    vlTitulos = valorTitulos.ToString("f").Replace(",", "");
                    trailer = trailer.PreencherValorNaLinha(206, 218, vlTitulos.PadLeft(13, '0'));
                }
                else
                    trailer = trailer.PreencherValorNaLinha(206, 218, valorTitulos.ToString().PadLeft(13, '0'));

                #endregion

                trailer = trailer.PreencherValorNaLinha(8, 20, valorTitulos.ToString("f").Replace(".", "").Replace(",", "").PadLeft(13, '0') /*Valor total dos títulos*/);
                trailer = trailer.PreencherValorNaLinha(21, 394, "0".PadRight(374, '0'));
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
