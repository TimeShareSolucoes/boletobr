using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Bradesco
{
    public class BancoBradesco : IBanco
    {
        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        public Image LogotipoBancoParaExibicao { get; set; }

        public BancoBradesco()
        {
            CodigoBanco = "237";
            DigitoBanco = "2";
            NomeBanco = "Bradesco";
            LocalDePagamento = "Pagável preferencialmente nas Agências Bradesco.";
            MoedaBanco = "9";
        }

        public string CalcularDigitoNossoNumero(Boleto boleto)
        {
            return Common.Mod11Base7Bradesco(boleto.CarteiraCobranca.Codigo + boleto.NossoNumeroFormatado, 7);
        }

        private int _digitoAutoConferenciaBoleto;
        private string _digitoAutoConferenciaNossoNumero;

        public string LocalDePagamento { get; private set; }
        public string MoedaBanco { get; private set; }

        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            if (boleto.CarteiraCobranca.Codigo != "02" && boleto.CarteiraCobranca.Codigo != "03" &&
                boleto.CarteiraCobranca.Codigo != "06" && boleto.CarteiraCobranca.Codigo != "09" &&
                boleto.CarteiraCobranca.Codigo != "19")
                throw new ValidacaoBoletoException(
                    "Carteira não implementada. Carteiras implementadas 02, 03, 06, 09, 19.");

            //O valor � obrigat�rio para a carteira 03
            if (boleto.CarteiraCobranca.Codigo == "03")
            {
                if (boleto.ValorBoleto == 0)
                    throw new ValidacaoBoletoException("Para a carteira 03, o valor do boleto n�o pode ser igual a zero");
            }

            //O valor � obrigat�rio para a carteira 09
            if (boleto.CarteiraCobranca.Codigo == "09")
            {
                if (boleto.ValorBoleto == 0)
                    throw new ValidacaoBoletoException("Para a carteira 09, o valor do boleto não pode ser igual a zero");
            }

            //Verifica se o nosso n�mero � v�lido
            //if (boleto.NossoNumeroFormatado.Length > 16)
            //    throw new ValidacaoBoletoException("A quantidade de dígitos do nosso número deve ser 16 números."
            //        + Environment.NewLine + "02->Carteira/" + Environment.NewLine + "11->Nosso Número-" + Environment.NewLine + "01->DV");

            //Verifica se a Agencia esta correta
            if (boleto.CedenteBoleto.ContaBancariaCedente.Agencia.Length > 4)
                throw new ValidacaoBoletoException("A quantidade de dígitos da Agência " +
                                                   boleto.CedenteBoleto.ContaBancariaCedente.Agencia +
                                                   ", deve ser de 4 números.");
            if (boleto.CedenteBoleto.ContaBancariaCedente.Agencia.Length < 4)
                boleto.CedenteBoleto.ContaBancariaCedente.Agencia =
                    boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0');

            //Verifica se a Conta esta correta
            if (boleto.CedenteBoleto.ContaBancariaCedente.Conta.Length > 7)
                throw new ValidacaoBoletoException("A quantidade de dígitos da Conta " +
                                                   boleto.CedenteBoleto.ContaBancariaCedente.Conta +
                                                   ", deve ser de 07 números.");
            if (boleto.CedenteBoleto.ContaBancariaCedente.Conta.Length < 7)
                boleto.CedenteBoleto.ContaBancariaCedente.Conta =
                    boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(7, '0');

            //Verifica se data do processamento � valida
            //if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;


            //Verifica se data do documento � valida
            //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;
        }

        public void FormataMoeda(Boleto boleto)
        {
            boleto.Moeda = MoedaBanco;

            if (String.IsNullOrEmpty(boleto.Moeda))
                throw new Exception("Espécie/Moeda para o boleto não foi informada.");

            if ((boleto.Moeda == "9") || (boleto.Moeda == "REAL") || (boleto.Moeda == "R$"))
                boleto.Moeda = "R$";
            else
                boleto.Moeda = "0";
        }

        public void FormatarBoleto(Boleto boleto)
        {
            //Atribui o local de pagamento
            boleto.LocalPagamento = LocalDePagamento;

            boleto.ValidaDadosEssenciaisDoBoleto();

            // Calcula o DAC do Nosso Número
            _digitoAutoConferenciaNossoNumero = CalcularDigitoNossoNumero(boleto);
            boleto.DigitoNossoNumero = _digitoAutoConferenciaNossoNumero;

            FormataNumeroDocumento(boleto);
            FormataNossoNumero(boleto);
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);

            FormataMoeda(boleto);

            ValidaBoletoComNormasBanco(boleto);
        }

        /// <summary>
        /// 
        ///   *******
        /// 
        ///	O c�digo de barra para cobran�a cont�m 44 posi��es dispostas da seguinte forma:
        ///    01 a 03 - 3 - Identifica��o  do  Banco
        ///    04 a 04 - 1 - C�digo da Moeda
        ///    05 a 05 � 1 - D�gito verificador do C�digo de Barras
        ///    06 a 09 - 4 - Fator de vencimento
        ///    10 a 19 - 10 - Valor
        ///    20 a 44 � 25 - Campo Livre
        /// 
        ///   *******
        /// 
        /// </summary>

        public void FormataNossoNumero(Boleto boleto)
        {
            boleto.SetNossoNumeroFormatado(boleto.SequencialNossoNumero);

            boleto.SetNossoNumeroFormatado(
                string.Format("{0}/{1}-{2}",
                    boleto.CarteiraCobranca.Codigo,
                    boleto.NossoNumeroFormatado.PadLeft(11, '0'),
                    boleto.DigitoNossoNumero));
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            var valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = valorBoleto.PadLeft(10, '0');

            if (boleto.CarteiraCobranca.Codigo == "02" || boleto.CarteiraCobranca.Codigo == "03" ||
                boleto.CarteiraCobranca.Codigo == "09" || boleto.CarteiraCobranca.Codigo == "19")
            {
                boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}", CodigoBanco, boleto.Moeda,
                    Common.FatorVencimento(boleto.DataVencimento), valorBoleto, FormataCampoLivre(boleto));
            }
            else if (boleto.CarteiraCobranca.Codigo == "06")
            {
                if (boleto.ValorBoleto == 0)
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}0000{2}{3}", CodigoBanco, boleto.Moeda,
                        valorBoleto, FormataCampoLivre(boleto));
                }
                else
                {
                    boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}", CodigoBanco, boleto.Moeda,
                        Common.FatorVencimento(boleto.DataVencimento), valorBoleto, FormataCampoLivre(boleto));
                }
            }
            else
            {
                throw new Exception("Carteira ainda não implementada.");
            }

            _digitoAutoConferenciaBoleto = Common.Mod11(boleto.CodigoBarraBoleto, 9);

            boleto.CodigoBarraBoleto = Common.Left(boleto.CodigoBarraBoleto, 4) + _digitoAutoConferenciaBoleto +
                                       Common.Right(boleto.CodigoBarraBoleto, 39);
        }

        ///<summary>
        /// Campo Livre
        ///    20 a 23 -  4 - Ag�ncia Cedente (Sem o digito verificador,completar com zeros a esquerda quandonecess�rio)
        ///    24 a 25 -  2 - Carteira
        ///    26 a 36 - 11 - N�mero do Nosso N�mero(Sem o digito verificador)
        ///    37 a 43 -  7 - Conta do Cedente (Sem o digito verificador,completar com zeros a esquerda quando necess�rio)
        ///    44 a 44	- 1 - Zero            
        ///</summary>
        public string FormataCampoLivre(Boleto boleto)
        {

            string formataCampoLivre = string.Format("{0}{1}{2}{3}{4}",
                boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'), boleto.CarteiraCobranca.Codigo,
                boleto.NossoNumeroFormatado.Replace("/", "").Replace("-", "").ExtrairValorDaLinha(1, 11),
                boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(7, '0'), "0");

            return formataCampoLivre;
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {

            //BBBMC.CCCCD1 CCCCC.CCCCCD2 CCCCC.CCCCCD3 D4 FFFFVVVVVVVVVV

            #region Campo 1

            var BBB = boleto.CodigoBarraBoleto.Substring(0, 3);
            var M = boleto.CodigoBarraBoleto.Substring(3, 1);
            var CCCCC = boleto.CodigoBarraBoleto.Substring(19, 5);
            var D1 = Common.Mod10(BBB + M + CCCCC).ToString(CultureInfo.InvariantCulture);

            var Grupo1 = string.Format("{0}{1}{2}.{3}{4} ", BBB, M, CCCCC.Substring(0, 1), CCCCC.Substring(1, 4), D1);


            #endregion Campo 1

            #region Campo 2

            var CCCCCCCCCC2 = boleto.CodigoBarraBoleto.Substring(24, 10);
            var D2 = Common.Mod10(CCCCCCCCCC2).ToString(CultureInfo.InvariantCulture);

            var Grupo2 = string.Format("{0}.{1}{2} ", CCCCCCCCCC2.Substring(0, 5), CCCCCCCCCC2.Substring(5, 5), D2);

            #endregion Campo 2

            #region Campo 3

            var CCCCCCCCCC3 = boleto.CodigoBarraBoleto.Substring(34, 10);
            var D3 = Common.Mod10(CCCCCCCCCC3).ToString(CultureInfo.InvariantCulture);

            var Grupo3 = string.Format("{0}.{1}{2} ", CCCCCCCCCC3.Substring(0, 5), CCCCCCCCCC3.Substring(5, 5), D3);

            #endregion Campo 3

            #region Campo 4

            var D4 = _digitoAutoConferenciaBoleto.ToString(CultureInfo.InvariantCulture);

            var Grupo4 = string.Format("{0} ", D4);

            #endregion Campo 4

            #region Campo 5

            //string FFFF = boleto.CodigoBarra.Codigo.Substring(5, 4);//FatorVencimento(boleto).ToString() ;
            var FFFF = Common.FatorVencimento(boleto.DataVencimento).ToString(CultureInfo.InvariantCulture);

            //if (boleto.CarteiraCobranca.Codigo == "06" && boleto.DataVencimento == DateTime.MinValue)
            //    FFFF = "0000";

            var VVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            VVVVVVVVVV = VVVVVVVVVV.PadLeft(10, '0');

            //if (Convert.ToInt64(VVVVVVVVVV) == 0)
            //    VVVVVVVVVV = "000";

            var Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

            #endregion Campo 5

            boleto.LinhaDigitavelBoleto = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;

        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            if (String.IsNullOrEmpty(boleto.NumeroDocumento) || String.IsNullOrEmpty(boleto.NumeroDocumento.TrimStart('0')))
                throw new Exception("Número do Documento não foi informado.");

            boleto.NumeroDocumento = boleto.NumeroDocumento.PadLeft(11, '0');
        }

        public IEspecieDocumento ObtemEspecieDocumento(EnumEspecieDocumento especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento.DuplicataMercantil:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 01,
                        Descricao = "Duplicata mercantil",
                        Sigla = "DM"
                    };
                }
                case EnumEspecieDocumento.NotaPromissoria:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 02,
                        Descricao = "Nota promissória",
                        Sigla = "NP"
                    };
                }
                case EnumEspecieDocumento.NotaDeSeguro:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 03,
                        Descricao = "Nota de seguro",
                        Sigla = "NS"
                    };
                }
                case EnumEspecieDocumento.CobrancaSeriada:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 04,
                        Descricao = "Cobrança seriada",
                        Sigla = "CS"
                    };
                }
                case EnumEspecieDocumento.Recibo:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 05,
                        Descricao = "Recibo",
                        Sigla = "RC"
                    };
                }
                case EnumEspecieDocumento.LetraCambio:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 10,
                        Descricao = "Letra de câmbio",
                        Sigla = "LC"
                    };
                }
                case EnumEspecieDocumento.NotaDebito:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 11,
                        Descricao = "Nota de débito",
                        Sigla = "ND"
                    };
                }
                case EnumEspecieDocumento.DuplicataServico:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 98,
                        Descricao = "Duplicata de Serv.",
                        Sigla = "DS"
                    };
                }
                case EnumEspecieDocumento.Outros:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 99,
                        Descricao = "Outros",
                        Sigla = "OU"
                    };
                }
            }

            throw new Exception(
                String.Format("Não foi possível obter instrução padronizada. Banco: {0} Código Espécie: {1}",
                    CodigoBanco, especie.ToString()));
        }

        public IInstrucao ObtemInstrucaoPadronizada(EnumTipoInstrucao tipoInstrucao, double valorInstrucao,
            DateTime dataInstrucao, int diasInstrucao)
        {
            switch (tipoInstrucao)
            {
                case EnumTipoInstrucao.NaoCobrarJurosDeMora:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 08,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não cobrar juros de mora."
                    };
                }
                case EnumTipoInstrucao.NaoReceberAposOVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 09,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber após o vencimento."
                    };
                }
                case EnumTipoInstrucao.MultaPercentualVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 10,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Multa de 10 % após o 4º dia do vencimento."
                    };
                }
                case EnumTipoInstrucao.NaoReceberAposNDiasCorridos:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 11,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber após o 8º dia do vencimento."
                    };
                }
                case EnumTipoInstrucao.CobrarEncargosApos5DiaVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 12,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Cobrar encargos após o 5º dia do vencimento."
                    };
                }
                case EnumTipoInstrucao.CobrarEncargosApos10DiaVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 13,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Cobrar encargos após o 10º dia do vencimento."
                    };
                }
                case EnumTipoInstrucao.CobrarEncargosApos15DiaVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 14,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Cobrar encargos após o 15º dia do vencimento."
                    };
                }
                case EnumTipoInstrucao.ConcederDescontoPagoAposVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 15,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Conceder desconto mesmo se pago após o vencimento."
                    };
                }
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter instrução padronizada. Banco: {0} Código Instrução: {1} Qtd Dias/Valor: {2}",
                    CodigoBanco, tipoInstrucao.ToString(), valorInstrucao));
        }

        public RetornoGenerico LerArquivoRetorno(List<string> linhasArquivo)
        {
            if (linhasArquivo == null || linhasArquivo.Any() == false)
                throw new ApplicationException("Arquivo informado é inválido.");

            /* Identifica o layout: 240 ou 400 */
            if (linhasArquivo.First().Length == 240)
            {
                var leitor = new LeitorRetornoCnab240Bradesco(linhasArquivo);
                var retornoProcessado = leitor.ProcessarRetorno();

                var objRetornar = new RetornoGenerico(retornoProcessado);
                return objRetornar;
            }
            if (linhasArquivo.First().Length == 400)
            {
                var leitor = new LeitorRetornoCnab400Bradesco(linhasArquivo);
                var retornoProcessado = leitor.ProcessarRetorno();

                var objRetornar = new RetornoGenerico(retornoProcessado);
                return objRetornar;
            }

            throw new Exception("Arquivo de RETORNO com " + linhasArquivo.First().Length + " posições, não é suportado.");
        }

        public RemessaCnab240 GerarArquivoRemessaCnab240(List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }

        public RemessaCnab400 GerarArquivoRemessaCnab400(List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }

        public RemessaCnab240 GerarArquivoRemessaCnab240(RemessaCnab240 remessaCnab240, List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }

        public RemessaCnab400 GerarArquivoRemessaCnab400(RemessaCnab400 remessaCnab400, List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }

        public ICodigoOcorrencia ObtemCodigoOcorrencia(EnumCodigoOcorrenciaRemessa ocorrencia, double valorOcorrencia,
            DateTime dataOcorrencia)
        {
            switch (ocorrencia)
            {
                case EnumCodigoOcorrenciaRemessa.Registro:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 01,
                        Descricao = "Remessa"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.Baixa:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 02,
                        Descricao = "Pedido de baixa"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.ConcessaoDeAbatimento:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 04,
                        Descricao = "Concessão de abatimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDeAbatimento:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 05,
                        Descricao = "Cancelamento de abatimento concedido"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeVencimento:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 06,
                        Descricao = "Alteração de vencimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDoControleDoParticipante:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 07,
                        Descricao = "Alteração do controle do participante"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoSeuNumero:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 08,
                        Descricao = "Alteração de seu número"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.Protesto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 09,
                        Descricao = "Pedido de Protesto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.SustarProtestoEBaixarTitulo:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 18,
                        Descricao = "Sustar protesto e baixar título"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.SustarProtestoEManterEmCarteira:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 19,
                        Descricao = "Sustar protesto e manter em carteira"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.TransferenciaCessaoCredito:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 22,
                        Descricao = "Transferência cessão crédito ID. Prod. 10"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.TransferenciaEntreCarteiras:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 23,
                        Descricao = "Transferência entre carteiras"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.DevTransferenciaEntreCarteiras:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 24,
                        Descricao = "Dev. Transferência entre carteiras"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeOutrosDados:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 31,
                        Descricao = "Alteração de outros dados"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.DesagendamentoDoDebitoAutomatico:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 35,
                        Descricao = "Desagendamento do débito automático"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AcertoNosDadosDoRateioDeCredito:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 68,
                        Descricao = "Acerto nos dados do rateio de crédito"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDoRateioDeCredito:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 69,
                        Descricao = "Cancelamento do rateio do crédito"
                    };
                }
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter Código de Comando/Movimento/Ocorrência. Banco: {0} Código: {1}",
                    CodigoBanco, ocorrencia.ToString()));
        }
    }
}
