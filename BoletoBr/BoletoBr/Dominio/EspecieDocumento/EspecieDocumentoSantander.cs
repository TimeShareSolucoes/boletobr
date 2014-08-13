using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos.Santander;

namespace BoletoBr.Dominio.EspecieDocumento
{
    public enum EnumEspecieDocumento_Santander
    {
        DuplicataMercantil,
        DuplicataServico,
        LetraCambio353,
        LetraCambio008,
        NotaPromissoria,
        NotaPromissoriaRural,
        Recibo,
        ApoliceSeguro,
        Cheque,
        NotaPromissoariaDireta
        //02   DM - DUPLICATA MERCANTIL                 
        //04   DS - DUPLICATA DE SERVICO                
        //07	LC - LETRA DE C�MBIO (SOMENTE PARA BANCO 353)
        //30	LC - LETRA DE C�MBIO (SOMENTE PARA BANCO 008)
        //12   NP - NOTA PROMISSORIA                    
        //13	NR - NOTA PROMISSORIA RURAL 
        //17   RC - RECIBO                              
        //20   AP � APOLICE DE SEGURO                   
        //97	CH � CHEQUE
        //98	ND - NOTA PROMISSORIA DIRETA
    }

    public class EspecieDocumentoSantander : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumentoSantander()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumentoSantander(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Santander especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Santander.DuplicataMercantil: return "2";
                case EnumEspecieDocumento_Santander.DuplicataServico:return "4";
                case EnumEspecieDocumento_Santander.LetraCambio353: return "7";
                case EnumEspecieDocumento_Santander.LetraCambio008:return "30";
                case EnumEspecieDocumento_Santander.NotaPromissoria:return "12";
                case EnumEspecieDocumento_Santander.NotaPromissoriaRural:return "13";
                case EnumEspecieDocumento_Santander.Recibo: return "17";
                case EnumEspecieDocumento_Santander.ApoliceSeguro:return "20";
                case EnumEspecieDocumento_Santander.Cheque: return "97";
                case EnumEspecieDocumento_Santander.NotaPromissoariaDireta: return "98";
                default: return "2"; //Duplicata Mercantil

            }
        }

        public EnumEspecieDocumento_Santander getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "2": return EnumEspecieDocumento_Santander.DuplicataMercantil;
                case "4": return EnumEspecieDocumento_Santander.DuplicataServico;
                case "7": return EnumEspecieDocumento_Santander.LetraCambio353;
                case "30": return EnumEspecieDocumento_Santander.LetraCambio008;
                case "12": return EnumEspecieDocumento_Santander.NotaPromissoria;
                case "13": return EnumEspecieDocumento_Santander.NotaPromissoriaRural;
                case "17": return EnumEspecieDocumento_Santander.Recibo;
                case "20": return EnumEspecieDocumento_Santander.ApoliceSeguro;
                case "97": return EnumEspecieDocumento_Santander.Cheque;
                case "98": return EnumEspecieDocumento_Santander.NotaPromissoariaDireta;
                default: return EnumEspecieDocumento_Santander.DuplicataMercantil;
            }
        }

        private void Carregar(string idCodigo)
        {
            try
            {
                this.Banco = new BancoSantander();
                EspecieDocumentoSantander ed = new EspecieDocumentoSantander();
                switch (ed.getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Santander.DuplicataMercantil:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander.DuplicataMercantil);
                        this.Especie = "Duplicata Mercantil";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Santander.DuplicataServico:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander.DuplicataServico);
                        this.Especie = "Duplicata de Servi�o";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Santander.Recibo:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander.Recibo);
                        this.Especie = "Recibo";
                        this.Sigla = "R";
                        break;
                    case EnumEspecieDocumento_Santander.LetraCambio353:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander.LetraCambio353);
                        this.Especie = "Letra de C�mbio (Somente para o banco 353)";
                        this.Sigla = "LS";
                        break;
                    case EnumEspecieDocumento_Santander.LetraCambio008:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander.LetraCambio008);
                        this.Especie = "Letra de C�mbio (Somente para o banco 008)";
                        this.Sigla = "LS";
                        break;
                    case EnumEspecieDocumento_Santander.ApoliceSeguro:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander.ApoliceSeguro);
                        this.Especie = "Ap�lice de Seguro";
                        this.Sigla = "AP";
                        break;
                    case EnumEspecieDocumento_Santander.NotaPromissoariaDireta:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander.NotaPromissoariaDireta);
                        this.Especie = "Nota Promiss�ria Direta";
                        this.Sigla = "ND";
                        break;
                    case EnumEspecieDocumento_Santander.NotaPromissoria:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander.NotaPromissoria);
                        this.Especie = "Nota Promiss�ria";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Santander.NotaPromissoriaRural:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander.NotaPromissoriaRural);
                        this.Especie = "Nota Promiss�ria Rural";
                        this.Sigla = "NR";
                        break;
                    case EnumEspecieDocumento_Santander.Cheque:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander.Cheque);
                        this.Especie = "Cheque";
                        this.Sigla = "CH";
                        break;
                    default:
                        this.Codigo = "0";
                        this.Especie = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public static EspeciesDocumento CarregaTodas()
        {
            EspeciesDocumento especiesDocumento = new EspeciesDocumento();
            EspecieDocumentoSantander ed = new EspecieDocumentoSantander();
            foreach (EnumEspecieDocumento_Santander item in Enum.GetValues(typeof(EnumEspecieDocumento_Santander)))
                especiesDocumento.Add(new EspecieDocumentoSantander(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        #endregion
    }
}
