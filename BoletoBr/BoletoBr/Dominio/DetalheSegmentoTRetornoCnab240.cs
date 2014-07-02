using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Dominio
{
    class DetalheSegmentoTRetornoCnab240
    {
#region Vari�veis

        private CodigoMovimento.CodigoMovimento _codigoMovimento;

        #endregion

        #region Construtores

        public DetalheSegmentoTRetornoCnab240(string registro)
        {
            ListaDetalhe = new List<DetalheSegmentoTRetornoCnab240>();
            Registro = registro;
        }

        #endregion

        #region Propriedades

        public int IdCodigoMovimento { get; set; }

        public int CodigoBanco { get; set; }

        public string Registro { get; private set; }

        public CodigoMovimento.CodigoMovimento CodigoMovimento
        {
            get 
            {
                _codigoMovimento = new CodigoMovimento.CodigoMovimento(CodigoBanco, IdCodigoMovimento); 
                return _codigoMovimento;
            }
            set 
            { 
                _codigoMovimento = value;
                IdCodigoMovimento = _codigoMovimento.Codigo;
            }
        }

        public int Agencia { get; set; }

        public string DigitoAgencia { get; set; }

        public long Conta { get; set; }

        public string DigitoConta { get; set; }

        public int DACAgenciaConta { get; set; }

        public string NossoNumero { get; set; }

        public int CodigoCarteira { get; set; }

        public string NumeroDocumento { get; set; }

        public DateTime DataVencimento { get; set; }

        public decimal ValorTitulo { get; set; }

        public string IdentificacaoTituloEmpresa { get; set; }

        public int TipoInscricao { get; set; }

        public string NumeroInscricao { get; set; }

        public string NomeSacado { get; set; }

        public decimal ValorTarifas { get; set; }

        public string CodigoRejeicao { get; set; }

        public List<DetalheSegmentoTRetornoCnab240> ListaDetalhe { get; set; }

        public string UsoFebraban { get; set; }

        #endregion

        #region M�todos de Inst�ncia

        public void LerDetalheSegmentoTRetornoCnab240(string registro)
        {
            try
            {
                Registro = registro;

                if (registro.Substring(13, 1) != "T")
                    throw new Exception("Registro inv�lido. O detalhe n�o possu� as caracter�sticas do segmento T.");

                CodigoBanco = Convert.ToInt32(registro.Substring(0, 3));
                IdCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                Agencia = Convert.ToInt32(registro.Substring(17, 5));
                DigitoAgencia = registro.Substring(22, 1);
                Conta = Convert.ToInt32(registro.Substring(23, 12));
                DigitoConta = registro.Substring(35, 1);

                NossoNumero = registro.Substring(37, 20);
                CodigoCarteira = Convert.ToInt32(registro.Substring(57, 1));
                NumeroDocumento = registro.Substring(58, 15);
                int dataVencimento = Convert.ToInt32(registro.Substring(73, 8));
                DataVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-####"));
                decimal valorTitulo = Convert.ToInt64(registro.Substring(81, 15));
                ValorTitulo = valorTitulo / 100;
                IdentificacaoTituloEmpresa = registro.Substring(105, 25);
                TipoInscricao = Convert.ToInt32(registro.Substring(132, 1));
                NumeroInscricao = registro.Substring(133, 15);
                NomeSacado = registro.Substring(148, 40);
                decimal valorTarifas = Convert.ToUInt64(registro.Substring(198, 15));
                ValorTarifas = valorTarifas / 100;
                CodigoRejeicao = registro.Substring(213, 10);
                UsoFebraban = registro.Substring(224, 17);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO T.", ex);
            }
        }

        #endregion
    }
}