using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Santander
{
    public class EscritorRemessaCnab400Santander : IEscritorArquivoRemessaCnab400
    {
        private RemessaCnab400 _remessaEscrever;
        private int _numeroSequencialDeRegistro = 0;
        private int _numeroAtualDeRegistro = 0;

        public EscritorRemessaCnab400Santander(RemessaCnab400 remessaEscrever)
        {
            _remessaEscrever = remessaEscrever;
        }

        public string EscreverHeader(HeaderRemessaCnab400 infoHeader, int numeroRegistro)
        {
            var nomeEmpresa = "";
            if (infoHeader.NomeEmpresa.Length > 30)
                nomeEmpresa = infoHeader.NomeEmpresa.Substring(0, 30);
            else
                nomeEmpresa = infoHeader.NomeEmpresa.PadRight(30, ' ');

            var header = new string(' ', 400);
            try
            {
                header = header.PreencherValorNaLinha(1, 1, "0");
                header = header.PreencherValorNaLinha(2, 2, "1");
                header = header.PreencherValorNaLinha(3, 9, "REMESSA");
                header = header.PreencherValorNaLinha(10, 11, "01");
                header = header.PreencherValorNaLinha(12, 26, "COBRANCA".PadRight(15, ' '));

                // Código de Transmissão fornecido pelo Banco
                if (String.IsNullOrEmpty(infoHeader.CodigoDeTransmissao))
                    header = header.PreencherValorNaLinha(27, 46, string.Empty.PadLeft(20, '0'));
                else
                    header = header.PreencherValorNaLinha(27, 46, infoHeader.CodigoDeTransmissao.PadLeft(20, '0'));

                header = header.PreencherValorNaLinha(47, 76, nomeEmpresa.ToUpper().PadRight(30, ' '));
                header = header.PreencherValorNaLinha(77, 79, "033");
                header = header.PreencherValorNaLinha(80, 94, "SANTANDER".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("ddMMyy").Replace("/", ""));
                header = header.PreencherValorNaLinha(101, 116, string.Empty.PadLeft(16, '0'));
                // Mensagem 1 a Mensagem 6
                header = header.PreencherValorNaLinha(117, 391, string.Empty.PadRight(275, ' '));
                header = header.PreencherValorNaLinha(392, 394, infoHeader.NumeroSequencialRemessa.ToString().PadLeft(3, '0'));
                header = header.PreencherValorNaLinha(395, 400, numeroRegistro.ToString().PadLeft(6, '0'));

                return header;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("BoletoBr{0}Falha na geração do HEADER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverDetalhe(DetalheRemessaCnab400 infoDetalhe, int numeroRegistro)
        {
            int identificadorMulta = 0;
            if (infoDetalhe.PercentualMulta > 0)
                identificadorMulta = 4;

            /* 
             *  Códigos de Carteira
             *  101 - Banco Emite - 1 - Eletrônica com registro
             *  101 - Beneficiário Emite - 5 - Rápida com registro
             *  201 - Banco Emite - 3 - Caucionada eletrônica
             *  201 - Beneficiário Emite - 6 - Caucionada rápida
             *  102 -  4 - Cobrança sem registro
             *  104 - 7 - Descontada eletrônica
             */

            /*
             * 033 - Banco Santander
             * Carteira 101 - Cobrança Simples Rápida com Registro - RCR
             * Carteira 102 - Cobrança Simples sem Registro - CSR
             * Carteira 104 - Cobrança Simples Rápida com Registro - RCR - Banco emite
             * Carteira 201 - Cobrança Penhor Rápida com Registro - RCR
             * Carteira COB - Cobrança simples, sem registro (antiga)
             * Carteira CSR - Cobrança Simples sem Registro (antiga)
             * Carteira ECR - Cobrança Simples com Registro (antiga)
             */

            var codigoCarteira = string.Empty;

            if (infoDetalhe.CarteiraCobranca == "101")
            {
                if (infoDetalhe.BancoEmiteBoleto)
                    codigoCarteira = "1";
                else
                    codigoCarteira = "5";
            }
            if (infoDetalhe.CarteiraCobranca == "102")
                codigoCarteira = "4";
            if (infoDetalhe.CarteiraCobranca == "104")
                codigoCarteira = "7";
            if (infoDetalhe.CarteiraCobranca == "201")
            {
                if (infoDetalhe.BancoEmiteBoleto)
                    codigoCarteira = "3";
                else
                    codigoCarteira = "6";
                
            }

        string enderecoSacado = string.Empty;
            string bairroSacado = string.Empty;
            string cidadeSacado = string.Empty;

            string nomeSacado = string.Empty;

            if (String.IsNullOrEmpty(infoDetalhe.EnderecoPagador))
                enderecoSacado = enderecoSacado.PadRight(40, ' ');
            else if (infoDetalhe.EnderecoPagador.Length > 40)
                enderecoSacado = infoDetalhe.EnderecoPagador.Substring(0, 40).ToUpper();
            else
                enderecoSacado = infoDetalhe.EnderecoPagador.PadRight(40, ' ').ToUpper();

            if (String.IsNullOrEmpty(infoDetalhe.BairroPagador))
                bairroSacado = bairroSacado.PadRight(12, ' ');
            else if (infoDetalhe.BairroPagador.Length > 12)
                bairroSacado = infoDetalhe.BairroPagador.Substring(0, 12).ToUpper();
            else
                bairroSacado = infoDetalhe.BairroPagador.PadRight(12, ' ').ToUpper();

            if (String.IsNullOrEmpty(infoDetalhe.CidadePagador))
                cidadeSacado = cidadeSacado.PadRight(15, ' ');
            else if (infoDetalhe.CidadePagador.Length > 15)
                cidadeSacado = infoDetalhe.CidadePagador.Substring(0, 15).ToUpper();
            else
                cidadeSacado = infoDetalhe.CidadePagador.PadRight(15, ' ').ToUpper();

            if (String.IsNullOrEmpty(infoDetalhe.NomePagador))
                nomeSacado = nomeSacado.PadRight(40, ' ');
            else if (infoDetalhe.NomePagador.Length > 40)
                nomeSacado = infoDetalhe.NomePagador.Substring(0, 40).ToUpper();
            else
                nomeSacado = infoDetalhe.NomePagador.PadRight(40, ' ').ToUpper();

            string complementoRegistro =
                infoDetalhe.ContaCorrente.Substring(infoDetalhe.ContaCorrente.Length - 1, 1) +
                infoDetalhe.DvContaCorrente;

            var detalhe = new string(' ', 400);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, "1");

                // 01 - CPF / 02 - CNPJ
                detalhe = detalhe.PreencherValorNaLinha(2, 3, infoDetalhe.InscricaoCedente.Length == 11 ? "01" : "02");
                detalhe = detalhe.PreencherValorNaLinha(4, 17, infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(18, 37, infoDetalhe.CodigoDeTransmissao.PadLeft(20, '0')); //Versão 2.0 do layout

                const string doc = "DOC";
                var seuNumero = doc + infoDetalhe.NossoNumeroFormatado.PadRight(25 - doc.Length, ' ');

                detalhe = detalhe.PreencherValorNaLinha(38, 62, seuNumero);

                // NossoNumero com DV, pegar os 8 primeiros dígitos, da direita para esquerda (para CNAB 400)
                detalhe = detalhe.PreencherValorNaLinha(63, 70, infoDetalhe.NossoNumeroFormatado.Replace("-", "").Substring(5, 8));

                if (infoDetalhe.DataLimiteConcessaoDesconto == DateTime.MinValue)
                    detalhe = detalhe.PreencherValorNaLinha(71, 76, string.Empty.PadLeft(6, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(71, 76, infoDetalhe.DataLimiteConcessaoDesconto.ToString("ddMMyy"));

                detalhe = detalhe.PreencherValorNaLinha(77, 77, string.Empty.PadLeft(1, ' '));
                detalhe = detalhe.PreencherValorNaLinha(78, 78, identificadorMulta.ToString());

                if (String.IsNullOrEmpty(infoDetalhe.PercentualMulta.ToString()))
                    detalhe = detalhe.PreencherValorNaLinha(79, 82, string.Empty.PadLeft(4, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(79, 82, infoDetalhe.PercentualMulta.ToString("f").Replace(",", "").PadLeft(4, '0'));

                detalhe = detalhe.PreencherValorNaLinha(83, 84, "00");
                detalhe = detalhe.PreencherValorNaLinha(85, 97, string.Empty.PadLeft(13, '0')); /*Vl do título em outra unidade (consultar banco)*/
                detalhe = detalhe.PreencherValorNaLinha(98, 101, string.Empty.PadLeft(4, ' '));

                detalhe = detalhe.PreencherValorNaLinha(102, 107, "000000");

                detalhe = detalhe.PreencherValorNaLinha(108, 108, codigoCarteira);
                detalhe = detalhe.PreencherValorNaLinha(109, 110, "01"); /* Código da Ocorrência*/
                detalhe = detalhe.PreencherValorNaLinha(111, 120, infoDetalhe.NumeroDocumento.PadRight(10, ' '));
                detalhe = detalhe.PreencherValorNaLinha(121, 126, infoDetalhe.DataVencimento.ToString("ddMMyy"));

                #region VALOR TOTAL

                detalhe = detalhe.PreencherValorNaLinha(127, 139, infoDetalhe.ValorBoleto.ToString("f").Replace(",", "").PadLeft(13, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(140, 142, "033");

                if (codigoCarteira == "5")
                    detalhe = detalhe.PreencherValorNaLinha(143, 147, infoDetalhe.Agencia.PadLeft(5, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(143, 147, string.Empty.PadLeft(5, '0'));

                detalhe = detalhe.PreencherValorNaLinha(148, 149, infoDetalhe.Especie.Sigla.Equals("DM") ? "01" : infoDetalhe.Especie.Codigo.ToString(CultureInfo.InvariantCulture));
                detalhe = detalhe.PreencherValorNaLinha(150, 150, "N");
                detalhe = detalhe.PreencherValorNaLinha(151, 156, infoDetalhe.DataEmissao.ToString("ddMMyy"));

                #region INSTRUÇÕES REMESSA

                var primeiraInstrucao = infoDetalhe.Instrucao1;
                if (primeiraInstrucao != null)
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, primeiraInstrucao);
                else
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, "00");
                /*
                 *  00 = NÃO HÁ INSTRUÇÕES
                 *  02 = BAIXAR APÓS QUINZE DIAS DO VENCIMENTO
                 *  03 = BAIXAR APÓS 30 DIAS DO VENCIMENTO
                 *  04 = NÃO BAIXAR
                 *  06 = PROTESTAR(VIDE POSIÇÃO392 / 393)
                 *  07 = NÃO PROTESTAR
                 *  08 = NÃO COBRAR JUROS DE MORA
                 */
                if (infoDetalhe.NroDiasParaProtesto > 0)
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, infoDetalhe.CodigoProtesto.ToString().PadLeft(2,'0'));
                else
                if (infoDetalhe.PrazoBaixaDevolucao == 15)
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, "02");
                else
                if (infoDetalhe.PrazoBaixaDevolucao == 30)
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, "03");
                else
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, "00");
                #endregion

                #region JUROS

                var jurosPorDia = string.Empty;

                if (infoDetalhe.ValorMoraDia > 0)
                {
                    var valorCobrarJuroDia = infoDetalhe.ValorBoleto * ((infoDetalhe.ValorMoraDia / 30) / 100);
                    infoDetalhe.ValorMoraDia = Math.Round(valorCobrarJuroDia, 2);
                }

                if (infoDetalhe.ValorMoraDia.ToString().Contains('.') &&
                    infoDetalhe.ValorMoraDia.ToString().Contains(','))
                {
                    jurosPorDia = infoDetalhe.ValorMoraDia.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosPorDia.PadLeft(13, '0'));
                }
                if (infoDetalhe.ValorMoraDia.ToString().Contains('.'))
                {
                    jurosPorDia = infoDetalhe.ValorMoraDia.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosPorDia.PadLeft(13, '0'));
                }
                if (infoDetalhe.ValorMoraDia.ToString().Contains(','))
                {
                    jurosPorDia = infoDetalhe.ValorMoraDia.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosPorDia.PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(161, 173, infoDetalhe.ValorMoraDia.ToString("f").Replace(",", "").PadLeft(13, '0'));

                #endregion

                #region DESCONTO

                if (infoDetalhe.DataLimiteConcessaoDesconto == DateTime.MinValue)
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, string.Empty.PadLeft(6, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, infoDetalhe.DataLimiteConcessaoDesconto.ToString("ddMMyy"));

                detalhe = detalhe.PreencherValorNaLinha(180, 192, infoDetalhe.ValorDesconto.ToString("f").Replace(",", "").PadLeft(13, '0'));

                #endregion

                #region IOF

                detalhe = detalhe.PreencherValorNaLinha(193, 205, infoDetalhe.ValorIof.ToString("f").Replace(",", "").PadLeft(13, '0'));

                #endregion

                #region ABATIMENTO

                detalhe = detalhe.PreencherValorNaLinha(206, 218, infoDetalhe.ValorAbatimento.ToString("f").Replace(",", "").PadLeft(13, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(219, 220, infoDetalhe.InscricaoPagador.Length == 11 ? "01" : "02");
                detalhe = detalhe.PreencherValorNaLinha(221, 234, infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(235, 274, nomeSacado);
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado);
                detalhe = detalhe.PreencherValorNaLinha(315, 326, bairroSacado);

                #region CEP

                var Cep = infoDetalhe.CepPagador;

                if (Cep.Contains(".") && Cep.Contains("-"))
                    Cep = Cep.Replace(".", "").Replace("-", "");
                if (Cep.Contains("."))
                    Cep = Cep.Replace(".", "");
                if (Cep.Contains("-"))
                    Cep = Cep.Replace("-", "");

                if (Cep.Length > 8) throw new Exception("Cep inválido." + Cep);

                detalhe = detalhe.PreencherValorNaLinha(327, 334, Cep.PadLeft(8, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(335, 349, cidadeSacado);
                detalhe = detalhe.PreencherValorNaLinha(350, 351, infoDetalhe.UfPagador.PadRight(2, ' '));

                if (String.IsNullOrEmpty(infoDetalhe.NomeAvalistaOuMensagem2))
                    detalhe = detalhe.PreencherValorNaLinha(352, 381, string.Empty.PadRight(30, ' '));
                else
                    detalhe = detalhe.PreencherValorNaLinha(352, 381, infoDetalhe.NomeAvalistaOuMensagem2.PadRight(30, ' '));

                detalhe = detalhe.PreencherValorNaLinha(382, 382, " ");
                detalhe = detalhe.PreencherValorNaLinha(383, 383, "i".ToUpper()); // Identificador do Complemento
                detalhe = detalhe.PreencherValorNaLinha(384, 385, complementoRegistro);
                detalhe = detalhe.PreencherValorNaLinha(386, 391, string.Empty.PadLeft(6, ' '));
                detalhe = detalhe.PreencherValorNaLinha(392, 393, infoDetalhe.NroDiasParaProtesto.ToString().PadLeft(2, '0'));
                detalhe = detalhe.PreencherValorNaLinha(394, 394, " ");

                detalhe = detalhe.PreencherValorNaLinha(395, 400, numeroRegistro.ToString().PadLeft(6, '0'));

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do DETALHE do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        /// <summary>
        /// Método que escreve a mensagem variável com código de registro 2 (Recibo Sacado)
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string EscreverMensagemVariavelReciboSacado(DetalheRemessaCnab400 info, int numeroRegistro)
        {
            string complementoRegistro =
                info.ContaCorrente.Remove(0, info.ContaCorrente.Length - 1) +
                info.DvContaCorrente;

            // A primeira instrução é a instrução do sacado conforme layout do SANTANDER
            var primeiraInstrucao = info.Instrucoes.FirstOrDefault();

            var msgVariavel = new string(' ', 400);
            try
            {
                msgVariavel = msgVariavel.PreencherValorNaLinha(1, 1, "2");
                msgVariavel = msgVariavel.PreencherValorNaLinha(2, 17, string.Empty.PadRight(16, ' ')); // Uso do Banco
                msgVariavel = msgVariavel.PreencherValorNaLinha(18, 37, info.CodigoDeTransmissao.PadLeft(20, '0'));
                msgVariavel = msgVariavel.PreencherValorNaLinha(38, 47, string.Empty.PadRight(10, ' ')); // Uso do Banco
                msgVariavel = msgVariavel.PreencherValorNaLinha(48, 49, "01");
                msgVariavel = msgVariavel.PreencherValorNaLinha(50, 99, primeiraInstrucao != null
                    ? primeiraInstrucao.TextoInstrucao.PadRight(50, ' ')
                    : string.Empty.PadRight(50, ' '));
                msgVariavel = msgVariavel.PreencherValorNaLinha(100, 382, string.Empty.PadRight(283, ' '));

                // Uso do Banco
                msgVariavel = msgVariavel.PreencherValorNaLinha(383, 383, "i".ToUpper());
                msgVariavel = msgVariavel.PreencherValorNaLinha(384, 385, complementoRegistro);
                msgVariavel = msgVariavel.PreencherValorNaLinha(386, 394, string.Empty.PadLeft(9, ' '));

                msgVariavel = msgVariavel.PreencherValorNaLinha(395, 400, numeroRegistro.ToString().PadLeft(6, '0'));
            }
            catch (Exception e)
            {
                throw new Exception(
                            string.Format(
                                "<BoletoBr>{0}Falha na geração das MENSAGENS PARA O BOLETO no arquivo de REMESSA.",
                                Environment.NewLine), e);
            }

            return msgVariavel;
        }

        /// <summary>
        /// Método que escreve a mensagem variável com código de registro 3 (Ficha de Compensação)
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string EscreverMensagemVariavelFichaCompensacao(DetalheRemessaCnab400 info, ref int numeroRegistro)
        {
            var listaLinhas = PreparaLinhaMensagemVariavel(info, ref numeroRegistro);
            string registroAtual = string.Empty;

            if (listaLinhas != null)
            {
                foreach (var linha in listaLinhas)
                {
                    if (listaLinhas.Last() == linha)
                        registroAtual += linha;
                    else
                        registroAtual += linha + Environment.NewLine;
                }
            }

            return registroAtual;
        }

        public List<string> PreparaLinhaMensagemVariavel(DetalheRemessaCnab400 info, ref int numeroRegistro)
        {
            string complementoRegistro =
                info.ContaCorrente.Remove(0, info.ContaCorrente.Length - 1) +
                info.DvContaCorrente;

            var lista = new List<string>();
            if (info.Instrucoes.Count > 0)
            {
                info.Instrucoes.RemoveAt(0);
                foreach (var instrucaoAtual in info.Instrucoes)
                {
                    numeroRegistro++;

                    // Primeira Mensagem (Mensagem 1) Variável que irá constar na linha 2 do detalhe atual
                    var msgVariavel = new string(' ', 400);
                    try
                    {
                        msgVariavel = msgVariavel.PreencherValorNaLinha(1, 1, "2");
                        msgVariavel = msgVariavel.PreencherValorNaLinha(2, 17, string.Empty.PadRight(16, ' ')); // Uso do Banco
                        msgVariavel = msgVariavel.PreencherValorNaLinha(18, 37, info.CodigoDeTransmissao.PadLeft(20, '0'));
                        msgVariavel = msgVariavel.PreencherValorNaLinha(38, 47, string.Empty.PadRight(10, ' ')); // Uso do Banco
                        msgVariavel = msgVariavel.PreencherValorNaLinha(48, 49, "01");
                        msgVariavel = msgVariavel.PreencherValorNaLinha(50, 99, instrucaoAtual != null
                            ? instrucaoAtual.TextoInstrucao.PadRight(50, ' ')
                            : string.Empty.PadRight(50, ' '));
                        msgVariavel = msgVariavel.PreencherValorNaLinha(100, 382, string.Empty.PadRight(283, ' '));

                        // Uso do Banco
                        msgVariavel = msgVariavel.PreencherValorNaLinha(383, 383, "i".ToUpper());
                        msgVariavel = msgVariavel.PreencherValorNaLinha(384, 385, complementoRegistro);
                        msgVariavel = msgVariavel.PreencherValorNaLinha(386, 394, string.Empty.PadLeft(9, ' '));

                        msgVariavel = msgVariavel.PreencherValorNaLinha(395, 400, numeroRegistro.ToString().PadLeft(6, '0'));

                        lista.Add(msgVariavel);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(
                            string.Format(
                                "<BoletoBr>{0}Falha na geração das MENSAGENS PARA O BOLETO no arquivo de REMESSA.",
                                Environment.NewLine), e);
                    }
                }
            }

            return lista;
        }

        public string EscreverTrailer(TrailerRemessaCnab400 infoTrailer, int numeroRegistro)
        {
            var trailer = new string(' ', 400);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, "9");
                trailer = trailer.PreencherValorNaLinha(2, 7, numeroRegistro.ToString().PadLeft(6, '0'));
                trailer = trailer.PreencherValorNaLinha(8, 20, infoTrailer.ValorTotalTitulos.ToString("f").Replace(",", "").PadLeft(13, '0') /*Valor total dos títulos*/);
                trailer = trailer.PreencherValorNaLinha(21, 394, string.Empty.PadRight(374, '0'));
                trailer = trailer.PreencherValorNaLinha(395, 400, numeroRegistro.ToString().PadLeft(6, '0'));

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

            var numeroRegistro = 1;
            listaRetornar.Add(EscreverHeader(remessaEscrever.Header, numeroRegistro));

            foreach (var detalheAdicionar in remessaEscrever.RegistrosDetalhe)
            {
                numeroRegistro++;
                listaRetornar.AddRange(new[] { EscreverDetalhe(detalheAdicionar, numeroRegistro) });

                if (detalheAdicionar.Instrucoes.Count > 0)
                {
                    numeroRegistro++;
                    listaRetornar.AddRange(new[] { EscreverMensagemVariavelReciboSacado(detalheAdicionar, numeroRegistro) });
                }
                if (detalheAdicionar.Instrucoes.Count > 1)
                    listaRetornar.AddRange(new[] { EscreverMensagemVariavelFichaCompensacao(detalheAdicionar, ref numeroRegistro) });
            }

            numeroRegistro++;
            listaRetornar.Add(EscreverTrailer(remessaEscrever.Trailer, numeroRegistro));

            return listaRetornar;
        }

        public void ValidarRemessa(RemessaCnab400 remessaValidar)
        {
            throw new NotImplementedException();
        }
    }
}
