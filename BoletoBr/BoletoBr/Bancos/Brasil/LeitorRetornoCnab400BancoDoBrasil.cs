using System;
using System.Collections.Generic;
using System.Linq;
using BoletoBr.Dominio;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Brasil
{
    public class LeitorRetornoCnab400BancoDoBrasil : ILeitorArquivoRetornoCnab400
    {
        private readonly List<string> _linhasArquivo;

        public LeitorRetornoCnab400BancoDoBrasil(List<string> linhasArquivo)
        {
            _linhasArquivo = linhasArquivo;
        }

        public RetornoCnab400 ProcessarRetorno()
        {
            /* Validações */
            #region Validações
            
            ValidaArquivoRetorno();
            
            #endregion

            var objRetornar = new RetornoCnab400 {RegistrosDetalhe = new List<DetalheRetornoCnab400>()};

            foreach (var linhaAtual in _linhasArquivo)
            {
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "0")
                {
                   objRetornar.Header = ObterHeader(linhaAtual);
                }
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "7")
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

            var qtdLinhasDetalhe = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(1, 1) == "7");

            if (qtdLinhasDetalhe <= 0)
                throw new Exception("Não foi encontrado DETALHE do arquivo de retorno.");

            var qtdLinhasTrailer = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(1, 1) == "9");

            if (qtdLinhasTrailer <= 0)
                throw new Exception("Não foi encontrado TRAILER do arquivo de retorno.");

            if (qtdLinhasTrailer > 1)
                throw new Exception("Não é permitido mais de um TRAILER no arquivo de retorno.");
        }

        /// <summary>
        /// Cód. Registro: 0
        /// Registro: HEADER
        /// Descrição: Abertura do arquivo
        /// </summary>
        /// <param name="linha"></param>
        /// <returns></returns>
        public HeaderRetornoCnab400 ObterHeader(string linha)
        {
            var objRetornar = new HeaderRetornoCnab400
            {
                CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt(),
                TipoRetorno = linha.ExtrairValorDaLinha(2, 2),
                LiteralRetorno = linha.ExtrairValorDaLinha(3, 9),
                CodigoDoServico = linha.ExtrairValorDaLinha(10, 11),
                LiteralServico = linha.ExtrairValorDaLinha(12, 19),
                CodigoAgenciaCedente = linha.ExtrairValorDaLinha(27, 30).BoletoBrToInt(),
                DvAgenciaCedente = linha.ExtrairValorDaLinha(31, 31),
                ContaCorrente = linha.ExtrairValorDaLinha(32, 39),
                DvContaCorrente = linha.ExtrairValorDaLinha(40, 40),
                NomeDoBeneficiario = linha.ExtrairValorDaLinha(47, 76),
                CodigoDoBanco = linha.ExtrairValorDaLinha(77, 79),
                CodigoENomeBanco = linha.ExtrairValorDaLinha(77, 94),
                DataGeracaoGravacao = Convert.ToDateTime(linha.ExtrairValorDaLinha(95, 100).ToDateTimeFromDdMmAa()),
                SequencialRetorno = linha.ExtrairValorDaLinha(101, 107),
                NumeroConvenio = linha.ExtrairValorDaLinha(150, 156).BoletoBrToInt(),
                NumeroSequencial = linha.ExtrairValorDaLinha(395, 400)
            };

            return objRetornar;
        }

        /// <summary>
        /// Cód. Registro: 7
        /// Registro: DETALHE
        /// Descrição: Informações do título
        /// </summary>
        /// <param name="linha"></param>
        /// <returns></returns>
        public DetalheRetornoCnab400 ObterRegistrosDetalhe(string linha)
        {
            var objRetornar = new DetalheRetornoCnab400
            {
                CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt(),
                CodigoDoServico = linha.ExtrairValorDaLinha(2, 3)
            };

            // Detalhe Geral
            if (objRetornar.CodigoDoRegistro == 7)
            {
                objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
                // Zeros
                // Zeros
                objRetornar.CodigoAgenciaCedente = linha.ExtrairValorDaLinha(18, 21).BoletoBrToInt();
                objRetornar.DvAgenciaCedente = linha.ExtrairValorDaLinha(22, 22);
                objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(23, 30);
                objRetornar.DvContaCorrente = linha.ExtrairValorDaLinha(31, 31);
                objRetornar.NumeroConvenio = linha.ExtrairValorDaLinha(32, 38).BoletoBrToInt();
                objRetornar.NumeroControle = linha.ExtrairValorDaLinha(39, 63);
                objRetornar.NossoNumero = linha.ExtrairValorDaLinha(64, 80);
                objRetornar.TipoCobranca = linha.ExtrairValorDaLinha(81, 81).BoletoBrToInt();
                objRetornar.TipoCobrancaCmd17 = linha.ExtrairValorDaLinha(82, 82).BoletoBrToInt();
                objRetornar.DiasCalculo = linha.ExtrairValorDaLinha(83, 86).BoletoBrToInt();
                objRetornar.NaturezaRecebimento = linha.ExtrairValorDaLinha(87, 88).BoletoBrToInt();
                objRetornar.PrefixoTitulo = linha.ExtrairValorDaLinha(89, 91);
                objRetornar.VariacaoCarteira = linha.ExtrairValorDaLinha(92, 94).BoletoBrToInt();
                objRetornar.ContaCaucao = linha.ExtrairValorDaLinha(95, 95).BoletoBrToInt();
                // PICTURE 9(005) - Taxa de desconto é composta por 5 números
                objRetornar.TaxaDesconto = linha.ExtrairValorDaLinha(96, 100).BoletoBrToInt()/1000;
                objRetornar.TaxaIof = linha.ExtrairValorDaLinha(101, 105).BoletoBrToInt()/100;
                // Brancos
                objRetornar.CodigoCarteira = linha.ExtrairValorDaLinha(107, 108);
                
                /* Comando e o mesmo de ocorrencia */
                objRetornar.Comando = linha.ExtrairValorDaLinha(109, 110).BoletoBrToInt();
                objRetornar.CodigoDeOcorrencia = linha.ExtrairValorDaLinha(109, 110).BoletoBrToInt();
                
                objRetornar.DataLiquidacao = Convert.ToDateTime(linha.ExtrairValorDaLinha(111, 116).ToDateTimeFromDdMmAa());
                
                /* Titulo dado pelo cedente mesmo numero do documento */
                objRetornar.TituloDadoCedente = linha.ExtrairValorDaLinha(117, 126);
                objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(117, 126);
                
                // Brancos
                objRetornar.DataDeVencimento = Convert.ToDateTime(linha.ExtrairValorDaLinha(147, 152).ToDateTimeFromDdMmAa());
                objRetornar.ValorDoTituloParcela = linha.ExtrairValorDaLinha(153, 165).BoletoBrToDecimal()/100;
                objRetornar.BancoCobrador = linha.ExtrairValorDaLinha(166, 168).BoletoBrToInt();
                objRetornar.AgenciaCobradora = linha.ExtrairValorDaLinha(169, 172).BoletoBrToInt();
                objRetornar.DvAgenciaCobradora = linha.ExtrairValorDaLinha(173, 173);
                objRetornar.Especie = linha.ExtrairValorDaLinha(174, 175);
                objRetornar.DataDeCredito = Convert.ToDateTime(linha.ExtrairValorDaLinha(176, 181).ToDateTimeFromDdMmAa());
                objRetornar.ValorTarifa = linha.ExtrairValorDaLinha(182, 188).BoletoBrToDecimal()/100;
                objRetornar.ValorOutrasDespesas = linha.ExtrairValorDaLinha(189, 201).BoletoBrToDecimal()/100;
                objRetornar.ValorJurosDesconto = linha.ExtrairValorDaLinha(202, 214).BoletoBrToDecimal()/100;
                objRetornar.ValorIofDesconto = linha.ExtrairValorDaLinha(215, 227).BoletoBrToDecimal()/100;
                objRetornar.ValorAbatimento = linha.ExtrairValorDaLinha(228, 240).BoletoBrToDecimal()/100;
                objRetornar.ValorDesconto = linha.ExtrairValorDaLinha(241, 253).BoletoBrToDecimal()/100;
                objRetornar.ValorLiquidoRecebido = linha.ExtrairValorDaLinha(254, 266).BoletoBrToDecimal()/100;
                objRetornar.ValorJurosDeMora = linha.ExtrairValorDaLinha(267, 279).BoletoBrToDecimal()/100;
                objRetornar.ValorOutrosRecebimentos = linha.ExtrairValorDaLinha(280, 292).BoletoBrToDecimal()/100;
                objRetornar.ValorAbatimentosNaoAproveitado = linha.ExtrairValorDaLinha(293, 305).BoletoBrToDecimal()/100;
                objRetornar.ValorLancamento = linha.ExtrairValorDaLinha(306, 318).BoletoBrToDecimal()/100;
                objRetornar.IndicativoDebitoCredito = linha.ExtrairValorDaLinha(319, 319);
                objRetornar.IndicadorValor = linha.ExtrairValorDaLinha(320, 320).BoletoBrToInt();
                objRetornar.ValorAjuste = linha.ExtrairValorDaLinha(321, 332).BoletoBrToDecimal()/100;
                // Brancos e Zeros específicos para cobrança compartilhada
                objRetornar.BrancosIndicadorConvenio = linha.ExtrairValorDaLinha(333, 333);
                objRetornar.BrancosValorPagoTitulo = linha.ExtrairValorDaLinha(334, 342).BoletoBrToDecimal()/100;
                objRetornar.ZerosNumeroPrimeiroConvenio = linha.ExtrairValorDaLinha(343, 349).BoletoBrToLong();
                objRetornar.ZerosValorPrimeiroConvenio = linha.ExtrairValorDaLinha(350, 358).BoletoBrToDecimal()/100;
                objRetornar.ZerosNumeroSegundoConvenio = linha.ExtrairValorDaLinha(359, 365).BoletoBrToLong();
                objRetornar.ZerosValorSegundoConvenio = linha.ExtrairValorDaLinha(366, 374).BoletoBrToDecimal()/100;
                objRetornar.ZerosNumeroTerceiroConvenio = linha.ExtrairValorDaLinha(375, 381).BoletoBrToLong();
                objRetornar.ZerosValorTerceiroConvenio = linha.ExtrairValorDaLinha(382, 390).BoletoBrToDecimal()/100;
                objRetornar.AutorizacaoLiquidacaoParcial = linha.ExtrairValorDaLinha(391, 391).BoletoBrToInt();
                // Brancos
                objRetornar.MeioApresentacaoTituloAoSacado = linha.ExtrairValorDaLinha(393, 394).BoletoBrToInt();
                objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();
            }

            // Detalhe Cobrança Partilhada Carteira 17
            if (objRetornar.CodigoDoRegistro == 2)
                ObterRegistrosDetalheAuxiliarCobrancaPartilhadaCarteira17(linha);

            // Detalhe Cobrança Vendor
            if (objRetornar.CodigoDoRegistro == 3)
                ObterRegistrosDetalheCobrancaVendor(linha);

            // Detalhe Bloqueto por E-mail
            if (objRetornar.CodigoDoRegistro == 5 && objRetornar.CodigoDoServico == "01")
                ObterRegistrosDetalheBloquetoEmail(linha);

            // Detalhe Dados do cheque utilizado para liquidação de título
            if (objRetornar.CodigoDoRegistro == 5 && objRetornar.CodigoDoServico == "04")
                ObterRegistrosDetalheDadosCheque(linha);

            // Detalhe Número do título do cedente com 15 posições (OPCIONAL)
            if (objRetornar.CodigoDoRegistro == 5 && objRetornar.CodigoDoServico == "06")
                ObterRegistrosDetalheDadosTitulo(linha);

            return objRetornar;
        }

        /// <summary>
        /// Cód. Registro: 2
        /// Registro: DETALHE
        /// Descrição: Tipo de Cobrança Simples Carteira 17
        /// </summary>
        /// <param name="linha"></param>
        /// <returns></returns>
        public DetalheRetornoCnab400 ObterRegistrosDetalheAuxiliarCobrancaPartilhadaCarteira17(string linha)
        {
            var objRetornar = new DetalheRetornoCnab400
            {
                CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt(),
                NossoNumero = linha.ExtrairValorDaLinha(2, 18),
                BancoParaCredito1 = linha.ExtrairValorDaLinha(19, 21).BoletoBrToInt(),
                CamaraCompensacao1 = linha.ExtrairValorDaLinha(22, 24).BoletoBrToInt(),
                AgenciaParaCredito1 = linha.ExtrairValorDaLinha(25, 28).BoletoBrToInt(),
                DvAgenciaParaCredito1 = linha.ExtrairValorDaLinha(29, 29),
                ContaParaCredito1 = linha.ExtrairValorDaLinha(30, 40).BoletoBrToLong(),
                DvContaParaCredito1 = linha.ExtrairValorDaLinha(41, 41),
                NomeFavorecido1 = linha.ExtrairValorDaLinha(42, 71),
                ValorInformadoPartilha1 = linha.ExtrairValorDaLinha(72, 84).BoletoBrToDecimal()/100,
                ValorEfetivamentePartilhado1 = linha.ExtrairValorDaLinha(85, 97).BoletoBrToDecimal()/100,
                BancoParaCredito2 = linha.ExtrairValorDaLinha(98, 100).BoletoBrToInt(),
                CamaraCompensacao2 = linha.ExtrairValorDaLinha(101, 103).BoletoBrToInt(),
                AgenciaParaCredito2 = linha.ExtrairValorDaLinha(104, 107).BoletoBrToInt(),
                DvAgenciaParaCredito2 = linha.ExtrairValorDaLinha(108, 108),
                ContaParaCredito2 = linha.ExtrairValorDaLinha(109, 119).BoletoBrToLong(),
                DvContaParaCredito2 = linha.ExtrairValorDaLinha(120, 120),
                NomeFavorecido2 = linha.ExtrairValorDaLinha(121, 150),
                ValorInformadoPartilha2 = linha.ExtrairValorDaLinha(151, 163).BoletoBrToDecimal()/100,
                ValorEfetivamentePartilhado2 = linha.ExtrairValorDaLinha(164, 176).BoletoBrToDecimal()/100,
                BancoParaCredito3 = linha.ExtrairValorDaLinha(177, 179).BoletoBrToInt(),
                CamaraCompensacao3 = linha.ExtrairValorDaLinha(180, 182).BoletoBrToInt(),
                AgenciaParaCredito3 = linha.ExtrairValorDaLinha(183, 186).BoletoBrToInt(),
                DvAgenciaParaCredito3 = linha.ExtrairValorDaLinha(187, 187),
                ContaParaCredito3 = linha.ExtrairValorDaLinha(188, 198).BoletoBrToLong(),
                DvContaParaCredito3 = linha.ExtrairValorDaLinha(199, 199),
                NomeFavorecido3 = linha.ExtrairValorDaLinha(200, 229),
                ValorInformadoPartilha3 = linha.ExtrairValorDaLinha(230, 242).BoletoBrToDecimal()/100,
                ValorEfetivamentePartilhado3 = linha.ExtrairValorDaLinha(243, 255).BoletoBrToDecimal()/100,
                BancoParaCredito4 = linha.ExtrairValorDaLinha(256, 258).BoletoBrToInt(),
                CamaraCompensacao4 = linha.ExtrairValorDaLinha(259, 261).BoletoBrToInt(),
                AgenciaParaCredito4 = linha.ExtrairValorDaLinha(262, 265).BoletoBrToInt(),
                DvAgenciaParaCredito4 = linha.ExtrairValorDaLinha(266, 266),
                ContaParaCredito4 = linha.ExtrairValorDaLinha(267, 277).BoletoBrToLong(),
                DvContaParaCredito4 = linha.ExtrairValorDaLinha(278, 278),
                NomeFavorecido4 = linha.ExtrairValorDaLinha(279, 308),
                ValorInformadoPartilha4 = linha.ExtrairValorDaLinha(309, 321).BoletoBrToDecimal()/100,
                ValorEfetivamentePartilhado4 = linha.ExtrairValorDaLinha(322, 334).BoletoBrToDecimal()/100,
                TipoInscricaoFavorecido1 = linha.ExtrairValorDaLinha(335, 335).BoletoBrToInt(),
                NumeroInscricaoFavorecido1 = linha.ExtrairValorDaLinha(336, 349).BoletoBrToInt(),
                TipoInscricaoFavorecido2 = linha.ExtrairValorDaLinha(350, 350).BoletoBrToInt(),
                NumeroInscricaoFavorecido2 = linha.ExtrairValorDaLinha(351, 364).BoletoBrToInt(),
                TipoInscricaoFavorecido3 = linha.ExtrairValorDaLinha(365, 365).BoletoBrToInt(),
                NumeroInscricaoFavorecido3 = linha.ExtrairValorDaLinha(366, 379).BoletoBrToInt(),
                TipoInscricaoFavorecido4 = linha.ExtrairValorDaLinha(380, 380).BoletoBrToInt(),
                NumeroInscricaoFavorecido4 = linha.ExtrairValorDaLinha(381, 394).BoletoBrToInt(),
                NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt()
            };

            return objRetornar;
        }

        /// <summary>
        /// Cód. Registro: 3
        /// Registro: DETALHE
        /// Descrição: Tipo de Cobrança Vendor
        /// </summary>
        /// <param name="linha"></param>
        /// <returns></returns>
        public DetalheRetornoCnab400 ObterRegistrosDetalheCobrancaVendor(string linha)
        {
            var objRetornar = new DetalheRetornoCnab400
            {
                CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt(),
                NossoNumero = linha.ExtrairValorDaLinha(2, 12),
                DvNossoNumero = linha.ExtrairValorDaLinha(13, 13),
                NumeroOperacaoBBVendor = linha.ExtrairValorDaLinha(14, 22).BoletoBrToLong(),
                DataOperacaoBBVendor = Convert.ToDateTime(linha.ExtrairValorDaLinha(23, 30).ToDateTimeFromDdMmAa()),
                TaxaJurosVendedor = linha.ExtrairValorDaLinha(31, 35).BoletoBrToInt()/1000,
                TaxaJurosComprador = linha.ExtrairValorDaLinha(36, 40).BoletoBrToInt()/1000,
                ValorTituloVencimento = linha.ExtrairValorDaLinha(41, 55).BoletoBrToDecimal()/100,
                ValorOriginal = linha.ExtrairValorDaLinha(56, 70).BoletoBrToDecimal()/100,
                ValorEncargosComprador = linha.ExtrairValorDaLinha(71, 85).BoletoBrToDecimal()/100,
                ValorIofFinanciado = linha.ExtrairValorDaLinha(86, 100).BoletoBrToDecimal()/100,
                ValorAcumuladoAbatimento = linha.ExtrairValorDaLinha(101, 115).BoletoBrToDecimal()/100,
                IndicativoEpocaEqualizacao = linha.ExtrairValorDaLinha(116, 116).BoletoBrToInt(),
                IndicativoNaturezaEqualizacao = linha.ExtrairValorDaLinha(117, 117).BoletoBrToInt(),
                ValorEqualizacao = linha.ExtrairValorDaLinha(118, 132).BoletoBrToDecimal()/100,
                ValorJurosProrrogacao = linha.ExtrairValorDaLinha(133, 147).BoletoBrToDecimal()/100,
                ValorIofProrrogacao = linha.ExtrairValorDaLinha(148, 162).BoletoBrToDecimal()/100,
                ValorIofPeriodoAtraso = linha.ExtrairValorDaLinha(163, 177).BoletoBrToDecimal()/100,
                NomeComprador = linha.ExtrairValorDaLinha(178, 214),
                TipoInscricaoComprador = linha.ExtrairValorDaLinha(215, 215).BoletoBrToInt(),
                NumeroInscricaoComprador = linha.ExtrairValorDaLinha(216, 229).BoletoBrToLong(),
                TipoConversaoCnab240 = linha.ExtrairValorDaLinha(230, 231).BoletoBrToInt(),
                NossoNumero17Posicoes = linha.ExtrairValorDaLinha(232, 248),
                NovoValorTitulo = linha.ExtrairValorDaLinha(249, 263).BoletoBrToDecimal()/100,
                ValorEqualizacaoEstornada = linha.ExtrairValorDaLinha(264, 278).BoletoBrToDecimal()/100,
                ValorNovaEqualizacao = linha.ExtrairValorDaLinha(279, 293).BoletoBrToDecimal()/100,
                ValorDiferencaEqualizacao = linha.ExtrairValorDaLinha(294, 308).BoletoBrToDecimal()/100,
                IndicativoDebitoCreditoCampo25 = linha.ExtrairValorDaLinha(309, 309).BoletoBrToInt(),
                IndicativoNaturezaEqualizacao1 = linha.ExtrairValorDaLinha(310, 310).BoletoBrToInt(),
                IndicativoNaturezaEqualizacao2 = linha.ExtrairValorDaLinha(311, 311).BoletoBrToInt(),
                ValorIofNaoFinanciado = linha.ExtrairValorDaLinha(312, 326).BoletoBrToDecimal()/100,
                ValorComissaoPermanencia = linha.ExtrairValorDaLinha(327, 341).BoletoBrToDecimal()/100
            };

            return objRetornar;
        }

        /// <summary>
        /// Cód. Registro: 5
        /// Registro: DETALHE
        /// Descrição: Bloqueto por e-mail
        /// </summary>
        /// <param name="linha"></param>
        /// <returns></returns>
        public DetalheRetornoCnab400 ObterRegistrosDetalheBloquetoEmail(string linha)
        {
            var objRetornar = new DetalheRetornoCnab400
            {
                CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt(),
                CodigoDoServico = linha.ExtrairValorDaLinha(2, 3),
                EnderecosEmail = linha.ExtrairValorDaLinha(6, 142),
                NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt()
            };

            return objRetornar;
        }

        /// <summary>
        /// Cód. Registro: 5
        /// Registro: DETALHE
        /// Descrição: Dados do cheque utilizado para liquidação de título
        /// </summary>
        /// <param name="linha"></param>
        /// <returns></returns>
        public DetalheRetornoCnab400 ObterRegistrosDetalheDadosCheque(string linha)
        {
            var objRetornar = new DetalheRetornoCnab400
            {
                CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt(),
                CodigoDoServico = linha.ExtrairValorDaLinha(2, 3),
                NossoNumero17Posicoes = linha.ExtrairValorDaLinha(6, 22),
                DataPagamento = linha.ExtrairValorDaLinha(23, 30).BoletoBrToInt(),
                ValorCheque = linha.ExtrairValorDaLinha(45, 59).BoletoBrToDecimal()/100,
                PrazoBloqueioCheque = linha.ExtrairValorDaLinha(60, 62).BoletoBrToInt(),
                MotivoDevolucaoCheque = linha.ExtrairValorDaLinha(66, 68).BoletoBrToInt(),
                TrilhaDoCheque = linha.ExtrairValorDaLinha(70, 103),
                TipoCaptura = linha.ExtrairValorDaLinha(104, 104),
                NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt()
            };

            return objRetornar;
        }

        /// <summary>
        /// Cód. Registro: 5
        /// Registro: DETALHE
        /// Descrição: Número do título do cedente com 15 posições
        /// </summary>
        /// <param name="linha"></param>
        /// <returns></returns>
        public DetalheRetornoCnab400 ObterRegistrosDetalheDadosTitulo(string linha)
        {
            // Registro Opcional
            var objRetornar = new DetalheRetornoCnab400
            {
                CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt(),
                CodigoDoServico = linha.ExtrairValorDaLinha(2, 3),
                NumeroTituloCedente = linha.ExtrairValorDaLinha(6, 20),
                NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt()
            };

            return objRetornar;
        }

        /// <summary>
        /// Cód. Registro: 9
        /// Registro: TRAILER
        /// Descrição: Encerramento do arquivo
        /// </summary>
        /// <param name="linha"></param>
        /// <returns></returns>
        public TrailerRetornoCnab400 ObterTrailer(string linha)
        {
            var objRetornar = new TrailerRetornoCnab400
            {
                CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt(),
                CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).BoletoBrToInt(),
                CodigoDoServico = linha.ExtrairValorDaLinha(3, 4),
                CodigoDoBanco = linha.ExtrairValorDaLinha(5, 7),
                //Brancos
                QtdTitulosCobrancaSimples = linha.ExtrairValorDaLinha(18, 25).BoletoBrToLong(),
                ValorTitulosCobrancaSimples = linha.ExtrairValorDaLinha(26, 39).BoletoBrToDecimal()/100,
                NumeroAvisoCobrancaSimples = linha.ExtrairValorDaLinha(40, 47).BoletoBrToInt(),
                //Brancos
                QtdTitulosCobrancaVinculada = linha.ExtrairValorDaLinha(58, 65).BoletoBrToLong(),
                ValorTitulosCobrancaVinculada = linha.ExtrairValorDaLinha(66, 79).BoletoBrToDecimal()/100,
                NumeroAvisoCobrancaVinculada = linha.ExtrairValorDaLinha(80, 87).BoletoBrToInt(),
                //Brancos
                QtdTitulosCobrancaCaucionada = linha.ExtrairValorDaLinha(98, 105).BoletoBrToInt(),
                ValorTitulosCobrancaCaucionada = linha.ExtrairValorDaLinha(106, 119).BoletoBrToDecimal()/100,
                NumeroAvisoCobrancaCaucionada = linha.ExtrairValorDaLinha(120, 127).BoletoBrToInt(),
                //Brancos
                QtdTitulosCobrancaDescontada = linha.ExtrairValorDaLinha(138, 145).BoletoBrToInt(),
                ValorTitulosCobrancaDescontada = linha.ExtrairValorDaLinha(146, 159).BoletoBrToDecimal()/100,
                NumeroAvisoCobrancaDescontada = linha.ExtrairValorDaLinha(160, 167).BoletoBrToInt(),
                //Brancos
                QtdTitulosCobrancaVendor = linha.ExtrairValorDaLinha(218, 225).BoletoBrToInt(),
                ValorTitulosCobrancaVendor = linha.ExtrairValorDaLinha(226, 239).BoletoBrToDecimal()/100,
                NumeroAvisoCobrancaVendor = linha.ExtrairValorDaLinha(240, 247).BoletoBrToInt(),
                //Brancos
                NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt()
            };

            return objRetornar;
        }
    }
}
