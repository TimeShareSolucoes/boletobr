using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.AccessControl;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.CalculoModulo;
using BoletoBr.Bancos;
using BoletoBr.Dominio;
using BoletoBr.Dominio.CodigoMovimento;
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
        private readonly List<CarteiraCobranca> _carteirasCobrancaHsbc;

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
            this.LocalDePagamento = "Pagável em qualquer banco até o vencimento.";
            this.MoedaBanco = "9";
        }

        #endregion

        #region Métodos de formatação do boleto

        public string LocalDePagamento { get; private set; }
        public string MoedaBanco { get; private set; }
        
        public List<CarteiraCobranca> GetCarteirasCobranca()
        {
            return _carteirasCobrancaHsbc;
        }

        public CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira)
        {
            return GetCarteirasCobranca().Find(fd => fd.Codigo == codigoCarteira);
        }

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
            if (Convert.ToInt32(boleto.NossoNumeroFormatado).ToString().Length > 10)
                throw new NotImplementedException("A quantidade de dígitos do nosso número para a carteira " +
                                                  boleto.CarteiraCobranca.Codigo + ", são 10 números.");
            else if (Convert.ToInt32(boleto.NossoNumeroFormatado).ToString().Length < 10)
                boleto.SetNossoNumeroFormatado(boleto.NossoNumeroFormatado.PadLeft(10, '0'));

            //Verifica se data do documento � valida
            //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataDocumento == DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;
        }

        public void FormataMoeda(Boleto boleto)
        {
            boleto.Moeda = this.MoedaBanco;

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
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 15,
                        QtdDias = (int) valorInstrucao,
                        TextoInstrucao = "Multa de " + valorInstrucao + " por cento após dia " + dataInstrucao
                    };
                }
                case EnumTipoInstrucao.MultaPorDiaVencimento:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 16,
                        QtdDias = (int) valorInstrucao,
                        TextoInstrucao =
                            "Após " + dataInstrucao + " multa dia de " + valorInstrucao + "  máximo " + "???"
                    };
                }
                case EnumTipoInstrucao.MultaPorDiaCorrido:
                {
                    return new InstrucaoPadronizada()
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
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 20,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Cobrar juros só após 07 dias do vencimento."
                    };
                }
                case EnumTipoInstrucao.MultaPorDiaUtil:
                {
                    return new InstrucaoPadronizada()
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
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 23,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber após o vencimento."
                    };
                }
                case EnumTipoInstrucao.MultaVencimento:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 24,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Multa de R$ " + valorInstrucao + " após o vencimento."
                    };
                }
                case EnumTipoInstrucao.JurosSoAposData:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 29,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Juros só após " + dataInstrucao + ", cobrar desde o vencimento."
                    };
                }
                case EnumTipoInstrucao.ConcederAbatimento:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 34,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Conceder abatimento conforme proposto pelo pagador."
                    };
                }
                case EnumTipoInstrucao.AposVencimentoMulta10PorCento:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 36,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Após vencimento multa de 10 por cento."
                    };
                }
                case EnumTipoInstrucao.ConcederDescontoPagoAposVencimento:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 40,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Conceder desconto mesmo se pago após o vencimento."
                    };
                }
                case EnumTipoInstrucao.NaoReceberAntesDoVencimento:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 42,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber antes do vencimento."
                    };
                }
                case EnumTipoInstrucao.AposVencimentoMulta20PorCentoMaisMora:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 53,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Após vencimento multa de 20% mais mora de 1% a.m."
                    };
                }
                case EnumTipoInstrucao.NaoReceberAntesdoVencimentoOu10DiasApos:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 56,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber antes do vencimento ou 10 dias após."
                    };
                }
                //case EnumTipoInstrucao.AbatimentoDesconto:
                //{
                //    return new InstrucaoPadronizada()
                //    {
                //        Codigo = 65,
                //        QtdDias = diasInstrucao,
                //        Valor = valorInstrucao,
                //        TextoInstrucao = "Abatimento/Desconto só com instrução do benefiário"
                //    };
                //}
                case EnumTipoInstrucao.TituloSujeitoAProtestoAposVencimento:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 67,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Título sujeito a protesto após o vencimento."
                    };
                }
                case EnumTipoInstrucao.AposVencimentoMulta2PorCento:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 68,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Após o vencimento multa de 2 por cento."
                    };
                }
                case EnumTipoInstrucao.NaoReceberAposNDiasCorridos:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 71,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber após " + diasInstrucao + " dias corridos do vencimento."
                    };
                }
                case EnumTipoInstrucao.NaoReceberAposNDiasUteis:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 72,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Não receber após " + diasInstrucao + " dias úteis do vencimento."
                    };
                }
                case EnumTipoInstrucao.MultaDeVPorCentoAposNDiasCorridos:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 73,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Multa de " + valorInstrucao + " por cento após " + diasInstrucao + " dias corridos do vencimento."
                    };
                }
                case EnumTipoInstrucao.MultaDeVPorCentoAposNDiasUteis:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 74,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Multa de " + valorInstrucao + " por cento após " + diasInstrucao + " dias úteis do vencimento."
                    };
                }
                case EnumTipoInstrucao.ProtestarAposNDiasCorridos:
                {
                    return new InstrucaoPadronizada()
                    {
                        Codigo = 75,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "Protestar " + diasInstrucao + " dias corridos após o vencimento, se não pago."
                    };
                }
                case EnumTipoInstrucao.ProtestarAposNDiasUteis:
                {
                    return new InstrucaoPadronizada()
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
                    return new InstrucaoPadronizada()
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
                    return new InstrucaoPadronizada()
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

        public ICodigoOcorrencia ObtemCodigoOcorrencia(CodigoOcorrenciaRemessa ocorrenciaRemessa, double valorOcorrencia, DateTime dataOcorrencia)
        {
            switch (ocorrenciaRemessa)
            {
                case CodigoOcorrenciaRemessa.Registro:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 01,
                        Descricao = "Entrada de títulos"
                    };
                }
                case CodigoOcorrenciaRemessa.Baixa:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 02,
                        Descricao = "Pedido de baixa"
                    };
                }
                case CodigoOcorrenciaRemessa.ConcessaoDeAbatimento:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 04,
                        Descricao = "Concessão de abatimento"
                    };
                }
                case CodigoOcorrenciaRemessa.CancelamentoDeAbatimento:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 05,
                        Descricao = "Cancelamento de abatimento concedido"
                    };
                }
                case CodigoOcorrenciaRemessa.AlteracaoDeVencimento:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 06,
                        Descricao = "Alteração de vencimento"
                    };
                }
                case CodigoOcorrenciaRemessa.ConcessaoDeDesconto:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 07,
                        Descricao = "Conceder desconto"
                    };
                }
                case CodigoOcorrenciaRemessa.AlteracaoDoControleDoParticipante:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 07,
                        Descricao = "Alteração do controle do participante"
                    };
                }
                case CodigoOcorrenciaRemessa.CancelamentoDeDesconto:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 08,
                        Descricao = "Cancelamento de desconto"
                    };
                }
                case CodigoOcorrenciaRemessa.AlteracaoSeuNumero:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 08,
                        Descricao = "Alteração do seu número"
                    };
                }
                case CodigoOcorrenciaRemessa.Protesto:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 09,
                        Descricao = "Protestar"
                    };
                }
                case CodigoOcorrenciaRemessa.SustarProtesto:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 10,
                        Descricao = "Sustar protesto"
                    };
                }
                case CodigoOcorrenciaRemessa.NaoCobrarJurosDeMora:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 11,
                        Descricao = "Não cobrar juros de mora"
                    };
                }
                case CodigoOcorrenciaRemessa.ConcessaoDeDescontoComData:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 13,
                        Descricao = "Conceder desconto R$ " + string.Format("{0:0.##}", valorOcorrencia) + " p/ pgto até " + dataOcorrencia
                    };
                }
                case CodigoOcorrenciaRemessa.CancelamentoDescontoFixo:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 14,
                        Descricao = "Cancelamento condição de desconto fixo"
                    };
                }
                case CodigoOcorrenciaRemessa.CancelamentoDescontoDiario:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 15,
                        Descricao = "Cancelamento de desconto diário"
                    };
                }
                case CodigoOcorrenciaRemessa.AlteracaoDeVencimentoComData:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 48,
                        Descricao = "Vencimento alterado para " + dataOcorrencia
                    };
                }
                case CodigoOcorrenciaRemessa.AlteracaoDeDiasParaEnvioACartorio:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 49,
                        Descricao = "Alteração de dias para envio a Cartório de Protesto"
                    };
                }
                case CodigoOcorrenciaRemessa.InclusaoDePagadorNoBoleto:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 50,
                        Descricao = "Inclusão de pagador no boleto eletrônico"
                    };
                }
                case CodigoOcorrenciaRemessa.ExclusaoDePagadorNoBoleto:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 51,
                        Descricao = "Exclusão de pagador no boleto eletrônico"
                    };
                }
                case CodigoOcorrenciaRemessa.Reemissao:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 52,
                        Descricao = "Reemissão"
                    };
                }
                case CodigoOcorrenciaRemessa.EntradaDeTitulosComParcelasFaltantes:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 53,
                        Descricao = "Entrada de títulos com parcelas faltantes"
                    };
                }
                case CodigoOcorrenciaRemessa.TransferenciaParaDesconto:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 55,
                        Descricao = "Transferência para desconto"
                    };
                }
                case CodigoOcorrenciaRemessa.ProtestoParaFinsFalimentares:
                {
                    return new CodigoOcorrencia()
                    {
                        Codigo = 57,
                        Descricao = "Protesto para fins falimentares"
                    };
                }
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter Código de Comando/Movimento/Ocorrência. Banco: {0} Código: {1}",
                    CodigoBanco, ocorrenciaRemessa.ToString()));
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
            try
            {
                if (boleto.CarteiraCobranca.Codigo == "CSB")
                {
                    string nossoNumeroComposto =
                        boleto.CedenteBoleto.CodigoCedente.PadLeft(5, '0')
                        +
                        boleto.SequencialNossoNumero.PadLeft(5, '0');

                    string digitoAutoConferenciaNossoNumero = Common.Mod11(nossoNumeroComposto, 7).ToString();

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
            boleto.Moeda = this.MoedaBanco;

            string nossoNumeroLinhaDigitavel = boleto.NossoNumeroFormatado.PadLeft(13, '0');
            string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0');
            string digitoAutoConferenciaNossoNumero = Common.Mod11(boleto.NossoNumeroFormatado, 7).ToString();

            string C1 = string.Empty;
            string C2 = string.Empty;
            string C3 = string.Empty;
            string C5 = string.Empty;

            string AAA;
            string B;
            string CCCCC;
            string X;


            string DDDDDD;
            string DD;
            string EEEE;
            string EEEEEEEE;
            string Y;
            string FFFFFFF;
            string FFFFF;
            string GGGGG;
            string Z;

            if (boleto.CarteiraCobranca.Codigo == "CSB")
            {
                #region AAABC.CCCCX

                AAA = this.CodigoBanco.PadLeft(3, '0');
                B = boleto.Moeda.ToString();
                CCCCC = boleto.NossoNumeroFormatado.Substring(0, 5);
                X = Common.Mod10(AAA + B + CCCCC).ToString();

                C1 = String.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
                C1 += String.Format("{0}{1} ", CCCCC.Substring(1, 4), X);

                #endregion

                #region DDDDD.DEEEEY

                DDDDDD = boleto.NossoNumeroFormatado.Substring(5, 5) + digitoAutoConferenciaNossoNumero;
                EEEE = boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0');
                Y = Common.Mod10(DDDDDD + EEEE).ToString();

                C2 = String.Format("{0}.", DDDDDD.Substring(0, 5));
                C2 += string.Format("{0}{1}{2} ", DDDDDD.Substring(5, 1), EEEE, Y);

                #endregion

                #region FFFFF.FF001Z

                FFFFFFF = boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(7, '0');
                Z = Common.Mod10(FFFFFFF + "001").ToString();

                C3 = String.Format("{0}.", FFFFFFF.Substring(0, 5));
                C3 += String.Format("{0}001{1}", FFFFFFF.Substring(5, 2), Z);

                #endregion
            }
            if (boleto.CarteiraCobranca.Codigo == "CNR")
            {
                #region AAABC.CCCCX

                AAA = this.CodigoBanco.PadLeft(3, '0');
                B = boleto.Moeda.ToString();
                CCCCC = boleto.CedenteBoleto.CodigoCedente.Substring(0, 5);
                X = Common.Mod10(AAA + B + CCCCC).ToString();

                C1 = string.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
                C1 += string.Format("{0}{1} ", CCCCC.Substring(1, 4), X);

                #endregion AAABC.CCDDX

                #region DDEEE.EEEEEY

                DD = boleto.CedenteBoleto.CodigoCedente.Substring(5, 2);
                EEEEEEEE = nossoNumeroLinhaDigitavel.Substring(0, 8);
                Y = Common.Mod10(DD + EEEEEEEE).ToString();

                C2 = string.Format("{0}{1}.", DD, EEEEEEEE.Substring(0, 3));
                C2 += string.Format("{0}{1} ", EEEEEEEE.Substring(3, 5), Y);

                #endregion DDEEE.EEEEEY

                #region FFFFF.GGGGGZ

                FFFFF = nossoNumeroLinhaDigitavel.Substring(8, 5);
                GGGGG =
                    (boleto.DataVencimento.DayOfYear + boleto.DataVencimento.ToString("yy").Substring(1, 1)).PadLeft(4,
                        '0') + "2";

                Z = Common.Mod10(FFFFF + GGGGG).ToString();

                C3 = string.Format("{0}.", FFFFF);
                C3 += string.Format("{0}{1}", GGGGG, Z);

                #endregion FFFFF.GGGGGZ
            }

            string W = String.Format(" {0} ", _digitoAutoConferenciaCodigoBarras);

            #region HHHHIIIIIIIIII

            string HHHH = Common.FatorVencimento(boleto.DataVencimento).ToString();
            string IIIIIIIIII = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");

            IIIIIIIIII = IIIIIIIIII.PadLeft(10, '0');
            C5 = HHHH + IIIIIIIIII;

            #endregion HHHHHHHHHHHHHH

            boleto.LinhaDigitavelBoleto = C1 + C2 + C3 + W + C5;
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            boleto.Moeda = this.MoedaBanco;

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

                string numeroDocumentoFormatado =
                    boleto.NumeroDocumento.PadLeft(7, '0');

                string codigoBarraSemDigitoVerificador = null;

                if (boleto.CarteiraCobranca.Codigo == "CSB")
                {
                    codigoBarraSemDigitoVerificador =
                        String.Format("{0}{1}{2}{3}{4}{5}{6}001",
                            this.CodigoBanco,
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
                            this.CodigoBanco,
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
                string codigoBarraComDigitoVerificador = null;

                _digitoAutoConferenciaCodigoBarras = Common.Mod11(codigoBarraSemDigitoVerificador, 9, 0);

                codigoBarraComDigitoVerificador =
                    Common.Left(codigoBarraSemDigitoVerificador, 4) +
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
            boleto.LocalPagamento = this.LocalDePagamento;

            boleto.ValidaDadosEssenciaisDoBoleto();

            FormataNumeroDocumento(boleto);
            FormataNossoNumero(boleto);
            // Calcula o DAC do Nosso N�mero
            // Nosso N�mero = Range(5) + Numero Sequencial(5)
            _digitoAutoConferenciaNossoNumero = Common.Mod11(boleto.NossoNumeroFormatado, 7).ToString();
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
            return Common.Mod11Base9(codigoPagador).ToString();
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
                    .ToString()
                )
                .ToString();
        }

        #endregion

        public void LerArquivoRetorno(IBanco banco, Stream arquivo)
        {
            throw new NotImplementedException();
        }

        public string GerarHeaderRemessa(string numeroConvenio, Cedente cendente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        public string GerarHeaderRemessa(string numeroConvenio, Cedente cendente, TipoArquivo tipoArquivo, int numeroArquivoRemessa,
            Boleto boletos)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
        }

        public string GerarHeaderRemessa(Cedente cendente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        public string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            throw new NotImplementedException();
        }

        public string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cendente, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        public string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cendente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
        }

        public string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cendente, int numeroArquivoRemessa, TipoArquivo tipoArquivo,
            Boleto boletos)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente,
            Boleto boletos)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, Sacado sacado)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
        }

        public string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            throw new NotImplementedException();
        }

        public string GerarTrailerArquivoRemessa(int numeroRegistro, Boleto boletos)
        {
            throw new NotImplementedException();
        }

        public string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            throw new NotImplementedException();
        }

        public string GerarTrailerLoteRemessa(int numeroRegistro, Boleto boletos)
        {
            throw new NotImplementedException();
        }

        public DetalheSegmentoTRetornoCnab240 LerDetalheSegmentoTRetornoCnab240(string registro)
        {
            throw new NotImplementedException();
        }

        public DetalheSegmentoURetornoCnab240 LerDetalheSegmentoURetornoCnab240(string registro)
        {
            throw new NotImplementedException();
        }

        public DetalheSegmentoWRetornoCnab240 LerDetalheSegmentoWRetornoCnab240(string registro)
        {
            throw new NotImplementedException();
        }

        public DetalheRetornoGenericoCnab400 LerDetalheRetornoCnab400(string registro)
        {
            throw new NotImplementedException();
        }

        public Cedente Cedente { get; private set; }
        public int Codigo { get; set; }
        public string Nome { get; private set; }
        public string Digito { get; private set; }
    }
}
