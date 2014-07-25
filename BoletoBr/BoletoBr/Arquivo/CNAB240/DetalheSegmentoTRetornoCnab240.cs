using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Dominio
{
    public class DetalheSegmentoTRetornoCnab240
    {
        #region Vari�veis

        private CodigoMovimento.CodigoMovimento _codigoMovimento;

        #endregion

        #region Construtores

        public DetalheSegmentoTRetornoCnab240() { }

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

        public int DacAgenciaConta { get; set; }

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

                if (MetodosExtensao.ExtrairValorDaLinha(registro, 14, 14) != "T")
                    throw new Exception("Registro inválido. O detalhe não possui as características do segmento T.");

                CodigoBanco = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 0, 3));
                IdCodigoMovimento = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 15, 17));
                Agencia = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 18, 22));
                DigitoAgencia = MetodosExtensao.ExtrairValorDaLinha(registro, 23, 23);
                Conta = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 24, 35));
                DigitoConta = MetodosExtensao.ExtrairValorDaLinha(registro, 36, 36);

                NossoNumero = MetodosExtensao.ExtrairValorDaLinha(registro, 37, 57);
                CodigoCarteira = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 58, 58));
                NumeroDocumento = MetodosExtensao.ExtrairValorDaLinha(registro, 59, 73);
                int dataVencimento = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 74, 81));
                DataVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-####"));
                decimal valorTitulo = Convert.ToInt64(MetodosExtensao.ExtrairValorDaLinha(registro, 82, 96));
                ValorTitulo = valorTitulo / 100;
                IdentificacaoTituloEmpresa = MetodosExtensao.ExtrairValorDaLinha(registro, 105, 130);
                TipoInscricao = Convert.ToInt32(MetodosExtensao.ExtrairValorDaLinha(registro, 132, 132));
                NumeroInscricao = MetodosExtensao.ExtrairValorDaLinha(registro, 133, 148);
                NomeSacado = MetodosExtensao.ExtrairValorDaLinha(registro, 149, 188);
                decimal valorTarifas = Convert.ToUInt64(MetodosExtensao.ExtrairValorDaLinha(registro, 198, 213));
                ValorTarifas = valorTarifas / 100;
                CodigoRejeicao = MetodosExtensao.ExtrairValorDaLinha(registro, 214, 223);
                UsoFebraban = MetodosExtensao.ExtrairValorDaLinha(registro, 224, 241);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO T.", ex);
            }
        }

        #endregion
    }
}