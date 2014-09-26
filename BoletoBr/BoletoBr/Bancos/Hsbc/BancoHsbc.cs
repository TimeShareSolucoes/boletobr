using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Hsbc
{
    public class BancoHsbc : IBanco
    {
        #region Variáveis

        private int _digitoAutoConferenciaCodigoBarras;
        private string _digitoAutoConferenciaNossoNumero;

        #endregion

        #region Propriedades

        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        public Image LogotipoBancoParaExibicao { get; set; }

        #endregion

        #region Construtores

        public BancoHsbc()
        {
            CodigoBanco = "399";
            DigitoBanco = "9";
            NomeBanco = "HSBC";
            LocalDePagamento = "Pagável em qualquer banco até o vencimento.";
            MoedaBanco = "9";
        }

        #endregion

        #region Métodos de formatação do boleto

        public string LocalDePagamento { get; private set; }
        public string MoedaBanco { get; private set; }

        /// <summary>
        /// Valida se o boleto está preenchido com os campos mínimos requeridos.
        /// Dispara uma ApplicationException caso esteja faltando alguma informação.
        /// </summary>
        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            //Verifica as carteiras implementadas
            if (!boleto.CarteiraCobranca.Codigo.Equals("CSB") &
                !boleto.CarteiraCobranca.Codigo.Equals("CNR"))
                throw new NotImplementedException("Carteira n�o implementada. Utilize a carteira 'CSB' ou 'CNR'.");

            //Verifica se o nosso n�mero � v�lido
            if (boleto.NossoNumeroFormatado.BoletoBrToStringSafe() == string.Empty)
                throw new NotImplementedException("Nosso número inválido");

            //Verifica se o nosso n�mero � v�lido
            if (boleto.NossoNumeroFormatado.BoletoBrToStringSafe().BoletoBrToLong() == 0)
                throw new NotImplementedException("Nosso número inválido");

            //Verifica se o tamanho para o NossoNumero s�o 10 d�gitos (5 range + 5 numero sequencial) - Válido para carteira CSB
            if (boleto.CarteiraCobranca.Codigo.Equals("CSB"))
            if (Convert.ToInt32(boleto.NossoNumeroFormatado).ToString(CultureInfo.InvariantCulture).Length > 10)
                throw new NotImplementedException("A quantidade de dígitos do nosso número para a carteira " +
                                                  boleto.CarteiraCobranca.Codigo + ", são 10 números.");
            else if (Convert.ToInt32(boleto.NossoNumeroFormatado).ToString(CultureInfo.InvariantCulture).Length < 10)
                boleto.SetNossoNumeroFormatado(boleto.NossoNumeroFormatado.PadLeft(10, '0'));

            //Verifica se data do documento � valida
            //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataDocumento == DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;
        }

        public void FormataMoeda(Boleto boleto)
        {
            boleto.Moeda = MoedaBanco;

            if (string.IsNullOrEmpty(boleto.Moeda))
                throw new Exception("Espécie/Moeda para o boleto não foi informada.");

            if ((boleto.Moeda == "9") || (boleto.Moeda == "REAL") || (boleto.Moeda == "R$"))
                boleto.Moeda = "R$";
            else
                boleto.Moeda = "0";
        }

        /// <summary>
        /// Formata número do documento conforme Tipo de Arquivo escolhido na emissão
        /// </summary>
        /// <param name="boleto"></param>
        public void FormataNumeroDocumento(Boleto boleto)
        {
            if (String.IsNullOrEmpty(boleto.NumeroDocumento) || String.IsNullOrEmpty(boleto.NumeroDocumento.TrimStart('0')))
                throw new Exception("Número do Documento não foi informado.");

            boleto.NumeroDocumento = boleto.NumeroDocumento.PadLeft(13, '0');
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
                        Descricao = "Duplicata Mercantil",
                        Sigla = "DP"
                    };
                }
                case EnumEspecieDocumento.NotaPromissoria:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 02,
                        Descricao = "Nota Promissória",
                        Sigla = "NP"
                    };
                }
                case EnumEspecieDocumento.NotaSeguro:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 03,
                        Descricao = "Nota de Seguro",
                        Sigla = "NS"
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
                case EnumEspecieDocumento.DuplicataServico:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 10,
                        Descricao = "Duplicata de Serviços",
                        Sigla = "DS"
                    };
                }
            }
            throw new Exception(
                String.Format("Não foi possível obter espécie. Banco: {0} Código Espécie: {1}",
                    CodigoBanco, especie.ToString()));
        }

        public IInstrucao ObtemInstrucaoPadronizada(EnumTipoInstrucao tipoInstrucao, double valorInstrucao,
            DateTime dataInstrucao, int diasInstrucao)
        {
            switch (tipoInstrucao)
            {
                case EnumTipoInstrucao.MultaPercentualVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 15,
                        QtdDias = (int) valorInstrucao,
                        TextoInstrucao = "Multa de " + valorInstrucao + " por cento após dia " + dataInstrucao
                    };
                }
                case EnumTipoInstrucao.MultaPorDiaVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 16,
                        QtdDias = (int) valorInstrucao,
                        TextoInstrucao =
                            "Após " + dataInstrucao + " multa dia de " + valorInstrucao + "  máximo " + "???"
                    };
                }
                case EnumTipoInstrucao.MultaPorDiaCorrido:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 19,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao =
                            "Multa de R$ " + valorInstrucao + " após " + diasInstrucao + " dias corridos do vencimento."
                    };
                }
                case EnumTipoInstrucao.CobrarJurosApos7DiasVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 20,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Cobrar juros só após 07 dias do vencimento."
                    };
                }
                case EnumTipoInstrucao.MultaPorDiaUtil:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 22,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao =
                            "Multa de R$ " + valorInstrucao + " após " + diasInstrucao + " dias úteis do vencimento."
                    };
                }
                case EnumTipoInstrucao.NaoReceberAposOVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 23,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber após o vencimento."
                    };
                }
                case EnumTipoInstrucao.MultaVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 24,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Multa de R$ " + valorInstrucao + " após o vencimento."
                    };
                }
                case EnumTipoInstrucao.JurosSoAposData:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 29,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Juros só após " + dataInstrucao + ", cobrar desde o vencimento."
                    };
                }
                case EnumTipoInstrucao.ConcederAbatimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 34,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Conceder abatimento conforme proposto pelo pagador."
                    };
                }
                case EnumTipoInstrucao.AposVencimentoMulta10PorCento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 36,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Após vencimento multa de 10 por cento."
                    };
                }
                case EnumTipoInstrucao.ConcederDescontoPagoAposVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 40,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Conceder desconto mesmo se pago após o vencimento."
                    };
                }
                case EnumTipoInstrucao.NaoReceberAntesDoVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 42,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber antes do vencimento."
                    };
                }
                case EnumTipoInstrucao.AposVencimentoMulta20PorCentoMaisMora:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 53,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Após vencimento multa de 20% mais mora de 1% a.m."
                    };
                }
                case EnumTipoInstrucao.NaoReceberAntesdoVencimentoOu10DiasApos:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 56,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber antes do vencimento ou 10 dias após."
                    };
                }
                //case EnumTipoInstrucao.AbatimentoDesconto:
                //{
                //    return new InstrucaoPadronizada
                //    {
                //        Codigo = 65,
                //        QtdDias = diasInstrucao,
                //        Valor = valorInstrucao,
                //        TextoInstrucao = "Abatimento/Desconto só com instrução do benefiário"
                //    };
                //}
                case EnumTipoInstrucao.TituloSujeitoAProtestoAposVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 67,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Título sujeito a protesto após o vencimento."
                    };
                }
                case EnumTipoInstrucao.AposVencimentoMulta2PorCento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 68,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Após o vencimento multa de 2 por cento."
                    };
                }
                case EnumTipoInstrucao.NaoReceberAposNDiasCorridos:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 71,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber após " + diasInstrucao + " dias corridos do vencimento."
                    };
                }
                case EnumTipoInstrucao.NaoReceberAposNDiasUteis:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 72,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber após " + diasInstrucao + " dias úteis do vencimento."
                    };
                }
                case EnumTipoInstrucao.MultaDeVPorCentoAposNDiasCorridos:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 73,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Multa de " + valorInstrucao + " por cento após " + diasInstrucao + " dias corridos do vencimento."
                    };
                }
                case EnumTipoInstrucao.MultaDeVPorCentoAposNDiasUteis:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 74,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Multa de " + valorInstrucao + " por cento após " + diasInstrucao + " dias úteis do vencimento."
                    };
                }
                case EnumTipoInstrucao.ProtestarAposNDiasCorridos:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 75,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Protestar " + diasInstrucao + " dias corridos após o vencimento, se não pago."
                    };
                }
                case EnumTipoInstrucao.ProtestarAposNDiasUteis:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 77,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Protestar " + diasInstrucao + " dias úteis após o vencimento, se não pago."
                    };
                }
                /* Instruções que não geram mensagens nos boletos */
                case EnumTipoInstrucao.ProtestarAposNDiasUteisNGM:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 76,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Protestar " + diasInstrucao + " dias úteis após o vencimento, se não pago."
                    };
                }
                /* Instruções que não geram mensagens nos boletos */
                case EnumTipoInstrucao.ProtestarAposNDiasCorridosNGM:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 84,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Protestar " + diasInstrucao + " dias corridos após o vencimento, se não pago."
                    };
                }
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter instrução padronizada. Banco: {0} Código Instrução: {1} Qtd Dias/Valor: {2}",
                    CodigoBanco, tipoInstrucao.ToString(), valorInstrucao));
        }

        public ICodigoOcorrencia ObtemCodigoOcorrencia(EnumCodigoOcorrenciaRemessa ocorrenciaRemessa, double valorOcorrencia, DateTime dataOcorrencia)
        {
            switch (ocorrenciaRemessa)
            {
                case EnumCodigoOcorrenciaRemessa.Registro:
                {
                    return new CodigoOcorrencia((int) ocorrenciaRemessa)
                    {
                        Codigo = 01,
                        Descricao = "Entrada de títulos"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.Baixa:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 02,
                        Descricao = "Pedido de baixa"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.ConcessaoDeAbatimento:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 04,
                        Descricao = "Concessão de abatimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDeAbatimento:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 05,
                        Descricao = "Cancelamento de abatimento concedido"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeVencimento:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 06,
                        Descricao = "Alteração de vencimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.ConcessaoDeDesconto:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 07,
                        Descricao = "Conceder desconto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDoControleDoParticipante:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 07,
                        Descricao = "Alteração do controle do participante"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDeDesconto:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 08,
                        Descricao = "Cancelamento de desconto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoSeuNumero:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 08,
                        Descricao = "Alteração do seu número"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.Protesto:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 09,
                        Descricao = "Protestar"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.SustarProtesto:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 10,
                        Descricao = "Sustar protesto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.NaoCobrarJurosDeMora:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 11,
                        Descricao = "Não cobrar juros de mora"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.ConcessaoDeDescontoComData:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 13,
                        Descricao = "Conceder desconto R$ " + string.Format("{0:0.##}", valorOcorrencia) + " p/ pgto até " + dataOcorrencia
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDescontoFixo:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 14,
                        Descricao = "Cancelamento condição de desconto fixo"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDescontoDiario:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 15,
                        Descricao = "Cancelamento de desconto diário"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeVencimentoComData:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 48,
                        Descricao = "Vencimento alterado para " + dataOcorrencia
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeDiasParaEnvioACartorio:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 49,
                        Descricao = "Alteração de dias para envio a Cartório de Protesto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.InclusaoDePagadorNoBoleto:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 50,
                        Descricao = "Inclusão de pagador no boleto eletrônico"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.ExclusaoDePagadorNoBoleto:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 51,
                        Descricao = "Exclusão de pagador no boleto eletrônico"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.Reemissao:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 52,
                        Descricao = "Reemissão"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.EntradaDeTitulosComParcelasFaltantes:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 53,
                        Descricao = "Entrada de títulos com parcelas faltantes"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.TransferenciaParaDesconto:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 55,
                        Descricao = "Transferência para desconto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.ProtestoParaFinsFalimentares:
                {
                    return new CodigoOcorrencia((int)ocorrenciaRemessa)
                    {
                        Codigo = 57,
                        Descricao = "Protesto para fins falimentares"
                    };
                }
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter Código de Comando/Movimento/Ocorrência. Banco: {0} Código: {1}",
                    CodigoBanco, ocorrenciaRemessa));
        }

        public RetornoGenerico LerArquivoRetorno(List<string> linhasArquivo)
        {
            throw new NotImplementedException();
        }

        public RemessaCnab240 GerarArquivoRemessaCnab240(List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }

        public RemessaCnab400 GerarArquivoRemessaCnab400(List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            if (String.IsNullOrEmpty(boleto.SequencialNossoNumero) || String.IsNullOrEmpty(boleto.SequencialNossoNumero.TrimStart('0')))
                throw new Exception("Sequencial Nosso Número não foi informado.");

            try
            {
                if (boleto.CarteiraCobranca.Codigo == "CSB")
                {
                    string nossoNumeroComposto =
                        boleto.CedenteBoleto.CodigoCedente.PadLeft(5, '0')
                        +
                        boleto.SequencialNossoNumero.PadLeft(5, '0');

                    string digitoAutoConferenciaNossoNumero = Common.Mod11(nossoNumeroComposto, 7).ToString(CultureInfo.InvariantCulture);

                    string nossoNumeroFormatado =
                        nossoNumeroComposto + digitoAutoConferenciaNossoNumero;

                    boleto.SetNossoNumeroFormatado(nossoNumeroFormatado);
                    return;
                }
                if (boleto.CarteiraCobranca.Codigo == "CNR")
                {
                    /* Seguindo documentação CNR - Cobrança Não Registrada
                     * Disponível em: https://www.hsbc.com.br/1/PA_esf-ca-app-content/content/hbbr-pws-gip16/portugues/business/comum/pdf/cnrbarra.pdf
                     */

                    string codigoDoPagador = boleto.SequencialNossoNumero;
                    string primeiroDigitoVerificador =
                        CalculaPrimeiroDigitoVerificadorCnrTipo4(boleto.SequencialNossoNumero);
                    string segundoDigitoVerificador =
                        CalculaSegundoDigitoVerificadorCnrTipo4(boleto.SequencialNossoNumero,
                            primeiroDigitoVerificador, boleto.CedenteBoleto.CodigoCedente,
                            boleto.DataVencimento);

                    boleto.SetNossoNumeroFormatado(
                        String.Format("{0}{1}4{2}",
                            codigoDoPagador,
                            primeiroDigitoVerificador,
                            segundoDigitoVerificador));

                    /* Padroniza com 16 dígitos */
                    boleto.SetNossoNumeroFormatado(
                        boleto.NossoNumeroFormatado.PadLeft(16, '0'));
                    return;
                }

                throw new NotImplementedException("Modelo de carteira de cobrança: " + boleto.CarteiraCobranca.Codigo +
                                                  " não está implementado.");
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao tentar formatar nosso número.", ex);
            }
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {
            boleto.Moeda = MoedaBanco;

            var nossoNumeroLinhaDigitavel = boleto.NossoNumeroFormatado.PadLeft(13, '0');
            var codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0');
            var digitoAutoConferenciaNossoNumero = Common.Mod11(boleto.NossoNumeroFormatado, 7).ToString(CultureInfo.InvariantCulture);

            var C1 = string.Empty;
            var C2 = string.Empty;
            var C3 = string.Empty;
            var C5 = string.Empty;

            string AAA;
            string B;
            string CCCCC;
            string DD;
            string DDDDDD;
            string EEEE;
            string EEEEEEEE;
            string FFFFF;
            string FFFFFFF;
            string GGGGG;
            string HHHH;
            string IIIIIIIIII;
            string X;
            string Y;
            string Z;
            string W;

            if (boleto.CarteiraCobranca.Codigo == "CSB")
            {
                #region AAABC.CCCCX

                AAA = CodigoBanco.PadLeft(3, '0');
                B = boleto.Moeda;
                CCCCC = boleto.NossoNumeroFormatado.Substring(0, 5);
                X = Common.Mod10(AAA + B + CCCCC).ToString(CultureInfo.InvariantCulture);

                C1 = String.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
                C1 += String.Format("{0}{1} ", CCCCC.Substring(1, 4), X);

                #endregion

                #region DDDDD.DEEEEY

                // ReSharper disable once InconsistentNaming
                DDDDDD = boleto.NossoNumeroFormatado.Substring(5, 5) + digitoAutoConferenciaNossoNumero;
                // ReSharper disable once InconsistentNaming
                EEEE = boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0');
                Y = Common.Mod10(DDDDDD + EEEE).ToString(CultureInfo.InvariantCulture);

                C2 = String.Format("{0}.", DDDDDD.Substring(0, 5));
                C2 += string.Format("{0}{1}{2} ", DDDDDD.Substring(5, 1), EEEE, Y);

                #endregion

                #region FFFFF.FF001Z

                // ReSharper disable once InconsistentNaming
                FFFFFFF = boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(7, '0');
                Z = Common.Mod10(FFFFFFF + "001").ToString(CultureInfo.InvariantCulture);

                C3 = String.Format("{0}.", FFFFFFF.Substring(0, 5));
                C3 += String.Format("{0}001{1}", FFFFFFF.Substring(5, 2), Z);

                #endregion
            }
            if (boleto.CarteiraCobranca.Codigo == "CNR")
            {
                #region AAABC.CCCCX

                AAA = CodigoBanco.PadLeft(3, '0');
                B = boleto.Moeda;
                CCCCC = boleto.CedenteBoleto.CodigoCedente.Substring(0, 5);
                X = Common.Mod10(AAA + B + CCCCC).ToString(CultureInfo.InvariantCulture);

                C1 = string.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
                C1 += string.Format("{0}{1} ", CCCCC.Substring(1, 4), X);

                #endregion AAABC.CCDDX

                #region DDEEE.EEEEEY

                // ReSharper disable once InconsistentNaming
                DD = boleto.CedenteBoleto.CodigoCedente.Substring(5, 2);
                // ReSharper disable once InconsistentNaming
                EEEEEEEE = nossoNumeroLinhaDigitavel.Substring(0, 8);
                Y = Common.Mod10(DD + EEEEEEEE).ToString(CultureInfo.InvariantCulture);

                C2 = string.Format("{0}{1}.", DD, EEEEEEEE.Substring(0, 3));
                C2 += string.Format("{0}{1} ", EEEEEEEE.Substring(3, 5), Y);

                #endregion DDEEE.EEEEEY

                #region FFFFF.GGGGGZ

                // ReSharper disable once InconsistentNaming
                FFFFF = nossoNumeroLinhaDigitavel.Substring(8, 5);
                // ReSharper disable once InconsistentNaming
                GGGGG = (boleto.DataVencimento.DayOfYear + boleto.DataVencimento.ToString("yy").Substring(1, 1)).PadLeft(4,
                    '0') + "2";

                Z = Common.Mod10(FFFFF + GGGGG).ToString(CultureInfo.InvariantCulture);

                C3 = string.Format("{0}.", FFFFF);
                C3 += string.Format("{0}{1}", GGGGG, Z);

                #endregion FFFFF.GGGGGZ
            }

            // ReSharper disable once InconsistentNaming
            W = String.Format(" {0} ", _digitoAutoConferenciaCodigoBarras);

            #region HHHHIIIIIIIIII

            // ReSharper disable once InconsistentNaming
            HHHH = Common.FatorVencimento(boleto.DataVencimento).ToString(CultureInfo.InvariantCulture);
            // ReSharper disable once InconsistentNaming
            IIIIIIIIII = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");

            IIIIIIIIII = IIIIIIIIII.PadLeft(10, '0');
            C5 = HHHH + IIIIIIIIII;

            #endregion HHHHHHHHHHHHHH

            boleto.LinhaDigitavelBoleto = C1 + C2 + C3 + W + C5;
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            boleto.Moeda = MoedaBanco;

            try
            {
                /* Preenche com 0´s a esquerda
                 * 10 caracteres
                 */
                string valorBoletoTexto =
                    boleto.ValorBoleto.ToString("f")
                        .Replace(",", "")
                        .Replace(".", "")
                        .PadLeft(10, '0');

                string codigoBarraSemDigitoVerificador = null;

                if (boleto.CarteiraCobranca.Codigo == "CSB")
                {
                    codigoBarraSemDigitoVerificador =
                        String.Format("{0}{1}{2}{3}{4}{5}{6}001",
                            CodigoBanco,
                            boleto.Moeda,
                            //9999 --> 21/02/2025
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoletoTexto,
                            boleto.NossoNumeroFormatado + boleto.DigitoNossoNumero,
                            boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'),
                            boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(7, '0')
                            );
                }
                if (boleto.CarteiraCobranca.Codigo == "CNR")
                {
                    codigoBarraSemDigitoVerificador =
                        String.Format("{0}{1}{2}{3}{4}{5}{6}2",
                            CodigoBanco,
                            boleto.Moeda,
                            //9999 --> 21/02/2025
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoletoTexto,
                            boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0'),
                            boleto.SequencialNossoNumero.PadLeft(13, '0'),
                            (boleto.DataVencimento.DayOfYear +
                             boleto.DataVencimento.ToString("yy").Substring(1, 1)).PadLeft(4, '0')
                            );
                }

                /* 
                 * 1. Calcula dígito de auto conferência
                 * 2. Insere no meio do código de barras
                 * 3. Atribui ao boleto
                 */

                _digitoAutoConferenciaCodigoBarras = Common.Mod11(codigoBarraSemDigitoVerificador, 9, 0);

                var codigoBarraComDigitoVerificador = Common.Left(codigoBarraSemDigitoVerificador, 4) +
                                                         _digitoAutoConferenciaCodigoBarras +
                                                         Common.Right(codigoBarraSemDigitoVerificador, 39);

                boleto.CodigoBarraBoleto = codigoBarraComDigitoVerificador;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao formatar código de barras.", ex);
            }
        }

        public void FormatarBoleto(Boleto boleto)
        {
            //Atribui local de pagamento
            boleto.LocalPagamento = LocalDePagamento;

            boleto.ValidaDadosEssenciaisDoBoleto();

            FormataNumeroDocumento(boleto);
            FormataNossoNumero(boleto);
            // Calcula o DAC do Nosso N�mero
            // Nosso N�mero = Range(5) + Numero Sequencial(5)
            _digitoAutoConferenciaNossoNumero = Common.Mod11(boleto.NossoNumeroFormatado, 7).ToString(CultureInfo.InvariantCulture);
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataMoeda(boleto);

            ValidaBoletoComNormasBanco(boleto);
        }

        /// <summary>
        /// Calcula primeiro dígito verificador
        /// </summary>
        /// <param name="codigoPagador">Equivalente a número do documento.</param>
        /// <returns></returns>
        public string CalculaPrimeiroDigitoVerificadorCnrTipo4(string codigoPagador)
        {
            return Common.Mod11Base9(codigoPagador).ToString(CultureInfo.InvariantCulture);
        }

        public string CalculaSegundoDigitoVerificadorCnrTipo4(string codigoPagador, string primeiroDigitoVerificador,
            string codigoBeneficiario, DateTime dataVencimento)
        {
            return Common.Mod11Base9(
                (
                    long.Parse(codigoPagador + primeiroDigitoVerificador + "4") +
                    long.Parse(codigoBeneficiario) +
                    long.Parse(dataVencimento.ToString("ddMMyy"))
                    )
                    .ToString(CultureInfo.InvariantCulture)
                )
                .ToString(CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
