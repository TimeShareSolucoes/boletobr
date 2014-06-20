using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos
{
    interface ILeitorArquivoRetornoCnab400
    {
        private readonly List<string> _linhasArquivo;
        RetornoCnab400 ProcessarRetorno()
        {
            /* Validações */
            #region Validações
            if (_linhasArquivo == null)
                throw new Exception("Dados do arquivo de retorno estão nulos. Impossível processar.");

            if (_linhasArquivo.Count <= 0)
                throw new Exception("Dados do arquivo de retorno estão nulos. Impossível processar.");

            if (_linhasArquivo.Count < 3)
                throw new Exception("Dados do arquivo de retorno não contém o mínimo de 3 linhas. Impossível processar.");
            #endregion

            var objRetornar = new RetornoCnab400();

            /* Processar Header */
            objRetornar.Header = ObterHeader();

            /* Processar Detalhe */
            objRetornar.RegistrosDetalhe = ObterRegistrosDetalhe();

            /* Processar Trailer */
            objRetornar.Trailer = ObterTrailer();

            return objRetornar;
        }
        HeaderRetornoCnab400 ObterHeader()
        {
            var objRetornar = new HeaderRetornoCnab400();
            /* Obter 1ª linha */
            var linha = _linhasArquivo[0];

            objRetornar.TipoDeRegistro = linha.ExtrairValorDaLinha(1, 1).ToInt();
            objRetornar.CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).ToInt();
            objRetornar.LiteralDeRemessa = linha.ExtrairValorDaLinha(3, 9);
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(10, 11).ToInt();
            objRetornar.LiteralDeServico = linha.ExtrairValorDaLinha(12, 26);
            objRetornar.Zeros = linha.ExtrairValorDaLinha(27, 27).ToInt();
            objRetornar.Agencia = linha.ExtrairValorDaLinha(28, 31).ToInt();
            objRetornar.SubConta = linha.ExtrairValorDaLinha(32, 33).ToInt();
            objRetornar.Conta = linha.ExtrairValorDaLinha(34, 44).ToInt();
            objRetornar.BancoTamanho2 = linha.ExtrairValorDaLinha(45, 46).ToInt();
            objRetornar.NomeCliente = linha.ExtrairValorDaLinha(47, 76);
            objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(77, 79).ToInt();
            objRetornar.NomeDoBanco = linha.ExtrairValorDaLinha(80, 94);
            objRetornar.DataDeGeracao = linha.ExtrairValorDaLinha(95, 100).ToInt();
            objRetornar.Densidade = linha.ExtrairValorDaLinha(101, 105).ToInt();
            objRetornar.LiteralDensidade = linha.ExtrairValorDaLinha(106, 108);
            objRetornar.BancoTamanho11 = linha.ExtrairValorDaLinha(109, 119).ToInt();
            objRetornar.DataDoCredito = linha.ExtrairValorDaLinha(120, 125).ToInt();
            objRetornar.BancoTamanho263 = linha.ExtrairValorDaLinha(126, 388).ToInt();
            objRetornar.SequencialDoArquivo = linha.ExtrairValorDaLinha(389, 393).ToInt();
            objRetornar.BancoTamanho1 = linha.ExtrairValorDaLinha(394, 394).ToInt();
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).ToInt();

            return objRetornar;
        }

        List<DetalheRetornoCnab400> ObterRegistrosDetalhe();

        TrailerRetornoCnab400 ObterTrailer()
        {
            var objRetornar = new TrailerRetornoCnab400();
            /* Obter última linha */
            var linha = _linhasArquivo.Last();

            objRetornar.TipoDeRegistro = linha.ExtrairValorDaLinha(1, 1).ToInt();
            objRetornar.CodigoArquivo = linha.ExtrairValorDaLinha(2, 2).ToInt();
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(3, 4).ToInt();
            objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(5, 7).ToInt();
            objRetornar.BancoTamanho10 = linha.ExtrairValorDaLinha(8, 17).ToInt();
            objRetornar.QuantidaDeTitulos = linha.ExtrairValorDaLinha(18, 25).ToInt();
            objRetornar.ValorDosTitulos = linha.ExtrairValorDaLinha(26, 39).ToInt();
            objRetornar.BancoTamanho355 = linha.ExtrairValorDaLinha(40, 394).ToInt();
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).ToInt();

            return objRetornar;
        }
    }
}
