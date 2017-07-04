using BoletoBr.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Sicoob
{
    public class LeitorRetornoCnab400Sicoob : ILeitorArquivoRetornoCnab400
    {
        private readonly List<string> _linhasArquivo;

        public LeitorRetornoCnab400Sicoob(List<string> linhasArquivo)
        {
            _linhasArquivo = linhasArquivo;
        }

        public RetornoCnab400 ProcessarRetorno(Dominio.TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
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
                    objRetornar.Header = ObterHeader(linhaAtual);
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "1")
                {
                    var objDetalhe = ObterRegistrosDetalhe(linhaAtual);
                    objRetornar.RegistrosDetalhe.Add(objDetalhe);
                }
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
            var objRetornar = new HeaderRetornoCnab400();

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
            objRetornar.TipoRetorno = linha.ExtrairValorDaLinha(2, 2);
            objRetornar.LiteralRetorno = linha.ExtrairValorDaLinha(3, 9);
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(10, 11);
            objRetornar.LiteralServico = linha.ExtrairValorDaLinha(12, 19);
            //20-26 BRANCOS
            objRetornar.CodigoAgenciaCedente = linha.ExtrairValorDaLinha(27, 30).BoletoBrToInt();
            objRetornar.DvAgenciaCedente = linha.ExtrairValorDaLinha(31, 31).BoletoBrToStringSafe();
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(32, 39).BoletoBrToStringSafe();
            objRetornar.DvContaCorrente = linha.ExtrairValorDaLinha(39, 39).BoletoBrToStringSafe();
            //41-46 BRANCOS
            objRetornar.NomeDoBeneficiario = linha.ExtrairValorDaLinha(47, 76);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(77, 79);
            objRetornar.NomeDoBanco = linha.ExtrairValorDaLinha(80, 94);
            objRetornar.DataGeracaoGravacao =
                (DateTime)linha.ExtrairValorDaLinha(95, 100).ToString().ToDateTimeFromDdMmAa();
            objRetornar.SequencialRetorno = linha.ExtrairValorDaLinha(101, 107);
            //108-394 BRANCOS
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
            objRetornar.DvAgenciaCedente = linha.ExtrairValorDaLinha(22, 22);
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(23, 30);
            objRetornar.DacAgenciaConta = linha.ExtrairValorDaLinha(31, 31).BoletoBrToInt();
            objRetornar.NumeroConvenio = linha.ExtrairValorDaLinha(32, 37).BoletoBrToInt();
            objRetornar.NumeroControle = linha.ExtrairValorDaLinha(38, 62);

            objRetornar.NossoNumero = linha.ExtrairValorDaLinha(63, 73);
            objRetornar.DacNossoNumero = linha.ExtrairValorDaLinha(74, 74).BoletoBrToInt();
            objRetornar.NumeroParcela = linha.ExtrairValorDaLinha(75, 76).BoletoBrToInt();

            //77-80 GRUPO DE VALOR 00
            objRetornar.CodigoBaixaRecusa = linha.ExtrairValorDaLinha(81, 82).BoletoBrToInt();
            //objRetornar.Especie = linha.ExtrairValorDaLinha(83, 85);
            objRetornar.VariacaoCarteira = linha.ExtrairValorDaLinha(86, 88).BoletoBrToInt();
            objRetornar.ContaCaucao = linha.ExtrairValorDaLinha(89, 89).BoletoBrToInt();
            objRetornar.CodigoDeResponsabilidade = linha.ExtrairValorDaLinha(90, 94).BoletoBrToInt();
            objRetornar.DVCodigoDeResponsabilidade = linha.ExtrairValorDaLinha(95, 95).BoletoBrToInt();

            objRetornar.ValorTarifa = linha.ExtrairValorDaLinha(96, 100).BoletoBrToDecimal() / 100;
            objRetornar.ValorIof = linha.ExtrairValorDaLinha(101, 105).BoletoBrToDecimal() / 100;

            //106-106 BRANCOS
            objRetornar.CodigoCarteira = linha.ExtrairValorDaLinha(107, 108);
            objRetornar.CodigoDeOcorrencia = linha.ExtrairValorDaLinha(109, 110).BoletoBrToInt();
            objRetornar.DataDaOcorrencia = (DateTime)linha.ExtrairValorDaLinha(111, 116).BoletoBrToStringSafe().ToDateTimeFromDdMmAa();
            objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(117, 126);

            //127-146 BRANCOS
            objRetornar.DataDeVencimento =
                linha.ExtrairValorDaLinha(147, 152).BoletoBrToStringSafe().ToDateTimeFromDdMmAa() ?? DateTime.MinValue;
            objRetornar.ValorDoTituloParcela = linha.ExtrairValorDaLinha(153, 165).BoletoBrToDecimal() / 100;

            objRetornar.BancoCobrador = linha.ExtrairValorDaLinha(166, 168).BoletoBrToInt();
            objRetornar.AgenciaCobradora = linha.ExtrairValorDaLinha(169, 172).BoletoBrToInt();
            objRetornar.DvAgenciaCobradora = linha.ExtrairValorDaLinha(173, 173);

            objRetornar.Especie = linha.ExtrairValorDaLinha(174, 175);
            objRetornar.DataDeCredito =
                linha.ExtrairValorDaLinha(176, 181).BoletoBrToStringSafe().ToDateTimeFromDdMmAa().Equals(null)
                    ? new DateTime(0001, 01, 01)
                    : (DateTime)linha.ExtrairValorDaLinha(176, 181).ToString().ToDateTimeFromDdMmAa();
            objRetornar.ValorTarifa = linha.ExtrairValorDaLinha(182, 188).BoletoBrToDecimal() / 100;
            objRetornar.ValorOutrasDespesas = linha.ExtrairValorDaLinha(189, 201).BoletoBrToDecimal() / 100;
            objRetornar.ValorJurosDesconto = linha.ExtrairValorDaLinha(202, 214).BoletoBrToDecimal() / 100;
            objRetornar.ValorIofDesconto = linha.ExtrairValorDaLinha(215, 227).BoletoBrToDecimal() / 100;
            objRetornar.ValorAbatimento = linha.ExtrairValorDaLinha(228, 240).BoletoBrToDecimal() / 100;
            objRetornar.ValorDesconto = linha.ExtrairValorDaLinha(241, 253).BoletoBrToDecimal() / 100;
            objRetornar.ValorLiquidoRecebido = linha.ExtrairValorDaLinha(254, 266).BoletoBrToDecimal() / 100;
            objRetornar.ValorJurosDeMora = linha.ExtrairValorDaLinha(267, 279).BoletoBrToDecimal() / 100;
            objRetornar.ValorOutrosRecebimentos = linha.ExtrairValorDaLinha(280, 292).BoletoBrToDecimal() / 100;
            objRetornar.ValorAbatimentosNaoAproveitado = linha.ExtrairValorDaLinha(293, 305).BoletoBrToDecimal() / 100;
            objRetornar.ValorLancamento = linha.ExtrairValorDaLinha(306, 318).BoletoBrToDecimal() / 100;
            objRetornar.IndicativoDebitoCredito = linha.ExtrairValorDaLinha(319, 319);
            objRetornar.IndicadorValor = linha.ExtrairValorDaLinha(320, 320).BoletoBrToInt();
            objRetornar.ValorAjuste = linha.ExtrairValorDaLinha(321, 332).BoletoBrToDecimal() / 100;

            //333-342 BRANCOS
            objRetornar.NumeroInscricao = linha.ExtrairValorDaLinha(343, 357).BoletoBrToLong();

            //358-394 BRANCOS
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();

            return objRetornar;
        }

        public TrailerRetornoCnab400 ObterTrailer(string linha)
        {
            var objRetornar = new TrailerRetornoCnab400();

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(2, 3);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(4, 6);
            objRetornar.CodigoCooperativa = linha.ExtrairValorDaLinha(7, 10);
            objRetornar.SiglaCooperativa = linha.ExtrairValorDaLinha(11, 35);
            objRetornar.EnderecoCooperativa = linha.ExtrairValorDaLinha(36, 85);
            objRetornar.BairroCooperativa = linha.ExtrairValorDaLinha(86, 115);
            objRetornar.CEPCooperativa = linha.ExtrairValorDaLinha(116, 123);
            objRetornar.CidadeCooperativa = linha.ExtrairValorDaLinha(124, 153);
            objRetornar.UFCooperativa = linha.ExtrairValorDaLinha(154, 155);
            objRetornar.DataMovimento = linha.ExtrairValorDaLinha(156, 163).ToDateTimeFromDdMmAaaa();
            objRetornar.QtdRegistrosDetalhe = linha.ExtrairValorDaLinha(164, 171).BoletoBrToInt();
            objRetornar.UltimoNossoNumero = linha.ExtrairValorDaLinha(172, 182);
            //183-394 BRANCOS
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();

            return objRetornar;
        }
    }
}
