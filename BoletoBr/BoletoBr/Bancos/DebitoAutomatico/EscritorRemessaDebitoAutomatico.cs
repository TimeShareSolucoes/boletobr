using System;
using System.Collections.Generic;
using System.Linq;
using BoletoBr.Arquivo.DebitoAutomatico.Remessa;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.DebitoAutomatico
{
    public class EscritorRemessaDebitoAutomatico : IEscritorArquivoRemessaDebitoAutomatico
    {
        private RemessaDebitoAutomatico _remessaEscrever;

        public EscritorRemessaDebitoAutomatico(RemessaDebitoAutomatico remessaEscrever)
        {
            _remessaEscrever = remessaEscrever;
        }

        public string EscreverHeader(HeaderRemessaDebitoAutomatico infoHeader)
        {
            if (infoHeader == null)
                throw new Exception("Não há informações para geração do HEADER");

            var nomeEmpresa = string.Empty;
            if (infoHeader.NomeEmpresa.Length > 20)
                nomeEmpresa = infoHeader.NomeEmpresa.Substring(0, 20);
            else
                nomeEmpresa = infoHeader.NomeEmpresa.PadRight(20, ' ');

            var header = new string(' ', 150);
            try
            {
                header = header.PreencherValorNaLinha(1, 1, infoHeader.CodigoRegistro);
                header = header.PreencherValorNaLinha(2, 2, infoHeader.CodigoRemessa.BoletoBrToStringSafe());
                header = header.PreencherValorNaLinha(3, 22, infoHeader.CodigoConvenio.PadRight(20, ' '));
                header = header.PreencherValorNaLinha(23, 42, nomeEmpresa);
                header = header.PreencherValorNaLinha(43, 45, infoHeader.CodigoBanco.PadLeft(3, '0'));
                header = header.PreencherValorNaLinha(46, 65, infoHeader.NomeBanco.PadRight(20, ' '));
                header = header.PreencherValorNaLinha(66, 73, infoHeader.DataGeracao.ToString("yyyyMMdd"));
                header = header.PreencherValorNaLinha(74, 79, infoHeader.NumeroSequencial.BoletoBrToStringSafe().PadLeft(6, '0'));
                header = header.PreencherValorNaLinha(80, 81, infoHeader.VersaoLayout);
                header = header.PreencherValorNaLinha(82, 98, infoHeader.IdentificacaoServico);
                header = header.PreencherValorNaLinha(99, 145, string.Empty.PadRight(47, ' '));
                header = header.PreencherValorNaLinha(146, 150, infoHeader.RemessaTeste ? "TESTE" : string.Empty.PadRight(5, ' '));

                return header;
            }
            catch (Exception e)
            {
                throw new Exception($@"<BoletoBr>{Environment.NewLine}Falha na geração do HEADER do arquivo de REMESSA.", e);
            }
        }

        public string EscreverDetalhe(DetalheRemessaRegistroC infoDetalhe, int sequenciaRegistro)
        {
            if (infoDetalhe == null)
                throw new Exception("Não há boleto para geração do DETALHE");


            var detalhe = new string(' ', 150);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, infoDetalhe.CodigoRegistro);
                detalhe = detalhe.PreencherValorNaLinha(2, 26, infoDetalhe.IdentificacaoClienteEmpresa.PadRight(25, ' '));
                detalhe = detalhe.PreencherValorNaLinha(27, 30, infoDetalhe.AgenciaDebito.PadRight(4, ' '));
                detalhe = detalhe.PreencherValorNaLinha(31, 44, infoDetalhe.IdentificacaoClienteBanco.PadRight(14, ' '));
                detalhe = detalhe.PreencherValorNaLinha(45, 84, infoDetalhe.Ocorrencia1.PadRight(40, ' '));
                detalhe = detalhe.PreencherValorNaLinha(85, 124, infoDetalhe.Ocorrencia2.PadRight(40, ' '));
                detalhe = detalhe.PreencherValorNaLinha(125, 149, string.Empty.PadRight(25, ' '));
                detalhe = detalhe.PreencherValorNaLinha(150, 150, infoDetalhe.CodigoMovimento.BoletoBrToStringSafe());


                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception($@"<BoletoBr>{Environment.NewLine}Falha na geração do DETALHE do arquivo de REMESSA.", e);
            }
        }

        public string EscreverDetalhe(DetalheRemessaRegistroD infoDetalhe, int sequenciaRegistro)
        {
            if (infoDetalhe == null)
                throw new Exception("Não há boleto para geração do DETALHE");


            var detalhe = new string(' ', 150);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, infoDetalhe.CodigoRegistro);
                detalhe = detalhe.PreencherValorNaLinha(2, 26, infoDetalhe.IdentificacaoClienteEmpresaAnterior.PadRight(25, ' '));
                detalhe = detalhe.PreencherValorNaLinha(27, 30, infoDetalhe.AgenciaDebito.PadRight(4, ' '));
                detalhe = detalhe.PreencherValorNaLinha(31, 44, infoDetalhe.IdentificacaoClienteBanco.PadRight(14, ' '));
                detalhe = detalhe.PreencherValorNaLinha(45, 69, infoDetalhe.IdentificacaoClienteEmpresaAtual.PadRight(25, ' '));
                detalhe = detalhe.PreencherValorNaLinha(70, 129, infoDetalhe.Ocorrencia.PadRight(60, ' '));
                detalhe = detalhe.PreencherValorNaLinha(130, 149, string.Empty.PadRight(20, ' '));
                detalhe = detalhe.PreencherValorNaLinha(150, 150, infoDetalhe.CodigoMovimento.BoletoBrToStringSafe());


                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception($@"<BoletoBr>{Environment.NewLine}Falha na geração do DETALHE do arquivo de REMESSA.", e);
            }
        }

        public string EscreverDetalhe(DetalheRemessaRegistroE infoDetalhe, int sequenciaRegistro)
        {
            if (infoDetalhe == null)
                throw new Exception("Não há boleto para geração do DETALHE");


            var detalhe = new string(' ', 150);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, infoDetalhe.CodigoRegistro);
                detalhe = detalhe.PreencherValorNaLinha(2, 26, infoDetalhe.IdentificacaoClienteEmpresa.PadRight(25, ' '));
                detalhe = detalhe.PreencherValorNaLinha(27, 30, infoDetalhe.AgenciaDebito.PadRight(4, ' '));
                detalhe = detalhe.PreencherValorNaLinha(31, 44, infoDetalhe.IdentificacaoClienteBanco.PadRight(14, ' '));
                detalhe = detalhe.PreencherValorNaLinha(45, 52, infoDetalhe.DataVencimento.ToString("yyyyMMdd"));
                detalhe = detalhe.PreencherValorNaLinha(53, 67, infoDetalhe.ValorDebito.ToStringParaValoresDecimais().PadLeft(15, '0'));
                detalhe = detalhe.PreencherValorNaLinha(68, 69, infoDetalhe.CodigoMoeda.PadRight(02, ' '));
                detalhe = detalhe.PreencherValorNaLinha(70, 118, infoDetalhe.UsoEmpresa.PadRight(49, ' '));
                detalhe = detalhe.PreencherValorNaLinha(119, 128, infoDetalhe.ValorTotalTributos.ToStringParaValoresDecimais().PadRight(10, ' '));
                detalhe = detalhe.PreencherValorNaLinha(129, 129, infoDetalhe.TipoCobranca);
                detalhe = detalhe.PreencherValorNaLinha(130, 149, string.Empty.PadRight(20, ' '));
                detalhe = detalhe.PreencherValorNaLinha(150, 150, infoDetalhe.CodigoMovimento.BoletoBrToStringSafe());


                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception($@"<BoletoBr>{Environment.NewLine}Falha na geração do DETALHE do arquivo de REMESSA.", e);
            }
        }

        public string EscreverDetalhe(DetalheRemessaRegistroI infoDetalhe, int sequenciaRegistro)
        {
            if (infoDetalhe == null)
                throw new Exception("Não há boleto para geração do DETALHE");

            var nomeContribuinte = string.Empty;
            if (infoDetalhe.Nome.Length > 40)
                nomeContribuinte = infoDetalhe.Nome.Substring(0, 40);
            else
                nomeContribuinte = infoDetalhe.Nome.PadRight(40, ' ');

            var cidade = string.Empty;
            if (infoDetalhe.Cidade.Length > 30)
                cidade = infoDetalhe.Cidade.Substring(0, 30);
            else
                cidade = infoDetalhe.Cidade.PadRight(30, ' ');


            var detalhe = new string(' ', 150);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, infoDetalhe.CodigoRegistro);
                detalhe = detalhe.PreencherValorNaLinha(2, 26, infoDetalhe.IdentificacaoClienteEmpresa.PadRight(25, ' '));
                detalhe = detalhe.PreencherValorNaLinha(27, 27, infoDetalhe.TipoIdentificacao.BoletoBrToStringSafe());
                detalhe = detalhe.PreencherValorNaLinha(28, 41, infoDetalhe.CpfCnpj.PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(42, 81, nomeContribuinte);
                detalhe = detalhe.PreencherValorNaLinha(82, 111, cidade);
                detalhe = detalhe.PreencherValorNaLinha(112, 113, infoDetalhe.Estado);
                detalhe = detalhe.PreencherValorNaLinha(114, 150, string.Empty.PadRight(37, ' '));

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception($@"<BoletoBr>{Environment.NewLine}Falha na geração do DETALHE do arquivo de REMESSA.", e);
            }
        }

        public string EscreverDetalhe(DetalheRemessaRegistroJ infoDetalhe, int sequenciaRegistro)
        {
            if (infoDetalhe == null)
                throw new Exception("Não há boleto para geração do DETALHE");

            var detalhe = new string(' ', 150);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, infoDetalhe.CodigoRegistro);
                detalhe = detalhe.PreencherValorNaLinha(2, 7, infoDetalhe.NumeroSequencial.BoletoBrToStringSafe().PadLeft(6, '0'));
                detalhe = detalhe.PreencherValorNaLinha(8, 15, infoDetalhe.DataGeracao.ToString("yyyyMMdd"));
                detalhe = detalhe.PreencherValorNaLinha(16, 21, infoDetalhe.TotalRegistroProcessado.BoletoBrToStringSafe().PadLeft(6, '0'));
                detalhe = detalhe.PreencherValorNaLinha(22, 38, infoDetalhe.ValorTotalProcessado.ToStringParaValoresDecimais().PadLeft(17, '0'));
                detalhe = detalhe.PreencherValorNaLinha(39, 46, infoDetalhe.DataProcessamento.ToString("yyyyMMdd"));
                detalhe = detalhe.PreencherValorNaLinha(47, 150, string.Empty.PadRight(104, ' '));

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception($@"<BoletoBr>{Environment.NewLine}Falha na geração do DETALHE do arquivo de REMESSA.", e);
            }
        }

        public string EscreverDetalhe(DetalheRemessaRegistroL infoDetalhe, int sequenciaRegistro)
        {
            if (infoDetalhe == null)
                throw new Exception("Não há boleto para geração do DETALHE");

            var detalhe = new string(' ', 150);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, infoDetalhe.CodigoRegistro);
                detalhe = detalhe.PreencherValorNaLinha(2, 9, infoDetalhe.DataFaturamentoContas.ToString("yyyyMMdd"));
                detalhe = detalhe.PreencherValorNaLinha(10, 17, infoDetalhe.DataVencimento.ToString("yyyyMMdd"));
                detalhe = detalhe.PreencherValorNaLinha(18, 25, infoDetalhe.DataRemessaArquivo.ToString("yyyyMMdd"));
                detalhe = detalhe.PreencherValorNaLinha(26, 33, infoDetalhe.DataRemessaContas.ToString("yyyyMMdd"));
                detalhe = detalhe.PreencherValorNaLinha(34, 150, string.Empty.PadRight(117, ' '));

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception($@"<BoletoBr>{Environment.NewLine}Falha na geração do DETALHE do arquivo de REMESSA.", e);
            }
        }


        public string EscreverTrailer(TrailerRemessaDebitoAutomatico infoTrailer, int sequenciaRegistro)
        {
            if (infoTrailer == null)
                throw new Exception("Os dados não foram informados na geração do TRAILER.");

            var trailer = new string(' ', 150);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, infoTrailer.CodigoRegistro);
                trailer = trailer.PreencherValorNaLinha(2, 7, infoTrailer.QuantidadeRegistros.BoletoBrToStringSafe().PadLeft(6, '0'));
                trailer = trailer.PreencherValorNaLinha(8, 24, infoTrailer.TotalRegistros.ToStringParaValoresDecimais().PadLeft(17, '0'));
                trailer = trailer.PreencherValorNaLinha(25, 150, string.Empty.PadRight(126, ' '));

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception($@"<BoletoBr>{Environment.NewLine}Falha na geração do TRAILER do arquivo de REMESSA.", e);
            }
        }

        public List<string> EscreverTexto(RemessaDebitoAutomatico remessaEscrever)
        {
            List<string> listaRetornar = new List<string>();

            listaRetornar.Add(EscreverHeader(remessaEscrever.Header));

            var sequenciaC = 2;
            if (remessaEscrever.RegistrosDetalheC != null)
                foreach (var detalheAdicionar in remessaEscrever.RegistrosDetalheC)
                {
                    listaRetornar.AddRange(new[] { EscreverDetalhe(detalheAdicionar, sequenciaC) });
                    sequenciaC++;
                }
            var sequenciaD = sequenciaC;
            if (remessaEscrever.RegistrosDetalheD != null)
                foreach (var detalheAdicionar in remessaEscrever.RegistrosDetalheD)
                {
                    listaRetornar.AddRange(new[] { EscreverDetalhe(detalheAdicionar, sequenciaD) });
                    sequenciaD++;
                }
            var sequenciaE = sequenciaD;
            if (remessaEscrever.RegistrosDetalheE != null)
                foreach (var detalheAdicionar in remessaEscrever.RegistrosDetalheE)
                {
                    listaRetornar.AddRange(new[] { EscreverDetalhe(detalheAdicionar, sequenciaE) });
                    sequenciaE++;
                }
            var sequenciaI = sequenciaE;
            if (remessaEscrever.RegistrosDetalheI != null)
                foreach (var detalheAdicionar in remessaEscrever.RegistrosDetalheI)
                {
                    listaRetornar.AddRange(new[] { EscreverDetalhe(detalheAdicionar, sequenciaI) });
                    sequenciaI++;
                }
            var sequenciaJ = sequenciaI;
            if (remessaEscrever.RegistrosDetalheJ != null)
                foreach (var detalheAdicionar in remessaEscrever.RegistrosDetalheJ)
                {
                    listaRetornar.AddRange(new[] { EscreverDetalhe(detalheAdicionar, sequenciaJ) });
                    sequenciaJ++;
                }
            var sequenciaL = sequenciaJ;
            if (remessaEscrever.RegistrosDetalheL != null)
                foreach (var detalheAdicionar in remessaEscrever.RegistrosDetalheL)
                {
                    listaRetornar.AddRange(new[] { EscreverDetalhe(detalheAdicionar, sequenciaL) });
                    sequenciaL++;
                }

            /*trailer*/
            var total = remessaEscrever.RegistrosDetalheE != null ? remessaEscrever.RegistrosDetalheE.Sum(s => s.ValorDebito) : 0;

            remessaEscrever.Trailer = new TrailerRemessaDebitoAutomatico(sequenciaL, total);

            listaRetornar.Add(EscreverTrailer(remessaEscrever.Trailer, sequenciaL));

            return listaRetornar;
        }
    }
}
