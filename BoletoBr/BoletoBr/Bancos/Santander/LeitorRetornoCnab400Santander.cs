using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Dominio;

namespace BoletoBr.Bancos.Santander
{
    public class LeitorRetornoCnab400Santander : ILeitorArquivoRetornoCnab400
    {
        private readonly List<string> _linhasArquivo;

        public LeitorRetornoCnab400Santander(List<string> linhasArquivo)
        {
            _linhasArquivo = linhasArquivo;
        }

        public RetornoCnab400 ProcessarRetorno()
        {
            /* Validações */
            #region Validações
            ValidaArquivoRetorno();
            #endregion

            var objRetornar = new RetornoCnab400();
            objRetornar.RegistrosDetalhe = new List<DetalheRetornoCnab400>();

            foreach (var linhaAtual in _linhasArquivo)
            {
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "0")
                {
                    objRetornar.Header = ObterHeader(linhaAtual);
                }
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "1")
                {
                    var objDetalhe = ObterRegistrosDetalhe(linhaAtual);
                    objRetornar.RegistrosDetalhe.Add(objDetalhe);
                }
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "9")
                {
                    objRetornar.Trailer = ObterTrailer(linhaAtual);
                }
            }

            return objRetornar;
        }

        public RetornoCnab400 ProcessarRetorno(TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
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
                _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(1, 1) == "0");

            if (qtdLinhasHeader <= 0)
                throw new Exception("Não foi encontrado HEADER do arquivo de retorno.");

            if (qtdLinhasHeader > 1)
                throw new Exception("Não é permitido mais de um HEADER no arquivo de retorno.");

            var qtdLinhasDetalhe = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(1, 1) == "1");

            if (qtdLinhasDetalhe <= 0)
                throw new Exception("Não foi encontrado DETALHE do arquivo de retorno.");

            var qtdLinhasTrailer = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(1, 1) == "9");

            if (qtdLinhasTrailer <= 0)
                throw new Exception("Não foi encontrado TRAILER do arquivo de retorno.");

            if (qtdLinhasTrailer > 1)
                throw new Exception("Não é permitido mais de um TRAILER no arquivo de retorno.");
        }

        public HeaderRetornoCnab400 ObterHeader(string linha)
        {
            var objRetornar = new HeaderRetornoCnab400();

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
            objRetornar.TipoRetorno = linha.ExtrairValorDaLinha(2, 2);
            objRetornar.LiteralRetorno = linha.ExtrairValorDaLinha(3, 9);
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(10, 11);
            objRetornar.LiteralServico = linha.ExtrairValorDaLinha(12, 26);
            objRetornar.CodigoAgenciaCedente = linha.ExtrairValorDaLinha(27, 30).BoletoBrToInt();
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(31, 38);
            objRetornar.ContaCobranca = linha.ExtrairValorDaLinha(39, 46);
            objRetornar.NomeDoBeneficiario = linha.ExtrairValorDaLinha(47, 76);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(77, 79);
            objRetornar.NomeDoBanco = linha.ExtrairValorDaLinha(80, 94);
            objRetornar.DataGeracaoGravacao =
                (DateTime)linha.ExtrairValorDaLinha(95, 100).ToString().ToDateTimeFromDdMmAa();
            // Zeros (101-110)
            objRetornar.CodigoDoBeneficiario = linha.ExtrairValorDaLinha(111, 117);
            // Brancos (118-391)
            objRetornar.Versao = linha.ExtrairValorDaLinha(114, 119).BoletoBrToInt();
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400);

            return objRetornar;
        }

        public DetalheRetornoCnab400 ObterRegistrosDetalhe(string linha)
        {
            var objRetornar = new DetalheRetornoCnab400();

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
            objRetornar.TipoInscricao = linha.ExtrairValorDaLinha(2, 3).BoletoBrToInt();
            objRetornar.NumeroInscricao = linha.ExtrairValorDaLinha(4, 17).BoletoBrToLong();
            objRetornar.CodigoAgenciaCedente = linha.ExtrairValorDaLinha(18, 21).BoletoBrToInt();
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(22, 29);
            objRetornar.ContaCobranca = linha.ExtrairValorDaLinha(29, 29);
            objRetornar.CodigoDoDocumentoEmpresa = linha.ExtrairValorDaLinha(38, 62);
            objRetornar.NossoNumero = linha.ExtrairValorDaLinha(63, 70);
            // Brancos (71-107)
            objRetornar.CodigoCarteira = linha.ExtrairValorDaLinha(108, 108);
            objRetornar.CodigoDeOcorrencia = linha.ExtrairValorDaLinha(109, 110).BoletoBrToInt();
            objRetornar.DataDaOcorrencia = (DateTime)linha.ExtrairValorDaLinha(111, 116).ToString().ToDateTimeFromDdMmAa();
            objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(117, 126);
            objRetornar.NossoNumero = linha.ExtrairValorDaLinha(127, 134);
            objRetornar.CodigoOriginalRemessa = linha.ExtrairValorDaLinha(135, 136);
            objRetornar.CodigoOcorrencia1 = linha.ExtrairValorDaLinha(137, 139);
            objRetornar.CodigoOcorrencia2 = linha.ExtrairValorDaLinha(140, 142);
            objRetornar.CodigoOcorrencia3 = linha.ExtrairValorDaLinha(143, 145);
            // Brancos (146-146)
            objRetornar.DataDeVencimento = (DateTime)linha.ExtrairValorDaLinha(147, 152).ToString().ToDateTimeFromDdMmAa();
            objRetornar.ValorDoTituloParcela = linha.ExtrairValorDaLinha(153, 165).BoletoBrToDecimal()/100;
            objRetornar.BancoCobrador = linha.ExtrairValorDaLinha(166, 168).BoletoBrToInt();
            objRetornar.AgenciaCobradora = linha.ExtrairValorDaLinha(169, 173).BoletoBrToInt();
            objRetornar.Especie = linha.ExtrairValorDaLinha(174, 175);
            objRetornar.ValorTarifa = linha.ExtrairValorDaLinha(176, 188).BoletoBrToDecimal()/100;
            objRetornar.ValorOutrasDespesas = linha.ExtrairValorDaLinha(189, 201).BoletoBrToDecimal()/100;
            objRetornar.ValorJurosAtraso = linha.ExtrairValorDaLinha(202, 214).BoletoBrToDecimal()/100;
            objRetornar.ValorIof = linha.ExtrairValorDaLinha(215, 227).BoletoBrToDecimal()/100;
            objRetornar.ValorAbatimento = linha.ExtrairValorDaLinha(228, 240).BoletoBrToDecimal()/100;
            objRetornar.ValorDesconto = linha.ExtrairValorDaLinha(241, 253).BoletoBrToDecimal()/100;
            objRetornar.ValorPrincipal = linha.ExtrairValorDaLinha(254, 266).BoletoBrToDecimal()/100;
            // Multa também é considerada
            objRetornar.ValorJurosDeMora = linha.ExtrairValorDaLinha(267, 279).BoletoBrToDecimal()/100;
            objRetornar.ValorOutrosCreditos = linha.ExtrairValorDaLinha(280, 292).BoletoBrToDecimal()/100;
            // Brancos (295-295)
            objRetornar.DataDeCredito = (DateTime)linha.ExtrairValorDaLinha(296, 301).ToString().ToDateTimeFromDdMmAa();
            objRetornar.NomeSacado = linha.ExtrairValorDaLinha(302, 337);
            objRetornar.IdentificadorComplemento = linha.ExtrairValorDaLinha(338, 338);
            objRetornar.UnidadeValorMoedaCorrente = linha.ExtrairValorDaLinha(339, 340);
            objRetornar.ValorTituloOutraUnidadeValor = linha.ExtrairValorDaLinha(341, 353).BoletoBrToDecimal()/100;
            objRetornar.ValorIOCOutraUnidadeValor = linha.ExtrairValorDaLinha(354, 366).BoletoBrToDecimal()/100;
            objRetornar.ValorDoDebitoCredito = linha.ExtrairValorDaLinha(367, 379).BoletoBrToDecimal()/100;
            objRetornar.IndicativoDebitoCredito = linha.ExtrairValorDaLinha(380, 380);
            // Brancos (381-391)
            objRetornar.Versao = linha.ExtrairValorDaLinha(392, 394).BoletoBrToInt();
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();

            return objRetornar;
        }

        public TrailerRetornoCnab400 ObterTrailer(string linha)
        {
            var objRetornar = new TrailerRetornoCnab400();

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
            objRetornar.CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).BoletoBrToInt();
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(3, 4);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(5, 7);
            // Brancos (8-17)
            objRetornar.QtdTitulosCobrancaSimples = linha.ExtrairValorDaLinha(18, 25).BoletoBrToLong();
            objRetornar.ValorTitulosCobrancaSimples = linha.ExtrairValorDaLinha(26, 39).BoletoBrToDecimal() / 100;
            objRetornar.ReferenciaAvisoBancario1 = linha.ExtrairValorDaLinha(40, 47);
            // Brancos (48-97)
            objRetornar.QtdTitulosCobrancaCaucionada = linha.ExtrairValorDaLinha(98, 105).BoletoBrToLong();
            objRetornar.ValorTitulosCobrancaCaucionada = linha.ExtrairValorDaLinha(106, 119).BoletoBrToDecimal() / 100;
            objRetornar.ReferenciaAvisoBancario2 = linha.ExtrairValorDaLinha(120, 127);
            // Brancos (128-137)
            objRetornar.QtdTitulosCobrancaDescontada = linha.ExtrairValorDaLinha(138, 145).BoletoBrToInt();
            objRetornar.ValorTitulosCobrancaDescontada = linha.ExtrairValorDaLinha(146, 159).BoletoBrToDecimal() / 100;
            objRetornar.ReferenciaAvisoBancario3 = linha.ExtrairValorDaLinha(160, 167);
            // Brancos (168-391)
            objRetornar.Versao = linha.ExtrairValorDaLinha(392, 394).BoletoBrToInt();
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();

            return objRetornar;
        }
    }
}
