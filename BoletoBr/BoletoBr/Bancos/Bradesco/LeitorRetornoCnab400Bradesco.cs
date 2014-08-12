using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BoletoBr.Arquivo.CNAB400.Retorno;
using BoletoBr.Dominio;

namespace BoletoBr.Bancos.Bradesco
{
    public class LeitorRetornoCnab400Bradesco : ILeitorArquivoRetornoCnab400
    {
        private readonly List<string> _linhasArquivo;

        public LeitorRetornoCnab400Bradesco(List<string> linhasArquivo)
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
            HeaderRetornoCnab400 objRetornar = new HeaderRetornoCnab400();

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
            objRetornar.CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).BoletoBrToInt();
            objRetornar.LiteralRetorno = linha.ExtrairValorDaLinha(3, 9);
            objRetornar.CodigoDoServico = linha.ExtrairValorDaLinha(10, 11);
            objRetornar.LiteralServico = linha.ExtrairValorDaLinha(12, 26);
            objRetornar.CodigoDoBeneficiario = linha.ExtrairValorDaLinha(27, 46).BoletoBrToLong();
            objRetornar.NomeDoBeneficiario = linha.ExtrairValorDaLinha(47, 76);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(77, 79);
            objRetornar.NomeDoBanco = linha.ExtrairValorDaLinha(80, 94);
            objRetornar.DataGeracaoGravacao = linha.ExtrairValorDaLinha(95, 100).BoletoBrToInt();
            objRetornar.NumeroAvisoBancario = linha.ExtrairValorDaLinha(109, 113).BoletoBrToInt();
            objRetornar.DataCredito = linha.ExtrairValorDaLinha(380, 385).BoletoBrToInt();
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400);

            return objRetornar;
        }

        public DetalheRetornoCnab400 ObterRegistrosDetalhe(string linha)
        {
            DetalheRetornoCnab400 objRetornar = new DetalheRetornoCnab400();

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();

            if (objRetornar.CodigoDoRegistro == 1)
            {
                objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
                objRetornar.TipoInscricao = linha.ExtrairValorDaLinha(2, 3).BoletoBrToInt();
                objRetornar.NumeroInscricao = linha.ExtrairValorDaLinha(4, 17).BoletoBrToLong();
                objRetornar.IdentificacaoEmpresaNoBanco = linha.ExtrairValorDaLinha(21, 37);
                objRetornar.NumeroControle = linha.ExtrairValorDaLinha(38, 62);
                objRetornar.NossoNumero = linha.ExtrairValorDaLinha(71, 82);
                objRetornar.IndicadorRateioCredito = linha.ExtrairValorDaLinha(105, 105);
                objRetornar.CodigoCarteira = linha.ExtrairValorDaLinha(108, 108);
                objRetornar.CodigoDeOcorrencia = linha.ExtrairValorDaLinha(109, 110).BoletoBrToInt();
                objRetornar.DataDaOcorrencia = linha.ExtrairValorDaLinha(111, 116).BoletoBrToInt();
                objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(117, 126);
                // 127 a 146 - Identificação do Título no Banco (Nosso Número)
                // Mesmo nosso número informado nas posições 71 a 82 do registro de transação.
                objRetornar.NossoNumero = linha.ExtrairValorDaLinha(127, 146);
                objRetornar.DataDeVencimento = linha.ExtrairValorDaLinha(147, 152).BoletoBrToInt();
                objRetornar.ValorDoTituloParcela = linha.ExtrairValorDaLinha(153, 165).BoletoBrToDecimal()/100;
                objRetornar.BancoCobrador = linha.ExtrairValorDaLinha(166, 168).BoletoBrToInt();
                objRetornar.AgenciaCobradora = linha.ExtrairValorDaLinha(169, 173).BoletoBrToInt();
                objRetornar.Especie = linha.ExtrairValorDaLinha(174, 175);
                objRetornar.ValorDespesas = linha.ExtrairValorDaLinha(176, 188).BoletoBrToDecimal()/100;
                objRetornar.ValorOutrasDespesas = linha.ExtrairValorDaLinha(189, 201).BoletoBrToDecimal()/100;
                objRetornar.ValorJurosAtraso = linha.ExtrairValorDaLinha(202, 214).BoletoBrToDecimal()/100;
                objRetornar.ValorIof = linha.ExtrairValorDaLinha(215, 227).BoletoBrToDecimal()/100;
                objRetornar.ValorAbatimento = linha.ExtrairValorDaLinha(228, 240).BoletoBrToDecimal()/100;
                objRetornar.ValorDesconto = linha.ExtrairValorDaLinha(241, 253).BoletoBrToDecimal()/100;
                objRetornar.ValorLiquidoRecebido = linha.ExtrairValorDaLinha(254, 266).BoletoBrToDecimal()/100;
                objRetornar.ValorJurosDeMora = linha.ExtrairValorDaLinha(267, 279).BoletoBrToDecimal()/100;
                objRetornar.ValorOutrosCreditos = linha.ExtrairValorDaLinha(280, 292).BoletoBrToDecimal()/100;
                objRetornar.MotivoCodigoOcorrencia = linha.ExtrairValorDaLinha(295, 295);
                objRetornar.DataDeCredito = linha.ExtrairValorDaLinha(296, 301).BoletoBrToInt();
                objRetornar.OrigemPagamento = linha.ExtrairValorDaLinha(302, 304).BoletoBrToInt();
                objRetornar.Cheque = linha.ExtrairValorDaLinha(315, 318);
                objRetornar.MotivoCodigoRejeicao = linha.ExtrairValorDaLinha(319, 328);
                objRetornar.NumeroCartorio = linha.ExtrairValorDaLinha(369, 370).BoletoBrToInt();
                objRetornar.NumeroProtocolo = linha.ExtrairValorDaLinha(371, 380);
                objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();
            }

            if (objRetornar.CodigoDoRegistro == 3)
                ObterRegistrosDetalheRateio(linha);

            return objRetornar;
        }


        /// <summary>
        /// Registro de Transação - TIPO 3 - Rateio de Crédito
        /// </summary>
        /// <param name="linha"></param>
        /// <returns></returns>
        public DetalheRateioRetornoCnab400 ObterRegistrosDetalheRateio(string linha)
        {
            DetalheRateioRetornoCnab400 objRetornar = new DetalheRateioRetornoCnab400();

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
            objRetornar.IdentificacaoEmpresaNoBanco = linha.ExtrairValorDaLinha(2, 17);
            objRetornar.NossoNumero = linha.ExtrairValorDaLinha(18, 29);
            objRetornar.CodigoCalculoRateio = linha.ExtrairValorDaLinha(30, 30).BoletoBrToInt();
            objRetornar.TipoValorInformado = linha.ExtrairValorDaLinha(31, 31).BoletoBrToInt();
            // Primeiro Beneficiário
            objRetornar.CodigoBancoPrimeiroBeneficiario = linha.ExtrairValorDaLinha(44, 46).BoletoBrToInt();
            objRetornar.CodigoAgenciaPrimeiroBeneficiario = linha.ExtrairValorDaLinha(47, 51).BoletoBrToInt();
            objRetornar.DvAgenciaPrimeiroBeneficiario = linha.ExtrairValorDaLinha(52, 52);
            objRetornar.ContaCorrentePrimeiroBeneficiario = linha.ExtrairValorDaLinha(53, 64).BoletoBrToLong();
            objRetornar.DvContaCorrentePrimeiroBeneficiario = linha.ExtrairValorDaLinha(65, 65);
            objRetornar.ValorRateioPrimeiroBeneficiario = linha.ExtrairValorDaLinha(66, 80).BoletoBrToDecimal()/100;
            objRetornar.NomePrimeiroBeneficiario = linha.ExtrairValorDaLinha(81, 120);
            objRetornar.ParcelaPrimeiroBeneficiario = linha.ExtrairValorDaLinha(142, 147);
            objRetornar.FloatingPrimeiroBeneficiario = linha.ExtrairValorDaLinha(148, 150).BoletoBrToInt();
            objRetornar.DataCreditoPrimeiroBeneficiario = linha.ExtrairValorDaLinha(151, 158).BoletoBrToInt();
            objRetornar.MotivoOcorrenciaPrimeiroBeneficiario = linha.ExtrairValorDaLinha(159, 160).BoletoBrToInt();
            // Segundo Beneficiário
            objRetornar.CodigoBancoSegundoBeneficiario = linha.ExtrairValorDaLinha(161, 163).BoletoBrToInt();
            objRetornar.CodigoAgenciaSegundoBeneficiario = linha.ExtrairValorDaLinha(164, 168).BoletoBrToInt();
            objRetornar.DvAgenciaSegundoBeneficiario = linha.ExtrairValorDaLinha(169, 169);
            objRetornar.ContaCorrenteSegundoBeneficiario = linha.ExtrairValorDaLinha(170, 181).BoletoBrToLong();
            objRetornar.DvContaCorrenteSegundoBeneficiario = linha.ExtrairValorDaLinha(182, 182);
            objRetornar.ValorRateioSegundoBeneficiario = linha.ExtrairValorDaLinha(183, 197).BoletoBrToDecimal() / 100;
            objRetornar.NomeSegundoBeneficiario = linha.ExtrairValorDaLinha(198, 237);
            objRetornar.ParcelaSegundoBeneficiario = linha.ExtrairValorDaLinha(259, 264);
            objRetornar.FloatingSegundoBeneficiario = linha.ExtrairValorDaLinha(265, 267).BoletoBrToInt();
            objRetornar.DataCreditoSegundoBeneficiario = linha.ExtrairValorDaLinha(268, 275).BoletoBrToInt();
            objRetornar.MotivoOcorrenciaSegundoBeneficiario = linha.ExtrairValorDaLinha(276, 277).BoletoBrToInt();
            // Terceiro Beneficiário
            objRetornar.CodigoBancoTerceiroBeneficiario = linha.ExtrairValorDaLinha(278, 280).BoletoBrToInt();
            objRetornar.CodigoAgenciaTerceiroBeneficiario = linha.ExtrairValorDaLinha(281, 285).BoletoBrToInt();
            objRetornar.DvAgenciaTerceiroBeneficiario = linha.ExtrairValorDaLinha(286, 286);
            objRetornar.ContaCorrenteTerceiroBeneficiario = linha.ExtrairValorDaLinha(287, 298).BoletoBrToLong();
            objRetornar.DvContaCorrenteTerceiroBeneficiario = linha.ExtrairValorDaLinha(299, 299);
            objRetornar.ValorRateioTerceiroBeneficiario = linha.ExtrairValorDaLinha(300, 314).BoletoBrToDecimal() / 100;
            objRetornar.NomeTerceiroBeneficiario = linha.ExtrairValorDaLinha(315, 354);
            objRetornar.ParcelaTerceiroBeneficiario = linha.ExtrairValorDaLinha(376, 381);
            objRetornar.FloatingTerceiroBeneficiario = linha.ExtrairValorDaLinha(382, 384).BoletoBrToInt();
            objRetornar.DataCreditoTerceiroBeneficiario = linha.ExtrairValorDaLinha(385, 392).BoletoBrToInt();
            objRetornar.MotivoOcorrenciaTerceiroBeneficiario = linha.ExtrairValorDaLinha(393, 394).BoletoBrToInt();
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();

            return objRetornar;
        }

        public TrailerRetornoCnab400 ObterTrailer(string linha)
        {
            TrailerRetornoCnab400 objRetornar = new TrailerRetornoCnab400();

            objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
            objRetornar.CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).BoletoBrToInt();
            objRetornar.TipoRegistro = linha.ExtrairValorDaLinha(3, 4);
            objRetornar.CodigoDoBanco = linha.ExtrairValorDaLinha(5, 7);
            objRetornar.QtdTitulosCobranca = linha.ExtrairValorDaLinha(18, 25).BoletoBrToInt();
            objRetornar.ValorTotalCobranca = linha.ExtrairValorDaLinha(26, 39).BoletoBrToDecimal()/100;
            objRetornar.NumeroAvisoBancario = linha.ExtrairValorDaLinha(40, 47).BoletoBrToLong();
            objRetornar.QtdRegistrosConfirmacaoEntrada = linha.ExtrairValorDaLinha(58, 62).BoletoBrToInt();
            objRetornar.ValorRegistrosConfirmacaoEntrada = linha.ExtrairValorDaLinha(63, 74).BoletoBrToDecimal()/100;
            // Layout técnico do Bradesco possui uma falha nas posições do registros TRAILER na qtd e valor dos regitros liquidados
            objRetornar.ValorRegistrosLiquidacao = linha.ExtrairValorDaLinha(75, 86).BoletoBrToDecimal()/100;
            objRetornar.QtdRegistrosLiquidacao = linha.ExtrairValorDaLinha(87, 91).BoletoBrToInt();
            objRetornar.ValorRegistrosLiquidacao = linha.ExtrairValorDaLinha(92, 103).BoletoBrToDecimal()/100;
            objRetornar.QtdRegistrosBaixados = linha.ExtrairValorDaLinha(104, 108).BoletoBrToInt();
            objRetornar.ValorRegistrosBaixados = linha.ExtrairValorDaLinha(109, 120).BoletoBrToDecimal()/100;
            objRetornar.QtdRegistrosAbatimentosCancelados = linha.ExtrairValorDaLinha(121, 125).BoletoBrToInt();
            objRetornar.ValorRegistrosAbatimentosCancelados = linha.ExtrairValorDaLinha(126, 137).BoletoBrToDecimal()/100;
            objRetornar.QtdRegistrosVencimentosAlterados = linha.ExtrairValorDaLinha(138, 142).BoletoBrToInt();
            objRetornar.ValorRegistrosVencimentosAlterados = linha.ExtrairValorDaLinha(143, 154).BoletoBrToDecimal()/100;
            objRetornar.QtdRegistrosAbatimentoConcedido = linha.ExtrairValorDaLinha(155, 159).BoletoBrToInt();
            objRetornar.ValorRegistrosAbatimentoConcedido = linha.ExtrairValorDaLinha(160, 171).BoletoBrToDecimal()/100;
            objRetornar.QtdRegistrosConfirmacaoInstrucaoProtesto = linha.ExtrairValorDaLinha(172, 176).BoletoBrToInt();
            objRetornar.ValorRegistrosConfirmacaoInstrucaoProtesto = linha.ExtrairValorDaLinha(177, 188).BoletoBrToDecimal()/100;
            objRetornar.ValorTotalRateiosEfetuados = linha.ExtrairValorDaLinha(363, 377).BoletoBrToDecimal()/100;
            objRetornar.QtdRateiosEfetuados = linha.ExtrairValorDaLinha(378, 385).BoletoBrToInt();
            objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();

            return objRetornar;
        }
    }
}
