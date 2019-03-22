using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Safra
{
    public class EscritorRemessaCnab400Safra : IEscritorArquivoRemessaCnab400
    {
        private RemessaCnab400 _remessaEscrever;

        public EscritorRemessaCnab400Safra(RemessaCnab400 remessaEscrever)
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

            var agencia = infoHeader.Agencia;
            if (agencia.Length > 5) agencia = agencia.ExtrairValorDaLinha(1, 5);
            else agencia = agencia.PadRight(5, '0');
            var contaCorrente = infoHeader.ContaCorrente + infoHeader.DvContaCorrente;
            if (contaCorrente.Length > 9) contaCorrente = contaCorrente.ExtrairValorDaLinha(1, 9);
            else contaCorrente = contaCorrente.PadLeft(9, '0');

            var header = new string(' ', 400);
            try
            {
                header = header.PreencherValorNaLinha(1, 1, "0");
                header = header.PreencherValorNaLinha(2, 2, "1");
                header = header.PreencherValorNaLinha(3, 9, "REMESSA".PadRight(7, ' '));
                header = header.PreencherValorNaLinha(10, 11, "01");
                header = header.PreencherValorNaLinha(12, 19, "COBRANCA");
                header = header.PreencherValorNaLinha(20, 26, string.Empty.PadRight(7, ' '));
                header = header.PreencherValorNaLinha(27, 40, agencia + contaCorrente);
                header = header.PreencherValorNaLinha(41, 46, string.Empty.PadRight(6, ' '));
                header = header.PreencherValorNaLinha(47, 76, nomeEmpresa.PadRight(30, ' '));
                header = header.PreencherValorNaLinha(77, 79, "422");
                header = header.PreencherValorNaLinha(80, 90, "BANCO SAFRA".PadRight(11, ' '));
                header = header.PreencherValorNaLinha(91, 94, string.Empty.PadRight(4, ' '));
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("ddMMyy"));
                header = header.PreencherValorNaLinha(101, 391, string.Empty.PadRight(291, ' '));
                header = header.PreencherValorNaLinha(392, 394,
                    infoHeader.NumeroSequencialRemessa.ToString().PadLeft(3, '0'));
                header = header.PreencherValorNaLinha(395, 400, "000001");

                return header;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Falha na geração do HEADER do arquivo de REMESSA."), e);
            }
        }

        public string EscreverDetalhe(DetalheRemessaCnab400 infoDetalhe, int sequenciaRegistro, int sequenciaNumeroRemessa)
        {
            if (infoDetalhe == null)
                throw new Exception("Não há boleto para geração do DETALHE");

            #region Dados Sacado

            string enderecoSacado = string.Empty;
            string bairroSacado = string.Empty;
            string cidadeSacado = string.Empty;
            string nomeSacado = string.Empty;

            if (string.IsNullOrEmpty(infoDetalhe.EnderecoPagador))
                enderecoSacado.PadRight(40, ' ');
            else if (infoDetalhe.EnderecoPagador.Length > 40)
                enderecoSacado = infoDetalhe.EnderecoPagador.Substring(0, 40).ToUpper();
            else
                enderecoSacado = infoDetalhe.EnderecoPagador.PadRight(40, ' ').ToUpper();

            if (string.IsNullOrEmpty(infoDetalhe.BairroPagador))
                bairroSacado = bairroSacado.PadRight(10, ' ');
            else if (infoDetalhe.BairroPagador.Length > 10)
                bairroSacado = infoDetalhe.BairroPagador.Substring(0, 10);
            else
                bairroSacado = infoDetalhe.BairroPagador.PadRight(10, ' ');

            if (string.IsNullOrEmpty(infoDetalhe.CidadePagador))
                cidadeSacado = cidadeSacado.PadRight(15, ' ');
            else if (infoDetalhe.CidadePagador.Length > 15)
                cidadeSacado = infoDetalhe.CidadePagador.Substring(0, 15);
            else
                cidadeSacado = infoDetalhe.CidadePagador.PadRight(15, ' ');

            if (string.IsNullOrEmpty(infoDetalhe.NomePagador))
                nomeSacado.PadRight(40, ' ');
            else if (infoDetalhe.NomePagador.Length > 40)
                nomeSacado = infoDetalhe.NomePagador.Substring(0, 40).ToUpper();
            else
                nomeSacado = infoDetalhe.NomePagador.PadRight(40, ' ').ToUpper();

            #endregion

            var detalhe = new string(' ', 400);
            try
            {
                var primeiraInstrucao = infoDetalhe.Instrucoes.FirstOrDefault();
                var segundaInstrucao = infoDetalhe.Instrucoes.LastOrDefault();

                detalhe = detalhe.PreencherValorNaLinha(1, 1, "1");

                if (infoDetalhe.InscricaoCedente.ToString().Replace(".", "").Replace("-", "").Length == 11)
                    detalhe = detalhe.PreencherValorNaLinha(2, 3, "01");
                else
                    detalhe = detalhe.PreencherValorNaLinha(2, 3, "02");

                detalhe = detalhe.PreencherValorNaLinha(4, 17,
                    infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", "").PadRight(14, ' '));

                var identificadorCedente = "";
                identificadorCedente += infoDetalhe.Agencia.PadRight(5, '0');
                identificadorCedente += infoDetalhe.ContaCorrente.PadLeft(8, '0');
                identificadorCedente += infoDetalhe.DvContaCorrente;
                detalhe = detalhe.PreencherValorNaLinha(18, 31, identificadorCedente);

                detalhe = detalhe.PreencherValorNaLinha(32, 37, string.Empty.PadLeft(6, ' '));
                detalhe = detalhe.PreencherValorNaLinha(38, 62, string.Empty.PadLeft(25, ' '));

                detalhe = detalhe.PreencherValorNaLinha(63, 71, infoDetalhe.NossoNumeroFormatado);
                detalhe = detalhe.PreencherValorNaLinha(72, 101, string.Empty.PadLeft(30, ' '));

                /*
                0 = Isento
                1 = 2%
                2 = 4%
                 */
                detalhe = detalhe.PreencherValorNaLinha(102, 102, "0");

                /*00 = Real*/
                detalhe = detalhe.PreencherValorNaLinha(103, 104, "00");
                detalhe = detalhe.PreencherValorNaLinha(105, 105, string.Empty.PadLeft(1, ' '));

                if (infoDetalhe.NroDiasParaProtesto > 0 && segundaInstrucao?.Codigo == 10)
                    detalhe = detalhe.PreencherValorNaLinha(106, 107, infoDetalhe.NroDiasParaProtesto.BoletoBrToStringSafe().PadLeft(2, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(106, 107, "00");

                detalhe = detalhe.PreencherValorNaLinha(108, 108, infoDetalhe.CarteiraCobranca);

                detalhe = detalhe.PreencherValorNaLinha(109, 110,
                    infoDetalhe.CodigoOcorrencia.Codigo.BoletoBrToStringSafe().PadLeft(2, '0'));

                detalhe = detalhe.PreencherValorNaLinha(111, 120, infoDetalhe.NossoNumero.PadRight(10, ' '));

                detalhe = detalhe.PreencherValorNaLinha(121, 126, infoDetalhe.DataVencimento.ToString("ddMMyy"));

                #region VALOR BOLETO

                detalhe = detalhe.PreencherValorNaLinha(127, 139,
                    infoDetalhe.ValorBoleto.ToString("f").Replace(".", "").Replace(",", "").PadLeft(13, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(140, 142, "422");
                detalhe = detalhe.PreencherValorNaLinha(143, 147, infoDetalhe.Agencia.PadRight(5, '0'));

                detalhe = detalhe.PreencherValorNaLinha(148, 149,
                    infoDetalhe.Especie.Sigla.Equals("DM") ? "01" : infoDetalhe.Especie.Codigo.ToString());

                detalhe = detalhe.PreencherValorNaLinha(150, 150, infoDetalhe.Aceite.Equals("A") ? "A" : "N");

                detalhe = detalhe.PreencherValorNaLinha(151, 156, infoDetalhe.DataEmissao.ToString("ddMMyy"));

                #region INSTRUÇÕES REMESSA

                if (primeiraInstrucao != null && primeiraInstrucao.Codigo > 0)
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, primeiraInstrucao.Codigo.BoletoBrToStringSafe().PadLeft(2, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, "00");

                if (segundaInstrucao != null && segundaInstrucao.Codigo > 0)
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, segundaInstrucao.Codigo.BoletoBrToStringSafe().PadLeft(2, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, "00");

                #endregion

                #region VALOR JUROS

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

                #region VALOR DESCONTO / DATA LIMITE DESCONTO

                if (infoDetalhe.DataLimiteConcessaoDesconto == DateTime.MinValue)
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, string.Empty.PadLeft(6, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(174, 179,
                        infoDetalhe.DataLimiteConcessaoDesconto.ToString("ddMMyy"));

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

                #endregion

                #region PERCENTUAL MULTA

                // Percentual de multa
                if (infoDetalhe.PercentualMulta > 0)
                {
                    detalhe = detalhe.PreencherValorNaLinha(206, 211,
                        infoDetalhe.DataVencimento.AddDays(1).ToString("ddMMyy"));

                    var multaBoleto = string.Empty;

                    if (infoDetalhe.PercentualMulta.ToString().Contains('.') &&
                        infoDetalhe.PercentualMulta.ToString().Contains(','))
                    {
                        multaBoleto = infoDetalhe.PercentualMulta.ToString().Replace(".", "").Replace(",", "");
                        detalhe = detalhe.PreencherValorNaLinha(212, 215, multaBoleto.PadLeft(4, '0'));
                    }
                    else if (infoDetalhe.PercentualMulta.ToString().Contains('.'))
                    {
                        multaBoleto = infoDetalhe.PercentualMulta.ToString().Replace(".", "");
                        detalhe = detalhe.PreencherValorNaLinha(212, 215, multaBoleto.PadLeft(4, '0'));
                    }
                    else if (infoDetalhe.PercentualMulta.ToString().Contains(','))
                    {
                        multaBoleto = infoDetalhe.PercentualMulta.ToString().Replace(",", "");
                        detalhe = detalhe.PreencherValorNaLinha(212, 215, multaBoleto.PadLeft(4, '0'));
                    }

                    //Informar instrução de Multa
                    if (infoDetalhe.PercentualMulta > 0)
                    {
                        if (primeiraInstrucao == null || primeiraInstrucao.Codigo == 0)
                            detalhe = detalhe.PreencherValorNaLinha(157, 158, "16"); //Instrução de Multa
                        else if (segundaInstrucao == null || segundaInstrucao.Codigo == 0)
                            detalhe = detalhe.PreencherValorNaLinha(159, 160, "16"); //Instrução de Multa
                    }

                    detalhe = detalhe.PreencherValorNaLinha(216, 218, "000");
                }
                else
                    detalhe = detalhe.PreencherValorNaLinha(206, 218, string.Empty.PadLeft(13, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(219, 220,
                    infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11
                        ? "01"
                        : "02"); // Identificação do tipo de inscrição/sacado
                detalhe = detalhe.PreencherValorNaLinha(221, 234,
                    infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(235, 274, nomeSacado);
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado);
                detalhe = detalhe.PreencherValorNaLinha(315, 324, bairroSacado);

                detalhe = detalhe.PreencherValorNaLinha(325, 326, string.Empty.PadLeft(2, ' '));

                var cep = infoDetalhe.CepPagador;

                if (cep.Contains(".") && cep.Contains("-"))
                    cep = cep.Replace(".", "").Replace("-", "");
                if (cep.Contains("."))
                    cep = cep.Replace(".", "");
                if (cep.Contains("-"))
                    cep = cep.Replace("-", "");
                detalhe = detalhe.PreencherValorNaLinha(327, 334, cep.PadLeft(8, ' ')); // Cep do Sacado
                detalhe = detalhe.PreencherValorNaLinha(335, 349, cidadeSacado);
                detalhe = detalhe.PreencherValorNaLinha(350, 351,
                    infoDetalhe.UfPagador.Length > 2
                        ? infoDetalhe.UfPagador.Substring(0, 2)
                        : infoDetalhe.UfPagador.PadRight(2, ' '));

                #region Sacador Avalista / Mensagem

                // Caso não tenha sacador/avalista informar mensagem no campo
                if (infoDetalhe.NomeAvalistaOuMensagem2.BoletoBrToStringSafe().Trim().Length > 0)
                {
                    // Mensagem
                    if (primeiraInstrucao != null &&
                        primeiraInstrucao.Codigo.BoletoBrToStringSafe().BoletoBrToInt() == 0)
                    {
                        var instrucao1 = primeiraInstrucao.TextoInstrucao;
                        if (instrucao1.BoletoBrToStringSafe().Trim().Length > 30)
                            instrucao1 = instrucao1.ExtrairValorDaLinha(1, 30);

                        detalhe = detalhe.PreencherValorNaLinha(352, 381, instrucao1.PadRight(30, ' '));
                    }
                    else if (segundaInstrucao != null &&
                             segundaInstrucao.Codigo.BoletoBrToStringSafe().BoletoBrToInt() == 0)
                    {
                        var instrucao2 = segundaInstrucao.TextoInstrucao;
                        if (instrucao2.BoletoBrToStringSafe().Trim().Length > 30)
                            instrucao2 = instrucao2.ExtrairValorDaLinha(1, 30);

                        detalhe = detalhe.PreencherValorNaLinha(352, 381, instrucao2.PadRight(30, ' '));
                    }
                }
                else
                {
                    // Nome do Sacador ou Avalista
                    detalhe = detalhe.PreencherValorNaLinha(352, 381,string.Empty.PadLeft(30, ' '));/* SE HOUVER MENSAGENS ESPECÍFICAS, TÍTULO A TÍTULO, UTILIZAR CAMPO "SACADOR / AVALISTA" COMO MENSAGEM (SOMENTE AS 28 PRIMEIRAS POSIÇÕES). CASO DESEJE */
                }

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(382, 388, string.Empty.PadRight(7, ' '));

                detalhe = detalhe.PreencherValorNaLinha(389, 391, "422");

                detalhe = detalhe.PreencherValorNaLinha(392, 394,
                    sequenciaNumeroRemessa.BoletoBrToStringSafe().PadLeft(3, '0'));
                detalhe = detalhe.PreencherValorNaLinha(395, 400,
                    sequenciaRegistro.BoletoBrToStringSafe().PadLeft(6, '0'));

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do DETALHE do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverTrailer(TrailerRemessaCnab400 infoTrailer, int sequenciaRegistro,
            int quantidadeTitulosArquivo, int sequencialNumeroRemessa)
        {
            if (infoTrailer == null)
                throw new Exception("Os dados não foram informados na geração do TRAILER.");

            var trailer = new string(' ', 400);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, "9");
                trailer = trailer.PreencherValorNaLinha(2, 368, string.Empty.PadRight(367, ' '));
                trailer = trailer.PreencherValorNaLinha(369, 376,
                    quantidadeTitulosArquivo.BoletoBrToStringSafe().PadLeft(8, '0'));
                trailer = trailer.PreencherValorNaLinha(377, 391,
                    infoTrailer.ValorTotalTitulos.ToString("f").Replace(".", "").Replace(",", "").PadLeft(15, '0'));
                trailer = trailer.PreencherValorNaLinha(392, 394,
                    sequencialNumeroRemessa.BoletoBrToStringSafe().PadLeft(3, '0'));
                trailer = trailer.PreencherValorNaLinha(395, 400, sequenciaRegistro.ToString().PadLeft(6, '0'));

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

            var sequencia = 2;
            foreach (var detalheAdicionar in remessaEscrever.RegistrosDetalhe)
            {
                listaRetornar.AddRange(new[]
                {EscreverDetalhe(detalheAdicionar, sequencia, remessaEscrever.Header.NumeroSequencialRemessa)});
                sequencia++;
            }

            listaRetornar.Add(EscreverTrailer(remessaEscrever.Trailer, sequencia, remessaEscrever.RegistrosDetalhe.Count,
                remessaEscrever.Header.NumeroSequencialRemessa));

            return listaRetornar;
        }

        public void ValidarRemessa(RemessaCnab400 remessaValidar)
        {
            throw new NotImplementedException();
        }
    }
}
