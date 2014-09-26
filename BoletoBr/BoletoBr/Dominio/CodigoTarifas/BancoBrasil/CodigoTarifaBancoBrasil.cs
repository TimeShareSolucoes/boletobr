using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Dominio.CodigoTarifas.BancoBrasil
{
    // C�digos de tarifas de 1 a 11 associados ao c�digo de movimento 28

    #region Enumerado

    public enum EnumCodigoTarifasBancoBrasil
    {
        TarifaDeExtratoDePosicao = 1,
        TarifaDeManutencaoTituloVencido = 2,
        TarifaDeSustacao = 3,
        TarifaDeProtesto = 4,
        TarifaDeOutrasInstrucoes = 5,
        TarifaDeOutrasOcorrencias = 6,
        TarifaDeEnvioDeDuplicataAoSacado = 7,
        CustasDeProtesto = 8,
        CustasDeSustacaoDeProtesto = 9,
        CustasDoCartorioDistribuidor = 10,
        CustasDeEdital = 11,
    }

    #endregion

    public class CodigoTarifasBancoBrasil : AbstractCodigoTarifas, ICodigoTarifas
    {
        #region Construtores

        public CodigoTarifasBancoBrasil()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoTarifasBancoBrasil(int codigo)
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

        private void Carregar(int codigo)
        {
            try
            {
                this.Banco = new Bancos.Brasil.BancoBrasil();

                switch ((EnumCodigoTarifasBancoBrasil)codigo)
                {
                    case EnumCodigoTarifasBancoBrasil.TarifaDeExtratoDePosicao:
                        this.Codigo = (int)EnumCodigoTarifasBancoBrasil.TarifaDeExtratoDePosicao;
                        this.Descricao = "Tarifa de extrato de posi��o";
                        break;
                    case EnumCodigoTarifasBancoBrasil.TarifaDeManutencaoTituloVencido:
                        this.Codigo = (int)EnumCodigoTarifasBancoBrasil.TarifaDeManutencaoTituloVencido;
                        this.Descricao = "Tarifa de manuten��o de t�tulo vencido";
                        break;
                    case EnumCodigoTarifasBancoBrasil.TarifaDeSustacao:
                        this.Codigo = (int)EnumCodigoTarifasBancoBrasil.TarifaDeSustacao;
                        this.Descricao = "Tarifa de susta��o";
                        break;
                    case EnumCodigoTarifasBancoBrasil.TarifaDeProtesto:
                        this.Codigo = (int)EnumCodigoTarifasBancoBrasil.TarifaDeProtesto;
                        this.Descricao = "Tarifa de protesto";
                        break;
                    case EnumCodigoTarifasBancoBrasil.TarifaDeOutrasInstrucoes:
                        this.Codigo = (int)EnumCodigoTarifasBancoBrasil.TarifaDeOutrasInstrucoes;
                        this.Descricao = "Tarifa de outras instru��es";
                        break;
                    case EnumCodigoTarifasBancoBrasil.TarifaDeOutrasOcorrencias:
                        this.Codigo = (int)EnumCodigoTarifasBancoBrasil.TarifaDeOutrasOcorrencias;
                        this.Descricao = "Tarifa de outras ocorr�ncias";
                        break;
                    case EnumCodigoTarifasBancoBrasil.TarifaDeEnvioDeDuplicataAoSacado:
                        this.Codigo = (int)EnumCodigoTarifasBancoBrasil.TarifaDeEnvioDeDuplicataAoSacado;
                        this.Descricao = "Tarifa de envio de duplicata ao sacado";
                        break;
                    case EnumCodigoTarifasBancoBrasil.CustasDeProtesto:
                        this.Codigo = (int)EnumCodigoTarifasBancoBrasil.CustasDeProtesto;
                        this.Descricao = "Custas de protesto";
                        break;
                    case EnumCodigoTarifasBancoBrasil.CustasDeSustacaoDeProtesto:
                        this.Codigo = (int)EnumCodigoTarifasBancoBrasil.CustasDeSustacaoDeProtesto;
                        this.Descricao = "Custas de susta��o de protesto";
                        break;
                    case EnumCodigoTarifasBancoBrasil.CustasDoCartorioDistribuidor:
                        this.Codigo = (int)EnumCodigoTarifasBancoBrasil.CustasDoCartorioDistribuidor;
                        this.Descricao = "Custas do cart�rio distribuidor";
                        break;
                    case EnumCodigoTarifasBancoBrasil.CustasDeEdital:
                        this.Codigo = (int)EnumCodigoTarifasBancoBrasil.CustasDeEdital;
                        this.Descricao = "Custas de edital";
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


