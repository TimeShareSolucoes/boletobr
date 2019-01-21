using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Dominio;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Banestes
{
    public class LeitorRetornoCnab400Banestes : ILeitorArquivoRetornoCnab400
    {
        private readonly List<string> _linhasArquivo;

        public LeitorRetornoCnab400Banestes(List<string> linhasArquivo)
        {
            _linhasArquivo = linhasArquivo;
        }

        public RetornoCnab400 ProcessarRetorno(TipoArquivo tipoArquivo)
        {
            #region Validações

            ValidaArquivoRetorno();

            #endregion

            var objRetornar = new RetornoCnab400 { RegistrosDetalhe = new List<DetalheRetornoCnab400>() };

            foreach (var linhaAtual in _linhasArquivo)
            {
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "0")
                    objRetornar.Header = ObterHeader(linhaAtual);
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "1")
                    objRetornar.RegistrosDetalhe.Add(ObterRegistrosDetalhe(linhaAtual));
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "9")
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
            var objRetornar = new HeaderRetornoCnab400
            {
                CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt(),
                CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).BoletoBrToInt(),
                LiteralRetorno = linha.ExtrairValorDaLinha(3, 9),
                CodigoDoServico = linha.ExtrairValorDaLinha(10, 11),
                LiteralServico = linha.ExtrairValorDaLinha(12, 26),
                CodigoDoBeneficiario = linha.ExtrairValorDaLinha(27, 37),
                NomeDoBeneficiario = linha.ExtrairValorDaLinha(47, 76),
                CodigoDoBanco = linha.ExtrairValorDaLinha(77, 79),
                NomeDoBanco = linha.ExtrairValorDaLinha(80, 87),
                DataGeracaoGravacao = Convert.ToDateTime(linha.ExtrairValorDaLinha(95, 100).ToDateTimeFromDdMmAa()),
                NumeroSequencial = linha.ExtrairValorDaLinha(395, 400)
            };

            return objRetornar;
        }

        public DetalheRetornoCnab400 ObterRegistrosDetalhe(string linha)
        {
            try
            {
                var objRetornar = new DetalheRetornoCnab400();

                objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
                objRetornar.TipoInscricao = linha.ExtrairValorDaLinha(2, 3).BoletoBrToInt();
                objRetornar.NumeroInscricao = linha.ExtrairValorDaLinha(4, 17).BoletoBrToLong();
                objRetornar.IdentificacaoEmpresaNoBanco = linha.ExtrairValorDaLinha(18, 28);
                objRetornar.NossoNumero = linha.ExtrairValorDaLinha(63, 72);
                objRetornar.IdentificadorLancamentoConta = linha.ExtrairValorDaLinha(83, 84).BoletoBrToInt();
                objRetornar.CodigoCarteira = linha.ExtrairValorDaLinha(108, 108);
                objRetornar.CodigoDeOcorrencia = linha.ExtrairValorDaLinha(109, 110).BoletoBrToInt();
                objRetornar.DataDaOcorrencia =
                    Convert.ToDateTime(linha.ExtrairValorDaLinha(111, 116).ToDateTimeFromDdMmAa());
                objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(117, 126);
                objRetornar.DataDeVencimento =
                    Convert.ToDateTime(linha.ExtrairValorDaLinha(147, 152).ToDateTimeFromDdMmAa());
                objRetornar.ValorDoTituloParcela = linha.ExtrairValorDaLinha(156, 165).BoletoBrToDecimal() / 100;
                objRetornar.BancoCobrador = linha.ExtrairValorDaLinha(166, 168).BoletoBrToInt();
                objRetornar.AgenciaCobradora = linha.ExtrairValorDaLinha(169, 173).BoletoBrToInt();
                objRetornar.Especie = linha.ExtrairValorDaLinha(174, 175);
                objRetornar.ValorTarifa = linha.ExtrairValorDaLinha(176, 188).BoletoBrToDecimal() / 100;
                objRetornar.ValorOutrasDespesas = linha.ExtrairValorDaLinha(189, 201).BoletoBrToDecimal() / 100;
                objRetornar.ValorIof = linha.ExtrairValorDaLinha(215, 227).BoletoBrToDecimal() / 100;
                objRetornar.ValorAbatimento = linha.ExtrairValorDaLinha(228, 240).BoletoBrToDecimal() / 100;
                objRetornar.ValorDesconto = linha.ExtrairValorDaLinha(241, 253).BoletoBrToDecimal() / 100;
                objRetornar.ValorLiquidoRecebido = linha.ExtrairValorDaLinha(254, 266).BoletoBrToDecimal() / 100;
                objRetornar.ValorJurosDeMora = linha.ExtrairValorDaLinha(267, 279).BoletoBrToDecimal() / 100;
                objRetornar.ValorOutrosCreditos = linha.ExtrairValorDaLinha(280, 292).BoletoBrToDecimal() / 100;
                objRetornar.MotivoDaOcorrencia = linha.ExtrairValorDaLinha(319, 320).BoletoBrToInt();
                objRetornar.CodigoOriginalRemessa = linha.ExtrairValorDaLinha(377, 382);
                objRetornar.DataDeCredito =
                    Convert.ToDateTime(linha.ExtrairValorDaLinha(111, 116).ToDateTimeFromDdMmAa());
                objRetornar.Moeda = linha.ExtrairValorDaLinha(394, 394).BoletoBrToInt();
                objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();

                return objRetornar;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TrailerRetornoCnab400 ObterTrailer(string linha)
        {
            var objRetornar = new TrailerRetornoCnab400
            {
                CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt(),
                CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).BoletoBrToInt(),
                TipoRegistro = linha.ExtrairValorDaLinha(3, 4),
                CodigoDoBanco = linha.ExtrairValorDaLinha(5, 7)
            };

            /** POSIÇÃO ATUAL DA COBRANÇA SIMPLES **/
            objRetornar.QtdTitulosCobranca = linha.ExtrairValorDaLinha(18, 25).BoletoBrToInt();
            objRetornar.ValorTotalCobranca = linha.ExtrairValorDaLinha(26, 39).BoletoBrToDecimal() / 100;
            objRetornar.NumeroAvisoBancario = linha.ExtrairValorDaLinha(40, 47).BoletoBrToLong();

            /** POSIÇÃO ATUAL DA COBRANÇA CAUCIONADA **/
            if (objRetornar.QtdTitulosCobranca == 0)
                objRetornar.QtdTitulosCobranca = linha.ExtrairValorDaLinha(98, 105).BoletoBrToInt();
            if (objRetornar.ValorTotalCobranca == 0)
                objRetornar.ValorTotalCobranca = linha.ExtrairValorDaLinha(106, 119).BoletoBrToDecimal() / 100;
            if (objRetornar.NumeroAvisoBancario == 0)
                objRetornar.NumeroAvisoBancario = linha.ExtrairValorDaLinha(120, 127).BoletoBrToLong();

            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();

            return objRetornar;
        }
    }
}
