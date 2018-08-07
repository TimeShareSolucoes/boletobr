using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Sicoob
{
    public class EscritorRemessaCnab400Sicoob : IEscritorArquivoRemessaCnab400
    {
        private RemessaCnab400 _remessaEscrever;

        public EscritorRemessaCnab400Sicoob(RemessaCnab400 remessaEscrever)
        {
            _remessaEscrever = remessaEscrever;
        }

        public string EscreverHeader(HeaderRemessaCnab400 infoHeader)
        {
            var nomeEmpresa = "";
            if (infoHeader.NomeEmpresa.Length > 30)
                nomeEmpresa = infoHeader.NomeEmpresa.Substring(0, 30);
            else
                nomeEmpresa = infoHeader.NomeEmpresa.ToUpper();

            var header = new string(' ', 400);
            try
            {
                header = header.PreencherValorNaLinha(1, 1, "0");
                header = header.PreencherValorNaLinha(2, 2, "1");
                header = header.PreencherValorNaLinha(3, 9, "REMESSA");
                header = header.PreencherValorNaLinha(10, 11, "01");
                header = header.PreencherValorNaLinha(12, 19, "COBRANÇA");
                header = header.PreencherValorNaLinha(20, 26, string.Empty.PadRight(7, ' '));
                header = header.PreencherValorNaLinha(27, 30, infoHeader.Agencia.PadRight(4, '0'));
                header = header.PreencherValorNaLinha(31, 31, infoHeader.DvAgencia.PadLeft(1, '0'));
                header = header.PreencherValorNaLinha(32, 39, infoHeader.CodigoEmpresa.PadLeft(8, '0'));
                header = header.PreencherValorNaLinha(40, 40, infoHeader.DigitoCedenteEmpresa.PadRight(1, '0'));
                header = header.PreencherValorNaLinha(41, 46, string.Empty.PadRight(6, ' '));
                header = header.PreencherValorNaLinha(47, 76, nomeEmpresa.PadRight(30, ' '));
                header = header.PreencherValorNaLinha(77, 94, "756BANCOOBCED".PadRight(18, ' '));
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("ddMMyy").Replace("/", ""));
                header = header.PreencherValorNaLinha(101, 107, infoHeader.NumeroSequencialRemessa.BoletoBrToStringSafe().PadLeft(7, '0'));
                header = header.PreencherValorNaLinha(108, 394, string.Empty.PadRight(287, ' '));
                header = header.PreencherValorNaLinha(395, 400, "000001");

                return header;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("BoletoBr{0}Falha na geração do HEADER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverDetalhe(DetalheRemessaCnab400 infoDetalhe, int sequenciaDetalhe)
        {
            if (String.IsNullOrEmpty(infoDetalhe.BairroPagador))
                throw new Exception("Não foi informado o bairro do pagador " + infoDetalhe.NomePagador + "(" +
                                    infoDetalhe.InscricaoPagador + ")");

            if (String.IsNullOrEmpty(infoDetalhe.CepPagador) || infoDetalhe.CepPagador.Length < 8)
                throw new Exception("CEP Inválido! Verifique o CEP do pagador " + infoDetalhe.NomePagador + "(" +
                                    infoDetalhe.InscricaoPagador + ")");

            #region Variáveis

            string enderecoSacado = string.Empty;
            string bairroSacado = string.Empty;
            string cidadeSacado = string.Empty;

            string nomeSacado = string.Empty;

            #endregion

            if (String.IsNullOrEmpty(infoDetalhe.EnderecoPagador))
                enderecoSacado.PadRight(37, ' ');
            else if (infoDetalhe.EnderecoPagador.Length > 37)
                enderecoSacado = infoDetalhe.EnderecoPagador.Substring(0, 37).ToUpper();
            else
                enderecoSacado = infoDetalhe.EnderecoPagador.PadRight(37, ' ').ToUpper();

            if (String.IsNullOrEmpty(infoDetalhe.BairroPagador))
                bairroSacado.PadRight(15, ' ');
            else if (infoDetalhe.BairroPagador.Length > 15)
                bairroSacado = infoDetalhe.BairroPagador.Substring(0, 15).ToUpper();
            else
                bairroSacado = infoDetalhe.BairroPagador.PadRight(15, ' ').ToUpper();

            if (String.IsNullOrEmpty(infoDetalhe.CidadePagador))
                cidadeSacado.PadRight(15, ' ');
            else if (infoDetalhe.CidadePagador.Length > 15)
                cidadeSacado = infoDetalhe.CidadePagador.Substring(0, 15).ToUpper();
            else
                cidadeSacado = infoDetalhe.CidadePagador.PadRight(15, ' ').ToUpper();

            if (String.IsNullOrEmpty(infoDetalhe.NomePagador))
                nomeSacado.PadRight(30, ' ');
            else if (infoDetalhe.NomePagador.Length > 30)
                nomeSacado = infoDetalhe.NomePagador.Substring(0, 30).ToUpper();
            else
                nomeSacado = infoDetalhe.NomePagador.PadRight(30, ' ').ToUpper();

            var detalhe = new string(' ', 400);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, "1");
                detalhe = detalhe.PreencherValorNaLinha(2, 3,
                    infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11
                        ? "01"
                        : "02");
                detalhe = detalhe.PreencherValorNaLinha(4, 17,
                    infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", ""));

                detalhe = detalhe.PreencherValorNaLinha(18, 21, infoDetalhe.Agencia.PadLeft(4, '0'));
                detalhe = detalhe.PreencherValorNaLinha(22, 22, infoDetalhe.DvAgencia.PadRight(1, '0'));
                detalhe = detalhe.PreencherValorNaLinha(23, 30, infoDetalhe.ContaCorrente.PadLeft(8, '0'));
                detalhe = detalhe.PreencherValorNaLinha(31, 31, infoDetalhe.DvContaCorrente.PadRight(1, '0'));
                detalhe = detalhe.PreencherValorNaLinha(32, 37, string.Empty.PadRight(6, '0'));

                detalhe = detalhe.PreencherValorNaLinha(38, 62, string.Empty.PadRight(25, ' '));

                detalhe = detalhe.PreencherValorNaLinha(63, 74, infoDetalhe.NossoNumeroFormatado.Replace("-", "").PadLeft(12, '0'));
                detalhe = detalhe.PreencherValorNaLinha(75, 76, "01");
                detalhe = detalhe.PreencherValorNaLinha(77, 78, "00");
                detalhe = detalhe.PreencherValorNaLinha(79, 81, string.Empty.PadRight(3, ' '));
                detalhe = detalhe.PreencherValorNaLinha(82, 82, string.Empty.PadRight(1, ' '));
                detalhe = detalhe.PreencherValorNaLinha(83, 85, string.Empty.PadRight(3, ' '));
                detalhe = detalhe.PreencherValorNaLinha(86, 88, "000");
                detalhe = detalhe.PreencherValorNaLinha(89, 89, "0");

                /* CARTEIRA 01 */
                detalhe = detalhe.PreencherValorNaLinha(90, 94, string.Empty.PadRight(5, '0'));
                detalhe = detalhe.PreencherValorNaLinha(95, 95, "0");

                detalhe = detalhe.PreencherValorNaLinha(96, 101, string.Empty.PadRight(6, '0'));
                detalhe = detalhe.PreencherValorNaLinha(102, 105, string.Empty.PadRight(4, ' '));

                if (infoDetalhe.BancoEmiteBoleto)
                    detalhe = detalhe.PreencherValorNaLinha(106, 106, "1");
                else
                    detalhe = detalhe.PreencherValorNaLinha(106, 106, "2");

                var carteira = infoDetalhe.CarteiraCobranca.Replace("/", "").Replace("-", "");
                if (carteira.Length >= 3) carteira = carteira.ExtrairValorDaLinha(2, 3);
                detalhe = detalhe.PreencherValorNaLinha(107, 108, carteira.PadLeft(2, '0'));

                detalhe = detalhe.PreencherValorNaLinha(109, 110,
                    infoDetalhe.CodigoOcorrencia.Codigo.ToString().PadLeft(2, '0'));

                detalhe = detalhe.PreencherValorNaLinha(111, 120, infoDetalhe.NumeroDocumento.PadRight(10, ' '));
                detalhe = detalhe.PreencherValorNaLinha(121, 126, infoDetalhe.DataVencimento.ToString("ddMMyy"));
                detalhe = detalhe.PreencherValorNaLinha(127, 139,
                    infoDetalhe.ValorBoleto.ToString("f").Replace(".", "").Replace(",", "").PadLeft(13, '0'));
                detalhe = detalhe.PreencherValorNaLinha(140, 142, "756");

                detalhe = detalhe.PreencherValorNaLinha(143, 146, infoDetalhe.Agencia.PadLeft(4, '0'));
                detalhe = detalhe.PreencherValorNaLinha(147, 147, infoDetalhe.DvAgencia.PadRight(1, '0'));

                detalhe = detalhe.PreencherValorNaLinha(148, 149,
                    infoDetalhe.Especie.Sigla.Equals("DM") ? "01" : infoDetalhe.Especie.Codigo.ToString());

                if (infoDetalhe.Aceite == "S")
                    detalhe = detalhe.PreencherValorNaLinha(150, 150, "1");
                else
                    detalhe = detalhe.PreencherValorNaLinha(150, 150, "0");

                detalhe = detalhe.PreencherValorNaLinha(151, 156, infoDetalhe.DataEmissao.ToString("ddMMyy"));

                #region INSTRUÇÕES REMESSA

                var primeiraInstrucao = infoDetalhe.Instrucoes.FirstOrDefault();
                var segundaInstrucao = infoDetalhe.Instrucoes.LastOrDefault();

                if (primeiraInstrucao != null && primeiraInstrucao.Codigo.BoletoBrToStringSafe().Length == 2)
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, primeiraInstrucao.Codigo.ToString());
                else
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, "00"); //IMPRIME MENSAGEM DO TRAILER

                if (segundaInstrucao != null && segundaInstrucao.Codigo.BoletoBrToStringSafe().Length == 2)
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, segundaInstrucao.Codigo.ToString());
                else
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, "00"); //IMPRIME MENSAGEM DO TRAILER

                #endregion

                //SICOOB TRABALHA COM 4 CASAS DECIMAIS - Marcos Eduardo - 21/09/2017
                detalhe = detalhe.PreencherValorNaLinha(161, 166,
                    infoDetalhe.ValorMoraDia.ToString("F4").Replace(",", "").PadLeft(6, '0'));

                //SICOOB TRABALHA COM 4 CASAS DECIMAIS - Marcos Eduardo - 21/09/2017
                detalhe = detalhe.PreencherValorNaLinha(167, 172,
                    infoDetalhe.PercentualMulta.ToString("F4").Replace(",", "").PadLeft(6, '0'));

                if (infoDetalhe.BancoEmiteBoleto)
                    detalhe = detalhe.PreencherValorNaLinha(173, 173, "1");
                else
                    detalhe = detalhe.PreencherValorNaLinha(173, 173, "2");

                if (infoDetalhe.DataLimiteConcessaoDesconto == DateTime.MinValue)
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, string.Empty.PadLeft(6, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, infoDetalhe.DataLimiteConcessaoDesconto.ToString("ddMMyy"));

                detalhe = detalhe.PreencherValorNaLinha(180, 192,
                    infoDetalhe.ValorDesconto.ToString().Replace(",", "").PadLeft(13, '0'));

                if (infoDetalhe.Moeda == "9" || infoDetalhe.Moeda == "09" || infoDetalhe.Moeda == "R$" ||
                    infoDetalhe.Moeda == "REAL")
                    detalhe = detalhe.PreencherValorNaLinha(193, 193, "9");

                detalhe = detalhe.PreencherValorNaLinha(194, 205,
                    infoDetalhe.ValorIof.ToString().Replace(",", "").PadLeft(12, '0'));

                /* ABATIMENTO (NÃO ENVIAR 0,00 NEM VALOR TOTAL) */
                //if (infoDetalhe.ValorAbatimento == 0)
                //    infoDetalhe.ValorAbatimento = Math.Round(infoDetalhe.ValorBoleto * (90M / 100M), 2);

                detalhe = detalhe.PreencherValorNaLinha(206, 218,
                    infoDetalhe.ValorAbatimento.ToString().Replace(",", "").Replace(".", "").PadLeft(13, '0'));

                detalhe = detalhe.PreencherValorNaLinha(219, 220,
                    infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11
                        ? "01"
                        : "02");
                detalhe = detalhe.PreencherValorNaLinha(221, 234,
                    infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(235, 274, nomeSacado.PadRight(40, ' '));

                detalhe = detalhe.PreencherValorNaLinha(275, 311, enderecoSacado.PadRight(37, ' '));
                detalhe = detalhe.PreencherValorNaLinha(312, 326, bairroSacado.PadRight(15, ' '));

                var Cep = infoDetalhe.CepPagador;

                if (Cep.Contains("."))
                    Cep = Cep.Replace(".", "");
                if (Cep.Contains("-"))
                    Cep = Cep.Replace("-", "");

                detalhe = detalhe.PreencherValorNaLinha(327, 334, Cep);
                detalhe = detalhe.PreencherValorNaLinha(335, 349, cidadeSacado.PadRight(15, ' '));
                detalhe = detalhe.PreencherValorNaLinha(350, 351, infoDetalhe.UfPagador.PadRight(2, ' '));

                if (infoDetalhe.NomeAvalistaOuMensagem2.BoletoBrToStringSafe().Trim().Length > 0)
                    detalhe = detalhe.PreencherValorNaLinha(352, 391, infoDetalhe.NomeAvalistaOuMensagem2.BoletoBrToStringSafe().PadRight(40, ' '));
                else
                {
                    if (primeiraInstrucao != null && primeiraInstrucao.Codigo == 0)
                    {
                        var instrucao = primeiraInstrucao.TextoInstrucao.BoletoBrToStringSafe();
                        if (instrucao.Trim().Length > 40)
                            instrucao = instrucao.ExtrairValorDaLinha(1, 40);

                        detalhe = detalhe.PreencherValorNaLinha(352, 391, instrucao.PadRight(40, ' '));
                    }
                    else if (segundaInstrucao != null && segundaInstrucao.Codigo == 0)
                    {
                        var instrucao = segundaInstrucao.TextoInstrucao.BoletoBrToStringSafe();
                        if (instrucao.Trim().Length > 40)
                            instrucao = instrucao.ExtrairValorDaLinha(1, 40);

                        detalhe = detalhe.PreencherValorNaLinha(352, 391, instrucao.PadRight(40, ' '));
                    }
                }

                detalhe = detalhe.PreencherValorNaLinha(392, 393, "00");

                detalhe = detalhe.PreencherValorNaLinha(394, 394, string.Empty.PadRight(1, ' '));
                detalhe = detalhe.PreencherValorNaLinha(395, 400, sequenciaDetalhe.ToString().PadLeft(6, '0'));

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do DETALHE do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverTrailer(TrailerRemessaCnab400 infoTrailer, int sequenciaTrailer)
        {
            var trailer = new string(' ', 400);

            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, "9");
                trailer = trailer.PreencherValorNaLinha(2, 194, string.Empty.PadRight(193, ' '));

                var mensagem1 = new string(' ', 40);
                var mensagem2 = new string(' ', 40);
                var mensagem3 = new string(' ', 40);
                var mensagem4 = new string(' ', 40);
                var mensagem5 = new string(' ', 40);

                /* MENSAGEM 1 */
                trailer = trailer.PreencherValorNaLinha(195, 234, mensagem1);
                
                /* MENSAGEM 2 */
                trailer = trailer.PreencherValorNaLinha(235, 274, mensagem2);
                
                /* MENSAGEM 3 */
                trailer = trailer.PreencherValorNaLinha(275, 314, mensagem3);
                
                /* MENSAGEM 4 */
                trailer = trailer.PreencherValorNaLinha(315, 354, mensagem4);
                
                /* MENSAGEM 5 */
                trailer = trailer.PreencherValorNaLinha(355, 394, mensagem5);

                trailer = trailer.PreencherValorNaLinha(395, 400, sequenciaTrailer.ToString().PadLeft(6, '0'));

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
                listaRetornar.AddRange(new[] { EscreverDetalhe(detalheAdicionar, sequencia) });
                sequencia++;
            }

            listaRetornar.Add(EscreverTrailer(remessaEscrever.Trailer, sequencia));

            return listaRetornar;
        }

        public void ValidarRemessa(RemessaCnab400 remessaValidar)
        {
            throw new NotImplementedException();
        }
    }
}
