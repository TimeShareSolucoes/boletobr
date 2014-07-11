using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos.BancoBrasil;
using BoletoBr.Bancos.Bradesco;

namespace BoletoBr.Dominio.Instrucao
{
    #region Enumerado

    public enum EnumInstrucoes_Bradesco
    {
        Protestar = 9,
        NaoProtestar = 10,
        ProtestoFinsFalimentares = 42,
        ProtestarAposNDiasCorridos = 81,
        ProtestarAposNDiasUteis = 82,
        NaoReceberAposNDias = 91,
        DevolverAposNDias = 92,

        OutrasInstrucoes_ExibeMensagem_MoraDiaria = 900,
        OutrasInstrucoes_ExibeMensagem_MultaVencimento = 901
    }

    #endregion

    public class Instrucao_Bradesco : AbstractInstrucao, IInstrucao
    {

        #region Construtores

        public Instrucao_Bradesco()
        {
            try
            {
                this.Banco = new BancoBradesco() {CodigoBanco = "237", DigitoBanco = null, NomeBanco = null};
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Bradesco(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_Bradesco(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias);
        }
        public Instrucao_Bradesco(int codigo, double valor)
        {
            this.carregar(codigo, valor);
        }
        #endregion Construtores

        #region Metodos Privados

        private void carregar(int idInstrucao, double valor)
        {
            try
            {
                this.Banco = new BancoBradesco();
                this.Valida();

                switch ((EnumInstrucoes_Bradesco)idInstrucao)
                {
                    case EnumInstrucoes_Bradesco.OutrasInstrucoes_ExibeMensagem_MoraDiaria:
                        this.Codigo = 0;
                        this.Descricao = "Ap�s vencimento cobrar mora di�ria de R$ " + valor;
                        break;
                    case EnumInstrucoes_Bradesco.OutrasInstrucoes_ExibeMensagem_MultaVencimento:
                        this.Codigo = 0;
                        this.Descricao = "Ap�s vencimento cobrar multa de " + valor + "%";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = " (Selecione) ";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void carregar(int idInstrucao, int nrDias)
        {
            try
            {
                this.Banco = new BancoBradesco();
                this.Valida();

                switch ((EnumInstrucoes_Bradesco)idInstrucao)
                {
                    case EnumInstrucoes_Bradesco.Protestar:
                        this.Codigo = (int)EnumInstrucoes_Bradesco.Protestar;
                        this.Descricao = "Protestar";
                        break;
                    case EnumInstrucoes_Bradesco.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_Bradesco.NaoProtestar;
                        this.Descricao = "N�o protestar";
                        break;
                    case EnumInstrucoes_Bradesco.ProtestoFinsFalimentares:
                        this.Codigo = (int)EnumInstrucoes_Bradesco.ProtestoFinsFalimentares;
                        this.Descricao = "Protesto para fins falimentares";
                        break;
                    case EnumInstrucoes_Bradesco.ProtestarAposNDiasCorridos:
                        this.Codigo = (int)EnumInstrucoes_Bradesco.ProtestarAposNDiasCorridos;
                        this.Descricao = "Protestar ap�s " + nrDias + " dias corridos do vencimento";
                        break;
                    case EnumInstrucoes_Bradesco.ProtestarAposNDiasUteis:
                        this.Codigo = (int)EnumInstrucoes_Bradesco.ProtestarAposNDiasUteis;
                        this.Descricao = "Protestar ap�s " + nrDias + " dias �teis do vencimento";
                        break;
                    case EnumInstrucoes_Bradesco.NaoReceberAposNDias:
                        this.Codigo = (int)EnumInstrucoes_Bradesco.NaoReceberAposNDias;
                        this.Descricao = "N�o receber ap�s " + nrDias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_Bradesco.DevolverAposNDias:
                        this.Codigo = (int)EnumInstrucoes_Bradesco.DevolverAposNDias;
                        this.Descricao = "Devolver ap�s " + nrDias + " dias do vencimento";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = " (Selecione) ";
                        break;
                }

                this.QuantidadeDias = nrDias;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public override void Valida()
        {
            //base.Valida();
        }

        #endregion

    }
}
