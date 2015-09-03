using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BoletoBr.Arquivo;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Brasil
{
    public class EscritorRemessaCnab400BancoDoBrasil : IEscritorArquivoRemessaCnab400
    {
        private RemessaCnab400 _remessaEscrever;

        public EscritorRemessaCnab400BancoDoBrasil(RemessaCnab400 remessaEscrever)
        {
            _remessaEscrever = remessaEscrever;
        }

        public string EscreverHeader(HeaderRemessaCnab400 infoHeader, int numeroRemessa, int numeroRegistro)
        {
            if (infoHeader == null)
                throw new Exception("Não há informações para geração do HEADER");

            if (numeroRemessa == 0)
                throw new Exception("Sequencial da remessa não foi informado na geração do HEADER.");

            if (numeroRegistro == 0)
                throw new Exception("Sequencial do registro não foi informado na geração do HEADER.");

            var header = new string(' ', 400);
            try
            {
                string nomeEmpresa = string.Empty;
                if (infoHeader.NomeEmpresa.BoletoBrToStringSafe().Length > 30)
                    nomeEmpresa = infoHeader.NomeEmpresa.ExtrairValorDaLinha(1, 30);
                else
                    nomeEmpresa = infoHeader.NomeEmpresa;

                header = header.PreencherValorNaLinha(1, 1, "0");
                header = header.PreencherValorNaLinha(2, 2, "1");
                //header = header.PreencherValorNaLinha(3, 9,
                //    infoHeader.Ambiente == Remessa.EnumTipoAmbiemte.Producao
                //        ? "REMESSA".PadRight(7, ' ')
                //        : "TESTE".PadRight(7, ' '));
                header = header.PreencherValorNaLinha(3, 9, "REMESSA".PadRight(7, ' '));
                header = header.PreencherValorNaLinha(10, 11, "01");
                header = header.PreencherValorNaLinha(12, 19, "COBRANCA".PadRight(8, ' '));
                header = header.PreencherValorNaLinha(20, 26, string.Empty.PadRight(7, ' '));
                header = header.PreencherValorNaLinha(27, 30, infoHeader.Agencia.PadLeft(4, '0'));
                header = header.PreencherValorNaLinha(31, 31, infoHeader.DvAgencia);
                header = header.PreencherValorNaLinha(32, 39, infoHeader.ContaCorrente.PadLeft(8, '0'));
                header = header.PreencherValorNaLinha(40, 40, infoHeader.DvContaCorrente);
                header = header.PreencherValorNaLinha(41, 46, string.Empty.PadRight(6, '0'));
                header = header.PreencherValorNaLinha(47, 76, nomeEmpresa.ToUpper().PadRight(30, ' '));
                header = header.PreencherValorNaLinha(77, 94, "001BANCODOBRASIL".PadRight(18, ' '));
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("ddMMyy"));
                header = header.PreencherValorNaLinha(101, 107,
                    numeroRemessa.ToString(CultureInfo.InvariantCulture).PadLeft(7, '0'));
                header = header.PreencherValorNaLinha(108, 129, string.Empty.PadRight(22, ' '));
                header = header.PreencherValorNaLinha(130, 136, infoHeader.Convenio.PadLeft(7, '0'));
                header = header.PreencherValorNaLinha(137, 394, string.Empty.PadRight(258, ' '));
                header = header.PreencherValorNaLinha(395, 400, "000001");

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
            if (infoDetalhe == null)
                throw new Exception("Não há boleto para geração do DETALHE");

            if (numeroRegistro == 0)
                throw new Exception("Sequencial do registro não foi informado na geração do DETALHE.");

            #region Variáveis

            var enderecoSacado = string.Empty;
            enderecoSacado += infoDetalhe.EnderecoPagador.ToUpper();

            if (enderecoSacado.Length > 40)
                enderecoSacado = enderecoSacado.ExtrairValorDaLinha(1, 40);
            //throw new Exception("Endereço do sacado excedeu o limite permitido.");

            #endregion

            var detalhe = new string(' ', 400);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, "7");
                detalhe = detalhe.PreencherValorNaLinha(2, 3,
                    infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11
                        ? "01"
                        : "02");
                detalhe = detalhe.PreencherValorNaLinha(4, 17,
                    infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(18, 21, infoDetalhe.Agencia.PadLeft(4, '0'));
                detalhe = detalhe.PreencherValorNaLinha(22, 22, infoDetalhe.DvAgencia);
                detalhe = detalhe.PreencherValorNaLinha(23, 30, infoDetalhe.ContaCorrente.PadLeft(8, '0'));
                detalhe = detalhe.PreencherValorNaLinha(31, 31, infoDetalhe.DvContaCorrente);
                detalhe = detalhe.PreencherValorNaLinha(32, 38, infoDetalhe.Convenio.PadLeft(7, '0'));

                const string doc = "DOC";
                var seuNumero = doc + infoDetalhe.NossoNumeroFormatado.PadRight(25 - doc.Length, ' ');

                detalhe = detalhe.PreencherValorNaLinha(39, 63, seuNumero);

                if (infoDetalhe.CarteiraCobranca == "11" || infoDetalhe.CarteiraCobranca == "31" ||
                    infoDetalhe.CarteiraCobranca == "51")
                    detalhe = detalhe.PreencherValorNaLinha(64, 80, string.Empty.PadRight(17, '0'));
                else if (infoDetalhe.CarteiraCobranca == "12" || infoDetalhe.CarteiraCobranca == "15" ||
                         infoDetalhe.CarteiraCobranca == "17")
                    detalhe = detalhe.PreencherValorNaLinha(64, 80,
                        infoDetalhe.Convenio.PadLeft(7, '0') + infoDetalhe.NossoNumero.PadLeft(10, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(64, 80, infoDetalhe.NossoNumeroFormatado);

                detalhe = detalhe.PreencherValorNaLinha(81, 82, "00");
                detalhe = detalhe.PreencherValorNaLinha(83, 84, "00");
                detalhe = detalhe.PreencherValorNaLinha(85, 87, string.Empty.PadRight(3, ' '));
                detalhe = detalhe.PreencherValorNaLinha(88, 88, " ");
                // Indicador de Observações/Mensagem posição 352-391
                detalhe = detalhe.PreencherValorNaLinha(89, 91, string.Empty.PadRight(3, ' '));

                detalhe = detalhe.PreencherValorNaLinha(92, 94,
                    infoDetalhe.Variacao.BoletoBrToStringSafe().PadLeft(3, '0')); /* VARIAÇÃO DA CARTEIRA */
                
                detalhe = detalhe.PreencherValorNaLinha(95, 95, "0");
                detalhe = detalhe.PreencherValorNaLinha(96, 101, "000000");

                if (infoDetalhe.CarteiraCobranca == "11" || infoDetalhe.CarteiraCobranca == "17")
                {
                    switch (infoDetalhe.Variacao)
                    {
                        case "DESCONTADA":
                            detalhe = detalhe.PreencherValorNaLinha(102, 106, "04DSC");
                            break;
                        case "BBVENDOR":
                            detalhe = detalhe.PreencherValorNaLinha(102, 106, "08VDR");
                            break;
                        case "VINCULADA":
                            detalhe = detalhe.PreencherValorNaLinha(102, 106, "02VIN");
                            break;
                        case "SIMPLES":
                            detalhe = detalhe.PreencherValorNaLinha(102, 106, string.Empty.PadRight(5, ' '));
                            break;
                        default:
                            detalhe = detalhe.PreencherValorNaLinha(102, 106, string.Empty.PadRight(5, ' '));
                            break;
                    }
                }

                if (infoDetalhe.CarteiraCobranca == "12" || infoDetalhe.CarteiraCobranca == "31" ||
                    infoDetalhe.CarteiraCobranca == "51")
                    detalhe = detalhe.PreencherValorNaLinha(102, 106, string.Empty.PadRight(5, ' '));

                detalhe = detalhe.PreencherValorNaLinha(107, 108, infoDetalhe.CarteiraCobranca.PadLeft(2, '0'));

                var codigoOcorrencia = string.Empty;
                codigoOcorrencia = infoDetalhe.CodigoOcorrencia != null
                    ? infoDetalhe.CodigoOcorrencia.Codigo.BoletoBrToStringSafe().PadLeft(2, '0')
                    : "00";
                detalhe = detalhe.PreencherValorNaLinha(109, 110,
                    codigoOcorrencia.ToString(CultureInfo.InvariantCulture));

                detalhe = detalhe.PreencherValorNaLinha(111, 120, infoDetalhe.NossoNumero.PadLeft(10, '0'));
                detalhe = detalhe.PreencherValorNaLinha(121, 126, infoDetalhe.DataVencimento.ToString("ddMMyy"));

                #region VALOR BOLETO

                string valorBoleto;

                if (infoDetalhe.ValorBoleto.ToString("f").Contains('.') &&
                    infoDetalhe.ValorBoleto.ToString("f").Contains(','))
                {
                    valorBoleto = infoDetalhe.ValorBoleto.ToString("f").Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(127, 139, valorBoleto.PadLeft(13, '0'));
                }
                if (infoDetalhe.ValorBoleto.ToString("f").Contains('.'))
                {
                    valorBoleto = infoDetalhe.ValorBoleto.ToString("f").Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(127, 139, valorBoleto.PadLeft(13, '0'));
                }
                if (infoDetalhe.ValorBoleto.ToString("f").Contains(','))
                {
                    valorBoleto = infoDetalhe.ValorBoleto.ToString("f").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(127, 139, valorBoleto.PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(127, 139,
                    infoDetalhe.ValorBoleto.ToString("f").Replace(",", "").PadLeft(13, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(140, 142, "001");
                detalhe = detalhe.PreencherValorNaLinha(143, 146, "0000");
                detalhe = detalhe.PreencherValorNaLinha(147, 147, " ");
                detalhe = detalhe.PreencherValorNaLinha(148, 149,
                    infoDetalhe.Especie.Sigla.Equals("DM")
                        ? "01"
                        : infoDetalhe.Especie.Codigo.ToString(CultureInfo.InvariantCulture));
                detalhe = detalhe.PreencherValorNaLinha(150, 150, infoDetalhe.Aceite.Equals("A") ? "A" : "N");
                detalhe = detalhe.PreencherValorNaLinha(151, 156, infoDetalhe.DataEmissao.ToString("ddMMyy"));

                #region INSTRUÇÕES REMESSA

                if (infoDetalhe.Instrucoes.Count > 2)
                    throw new Exception(string.Format("<BoletoBr>{0}" +
                                                      "Não são aceitas mais que 2 instruções padronizadas para remessa de boletos no Banco do Brasil.",
                        Environment.NewLine));

                var primeiraInstrucao = infoDetalhe.Instrucao1;
                var segundaInstrucao = infoDetalhe.Instrucao2;

                if (primeiraInstrucao != null)
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, primeiraInstrucao.ToString());
                else
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, "00");
                if (segundaInstrucao != null)
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, segundaInstrucao.ToString());
                else
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, "00");

                #endregion

                #region JUROS DE MORA POR DIA DE ATRASO

                var jurosPorDia = string.Empty;
                
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

                detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosPorDia.ToString().PadLeft(13, '0'));

                #endregion

                if (infoDetalhe.DataLimiteConcessaoDesconto == DateTime.MinValue)
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, "000000");
                else
                    detalhe = detalhe.PreencherValorNaLinha(174, 179,
                        infoDetalhe.DataLimiteConcessaoDesconto.ToString("ddMMyy"));

                #region DESCONTO

                string descontoBoleto;

                if (infoDetalhe.ValorDesconto.ToString().Contains('.') &&
                    infoDetalhe.ValorDesconto.ToString().Contains(','))
                {
                    descontoBoleto = infoDetalhe.ValorDesconto.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(180, 192, descontoBoleto.PadLeft(13, '0'));
                }
                if (infoDetalhe.ValorDesconto.ToString().Contains('.'))
                {
                    descontoBoleto = infoDetalhe.ValorDesconto.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(180, 192, descontoBoleto.PadLeft(13, '0'));
                }
                if (infoDetalhe.ValorDesconto.ToString().Contains(','))
                {
                    descontoBoleto = infoDetalhe.ValorDesconto.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(180, 192, descontoBoleto.PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(180, 192, infoDetalhe.ValorDesconto.ToString().PadLeft(13, '0'));

                #endregion

                #region IOF

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

                detalhe = detalhe.PreencherValorNaLinha(193, 205,
                    infoDetalhe.ValorIof.ToString().Replace(",", "").PadLeft(13, '0'));

                #endregion

                #region ABATIMENTO

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
                    infoDetalhe.ValorAbatimento.ToString().Replace(",", "").PadLeft(13, '0'));

                #endregion

                var nomePagador = string.Empty;
                nomePagador = infoDetalhe.NomePagador.Length > 37
                    ? infoDetalhe.NomePagador.ExtrairValorDaLinha(1, 37)
                    : infoDetalhe.NomePagador;

                detalhe = detalhe.PreencherValorNaLinha(219, 220,
                    infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11
                        ? "01"
                        : "02");
                detalhe = detalhe.PreencherValorNaLinha(221, 234,
                    infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(235, 271, nomePagador.ToUpper().PadRight(37, ' '));
                detalhe = detalhe.PreencherValorNaLinha(272, 274, string.Empty.PadRight(3, ' '));
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado.PadRight(40, ' '));

                var bairroSacado = string.Empty;
                if (String.IsNullOrEmpty(infoDetalhe.BairroPagador))
                    bairroSacado = bairroSacado.PadRight(12, ' ');
                else
                {
                    bairroSacado = infoDetalhe.BairroPagador.Length > 12
                        ? infoDetalhe.BairroPagador.ExtrairValorDaLinha(1, 12)
                        : infoDetalhe.BairroPagador.PadRight(12, ' ');
                }
                detalhe = detalhe.PreencherValorNaLinha(315, 326, bairroSacado.ToUpper());

                var cep = infoDetalhe.CepPagador;

                if (cep.Contains(".") && cep.Contains("-"))
                    cep = cep.Replace(".", "").Replace("-", "");
                if (cep.Contains("."))
                    cep = cep.Replace(".", "");
                if (cep.Contains("-"))
                    cep = cep.Replace("-", "");

                detalhe = detalhe.PreencherValorNaLinha(327, 334, cep.PadLeft(8, ' ')); // Cep do Sacado

                var cidadeSacado = string.Empty;
                if (String.IsNullOrEmpty(infoDetalhe.CidadePagador))
                    cidadeSacado = cidadeSacado.PadRight(15, ' ');
                else
                {
                    cidadeSacado = infoDetalhe.CidadePagador.Length > 15
                        ? infoDetalhe.CidadePagador.ExtrairValorDaLinha(1, 15).ToUpper()
                        : infoDetalhe.CidadePagador.PadRight(15, ' ').ToUpper();
                }

                detalhe = detalhe.PreencherValorNaLinha(335, 349, cidadeSacado.ToUpper());
                detalhe = detalhe.PreencherValorNaLinha(350, 351, infoDetalhe.UfPagador.ToUpper().PadRight(2, ' '));

                var obs = string.Empty;
                if (infoDetalhe.Instrucoes != null)
                {
                    foreach (var instrucao in infoDetalhe.Instrucoes)
                    {
                        if (obs.Length > 0) obs += " ";
                        obs += instrucao.TextoInstrucao;
                    }
                }
                if (obs.Length > 40) obs = obs.ExtrairValorDaLinha(1, 40);
                detalhe = detalhe.PreencherValorNaLinha(352, 391, obs.PadRight(40, ' '));

                /* Refere-se a protesto */
                var quantidadeDiaProtesto = string.Empty.PadRight(2, ' ');
                if (infoDetalhe.NroDiasParaProtesto > 0)
                    quantidadeDiaProtesto =
                        infoDetalhe.NroDiasParaProtesto.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
                detalhe = detalhe.PreencherValorNaLinha(392, 393, quantidadeDiaProtesto);

                detalhe = detalhe.PreencherValorNaLinha(394, 394, " ");
                detalhe = detalhe.PreencherValorNaLinha(395, 400,
                    numeroRegistro.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(
                    string.Format("<BoletoBr>{0}Falha na geração do DETALHE do arquivo de REMESSA.",
                        Environment.NewLine), e);
            }
        }

        public string EscreverTrailer(TrailerRemessaCnab400 infoTrailer, int numeroRegistro)
        {
            if (numeroRegistro == 0)
                throw new Exception("Sequencial do registro não foi informado na geração do TRAILER.");

            var trailer = new string(' ', 400);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, "9");
                trailer = trailer.PreencherValorNaLinha(2, 394, string.Empty.PadRight(393, ' '));
                // Contagem total de linhas do arquivo no formato '000000' - 6 dígitos
                trailer = trailer.PreencherValorNaLinha(395, 400,
                    numeroRegistro.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));

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
            var listaRetornar = new List<string>();

            listaRetornar.Add(EscreverHeader(remessaEscrever.Header, remessaEscrever.Header.NumeroSequencialRemessa,
                remessaEscrever.Header.NumeroSequencialRegistro));

            foreach (var detalheAdicionar in remessaEscrever.RegistrosDetalhe)
            {
                listaRetornar.AddRange(new[]
                {EscreverDetalhe(detalheAdicionar, detalheAdicionar.NumeroSequencialRegistro)});
            }

            listaRetornar.Add(EscreverTrailer(remessaEscrever.Trailer, remessaEscrever.Trailer.NumeroSequencialRegistro));

            return listaRetornar;
        }

        public void ValidarRemessa(RemessaCnab400 remessaValidar)
        {
            throw new NotImplementedException();
        }
    }
}
