using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos.BancoBrasil;

namespace BoletoBr.Dominio.CodigoMovimento
{
    public enum EnumCodigoMovimentoBancoBrasil
    {
        EntradaConfirmada = 2,
        EntradaRejeitada = 3,
        TransferenciaCarteiraEntrada = 4,
        TransferenciaCarteiraBaixa = 5,
        Liquidacao = 6,
        Baixa = 9,
        TitulosCarteiraEmSer = 11,
        ConfirmacaoRecebimentoInstrucaoAbatimento = 12,
        ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento = 13,
        ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento = 14,
        FrancoPagamento = 15,
        LiquidacaoAposBaixa = 17,
        ConfirmacaoRecebimentoInstrucaoProtesto = 19,
        ConfirmacaoRecebimentoInstrucaoSustacaoProtesto = 20,
        RemessaCartorio = 23,
        RetiradaCartorioManutencaoCarteira = 24,
        ProtestadoBaixado = 25,
        InstrucaoRejeitada = 26,
        ConfirmacaoPedidoAlteracaoOutrosDados = 27,
        DebitoTarifas = 28,
        OcorrenciaSacado = 29,
        AlteracaoDadosRejeitada = 30,
    }

    public partial class CodigoMovimentoBancoBrasil : AbstractCodigoMovimento, ICodigoMovimento
    {
        #region Construtores 

        public CodigoMovimentoBancoBrasil(int codigo)
        {
            try
            {
                this.carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

		#endregion 

        #region Metodos Privados

        private void carregar(int codigo)
        {
            try
            {
                this.Banco = new BancoBrasil();

                switch ((EnumCodigoMovimentoBancoBrasil)codigo)
                {
                    case  EnumCodigoMovimentoBancoBrasil.EntradaConfirmada:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.EntradaConfirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.EntradaRejeitada:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.EntradaRejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.TransferenciaCarteiraEntrada:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.TransferenciaCarteiraEntrada;
                        this.Descricao = "Transfer�ncia de carteira/entrada";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.TransferenciaCarteiraBaixa:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.TransferenciaCarteiraBaixa;
                        this.Descricao = "Transfer�ncia de carteira/baixa";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.Liquidacao:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.Liquidacao;
                        this.Descricao = "Liquida��o normal";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.Baixa:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.TitulosCarteiraEmSer:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.TitulosCarteiraEmSer;
                        this.Descricao = "T�tulos em carteira em ser";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoAbatimento:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoAbatimento;
                        this.Descricao = "Confirma��o recebimento instru��o de abatimento";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento;
                        this.Descricao = "Confirma��o recebimento instru��o de cancelamento de abatimento";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento;
                        this.Descricao = "Confirma��o recebimento instru��o altera��o de vencimento";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.FrancoPagamento:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.FrancoPagamento;
                        this.Descricao = "Franco pagamento";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.LiquidacaoAposBaixa:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.LiquidacaoAposBaixa;
                        this.Descricao = "Liquida��o ap�s baixa";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoProtesto:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoProtesto;
                        this.Descricao = "Confirma��o de recebimento de instru��o de protesto";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto;
                        this.Descricao = "Confirma��o de recebimento de instru��o de susta��o de protesto";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.RemessaCartorio:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.RemessaCartorio;
                        this.Descricao = "Remessa a cart�rio/aponte em cart�rio";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.RetiradaCartorioManutencaoCarteira:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.RetiradaCartorioManutencaoCarteira;
                        this.Descricao = "Retirada de cart�rio e manuten��o em carteira";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.ProtestadoBaixado:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.ProtestadoBaixado;
                        this.Descricao = "Protestado e baixado/baixa por ter sido protestado";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.InstrucaoRejeitada:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.InstrucaoRejeitada;
                        this.Descricao = "Instru��o rejeitada";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.ConfirmacaoPedidoAlteracaoOutrosDados:
                        this.Codigo = (int) EnumCodigoMovimentoBancoBrasil.ConfirmacaoPedidoAlteracaoOutrosDados;
                        this.Descricao = "Confirma��o do pedido de altera��o de outros dados";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.DebitoTarifas:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.DebitoTarifas;
                        this.Descricao = "Debito de tarifas/custas";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.OcorrenciaSacado:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.OcorrenciaSacado;
                        this.Descricao = "Ocorrencias do sacado";
                        break;
                    case EnumCodigoMovimentoBancoBrasil.AlteracaoDadosRejeitada:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.AlteracaoDadosRejeitada;
                        this.Descricao = "Altera��o de dados rejeitada";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void Ler(int codigo)
        {
            try
            {
                switch (codigo)
                {
                    case 2:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.EntradaConfirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case 3:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.EntradaRejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case 4:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.TransferenciaCarteiraEntrada;
                        this.Descricao = "Transfer�ncia de carteira/entrada";
                        break;
                    case 5:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.TransferenciaCarteiraBaixa;
                        this.Descricao = "Transfer�ncia de carteira/baixa";
                        break;
                    case 6:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.Liquidacao;
                        this.Descricao = "Liquida��o";
                        break;
                    case 9:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case 11:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.TitulosCarteiraEmSer;
                        this.Descricao = "T�tulos em carteira em ser";
                        break;
                    case 12:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoAbatimento;
                        this.Descricao = "Confirma��o recebimento instru��o de abatimento";
                        break;
                    case 13:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento;
                        this.Descricao = "Confirma��o recebimento instru��o de cancelamento de abatimento";
                        break;
                    case 14:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento;
                        this.Descricao = "Confirma��o recebimento instru��o altera��o de vencimento";
                        break;
                    case 15:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.FrancoPagamento;
                        this.Descricao = "Franco pagamento";
                        break;
                    case 17:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.LiquidacaoAposBaixa;
                        this.Descricao = "Liquida��o ap�s baixa";
                        break;
                    case 19:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoProtesto;
                        this.Descricao = "Confirma��o de recebimento de instru��o de protesto";
                        break;
                    case 20:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto;
                        this.Descricao = "Confirma��o de recebimento de instru��o de susta��o de protesto";
                        break;
                    case 23:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.RemessaCartorio;
                        this.Descricao = "Remessa a cart�rio/aponte em cart�rio";
                        break;
                    case 24:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.RetiradaCartorioManutencaoCarteira;
                        this.Descricao = "Retirada de cart�rio e manuten��o em carteira";
                        break;
                    case 25:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.ProtestadoBaixado;
                        this.Descricao = "Protestado e baixado/baixa por ter sido protestado";
                        break;
                    case 26:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.InstrucaoRejeitada;
                        this.Descricao = "Instru��o rejeitada";
                        break;
                    case 27:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.ConfirmacaoPedidoAlteracaoOutrosDados;
                        this.Descricao = "Confirma��o do pedido de altera��o de outros dados";
                        break;
                    case 28:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.DebitoTarifas;
                        this.Descricao = "Debito de tarifas/custas";
                        break;
                    case 29:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.OcorrenciaSacado;
                        this.Descricao = "Ocorrencias do sacado";
                        break;
                    case 30:
                        this.Codigo = (int)EnumCodigoMovimentoBancoBrasil.AlteracaoDadosRejeitada;
                        this.Descricao = "Altera��o de dados rejeitada";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }


        #endregion
    }
}
