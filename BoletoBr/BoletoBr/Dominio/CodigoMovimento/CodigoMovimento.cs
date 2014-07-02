using System;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.CodigoMovimento
{
    public class CodigoMovimento : AbstractCodigoMovimento, ICodigoMovimento
    {

        #region Variaveis

        private ICodigoMovimento _iCodigoMovimento;

        #endregion

        # region Construtores

        internal CodigoMovimento()
        {
        }

        public CodigoMovimento(int codigoBanco, int codigoMovimento)
        {
            try
            {
                InstanciaCodigoMovimento(codigoBanco, codigoMovimento);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        # endregion

        #region Propriedades da interface

        public override IBanco Banco
        {
            get { return _iCodigoMovimento.Banco; }
        }

        public override int Codigo
        {
            get { return _iCodigoMovimento.Codigo; }
        }

        public override string Descricao
        {
            get { return _iCodigoMovimento.Descricao; }
        }

        #endregion

        # region M�todos Privados

        private void InstanciaCodigoMovimento(int codigoBanco, int codigoMovimento)
        {
            try
            {
                switch (codigoBanco)
                {

                    // Caixa
                    case 104:
                        _iCodigoMovimento = new CodigoMovimentoCaixa(codigoMovimento);
                        break;
                    //341 - Ita�
                    case 341:
                        //_ICodigoMovimento = new CodigoMovimento_Itau();
                        throw new Exception("C�digo do banco n�o implementando: " + codigoBanco);
                    //1 - Banco do Brasil
                    case 1:
                        _iCodigoMovimento = new CodigoMovimentoBancoBrasil(codigoMovimento);
                        break;
                    //356 - Real
                    case 356:
                        //_ICodigoMovimento = new CodigoMovimento_Real();
                        throw new Exception("C�digo do banco n�o implementando: " + codigoBanco);
                    //422 - Safra
                    case 422:
                        //_ICodigoMovimento = new CodigoMovimento_Safra();
                        throw new Exception("C�digo do banco n�o implementando: " + codigoBanco);
                    //237 - Bradesco
                    case 237:
                        //_ICodigoMovimento = new CodigoMovimento_Bradesco();
                        throw new Exception("C�digo do banco n�o implementando: " + codigoBanco);
                    //347 - Sudameris
                    case 347:
                        //_ICodigoMovimento = new CodigoMovimento_Sudameris();
                        throw new Exception("C�digo do banco n�o implementando: " + codigoBanco);
                    //353 - Santander
                    case 353:
                        //_ICodigoMovimento = new CodigoMovimento_Santander();
                        throw new Exception("C�digo do banco n�o implementando: " + codigoBanco);
                    //070 - BRB
                    case 70:
                        //_ICodigoMovimento = new CodigoMovimento_BRB();
                        throw new Exception("C�digo do banco n�o implementando: " + codigoBanco);
                    //479 - BankBoston
                    case 479:
                        //_ICodigoMovimento = new CodigoMovimento_BankBoston();
                        throw new Exception("C�digo do banco n�o implementando: " + codigoBanco);
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

    }
}
