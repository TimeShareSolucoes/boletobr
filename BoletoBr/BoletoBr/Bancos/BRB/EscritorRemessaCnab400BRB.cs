using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Enums;
using BoletoBr.Fabricas;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.BRB
{
    public class EscritorRemessaCnab400BRB : IEscritorArquivoRemessaCnab400
    {
        private RemessaCnab400 _remessaEscrever;

        public EscritorRemessaCnab400BRB(RemessaCnab400 remessaEscrever)
        {
            _remessaEscrever = remessaEscrever;
        }

        public string EscreverHeader(HeaderRemessaCnab400 infoHeader)
        {
            var header = new string(' ', 39);
            try
            {
                header = header.PreencherValorNaLinha(1, 3, "DCB");
                header = header.PreencherValorNaLinha(4, 6, "001");
                header = header.PreencherValorNaLinha(7, 9, "075");
                header = header.PreencherValorNaLinha(10, 12, infoHeader.Agencia);
                header = header.PreencherValorNaLinha(13, 19,
                    (infoHeader.ContaCorrente + infoHeader.DvContaCorrente).PadLeft(7, '0'));
                header = header.PreencherValorNaLinha(20, 27,
                    infoHeader.DataDeGravacao.ToString("yyyyMMdd").Replace("/", ""));
                header = header.PreencherValorNaLinha(28, 33,
                    infoHeader.DataDeGravacao.ToString("HHmmss").Replace(":", ""));

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
            if (infoDetalhe.TipoCarteiraCobranca != "1" &&
                infoDetalhe.TipoCarteiraCobranca != "2" &&
                infoDetalhe.TipoCarteiraCobranca != "3")
                throw new Exception(
                    "Informe o tipo carteira de cobrança. 1- Sem Registro; 2- Com Registro- Impressão Local ou 3- Com Registro- Impressão pelo BRB");

            #region Variáveis

            var objBanco = BancoFactory.ObterBanco(infoDetalhe.CodigoBanco);

            var enderecoSacado = string.Empty;
            var cidadeSacado = string.Empty;
            var nomeSacado = string.Empty;
            var tipoSacado = string.Empty;

            #endregion

            if (String.IsNullOrEmpty(infoDetalhe.EnderecoPagador))
                enderecoSacado.PadRight(35, ' ');
            else
            {
                enderecoSacado = infoDetalhe.EnderecoPagador.Length > 35
                    ? infoDetalhe.EnderecoPagador.ExtrairValorDaLinha(0, 35).ToUpper()
                    : infoDetalhe.EnderecoPagador.PadRight(35, ' ').ToUpper();
            }

            if (String.IsNullOrEmpty(infoDetalhe.CidadePagador))
                cidadeSacado = cidadeSacado.PadRight(15, ' ');
            else
            {
                cidadeSacado = infoDetalhe.CidadePagador.Length > 15
                    ? infoDetalhe.CidadePagador.ExtrairValorDaLinha(0, 15).ToUpper()
                    : infoDetalhe.CidadePagador.PadRight(15, ' ').ToUpper();
            }

            if (String.IsNullOrEmpty(infoDetalhe.NomePagador))
                nomeSacado = nomeSacado.PadRight(35, ' ');
            else
            {
                nomeSacado = infoDetalhe.NomePagador.Length > 35
                    ? infoDetalhe.NomePagador.ExtrairValorDaLinha(0, 35).ToUpper()
                    : infoDetalhe.NomePagador.PadRight(35, ' ').ToUpper();
            }

            //1- Física; 2- Jurídica ou 9- Isenta
            switch (infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").Length)
            {
                case 11:
                    tipoSacado = "1";
                    break;
                case 14:
                    tipoSacado = "2";
                    break;
                default:
                    tipoSacado = "9";
                    break;
            }

            var detalhe = new string(' ', 400);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 2, "01"); // Identificação do Registro Transação
                detalhe = detalhe.PreencherValorNaLinha(3, 5, infoDetalhe.Agencia);
                detalhe = detalhe.PreencherValorNaLinha(6, 12,
                    (infoDetalhe.ContaCorrente + infoDetalhe.DvContaCorrente).PadLeft(7, '0'));
                detalhe = detalhe.PreencherValorNaLinha(13, 26,
                    infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").PadRight(14, ' '));
                detalhe = detalhe.PreencherValorNaLinha(27, 61, nomeSacado);
                detalhe = detalhe.PreencherValorNaLinha(62, 96, enderecoSacado);
                detalhe = detalhe.PreencherValorNaLinha(97, 111, cidadeSacado);
                detalhe = detalhe.PreencherValorNaLinha(112, 113,
                    infoDetalhe.UfPagador.BoletoBrToStringSafe().PadRight(2, ' '));
                detalhe = detalhe.PreencherValorNaLinha(114, 121,
                    infoDetalhe.CepPagador.BoletoBrToStringSafe().Replace(".", "").Replace("-", "").PadLeft(8, '0'));
                detalhe = detalhe.PreencherValorNaLinha(122, 122, tipoSacado);
                detalhe = detalhe.PreencherValorNaLinha(123, 135, infoDetalhe.NumeroDocumento.PadLeft(13, '0'));
                detalhe = detalhe.PreencherValorNaLinha(136, 136, infoDetalhe.TipoCarteiraCobranca);
                detalhe = detalhe.PreencherValorNaLinha(137, 144, infoDetalhe.DataEmissao.ToString("ddMMyyyy"));
                detalhe = detalhe.PreencherValorNaLinha(145, 146, infoDetalhe.Especie.Codigo.ToString().PadLeft(2, '0'));

                //0- Simples
                detalhe = detalhe.PreencherValorNaLinha(147, 147, "0");

                //0- No vencimento; 1- À Vista ou 2- Contra Apresentação
                detalhe = detalhe.PreencherValorNaLinha(148, 148, "0");

                //02- Real; 51- UFIR ou 91- UPDF
                detalhe = detalhe.PreencherValorNaLinha(149, 150, "02");

                detalhe = detalhe.PreencherValorNaLinha(151, 153, "070");
                detalhe = detalhe.PreencherValorNaLinha(154, 157, infoDetalhe.Agencia.PadLeft(4, '0'));
                //detalhe = detalhe.PreencherValorNaLinha(158, 187, " ");
                detalhe = detalhe.PreencherValorNaLinha(188, 195, infoDetalhe.DataVencimento.ToString("ddMMyyyy"));
                detalhe = detalhe.PreencherValorNaLinha(196, 209,
                    infoDetalhe.ValorBoleto.ToString("f").Replace(".", "").Replace(",", "").PadLeft(14, '0'));

                if (infoDetalhe.TipoCarteiraCobranca == "3")
                    detalhe = detalhe.PreencherValorNaLinha(210, 221, string.Empty.PadLeft(12, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(210, 221, infoDetalhe.NossoNumeroFormatado);

                //00- Sem Juros ('Não Cobrar Juros'); 
                //50-Diário ("Juro de mora ao dia de...") ou 
                //51- Mensal ("Juro de mora ao mês de ...%")
                if (infoDetalhe.TipoCobrancaJuro == TipoCobrancaJuro.JurosDiario)
                    detalhe = detalhe.PreencherValorNaLinha(222, 223, "50");
                else if (infoDetalhe.TipoCobrancaJuro == TipoCobrancaJuro.JurosMensal)
                    detalhe = detalhe.PreencherValorNaLinha(222, 223, "51");
                else
                    detalhe = detalhe.PreencherValorNaLinha(222, 223, "00");

                detalhe = detalhe.PreencherValorNaLinha(224, 237,
                    infoDetalhe.ValorJuros.ToString("f").Replace(",", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(238, 251,
                    infoDetalhe.ValorAbatimento.ToString("f").Replace(",", "").PadLeft(14, '0'));

                if (infoDetalhe.ValorDescontoDia > 0)
                    detalhe = detalhe.PreencherValorNaLinha(252, 253, "52");
                else if (infoDetalhe.ValorDesconto > 0)
                    detalhe = detalhe.PreencherValorNaLinha(252, 253, "53");
                else
                    detalhe = detalhe.PreencherValorNaLinha(252, 253, "00");

                if (infoDetalhe.DataLimiteConcessaoDesconto == DateTime.MinValue)
                    detalhe = detalhe.PreencherValorNaLinha(254, 261, string.Empty.PadLeft(8, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(254, 261,
                        infoDetalhe.DataLimiteConcessaoDesconto.ToString("ddMMyyyy"));

                if (infoDetalhe.ValorDescontoDia > 0)
                    detalhe = detalhe.PreencherValorNaLinha(262, 275,
                        infoDetalhe.ValorDescontoDia.ToString("f").Replace(",", "").PadLeft(14, '0'));
                else if (infoDetalhe.ValorDesconto > 0)
                    detalhe = detalhe.PreencherValorNaLinha(262, 275,
                        infoDetalhe.ValorDesconto.ToString("f").Replace(",", "").PadLeft(14, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(262, 275, string.Empty.PadLeft(14, '0'));

                #region Instruções

                if (infoDetalhe.Instrucoes.Count > 2)
                    throw new Exception(
                        string.Format(
                            "<BoletoBr>{0}Não são aceitas mais que 2 instruções padronizadas para remessa de boletos no banco BRB.",
                            Environment.NewLine));

                var primeiraInstrucao = infoDetalhe.Instrucoes.FirstOrDefault();

                if (primeiraInstrucao != null)
                {
                    detalhe = detalhe.PreencherValorNaLinha(276, 277,
                        primeiraInstrucao.Codigo.BoletoBrToStringSafe());
                    detalhe = detalhe.PreencherValorNaLinha(278, 279,
                        primeiraInstrucao.QtdDias.BoletoBrToStringSafe());

                    detalhe = detalhe.PreencherValorNaLinha(284, 288,
                        primeiraInstrucao.Valor.ToString("f").Replace(",", "").PadLeft(5, '0'));
                }
                else
                {
                    detalhe = detalhe.PreencherValorNaLinha(276, 277, "00");
                    detalhe = detalhe.PreencherValorNaLinha(278, 279, "00");

                    detalhe = detalhe.PreencherValorNaLinha(284, 288, string.Empty.PadLeft(5, '0'));
                }

                if (infoDetalhe.Instrucoes.Count > 1)
                {
                    var segundaIntrucao = infoDetalhe.Instrucoes.LastOrDefault();
                    detalhe = detalhe.PreencherValorNaLinha(280, 281, segundaIntrucao.Codigo.BoletoBrToStringSafe());
                    detalhe = detalhe.PreencherValorNaLinha(282, 283, segundaIntrucao.QtdDias.BoletoBrToStringSafe());
                }
                else
                {
                    detalhe = detalhe.PreencherValorNaLinha(280, 281, "00");
                    detalhe = detalhe.PreencherValorNaLinha(282, 283, "00");
                }

                #endregion

                var razao = "";
                if (infoDetalhe.RazaoContaCorrente.Length > 40)
                    razao = infoDetalhe.RazaoContaCorrente.ExtrairValorDaLinha(1, 40);
                else razao = infoDetalhe.RazaoContaCorrente;
                detalhe = detalhe.PreencherValorNaLinha(289, 328, razao.PadRight(40, ' '));

                var obs = "";
                if (infoDetalhe.Mensagem1.BoletoBrToStringSafe().Length > 40)
                    obs = infoDetalhe.Mensagem1.ExtrairValorDaLinha(1, 40);
                else
                    obs = infoDetalhe.Mensagem1.BoletoBrToStringSafe();
                detalhe = detalhe.PreencherValorNaLinha(329, 368, obs.PadRight(40, ' '));

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do DETALHE do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public List<string> EscreverTexto(RemessaCnab400 remessaEscrever)
        {
            var listaRetornar = new List<string> {EscreverHeader(remessaEscrever.Header)};

            foreach (var detalheAdicionar in remessaEscrever.RegistrosDetalhe)
            {
                listaRetornar.AddRange(new[] {EscreverDetalhe(detalheAdicionar)});
            }

            if (listaRetornar.Count > 0)
            {
                listaRetornar[0] = listaRetornar[0].PreencherValorNaLinha(34, 39,
                    listaRetornar.Count.ToString().PadLeft(6, '0'));
            }
            
            listaRetornar.Add(Environment.NewLine);

            return listaRetornar;
        }

        public void ValidarRemessa(RemessaCnab400 remessaValidar)
        {
            throw new System.NotImplementedException();
        }
    }
}
