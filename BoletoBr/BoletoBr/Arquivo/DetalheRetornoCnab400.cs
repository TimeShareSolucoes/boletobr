using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Dominio
{
    public class DetalheRetorno
    {

        #region Construtores

        public DetalheRetorno()
        {
            Sequencial = 0;
            DataLiquidacao = new DateTime(1, 1, 1);
            Abatimentos = 0;
            Juros = 0;
            OutrasDespesas = 0;
            DespesasDeCobranca = 0;
            EspecieTitulo = string.Empty;
            BancoCobrador = 0;
            CodigoRateio = 0;
            Instrucao = 0;
            SeuNumero = string.Empty;
            ContaCorrente = 0;
            CgcCpf = string.Empty;
            TipoInscricao = 0;
            IdentificacaoDoRegistro = 0;
            NumeroControle = string.Empty;
            NumeroCartorio = 0;
            NumeroProtocolo = string.Empty;
            MotivosRejeicao = string.Empty;
            IdentificacaoTitulo = string.Empty;
            OrigemPagamento = string.Empty;
            MotivoCodigoOcorrencia = string.Empty;
            ValorPago = 0;
            ValorOutrasDespesas = 0;
            ValorDespesa = 0;
            Registro = string.Empty;
            NumeroSequencial = 0;
            CodigoLiquidacao = string.Empty;
            Erros = string.Empty;
            NomeSacado = string.Empty;
            InstrucaoCancelada = 0;
            DataCredito = new DateTime(1, 1, 1);
            OutrosDebitos = 0;
            OutrosCreditos = 0;
            JurosMora = 0;
            ValorPrincipal = 0;
            Descontos = 0;
            ValorAbatimento = 0;
            Iof = 0;
            TarifaCobranca = 0;
            Especie = 0;
            DacAgenciaCobradora = 0;
            AgenciaCobradora = 0;
            CodigoBanco = 0;
            ValorTitulo = 0;
            DataVencimento = new DateTime(1, 1, 1);
            ConfirmacaoNossoNumero = 0;
            NumeroDocumento = string.Empty;
            DataOcorrencia = new DateTime(1, 1, 1);
            DescricaoOcorrencia = string.Empty;
            CodigoOcorrencia = 0;
            Carteira = string.Empty;
            DacNossoNumero = string.Empty;
            NossoNumero = string.Empty;
            UsoEmpresa = string.Empty;
            DacConta = 0;
            Conta = 0;
            Agencia = 0;
            NumeroInscricao = string.Empty;
            CodigoInscricao = 0;
        }

        public DetalheRetorno(string registro)
        {
            Sequencial = 0;
            DataLiquidacao = new DateTime(1, 1, 1);
            Abatimentos = 0;
            Juros = 0;
            OutrasDespesas = 0;
            DespesasDeCobranca = 0;
            EspecieTitulo = string.Empty;
            BancoCobrador = 0;
            CodigoRateio = 0;
            Instrucao = 0;
            SeuNumero = string.Empty;
            ContaCorrente = 0;
            CgcCpf = string.Empty;
            TipoInscricao = 0;
            IdentificacaoDoRegistro = 0;
            NumeroControle = string.Empty;
            NumeroCartorio = 0;
            NumeroProtocolo = string.Empty;
            MotivosRejeicao = string.Empty;
            IdentificacaoTitulo = string.Empty;
            OrigemPagamento = string.Empty;
            MotivoCodigoOcorrencia = string.Empty;
            ValorPago = 0;
            ValorOutrasDespesas = 0;
            ValorDespesa = 0;
            NumeroSequencial = 0;
            CodigoLiquidacao = string.Empty;
            Erros = string.Empty;
            NomeSacado = string.Empty;
            InstrucaoCancelada = 0;
            DataCredito = new DateTime(1, 1, 1);
            OutrosDebitos = 0;
            OutrosCreditos = 0;
            JurosMora = 0;
            ValorPrincipal = 0;
            Descontos = 0;
            ValorAbatimento = 0;
            Iof = 0;
            TarifaCobranca = 0;
            Especie = 0;
            DacAgenciaCobradora = 0;
            AgenciaCobradora = 0;
            CodigoBanco = 0;
            ValorTitulo = 0;
            DataVencimento = new DateTime(1, 1, 1);
            ConfirmacaoNossoNumero = 0;
            NumeroDocumento = string.Empty;
            DataOcorrencia = new DateTime(1, 1, 1);
            DescricaoOcorrencia = string.Empty;
            CodigoOcorrencia = 0;
            Carteira = string.Empty;
            DacNossoNumero = string.Empty;
            NossoNumero = string.Empty;
            UsoEmpresa = string.Empty;
            DacConta = 0;
            Conta = 0;
            Agencia = 0;
            NumeroInscricao = string.Empty;
            CodigoInscricao = 0;
            Registro = registro;
        }

        #endregion

        #region Propriedades

        public int CodigoInscricao { get; set; }

        public string NumeroInscricao { get; set; }

        /// <summary>
        /// Ag�ncia com o D�gito Verificador, quando houver
        /// </summary>
        public int Agencia { get; set; }

        public int Conta { get; set; }

        public int DacConta { get; set; }

        public string UsoEmpresa { get; set; }

        /// <summary>
        /// Nosso Numero Sem o DV
        /// </summary>
        public string NossoNumero { get; set; }

        /// <summary>
        /// DV do Nosso Numero
        /// </summary>
        public string DacNossoNumero { get; set; }

        /// <summary>
        /// Nosso Numero Completo Com o D�gito Verificador
        /// </summary>
        public string NossoNumeroComDV { get; set; }

        public string Carteira { get; set; }

        public int CodigoOcorrencia { get; set; }

        public string DescricaoOcorrencia { get; set; }

        public DateTime DataOcorrencia { get; set; }

        public string NumeroDocumento { get; set; }

        public int ConfirmacaoNossoNumero { get; set; }

        public DateTime DataVencimento { get; set; }

        public int CodigoBanco { get; set; }

        public int AgenciaCobradora { get; set; }

        public int DacAgenciaCobradora { get; set; }

        public int Especie { get; set; }

        public decimal TarifaCobranca { get; set; }

        public decimal Iof { get; set; }

        public decimal ValorAbatimento { get; set; }

        public decimal Descontos { get; set; }

        public decimal ValorPrincipal { get; set; }

        public decimal ValorTitulo { get; set; }

        public decimal JurosMora { get; set; }

        public decimal OutrosCreditos { get; set; }

        public decimal OutrosDebitos { get; set; }

        public DateTime DataCredito { get; set; }

        public int InstrucaoCancelada { get; set; }

        public string NomeSacado { get; set; }

        public string Erros { get; set; }

        public string CodigoLiquidacao { get; set; }

        public int NumeroSequencial { get; set; }

        public string Registro { get; private set; }

        public decimal ValorDespesa { get; set; }

        public decimal ValorOutrasDespesas { get; set; }

        public decimal ValorPago { get; set; }

        public string MotivoCodigoOcorrencia { get; set; }

        public string OrigemPagamento { get; set; }

        public string IdentificacaoTitulo { get; set; }

        public string MotivosRejeicao { get; set; }

        public string NumeroProtocolo { get; set; }

        public int NumeroCartorio { get; set; }

        public string NumeroControle { get; set; }

        public int IdentificacaoDoRegistro { get; set; }

        public int TipoInscricao { get; set; }

        public string CgcCpf { get; set; }

        public int ContaCorrente { get; set; }

        public string SeuNumero { get; set; }

        public int Instrucao { get; set; }

        public int CodigoRateio { get; set; }

        public int BancoCobrador { get; set; }

        public string EspecieTitulo { get; set; }

        public decimal DespesasDeCobranca { get; set; }

        public decimal OutrasDespesas { get; set; }

        public decimal Juros { get; set; }

        public decimal Abatimentos { get; set; }

        public DateTime DataLiquidacao { get; set; }

        public int Sequencial { get; set; }

        #endregion

        #region M�todos de Inst�ncia

        public void LerDetalheRetornoCnab400(string registro)
        {
            try
            {
                int dataOcorrencia = Convert.ToInt32(registro.Substring(110, 6));
                int dataVencimento = Convert.ToInt32(registro.Substring(146, 6));
                int dataCredito = Convert.ToInt32(registro.Substring(295, 6));

                CodigoInscricao = Convert.ToInt32(registro.Substring(1, 2));
                NumeroInscricao = registro.Substring(3, 14);
                Agencia = Convert.ToInt32(registro.Substring(17, 4));
                Conta = Convert.ToInt32(registro.Substring(23, 5));
                DacConta = Convert.ToInt32(registro.Substring(28, 1));
                UsoEmpresa = registro.Substring(37, 25);
                NossoNumero = Convert.ToString(registro.Substring(85, 8));
                DacNossoNumero = registro.Substring(93, 1);
                Carteira = registro.Substring(107, 1);
                CodigoOcorrencia = Convert.ToInt32(registro.Substring(108, 2));
                DataOcorrencia = Convert.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                NumeroDocumento = registro.Substring(116, 10);
                NossoNumero = Convert.ToString(registro.Substring(126, 9));
                DataVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-##"));
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                ValorTitulo = valorTitulo/100;
                CodigoBanco = Convert.ToInt32(registro.Substring(165, 3));
                AgenciaCobradora = Convert.ToInt32(registro.Substring(168, 4));
                Especie = Convert.ToInt32(registro.Substring(173, 2));
                decimal tarifaCobranca = Convert.ToUInt64(registro.Substring(175, 13));
                TarifaCobranca = tarifaCobranca/100;
                // 26 brancos
                decimal iof = Convert.ToUInt64(registro.Substring(214, 13));
                Iof = iof/100;
                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                ValorAbatimento = valorAbatimento/100;
                decimal valorPrincipal = Convert.ToUInt64(registro.Substring(253, 13));
                ValorPrincipal = valorPrincipal/100;
                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                JurosMora = jurosMora/100;
                DataOcorrencia = Convert.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                // 293 - 3 brancos
                DataCredito = Convert.ToDateTime(dataCredito.ToString("##-##-##"));
                InstrucaoCancelada = Convert.ToInt32(registro.Substring(301, 4));
                // 306 - 6 brancos
                // 311 - 13 zeros
                NomeSacado = registro.Substring(324, 30);
                // 354 - 23 brancos
                Erros = registro.Substring(377, 8);
                // 377 - Registros rejeitados ou alega��o do sacado
                // 386 - 7 brancos

                CodigoLiquidacao = registro.Substring(392, 2);
                NumeroSequencial = Convert.ToInt32(registro.Substring(394, 6));

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public static string PrimeiroCaracter(string retorno)
        {
            try
            {
                return retorno.Substring(0, 1);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao desmembrar registro.", ex);
            }
        }
        #endregion
    }
}
