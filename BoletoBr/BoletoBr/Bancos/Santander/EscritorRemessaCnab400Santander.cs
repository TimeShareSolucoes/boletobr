using System;
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

        public EscritorRemessaCnab400Santander(RemessaCnab400 remessaEscrever)
        {
            _remessaEscrever = remessaEscrever;
        }

        public string EscreverHeader(HeaderRemessaCnab400 infoHeader)
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
                if (String.IsNullOrEmpty(infoHeader.CodigoDeTransmissao))
                    header = header.PreencherValorNaLinha(27, 46, string.Empty.PadLeft(20, '0'));
                else
                    header = header.PreencherValorNaLinha(27, 46, infoHeader.CodigoDeTransmissao.PadLeft(20, '0')); 
                header = header.PreencherValorNaLinha(47, 76, infoHeader.NomeEmpresa.PadRight(30, ' '));
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

        public string EscreverDetalhe(DetalheRemessaCnab400 infoDetalhe)
        {
            int identificadorMulta = 0;
            if (infoDetalhe.PercentualMulta > 0)
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

            if (infoDetalhe.CarteiraCobranca == "101")
                codigoCarteira = "5";
            if (infoDetalhe.CarteiraCobranca == "102")
                codigoCarteira = "4";

            string enderecoSacado = infoDetalhe.EnderecoPagador;

            if (enderecoSacado.Length > 40)
                enderecoSacado.Substring(0, 40);

            string bairroSacado = string.Empty;

            if (String.IsNullOrEmpty(infoDetalhe.BairroPagador))
                bairroSacado.PadRight(12, ' ');
            else
                if (infoDetalhe.BairroPagador.Length > 12) 
                    bairroSacado = infoDetalhe.BairroPagador.Substring(0, 12);
                else
                    bairroSacado = infoDetalhe.BairroPagador.PadRight(12, ' ');

            string cidadeSacado = string.Empty;

            if (String.IsNullOrEmpty(infoDetalhe.CidadePagador))
                cidadeSacado.PadRight(12, ' ');
            else
                if (infoDetalhe.CidadePagador.Length > 12)
                    cidadeSacado = infoDetalhe.CidadePagador.Substring(0, 12);
                else
                    cidadeSacado = infoDetalhe.CidadePagador.PadRight(12, ' ');


            string complementoRegistro = 
                infoDetalhe.ContaCorrente.Remove(0, infoDetalhe.ContaCorrente.Length - 1) + 
                infoDetalhe.DvContaCorrente;

            var detalhe = new string(' ', 400);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, "1");
                // 01 - CPF
                // 02 - CNPJ
                detalhe = detalhe.PreencherValorNaLinha(2, 3, infoDetalhe.InscricaoCedente.Length == 11 ? "01" : "02");
                detalhe = detalhe.PreencherValorNaLinha(4, 17, infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(18, 37, infoDetalhe.CodigoDeTransmissao.PadLeft(20, '0'));

                const string doc = "DOC";
                var seuNumero = doc + infoDetalhe.NossoNumeroFormatado.PadRight(25 - doc.Length, ' ');

                detalhe = detalhe.PreencherValorNaLinha(38, 62, seuNumero);
                // NossoNumero com DV, pegar os 8 primeiros dígitos, da direita para esquerda
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
                    detalhe = detalhe.PreencherValorNaLinha(79, 82, infoDetalhe.PercentualMulta.ToString().PadLeft(4, '0'));

                detalhe = detalhe.PreencherValorNaLinha(83, 84, "00");
                detalhe = detalhe.PreencherValorNaLinha(85, 97, "0000000000000" /*Vl do título em outra unidade (consultar banco)*/);
                detalhe = detalhe.PreencherValorNaLinha(98, 101, string.Empty.PadLeft(4, ' '));

                detalhe = detalhe.PreencherValorNaLinha(102, 107, "000000");

                detalhe = detalhe.PreencherValorNaLinha(108, 108, codigoCarteira);
                detalhe = detalhe.PreencherValorNaLinha(109, 110, "01" /* Código da Ocorrência*/);
                detalhe = detalhe.PreencherValorNaLinha(111, 120, infoDetalhe.NumeroDocumento.PadRight(10, ' '));
                detalhe = detalhe.PreencherValorNaLinha(121, 126, infoDetalhe.DataVencimento.ToString("ddMMyy"));

                #region VALOR TOTAL

                detalhe = detalhe.PreencherValorNaLinha(127, 139, infoDetalhe.ValorBoleto.ToString("f").Replace(",", "").PadLeft(13, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(140, 142, "033");
                if (codigoCarteira == "5")
                    detalhe = detalhe.PreencherValorNaLinha(143, 147, infoDetalhe.Agencia.PadLeft(4, '0') + infoDetalhe.DvAgencia);
                else
                    detalhe = detalhe.PreencherValorNaLinha(143, 147, string.Empty.PadLeft(5, '0'));
                detalhe = detalhe.PreencherValorNaLinha(148, 149, infoDetalhe.Especie.Sigla.Equals("DM") ? "01" : infoDetalhe.Especie.Codigo.ToString(CultureInfo.InvariantCulture));
                detalhe = detalhe.PreencherValorNaLinha(150, 150, "N");
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

                #region JUROS

                detalhe = detalhe.PreencherValorNaLinha(161, 173, infoDetalhe.ValorMoraDia.ToString("f").Replace(",", "").PadLeft(13, '0'));

                #endregion

                #region DESCONTO

                if (infoDetalhe.DataDesconto == DateTime.MinValue)
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, string.Empty.PadLeft(6, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, infoDetalhe.DataDesconto.ToString("ddMMyy"));

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
                detalhe = detalhe.PreencherValorNaLinha(235, 274, infoDetalhe.NomePagador.PadRight(40, ' '));
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado.PadRight(40, ' '));
                detalhe = detalhe.PreencherValorNaLinha(315, 326, bairroSacado.PadRight(12, ' '));

                #region CEP

                var Cep = infoDetalhe.CepPagador;

                if (Cep.Contains(".") && Cep.Contains("-"))
                    Cep = Cep.Replace(".", "").Replace("-", "");
                if (Cep.Contains("."))
                    Cep = Cep.Replace(".", "");
                if (Cep.Contains("-"))
                    Cep = Cep.Replace("-", "");

                detalhe = detalhe.PreencherValorNaLinha(327, 334, Cep.PadLeft(8, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(335, 349, cidadeSacado.PadRight(15, ' '));
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
                detalhe = detalhe.PreencherValorNaLinha(394, 394, "0");
                detalhe = detalhe.PreencherValorNaLinha(395, 400, infoDetalhe.NumeroSequencialRegistro.ToString().PadLeft(6, '0'));

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
            var trailer = new string(' ', 400);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, "9");
                trailer = trailer.PreencherValorNaLinha(2, 7, infoTrailer.TotalLinhasArquivo.ToString().PadLeft(6, '0'));
                trailer = trailer.PreencherValorNaLinha(8, 20, infoTrailer.ValorTotalTitulos.ToString("f").Replace(",", "").PadLeft(13, '0') /*Valor total dos títulos*/);
                trailer = trailer.PreencherValorNaLinha(21, 394, string.Empty.PadRight(374, '0'));
                trailer = trailer.PreencherValorNaLinha(395, 400, infoTrailer.NumeroSequencialRegistro.ToString().PadLeft(6, '0'));

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
