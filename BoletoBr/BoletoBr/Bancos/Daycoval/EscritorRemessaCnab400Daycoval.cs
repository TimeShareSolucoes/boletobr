using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Interfaces;
using BoletoBr.Arquivo.CNAB400.Remessa;

namespace BoletoBr.Bancos.Daycoval
{
    public class EscritorRemessaCnab400Daycoval : IEscritorArquivoRemessaCnab400
    {
        private RemessaCnab400 _remessaEscrever;

        public EscritorRemessaCnab400Daycoval(RemessaCnab400 remessaEscrever)
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
                header = header.PreencherValorNaLinha(12, 26, "COBRANÇA".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(27, 38, infoHeader.Convenio.PadRight(12, ' '));
                header = header.PreencherValorNaLinha(39, 46, string.Empty.PadRight(8, ' '));
                header = header.PreencherValorNaLinha(47, 76, nomeEmpresa.PadRight(30, ' '));
                header = header.PreencherValorNaLinha(77, 79, "707");
                header = header.PreencherValorNaLinha(80, 94, "BANCO DAYCOVAL".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("ddMMyy"));
                header = header.PreencherValorNaLinha(101, 394, string.Empty.PadRight(294, ' '));
                header = header.PreencherValorNaLinha(395, 400, "000001");

                return header;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Falha na geração do HEADER do arquivo de REMESSA."), e);
            }
        }

        public string EscreverDetalhe(DetalheRemessaCnab400 infoDetalhe, int sequenciaRegistro,
            int sequenciaNumeroRemessa)
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
                nomeSacado.PadRight(30, ' ');
            else if (infoDetalhe.NomePagador.Length > 30)
                nomeSacado = infoDetalhe.NomePagador.Substring(0, 30).ToUpper();
            else
                nomeSacado = infoDetalhe.NomePagador.PadRight(30, ' ').ToUpper();

            #endregion

