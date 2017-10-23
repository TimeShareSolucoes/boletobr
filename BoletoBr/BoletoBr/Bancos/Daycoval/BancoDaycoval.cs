using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;
using BoletoBr.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Daycoval
{
    public class BancoDaycoval : IBanco
    {
        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        public Image LogotipoBancoParaExibicao { get; set; }
        public string LocalDePagamento { get; }
        public string MoedaBanco { get; }

        private int _digitoAutoConferenciaBoleto;
        private int _digitoAutoConferenciaNossoNumero;

        public BancoDaycoval()
        {
            CodigoBanco = "707";
            DigitoBanco = "2";
            NomeBanco = "Banco Daycoval";
            LocalDePagamento = "Pagável em qualquer banco até o vencimento.";
            MoedaBanco = "9";
        }

        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            if (boleto.CarteiraCobranca.Codigo != "3" && boleto.CarteiraCobranca.Codigo != "4")
                throw new ValidacaoBoletoException(
                    "Carteira não implementada. Carteiras implementadas 3, 4.");

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
                Common.Mod11Peso2a9(boleto.IdentificadorInternoBoleto.BoletoBrToStringSafe());
            boleto.DigitoNossoNumero = _digitoAutoConferenciaNossoNumero.BoletoBrToStringSafe();

            FormataNumeroDocumento(boleto);
            FormataNossoNumero(boleto);
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);

            FormataMoeda(boleto);

            ValidaBoletoComNormasBanco(boleto);

            boleto.CedenteBoleto.CodigoCedenteFormatado = string.Format("{0}/{1}-{2}",
                boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(5, '0'),
                boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(9, '0'),
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
                DigitoBanco, boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(5, '0'),
                boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(9, '0'),
                boleto.IdentificadorInternoBoleto.PadLeft(9, '0'),
                "2");

            return formataCampoLivre;
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {
            //#region Campo 1

            //var BBB = boleto.CodigoBarraBoleto.Substring(0, 3);
            //var M = boleto.CodigoBarraBoleto.Substring(3, 1);
            //var CCCCC = boleto.CodigoBarraBoleto.Substring(19, 5);
            //var D1 = Common.Mod10(BBB + M + CCCCC).ToString(CultureInfo.InvariantCulture);

            //var Grupo1 = string.Format("{0}{1}{2}.{3}{4} ", BBB, M, CCCCC.Substring(0, 1), CCCCC.Substring(1, 4), D1);

            //#endregion

            //#region Campo 2

            //var CCCCCCCCCC2 = boleto.CodigoBarraBoleto.Substring(24, 10);
            //var D2 = Common.Mod10(CCCCCCCCCC2).ToString(CultureInfo.InvariantCulture);

            //var Grupo2 = string.Format("{0}.{1}{2} ", CCCCCCCCCC2.Substring(0, 5), CCCCCCCCCC2.Substring(5, 5), D2);

            //#endregion Campo 2

            //#region Campo 3

            //var CCCCCCCCCC3 = boleto.CodigoBarraBoleto.Substring(34, 10);
            //var D3 = Common.Mod10(CCCCCCCCCC3).ToString(CultureInfo.InvariantCulture);

            //var Grupo3 = string.Format("{0}.{1}{2} ", CCCCCCCCCC3.Substring(0, 5), CCCCCCCCCC3.Substring(5, 5), D3);

            //#endregion Campo 3

            //#region Campo 4

            //var D4 = _digitoAutoConferenciaBoleto.ToString(CultureInfo.InvariantCulture);

            //var Grupo4 = string.Format("{0} ", D4);

            //#endregion Campo 4

            //#region Campo 5

            //var FFFF = Common.FatorVencimento(boleto.DataVencimento).ToString(CultureInfo.InvariantCulture);
            //var VVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            //VVVVVVVVVV = VVVVVVVVVV.PadLeft(10, '0');

            //var Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

            //#endregion Campo 5

            //boleto.LinhaDigitavelBoleto = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;
            boleto.LinhaDigitavelBoleto = "";
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
                case 01:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 01,
                        Descricao = "ENTRADA CONFIRMADA NA CIP (*)"
                    };
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
                case 05:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 05,
                        Descricao = "CAMPO LIVRE ALTERADO"
                    };
                case 06:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 06,
                        Descricao = "LIQUIDAÇÃO NORMAL"
                    };
                case 08:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 08,
                        Descricao = "LIQUIDAÇÃO EM CARTÓRIO"
                    };
                case 09:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 09,
                        Descricao = "BAIXA AUTOMÁTICA"
                    };
                case 10:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 10,
                        Descricao = "BAIXA PÔR TER SIDO LIQUIDADO"
                    };
                case 12:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 12,
                        Descricao = "CONFIRMA ABATIMENTO"
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
                        Descricao = "BAIXA REJEITADA"
                    };
                case 16:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 16,
                        Descricao = "INSTRUÇÃO REJEITADA"
                    };
                case 19:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 19,
                        Descricao = "CONFIRMA RECEBIMENTO DE ORDEM DE PROTESTO"
                    };
                case 20:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 20,
                        Descricao = "CONFIRMA RECEBIMENTO DE ORDEM DE SUSTAÇÃO"
                    };
                case 22:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 22,
                        Descricao = "SEU NÚMERO ALTERADO"
                    };
                case 23:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 23,
                        Descricao = "TÍTULO ENVIADO A CARTÓRIO"
                    };
                case 24:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 24,
                        Descricao = "CONFIRMA RECEBIMENTO DE ORDEM DE NÃO PROTESTAR"
                    };
                case 28:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 28,
                        Descricao = "DÉBITO DE TARIFAS/CUSTAS – CORRESPONDENTES"
                    };
                case 40:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 40,
                        Descricao = "TARIFA DE ENTRADA (DEBITADA NA LIQUIDAÇÃO)"
                    };
                case 43:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 43,
                        Descricao = "BAIXADO POR TER SIDO PROTESTADO"
                    };
                case 96:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 96,
                        Descricao = "TARIFA SOBRE INSTRUÇÕES – MÊS ANTERIOR"
                    };
                case 97:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 97,
                        Descricao = "TARIFA SOBRE BAIXAS – MÊS ANTERIOR"
                    };
                case 98:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 98,
                        Descricao = "TARIFA SOBRE ENTRADAS – MÊS ANTERIOR"
                    };
                case 99:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 99,
                        Descricao = "TARIFA SOBRE INSTRUÇÃO DE PROTESTO/SUSTAÇÃO – MÊS ANTERIOR"
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
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeVencimento:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 06,
                        Descricao = "Alteração de vencimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.Protesto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 09,
                        Descricao = "Protestar"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.NaoProtestar:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 10,
                        Descricao = "Pedido de não protestar"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.NaoNegativarAutomaticamente:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 18,
                        Descricao = "Sustar protesto"
                    };
                }
            }
            throw new Exception(
                string.Format(
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
                        Descricao = "Duplicata",
                        Sigla = "DM"
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
                        Codigo = 12,
                        Descricao = "Duplicata de Serviço",
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

            /* Identifica o layout: 400 */
            if (linhasArquivo.First().Length == 400)
            {
                var leitor = new LeitorRetornoCnab400Daycoval(linhasArquivo);
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
    }
}
