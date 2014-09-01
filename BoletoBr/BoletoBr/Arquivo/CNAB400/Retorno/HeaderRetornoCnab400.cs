using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BoletoBr
{
    public class HeaderRetornoCnab400
    {
        #region Propriedades Comuns

        public int CodigoDoRegistro { get; set; }
        public int CodigoDeRetorno { get; set; }
        public string LiteralRetorno { get; set; }
        public string CodigoDoServico { get; set; }
        public string LiteralServico { get; set; }
        public int CodigoAgenciaCedente { get; set; }
        public string NomeDoBeneficiario { get; set; }
        public string CodigoDoBanco { get; set; }
        public string NomeDoBanco { get; set; }
        public string ContaCorrente { get; set; }
        public string ContaCobranca { get; set; }
        public DateTime DataGeracaoGravacao { get; set; }
        public DateTime DataCredito { get; set; }
        public string CodigoDoBeneficiario { get; set; }
        public string NumeroSequencial { get; set; }

        #endregion

        #region Banco do Brasil CBR 643

        public string DvAgenciaCedente { get; set; }
        public string DvContaCorrente { get; set; }
        public string CodigoENomeBanco { get; set; }
        /// <summary>
        /// Número sequencial atribuído pelo sistema do banco.
        /// </summary>
        public string SequencialRetorno { get; set; }
        public int NumeroConvenio { get; set; }

        #endregion

        #region Bradesco

        public int IdentificacaoArquivoRetorno { get; set; }
        public string Zeros { get; set; }
        public int NumeroAvisoBancario { get; set; }

        #endregion

        #region Caixa Econômica Federal

        public string UsoExclusivo { get; set; }
        public string MensagemRetorno { get; set; }
        public string NumeroSequencialA { get; set; }
        public string NumeroSequencialB { get; set; }

        #endregion

        #region HSBC

        public string Constante { get; set; }
        public string TipoRetorno { get; set; }
        public string Densidade { get; set; }
        public string LiteralDensidade { get; set; }
        public string NomeAgencia { get; set; }
        public int CodigoFormulario { get; set; }
        public string Volser { get; set; }

        #endregion

        #region Itaú

        public int DacAgenciaConta { get; set; }

        #endregion

        #region Santander

        public int Versao { get; set; }

        #endregion

        #region Método de Leitura HEADER Banco HSBC

        /// <summary>
        /// Faz a leitura do Header para arquivos do formato CNAB 400
        /// Bancos:
        /// HSBC
        /// </summary>
        /// <param name="registro"></param>
        public void LerHeaderRetornoCnab400Hsbc(string registro)
        {
            try
            {
                int dataGravacao = Convert.ToInt32(registro.ExtrairValorDaLinha(95, 100));

                CodigoDoRegistro = Convert.ToInt32(registro.ExtrairValorDaLinha(1, 1));
                CodigoDeRetorno = Convert.ToInt32(registro.ExtrairValorDaLinha(2, 2));
                LiteralRetorno = registro.ExtrairValorDaLinha(3, 9);
                CodigoDoServico = registro.ExtrairValorDaLinha(10, 11);
                LiteralServico = registro.ExtrairValorDaLinha(12, 26);
                CodigoAgenciaCedente = Convert.ToInt32(registro.ExtrairValorDaLinha(27, 31));
                Constante = registro.ExtrairValorDaLinha(32, 33);
                ContaCorrente = registro.ExtrairValorDaLinha(34, 44);
                TipoRetorno = registro.ExtrairValorDaLinha(45, 45);
                // 46 -> 1 branco
                NomeDoBeneficiario = registro.ExtrairValorDaLinha(47, 76);
                CodigoDoBanco = registro.ExtrairValorDaLinha(77, 79);
                NomeDoBanco = registro.ExtrairValorDaLinha(80, 94);
                DataGeracaoGravacao = (DateTime) dataGravacao.ToString().ToDateTimeFromDdMmAa();
                Densidade = registro.ExtrairValorDaLinha(101, 105);
                LiteralDensidade = registro.ExtrairValorDaLinha(106, 108);
                CodigoDoBeneficiario = registro.ExtrairValorDaLinha(109, 118);
                NomeAgencia = registro.ExtrairValorDaLinha(119, 138);
                CodigoFormulario = Convert.ToInt32(registro.ExtrairValorDaLinha(139, 142));
                // 143 - 388 -> 246 brancos
                Volser = registro.ExtrairValorDaLinha(389, 394);
                NumeroSequencial = registro.ExtrairValorDaLinha(395, 400);
            }
            catch (Exception)
            {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 400.");
            }
        }

        public static string PrimeiroCaracter(string retorno)
        {
            try
            {
                return retorno.Substring(0, 1);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao desmembrar registro.", ex);
            }
        }
        #endregion
    }
}

