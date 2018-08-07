using System;
using System.Collections.Generic;
using System.Linq;
using BoletoBr.Dominio;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Itau
{
    public class LeitorRetornoCnab400Banrisul : ILeitorArquivoRetornoCnab400
    {
        private readonly List<string> _linhasArquivo;

        public LeitorRetornoCnab400Banrisul(List<string> linhasArquivo)
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
            objRetornar.LiteralServico = linha.ExtrairValorDaLinha(12, 19);
            objRetornar.CodigoAgenciaCedente = linha.ExtrairValorDaLinha(27, 30).BoletoBrToInt();
            objRetornar.CodigoDoBeneficiario = linha.ExtrairValorDaLinha(31, 39);
            

            objRetornar.NomeDoBeneficiario = linha.ExtrairValorDaLinha(47, 76);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(77, 79);
            objRetornar.NomeDoBanco = linha.ExtrairValorDaLinha(80, 87);

            objRetornar.DataGeracaoGravacao =(DateTime) linha.ExtrairValorDaLinha(95, 100).ToString().ToDateTimeFromDdMmAa();
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
            objRetornar.CodigoDoBeneficiario = linha.ExtrairValorDaLinha(22, 30).BoletoBrToInt();
            /*
             * 1 – Cobrança Simples (8050.76)
             * 3 – Cobrança Caucionada (8150.55) Reservado
             * 4 – Cobrança em IGPM (8450.94) *
             * 5 – Cobrança Caucionada CGB Especial (8355.01) Reservado
             * 6 – Cobrança Simples Seguradora (8051.57)
             * 7 – Cobrança em UFIR (8257.86) *
             * 8 – Cobrança em IDTR (8356.84) *
             */
            objRetornar.SeuNumero = linha.ExtrairValorDaLinha(38, 62);
            //objRetornar.NossoNumero = linha.ExtrairValorDaLinha(63, 72);
            //objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(73, 82);
            objRetornar.TipoCobranca = linha.ExtrairValorDaLinha(108, 108).BoletoBrToInt();
            objRetornar.CodigoDeOcorrencia = linha.ExtrairValorDaLinha(109, 110).BoletoBrToStringSafe().BoletoBrToInt();
            objRetornar.DataDaOcorrencia = (DateTime) linha.ExtrairValorDaLinha(111, 116).BoletoBrToStringSafe().ToDateTimeFromDdMmAa();

            objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(117, 126);
            objRetornar.NossoNumero = linha.ExtrairValorDaLinha(127, 134);
            // Brancos
            objRetornar.DataDeVencimento = linha.ExtrairValorDaLinha(147, 152).BoletoBrToStringSafe().ToDateTimeFromDdMmAa() ?? DateTime.MinValue;
            objRetornar.ValorDoTituloParcela = linha.ExtrairValorDaLinha(153, 165).BoletoBrToDecimal()/100;

            objRetornar.BancoCobrador = linha.ExtrairValorDaLinha(166, 168).BoletoBrToInt();
            objRetornar.AgenciaCobradora = linha.ExtrairValorDaLinha(169, 172).BoletoBrToInt();
            objRetornar.DvAgenciaCobradora = linha.ExtrairValorDaLinha(173, 173);
            objRetornar.Especie = linha.ExtrairValorDaLinha(174, 175);
            objRetornar.ValorTarifa = linha.ExtrairValorDaLinha(176, 188).BoletoBrToDecimal()/100;
            objRetornar.ValorAbatimento = linha.ExtrairValorDaLinha(228, 240).BoletoBrToDecimal()/100;
            objRetornar.ValorDesconto = linha.ExtrairValorDaLinha(241, 253).BoletoBrToDecimal()/100;
            objRetornar.ValorPrincipal = linha.ExtrairValorDaLinha(254, 266).BoletoBrToDecimal()/100;
            objRetornar.ValorLiquidoRecebido = linha.ExtrairValorDaLinha(254, 266).BoletoBrToDecimal()/100;
            objRetornar.ValorLiquidoRecebido += objRetornar.ValorTarifa;
            objRetornar.ValorJurosDeMora = linha.ExtrairValorDaLinha(267, 279).BoletoBrToDecimal()/100;
            objRetornar.ValorOutrosCreditos = linha.ExtrairValorDaLinha(280, 292).BoletoBrToDecimal()/100;
            objRetornar.DataDeCredito =
                linha.ExtrairValorDaLinha(296, 301).BoletoBrToStringSafe().ToDateTimeFromDdMmAa().Equals(null)
                    ? new DateTime(0001, 01, 01)
                    : (DateTime) linha.ExtrairValorDaLinha(296, 301).ToString().ToDateTimeFromDdMmAa();
            objRetornar.MotivoDaOcorrencia = linha.ExtrairValorDaLinha(383, 392).BoletoBrToInt();
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();

            return objRetornar;
        }

        public TrailerRetornoCnab400 ObterTrailer(string linha)
        {
            var objRetornar = new TrailerRetornoCnab400();

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
            objRetornar.QtdTitulosCobrancaSimples = linha.ExtrairValorDaLinha(18, 25).BoletoBrToLong();
            objRetornar.ValorTitulosCobrancaSimples = linha.ExtrairValorDaLinha(26, 39).BoletoBrToDecimal()/100;
            objRetornar.ReferenciaAvisoBancario1 = linha.ExtrairValorDaLinha(40, 47);
            objRetornar.QtdTitulosCobrancaVinculada = linha.ExtrairValorDaLinha(49, 55).BoletoBrToLong();
            objRetornar.ValorTitulosCobrancaVinculada = linha.ExtrairValorDaLinha(56, 70).BoletoBrToDecimal()/100;
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();

            return objRetornar;
        }
    }
}
