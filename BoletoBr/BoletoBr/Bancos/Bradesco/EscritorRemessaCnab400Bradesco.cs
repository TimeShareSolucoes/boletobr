using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Bradesco
{
    public class EscritorRemessaCnab400Bradesco : IEscritorArquivoRemessaCnab400
    {
        private RemessaCnab400 _remessaEscrever;

        public EscritorRemessaCnab400Bradesco(RemessaCnab400 remessaEscrever)
        {
            _remessaEscrever = remessaEscrever;
        }

        public string EscreverHeader(HeaderRemessaCnab400 infoHeader)
        {
            if (infoHeader == null)
                throw new Exception("Não há informações para geração do HEADER");

            if (infoHeader.NumeroSequencialRemessa == 0)
                throw new Exception("Sequencial da remessa não foi informado na geração do HEADER.");

            var nomeEmpresa = string.Empty;
            if (infoHeader.NomeEmpresa.Length > 30)
                nomeEmpresa = infoHeader.NomeEmpresa.Substring(0, 30);
            else
                nomeEmpresa = infoHeader.NomeEmpresa.PadRight(30, ' ');

            var header = new string(' ', 400);
            try
            {
                header = header.PreencherValorNaLinha(1, 1, "0");
                header = header.PreencherValorNaLinha(2, 2, "1");
                header = header.PreencherValorNaLinha(3, 9, "REMESSA".PadRight(7, ' '));
                header = header.PreencherValorNaLinha(10, 11, "01");
                header = header.PreencherValorNaLinha(12, 26, "COBRANCA".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(27, 46, infoHeader.CodigoEmpresa.PadLeft(20, '0'));
                header = header.PreencherValorNaLinha(47, 76, nomeEmpresa.ToUpper());
                header = header.PreencherValorNaLinha(77, 79, "237");
                header = header.PreencherValorNaLinha(80, 94, "BRADESCO".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("ddMMyy"));
                header = header.PreencherValorNaLinha(101, 108, string.Empty.PadRight(8, ' '));
                header = header.PreencherValorNaLinha(109, 110, "MX");
                header = header.PreencherValorNaLinha(111, 117,
                    infoHeader.NumeroSequencialRemessa.ToString().PadLeft(7, '0'));
                header = header.PreencherValorNaLinha(118, 394, string.Empty.PadRight(277, ' '));
                header = header.PreencherValorNaLinha(395, 400, "000001");

                return header;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Falha na geração do HEADER do arquivo de REMESSA."), e);
            }
        }

        public string EscreverDetalhe(DetalheRemessaCnab400 infoDetalhe)
        {
            if (infoDetalhe == null)
                throw new Exception("Não há boleto para geração do DETALHE");

            if (infoDetalhe.NumeroSequencialRegistro == 0)
                throw new Exception("Sequencial do registro não foi informado na geração do DETALHE.");

            string enderecoSacado = string.Empty;

            string nomeSacado = string.Empty;

            if (String.IsNullOrEmpty(infoDetalhe.EnderecoPagador))
                enderecoSacado.PadRight(40, ' ');
            else if (infoDetalhe.EnderecoPagador.Length > 40)
                enderecoSacado = infoDetalhe.EnderecoPagador.Substring(0, 40).ToUpper();
            else
                enderecoSacado = infoDetalhe.EnderecoPagador.PadRight(40, ' ').ToUpper();

            if (String.IsNullOrEmpty(infoDetalhe.NomePagador))
                nomeSacado.PadRight(40, ' ');
            else if (infoDetalhe.NomePagador.Length > 40)
                nomeSacado = infoDetalhe.NomePagador.Substring(0, 40).ToUpper();
            else
                nomeSacado = infoDetalhe.NomePagador.PadRight(40, ' ').ToUpper();

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

                //021 a 037 - Identificação da Empresa Cedente no Banco
                //Deverá ser preenchido (esquerda para direita), da seguinte maneira:

                var identificadorCedente = "";
                //21 a 21 - Zero
                identificadorCedente = "0";

                //22 a 24 - código da carteira
                identificadorCedente += infoDetalhe.CarteiraCobranca.PadLeft(3, '0');

                //25 a 29 - código da Agência Cedente, sem o dígito
                identificadorCedente += infoDetalhe.Agencia.PadLeft(5, '0');

                //30 a 36 - Conta Corrente
                identificadorCedente += infoDetalhe.ContaCorrente.PadLeft(7, '0');

                //37 a 37 - dígito da Conta
                identificadorCedente += infoDetalhe.DvContaCorrente.PadLeft(1, '0');

                detalhe = detalhe.PreencherValorNaLinha(21, 37, identificadorCedente);

                #endregion

                const string doc = "DOC";
                var seuNumero = doc + infoDetalhe.NossoNumeroFormatado.PadRight(25 - doc.Length, ' ');

                detalhe = detalhe.PreencherValorNaLinha(38, 62, seuNumero);
                detalhe = detalhe.PreencherValorNaLinha(63, 65, "000");

                #region PERCENTUAL MULTA

                if (infoDetalhe.PercentualMulta.BoletoBrToStringSafe().BoletoBrToDecimal() > 0)
                    detalhe = detalhe.PreencherValorNaLinha(66, 66, "2"); // 2 - Com cobrança de multa
                else
                    detalhe = detalhe.PreencherValorNaLinha(66, 66, "0"); // 0 - Sem cobrança de multa

                // Percentual de multa
                var multaBoleto = string.Empty;

                if (infoDetalhe.PercentualMulta.ToString().Contains('.') &&
                    infoDetalhe.PercentualMulta.ToString().Contains(','))
                {
                    multaBoleto = infoDetalhe.PercentualMulta.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(67, 70, multaBoleto.PadLeft(4, '0'));
                }
                if (infoDetalhe.PercentualMulta.ToString().Contains('.'))
                {
                    multaBoleto = infoDetalhe.PercentualMulta.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(67, 70, multaBoleto.PadLeft(4, '0'));
                }
                if (infoDetalhe.PercentualMulta.ToString().Contains(','))
                {
                    multaBoleto = infoDetalhe.PercentualMulta.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(67, 70, multaBoleto.PadLeft(4, '0'));
                }

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(71, 81, infoDetalhe.NossoNumero.PadLeft(11, '0'));
                detalhe = detalhe.PreencherValorNaLinha(82, 82, infoDetalhe.DvNossoNumero);

                #region VALOR DESCONTO POR DIA

                var valorDescontoDia = string.Empty;

                if (infoDetalhe.ValorDescontoDia.ToString().Contains('.') &&
                    infoDetalhe.ValorDescontoDia.ToString().Contains(','))
                {
                    valorDescontoDia = infoDetalhe.ValorDescontoDia.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(83, 92, valorDescontoDia.PadLeft(10, '0'));
                }
                if (infoDetalhe.ValorDesconto.ToString().Contains('.'))
                {
                    valorDescontoDia = infoDetalhe.ValorDescontoDia.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(83, 92, valorDescontoDia.PadLeft(10, '0'));
                }
                if (infoDetalhe.ValorDesconto.ToString().Contains(','))
                {
                    valorDescontoDia = infoDetalhe.ValorDescontoDia.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(83, 92, valorDescontoDia.PadLeft(10, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(83, 92, valorDescontoDia.PadLeft(10, '0'));

                #endregion

                //posição 93: preencher com a condição para emissão do boleto.
                //Caso a impressão seja via banco, preencher com o número 1.
                //Caso a impressão seja via empresa, preencher com o número 2;
                if (infoDetalhe.BancoEmiteBoleto)
                    detalhe = detalhe.PreencherValorNaLinha(93, 93, "1");
                else
                    detalhe = detalhe.PreencherValorNaLinha(93, 93, "2");

                detalhe = detalhe.PreencherValorNaLinha(94, 94, "S");
                detalhe = detalhe.PreencherValorNaLinha(95, 104, string.Empty.PadLeft(10, ' '));
                detalhe = detalhe.PreencherValorNaLinha(105, 105, " ");
                detalhe = detalhe.PreencherValorNaLinha(106, 106, "2");
                detalhe = detalhe.PreencherValorNaLinha(107, 108, string.Empty.PadLeft(2, ' '));
                detalhe = detalhe.PreencherValorNaLinha(109, 110, "01");
                detalhe = detalhe.PreencherValorNaLinha(111, 120, infoDetalhe.NumeroDocumento.PadLeft(10, '0'));
                detalhe = detalhe.PreencherValorNaLinha(121, 126, infoDetalhe.DataVencimento.ToString("ddMMyy"));

                #region VALOR BOLETO

                detalhe = detalhe.PreencherValorNaLinha(127, 139,
                    infoDetalhe.ValorBoleto.ToString("f").Replace(".", "").Replace(",", "").PadLeft(13, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(140, 142, string.Empty.PadLeft(3, '0'));
                detalhe = detalhe.PreencherValorNaLinha(143, 147, string.Empty.PadLeft(5, '0'));

                detalhe = detalhe.PreencherValorNaLinha(148, 149,
                    infoDetalhe.Especie.Sigla.Equals("DM")
                        ? "01"
                        : infoDetalhe.Especie.Codigo.ToString());
                detalhe = detalhe.PreencherValorNaLinha(150, 150, infoDetalhe.Aceite.Equals("A") ? "A" : "N");
                detalhe = detalhe.PreencherValorNaLinha(151, 156, infoDetalhe.DataEmissao.ToString("ddMMyy"));

                #region INSTRUÇÕES REMESSA

                var primeiraInstrucao = infoDetalhe.Instrucao1;
                var segundaInstrucao = infoDetalhe.Instrucao2;

                if (primeiraInstrucao != null)
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, primeiraInstrucao);
                else
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, "00");
                if (segundaInstrucao != null)
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, segundaInstrucao);
                else
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, "00");

                #endregion

                #region VALOR JUROS

                // Valor de Mora Por Dia de Atraso
                /* Bradesco não passa % para juros e sim o valor calculado a ser cobrado por dia */
                var jurosBoleto = string.Empty;

                if (infoDetalhe.ValorMoraDia > 0)
                {
                    var valorCobrarJuroDia = infoDetalhe.ValorBoleto * ((infoDetalhe.ValorMoraDia / 30) / 100);
                    infoDetalhe.ValorCobradoDiaAtraso = Math.Round(valorCobrarJuroDia, 2);
                }

                if (infoDetalhe.ValorCobradoDiaAtraso.ToString().Contains('.') &&
                    infoDetalhe.ValorCobradoDiaAtraso.ToString().Contains(','))
                {
                    jurosBoleto = infoDetalhe.ValorCobradoDiaAtraso.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosBoleto.PadLeft(13, '0'));
                }
                if (infoDetalhe.ValorCobradoDiaAtraso.ToString().Contains('.'))
                {
                    jurosBoleto = infoDetalhe.ValorCobradoDiaAtraso.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosBoleto.PadLeft(13, '0'));
                }
                if (infoDetalhe.ValorCobradoDiaAtraso.ToString().Contains(','))
                {
                    jurosBoleto = infoDetalhe.ValorCobradoDiaAtraso.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosBoleto.PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosBoleto.PadLeft(13, '0'));

                #endregion

                if (infoDetalhe.DataLimiteConcessaoDesconto == DateTime.MinValue)
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, string.Empty.PadLeft(6, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(174, 179,
                        infoDetalhe.DataLimiteConcessaoDesconto.ToString("ddMMyy"));
                // Data Limite para Concesão de Desconto

                #region VALOR DESCONTO

                detalhe = detalhe.PreencherValorNaLinha(180, 192,
                    infoDetalhe.ValorDesconto.ToString("f").Replace(".", "").Replace(",", "").PadLeft(13, '0'));

                #endregion

                #region VALOR IOF

                string iofBoleto;

                if (infoDetalhe.ValorIof.ToString().Contains('.') && infoDetalhe.ValorIof.ToString().Contains(','))
                {
                    iofBoleto = infoDetalhe.ValorIof.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, iofBoleto.PadLeft(13, '0'));
                }
                if (infoDetalhe.ValorIof.ToString().Contains('.'))
                {
                    iofBoleto = infoDetalhe.ValorIof.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, iofBoleto.PadLeft(13, '0'));
                }
                if (infoDetalhe.ValorIof.ToString().Contains(','))
                {
                    iofBoleto = infoDetalhe.ValorIof.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, iofBoleto.PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(193, 205, infoDetalhe.ValorIof.ToString().PadLeft(13, '0'));
                // Valor do I.O.F. recolhido p/ notas seguro

                #endregion

                #region VALOR ABATIMENTO

                string abatimentoBoleto;

                if (infoDetalhe.ValorAbatimento.ToString().Contains('.') &&
                    infoDetalhe.ValorAbatimento.ToString().Contains(','))
                {
                    abatimentoBoleto = infoDetalhe.ValorAbatimento.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(206, 218, abatimentoBoleto.PadLeft(13, '0'));
                }
                if (infoDetalhe.ValorAbatimento.ToString().Contains('.'))
                {
                    abatimentoBoleto = infoDetalhe.ValorAbatimento.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(206, 218, abatimentoBoleto.PadLeft(13, '0'));
                }
                if (infoDetalhe.ValorAbatimento.ToString().Contains(','))
                {
                    abatimentoBoleto = infoDetalhe.ValorAbatimento.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(206, 218, abatimentoBoleto.PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(206, 218,
                    infoDetalhe.ValorAbatimento.ToString().PadLeft(13, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(219, 220,
                    infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11
                        ? "01"
                        : "02"); // Identificação do tipo de inscrição/sacado
                detalhe = detalhe.PreencherValorNaLinha(221, 234,
                    infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(235, 274, nomeSacado);
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado);

                #region 1ª Mensagem

                /* POSIÇÃO: 315 a 326 - 1ª Mensagem
                 * Campo livre para uso da empresa.
                 * A mensagem enviada nesse campo será impressa somente no boleto enão será confirmada no arquivo retorno.
                 */
                var mensagem1 = "";
                if (infoDetalhe.Instrucoes != null && infoDetalhe.Instrucoes.Count > 0)
                    mensagem1 = infoDetalhe.Instrucoes[0].TextoInstrucao;
                if (mensagem1.Trim().Length > 12)
                    mensagem1 = mensagem1.ExtrairValorDaLinha(1, 12);

                detalhe = detalhe.PreencherValorNaLinha(315, 326, mensagem1.PadRight(12, ' ')); // 1ª Mensagem

                #endregion

                var cep = infoDetalhe.CepPagador;

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

                if (infoDetalhe.NomeAvalistaOuMensagem2.BoletoBrToStringSafe().Trim().Length > 0)
                {
                    str = infoDetalhe.NomeAvalistaOuMensagem2;
                    if (str.BoletoBrToStringSafe().Trim().Length > 60)
                        str = str.ExtrairValorDaLinha(1, 60);

                    detalhe = detalhe.PreencherValorNaLinha(335, 394, str.PadLeft(60, ' '));
                }
                else
                {
                    if (infoDetalhe.Instrucoes != null && infoDetalhe.Instrucoes.Count > 1)
                    {
                        var mensagem2 = "";
                        mensagem2 = infoDetalhe.Instrucoes[1].TextoInstrucao;

                        if (mensagem2.Trim().Length > 60)
                            mensagem2 = mensagem2.ExtrairValorDaLinha(1, 60);

                        detalhe = detalhe.PreencherValorNaLinha(335, 394, mensagem2.PadRight(60, ' '));
                    }
                }

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(395, 400,
                    infoDetalhe.NumeroSequencialRegistro.ToString().PadLeft(6, '0'));

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do DETALHE do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverTrailer(TrailerRemessaCnab400 infoTrailer)
        {
            if (infoTrailer == null)
                throw new Exception("Os dados não foram informados na geração do TRAILER.");

            if (infoTrailer.NumeroSequencialRegistro == 0)
                throw new Exception("Sequencial do registro não foi informado na geração do TRAILER.");

            var trailer = new string(' ', 400);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, "9");
                trailer = trailer.PreencherValorNaLinha(2, 394, string.Empty.PadRight(393, ' '));
                trailer = trailer.PreencherValorNaLinha(395, 400,
                    infoTrailer.NumeroSequencialRegistro.ToString().PadLeft(6, '0'));

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do TRAILER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public List<string> EscreverTexto(RemessaCnab400 remessaEscrever)
        {
            List<string> listaRetornar = new List<string>();

            listaRetornar.Add(EscreverHeader(remessaEscrever.Header));

            foreach (var detalheAdicionar in remessaEscrever.RegistrosDetalhe)
            {
                listaRetornar.AddRange(new[] { EscreverDetalhe(detalheAdicionar) });
            }

            listaRetornar.Add(EscreverTrailer(remessaEscrever.Trailer));

            return listaRetornar;
        }

        public void ValidarRemessa(RemessaCnab400 remessaValidar)
        {
            throw new NotImplementedException();
        }
    }
}
