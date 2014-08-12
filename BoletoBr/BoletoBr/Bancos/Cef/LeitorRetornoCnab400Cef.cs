using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.CNAB400.Retorno;

namespace BoletoBr.Bancos.Cef
{
    public class LeitorRetornoCnab400Cef : ILeitorArquivoRetornoCnab400
    {
       private readonly List<string> _linhasArquivo;

       public LeitorRetornoCnab400Cef(List<string> linhasArquivo)
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

        public HeaderRetornoCnab400 ObterHeader(string linhaObterInformacoes)
        {
            var objRetornar = new HeaderRetornoCnab400();

            var linha = linhaObterInformacoes;

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
            objRetornar.CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).BoletoBrToInt();
            objRetornar.LiteralRetorno = linha.ExtrairValorDaLinha(3, 9);
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(10, 11);
            objRetornar.LiteralServico = linha.ExtrairValorDaLinha(12, 26);
            objRetornar.CodigoAgenciaCedente = linha.ExtrairValorDaLinha(27, 30).BoletoBrToInt();
            objRetornar.CodigoDoBeneficiario = linha.ExtrairValorDaLinha(31, 36).BoletoBrToInt();
            // Uso Exclusivo CAIXA (37 - 46)
            objRetornar.NomeDoBeneficiario = linha.ExtrairValorDaLinha(47, 76);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(77, 79);
            objRetornar.NomeDoBanco = linha.ExtrairValorDaLinha(80, 94);
            objRetornar.DataGeracaoGravacao = Convert.ToInt32(linha.ExtrairValorDaLinha(95, 100));
            objRetornar.MensagemRetorno = linha.ExtrairValorDaLinha(101, 158);
            // Uso Exclusivo CAIXA (159 - 389)
            objRetornar.NumeroSequencialA = linha.ExtrairValorDaLinha(390, 394);
            objRetornar.NumeroSequencialB = linha.ExtrairValorDaLinha(395, 400);

            return objRetornar;
        }

        public DetalheRetornoCnab400 ObterRegistrosDetalhe(string linhaProcessar)
        {
            var objRetornar = new DetalheRetornoCnab400();
            
            var linha = linhaProcessar;

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
            objRetornar.CodigoDeInscricao = linha.ExtrairValorDaLinha(2, 3).BoletoBrToInt();
            objRetornar.CodigoDoBeneficiario = linha.ExtrairValorDaLinha(4, 17).BoletoBrToInt();
            objRetornar.CodigoAgenciaCedente = linha.ExtrairValorDaLinha(18, 21).BoletoBrToInt();
            objRetornar.CodigoDoBeneficiario = linha.ExtrairValorDaLinha(22, 27).BoletoBrToInt();
            objRetornar.IdEmissao = linha.ExtrairValorDaLinha(28, 28).BoletoBrToInt();
            objRetornar.IdPostagem = linha.ExtrairValorDaLinha(29, 29).BoletoBrToInt();
            // Uso Exclusivo CAIXA (30 - 31)
            objRetornar.UsoDaEmpresa = linha.ExtrairValorDaLinha(32, 56);
            objRetornar.ModalidadeNossoNumero = linha.ExtrairValorDaLinha(57, 58).BoletoBrToInt();
            objRetornar.NossoNumero = linha.ExtrairValorDaLinha(59, 73);
            objRetornar.CodigoRejeicao = linha.ExtrairValorDaLinha(80, 82).BoletoBrToInt();
            objRetornar.CodigoCarteira = linha.ExtrairValorDaLinha(107, 108);
            objRetornar.CodigoDeOcorrencia = linha.ExtrairValorDaLinha(109, 110).BoletoBrToInt();
            objRetornar.DataDaOcorrencia = linha.ExtrairValorDaLinha(111, 116).BoletoBrToInt();
            objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(117, 126);
            // Uso Exclusivo CAIXA (127 - 146)
            objRetornar.DataDeVencimento = linha.ExtrairValorDaLinha(147, 152).BoletoBrToInt();
            objRetornar.ValorDoTituloParcela = linha.ExtrairValorDaLinha(153, 165).BoletoBrToDecimal()/100;
            objRetornar.BancoCobrador = linha.ExtrairValorDaLinha(166, 168).BoletoBrToInt();
            objRetornar.AgenciaCobradora = linha.ExtrairValorDaLinha(169, 173).BoletoBrToInt();
            objRetornar.Especie = linha.ExtrairValorDaLinha(174, 175);
            objRetornar.ValorTarifa = linha.ExtrairValorDaLinha(176, 188).BoletoBrToDecimal()/100;
            objRetornar.CodigoBaixaTitulo = linha.ExtrairValorDaLinha(189, 191).BoletoBrToInt();
            objRetornar.CodigoFormaPagamento = linha.ExtrairValorDaLinha(192, 192);
            objRetornar.FloatNegociado = linha.ExtrairValorDaLinha(193, 194).BoletoBrToInt();
            objRetornar.DataDebitoTarifa = linha.ExtrairValorDaLinha(195, 200).BoletoBrToInt();
            // Uso Exclusivo CAIXA (201 - 214)
            objRetornar.ValorIof = linha.ExtrairValorDaLinha(215, 227).BoletoBrToDecimal()/100;
            objRetornar.ValorAbatimento = linha.ExtrairValorDaLinha(228, 240).BoletoBrToDecimal()/100;
            objRetornar.ValorDesconto = linha.ExtrairValorDaLinha(241, 253).BoletoBrToDecimal()/100;
            objRetornar.ValorPrincipal = linha.ExtrairValorDaLinha(254, 266).BoletoBrToDecimal()/100;
            objRetornar.ValorJurosDeMora = linha.ExtrairValorDaLinha(267, 279).BoletoBrToDecimal()/100;
            objRetornar.ValorMulta = linha.ExtrairValorDaLinha(280, 292).BoletoBrToDecimal()/100;
            objRetornar.Moeda = linha.ExtrairValorDaLinha(293, 293).BoletoBrToInt();
            objRetornar.DataDeCredito = linha.ExtrairValorDaLinha(294, 299).BoletoBrToInt();
            // Uso Exclusivo CAIXA (300 - 394)
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();

            return objRetornar;
        }

        public TrailerRetornoCnab400 ObterTrailer(string linhaObterInformacoes)
        {
            var objRetornar = new TrailerRetornoCnab400();

            var linha = linhaObterInformacoes;

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
            objRetornar.CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).BoletoBrToInt();
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(3, 4);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(5, 7);
            // Uso Exclusivo CAIXA (8 - 394)
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();

            return objRetornar;
        }
    }
}
