using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.CalculoModulo;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;
using BoletoBr.Interfaces;
using System.Linq;

namespace BoletoBr.Bancos.Safra
{
    public class BancoSafra : IBanco
    {
        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        public Image LogotipoBancoParaExibicao { get; set; }
        public string LocalDePagamento { get; }
        public string MoedaBanco { get; }

        private int _digitoAutoConferenciaBoleto;
        private int _digitoAutoConferenciaNossoNumero;

        public BancoSafra()
        {
            CodigoBanco = "422";
            DigitoBanco = "7";
            NomeBanco = "Banco Safra";
            LocalDePagamento = "Pagável em qualquer banco até o vencimento.";
            MoedaBanco = "9";
        }

        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            //Verifica se data do processamento é valida
            if (boleto.DataProcessamento == DateTime.MinValue)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento é valida
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

        public void FormatarBoleto(Boleto boleto)
        {
            if (boleto.IdentificadorInternoBoleto.Length > 8)
                throw new ValidacaoBoletoException(
                    "Tamanho máximo para o campo Identificador interno do boleto é de 8.");

            if (boleto.IdentificadorInternoBoleto.Length < 8)
                boleto.IdentificadorInternoBoleto = boleto.IdentificadorInternoBoleto.PadLeft(8, '0');

            //Atribui o local de pagamento
            boleto.LocalPagamento = LocalDePagamento;

            boleto.ValidaDadosEssenciaisDoBoleto();

            // Calcula o DAC do Nosso Número
            _digitoAutoConferenciaNossoNumero =
                Common.Mod11Base9Safra(boleto.IdentificadorInternoBoleto.BoletoBrToStringSafe());
            boleto.DigitoNossoNumero = _digitoAutoConferenciaNossoNumero.BoletoBrToStringSafe();

            FormataNumeroDocumento(boleto);
            FormataNossoNumero(boleto);
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);

            FormataMoeda(boleto);

            ValidaBoletoComNormasBanco(boleto);

            boleto.CedenteBoleto.CodigoCedenteFormatado = string.Format("{0}/{1}{2}",
                boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadRight(5, '0'),
                boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(8, '0'),
                boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            var valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = valorBoleto.PadLeft(10, '0');

            boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}", CodigoBanco, boleto.Moeda,
                Common.FatorVencimento(boleto.DataVencimento), valorBoleto, FormataCampoLivre(boleto));

            _digitoAutoConferenciaBoleto = Common.Mod11(boleto.CodigoBarraBoleto, 9);

