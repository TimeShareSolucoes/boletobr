using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.CNAB400.Retorno;

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
            HeaderRetornoCnab400 objRetornar = new HeaderRetornoCnab400();

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).ToInt();
            objRetornar.TipoRetorno = linha.ExtrairValorDaLinha(2, 2);
            objRetornar.LiteralRetorno = linha.ExtrairValorDaLinha(3, 9);
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(10, 11);
            objRetornar.LiteralServico = linha.ExtrairValorDaLinha(12, 26);
            objRetornar.CodigoAgenciaCedente = linha.ExtrairValorDaLinha(27, 30).ToInt();
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(33, 37).ToInt();
            objRetornar.DacAgenciaConta = linha.ExtrairValorDaLinha(38, 38).ToInt();
            objRetornar.NomeDoBeneficiario = linha.ExtrairValorDaLinha(47, 76);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(77, 79);
            objRetornar.NomeDoBanco = linha.ExtrairValorDaLinha(80, 94);
            objRetornar.DataGeracaoGravacao = linha.ExtrairValorDaLinha(95, 100).ToInt();
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400);

            return objRetornar;
        }

        public DetalheRetornoCnab400 ObterRegistrosDetalhe(string linha)
        {
            DetalheRetornoCnab400 objRetornar = new DetalheRetornoCnab400();

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).ToInt();
            objRetornar.CodigoLayout = linha.ExtrairValorDaLinha(2, 2).ToInt();
            objRetornar.CodigoAgenciaCedente = linha.ExtrairValorDaLinha(3, 6).ToInt();
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(9, 13).ToInt();
            objRetornar.DacAgenciaConta = linha.ExtrairValorDaLinha(14, 14).ToInt();
            objRetornar.Carteira = linha.ExtrairValorDaLinha(15, 17).ToInt();
            objRetornar.NossoNumero = linha.ExtrairValorDaLinha(18, 25);
            objRetornar.DacNossoNumero = linha.ExtrairValorDaLinha(26, 26).ToInt();
            objRetornar.Moeda = linha.ExtrairValorDaLinha(27, 27).ToInt();
            objRetornar.LiteralMoeda = linha.ExtrairValorDaLinha(28, 31);
            objRetornar.ValorDoTituloParcela = linha.ExtrairValorDaLinha(32, 44).ToDecimal()/100;
            objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(45, 54);
            objRetornar.DataDeVencimento = linha.ExtrairValorDaLinha(55, 60).ToInt();
            objRetornar.Especie = linha.ExtrairValorDaLinha(61, 62);
            objRetornar.Aceite = linha.ExtrairValorDaLinha(63, 63);
            objRetornar.DataEmissao = linha.ExtrairValorDaLinha(64, 69).ToInt();
            objRetornar.TipoInscricaoSacado = linha.ExtrairValorDaLinha(70, 71).ToInt();
            objRetornar.NumeroInscricaoSacado = linha.ExtrairValorDaLinha(72, 86).ToLong();
            objRetornar.NomeSacado = linha.ExtrairValorDaLinha(87, 116);
            objRetornar.LogradouroSacado = linha.ExtrairValorDaLinha(126, 165);
            objRetornar.BairroSacado = linha.ExtrairValorDaLinha(166, 177);
            objRetornar.CepSacado = linha.ExtrairValorDaLinha(178, 185);
            objRetornar.CidadeSacado = linha.ExtrairValorDaLinha(186, 200);
            objRetornar.EstadoSacado = linha.ExtrairValorDaLinha(201, 202);
            objRetornar.SacadorAvalista = linha.ExtrairValorDaLinha(203, 232);
            objRetornar.LocalPagamento1 = linha.ExtrairValorDaLinha(237, 291);
            objRetornar.LocalPagamento2 = linha.ExtrairValorDaLinha(292, 346);
            objRetornar.TipoInscricaoSacadorAvalista = linha.ExtrairValorDaLinha(347, 348).ToInt();
            objRetornar.NumeroInscricaoSacadorAvalista = linha.ExtrairValorDaLinha(349, 363).ToLong();
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).ToInt();

            return objRetornar;
        }

        public DetalheRateioRetornoCnab400 ObterRegistrosDetalheRateio(string linha)
        {
            throw new NotImplementedException();
        }

        public TrailerRetornoCnab400 ObterTrailer(string linha)
        {
            TrailerRetornoCnab400 objRetornar = new TrailerRetornoCnab400();

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1,1).ToInt();
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).ToInt();

            return objRetornar;
        }
    }
}
