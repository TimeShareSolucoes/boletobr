using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.CNAB400.Retorno;
using BoletoBr.Dominio;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos
{
    public class LeitorRetornoCnab400Hsbc : ILeitorArquivoRetornoCnab400
    {
        private readonly List<string> _linhasArquivo;

        public LeitorRetornoCnab400Hsbc(List<string> linhasArquivo)
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
                    objRetornar.Header = ObterHeader(linhaAtual);
                else if (linhaAtual.ExtrairValorDaLinha(1, 1) == "1")
                {
                    var objDetalhe = ObterRegistrosDetalhe(linhaAtual);
                    objRetornar.RegistrosDetalhe.Add(objDetalhe);
                }
                else if (linhaAtual.ExtrairValorDaLinha(1, 1) == "9")
                    objRetornar.Trailer = ObterTrailer(linhaAtual);
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

        public HeaderRetornoCnab400 ObterHeader(string linhaObterInformacoes)
        {
            var objRetornar = new HeaderRetornoCnab400();

            var linha = linhaObterInformacoes;

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
            objRetornar.CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).BoletoBrToInt();
            objRetornar.LiteralRetorno = linha.ExtrairValorDaLinha(3, 9);
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(10, 11);
            objRetornar.LiteralServico = linha.ExtrairValorDaLinha(12, 26);
            objRetornar.CodigoAgenciaCedente = linha.ExtrairValorDaLinha(27, 31).BoletoBrToInt();
            objRetornar.Constante = linha.ExtrairValorDaLinha(32, 33);
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(34, 44);
            objRetornar.TipoRetorno = linha.ExtrairValorDaLinha(45, 45);
            // Posição 46 branco
            objRetornar.NomeDoBeneficiario = linha.ExtrairValorDaLinha(47, 76);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(77, 79);
            objRetornar.NomeDoBanco = linha.ExtrairValorDaLinha(80, 94);
            objRetornar.DataGeracaoGravacao = (DateTime) linha.ExtrairValorDaLinha(95, 100).ToString().ToDateTimeFromDdMmAa();
            objRetornar.Densidade = linha.ExtrairValorDaLinha(101, 105);
            objRetornar.LiteralDensidade = linha.ExtrairValorDaLinha(106, 108);
            objRetornar.CodigoDoBeneficiario = linha.ExtrairValorDaLinha(109, 118);
            objRetornar.NomeAgencia = linha.ExtrairValorDaLinha(119, 138);
            objRetornar.CodigoFormulario = linha.ExtrairValorDaLinha(139, 142).BoletoBrToInt();
            // Posição 143 - 388 brancos
            objRetornar.Volser = linha.ExtrairValorDaLinha(389, 394);
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400);

            return objRetornar;
        }

        public DetalheRetornoCnab400 ObterRegistrosDetalhe(string linhaProcessar)
        {
            var objRetornar = new DetalheRetornoCnab400();

            var linha = linhaProcessar;

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
            objRetornar.CodigoDeInscricao = linha.ExtrairValorDaLinha(2, 3).BoletoBrToInt();
            objRetornar.CodigoDoBeneficiario = linha.ExtrairValorDaLinha(4, 17).BoletoBrToInt();
            // Posição 18-18 "0"
            objRetornar.CodigoAgenciaCedente = linha.ExtrairValorDaLinha(19, 22).BoletoBrToInt();
            objRetornar.SubConta = linha.ExtrairValorDaLinha(23, 24).BoletoBrToInt();
            objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(25, 35);
            // Posição 36-36 Origem do pagamento
            // Posição 37-37 brancos
            objRetornar.SeuNumero = linha.ExtrairValorDaLinha(38, 62);
            objRetornar.NossoNumero = linha.ExtrairValorDaLinha(63, 73);
            //Data limite para desconto-2 074 079
            //Valor do desconto-2 a conceder 080 090
            //Data limite para desconto-3 091 096
            //Valor do desconto-3 a conceder 097 107
            objRetornar.CodigoCarteira = linha.ExtrairValorDaLinha(108, 108);
            objRetornar.CodigoDeOcorrencia = linha.ExtrairValorDaLinha(109, 110).BoletoBrToInt();
            objRetornar.DataDaOcorrencia = (DateTime)linha.ExtrairValorDaLinha(111, 116).ToString().ToDateTimeFromDdMmAa();
            objRetornar.DataDeCredito = (DateTime)linha.ExtrairValorDaLinha(111, 116).ToString().ToDateTimeFromDdMmAa(); /* Data de ocorrencia a mesma de crédito pois não possui data para crédito */
            objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(117, 126).BoletoBrToStringSafe();
            if (objRetornar.NossoNumero.BoletoBrToStringSafe().BoletoBrToInt() == 0)
                objRetornar.NossoNumero = linha.ExtrairValorDaLinha(127, 137); //Número atribuído pelo Banco
            //Uso do Banco 138 146
            if (linha.ExtrairValorDaLinha(147, 152).ToString().ToDateTimeFromDdMmAa() != null)
                objRetornar.DataDeVencimento = (DateTime)linha.ExtrairValorDaLinha(147, 152).ToString().ToDateTimeFromDdMmAa();

            objRetornar.ValorDoTituloParcela = linha.ExtrairValorDaLinha(153, 165).BoletoBrToDecimal() / 100;
            objRetornar.BancoCobrador = linha.ExtrairValorDaLinha(166, 168).BoletoBrToStringSafe().BoletoBrToInt();
            objRetornar.AgenciaCobradora = linha.ExtrairValorDaLinha(169, 173).BoletoBrToStringSafe().BoletoBrToInt();
            objRetornar.Especie = linha.ExtrairValorDaLinha(174, 175);
            objRetornar.ValorIof = linha.ExtrairValorDaLinha(176, 186).BoletoBrToDecimal() / 100;
            //Uso do Banco 189 227
            objRetornar.ValorAbatimento = linha.ExtrairValorDaLinha(228, 240).BoletoBrToDecimal() / 100;
            objRetornar.ValorDesconto = linha.ExtrairValorDaLinha(241, 253).BoletoBrToDecimal() / 100;
            objRetornar.ValorLiquidoRecebido = linha.ExtrairValorDaLinha(254, 266).BoletoBrToDecimal() / 100;
            objRetornar.ValorJurosDeMora = linha.ExtrairValorDaLinha(267, 279).BoletoBrToDecimal() / 100;
            //Uso do Banco 280 301
            //Complemento da Ocorrência 302 303
            //Indicativo de crédito 304 304
            //Uso do Banco 305 388
            //Número sequencial do
            //Aviso de
            //Movimentação
            //da Cobrança 389 393
            //Tipo de moeda 394 394
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
            // Brancos
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();
            
            return objRetornar;
        }

        public static string PrimeiroCaracter(string retorno)
        {
            try
            {
                return retorno.ExtrairValorDaLinha(1, 1);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao desmembrar registro.", ex);
            }
        }
    }
}
