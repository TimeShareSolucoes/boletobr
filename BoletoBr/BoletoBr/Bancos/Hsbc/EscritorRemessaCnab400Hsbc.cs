using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Hsbc
{
    public class EscritorRemessaCnab400Hsbc : IEscritorArquivoRemessaCnab400
    {
        private RemessaCnab400 _remessaEscrever;

        public EscritorRemessaCnab400Hsbc(RemessaCnab400 remessaEscrever)
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

            var contaCorrente = string.Empty;
            contaCorrente = infoHeader.Agencia.PadLeft(4, '0');
            contaCorrente += infoHeader.ContaCorrente.PadLeft(5, '0');
            contaCorrente += infoHeader.DvContaCorrente.PadLeft(2, '0');

            var header = new string(' ', 400);
            try
            {
                header = header.PreencherValorNaLinha(1, 1, "0");
                header = header.PreencherValorNaLinha(2, 2, "1");
                header = header.PreencherValorNaLinha(3, 9, "REMESSA".PadRight(7, ' '));
                header = header.PreencherValorNaLinha(10, 11, "01");
                header = header.PreencherValorNaLinha(12, 26, "COBRANCA".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(27, 27, "0");
                header = header.PreencherValorNaLinha(28, 31, infoHeader.Agencia.PadLeft(4, '0'));
                header = header.PreencherValorNaLinha(32, 33, "55");
                header = header.PreencherValorNaLinha(34, 44, contaCorrente);
                header = header.PreencherValorNaLinha(45, 46, string.Empty.PadRight(2, ' '));
                header = header.PreencherValorNaLinha(47, 76, nomeEmpresa.ToUpper());
                header = header.PreencherValorNaLinha(77, 79, "399");
                header = header.PreencherValorNaLinha(80, 94, "HSBC".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(95, 100, infoHeader.DataDeGravacao.ToString("ddMMyy"));
                header = header.PreencherValorNaLinha(101, 105, "01600");
                header = header.PreencherValorNaLinha(106, 108, "BPI");
                header = header.PreencherValorNaLinha(109, 110, string.Empty.PadRight(2, ' '));
                header = header.PreencherValorNaLinha(111, 117, "LANCV08");
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
                enderecoSacado.PadRight(38, ' ');
            else if (infoDetalhe.EnderecoPagador.Length > 38)
                enderecoSacado = infoDetalhe.EnderecoPagador.Substring(0, 38).ToUpper();
            else
                enderecoSacado = infoDetalhe.EnderecoPagador.PadRight(38, ' ').ToUpper();

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

                var tipoIncricaoCedente = "99"; //99 - Outros
                if (infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11)
                    tipoIncricaoCedente = "01"; //01 - CPF
                else if (infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11)
                    tipoIncricaoCedente = "02"; //02 - CNPJ
                else if (infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", "").Length == 0)
                    tipoIncricaoCedente = "98"; //98 - Não tem
                detalhe = detalhe.PreencherValorNaLinha(2, 3, tipoIncricaoCedente);

                detalhe = detalhe.PreencherValorNaLinha(4, 17,
                    infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));

                detalhe = detalhe.PreencherValorNaLinha(18, 18, "0"); //Numérico igual a “0” (zero)

                detalhe = detalhe.PreencherValorNaLinha(19, 22, infoDetalhe.Agencia.PadLeft(4, '0'));
                detalhe = detalhe.PreencherValorNaLinha(23, 24, "55"); //Numérico igual a “55”
                detalhe = detalhe.PreencherValorNaLinha(25, 35,
                    infoDetalhe.Agencia.PadLeft(4, '0') + infoDetalhe.ContaCorrente.PadLeft(5, '0') +
                    infoDetalhe.DvContaCorrente.PadLeft(2, '0'));
                detalhe = detalhe.PreencherValorNaLinha(36, 37, string.Empty.PadLeft(2, ' '));

                const string doc = "DOC";
                var seuNumero = doc + infoDetalhe.NossoNumeroFormatado.PadRight(25 - doc.Length, ' ');
                detalhe = detalhe.PreencherValorNaLinha(38, 62, seuNumero);
                detalhe = detalhe.PreencherValorNaLinha(63, 73, infoDetalhe.NossoNumeroFormatado.PadLeft(11, '0'));

                #region DESCONTO 2/3

                detalhe = detalhe.PreencherValorNaLinha(74, 79, string.Empty.PadLeft(6, '0'));
                detalhe = detalhe.PreencherValorNaLinha(80, 90, string.Empty.PadLeft(11, '0'));

                detalhe = detalhe.PreencherValorNaLinha(91, 96, string.Empty.PadLeft(6, '0'));
                detalhe = detalhe.PreencherValorNaLinha(97, 107, string.Empty.PadLeft(11, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(108, 108, "1"); //1 - Cobrança Simples
                detalhe = detalhe.PreencherValorNaLinha(109, 110, "01");
                detalhe = detalhe.PreencherValorNaLinha(111, 120,
                    infoDetalhe.NumeroDocumento.BoletoBrToInt().BoletoBrToStringSafe().PadLeft(10, '0'));
                detalhe = detalhe.PreencherValorNaLinha(121, 126, infoDetalhe.DataVencimento.ToString("ddMMyy"));

                #region VALOR BOLETO

                detalhe = detalhe.PreencherValorNaLinha(127, 139,
                    infoDetalhe.ValorBoleto.ToString("f").Replace(".", "").Replace(",", "").PadLeft(13, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(140, 142, "399");
                detalhe = detalhe.PreencherValorNaLinha(143, 147, string.Empty.PadLeft(5, '0'));

                //COBRANÇA  ESCRITURAL 09-CE – Cobrança com emissão total do Boleto pelo Banco
                //COBRANÇA  DIRETIVA  98-PD – Cobrança com emissão total do Boleto pelo cliente
                if (infoDetalhe.BancoEmiteBoleto)
                {
                    detalhe = detalhe.PreencherValorNaLinha(148, 149, infoDetalhe.Especie.Sigla.Equals("DM")
                        ? "09"
                        : infoDetalhe.Especie.Codigo.ToString());
                }
                else
                {
                    if (infoDetalhe.Especie.Sigla.Equals("DM") || infoDetalhe.Especie.Sigla.Equals("PD"))
                        detalhe = detalhe.PreencherValorNaLinha(148, 149, "98");
                    else
                        detalhe = detalhe.PreencherValorNaLinha(148, 149,
                            infoDetalhe.Especie.Codigo.ToString().PadLeft(2, '0'));
                }

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

                var jurosBoleto = string.Empty;
                if (infoDetalhe.ValorMoraDia > 0)
                {
                    var valorCobrarJuroDia = infoDetalhe.ValorBoleto*((infoDetalhe.ValorMoraDia/30)/100);
                    infoDetalhe.ValorCobradoDiaAtraso = Math.Round(valorCobrarJuroDia, 2);
                }

                if (infoDetalhe.ValorCobradoDiaAtraso.ToString().Contains('.') &&
                    infoDetalhe.ValorCobradoDiaAtraso.ToString().Contains(','))
                    jurosBoleto = infoDetalhe.ValorCobradoDiaAtraso.ToString().Replace(".", "").Replace(",", "");
                if (infoDetalhe.ValorCobradoDiaAtraso.ToString().Contains('.'))
                    jurosBoleto = infoDetalhe.ValorCobradoDiaAtraso.ToString().Replace(".", "");
                if (infoDetalhe.ValorCobradoDiaAtraso.ToString().Contains(','))
                    jurosBoleto = infoDetalhe.ValorCobradoDiaAtraso.ToString().Replace(",", "");

                detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosBoleto.PadLeft(13, '0'));

                #endregion

                #region DESCONTO 1

                if (infoDetalhe.DataLimiteConcessaoDesconto == DateTime.MinValue)
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, string.Empty.PadLeft(6, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(174, 179,
                        infoDetalhe.DataLimiteConcessaoDesconto.ToString("ddMMyy"));

                var valorDescontoDia = string.Empty;

                if (infoDetalhe.ValorDescontoDia.ToString().Contains('.') &&
                    infoDetalhe.ValorDescontoDia.ToString().Contains(','))
                    valorDescontoDia = infoDetalhe.ValorDescontoDia.ToString().Replace(".", "").Replace(",", "");
                if (infoDetalhe.ValorDesconto.ToString().Contains('.'))
                    valorDescontoDia = infoDetalhe.ValorDescontoDia.ToString().Replace(".", "");
                if (infoDetalhe.ValorDesconto.ToString().Contains(','))
                    valorDescontoDia = infoDetalhe.ValorDescontoDia.ToString().Replace(",", "");

                detalhe = detalhe.PreencherValorNaLinha(180, 192, valorDescontoDia.PadLeft(13, '0'));

                #endregion

                #region VALOR IOF

                string iofBoleto;
                if (infoDetalhe.ValorIof.ToString().Contains('.') && infoDetalhe.ValorIof.ToString().Contains(','))
                    iofBoleto = infoDetalhe.ValorIof.ToString().Replace(".", "").Replace(",", "");
                if (infoDetalhe.ValorIof.ToString().Contains('.'))
                    iofBoleto = infoDetalhe.ValorIof.ToString().Replace(".", "");
                if (infoDetalhe.ValorIof.ToString().Contains(','))
                    iofBoleto = infoDetalhe.ValorIof.ToString().Replace(",", "");

                detalhe = detalhe.PreencherValorNaLinha(193, 205, infoDetalhe.ValorIof.ToString().PadLeft(13, '0'));

                #endregion

                #region PERCENTUAL MULTA

                if (infoDetalhe.PercentualMulta.BoletoBrToStringSafe().BoletoBrToDecimal() > 0)
                {
                    var multaCalculada = infoDetalhe.ValorBoleto*(infoDetalhe.PercentualMulta/100);
                    var multaBoleto = string.Empty;

                    if (multaCalculada.ToString().Contains('.') && multaCalculada.ToString().Contains(','))
                        multaBoleto = multaCalculada.ToString().Replace(".", "").Replace(",", "");
                    if (multaCalculada.ToString().Contains('.'))
                        multaBoleto = multaCalculada.ToString().Replace(".", "");
                    if (multaCalculada.ToString().Contains(','))
                        multaBoleto = multaCalculada.ToString().Replace(",", "");

                    detalhe = detalhe.PreencherValorNaLinha(206, 215, multaBoleto.ToString().PadLeft(10, '0'));
                    detalhe = detalhe.PreencherValorNaLinha(216, 218, "000");
                }
                else
                {
                    // Sem multa
                    detalhe = detalhe.PreencherValorNaLinha(206, 218, string.Empty.PadLeft(13, '0'));
                }

                #endregion

                var tipoIncricaoSacado = "99"; //99 - Outros
                if (infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11)
                    tipoIncricaoSacado = "01"; //01 - CPF
                else if (infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11)
                    tipoIncricaoSacado = "02"; //02 - CNPJ
                else if (infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").Length == 0)
                    tipoIncricaoSacado = "98"; //98 - Não tem
                detalhe = detalhe.PreencherValorNaLinha(219, 220, tipoIncricaoSacado);
                detalhe = detalhe.PreencherValorNaLinha(221, 234,
                    infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(235, 274, nomeSacado);
                detalhe = detalhe.PreencherValorNaLinha(275, 312, enderecoSacado);

                detalhe = detalhe.PreencherValorNaLinha(313, 314, string.Empty.PadRight(2, ' '));

                string bairroSacado = string.Empty;
                if (infoDetalhe.BairroPagador.BoletoBrToStringSafe().Trim().Length > 12)
                    bairroSacado = infoDetalhe.BairroPagador.BoletoBrToStringSafe().Trim().ExtrairValorDaLinha(1, 12);
                else
                    bairroSacado = infoDetalhe.BairroPagador;
                detalhe = detalhe.PreencherValorNaLinha(315, 326, bairroSacado.PadRight(12, ' '));

                var cep = infoDetalhe.CepPagador;
                if (cep.Contains(".") && cep.Contains("-"))
                    cep = cep.Replace(".", "").Replace("-", "");
                if (cep.Contains("."))
                    cep = cep.Replace(".", "");
                if (cep.Contains("-"))
                    cep = cep.Replace("-", "");

                detalhe = detalhe.PreencherValorNaLinha(327, 334, cep.PadLeft(8, ' '));

                string cidadeSacado = string.Empty;
                if (infoDetalhe.CidadePagador.BoletoBrToStringSafe().Trim().Length > 15)
                    cidadeSacado = infoDetalhe.CidadePagador.BoletoBrToStringSafe().Trim().ExtrairValorDaLinha(1, 15);
                else
                    cidadeSacado = infoDetalhe.CidadePagador;
                detalhe = detalhe.PreencherValorNaLinha(335, 349, cidadeSacado.PadRight(15, ' '));

                string ufSacado = string.Empty;
                if (infoDetalhe.UfPagador.BoletoBrToStringSafe().Trim().Length > 2)
                    ufSacado = infoDetalhe.UfPagador.BoletoBrToStringSafe().Trim().ExtrairValorDaLinha(1, 2);
                else
                    ufSacado = infoDetalhe.UfPagador;
                detalhe = detalhe.PreencherValorNaLinha(350, 351, ufSacado.PadRight(2, ' '));

                string avalista = string.Empty;
                if (infoDetalhe.NomeAvalistaOuMensagem2.BoletoBrToStringSafe().Trim().Length > 0)
                {
                    if (infoDetalhe.NomeAvalistaOuMensagem2.BoletoBrToStringSafe().Trim().Length > 39)
                        avalista =
                            infoDetalhe.NomeAvalistaOuMensagem2.BoletoBrToStringSafe().Trim().ExtrairValorDaLinha(1, 39);
                    else
                        avalista = infoDetalhe.NomeAvalistaOuMensagem2.BoletoBrToStringSafe().Trim();
                }
                detalhe = detalhe.PreencherValorNaLinha(352, 390, avalista.PadRight(39, ' '));

                detalhe = detalhe.PreencherValorNaLinha(391, 391, "N");
                detalhe = detalhe.PreencherValorNaLinha(392, 393, string.Empty.PadLeft(2, ' '));
                detalhe = detalhe.PreencherValorNaLinha(394, 394, "9");

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

        public List<string> EscreverTexto(Arquivo.CNAB400.Remessa.RemessaCnab400 remessaEscrever)
        {
            List<string> listaRetornar = new List<string>();

            listaRetornar.Add(EscreverHeader(remessaEscrever.Header));

            foreach (var detalheAdicionar in remessaEscrever.RegistrosDetalhe)
            {
                listaRetornar.AddRange(new[] {EscreverDetalhe(detalheAdicionar)});
            }

            listaRetornar.Add(EscreverTrailer(remessaEscrever.Trailer));

            return listaRetornar;
        }

        public void ValidarRemessa(Arquivo.CNAB400.Remessa.RemessaCnab400 remessaValidar)
        {
            throw new NotImplementedException();
        }
    }
}