            var detalhe = new string(' ', 400);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, "1");

                if (infoDetalhe.InscricaoCedente.ToString().Replace(".", "").Replace("-", "").Length == 11)
                    detalhe = detalhe.PreencherValorNaLinha(2, 3, "01");
                else
                    detalhe = detalhe.PreencherValorNaLinha(2, 3, "02");

                detalhe = detalhe.PreencherValorNaLinha(4, 17,
                    infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", "").PadRight(14, ' '));

                detalhe = detalhe.PreencherValorNaLinha(18, 37, infoDetalhe.Convenio.PadRight(20, ' '));
                detalhe = detalhe.PreencherValorNaLinha(38, 62, infoDetalhe.NumeroDocumento.PadLeft(25, ' '));
                detalhe = detalhe.PreencherValorNaLinha(63, 70, string.Empty.PadLeft(8, '0'));

                detalhe = detalhe.PreencherValorNaLinha(84, 107, string.Empty.PadLeft(24, ' '));

                /*
                3 Cobrança com determinação do cobrador pos. 140-142.
                4 N/Número do Banco Cobrador deve vir nas posições 71 a 83.
                Nesta Carteira, o Cliente envia o Título para Nosso Banco, já
                com o Nosso Número do Correspondente e o seu DV, calculado.
                O Campo deve ocupar 13 posições, com zeros à esquerda.
                 */
                if (infoDetalhe.CarteiraCobranca == "3")
                {
                    /* NOSSONUMERO */
                    detalhe = detalhe.PreencherValorNaLinha(71, 83, string.Empty.PadLeft(13, ' '));
                    detalhe = detalhe.PreencherValorNaLinha(108, 108, infoDetalhe.CarteiraCobranca);
                }
                else if (infoDetalhe.CarteiraCobranca == "4")
                {
                    /* NOSSONUMERO */
                    detalhe = detalhe.PreencherValorNaLinha(71, 83, infoDetalhe.NossoNumeroFormatado.PadLeft(13, '0'));
                    detalhe = detalhe.PreencherValorNaLinha(108, 108, infoDetalhe.CarteiraCobranca);
                }
                else
                    throw new Exception("Carteira inválida. Informe carteira 3 ou 4.");

                detalhe = detalhe.PreencherValorNaLinha(109, 110,
                    infoDetalhe.CodigoOcorrencia.Codigo.BoletoBrToStringSafe().PadLeft(2, '0'));
                detalhe = detalhe.PreencherValorNaLinha(111, 120, infoDetalhe.NossoNumero.PadRight(10, ' '));

                detalhe = detalhe.PreencherValorNaLinha(121, 126, infoDetalhe.DataVencimento.ToString("ddMMyy"));

                #region VALOR BOLETO

                detalhe = detalhe.PreencherValorNaLinha(127, 139,
                    infoDetalhe.ValorBoleto.ToString("f").Replace(".", "").Replace(",", "").PadLeft(13, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(140, 142, "707");
                detalhe = detalhe.PreencherValorNaLinha(143, 146, string.Empty.PadLeft(4, '0'));
                detalhe = detalhe.PreencherValorNaLinha(147, 147, string.Empty.PadLeft(1, '0'));
                detalhe = detalhe.PreencherValorNaLinha(148, 149,
                    infoDetalhe.Especie.Sigla.Equals("DM") ? "01" : infoDetalhe.Especie.Codigo.ToString());
                detalhe = detalhe.PreencherValorNaLinha(150, 150, infoDetalhe.Aceite.Equals("A") ? "A" : "N");
                detalhe = detalhe.PreencherValorNaLinha(151, 156, infoDetalhe.DataEmissao.ToString("ddMMyy"));
                detalhe = detalhe.PreencherValorNaLinha(157, 158, "00");
                detalhe = detalhe.PreencherValorNaLinha(159, 160, "00");

                #region VALOR JUROS

                var jurosBoleto = string.Empty;

                if (infoDetalhe.ValorMoraDia > 0)
                {
                    var valorCobrarJuroDia = infoDetalhe.ValorBoleto*((infoDetalhe.ValorMoraDia/30)/100);
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

                detalhe = detalhe.PreencherValorNaLinha(193, 218, string.Empty.PadLeft(26, '0'));

                detalhe = detalhe.PreencherValorNaLinha(219, 220,
                    infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11
                        ? "01"
                        : "02"); // Identificação do tipo de inscrição/sacado
                detalhe = detalhe.PreencherValorNaLinha(221, 234,
                    infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(235, 264, nomeSacado);
                detalhe = detalhe.PreencherValorNaLinha(265, 274, string.Empty.PadLeft(10, ' '));
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado);
                detalhe = detalhe.PreencherValorNaLinha(315, 326, bairroSacado);

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

                #region Sacador Avalista

                var nomeEmpresa = string.Empty;
                if (infoDetalhe.RazaoContaCorrente.Length > 30)
                    nomeEmpresa = infoDetalhe.RazaoContaCorrente.Substring(0, 30);
                else
                    nomeEmpresa = infoDetalhe.RazaoContaCorrente.PadRight(30, ' ');

                // Nome do Sacador ou Avalista
                detalhe = detalhe.PreencherValorNaLinha(352, 381, nomeEmpresa);

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(382, 385, string.Empty.PadRight(4, ' '));
                detalhe = detalhe.PreencherValorNaLinha(386, 391, string.Empty.PadRight(6, ' '));
                detalhe = detalhe.PreencherValorNaLinha(392, 393, string.Empty.PadRight(2, '0'));
                detalhe = detalhe.PreencherValorNaLinha(394, 394, "0");
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
                trailer = trailer.PreencherValorNaLinha(2, 394, string.Empty.PadRight(393, ' '));
                trailer = trailer.PreencherValorNaLinha(395, 400, sequenciaRegistro.ToString().PadLeft(6, '0'));

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do TRAILER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverMensagem(DetalheRemessaCnab400 infoDetalhe, int sequenciaRegistro)
        {
            if (infoDetalhe == null)
                throw new Exception("Os dados não foram informados na geração do DETALHE MENSAGEM.");

            var tipo2 = new string(' ', 400);
            try
            {
                var mensagem1 = "";
                var mensagem2 = "";
                var mensagem3 = "";
                var mensagem4 = "";

                foreach (var item in infoDetalhe.Instrucoes)
                {
                    if (mensagem1 == "")
                    {
                        mensagem1 = item.TextoInstrucao;
                        if (mensagem1.Length > 69) mensagem1 = mensagem1.ExtrairValorDaLinha(1, 69);
                    }
                    else if (mensagem2 == "")
                    {
                        mensagem2 = item.TextoInstrucao;
                        if (mensagem2.Length > 69) mensagem2 = mensagem2.ExtrairValorDaLinha(1, 69);
                    }
                    else if (mensagem3 == "")
                    {
                        mensagem3 = item.TextoInstrucao;
                        if (mensagem3.Length > 69) mensagem3 = mensagem3.ExtrairValorDaLinha(1, 69);
                    }
                    else if (mensagem4 == "")
                    {
                        mensagem4 = item.TextoInstrucao;
                        if (mensagem4.Length > 69) mensagem4 = mensagem4.ExtrairValorDaLinha(1, 69);
                    }
                }

                tipo2 = tipo2.PreencherValorNaLinha(1, 1, "2");
                tipo2 = tipo2.PreencherValorNaLinha(2, 2, "0");
                tipo2 = tipo2.PreencherValorNaLinha(3, 71, mensagem1.PadRight(69, ' '));
                tipo2 = tipo2.PreencherValorNaLinha(72, 140, mensagem2.PadRight(69, ' '));
                tipo2 = tipo2.PreencherValorNaLinha(141, 209, mensagem3.PadRight(69, ' '));
                tipo2 = tipo2.PreencherValorNaLinha(210, 278, mensagem4.PadRight(69, ' '));
                tipo2 = tipo2.PreencherValorNaLinha(279, 394, string.Empty.PadRight(116, ' '));
                tipo2 = tipo2.PreencherValorNaLinha(395, 400, sequenciaRegistro.ToString().PadLeft(6, '0'));

                return tipo2;
            }
            catch (Exception e)
            {
                throw new Exception(
                    string.Format("<BoletoBr>{0}Falha na geração do Registro Tipo 2 do arquivo de REMESSA.",
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

                /* ESCREVER MENSAGENS */
                if (detalheAdicionar.Instrucoes != null && detalheAdicionar.Instrucoes.Count > 0)
                {
                    listaRetornar.AddRange(new[] {EscreverMensagem(detalheAdicionar, sequencia)});
                    sequencia++;
                }
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
