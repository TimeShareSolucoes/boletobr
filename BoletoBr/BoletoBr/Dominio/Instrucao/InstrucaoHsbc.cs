using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;
using BoletoBr.Bancos.Hsbc;

namespace BoletoBr.Dominio.Instrucao
{
    #region Enumerado

    public enum EnumInstrucoes_HSBC
    {
    }

    #endregion

    class Instrucao_HSBC : AbstractInstrucao, IInstrucao
    {

        #region Construtores
        public Instrucao_HSBC()
        {
            try
            {
                this.Banco = new BancoHsbc() {CodigoBanco = "399", DigitoBanco = null, NomeBanco = null};
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }
        public Instrucao_HSBC(IBanco banco, int codigo)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_HSBC(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_HSBC(EnumInstrucoes_HSBC codigo)
        {
            this.carregar((int)codigo, 0);
        }

        public Instrucao_HSBC(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias);
        }
        #endregion

        #region Metodos Privados

        private void carregar(int idInstrucao, int nrDias)
        {
            try
            {
                this.Banco = new BancoHsbc();
                this.Valida();

                switch ((EnumInstrucoes_HSBC)idInstrucao)
                {
                    //case EnumInstrucoes_Bradesco.Protestar:
                    //    this.Codigo = (int)EnumInstrucoes_Bradesco.Protestar;
                    //    this.Descricao = "Protestar";
                    //    break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "";
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
