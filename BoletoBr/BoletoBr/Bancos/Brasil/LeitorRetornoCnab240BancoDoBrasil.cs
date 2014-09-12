using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BoletoBr.Arquivo.CNAB240;
using BoletoBr.Arquivo.CNAB240.Retorno;
using BoletoBr.Dominio;

namespace BoletoBr.Bancos.Brasil
{
    public class LeitorRetornoCnab240BancoDoBrasil : ILeitorArquivoRetornoCnab240
    {
        private readonly List<string> _linhasArquivo;

        public LeitorRetornoCnab240BancoDoBrasil(List<string> linhasArquivo)
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
                    ultimoRegistroDetalheIdentificado = new DetalheRetornoCnab240();
                    objRetornar.Lotes.Add(ultimoLoteIdentificado);

                    ultimoLoteIdentificado.HeaderLote = ObterHeaderLote(linhaAtual);
                }
                if (linhaAtual.ExtrairValorDaLinha(8, 8) == "3")
                {
                    if (linhaAtual.ExtrairValorDaLinha(14, 14) == "T")
                    {
                        var objDetalhe = ObterRegistrosDetalheT(linhaAtual);
                        if (ultimoLoteIdentificado == null)
                            throw new Exception("Não foi encontrado header de lote para o segmento atual.");

                        //ultimoLoteIdentificado.RegistrosDetalheSegmentoT.Add(objDetalhe);
                        ultimoRegistroDetalheIdentificado.SegmentoT = ObterRegistrosDetalheT(linhaAtual);
                    }
                    if (linhaAtual.ExtrairValorDaLinha(14, 14) == "U")
                    {
                        var objDetalhe = ObterRegistrosDetalheU(linhaAtual);
                        if (ultimoLoteIdentificado == null)
                            throw new Exception("Não foi encontrado header de lote para o segmento atual.");

                        //ultimoLoteIdentificado.RegistrosDetalheSegmentoU.Add(objDetalhe);
                        ultimoRegistroDetalheIdentificado.SegmentoU = ObterRegistrosDetalheU(linhaAtual);
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

        public HeaderRetornoCnab240 ObterHeader(string linha)
        {
            HeaderRetornoCnab240 objRetornar = new HeaderRetornoCnab240();

            objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt();
            objRetornar.LoteServico = linha.ExtrairValorDaLinha(4, 7);
            objRetornar.CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt();
            objRetornar.TipoInscricaoEmpresa = linha.ExtrairValorDaLinha(18, 18).BoletoBrToInt();
            objRetornar.NumeroInscricaoEmpresa = linha.ExtrairValorDaLinha(19, 32);
            objRetornar.Convenio = linha.ExtrairValorDaLinha(33, 52);
            // Dados convênio (33 - 52)
            objRetornar.ConvenioNumeroCobranca = linha.ExtrairValorDaLinha(33, 41);
            objRetornar.CedenteCobranca = linha.ExtrairValorDaLinha(42, 45).BoletoBrToInt();
            objRetornar.CarteiraCobranca = linha.ExtrairValorDaLinha(46, 47).BoletoBrToInt();
            objRetornar.VariacaoCarteiraCobranca = linha.ExtrairValorDaLinha(48, 50).BoletoBrToInt();
            // ...
            objRetornar.CodigoAgencia = linha.ExtrairValorDaLinha(53, 57).BoletoBrToInt();
            objRetornar.DvAgenciaConta = linha.ExtrairValorDaLinha(58, 58);
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(59, 70);
            objRetornar.DvContaCorrente = linha.ExtrairValorDaLinha(71, 71);
            objRetornar.DvAgenciaConta = linha.ExtrairValorDaLinha(72, 72);
            objRetornar.NomeDoBeneficiario = linha.ExtrairValorDaLinha(73, 102);
            objRetornar.NomeDoBanco = linha.ExtrairValorDaLinha(103, 132);
            objRetornar.CodigoRemessaRetorno = linha.ExtrairValorDaLinha(143, 143).BoletoBrToInt();
            objRetornar.DataGeracaoGravacao = (DateTime) linha.ExtrairValorDaLinha(144, 151).ToString().ToDateTimeFromDdMmAa();
            objRetornar.HoraGeracaoGravacao = linha.ExtrairValorDaLinha(152, 157).BoletoBrToInt();
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(158, 163).BoletoBrToInt();
            objRetornar.VersaoLayout = linha.ExtrairValorDaLinha(164, 166);
            objRetornar.Densidade = linha.ExtrairValorDaLinha(167, 171);
            objRetornar.UsoBanco = linha.ExtrairValorDaLinha(172, 191);
            objRetornar.UsoEmpresa = linha.ExtrairValorDaLinha(192, 211);
            objRetornar.UsoFebraban = linha.ExtrairValorDaLinha(212, 240);

            return objRetornar;
        }

        public HeaderLoteRetornoCnab240 ObterHeaderLote(string linha)
        {
            HeaderLoteRetornoCnab240 objRetornar = new HeaderLoteRetornoCnab240();

            objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt();
            objRetornar.LoteServico = linha.ExtrairValorDaLinha(4, 7);
            objRetornar.CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt();
            objRetornar.TipoOperacao = linha.ExtrairValorDaLinha(9, 9);
            objRetornar.TipoServico = linha.ExtrairValorDaLinha(10, 11).BoletoBrToInt();
            objRetornar.VersaoLayoutLote = linha.ExtrairValorDaLinha(14, 16).BoletoBrToInt();
            objRetornar.TipoInscricaoEmpresa = linha.ExtrairValorDaLinha(18, 18).BoletoBrToInt();
            objRetornar.NumeroInscricaoEmpresa = linha.ExtrairValorDaLinha(19, 33);
            objRetornar.Convenio = linha.ExtrairValorDaLinha(34, 53);
            objRetornar.CodigoAgencia = linha.ExtrairValorDaLinha(54, 58).BoletoBrToInt();
            objRetornar.DvCodigoAgencia = linha.ExtrairValorDaLinha(59, 59);
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(60, 71);
            objRetornar.DvContaCorrente = linha.ExtrairValorDaLinha(72, 72);
            objRetornar.DvAgenciaConta = linha.ExtrairValorDaLinha(73, 73);
            objRetornar.NomeDoBeneficiario = linha.ExtrairValorDaLinha(74, 103);
            objRetornar.Mensagem1 = linha.ExtrairValorDaLinha(104, 143);
            objRetornar.Mensagem2 = linha.ExtrairValorDaLinha(144, 183);
            objRetornar.NumeroRemessaRetorno = linha.ExtrairValorDaLinha(184, 191);
            objRetornar.DataGeracaoGravacao = (DateTime) linha.ExtrairValorDaLinha(192, 199).ToString().ToDateTimeFromDdMmAaaa();
            objRetornar.DataDeCredito = (DateTime) linha.ExtrairValorDaLinha(200, 207).ToString().ToDateTimeFromDdMmAaaa();

            return objRetornar;
        }

        public DetalheSegmentoTRetornoCnab240 ObterRegistrosDetalheT(string linha)
        {
            DetalheSegmentoTRetornoCnab240 objRetornar = new DetalheSegmentoTRetornoCnab240();

            objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt();
            objRetornar.LoteServico = linha.ExtrairValorDaLinha(4, 7);
            objRetornar.CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt();
            objRetornar.NumeroRegistro = linha.ExtrairValorDaLinha(9, 13).BoletoBrToInt();
            objRetornar.CodigoSegmento = linha.ExtrairValorDaLinha(14, 14);
            objRetornar.CodigoMovimento = linha.ExtrairValorDaLinha(16, 17).BoletoBrToInt();
            objRetornar.Agencia = linha.ExtrairValorDaLinha(18, 22).BoletoBrToInt();
            objRetornar.DigitoAgencia = linha.ExtrairValorDaLinha(23, 23);
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(24, 35).BoletoBrToInt();
            objRetornar.DigitoContaCorrente = linha.ExtrairValorDaLinha(36, 36);
            objRetornar.DvAgenciaConta = linha.ExtrairValorDaLinha(37, 37);
            objRetornar.IdentificacaoTitulo = linha.ExtrairValorDaLinha(38, 57);
            objRetornar.CodigoCarteira = linha.ExtrairValorDaLinha(58, 58).BoletoBrToInt();
            objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(59, 73);
            objRetornar.ValorTitulo = linha.ExtrairValorDaLinha(82, 96).BoletoBrToDecimal()/100;
            objRetornar.BancoCobradorRecebedor = linha.ExtrairValorDaLinha(97, 99).BoletoBrToInt();
            objRetornar.AgenciaCobradoraRecebedora = linha.ExtrairValorDaLinha(100, 104).BoletoBrToInt();
            objRetornar.DvAgenciaCobradoraRecebedora = linha.ExtrairValorDaLinha(105, 105);
            objRetornar.IdentificacaoTituloNaEmpresa = linha.ExtrairValorDaLinha(106, 130);
            objRetornar.Moeda = linha.ExtrairValorDaLinha(131, 132).BoletoBrToInt();
            objRetornar.TipoInscricaoSacado = linha.ExtrairValorDaLinha(133, 133).BoletoBrToInt();
            objRetornar.NumeroInscricaoSacado = linha.ExtrairValorDaLinha(134, 148).BoletoBrToLong();
            objRetornar.NomeSacado = linha.ExtrairValorDaLinha(149, 188);
            objRetornar.NumeroContrato = linha.ExtrairValorDaLinha(189, 198).BoletoBrToLong();
            objRetornar.ValorTarifas = linha.ExtrairValorDaLinha(199, 213).BoletoBrToDecimal()/100;
            objRetornar.MotivoOcorrencia = linha.ExtrairValorDaLinha(214, 223);
            objRetornar.UsoFebraban = linha.ExtrairValorDaLinha(224, 240);

            return objRetornar;
        }

        public DetalheSegmentoURetornoCnab240 ObterRegistrosDetalheU(string linha)
        {
            DetalheSegmentoURetornoCnab240 objRetornar = new DetalheSegmentoURetornoCnab240();

            objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt();
            objRetornar.LoteServico = linha.ExtrairValorDaLinha(4, 7);
            objRetornar.CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt();
            objRetornar.NumeroRegistro = linha.ExtrairValorDaLinha(9, 13).BoletoBrToInt();
            objRetornar.CodigoSegmento = linha.ExtrairValorDaLinha(14, 14);
            objRetornar.CodigoMovimento = linha.ExtrairValorDaLinha(16, 17).BoletoBrToInt();
            objRetornar.JurosMultaEncargos = linha.ExtrairValorDaLinha(18, 32).BoletoBrToDecimal()/100;
            objRetornar.ValorDescontoConcedido = linha.ExtrairValorDaLinha(33, 47).BoletoBrToDecimal()/100;
            objRetornar.ValorAbatimentoConcedido = linha.ExtrairValorDaLinha(48, 62).BoletoBrToDecimal()/100;
            objRetornar.ValorIofRecolhido = linha.ExtrairValorDaLinha(63, 77).BoletoBrToDecimal()/100;
            objRetornar.ValorPagoPeloSacado = linha.ExtrairValorDaLinha(78, 92).BoletoBrToDecimal()/100;
            objRetornar.ValorLiquidoASerCreditado = linha.ExtrairValorDaLinha(93, 107).BoletoBrToDecimal()/100;
            objRetornar.ValorOutrasDespesas = linha.ExtrairValorDaLinha(108, 122).BoletoBrToDecimal()/100;
            objRetornar.ValorOutrosCreditos = linha.ExtrairValorDaLinha(123, 137).BoletoBrToDecimal()/100;
            objRetornar.DataOcorrencia = (DateTime) linha.ExtrairValorDaLinha(138, 145).ToString().ToDateTimeFromDdMmAa();
            objRetornar.DataCredito = (DateTime) linha.ExtrairValorDaLinha(146, 153).ToString().ToDateTimeFromDdMmAa();
            objRetornar.CodigoOcorrenciaPagador = linha.ExtrairValorDaLinha(154, 157);
            objRetornar.DataOcorrenciaPagador = (DateTime) linha.ExtrairValorDaLinha(158, 165).ToString().ToDateTimeFromDdMmAa();
            objRetornar.ValorOcorrenciaPagador = linha.ExtrairValorDaLinha(166, 180).BoletoBrToDecimal()/100;
            objRetornar.ComplementoOcorrenciaPagador = linha.ExtrairValorDaLinha(181, 210);
            objRetornar.CodigoBancoCompensacao = linha.ExtrairValorDaLinha(211, 213).BoletoBrToInt();
            objRetornar.NossoNumeroBancoCompensacao = linha.ExtrairValorDaLinha(214, 233);

            return objRetornar;
        }

        public TrailerLoteRetornoCnab240 ObterTrailerLote(string linha)
        {
            TrailerLoteRetornoCnab240 objRetornar = new TrailerLoteRetornoCnab240();

            objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt();
            objRetornar.LoteServico = linha.ExtrairValorDaLinha(4, 7);
            objRetornar.CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt();
            objRetornar.QtdRegistrosLote = linha.ExtrairValorDaLinha(18, 23).BoletoBrToInt();

            return objRetornar;
        }

        public TrailerRetornoCnab240 ObterTrailer(string linha)
        {
            TrailerRetornoCnab240 objRetornar = new TrailerRetornoCnab240();

            objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt();
            objRetornar.LoteServico = linha.ExtrairValorDaLinha(4, 7);
            objRetornar.CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt();
            objRetornar.QtdLotesArquivo = linha.ExtrairValorDaLinha(18, 23).BoletoBrToInt();
            objRetornar.QtdRegistrosArquivo = linha.ExtrairValorDaLinha(24, 29).BoletoBrToInt();
            objRetornar.QtdContasConc = linha.ExtrairValorDaLinha(30, 35).BoletoBrToInt();

            return objRetornar;
        }
    }
}
