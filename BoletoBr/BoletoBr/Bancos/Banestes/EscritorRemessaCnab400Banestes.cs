using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Banestes
{
    public class EscritorRemessaCnab400Banestes : IEscritorArquivoRemessaCnab400
    {
        private RemessaCnab400 _remessaEscrever;
        public EscritorRemessaCnab400Banestes(RemessaCnab400 remessaEscrever)
        {
            _remessaEscrever = remessaEscrever;
        }

        public string EscreverHeader(HeaderRemessaCnab400 infoHeader)
        {
            var header = new string(' ', 400);
            if (infoHeader == null)
                throw new Exception("Não há informações para geração do HEADER");

            if (infoHeader.NumeroSequencialRemessa == 0)
                throw new Exception("Sequencial da remessa não foi informado na geração do HEADER.");

            var nomeEmpresa = string.Empty;
            if (infoHeader.NomeEmpresa.Length > 30)
                nomeEmpresa = infoHeader.NomeEmpresa.Substring(0, 30);
            else
                nomeEmpresa = infoHeader.NomeEmpresa.PadRight(30, ' ');

            var contaCorrente = $"{infoHeader.ContaCorrente}{infoHeader.DvContaCorrente}";
            if (contaCorrente.Length > 11) contaCorrente = contaCorrente.ExtrairValorDaLinha(1, 9);
            else contaCorrente = contaCorrente.PadLeft(11, '0');


            try
            {
                header = header.PreencherValorNaLinha(1, 1, "0");
                header = header.PreencherValorNaLinha(2, 2, "1");
                header = header.PreencherValorNaLinha(3, 9, "REMESSA");
                header = header.PreencherValorNaLinha(10, 11, "01");
                header = header.PreencherValorNaLinha(12, 26, "COBRANÇA".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(27, 37, contaCorrente);
                header = header.PreencherValorNaLinha(38, 46, string.Empty.PadRight(9, ' '));
                header = header.PreencherValorNaLinha(47, 76, nomeEmpresa);
                header = header.PreencherValorNaLinha(77, 79, "021");
                header = header.PreencherValorNaLinha(80, 87, "BANESTES");
                header = header.PreencherValorNaLinha(88, 94, string.Empty.PadRight(7, ' '));
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("ddMMyy"));
                header = header.PreencherValorNaLinha(101, 394, string.Empty.PadRight(294, ' '));
                header = header.PreencherValorNaLinha(395, 400, infoHeader.NumeroSequencialRemessa.ToString().PadLeft(6, '0'));


                return header;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("BoletoBr{0}Falha na geração do HEADER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverDetalhe(DetalheRemessaCnab400 infoDetalhe, int sequenciaRegistro)
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
                bairroSacado = bairroSacado.PadRight(12, ' ');
            else if (infoDetalhe.BairroPagador.Length > 12)
                bairroSacado = infoDetalhe.BairroPagador.Substring(0, 12);
            else
                bairroSacado = infoDetalhe.BairroPagador.PadRight(12, ' ');

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
                detalhe = detalhe.PreencherValorNaLinha(1, 1, "1");

                detalhe = detalhe.PreencherValorNaLinha(2, 3, infoDetalhe.InscricaoCedente.BoletoBrToStringSafe().Replace(".", "").Replace("-", "").Length == 11 ? "01" : "02");

                detalhe = detalhe.PreencherValorNaLinha(4, 17,
                    infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", "").PadRight(14, ' '));

                 
                var identificadorCedente = $"{infoDetalhe.ContaCorrente}{infoDetalhe.DvContaCorrente}";
                if (identificadorCedente.Length > 11) identificadorCedente = identificadorCedente.ExtrairValorDaLinha(1, 9);
                else identificadorCedente = identificadorCedente.PadLeft(11, '0');

                detalhe = detalhe.PreencherValorNaLinha(18, 28, identificadorCedente);

                detalhe = detalhe.PreencherValorNaLinha(29, 37, string.Empty.PadLeft(9, ' '));
                detalhe = detalhe.PreencherValorNaLinha(38, 62, string.Empty.PadLeft(25, ' '));

                detalhe = detalhe.PreencherValorNaLinha(63, 72, infoDetalhe.NossoNumeroFormatado.BoletoBrToStringSafe().Replace(".", "").Replace("-", "").PadLeft(10, '0'));
                detalhe = detalhe.PreencherValorNaLinha(73, 73, "1"); /*0 - R$ | 1 - %*/
                #region PERCENTUAL MULTA

                // Percentual de multa
                if (infoDetalhe.PercentualMulta > 0)
                {
                    var multaBoleto = infoDetalhe.PercentualMulta.BoletoBrToStringSafe().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(74, 82, multaBoleto.PadLeft(9, '0'));
                }
                else
                    detalhe = detalhe.PreencherValorNaLinha(74, 82, string.Empty.PadLeft(9, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(83, 88, string.Empty.PadLeft(6, '0'));
                detalhe = detalhe.PreencherValorNaLinha(89, 90, string.Empty.PadLeft(2, '0'));
                detalhe = detalhe.PreencherValorNaLinha(91, 92, string.Empty.PadLeft(2, '0'));

                detalhe = detalhe.PreencherValorNaLinha(93, 93, infoDetalhe.InscricaoPagador.BoletoBrToStringSafe().Replace(".", "").Replace("-", "").Length == 11 ? "1" : "2");
                detalhe = detalhe.PreencherValorNaLinha(94, 107, infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));

                detalhe = detalhe.PreencherValorNaLinha(108, 108, infoDetalhe.CarteiraCobranca == "11" ? "1" : "3");/*1 - simples | 3- Caucionada*/
                detalhe = detalhe.PreencherValorNaLinha(109, 110, infoDetalhe.CodigoOcorrencia.Codigo.BoletoBrToStringSafe().PadLeft(2, '0'));

                detalhe = detalhe.PreencherValorNaLinha(111, 120, infoDetalhe.NumeroDocumento.BoletoBrToStringSafe().PadRight(10, '0'));
                detalhe = detalhe.PreencherValorNaLinha(121, 126, infoDetalhe.DataVencimento.ToString("ddMMyy"));


                detalhe = detalhe.PreencherValorNaLinha(127, 129, string.Empty.PadLeft(3, '0'));
                detalhe = detalhe.PreencherValorNaLinha(130, 139, infoDetalhe.ValorBoleto.ToStringParaVoloresDecimais().PadLeft(10, '0'));
                detalhe = detalhe.PreencherValorNaLinha(140, 142, "021");

                detalhe = detalhe.PreencherValorNaLinha(143, 147, infoDetalhe.BancoEmiteBoleto ? string.Empty.PadLeft(5, '0') : "00501"); /*00501 - envio pelo cliente | 00000 - envio pelo banco*/
                detalhe = detalhe.PreencherValorNaLinha(148, 149, "01"); /*01 - Duplicata- impressa pelo cliente - Embora setado essa opção, em caso de divergência prevalece as posições 01 a 20 que informam os dados do contrato do cliente*/

                detalhe = detalhe.PreencherValorNaLinha(150, 150, infoDetalhe.Aceite != "N" && infoDetalhe.Aceite != "A" ? "N" : infoDetalhe.Aceite);

                detalhe = detalhe.PreencherValorNaLinha(151, 156, infoDetalhe.DataEmissao.ToString("ddMMyy"));

                detalhe = detalhe.PreencherValorNaLinha(157, 158, infoDetalhe.NroDiasParaProtesto > 0 ? "P6" : "P7");
                detalhe = detalhe.PreencherValorNaLinha(159, 160, infoDetalhe.NroDiasParaProtesto > 0 ? infoDetalhe.NroDiasParaProtesto.BoletoBrToStringSafe().PadLeft(2, '0') : "00");
                detalhe = detalhe.PreencherValorNaLinha(161, 161, "0");
                detalhe = detalhe.PreencherValorNaLinha(162, 173, infoDetalhe.ValorMoraDia.ToStringParaVoloresDecimais().PadLeft(12, '0'));

                var dataLimiteDesconto = infoDetalhe.DataLimiteConcessaoDesconto == DateTime.MinValue ? "000000": infoDetalhe
                    .DataLimiteConcessaoDesconto.ToString("ddMMyy");

                detalhe = detalhe.PreencherValorNaLinha(174, 179, dataLimiteDesconto);
                detalhe = detalhe.PreencherValorNaLinha(180, 192, infoDetalhe.ValorDesconto.ToStringParaVoloresDecimais().PadLeft(13, '0'));
                detalhe = detalhe.PreencherValorNaLinha(193, 205, infoDetalhe.ValorIof.ToStringParaVoloresDecimais().PadLeft(13, '0')); /*!---!*/
                detalhe = detalhe.PreencherValorNaLinha(206, 218, infoDetalhe.ValorAbatimento.ToStringParaVoloresDecimais().PadLeft(13, '0'));


                detalhe = detalhe.PreencherValorNaLinha(219, 220, infoDetalhe.InscricaoPagador.BoletoBrToStringSafe().Replace(".", "").Replace("-", "").Length == 11 ? "01" : "02");
                detalhe = detalhe.PreencherValorNaLinha(221, 234, infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(235, 274, nomeSacado);
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado);
                detalhe = detalhe.PreencherValorNaLinha(315, 326, bairroSacado);
                detalhe = detalhe.PreencherValorNaLinha(327, 334, infoDetalhe.CepPagador.PadRight(8, ' '));
                detalhe = detalhe.PreencherValorNaLinha(335, 349, cidadeSacado);
                detalhe = detalhe.PreencherValorNaLinha(350, 351, infoDetalhe.UfPagador.PadRight(2, ' '));

                var instrucao = "";
                foreach (var inst in infoDetalhe.Instrucoes)
                {
                    if (instrucao == "")
                        instrucao = inst.TextoInstrucao;
                    else
                        instrucao += $@" {inst.TextoInstrucao}";
                }
                if (instrucao.Length > 40)
                    instrucao = instrucao.Substring(0, 40);

                detalhe = detalhe.PreencherValorNaLinha(352, 391, instrucao.PadRight(40, ' '));
                detalhe = detalhe.PreencherValorNaLinha(392, 393, "00");
                detalhe = detalhe.PreencherValorNaLinha(394, 394, "0");
                detalhe = detalhe.PreencherValorNaLinha(395, 400, sequenciaRegistro.BoletoBrToStringSafe().PadLeft(6, '0'));

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do DETALHE do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }
        public string EscreverTrailer(int sequenciaRegistro)
        {
            var trailer = new string(' ', 400);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, "9");
                trailer = trailer.PreencherValorNaLinha(2, 394, string.Empty.PadRight(393, ' '));
                trailer = trailer.PreencherValorNaLinha(395, 400,
                    sequenciaRegistro.BoletoBrToStringSafe().PadLeft(6, '0'));

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
                    {EscreverDetalhe(detalheAdicionar, sequencia)});
                sequencia++;
            }

            listaRetornar.Add(EscreverTrailer(sequencia));

            return listaRetornar;
        }

        public void ValidarRemessa(RemessaCnab400 remessaValidar)
        {
            throw new NotImplementedException();
        }
    }
}
