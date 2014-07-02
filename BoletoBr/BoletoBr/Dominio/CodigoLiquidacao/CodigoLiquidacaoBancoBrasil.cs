using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos.BancoBrasil;

namespace BoletoBr.Dominio.CodigoLiquidacao
{
 // C�digos de liquida��o de 1 a 13 associados aos c�digos de movimento 06, 09 e 17

    #region Enumerado

    public enum EnumCodigoLiquidacaoBancoBrasil
    {
        PorSaldo = 1,
        PorConta = 2,
        NoProprioBanco = 3,
        CompensacaoEletronica = 4,
        CompensacaoConvencional = 5,
        PorMeioEletronico = 6,
        AposFeriadoLocal = 7,
        EmCartorio = 8,
        ComandadaBanco = 9,
        ComandadaClienteArquivo = 10,
        ComandadaClienteOnline = 11,
        DecursoDePrazoCliente = 12,
        DecursoDePrazoBanco = 13,        
    }

    #endregion 

    public class CodigoLiquidacaoBancoBrasil: AbstractCodigoLiquidacao, ICodigoLiquidacao
    {
        #region Construtores 

		public CodigoLiquidacaoBancoBrasil()
		{
			try
			{
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public CodigoLiquidacaoBancoBrasil(int codigo)
        {
            try
            {
                this.Carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

		#endregion 

        #region Metodos Privados

        private void Carregar(int idCodigo)
        {
            try
            {
                this.Banco = new BancoBrasil();

                switch ((EnumCodigoLiquidacaoBancoBrasil)idCodigo)
                {
                    case EnumCodigoLiquidacaoBancoBrasil.PorSaldo:
                        this.Enumerado = (int)EnumCodigoLiquidacaoBancoBrasil.PorSaldo;
                        this.Codigo = "";
                        this.Descricao = "Por saldo";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacaoBancoBrasil.PorConta:
                        this.Enumerado = (int)EnumCodigoLiquidacaoBancoBrasil.PorConta;
                        this.Codigo = "";
                        this.Descricao = "Por conta";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacaoBancoBrasil.NoProprioBanco:
                        this.Enumerado = (int)EnumCodigoLiquidacaoBancoBrasil.NoProprioBanco;
                        this.Codigo = "";
                        this.Descricao = "No pr�prio banco";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacaoBancoBrasil.CompensacaoEletronica:
                        this.Enumerado = (int)EnumCodigoLiquidacaoBancoBrasil.CompensacaoEletronica;
                        this.Codigo = "";
                        this.Descricao = "Compensa��o eletr�nica";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacaoBancoBrasil.CompensacaoConvencional:
                        this.Enumerado = (int)EnumCodigoLiquidacaoBancoBrasil.CompensacaoConvencional;
                        this.Codigo = "";
                        this.Descricao = "Compensa��o convencional";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacaoBancoBrasil.PorMeioEletronico:
                        this.Enumerado = (int)EnumCodigoLiquidacaoBancoBrasil.PorMeioEletronico;
                        this.Codigo = "";
                        this.Descricao = "Por meio eletr�nico";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacaoBancoBrasil.AposFeriadoLocal:
                        this.Enumerado = (int)EnumCodigoLiquidacaoBancoBrasil.AposFeriadoLocal;
                        this.Codigo = "";
                        this.Descricao = "Ap�s feriado nacional";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacaoBancoBrasil.EmCartorio:
                        this.Enumerado = (int)EnumCodigoLiquidacaoBancoBrasil.EmCartorio;
                        this.Codigo = "";
                        this.Descricao = "Em cart�rio";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacaoBancoBrasil.ComandadaBanco:
                        this.Enumerado = (int)EnumCodigoLiquidacaoBancoBrasil.ComandadaBanco;
                        this.Codigo = "";
                        this.Descricao = "Comandada banco";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacaoBancoBrasil.ComandadaClienteArquivo:
                        this.Enumerado = (int)EnumCodigoLiquidacaoBancoBrasil.ComandadaClienteArquivo;
                        this.Codigo = "";
                        this.Descricao = "Comandada cliente - arquivo";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacaoBancoBrasil.ComandadaClienteOnline:
                        this.Enumerado = (int)EnumCodigoLiquidacaoBancoBrasil.ComandadaClienteOnline;
                        this.Codigo = "";
                        this.Descricao = "Comandada cliente - online";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacaoBancoBrasil.DecursoDePrazoCliente:
                        this.Enumerado = (int)EnumCodigoLiquidacaoBancoBrasil.DecursoDePrazoCliente;
                        this.Codigo = "";
                        this.Descricao = "Decurso de prazo - cliente";
                        this.Recurso = "";
                        break;
                    case EnumCodigoLiquidacaoBancoBrasil.DecursoDePrazoBanco:
                        this.Enumerado = (int)EnumCodigoLiquidacaoBancoBrasil.DecursoDePrazoBanco;
                        this.Codigo = "";
                        this.Descricao = "Decurso de prazo - banco";
                        this.Recurso = "";
                        break;
                    default:
                        this.Enumerado = 0;
                        this.Codigo = "";
                        this.Descricao = "( Selecione )";
                        this.Recurso = "";
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