            boleto.CodigoBarraBoleto = Common.Left(boleto.CodigoBarraBoleto, 4) + _digitoAutoConferenciaBoleto +
                                       Common.Right(boleto.CodigoBarraBoleto, 39);
        }

        public string FormataCampoLivre(Boleto boleto)
        {
            string formataCampoLivre = string.Format("{0}{1}{2}{3}{4}",
                DigitoBanco, boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadRight(5, '0'),
                (boleto.CedenteBoleto.ContaBancariaCedente.Conta+ boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta).PadLeft(9, '0'),
                boleto.NossoNumeroFormatado.PadLeft(9, '0'),
                "2");

            return formataCampoLivre;
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {
            #region Campo 1

            var BBB = boleto.CodigoBarraBoleto.Substring(0, 3);
            var M = boleto.CodigoBarraBoleto.Substring(3, 1);
            var CCCCC = boleto.CodigoBarraBoleto.Substring(19, 5);
            var D1 = Common.Mod10(BBB + M + CCCCC).ToString(CultureInfo.InvariantCulture);

            var Grupo1 = string.Format("{0}{1}{2}.{3}{4} ", BBB, M, CCCCC.Substring(0, 1), CCCCC.Substring(1, 4), D1);

            #endregion

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

            var FFFF = Common.FatorVencimento(boleto.DataVencimento).ToString(CultureInfo.InvariantCulture);
            var VVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            VVVVVVVVVV = VVVVVVVVVV.PadLeft(10, '0');

            var Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

            #endregion Campo 5

            boleto.LinhaDigitavelBoleto = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            boleto.SetNossoNumeroFormatado(boleto.IdentificadorInternoBoleto);

            boleto.SetNossoNumeroFormatado(
                string.Format("{0}{1}",
                    boleto.NossoNumeroFormatado.PadLeft(8, '0'),
                    boleto.DigitoNossoNumero));
        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            if (string.IsNullOrEmpty(boleto.NumeroDocumento) ||
                string.IsNullOrEmpty(boleto.NumeroDocumento.TrimStart('0')))
                throw new Exception("Número do Documento não foi informado.");

            boleto.NumeroDocumento = boleto.NumeroDocumento.PadLeft(8, '0');
        }

        public ICodigoOcorrencia ObtemCodigoOcorrenciaByInt(int numeroOcorrencia)
        {
            switch (numeroOcorrencia)
            {
                case 02:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 02,
                        Descricao = "ENTRADA CONFIRMADA"
                    };
                case 03:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 03,
                        Descricao = "ENTRADA REJEITADA"
                    };
                case 04:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 04,
                        Descricao = "TRANSFERÊNCIA DE CARTEIRA (ENTRADA)"
                    };
                case 05:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 05,
                        Descricao = "TRANSFERÊNCIA DE CARTEIRA (BAIXA)"
                    };
                case 06:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 06,
                        Descricao = "LIQUIDAÇÃO NORMAL"
                    };
                case 09:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 09,
                        Descricao = "BAIXADO AUTOMATICAMENTE"
                    };
                case 10:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 10,
                        Descricao = "BAIXADO CONFORME INSTRUÇÕES"
                    };
                case 11:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 11,
                        Descricao = "TÍTULOS EM SER (PARA ARQUIVO MENSAL)"
                    };
                case 12:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 12,
                        Descricao = "ABATIMENTO CONCEDIDO"
                    };
                case 13:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 13,
                        Descricao = "ABATIMENTO CANCELADO"
                    };
                case 14:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 14,
                        Descricao = "VENCIMENTO ALTERADO"
                    };
                case 15:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 15,
                        Descricao = "LIQUIDAÇÃO EM CARTÓRIO"
                    };
                case 19:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 19,
                        Descricao = "CONFIRMAÇÃO DE INSTRUÇÃO DE PROTESTO"
                    };
                case 20:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 20,
                        Descricao = "CONFIRMAÇÃO DE SUSTAR PROTESTO"
                    };
                case 21:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 21,
                        Descricao = "TRANSFERÊNCIA DE BENEFICIÁRIO"
                    };
                case 23:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 23,
                        Descricao = "TÍTULO ENVIADO A CARTÓRIO"
                    };
                case 40:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 40,
                        Descricao = "BAIXA DE TÍTULO PROTESTADO"
                    };
                case 41:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 41,
                        Descricao = "LIQUIDAÇÃO DE TÍTULO BAIXADO"
                    };
                case 42:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 42,
                        Descricao = "TÍTULO RETIRADO DO CARTÓRIO"
                    };
                case 43:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 43,
                        Descricao = "DESPESA DE CARTÓRIO"
                    };
                case 44:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 44,
                        Descricao = "ACEITE DO TÍTULO DDA PELO PAGADOR"
                    };
                case 45:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 45,
                        Descricao = "NÃO ACEITE DO TÍTULO DDA PELO PAGADOR"
                    };
                case 51:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 51,
                        Descricao = "VALOR DO TÍTULO ALTERADO"
                    };
                case 52:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 52,
                        Descricao = "ACERTO DE DATA DE EMISSAO"
                    };
                case 53:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 53,
                        Descricao = "ACERTO DE COD ESPECIE DOCTO"
                    };
                case 54:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 54,
                        Descricao = "ALTERACAO DE SEU NUMERO"
                    };
                case 56:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 56,
                        Descricao = "INSTRUÇÃO NEGATIVAÇÃO ACEITA"
                    };
                case 57:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 57,
                        Descricao = "INSTRUÇÃO BAIXA DE NEGATIVAÇÃO ACEITA"
                    };
                case 58:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 58,
                        Descricao = "INSTRUÇÃO NÃO NEGATIVAR ACEITA"
                    };
                default:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = numeroOcorrencia,
                        Descricao = "Código de ocorrência não encontrado, n° ".ToUpper() + numeroOcorrencia
                    };
            }
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
                case EnumCodigoOcorrenciaRemessa.NaoProtestar:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 10,
                        Descricao = "Não Protestar"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.NaoCobrarJurosDeMora:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 11,
                        Descricao = "Não Cobrar Juros de Mora"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CobrarJurosdeMora:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 16,
                        Descricao = "Cobrar Juros de Mora"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoValorTitulo:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 31,
                        Descricao = "(*) Alteração do Valor do Título"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.Negativar:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 90,
                        Descricao = "NEGATIVAR"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.BaixaNegativacao:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 91,
                        Descricao = "BAIXA DE NEGATIVAÇÃO"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.NaoNegativarAutomaticamente:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 92,
                        Descricao = "NÃO NEGATIVAR AUTOMATICAMENTE"
                    };
                }
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter Código de Comando/Movimento/Ocorrência. Banco: {0} Código: {1}",
                    CodigoBanco, ocorrencia.ToString()));
        }

        public ICodigoOcorrencia ObtemCodigoOcorrencia(EnumCodigoOcorrenciaRetorno ocorrenciaRetorno)
        {
            throw new NotImplementedException();
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
                        Descricao = "DUPLICATA MERCANTIL",
                        Sigla = "DM"
                    };
                }
                case EnumEspecieDocumento.NotaPromissoria:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 02,
                        Descricao = "NOTA PROMISSÓRIA",
                        Sigla = "NP"
                    };
                }
                case EnumEspecieDocumento.NotaDeSeguro:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 03,
                        Descricao = "NOTA DE SEGURO",
                        Sigla = "NS"
                    };
                }
                case EnumEspecieDocumento.Recibo:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 05,
                        Descricao = "RECIBO",
                        Sigla = "RC"
                    };
                }
                case EnumEspecieDocumento.DuplicataServico:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 09,
                        Descricao = "DUPLICATA DE SERVIÇOS",
                        Sigla = "DS"
                    };
                }
            }

            throw new Exception(
                String.Format("Não foi possível obter instrução padronizada. Banco: {0} Código Espécie: {1}",
                    CodigoBanco, especie.ToString()));
        }

        public IInstrucao ObtemInstrucaoPadronizada(EnumTipoInstrucao tipoInstrucao, double valorInstrucao,
            DateTime dataInstrucao,
            int diasInstrucao)
        {
            switch (tipoInstrucao)
            {
                case EnumTipoInstrucao.NaoReceberPrincipalSemJurosdeMora:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 01,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "NÃO RECEBER PRINCIPAL, SEM JUROS DE MORA"
                    };
                }
                case EnumTipoInstrucao.DevolverSenaoPagoAte15DiasAposVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 02,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "DEVOLVER, SE NÃO PAGO, ATÉ 15 DIAS APÓS O VENCIMENTO"
                    };
                }
                case EnumTipoInstrucao.DevolverSenaoPagoAte30DiasAposVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 03,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "DEVOLVER, SE NÃO PAGO, ATÉ 30 DIAS APÓS O VENCIMENTO"
                    };
                }
                case EnumTipoInstrucao.NaoProtestar:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 07,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "NÃO PROTESTAR"
                    };
                }
                case EnumTipoInstrucao.NaoCobrarJurosDeMora:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 08,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "NÃO COBRAR JUROS DE MORA"
                    };
                }
                case EnumTipoInstrucao.MultaVencimento:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 16,
                        QtdDias = diasInstrucao,
                        Valor = valorInstrucao,
                        TextoInstrucao = "MULTA"
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
                throw new ApplicationException("Arquivo informado é inválido/Não existem títulos no retorno.");

            /* Identifica o layout: 400 */
            if (linhasArquivo.First().Length == 400)
            {
                var leitor = new LeitorRetornoCnab400Safra(linhasArquivo);
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
        public int CodigoJurosMora(CodigoJurosMora codigoJurosMora)
        {
            return 0;
        }

        public int CodigoProteso(bool protestar = true)
        {
            return 0;
        }
    }
}
