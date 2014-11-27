using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.CNAB400.Retorno;
using BoletoBr.Dominio;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Itau
{
    public class LeitorRetornoCnab400Itau : ILeitorArquivoRetornoCnab400
    {
       private readonly List<string> _linhasArquivo;

       public LeitorRetornoCnab400Itau(List<string> linhasArquivo)
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
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(33, 37);
            objRetornar.DacAgenciaConta = linha.ExtrairValorDaLinha(38, 38).BoletoBrToInt();
            objRetornar.NomeDoBeneficiario = linha.ExtrairValorDaLinha(47, 76);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(77, 79);
            objRetornar.NomeDoBanco = linha.ExtrairValorDaLinha(80, 94);
            objRetornar.DataGeracaoGravacao =
                (DateTime) linha.ExtrairValorDaLinha(95, 100).ToString().ToDateTimeFromDdMmAa();
            objRetornar.Densidade = linha.ExtrairValorDaLinha(101, 105);
            objRetornar.LiteralDensidade = linha.ExtrairValorDaLinha(106, 108);
            objRetornar.SequencialRetorno = linha.ExtrairValorDaLinha(109, 113);
            objRetornar.DataCredito = (DateTime) linha.ExtrairValorDaLinha(114, 119).ToString().ToDateTimeFromDdMmAa();
            // Brancos
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
            // Zeros
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(24, 28);
            objRetornar.DacAgenciaConta = linha.ExtrairValorDaLinha(29, 29).BoletoBrToInt();
            // Brancos
            objRetornar.CodigoDoDocumentoEmpresa = linha.ExtrairValorDaLinha(38, 62);
            objRetornar.CodigoDoDocumentoBanco = linha.ExtrairValorDaLinha(63, 70);
            // Brancos
            objRetornar.NumeroCarteira = linha.ExtrairValorDaLinha(83, 85).BoletoBrToInt();
            objRetornar.NossoNumero = linha.ExtrairValorDaLinha(86, 93);
            objRetornar.DacNossoNumero = linha.ExtrairValorDaLinha(94, 94).BoletoBrToInt();
            // Brancos
            objRetornar.CodigoCarteira = linha.ExtrairValorDaLinha(108, 108);
            objRetornar.CodigoDeOcorrencia = linha.ExtrairValorDaLinha(109, 110).BoletoBrToInt();
            objRetornar.DataDaOcorrencia = (DateTime) linha.ExtrairValorDaLinha(111, 116).ToString().ToDateTimeFromDdMmAa();
            objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(117, 126);
            objRetornar.NossoNumero = linha.ExtrairValorDaLinha(127, 134);
            // Brancos
            objRetornar.DataDeVencimento = (DateTime) linha.ExtrairValorDaLinha(147, 152).ToString().ToDateTimeFromDdMmAa();
            objRetornar.ValorDoTituloParcela = linha.ExtrairValorDaLinha(153, 165).BoletoBrToDecimal()/100;
            objRetornar.BancoCobrador = linha.ExtrairValorDaLinha(166, 168).BoletoBrToInt();
            objRetornar.AgenciaCobradora = linha.ExtrairValorDaLinha(169, 172).BoletoBrToInt();
            objRetornar.DvAgenciaCobradora = linha.ExtrairValorDaLinha(173, 173);
            objRetornar.Especie = linha.ExtrairValorDaLinha(174, 175);
            objRetornar.ValorTarifa = linha.ExtrairValorDaLinha(176, 188).BoletoBrToDecimal()/100;
            // Brancos
            objRetornar.ValorIof = linha.ExtrairValorDaLinha(215, 227).BoletoBrToDecimal()/100;
            objRetornar.ValorAbatimento = linha.ExtrairValorDaLinha(228, 240).BoletoBrToDecimal()/100;
            objRetornar.ValorDesconto = linha.ExtrairValorDaLinha(241, 253).BoletoBrToDecimal()/100;
            objRetornar.ValorPrincipal = linha.ExtrairValorDaLinha(254, 266).BoletoBrToDecimal()/100;
            // Multa também é considerada
            objRetornar.ValorJurosDeMora = linha.ExtrairValorDaLinha(267, 279).BoletoBrToDecimal()/100;
            objRetornar.ValorOutrosCreditos = linha.ExtrairValorDaLinha(280, 292).BoletoBrToDecimal()/100;
            objRetornar.IndicadorBoletoDDA = linha.ExtrairValorDaLinha(293, 293);
            // Brancos
            objRetornar.DataDeCredito = linha.ExtrairValorDaLinha(296, 301).ToString().ToDateTimeFromDdMmAa().Equals(null) ? new DateTime(0001, 01, 01) : (DateTime)linha.ExtrairValorDaLinha(296, 301).ToString().ToDateTimeFromDdMmAa();
            objRetornar.CodigoInstrucaoCancelada = linha.ExtrairValorDaLinha(302, 305).BoletoBrToInt();
            // Brancos
            // Zeros
            objRetornar.NomeSacado = linha.ExtrairValorDaLinha(325, 354);
            // Brancos
            objRetornar.MensagemInformativa = linha.ExtrairValorDaLinha(378, 385);
            // Brancos
            objRetornar.CodigoFormaPagamento = linha.ExtrairValorDaLinha(393, 394);
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();

            return objRetornar;
        }

        public TrailerRetornoCnab400 ObterTrailer(string linha)
        {
            var objRetornar = new TrailerRetornoCnab400();

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1,1).BoletoBrToInt();
            objRetornar.CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).BoletoBrToInt();
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(3, 4);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(5, 7);
            // Brancos
            objRetornar.QtdTitulosCobrancaSimples = linha.ExtrairValorDaLinha(18, 25).BoletoBrToLong();
            objRetornar.ValorTitulosCobrancaSimples = linha.ExtrairValorDaLinha(26, 39).BoletoBrToDecimal()/100;
            objRetornar.ReferenciaAvisoBancario1 = linha.ExtrairValorDaLinha(40, 47);
            // Brancos
            objRetornar.QtdTitulosCobrancaVinculada = linha.ExtrairValorDaLinha(58, 65).BoletoBrToLong();
            objRetornar.ValorTitulosCobrancaVinculada = linha.ExtrairValorDaLinha(66, 79).BoletoBrToDecimal()/100;
            objRetornar.ReferenciaAvisoBancario2 = linha.ExtrairValorDaLinha(80, 87);
            // Brancos
            objRetornar.QtdTitulosCobrancaDiretivaEscritural = linha.ExtrairValorDaLinha(178, 185).BoletoBrToInt();
            objRetornar.ValorTitulosCobrancaDiretivaEscritural = linha.ExtrairValorDaLinha(186, 199).BoletoBrToDecimal()/100;
            objRetornar.ReferenciaAvisoBancario3 = linha.ExtrairValorDaLinha(200, 207);
            objRetornar.NumeroSequencialRetorno = linha.ExtrairValorDaLinha(208, 212).BoletoBrToInt();
            objRetornar.QtdRegistrosDetalhe = linha.ExtrairValorDaLinha(213, 220).BoletoBrToInt();
            objRetornar.ValorTotalCobranca = linha.ExtrairValorDaLinha(221, 234).BoletoBrToDecimal()/100;
            // Brancos
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();

            return objRetornar;
        }
    }
}
