using System;
using System.Collections.Generic;
using System.Linq;
using BoletoBr.Arquivo.CNAB240.Retorno;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Cef
{
    public class LeitorRetornoCnab240Cef : ILeitorArquivoRetornoCnab240
    {
       private readonly List<string> _linhasArquivo;

       public LeitorRetornoCnab240Cef(List<string> linhasArquivo)
        {
            _linhasArquivo = linhasArquivo;
        }

        public RetornoCnab240 ProcessarRetorno()
        {
            /* Validações */
            #region Validações
            ValidaArquivoRetorno();
            #endregion

            var objRetornar = new RetornoCnab240();

            LoteRetornoCnab240 ultimoLoteIdentificado = null;
            DetalheRetornoCnab240 ultimoRegistroDetalheIdentificado = null;

            foreach (var linhaAtual in _linhasArquivo)
            {
                /* Header de arquivo */
                if (linhaAtual.ExtrairValorDaLinha(8, 8) == "0")
                   objRetornar.Header = ObterHeader(linhaAtual);

                /* Header de Lote */
                if (linhaAtual.ExtrairValorDaLinha(8, 8) == "1")
                {
                    ultimoLoteIdentificado = new LoteRetornoCnab240();
                    
                    objRetornar.Lotes.Add(ultimoLoteIdentificado);

                    ultimoLoteIdentificado.HeaderLote = ObterHeaderLote(linhaAtual);
                }
                if (linhaAtual.ExtrairValorDaLinha(8, 8) == "3")
                {
                    if (linhaAtual.ExtrairValorDaLinha(14, 14) == "T")
                    {
                        if (ultimoLoteIdentificado == null)
                            throw new Exception("Não foi encontrado header de lote para o segmento atual.");

                        ultimoRegistroDetalheIdentificado = new DetalheRetornoCnab240();
                        ultimoLoteIdentificado.RegistrosDetalheSegmentos.Add(ultimoRegistroDetalheIdentificado);

                        var objSegmentoT = ObterRegistrosDetalheT(linhaAtual);
                        ultimoRegistroDetalheIdentificado.SegmentoT = objSegmentoT;

                    }
                    if (linhaAtual.ExtrairValorDaLinha(14, 14) == "U")
                    {
                        var objSegmentoU = ObterRegistrosDetalheU(linhaAtual);
                        if (ultimoLoteIdentificado == null)
                            throw new Exception("Não foi encontrado header de lote para o segmento atual.");
                        if (ultimoRegistroDetalheIdentificado == null)
                            throw new Exception("Não deveria ser nulo o último detalhe. Deveria ter sido criado no segmento T, anterior a este.");

                        ultimoRegistroDetalheIdentificado.SegmentoU = objSegmentoU;
                    }
                }
                /* Trailer de Lote */
                if (linhaAtual.ExtrairValorDaLinha(8, 8) == "5")
                    if (ultimoLoteIdentificado != null)
                        ultimoLoteIdentificado.TrailerLote = ObterTrailerLote(linhaAtual);

                /* Trailer de arquivo */
                if (linhaAtual.ExtrairValorDaLinha(8, 8) == "9")
                    objRetornar.Trailer = ObterTrailer(linhaAtual);
            }

            return objRetornar;
        }

        public void ValidaArquivoRetorno()
        {
            if (_linhasArquivo == null)
                throw new Exception("Dados do arquivo de retorno estão nulos. Impossível processar.");

            if (_linhasArquivo.Count <= 0)
                throw new Exception("Dados do arquivo de retorno não estão corretos. Impossível processar.");

            if (_linhasArquivo.Count < 3)
                throw new Exception("Dados do arquivo de retorno não contém o mínimo de 3 linhas. Impossível processar.");
            var qtdLinhasHeader =
                _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(8, 8) == "0");

            if (qtdLinhasHeader <= 0)
                throw new Exception("Não foi encontrado HEADER do arquivo de retorno.");

            if (qtdLinhasHeader > 1)
                throw new Exception("Não é permitido mais de um HEADER no arquivo de retorno.");

            var qtdLinhasHeaderLote = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(8, 8) == "1");

            if (qtdLinhasHeaderLote <= 0)
                throw new Exception("Não foi encontrado HEADER do arquivo de retorno.");

            if (qtdLinhasHeaderLote > 1)
                throw new Exception("Não é permitido mais de um HEADER no arquivo de retorno.");

            var qtdLinhasDetalheSegmentoT = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(14, 14) == "T");

            if (qtdLinhasDetalheSegmentoT <= 0)
                throw new Exception("Não foi encontrado DETALHE para o Segmento T no arquivo de retorno.");

            var qtdLinhasDetalheSegmentoU = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(14, 14) == "U");

            if (qtdLinhasDetalheSegmentoU <= 0)
                throw new Exception("Não foi encontrado DETALHE para o Segmento U no arquivo de retorno.");

            var qtdLinhasTrailerLote = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(8, 8) == "5");

            if (qtdLinhasTrailerLote <= 0)
                throw new Exception("Não foi encontrado TRAILER do arquivo de retorno.");

            if (qtdLinhasTrailerLote > 1)
                throw new Exception("Não é permitido mais de um TRAILER no arquivo de retorno.");

            var qtdLinhasTrailer = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(8, 8) == "9");

            if (qtdLinhasTrailer <= 0)
                throw new Exception("Não foi encontrado TRAILER do arquivo de retorno.");

            if (qtdLinhasTrailer > 1)
                throw new Exception("Não é permitido mais de um TRAILER no arquivo de retorno.");
        }

        public HeaderRetornoCnab240 ObterHeader(string linhaObterInformacoes)
        {
            var objRetornar = new HeaderRetornoCnab240();

            var linha = linhaObterInformacoes;

            objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt();
            objRetornar.LoteServico = linha.ExtrairValorDaLinha(4, 7);
            objRetornar.CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt();
            objRetornar.TipoInscricaoEmpresa = linha.ExtrairValorDaLinha(18, 18).BoletoBrToInt();
            objRetornar.NumeroInscricaoEmpresa = linha.ExtrairValorDaLinha(19, 32);
            objRetornar.CodigoAgencia = linha.ExtrairValorDaLinha(53, 57).BoletoBrToInt();
            objRetornar.DvCodigoAgencia = linha.ExtrairValorDaLinha(58, 58);
            // Uso Exclusivo CAIXA (37 - 46)
            objRetornar.CodigoCedente = linha.ExtrairValorDaLinha(59, 64).BoletoBrToInt();
            objRetornar.NomeDoBeneficiario = linha.ExtrairValorDaLinha(73, 102);
            objRetornar.NomeDoBanco = linha.ExtrairValorDaLinha(103, 132);
            objRetornar.CodigoRemessaRetorno = Convert.ToInt32(linha.ExtrairValorDaLinha(143, 143));
            objRetornar.DataGeracaoGravacao = linha.ExtrairValorDaLinha(144, 151).ToDateTimeFromDdMmAaaa();
            // Uso Exclusivo CAIXA (159 - 389)
            objRetornar.HoraGeracaoGravacao = linha.ExtrairValorDaLinha(152, 157).BoletoBrToInt();
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(158, 163).BoletoBrToInt();
            objRetornar.VersaoLayout = linha.ExtrairValorDaLinha(164, 166);
            objRetornar.Densidade = linha.ExtrairValorDaLinha(167, 171);
            objRetornar.UsoBanco = linha.ExtrairValorDaLinha(172, 191);
            objRetornar.UsoEmpresa = linha.ExtrairValorDaLinha(192, 211);
            objRetornar.VersaoAplicativo = linha.ExtrairValorDaLinha(212, 215);

            return objRetornar;
        }

        public HeaderLoteRetornoCnab240 ObterHeaderLote(string linhaObterInformacoes)
        {
            var objRetornar = new HeaderLoteRetornoCnab240();

            var linha = linhaObterInformacoes;

            objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt();
            objRetornar.LoteServico = linha.ExtrairValorDaLinha(4, 7);
            objRetornar.CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt();
            objRetornar.TipoOperacao = linha.ExtrairValorDaLinha(9, 9);
            objRetornar.TipoServico = linha.ExtrairValorDaLinha(10, 11).BoletoBrToInt();
            objRetornar.VersaoLayoutLote = linha.ExtrairValorDaLinha(14, 16).BoletoBrToInt();
            objRetornar.TipoInscricaoEmpresa = linha.ExtrairValorDaLinha(18, 18).BoletoBrToInt();
            objRetornar.NumeroInscricaoEmpresa = linha.ExtrairValorDaLinha(19, 33);
            objRetornar.Convenio = linha.ExtrairValorDaLinha(34, 39);
            objRetornar.CodigoAgencia = linha.ExtrairValorDaLinha(54, 58).BoletoBrToInt();
            objRetornar.DvCodigoAgencia = linha.ExtrairValorDaLinha(59, 59);
            objRetornar.CodigoCedente = linha.ExtrairValorDaLinha(60, 65).BoletoBrToInt();
            objRetornar.CodigoModeloPersonalizado = linha.ExtrairValorDaLinha(66, 72).BoletoBrToInt();
            objRetornar.NomeDoBeneficiario = linha.ExtrairValorDaLinha(74, 103);
            objRetornar.Mensagem1 = linha.ExtrairValorDaLinha(104, 143);
            objRetornar.Mensagem2 = linha.ExtrairValorDaLinha(144, 183);
            objRetornar.NumeroRemessaRetorno = linha.ExtrairValorDaLinha(184, 191);
            objRetornar.DataGeracaoGravacao = Convert.ToDateTime(linha.ExtrairValorDaLinha(192, 199).ToDateTimeFromDdMmAaaa());
            objRetornar.DataDeCredito = Convert.ToDateTime(linha.ExtrairValorDaLinha(200, 207).ToDateTimeFromDdMmAaaa());

            return objRetornar;
        }

        public DetalheSegmentoTRetornoCnab240 ObterRegistrosDetalheT(string linhaProcessar)
        {
            var objRetornar = new DetalheSegmentoTRetornoCnab240();

            var linha = linhaProcessar;

            objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt();
            objRetornar.LoteServico = linha.ExtrairValorDaLinha(4, 7);
            objRetornar.CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt();
            objRetornar.NumeroRegistro = linha.ExtrairValorDaLinha(9, 13).BoletoBrToInt();
            objRetornar.CodigoSegmento = linha.ExtrairValorDaLinha(14, 14);
            objRetornar.CodigoMovimento = linha.ExtrairValorDaLinha(16, 17).BoletoBrToInt();
            objRetornar.Agencia = linha.ExtrairValorDaLinha(18, 22).BoletoBrToInt();
            objRetornar.DigitoAgencia = linha.ExtrairValorDaLinha(23, 23);
            objRetornar.CodigoCedente = linha.ExtrairValorDaLinha(24, 29).BoletoBrToInt();
            objRetornar.NumeroBanco = linha.ExtrairValorDaLinha(33, 35).BoletoBrToInt();
            objRetornar.ModalidadeNossoNumero = linha.ExtrairValorDaLinha(40, 41).BoletoBrToInt();
            objRetornar.NossoNumero = linha.ExtrairValorDaLinha(42, 56);
            objRetornar.CodigoCarteira = linha.ExtrairValorDaLinha(58, 58).BoletoBrToInt();
            objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(59, 69);

            var dataVencimentoObtidaRetorno = linha.ExtrairValorDaLinha(74, 81).Trim();
            if (String.IsNullOrEmpty(dataVencimentoObtidaRetorno) == false)
                objRetornar.DataVencimento = dataVencimentoObtidaRetorno.ToDateTimeFromDdMmAaaa();

            objRetornar.ValorTitulo = linha.ExtrairValorDaLinha(82, 96).BoletoBrToDecimal() / 100;
            objRetornar.BancoCobradorRecebedor = linha.ExtrairValorDaLinha(97, 99).BoletoBrToInt();
            objRetornar.AgenciaCobradoraRecebedora = linha.ExtrairValorDaLinha(100, 104).BoletoBrToInt();
            objRetornar.DvAgenciaCobradoraRecebedora = linha.ExtrairValorDaLinha(105, 105);
            objRetornar.IdentificacaoTituloNaEmpresa = linha.ExtrairValorDaLinha(106, 130);
            objRetornar.Moeda = linha.ExtrairValorDaLinha(131, 132).BoletoBrToInt();
            objRetornar.TipoInscricaoSacado = linha.ExtrairValorDaLinha(133, 133).BoletoBrToInt();
            objRetornar.NumeroInscricaoSacado = linha.ExtrairValorDaLinha(134, 138).BoletoBrToLong();
            objRetornar.NomeSacado = linha.ExtrairValorDaLinha(149, 188);
            objRetornar.ValorTarifas = linha.ExtrairValorDaLinha(199, 213).BoletoBrToDecimal() / 100;
            objRetornar.MotivoOcorrencia = linha.ExtrairValorDaLinha(214, 223);

            return objRetornar;
        }

        public DetalheSegmentoURetornoCnab240 ObterRegistrosDetalheU(string linhaProcessar)
        {
            var objRetornar = new DetalheSegmentoURetornoCnab240();

            var linha = linhaProcessar;

            objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt();
            objRetornar.LoteServico = linha.ExtrairValorDaLinha(4, 7);
            objRetornar.CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt();
            objRetornar.NumeroRegistro = linha.ExtrairValorDaLinha(9, 13).BoletoBrToInt();
            objRetornar.CodigoSegmento = linha.ExtrairValorDaLinha(14, 14);
            objRetornar.CodigoMovimento = linha.ExtrairValorDaLinha(16, 17).BoletoBrToInt();
            objRetornar.JurosMultaEncargos = linha.ExtrairValorDaLinha(18, 32).BoletoBrToDecimal() / 100;
            objRetornar.ValorDescontoConcedido = linha.ExtrairValorDaLinha(33, 47).BoletoBrToDecimal() / 100;
            objRetornar.ValorAbatimentoConcedido = linha.ExtrairValorDaLinha(48, 62).BoletoBrToDecimal() / 100;
            objRetornar.ValorIofRecolhido = linha.ExtrairValorDaLinha(63, 77).BoletoBrToDecimal() / 100;
            objRetornar.ValorPagoPeloSacado = linha.ExtrairValorDaLinha(78, 92).BoletoBrToDecimal() / 100;
            objRetornar.ValorLiquidoASerCreditado = linha.ExtrairValorDaLinha(93, 107).BoletoBrToDecimal() / 100;
            objRetornar.ValorOutrasDespesas = linha.ExtrairValorDaLinha(108, 122).BoletoBrToDecimal() / 100;
            objRetornar.ValorOutrosCreditos = linha.ExtrairValorDaLinha(123, 137).BoletoBrToDecimal() / 100;

            var dataOcorrenciaObtidaRetorno = linha.ExtrairValorDaLinha(138, 145).Trim();
            if (!String.IsNullOrEmpty(dataOcorrenciaObtidaRetorno))
                objRetornar.DataOcorrencia = dataOcorrenciaObtidaRetorno.ToDateTimeFromDdMmAaaa();

            var dataCreditoArquivo = linha.ExtrairValorDaLinha(146, 153).Trim();
            if (String.IsNullOrEmpty(dataCreditoArquivo) == false)
                objRetornar.DataCredito = dataCreditoArquivo.ToDateTimeFromDdMmAaaa();
            
            if (objRetornar.CodigoMovimento == 35 || objRetornar.CodigoMovimento == 36 ||
                objRetornar.CodigoMovimento == 37)
            {
                objRetornar.NumeroBancoDeSacados = linha.ExtrairValorDaLinha(154, 156).BoletoBrToInt();
                objRetornar.NomeBancoDeSacados = linha.ExtrairValorDaLinha(157, 176);
                objRetornar.IdAjusteVencimento = linha.ExtrairValorDaLinha(177, 177);
                objRetornar.IdAjusteEmissao = linha.ExtrairValorDaLinha(178, 178);
                objRetornar.IdModeloBloqueto = linha.ExtrairValorDaLinha(179, 180);
                objRetornar.IdViaEntregaDistribuicao = linha.ExtrairValorDaLinha(181, 181);
                objRetornar.IdEspecieTitulo = linha.ExtrairValorDaLinha(182, 183);
                objRetornar.IdAceite = linha.ExtrairValorDaLinha(184, 184);
                objRetornar.CodigoSacadoNoBanco = linha.ExtrairValorDaLinha(185, 199);
            }
            else
            {
                var dataDebitoTarifaObtidaRetorno = linha.ExtrairValorDaLinha(158, 165).Trim();
                if (!String.IsNullOrEmpty(dataDebitoTarifaObtidaRetorno))
                    objRetornar.DataDebitoTarifa = dataDebitoTarifaObtidaRetorno.ToDateTimeFromDdMmAaaa();

                objRetornar.CodigoSacadoNoBanco = linha.ExtrairValorDaLinha(166, 180);
                objRetornar.CodigoBancoCompensacao = linha.ExtrairValorDaLinha(211, 213).BoletoBrToInt();
                objRetornar.NossoNumeroBancoCompensacao = linha.ExtrairValorDaLinha(214, 233);
            }

            return objRetornar;
        }

        public TrailerLoteRetornoCnab240 ObterTrailerLote(string linhaObterInformacoes)
        {
            var objRetornar = new TrailerLoteRetornoCnab240();

            var linha = linhaObterInformacoes;

            objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt();
            objRetornar.LoteServico = linha.ExtrairValorDaLinha(4, 7);
            objRetornar.CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt();
            objRetornar.QtdRegistrosLote = linha.ExtrairValorDaLinha(18, 23).BoletoBrToLong();
            objRetornar.QtdTitulosCobrancaSimples = linha.ExtrairValorDaLinha(24, 29).BoletoBrToLong();
            objRetornar.ValorTitulosCobrancaSimples = linha.ExtrairValorDaLinha(30, 46).BoletoBrToDecimal() / 100;
            objRetornar.QtdTitulosCobrancaCaucionada = linha.ExtrairValorDaLinha(47, 52).BoletoBrToLong();
            objRetornar.ValorTitulosCobrancaCaucionada = linha.ExtrairValorDaLinha(53, 69).BoletoBrToDecimal() / 100;
            objRetornar.QtdTitulosCobrancaDescontada = linha.ExtrairValorDaLinha(70, 75).BoletoBrToLong();
            objRetornar.ValorTitulosCobrancaDescontada = linha.ExtrairValorDaLinha(76, 92).BoletoBrToDecimal() / 100;

            return objRetornar;
        }

        public TrailerRetornoCnab240 ObterTrailer(string linhaObterInformacoes)
        {
            var objRetornar = new TrailerRetornoCnab240();

            var linha = linhaObterInformacoes;

            objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt();
            objRetornar.LoteServico = linha.ExtrairValorDaLinha(4, 7);
            objRetornar.CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt();
            objRetornar.QtdLotesArquivo = linha.ExtrairValorDaLinha(18, 23).BoletoBrToInt();
            objRetornar.QtdRegistrosArquivo = linha.ExtrairValorDaLinha(24, 29).BoletoBrToInt();

            return objRetornar;
        }

        public DetalheSegmentoERetornoCnab240 ObterRegistrosDetalheE(string linha)
        {
            throw new NotImplementedException();
        }
    }
}
