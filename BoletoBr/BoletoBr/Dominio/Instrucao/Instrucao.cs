using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.Instrucao
{
    public class Instrucao : AbstractInstrucao, IInstrucao
    {

        #region Variaveis

        private IInstrucao _IInstrucao;

        #endregion

        #region Construtores

        internal Instrucao()
        {
        }

        public Instrucao(int codigoBanco)
        {
            try
            {
                InstanciaInstrucao(codigoBanco);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        #endregion

        # region M�todos Privados

        private void InstanciaInstrucao(int codigoBanco)
        {
            try
            {
                switch (codigoBanco)
                {
                    //1 - Banco do Brasil
                    case 1:
                        _IInstrucao = new InstrucaoBancoBrasil();
                        break;
                    //104 - Caixa
                    case 104:
                        _IInstrucao = new Instrucao_Caixa();
                        break;
                    //237 - Bradesco
                    case 237:
                        _IInstrucao = new Instrucao_Bradesco();
                        break;
                    //341 - Ita�
                    case 341:
                        _IInstrucao = new Instrucao_Itau();
                        break;
                    //399 - HSBC
                    case 399:
                        _IInstrucao = new Instrucao_HSBC();
                        break;
                    default:
                        throw new Exception("C�digo do banco n�o implementando: " + codigoBanco);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execu��o da transa��o.", ex);
            }
        }

        # endregion

        #region Propriedades da interface

        public override IBanco Banco
        {
            get { return _IInstrucao.Banco; }
            set { _IInstrucao.Banco = value; }
        }

        public override int Codigo
        {
            get { return _IInstrucao.Codigo; }
            set { _IInstrucao.Codigo = value; }
        }

        public override string Descricao
        {
            get { return _IInstrucao.Descricao; }
            set { _IInstrucao.Descricao = value; }
        }

        public override int QuantidadeDias
        {
            get { return _IInstrucao.QuantidadeDias; }
            set { _IInstrucao.QuantidadeDias = value; }
        }

        #endregion

        #region M�todos de interface

        public override void Valida()
        {
            try
            {
                //_IInstrucao.Valida();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a validação dos campos.", ex);
            }
        }

        #endregion

    }
}
