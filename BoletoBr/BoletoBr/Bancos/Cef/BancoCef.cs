using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.Generico;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;
using Microsoft.SqlServer.Server;

namespace BoletoBr.Bancos.Cef
{
    public class BancoCef : IBanco
    {
        /* 
        * TipoDocumento 1 - SICGB - Com registro
        * TipoDocumento 2 - SICGB - Sem registro
        */

        // Identificador de Tipo de CobrançaII
        private const string IdentificadorTipoCobrancaCarteiraSicgbRg = "1";
        private const string IdentificadorTipoCobrancaCarteiraSicgbSr = "2";
        // Identificador de Emissão do Boleto (4 - Beneficiário)
        private const string IdentificadorEmissaoCedente = "4";

        private string _dacBoleto = String.Empty;

        private bool protestar = false;
        private bool baixaDevolver = false;
        private bool desconto = false;
        private int diasProtesto = 0;
        private int diasDevolucao = 0;
        private int diasDesconto = 0;

        public BancoCef()
        {
            this.CodigoBanco = "104";
            this.DigitoBanco = "0";
            this.NomeBanco = "Caixa Econômica Federal";
            this.LocalDePagamento = "Pagável preferencialmente nas agências da Caixa ou Lotéricas.";
            this.MoedaBanco = "9";

            /* Adiciona carteiras de cobrança */
            _carteirasCobrancaCef = new List<CarteiraCobranca>();
            _carteirasCobrancaCef.Add(new CarteiraCobrancaCefRg());
            _carteirasCobrancaCef.Add(new CarteiraCobrancaCefSr());
        }

        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        public Image LogotipoBancoParaExibicao { get; set; }

        private readonly List<CarteiraCobranca> _carteirasCobrancaCef;

        public string LocalDePagamento { get; private set; }
        public string MoedaBanco { get; private set; }

        public List<CarteiraCobranca> GetCarteirasCobranca()
        {
            return _carteirasCobrancaCef;
        }

        public CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira)
        {
            return GetCarteirasCobranca().Find(fd => fd.Codigo == codigoCarteira);
        }

        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            if (boleto.CarteiraCobranca == null)
            {
                throw new Exception("Carteira de cobrança não especificada");
            }

            if (!(boleto.CarteiraCobranca.Codigo.Equals("SR") || boleto.CarteiraCobranca.Codigo.Equals("RG")))
            {
                throw new Exception("Carteira cobrança com código: " + boleto.CarteiraCobranca.Codigo + " não é suportada.");
            }

            if (boleto.NossoNumeroFormatado.Length != 19)
            {
                throw new Exception("Nosso Número Formatado não pode ter tamanho diferente de 19 dígitos.");
            }

            if (String.IsNullOrEmpty(boleto.CedenteBoleto.CodigoCedente))
                throw new Exception("Código do cedente não foi informado.");

            if (boleto.CedenteBoleto.CodigoCedente.Length > 6)
                throw new Exception("O código do cedente deve no máximo 6 dígitos");
        }

        public void FormataMoeda(Boleto boleto)
        {
            boleto.Moeda = this.MoedaBanco;

            if (string.IsNullOrEmpty(boleto.Moeda))
                throw new Exception("Espécie/Moeda para o boleto não foi informada.");

            if ((boleto.Moeda == "1") || (boleto.Moeda == "REAL") || (boleto.Moeda == "R$"))
                boleto.Moeda = "R$";
            else
                boleto.Moeda = "0";
        }

        public void FormatarBoleto(Boleto boleto)
        {
            //Atribui o local de pagamento
            boleto.LocalPagamento = this.LocalDePagamento;

            boleto.ValidaDadosEssenciaisDoBoleto();

            FormataNumeroDocumento(boleto);
            FormataNossoNumero(boleto);
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataMoeda(boleto);

            ValidaBoletoComNormasBanco(boleto);

            /* Formata o código do cedente
             * Inserindo o dígito verificador
             */
            string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(6, '0');
            string dvCodigoCedente = Common.Mod11Base9(codigoCedente).ToString(); //Base9 

            if (boleto.CedenteBoleto.DigitoCedente.Equals(-1))
                boleto.CedenteBoleto.DigitoCedente = Convert.ToInt32(dvCodigoCedente);

            boleto.CedenteBoleto.CodigoCedente = String.Format("{0}/{1}-{2}",
                boleto.CedenteBoleto.ContaBancariaCedente.Agencia, codigoCedente, dvCodigoCedente);
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            if (boleto.CarteiraCobranca == null)
            {
                throw new Exception("Carteira de cobrança não especificada");
            }
            
            if (!(boleto.CarteiraCobranca.Codigo.Equals("SR") || boleto.CarteiraCobranca.Codigo.Equals("RG")))
            {
                throw new Exception("Carteira cobrança com código: " + boleto.CarteiraCobranca.Codigo + " não é suportada.");
            }

            if (boleto.NossoNumeroFormatado.Length != 19)
            {
                throw new Exception("Nosso Número Formatado não pode ter tamanho diferente de 19 dígitos.");
            }

            // Posi��o 01-03
            string banco = CodigoBanco;

            //Posi��o 04
            string moeda = MoedaBanco;

            //Posi��o 05 - No final ...   

            // Posi��o 06 - 09
            long fatorVencimento = Common.FatorVencimento(boleto.DataVencimento);

            // Posi��o 10 - 19     
            string valorDocumento = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorDocumento = valorDocumento.PadLeft(10, '0');

            // Inicio Campo livre
            string campoLivre = string.Empty;

            /* Presumimos que o nosso número formatado tem:
             * 19 dígitos no total se incluir o dígito verificador.
             * Para o cálculo do código de barras, precisamos do nosso número, sem o dígito verificador.
             * Para obter esse valor, vamos remover os 2 últimos dígitos.
             */
            string nossoNumeroFormatadoSemDigito =
                boleto.NossoNumeroFormatado.Substring(0, 17);

            //104 - Caixa Econ�mica Federal S.A. 
            //Carteira SR - 24 (cobran�a sem registro) || Carteira RG - 14 (cobran�a com registro)
            //Cobran�a sem registro, nosso n�mero com 17 d�gitos. 

            //Posi��o 20 - 25
            string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(6, '0');

            // Posi��o 26
            string dvCodigoCedente = Common.Mod11Base9(codigoCedente).ToString();

            //Posi��o 27 - 29
            //De acordo com documenta��o, posi��o 3 a 5 do nosso numero
            string primeiraParteNossoNumeroSemDigito = nossoNumeroFormatadoSemDigito.Substring(2, 3);

            //Posi��o 30
            string primeiraConstante;
            switch (boleto.CarteiraCobranca.Codigo)
            {
                case "SR":
                    primeiraConstante = "2";
                    break;
                case "RG":
                    primeiraConstante = "1";
                    break;
                default:
                    primeiraConstante = boleto.CarteiraCobranca.Codigo;
                    break;
            }

            // Posi��o 31 - 33
            //DE acordo com documenta��o, posi��o 6 a 8 do nosso numero
            string segundaParteNossoNumeroSemDigito = nossoNumeroFormatadoSemDigito.Substring(5, 3);

            // Posi��o 34
            const string segundaConstante = "4"; // 4 => emiss�o do boleto pelo cedente

            //Posi��o 35 - 43
            //De acordo com documenta�ao, posi��o 9 a 17 do nosso numero
            string terceiraParteNossoNumeroSemDigito = nossoNumeroFormatadoSemDigito.Substring(8, 9);

            //Posi��o 44
            string ccc = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                codigoCedente,
                dvCodigoCedente,
                primeiraParteNossoNumeroSemDigito,
                primeiraConstante,
                segundaParteNossoNumeroSemDigito,
                segundaConstante,
                terceiraParteNossoNumeroSemDigito);
            string dvCampoLivre = Common.Mod11Base9(ccc).ToString();
            campoLivre = string.Format("{0}{1}", ccc, dvCampoLivre);

            string xxxx = string.Format("{0}{1}{2}{3}{4}", banco, moeda, fatorVencimento, valorDocumento, campoLivre);

            string dvGeral = Common.Mod11(xxxx, 9).ToString();
            // Posi��o 5
            _dacBoleto = dvGeral;

            boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}",
                banco,
                moeda,
                dvGeral,
                fatorVencimento,
                valorDocumento,
                campoLivre
                );
            
            
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {
            string Grupo1 = string.Empty;
            string Grupo2 = string.Empty;
            string Grupo3 = string.Empty;
            string Grupo4 = string.Empty;
            string Grupo5 = string.Empty;

            string str1 = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;

            /// <summary>
            ///   IMPLEMENTA��O PARA NOSSO N�MERO COM 17 POSI��ES
            ///   Autor.: F�bio Marcos
            ///   E-Mail: fabiomarcos@click21.com.br
            ///   Data..: 01/03/2011
            /// </summary>
            if (boleto.NossoNumeroFormatado.Length == 17)
            {
                #region Campo 1

                //POSI��O 1 A 4 DO CODIGO DE BARRAS
                str1 = boleto.CodigoBarraBoleto.Substring(0, 4);
                //POSICAO 20 A 24 DO CODIGO DE BARRAS
                str2 = boleto.CodigoBarraBoleto.Substring(19, 5);
                //CALCULO DO DIGITO
                str3 = Common.Mod10(str1 + str2).ToString();

                Grupo1 = str1 + str2 + str3;
                Grupo1 = Grupo1.Substring(0, 5) + "." + Grupo1.Substring(5) + " ";

                #endregion Campo 1

                #region Campo 2

                //POSI��O 25 A 34 DO COD DE BARRAS
                str1 = boleto.CodigoBarraBoleto.Substring(24, 10);
                //DIGITO
                str2 = Common.Mod10(str1).ToString();

                Grupo2 = string.Format("{0}.{1}{2} ", str1.Substring(0, 5), str1.Substring(5, 5), str2);

                #endregion Campo 2

                #region Campo 3

                //POSI��O 35 A 44 DO CODIGO DE BARRAS
                str1 = boleto.CodigoBarraBoleto.Substring(34, 10);
                //DIGITO
                str2 = Common.Mod10(str1).ToString();

                Grupo3 = string.Format("{0}.{1}{2} ", str1.Substring(0, 5), str1.Substring(5, 5), str2);

                #endregion Campo 3

                #region Campo 4

                string D4 = _dacBoleto.ToString();

                Grupo4 = string.Format("{0} ", D4);

                #endregion Campo 4

                #region Campo 5

                //POSICAO 6 A 9 DO CODIGO DE BARRAS
                str1 = boleto.CodigoBarraBoleto.Substring(5, 4);

                //POSICAO 10 A 19 DO CODIGO DE BARRAS
                str2 = boleto.CodigoBarraBoleto.Substring(9, 10);

                Grupo5 = string.Format("{0}{1}", str1, str2);

                #endregion Campo 5
            }
            else
            {
                #region Campo 1

                string BBB = boleto.CodigoBarraBoleto.Substring(0, 3);
                string M = boleto.CodigoBarraBoleto.Substring(3, 1);
                string CCCCC = boleto.CodigoBarraBoleto.Substring(19, 5);
                string D1 = Common.Mod10(BBB + M + CCCCC).ToString();

                Grupo1 = string.Format("{0}{1}{2}.{3}{4} ",
                    BBB,
                    M,
                    CCCCC.Substring(0, 1),
                    CCCCC.Substring(1, 4), D1);


                #endregion Campo 1

                #region Campo 2

                string CCCCCCCCCC2 = boleto.CodigoBarraBoleto.Substring(24, 10);
                string D2 = Common.Mod10(CCCCCCCCCC2).ToString();

                Grupo2 = string.Format("{0}.{1}{2} ", CCCCCCCCCC2.Substring(0, 5), CCCCCCCCCC2.Substring(5, 5), D2);

                #endregion Campo 2

                #region Campo 3

                string CCCCCCCCCC3 = boleto.CodigoBarraBoleto.Substring(34, 10);
                string D3 = Common.Mod10(CCCCCCCCCC3).ToString();

                Grupo3 = string.Format("{0}.{1}{2} ", CCCCCCCCCC3.Substring(0, 5), CCCCCCCCCC3.Substring(5, 5), D3);


                #endregion Campo 3

                #region Campo 4

                string D4 = _dacBoleto.ToString();

                Grupo4 = string.Format(" {0} ", D4);

                #endregion Campo 4

                #region Campo 5

                long FFFF = Common.FatorVencimento(boleto.DataVencimento);

                string VVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
                VVVVVVVVVV = VVVVVVVVVV.PadLeft(10, '0');

                if (Convert.ToInt64(VVVVVVVVVV) == 0)
                    VVVVVVVVVV = "000";

                Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

                #endregion Campo 5
            }

            //MONTA OS DADOS DA LINHA DIGIT�VEL DE ACORDO COM OS DADOS OBTIDOS ACIMA
            boleto.LinhaDigitavelBoleto = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            if (String.IsNullOrEmpty(boleto.SequencialNossoNumero))
                throw new Exception("Sequencial nosso número não pode estar em branco.");

            if (boleto.SequencialNossoNumero.Length > 15)
                throw new Exception("Sequencial nosso número não pode exceder 15 dígitos.");

            string dvNossoNumero;

            boleto.SetNossoNumeroFormatado(boleto.SequencialNossoNumero.PadLeft(15, '0'));

            //Atribui ao Nosso Número o Identificador de Cobrança + Identificador do Emissor
            if (boleto.CarteiraCobranca.Codigo.Equals("RG"))
            {
                boleto.SetNossoNumeroFormatado(
                    IdentificadorTipoCobrancaCarteiraSicgbRg +
                    IdentificadorEmissaoCedente +
                    boleto.NossoNumeroFormatado);

                // Permite 0 (zero) no DV do Nosso Número
                dvNossoNumero = Common.Mod11(boleto.NossoNumeroFormatado).ToString();

                boleto.SetNossoNumeroFormatado(string.Format("{0}-{1}", boleto.NossoNumeroFormatado, dvNossoNumero));
            }
            else if (boleto.CarteiraCobranca.Codigo.Equals("SR"))
            {
                boleto.SetNossoNumeroFormatado(
                    IdentificadorTipoCobrancaCarteiraSicgbSr +
                    IdentificadorEmissaoCedente +
                    boleto.NossoNumeroFormatado);

                // Permite 0 (zero) no DV do Nosso Número
                dvNossoNumero = Common.Mod11(boleto.NossoNumeroFormatado).ToString();

                boleto.SetNossoNumeroFormatado(string.Format("{0}-{1}", boleto.NossoNumeroFormatado, dvNossoNumero));
            }
            else
            {
                throw new Exception("Erro ao formatar nosso número");
            }
        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            boleto.NumeroDocumento = boleto.NumeroDocumento.PadLeft(10, '0');

            //if (boleto.TipoArquivo == TipoArquivo.Cnab240)
            //    boleto.NumeroDocumento = boleto.NumeroDocumento.PadLeft(11, '0');

            //if (boleto.TipoArquivo == TipoArquivo.Cnab400)
            //    boleto.NumeroDocumento = boleto.NumeroDocumento.PadLeft(10, '0');

            //if (boleto.TipoArquivo == TipoArquivo.Outro)
            //    throw new Exception("Tipo de arquivo incorreto!" + Environment.NewLine + "Tipos aceitos: CNAB240 ou CNAB400");
        }

        public IInstrucao ObtemInstrucaoPadronizada(EnumTipoInstrucao tipoInstrucao, double valorInstrucao)
        {
            throw new NotImplementedException();
        }

        public RetornoGenerico LerArquivoRetorno(List<string> linhasArquivo)
        {
            if (linhasArquivo == null || linhasArquivo.Any() == false)
                throw new ApplicationException("Arquivo informado é inválido.");

            /* Identifica o layout: 240 ou 400 */
            if (linhasArquivo.First().Length == 240)
            {
                var leitor = new LeitorRetornoCnab240Cef(linhasArquivo);
                var retornoProcessado = leitor.ProcessarRetorno();

                var objRetornar = new RetornoGenerico(retornoProcessado);
                return objRetornar;
            }
            if (linhasArquivo.First().Length == 400)
            {
                var leitor = new LeitorRetornoCnab400Cef(linhasArquivo);
                var retornoProcessado = leitor.ProcessarRetorno();

                var objRetornar = new RetornoGenerico(retornoProcessado);
                return objRetornar;
            }

            throw new Exception("Arquivo de retorno com " + linhasArquivo.First().Length + " posições, não é suportado.");
        }


        #region M�todos de gera��o do arquivo remessa

        public RemessaCnab240 GerarArquivoRemessaCnab240(List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }

        public RemessaCnab400 GerarArquivoRemessaCnab400(List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Efetua as Valida��es dentro da classe Boleto, para garantir a gera��o da remessa
        /// </summary>
        public bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco,
            Cedente cedente, List<Boleto> boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //            
            switch (tipoArquivo)
            {
                case TipoArquivo.Cnab240:
                    vRetorno = ValidarRemessaCNAB240(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa,
                        out vMsg);
                    break;
                case TipoArquivo.Cnab400:
                    vRetorno = ValidarRemessaCnab400(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa,
                        out vMsg);
                    break;
                case TipoArquivo.Outro:
                    throw new Exception("Tipo de arquivo inexistente.");
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }

        public string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = " ";

                //base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {

                    case TipoArquivo.Cnab240:
                        //_detalhe = GerarDetalheRemessaCnab240(boleto);
                        break;
                    case TipoArquivo.Cnab400:
                        _detalhe = GerarDetalheRemessaCnab400(boleto, 1, TipoArquivo.Cnab400);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _detalhe;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public string GerarHeaderRemessa(Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            return GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);
        }

        public string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente,
            decimal vltitulostotal)
        {
            throw new NotImplementedException();
        }

        public string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cendente, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// HEADER do arquivo CNAB
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo,
            int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";

                //base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {

                    case TipoArquivo.Cnab240:
                        _header = GerarHeaderRemessaCnab240(cedente);
                        break;
                    case TipoArquivo.Cnab400:
                        _header = GerarHeaderRemessaCnab400(0, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do HEADER do arquivo de REMESSA.", ex);
            }
        }

        public string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo,
            int numeroArquivoRemessa, Boleto boletos)
        {
            try
            {
                string _header = " ";

                //base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        if (boletos.Remessa.TipoDocumento.Equals("2") || boletos.Remessa.TipoDocumento.Equals("1"))
                            _header = GerarHeaderRemessaCnab240Sigcb(cedente);
                        else
                            _header = GerarHeaderRemessaCnab240(cedente);
                        break;
                    case TipoArquivo.Cnab400:
                        _header = GerarHeaderRemessaCnab400(0, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do HEADER do arquivo de REMESSA.", ex);
            }
        }

        public string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio,
            Cedente cedente)
        {
            if (boleto.Remessa.TipoDocumento.Equals("2") || boleto.Remessa.TipoDocumento.Equals("1"))
                return GerarDetalheSegmentoPRemessaCnab240Sigcb(cedente, boleto, numeroRegistro);
            else
                return GerarDetalheSegmentoPRemessaCnab240(boleto, numeroRegistro, numeroConvenio, cedente);
        }

        public string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio,
            Cedente cedente,
            Boleto boletos)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            return GerarDetalheSegmentoQRemessaCnab240(boleto, numeroRegistro, tipoArquivo);
        }

        public string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, Sacado sacado)
        {
            return GerarDetalheSegmentoQRemessaCnab240Sigcb(boleto, numeroRegistro, sacado);
        }

        public string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistroDetalhe,
            TipoArquivo CNAB240)
        {
            return GerarDetalheSegmentoRRemessaCnab240(boleto, numeroRegistroDetalhe, CNAB240);
        }

        public string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            throw new NotImplementedException();
        }

        public string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            throw new NotImplementedException();
        }

        public string GerarTrailerLoteRemessa(int numeroRegistro, Boleto boletos)
        {
            if (boletos.Remessa.TipoDocumento.Equals("2") || boletos.Remessa.TipoDocumento.Equals("1"))
                return GerarTrailerLoteRemessaCnac240Sigcb(numeroRegistro);
            else
                return GerarTrailerLoteRemessaCnab240(numeroRegistro);
        }

        public DetalheSegmentoTRetornoCnab240 LerDetalheSegmentoTRetornoCnab240(string registro)
        {
            throw new NotImplementedException();
        }

        public DetalheSegmentoURetornoCnab240 LerDetalheSegmentoURetornoCnab240(string registro)
        {
            throw new NotImplementedException();
        }

        public DetalheSegmentoWRetornoCnab240 LerDetalheSegmentoWRetornoCnab240(string registro)
        {
            throw new NotImplementedException();
        }

        public DetalheRetornoGenericoCnab400 LerDetalheRetornoCnab400(string registro)
        {
            throw new NotImplementedException();
        }

        public Cedente Cedente { get; private set; }
        public int Codigo { get; set; }
        public string Nome { get; private set; }
        public string Digito { get; private set; }

        public string GerarTrailerArquivoRemessa(int numeroRegistro, Boleto boletos)
        {
            if (boletos.Remessa.TipoDocumento.Equals("2") || boletos.Remessa.TipoDocumento.Equals("1"))
                return GerarTrailerRemessaCnab240Sigcb(numeroRegistro);
            else
                return GerarTrailerArquivoRemessaCnab240(numeroRegistro);
        }

        public string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa,
            TipoArquivo tipoArquivo)
        {
            try
            {
                string header = " ";

                switch (tipoArquivo)
                {

                    case TipoArquivo.Cnab240:
                        header = GerarHeaderLoteRemessaCnab240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Cnab400:
                        //header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
        }

        public string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa,
            TipoArquivo tipoArquivo, Boleto boletos)
        {
            try
            {
                string header = " ";

                switch (tipoArquivo)
                {

                    case TipoArquivo.Cnab240:
                        if (boletos.Remessa.TipoDocumento.Equals("2") || boletos.Remessa.TipoDocumento.Equals("1"))
                            header = GerarHeaderLoteRemessaCnac240Sigcb(cedente, numeroArquivoRemessa);
                        else
                            header = GerarHeaderLoteRemessaCnab240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Cnab400:
                        //header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
        }

        #endregion

        #region CNAB 240

        public bool ValidarRemessaCNAB240(string numeroConvenio, IBanco banco, Cedente cedente, List<Boleto> boletos,
            int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //

            #region Pr� Valida��es

            if (banco == null)
            {
                vMsg += String.Concat("Remessa: O Banco � Obrigat�rio!", Environment.NewLine);
                vRetorno = false;
            }
            if (cedente == null)
            {
                vMsg += String.Concat("Remessa: O Cedente/Benefici�rio � Obrigat�rio!", Environment.NewLine);
                vRetorno = false;
            }
            if (boletos == null || boletos.Equals(0))
            {
                vMsg += String.Concat("Remessa: Dever� existir ao menos 1 boleto para gera��o da remessa!",
                    Environment.NewLine);
                vRetorno = false;
            }

            #endregion

            //
            //valida��o de cada boleto
            foreach (Boleto boleto in boletos)
            {
                #region Valida��o de cada boleto

                if (boleto.Remessa == null)
                {
                    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento,
                        "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                else if (boleto.Remessa.TipoDocumento.Equals("1") &&
                         String.IsNullOrEmpty(boleto.SacadoBoleto.EnderecoSacado.Cep))
                    //1 - SICGB - Com registro
                {
                    //Para o "Remessa.TipoDocumento = "1", o CEP � Obrigat�rio!
                    vMsg +=
                        String.Concat(
                            "Para o Tipo Documento [1 - SIGCB - COM REGISTRO], o CEP do SACADO � Obrigat�rio!",
                            Environment.NewLine);
                    vRetorno = false;
                }
                if (boleto.NossoNumeroFormatado.Length > 15)
                    boleto.SetNossoNumeroFormatado(boleto.NossoNumeroFormatado.Substring(0, 15));
                //if (!boleto.Remessa.TipoDocumento.Equals("2")) //2 - SIGCB - SEM REGISTRO
                //{
                //    //Para o "Remessa.TipoDocumento = "2", n�o poder� ter NossoNumero Gerado!
                //    vMsg += String.Concat("Tipo Documento de boleto n�o Implementado!", Environment.NewLine);
                //    vRetorno = false;
                //}

                #endregion
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }

        /// <summary>
        /// Varre as instrucoes para inclusao no Segmento P
        /// </summary>
        /// <param name="boleto"></param>
        private void ValidaInstrucoes240(Boleto boleto)
        {
            if (boleto.InstrucoesDoBoleto.Count.Equals(0))
                return;

            protestar = false;
            baixaDevolver = false;
            desconto = false;
            diasProtesto = 0;
            diasDevolucao = 0;
            diasDesconto = 0;
            foreach (IInstrucao instrucao in boleto.InstrucoesDoBoleto)
            {
                if (instrucao.Codigo.Equals(9) || instrucao.Codigo.Equals(42) || instrucao.Codigo.Equals(81) ||
                    instrucao.Codigo.Equals(82))
                {
                    protestar = true;
                    diasProtesto = instrucao.QtdDias;
                }
                else if (instrucao.Codigo.Equals(91) || instrucao.Codigo.Equals(92))
                {
                    baixaDevolver = true;
                    diasDevolucao = instrucao.QtdDias;
                }
                else if (instrucao.Codigo.Equals(999))
                {
                    desconto = true;
                    diasDesconto = instrucao.QtdDias;
                }
            }
        }

        public string GerarHeaderRemessaCnab240(Cedente cedente)
        {
            try
            {
                string header = Codigo.ToString().PadLeft(3, '0'); // c�digo do banco na compensa��o
                header += "0000"; // Lote de Servi�o 
                header += "0"; // Tipo de Registro 
                header += Common.CompletarCadeiaAEsquerda(""," ", 9); // Uso Exclusivo FEBRABAN/CNAB
                header += cedente.CpfCnpj.Length == 11 ? "1" : "2"; // Tipo de Inscri��o 
                header += cedente.CpfCnpj; // CPF/CNPJ do cedente 
                header += cedente.CodigoCedente + cedente.DigitoCedente;
                // C�digo do Conv�nio no Banco 
                header += Common.CompletarCadeiaAEsquerda("", "0", 4); // Uso Exclusivo CAIXA
                header += cedente.ContaBancariaCedente.Agencia.PadLeft(5, '0'); // Ag�ncia Mantenedora da Conta 
                header += cedente.ContaBancariaCedente.DigitoAgencia;
                // D�gito Verificador da Ag�ncia 
                header += cedente.ContaBancariaCedente.Conta.PadLeft(12, '0'); // C�digo do Cedente (sem opera��o)  
                header += cedente.ContaBancariaCedente.DigitoConta; // D�g. Verif. Cedente (sem opera��o) 
                header +=
                    Common.Mod11(cedente.ContaBancariaCedente.Agencia + cedente.ContaBancariaCedente.Conta).ToString();
                // D�gito Verif. Ag./Ced  (sem opera��o)
                header += Common.CompletarCadeiaAEsquerda(cedente.Nome, " ", 30); // Nome do cedente
                header += Common.CompletarCadeiaAEsquerda("CAIXA ECONOMICA FEDERAL", " ", 30); // Nome do Banco
                header += Common.CompletarCadeiaAEsquerda("", " ", 10); // Uso Exclusivo FEBRABAN/CNAB
                header += "1"; // C�digo 1 - Remessa / 2 - Retorno 
                header += DateTime.Now.ToString("ddMMyyyy"); // Data de Gera��o do Arquivo
                header += string.Format("{0:hh:mm:ss}", DateTime.Now).Replace(":", ""); // Hora de Gera��o do Arquivo
                header += "000001"; // N�mero Seq�encial do Arquivo 
                header += "030"; // N�mero da Vers�o do Layout do Arquivo 
                header += "0"; // Densidade de Grava��o do Arquivo 
                header += Common.CompletarCadeiaAEsquerda("", " ", 20); // Para Uso Reservado do Banco
                // Na fase de teste deve conter "remessa-produ��o", ap�s aprovado deve conter espa�os em branco
                header += Common.CompletarCadeiaAEsquerda("REMESSA-PRODUCAO", " ", 20); // Para Uso Reservado da Empresa  
                //header += Common.CompletarCadeiaAEsquerda("", " ", 20);                                              // Para Uso Reservado da Empresa
                header += Common.CompletarCadeiaAEsquerda("", " ", 29); // Uso Exclusivo FEBRABAN/CNAB

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }

        private string GerarHeaderLoteRemessaCnab240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string header = Codigo.ToString().PadLeft(3, '0'); // c�digo do banco na compensa��o
                header += "0001"; // Lote de Servi�o
                header += "1"; // Tipo de Registro 
                header += "R"; // Tipo de Opera��o 
                header += "01"; // Tipo de Servi�o '01' = Cobran�a, '03' = Bloqueto Eletr�nico 
                header += "  "; // Uso Exclusivo FEBRABAN/CNAB
                header += "020"; // N�mero da Vers�o do Layout do Arquivo 
                header += " "; // Uso Exclusivo FEBRABAN/CNAB
                header += cedente.CpfCnpj.Length == 11 ? "1" : "2"; // Tipo de Inscri��o 
                header += cedente.CpfCnpj; // CPF/CNPJ do cedente
                header += cedente.CodigoCedente + cedente.DigitoCedente.ToString().PadRight(16, ' ');
                // C�digo do Conv�nio no Banco 
                header += "".PadRight(4, ' '); // Uso Exclusivo CAIXA
                header += Common.CompletarCadeiaAEsquerda(cedente.ContaBancariaCedente.Agencia, "0", 5);
                // Ag�ncia Mantenedora da Conta 
                header += Common.CompletarCadeiaAEsquerda(cedente.ContaBancariaCedente.DigitoAgencia, "0", 5);
                // D�gito Verificador da Ag�ncia 
                header += Common.CompletarCadeiaAEsquerda(cedente.ContaBancariaCedente.Conta, "0", 12);
                // N�mero da Conta Corrente 
                header += cedente.ContaBancariaCedente.DigitoConta; // Digito Verificador da Conta Corrente 
                header +=
                    Common.Mod11(cedente.ContaBancariaCedente.Agencia + cedente.ContaBancariaCedente.Conta).ToString();
                // D�gito Verif. Ag./Ced  (sem opera��o)
                header += Common.CompletarCadeiaAEsquerda(cedente.Nome, " ", 30); // Nome do cedente
                header += Common.CompletarCadeiaAEsquerda("", " ", 40); // Mensagem 1
                header += Common.CompletarCadeiaAEsquerda("", " ", 40); // Mensagem 2
                header += numeroArquivoRemessa.ToString("00000000"); // N�mero Remessa/Retorno
                header += DateTime.Now.ToString("ddMMyyyy"); // Data de Grava��o Remessa/Retorno 
                header += Common.CompletarCadeiaAEsquerda("", "0", 8); // Data do Cr�dito 
                header += Common.CompletarCadeiaAEsquerda("", " ", 33); // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }

        public string GerarDetalheSegmentoPRemessaCnab240(Boleto boleto, int numeroRegistro, string numeroConvenio,
            Cedente cedente)
        {
            try
            {
                ValidaInstrucoes240(boleto); // Para protestar, devolver ou desconto.

                string detalhe = Codigo.ToString().PadLeft(3, '0'); // c�digo do banco na compensa��o
                detalhe += "0001"; // Lote de Servi�o
                detalhe += "3"; // Tipo de Registro 
                detalhe += Common.CompletarCadeiaAEsquerda(numeroRegistro.ToString(), "0", 5);
                // N� Sequencial do Registro no Lote 
                detalhe += "P"; // C�d. Segmento do Registro Detalhe
                detalhe += " "; // Uso Exclusivo FEBRABAN/CNAB
                detalhe += "01"; // C�digo de Movimento Remessa 
                detalhe += Common.CompletarCadeiaAEsquerda(cedente.ContaBancariaCedente.Agencia, "0", 5);
                // Ag�ncia Mantenedora da Conta 
                detalhe += cedente.ContaBancariaCedente.DigitoAgencia; // D�gito Verificador da Ag�ncia 
                detalhe += Common.CompletarCadeiaAEsquerda(cedente.ContaBancariaCedente.Conta, "0", 12);
                // N�mero da Conta Corrente 
                detalhe += cedente.ContaBancariaCedente.DigitoConta; // Digito Verificador da Conta Corrente 
                detalhe +=
                    Common.Mod11(cedente.ContaBancariaCedente.Agencia + cedente.ContaBancariaCedente.Conta).ToString();
                // D�gito Verif. Ag./Ced  (sem opera��o)
                detalhe += Common.CompletarCadeiaAEsquerda("", "0", 9); // Uso Exclusivo CAIXA
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.NossoNumeroFormatado, "0", 11);
                // Identifica��o do T�tulo no Banco 
                detalhe += "01"; // C�digo da Carteira 
                detalhe += (boleto.CarteiraCobranca.Codigo == "14" ? "2" : "1"); // Forma de Cadastr. do T�tulo no Banco 
                // '1' = Com Cadastramento (Cobran�a Registrada) 
                // '2' = Sem Cadastramento (Cobran�a sem Registro) 
                detalhe += "2"; // Tipo de Documento 
                detalhe += "2"; // Identifica��o da Emiss�o do Bloqueto 
                detalhe += "2"; // Identifica��o da Distribui��o
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.NumeroDocumento, "0", 11); // N�mero do Documento de Cobran�a 
                detalhe += "    "; // Uso Exclusivo CAIXA
                detalhe += boleto.DataVencimento.ToString("ddMMyyyy"); // Data de Vencimento do T�tulo
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.ValorBoleto.ToString().Replace(",", "").Replace(".", ""), "0",
                    13);
                // Valor Nominal do T�tulo 13
                detalhe += Common.CompletarCadeiaAEsquerda(cedente.ContaBancariaCedente.Agencia, "0", 5);
                // Ag�ncia Encarregada da Cobran�a 
                detalhe += cedente.ContaBancariaCedente.DigitoAgencia; // D�gito Verificador da Ag�ncia 
                detalhe += boleto.Especie.Codigo; // Esp�cie do T�tulo 
                detalhe += boleto.Aceite; // Identific. de T�tulo Aceito/N�o Aceito
                // Data da Emiss�o do T�tulo 
                if (boleto.DataProcessamento == DateTime.MinValue)
                    DateTime.Now.ToString("ddMMyyyy");
                else
                    boleto.DataProcessamento.ToString().ToDateTimeFromDdMmAaaa();
                detalhe += boleto.DataProcessamento;
                detalhe += "1"; // C�digo do Juros de Mora '1' = Valor por Dia - '2' = Taxa Mensal 
                detalhe += (boleto.DataMulta.ToString("ddMMyyyy") == "01010001"
                    ? "00000000"
                    : boleto.DataMulta.ToString("ddMMyyyy")); // Data do Juros de Mora 
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.ValorMulta.ToString().Replace(",", "").Replace(".", ""), "0", 13);
                // Juros de Mora por Dia/Taxa 
                detalhe += (desconto ? "1" : "0"); // C�digo do Desconto 
                detalhe += (boleto.DataDesconto.ToString("ddMMyyyy") == "01010001"
                    ? "00000000"
                    : boleto.DataDesconto.ToString("ddMMyyyy")); // Data do Desconto
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.ValorDesconto.ToString().Replace(",", "").Replace(".", ""), "0", 13);
                // Valor/Percentual a ser Concedido 
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.Iof.ToString().Replace(",", "").Replace(".", ""), "0", 13);
                // Valor do IOF a ser Recolhido 
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.ValorAbatimento.ToString().Replace(",", "").Replace(".", ""), "0", 13);
                // Valor do Abatimento 
                detalhe += Common.CompletarCadeiaAEsquerda("", " ", 25); // Identifica��o do T�tulo na Empresa
                detalhe += (protestar ? "1" : "3"); // C�digo para Protesto
                detalhe += diasProtesto.ToString("00"); // N�mero de Dias para Protesto 2 posi
                detalhe += (baixaDevolver ? "1" : "2"); // C�digo para Baixa/Devolu��o 1 posi
                detalhe += diasDevolucao.ToString("00"); // N�mero de Dias para Baixa/Devolu��o 3 posi
                detalhe += boleto.Moeda.PadLeft(2, '0'); // C�digo da Moeda 
                detalhe += Common.CompletarCadeiaAEsquerda("", " ", 10); // Uso Exclusivo FEBRABAN/CNAB 
                detalhe += Common.CompletarCadeiaAEsquerda("", " ", 1); // Uso Exclusivo FEBRABAN/CNAB 

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar SEGMENTO P do arquivo de remessa.", e);
            }
        }

        public string GerarDetalheSegmentoQRemessaCnab240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string header = Codigo.ToString().PadLeft(3, '0'); // c�digo do banco na compensa��o
                header += "0001"; // Lote de Servi�o
                header += "3"; // Tipo de Registro 
                header += Common.CompletarCadeiaAEsquerda(numeroRegistro.ToString(), "0", 5);
                // N� Sequencial do Registro no Lote 
                header += "Q"; // C�d. Segmento do Registro Detalhe
                header += " "; // Uso Exclusivo FEBRABAN/CNAB
                header += "01"; // C�digo de Movimento Remessa
                header += (boleto.SacadoBoleto.CpfCnpj.Length == 11 ? "1" : "2"); // Tipo de Inscri��o 
                header += Common.CompletarCadeiaAEsquerda(boleto.SacadoBoleto.CpfCnpj, "0", 15); // N�mero de Inscri��o 
                header += Common.CompletarCadeiaAEsquerda(boleto.SacadoBoleto.Nome, " ", 40); // Nome
                header += Common.CompletarCadeiaAEsquerda(boleto.SacadoBoleto.EnderecoSacado.Logradouro, " ", 40); // Endere�o
                header += Common.CompletarCadeiaAEsquerda(boleto.SacadoBoleto.EnderecoSacado.Bairro, " ", 15); // Bairro 
                header += boleto.SacadoBoleto.EnderecoSacado.Cep; // CEP + Sufixo do CEP
                header += Common.CompletarCadeiaAEsquerda(boleto.SacadoBoleto.EnderecoSacado.Cidade, " ", 15); // Cidade 
                header += boleto.SacadoBoleto.EnderecoSacado.SiglaUf; // Unidade da Federa��o
                // Estes campos dever�o estar preenchidos quando n�o for o Cedente original do t�tulo.
                header += "0"; // Tipo de Inscri��o 
                header += Common.CompletarCadeiaAEsquerda("", "0", 15); // N�mero de Inscri��o CPF/CNPJ
                header += Common.CompletarCadeiaAEsquerda("", " ", 40); // Nome do Sacador/Avalista 
                //*********
                header += Common.CompletarCadeiaAEsquerda("", " ", 31); // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar SEGMENTO Q do arquivo de remessa.", e);
            }
        }

        public string GerarDetalheSegmentoRRemessaCnab240(Boleto boleto, int numeroRegistroDetalhe, TipoArquivo CNAB240)
        {
            try
            {
                string header = Codigo.ToString().PadLeft(3, '0'); // c�digo do banco na compensa��o
                header += "0001"; // Lote de Servi�o
                header += "3"; // Tipo de Registro 
                header += Common.CompletarCadeiaAEsquerda(numeroRegistroDetalhe.ToString(), "0", 5);
                // N� Sequencial do Registro no Lote 
                header += "R"; // C�d. Segmento do Registro Detalhe
                header += " "; // Uso Exclusivo FEBRABAN/CNAB
                header += "01"; // C�digo de Movimento Remessa
                header += Common.CompletarCadeiaAEsquerda("", " ", 48); // Uso Exclusivo FEBRABAN/CNAB 
                header += "1"; // C�digo da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                header += boleto.DataMulta.ToString("ddMMyyyy"); // Data da Multa 
                header += Common.CompletarCadeiaAEsquerda(boleto.ValorMulta.ToString().Replace(",", "").Replace(".", ""), "0", 13);
                // Valor/Percentual a Ser Aplicado
                header += Common.CompletarCadeiaAEsquerda("", " ", 10); // Informa��o ao Sacado
                header += Common.CompletarCadeiaAEsquerda("", " ", 40); // Mensagem 3
                header += Common.CompletarCadeiaAEsquerda("", " ", 40); // Mensagem 4
                header += Common.CompletarCadeiaAEsquerda("", " ", 61); // Uso Exclusivo FEBRABAN/CNAB 

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar SEGMENTO Q do arquivo de remessa.", e);
            }
        }

        public string GerarTrailerLoteRemessaCnab240(int numeroRegistro)
        {
            try
            {
                string header = Codigo.ToString().PadLeft(3, '0'); // c�digo do banco na compensa��o
                header += "0001"; // Lote de Servi�o
                header += "5"; // Tipo de Registro 
                header += Common.CompletarCadeiaAEsquerda("", " ", 61); // Uso Exclusivo FEBRABAN/CNAB
                header += Common.CompletarCadeiaAEsquerda(numeroRegistro.ToString(), "0", 5);
                // N� Sequencial do Registro no Lote 

                // Totaliza��o da Cobran�a Simples
                header += Common.CompletarCadeiaAEsquerda("", "0", 6); // Quantidade de T�tulos em Cobran�a
                header += Common.CompletarCadeiaAEsquerda("", "0", 15); // Valor Total dos T�tulos em Carteiras

                header += Common.CompletarCadeiaAEsquerda("", "0", 6); // Uso Exclusivo FEBRABAN/CNAB
                header += Common.CompletarCadeiaAEsquerda("", "0", 15); // Uso Exclusivo FEBRABAN/CNAB 

                // Totaliza��o da Cobran�a Caucionada
                header += Common.CompletarCadeiaAEsquerda("", "0", 6); // Quantidade de T�tulos em Cobran�a
                header += Common.CompletarCadeiaAEsquerda("", "0", 15); // Valor Total dos T�tulos em Carteiras

                // Totaliza��o da Cobran�a Descontada
                header += Common.CompletarCadeiaAEsquerda("", "0", 6); // Quantidade de T�tulos em Cobran�a
                header += Common.CompletarCadeiaAEsquerda("", "0", 15); // Valor Total dos T�tulos em Carteiras

                header += Common.CompletarCadeiaAEsquerda("", " ", 8); // Uso Exclusivo FEBRABAN/CNAB
                header += Common.CompletarCadeiaAEsquerda("", " ", 117); // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar Trailer de Lote do arquivo de remessa.", e);
            }
        }

        public string GerarTrailerArquivoRemessaCnab240(int numeroRegistro)
        {
            try
            {
                string header = Codigo.ToString().PadLeft(3, '0'); // c�digo do banco na compensa��o
                header += "9999"; // Lote de Servi�o
                header += "9"; // Tipo de Registro 
                header += Common.CompletarCadeiaAEsquerda("", " ", 9); // Uso Exclusivo FEBRABAN/CNAB
                header += "000001"; // Quantidade de Lotes do Arquivo
                header += Common.CompletarCadeiaAEsquerda(numeroRegistro.ToString(), "0", 6);
                // Quantidade de Registros do Arquivo
                header += Common.CompletarCadeiaAEsquerda("", " ", 6); // Uso Exclusivo FEBRABAN/CNAB
                header += Common.CompletarCadeiaAEsquerda("", " ", 205); // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar Trailer de arquivo de remessa.", e);
            }
        }

        public DetalheSegmentoTRetornoCnab240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            throw new NotImplementedException();
        }

        public DetalheSegmentoURetornoCnab240 LerDetalheSegmentoURetornoCNAB240(string registro)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region CNAB 240 - SIGCB

        public string GerarHeaderRemessaCnab240Sigcb(Cedente cedente)
        {
            try
            {
                string header = Codigo.ToString().PadLeft(3, '0'); // posi��o 1 at� 3     (3) - c�digo do banco na compensa��o   
                header += "0000"; // posi��o 4 at� 7     (4) - Lote de Servi�o
                header += "0"; // posi��o 8 at� 8     (1) - Tipo de Registro
                header += Common.CompletarCadeiaAEsquerda("", " ", 9); // posi��o 9 at� 17     (9) - Uso Exclusivo FEBRABAN/CNAB

                #region Regra Tipo de Inscri��o Cedente

                string vCpfCnpjEmi = "0"; //n�o informado
                if (cedente.CpfCnpj.Length.Equals(11)) vCpfCnpjEmi = "1"; //Cpf
                else if (cedente.CpfCnpj.Length.Equals(14)) vCpfCnpjEmi = "2"; //Cnpj

                #endregion

                header += vCpfCnpjEmi; // posi��o 18 at� 18   (1) - Tipo de Inscri��o 
                header += cedente.CpfCnpj; // posi��o 19 at� 32   (14)- N�mero de Inscri��o da empresa
                header += Common.CompletarCadeiaAEsquerda("", " ", 20); // posi��o 33 at� 52   (20)- Uso Exclusivo CAIXA
                header += cedente.ContaBancariaCedente.Agencia.PadLeft(5, '0'); // posi��o 53 at� 57   (5) - Ag�ncia Mantenedora da Conta
                header += cedente.ContaBancariaCedente.DigitoAgencia; // posi��o 58 at� 58   (1) - D�gito Verificador da Ag�ncia
                header += Common.CompletarCadeiaAEsquerda(cedente.Convenio, " ", 6); // posi��o 59 at� 64   (6) - C�digo do Conv�nio no Banco (C�digo do Cedente)
                header += Common.CompletarCadeiaAEsquerda("", " ", 7); // posi��o 65 at� 71   (7) - Uso Exclusivo CAIXA
                header += Common.CompletarCadeiaAEsquerda("", " ", 1); // posi��o 72 at� 72   (1) - Uso Exclusivo CAIXA
                header += Common.CompletarCadeiaAEsquerda(cedente.Nome.ToUpper(), " ", 30); // posi��o 73 at� 102  (30)- Nome da Empresa
                header += Common.CompletarCadeiaAEsquerda(NomeBanco.ToUpper(), " ", 30); // posi��o 103 at� 132 (30)- Nome do Banco
                header += Common.CompletarCadeiaAEsquerda("", " ", 10); // posi��o 133 at� 142 (10)- Uso Exclusivo FEBRABAN/CNAB
                header += "1"; // posi��o 143 at� 143 (1) - C�digo 1 - Remessa / 2 - Retorno
                header += DateTime.Now.ToString().Replace("/", ""); // posi��o 144 at� 151 (8) - Data de Gera��o do Arquivo
                header += DateTime.Now.AddHours(23).AddMinutes(59).AddSeconds(59).ToString().Replace(":", ""); // posi��o 152 at� 157 (6) - Hora de Gera��o do Arquivo
                header += cedente.NumeroSequencial.PadLeft(6, '0'); // posi��o 158 at� 163 (6) - N�mero Seq�encial do Arquivo
                header += "050"; // posi��o 164 at� 166 (3) - Nro da Vers�o do Layout do Arquivo
                header += "0"; // posi��o 167 at� 171 (5) - Densidade de Grava��o do Arquivo
                header += Common.CompletarCadeiaAEsquerda("", " ", 20); // posi��o 172 at� 191 (20)- Para Uso Reservado do Banco
                header += Common.CompletarCadeiaAEsquerda("REMESSA-PRODUCAO", " ", 20); // posi��o 192 at� 211 (20)- Para Uso Reservado da Empresa
                header += Common.CompletarCadeiaAEsquerda("", " ", 4); // posi��o 212 at� 215 (4) - Vers�o Aplicativo CAIXA
                header += Common.CompletarCadeiaAEsquerda("", " ", 25); // posi��o 216 at� 240 (25)- Para Uso Reservado da Empresa

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240 SIGCB.", ex);
            }
        }

        public string GerarHeaderLoteRemessaCnac240Sigcb(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string headerLote = Codigo.ToString().PadLeft(3, '0');// posi��o 1 at� 3     (3) - c�digo do banco na compensa��o  
                headerLote += "0000"; // posi��o 4 at� 7     (4) - Lote de Servi�o
                headerLote += "1"; // posi��o 8 at� 8     (1) - Tipo de Registro
                headerLote += "R"; // posi��o 9 at� 9     (1) - Tipo de Opera��o
                headerLote += "01"; // posi��o 10 at� 11   (2) - Tipo de Servi�o
                headerLote += "00"; // posi��o 12 at� 13   (2) - Uso Exclusivo FEBRABAN/CNAB
                headerLote += "030"; // posi��o 14 at� 16   (3) - N� da Vers�o do Layout do Lote
                headerLote += Common.CompletarCadeiaAEsquerda("", " ", 1); // posi��o 17 at� 17   (1) - Uso Exclusivo FEBRABAN/CNAB

                #region Regra Tipo de Inscri��o Cedente

                string vCpfCnpjEmi = "0"; //n�o informado
                if (cedente.CpfCnpj.Length.Equals(11)) vCpfCnpjEmi = "1"; //Cpf
                else if (cedente.CpfCnpj.Length.Equals(14)) vCpfCnpjEmi = "2"; //Cnpj

                #endregion

                headerLote += vCpfCnpjEmi; // posi��o 18 at� 18   (1) - Tipo de Inscri��o 
                headerLote += cedente.CpfCnpj; // posi��o 19 at� 33   (15)- N�mero de Inscri��o da empresa
                headerLote += cedente.Convenio; // posi��o 34 at� 39   (6) - C�digo do Conv�nio no Banco
                headerLote += Common.CompletarCadeiaAEsquerda("", "0", 14); // posi��o 40 at� 53   (14)- Uso Exclusivo CAIXA
                headerLote += cedente.ContaBancariaCedente.Agencia.PadLeft(5, '0'); // posi��o 54 at� 58   (5) - Ag�ncia Mantenedora da Conta
                headerLote += cedente.ContaBancariaCedente.DigitoAgencia; // posi��o 59 at� 59   (1) - D�gito Verificador da Ag�ncia
                headerLote += cedente.Convenio; // posi��o 60 at� 65   (6) - C�digo do Conv�nio no Banco  
                headerLote += Common.CompletarCadeiaAEsquerda("", "0", 7); // posi��o 66 at� 72   (7) - C�digo do Modelo Personalizado
                headerLote += Common.CompletarCadeiaAEsquerda("", "0", 1); // posi��o 73 at� 73   (1) - Uso Exclusivo CAIXA
                headerLote += Common.CompletarCadeiaAEsquerda(cedente.Nome.ToUpper(), " ", 30); // posi��o 73 at� 103  (30)- Nome da Empresa                
                
                //TODO.: ROGER KLEIN - INSTRU��ES N�O TRATADAS

                #region Instru��es

                //string descricao = string.Empty;
                ////
                string vInstrucao1 = string.Empty;
                string vInstrucao2 = string.Empty;
                //foreach (Instrucao_Caixa instrucao in boleto.Instrucoes)
                //{
                //    switch ((EnumInstrucoes_Caixa)instrucao.Codigo)
                //    {
                //        case EnumInstrucoes_Caixa.Protestar:
                //            //
                //            //instrucao.Descricao.ToString().ToUpper();
                //            break;
                //        case EnumInstrucoes_Caixa.NaoProtestar:
                //            //
                //            break;
                //        case EnumInstrucoes_Caixa.ImportanciaporDiaDesconto:
                //            //
                //            break;
                //        case EnumInstrucoes_Caixa.ProtestoFinsFalimentares:
                //            //
                //            break;
                //        case EnumInstrucoes_Caixa.ProtestarAposNDiasCorridos:
                //            break;
                //    }
                //}

                #endregion

                headerLote += Common.CompletarCadeiaAEsquerda(vInstrucao1, " ", 40); // posi��o 104 at� 143 (40) - Mensagem 1
                headerLote += Common.CompletarCadeiaAEsquerda(vInstrucao2, " ", 40); // posi��o 144 at� 183 (40) - Mensagem 2
                headerLote += Common.CompletarCadeiaAEsquerda(numeroArquivoRemessa.ToString(), "0", 8); // posi��o 184 at� 191 (8)  - N�mero Remessa/Retorno
                headerLote += DateTime.Now.ToString().Replace("/", ""); // posi��o 192 at� 199 (8) - Data de Gera��o do Arquivo   
                /* Data do Cr�dito
                 * Data de efetiva��o do cr�dito referente ao pagamento do t�tulo de cobran�a. 
                 * Informa��o enviada somente no arquivo de retorno.
                 */
                headerLote += Common.CompletarCadeiaAEsquerda("", " ", 8); // posi��o 200 at� 207 (8) - Data do Cr�dito
                headerLote += Common.CompletarCadeiaAEsquerda("", " ", 33); // posi��o 208 at� 240(33) - Uso Exclusivo FEBRABAN/CNAB

                return headerLote;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do lote no arquivo de remessa do CNAB400.", ex);
            }
        }

        #region Detalhes

        public string GerarDetalheSegmentoPRemessaCnab240Sigcb(Cedente cedente, Boleto boleto, int numeroRegistro)
        {
            try
            {
                #region Segmento P

                ValidaInstrucoes240(boleto);

                string detalhe = Codigo.ToString().PadLeft(3, '0'); // posi��o 1 at� 3     (3) - c�digo do banco na compensa��o  
                detalhe += "0001"; // posi��o 4 at� 7     (4) - Lote de Servi�o
                detalhe += "3"; // posi��o 8 at� 8     (1) - Tipo de Registro
                detalhe += numeroRegistro.ToString().PadLeft(5, '0'); // posi��o 9 at� 13    (5) - N� Sequencial do Registro no Lote
                detalhe += "P"; // posi��o 14 at� 14   (1) - C�d. Segmento do Registro Detalhe
                detalhe += Common.CompletarCadeiaAEsquerda("", " ", 1); // posi��o 15 at� 15   (1) - Uso Exclusivo FEBRABAN/CNAB
                detalhe += "01"; // posi��o 16 at� 17   (2) - C�digo de Movimento Remessa
                detalhe += cedente.ContaBancariaCedente.Agencia.PadLeft(5, '0'); // posi��o 18 at� 22   (5) - Ag�ncia Mantenedora da Conta
                detalhe += cedente.ContaBancariaCedente.DigitoAgencia.ToUpper(); // posi��o 23 at� 23   (1) - D�gito Verificador da Ag�ncia
                detalhe += cedente.Convenio.PadLeft(6, '0'); // posi��o 24 at� 29   (6) - C�digo do Conv�nio no Banco
                detalhe += Common.CompletarCadeiaAEsquerda("", " ", 11); // posi��o 30 at� 40   (11)- Uso Exclusivo CAIXA
                detalhe += boleto.NossoNumeroFormatado; // posi��o 43 at� 57   (15)- Identifica��o do T�tulo no Banco

                #region C�digo da Carteira

                //C�digo adotado pela FEBRABAN, para identificar a caracter�stica dos t�tulos dentro das modalidades de cobran�a existentes no banco.
                //�1� = Cobran�a Simples; �3� = Cobran�a Caucionada; �4� = Cobran�a Descontada
                detalhe += "1"; // posi��o 58 at� 58   (1) - C�digo Carteira

                #endregion

                #region Forma de Cadastramento do Título no Banco

                string vFormaCadastramento = "1"; // Com registro
                if (boleto.Remessa.TipoDocumento.Equals("2"))
                    vFormaCadastramento = "2"; //Sem registro               

                #endregion

                detalhe += vFormaCadastramento; // posi��o 59 at� 59   (1) - Forma de Cadastr. do T�tulo no Banco 1 - Com Registro 2 - Sem registro.
                detalhe += "2"; // posi��o 60 at� 60   (1) - Tipo de Documento
                String emissao = boleto.CarteiraCobranca.Codigo.Equals("CS") ? "1" : "2";
                detalhe += emissao; // posi��o 61 at� 61   (1) - Identifica��o da Emiss�o do Bloqueto -- �1�-Banco Emite, '2'-entrega do bloqueto pelo Cedente  
                detalhe += "0"; // posi��o 62 at� 62   (1) - Identifica��o da Entrega do Bloqueto /* �0� = Postagem pelo Cedente �1� = Sacado via Correios �2� = Cedente via Ag�ncia CAIXA*/ 
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.NumeroDocumento, " ", 11); // posi��o 63 at� 73   (11)- N�mero do Documento de Cobran�a 
                detalhe += Common.CompletarCadeiaAEsquerda("", " ", 4); // posi��o 74 at� 77   (4) - Uso Exclusivo CAIXA
                detalhe += boleto.DataVencimento.ToString().Replace("/", ""); // posi��o 78 at� 85   (8) - Data de Vencimento do T�tulo
                detalhe += boleto.ValorBoleto.ToString().PadLeft(15, '0'); // posi��o 86 at� 100  (15)- Valor Nominal do T�tulo
                detalhe += Common.CompletarCadeiaAEsquerda("", "0", 5); // O sistema atribui AEC pelo CEP do sacado  // posi��o 101 at� 105 (5) - AEC = Ag�ncia Encarregada da Cobran�a
                detalhe += Common.CompletarCadeiaAEsquerda("", "0", 1); // posi��o 106 at� 106 (1) - D�gito Verificador da Ag�ncia

                string EspDoc = boleto.Especie.Sigla.Equals("DM") ? "02" : boleto.Especie.Codigo;
                detalhe += Common.CompletarCadeiaAEsquerda(EspDoc, "0", 2); // posi��o 107 at� 108 (2) - Esp�cie do T�tulo
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.Aceite, " ", 1); // posi��o 109 at� 109 (1) - Identific. de T�tulo Aceito/N�o Aceito
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.DataDocumento.ToString().Replace("/", ""), "0", 8); // posi��o 110 at� 117 (8) - Data da Emiss�o do T�tulo

                #region C�digo de juros

                string codJurosMora;
                if (boleto.JurosMora == 0 && boleto.PercentualJurosMora == 0)
                    codJurosMora = "3";
                else
                    codJurosMora = "1";

                #endregion

                detalhe += codJurosMora; // posi��o 118 at� 118 (1) - C�digo do Juros de Mora
                detalhe += boleto.DataJurosMora.ToString().Replace("/", ""); // posi��o 119 at� 126 (8) - Data do Juros de Mora
                detalhe += boleto.JurosMora.ToString().PadLeft(15, '0'); // posi��o 127 at� 141 (15)- Juros de Mora por Dia/Taxa
                detalhe += "0"; // posi��o 142 at� 142 (1) -  C�digo do Desconto 1 - "0" = Sem desconto. "1"= Valor Fixo at� a data informada "2" = Percentual at� a data informada
                detalhe += boleto.DataDesconto.ToString("ddMMyyyy"); // posi��o 143 at� 150 (8) - Data do Desconto
                detalhe += boleto.ValorDesconto.ToString().PadLeft(15, '0'); // posi��o 151 at� 165 (15)- Valor/Percentual a ser Concedido
                detalhe += boleto.Iof.ToString().PadLeft(15, '0'); // posi��o 166 at� 180 (15)- Valor do IOF a ser concedido
                detalhe += boleto.ValorAbatimento.ToString().PadLeft(15, '0'); // posi��o 181 at� 195 (15)- Valor do Abatimento
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.NumeroDocumento, " ", 25); // posi��o 196 at� 220 (25)- Identifica��o do T�tulo na Empresa. Informar o N�mero do Documento - Seu N�mero (mesmo das posi��es 63-73 do Segmento P) 
                detalhe += protestar ? "1" : "3"; // posi��o 221 at� 221 (1) -  C�digo para protesto  - �1� = Protestar. "3" = N�o Protestar. "9" = Cancelamento Protesto Autom�tico
                detalhe += diasProtesto.ToString().PadLeft(2, '0'); // posi��o 222 at� 223 (2) -  N�mero de Dias para Protesto     
                detalhe += baixaDevolver ? "1" : "2"; // posi��o 224 at� 224 (1) -  C�digo para Baixa/Devolu��o �1� = Baixar / Devolver. "2" = N�o Baixar / N�o Devolver
                detalhe += diasDevolucao.ToString().PadLeft(3, '0'); // posi��o 225 at� 227 (3) - N�mero de Dias para Baixa/Devolu��o
                detalhe += "09"; // posi��o 228 at� 229 (2) - C�digo da Moeda. Informar fixo: �09� = REAL
                detalhe += Common.CompletarCadeiaAEsquerda("", "0", 10); // posi��o 230 at� 239 (10)- Uso Exclusivo CAIXA                
                detalhe += Common.CompletarCadeiaAEsquerda("", " ", 1); // posi��o 240 at� 240 (1) - Uso Exclusivo FEBRABAN/CNAB

                return detalhe;

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do Segmento P no arquivo de remessa do CNAB240 SIGCB.", ex);
            }

        }

        public string GerarDetalheSegmentoQRemessaCnab240Sigcb(Boleto boleto, int numeroRegistro, Sacado sacado)
        {
            try
            {
                #region Segmento Q

                string detalhe = Codigo.ToString().PadLeft(3, '0'); // posi��o 1 at� 3     (3) - c�digo do banco na compensa��o  
                detalhe += "0001"; // posi��o 4 at� 7     (4) - Lote de Servi�o
                detalhe += "3"; // posi��o 8 at� 8     (1) - Tipo de Registro
                detalhe += numeroRegistro.ToString().PadLeft(5, '0'); // posi��o 9 at� 13    (5) - N� Sequencial do Registro no Lote
                detalhe += "Q"; // posi��o 14 at� 14   (1) - C�d. Segmento do Registro Detalhe
                detalhe += Common.CompletarCadeiaAEsquerda("", " ", 1); // posi��o 15 at� 15   (1) - Uso Exclusivo FEBRABAN/CNAB
                detalhe += "01"; // posi��o 16 at� 17   (2) - C�digo de Movimento Remessa

                #region Regra Tipo de Inscri��o Sacado/Pagador

                string vCpfCnpjPagador = "0"; //n�o informado
                if (sacado.CpfCnpj.Length.Equals(11)) vCpfCnpjPagador = "1"; //Cpf
                else if (sacado.CpfCnpj.Length.Equals(14)) vCpfCnpjPagador = "2"; //Cnpj

                #endregion

                detalhe += vCpfCnpjPagador; // posi��o 18 at� 18   (1) - Tipo de Inscri��o 
                detalhe += sacado.CpfCnpj; // posi��o 19 at� 33   (15)- N�mero de Inscri��o da empresa
                detalhe += Common.CompletarCadeiaAEsquerda(sacado.Nome.ToUpper(), " ", 40); // posi��o 34 at� 73   (40)- Nome
                detalhe +=
                    Common.CompletarCadeiaAEsquerda(sacado.EnderecoSacado.TipoLogradouro.ToUpper() + sacado.EnderecoSacado.Logradouro.ToUpper(), " ",
                        40); // posi��o 74 at� 113  (40)- Endere�o
                detalhe += Common.CompletarCadeiaAEsquerda(sacado.EnderecoSacado.Bairro.ToUpper(), " ", 15); // posi��o 114 at� 128 (15)- Bairro
                detalhe += Common.CompletarCadeiaAEsquerda(sacado.EnderecoSacado.Cep, " ", 8); // posi��o 129 at� 133 (5)- CEP      
                // posi��o 134 at� 136 (3) - sufixo cep** j� incluso em CEP   
                detalhe += Common.CompletarCadeiaAEsquerda(sacado.EnderecoSacado.Cidade.ToUpper(), " ", 15); // posi��o 137 at� 151 (15)- Cidade
                detalhe += Common.CompletarCadeiaAEsquerda(sacado.EnderecoSacado.SiglaUf.ToUpper(), "", 2); // posi��o 152 at� 153 (15)- UF

                #region Regra Tipo de Inscri��o Avalista

                string vCpfCnpjAvalista = "0"; //n�o informado
                if (sacado.CpfCnpjAvalista.Length.Equals(11)) vCpfCnpjAvalista = "1"; //Cpf
                else if (sacado.CpfCnpjAvalista.Length.Equals(14)) vCpfCnpjAvalista = "2"; //Cnpj

                #endregion

                detalhe += vCpfCnpjAvalista; // posi��o 154 at� 154 (1) - Tipo de Inscri��o Sacador Avalialista
                detalhe += sacado.CpfCnpjAvalista; // posi��o 155 at� 169 (15)- Inscri��o Sacador Avalialista
                detalhe += Common.CompletarCadeiaAEsquerda(sacado.NomeAvalista, " ", 40); // posi��o 170 at� 209 (40)- Nome do Sacador/Avalista
                detalhe += Common.CompletarCadeiaAEsquerda("", " ", 3); // posi��o 210 at� 212 (3) - C�d. Bco. Corresp. na Compensa��o
                detalhe += Common.CompletarCadeiaAEsquerda("", " ", 20); // posi��o 213 at� 232 (20)- Nosso N� no Banco Correspondente
                detalhe += Common.CompletarCadeiaAEsquerda("", " ", 8); // posi��o 213 at� 232 (8)- Uso Exclusivo FEBRABAN/CNAB

                return detalhe;

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do Segmento Q no arquivo de remessa do CNAB240 SIGCB.", ex);
            }
        }

        public string GerarDetalheSegmentoRRemessaCnab240Sigcb()
        {
            try
            {
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Gerar DETALHE do Segmento R no arquivo de remessa do CNAB240 SIGCB.", ex);
            }
        }

        public string GerarDetalheSegmentoYRemessaCnab240Sigcb()
        {
            try
            {
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Gerar DETALHE do Segmento Y no arquivo de remessa do CNAB240 SIGCB.", ex);
            }
        }

        #endregion

        public string GerarTrailerLoteRemessaCnac240Sigcb(int numeroRegistro)
        {
            try
            {
                string trailerLote = Codigo.ToString().PadLeft(3, '0'); // posi��o 1 at� 3     (3) - c�digo do banco na compensa��o  
                trailerLote += "0001"; // posi��o 4 at� 7     (4) - Lote de Servi�o
                trailerLote += "5"; // posi��o 8 at� 8     (1) - Tipo de Registro
                trailerLote += Common.CompletarCadeiaAEsquerda("", " ", 9); // posi��o 9 at� 17    (9) - Uso Exclusivo FEBRABAN/CNAB

                #region Pega o Numero de Registros + 1(HeaderLote) + 1(TrailerLote)

                int vQtdeRegLote = numeroRegistro; // (numeroRegistro + 2);
                trailerLote += vQtdeRegLote.ToString().PadLeft(6, '0'); // posi��o 18 at� 23   (6) - Quantidade de Registros no Lote

                #endregion

                trailerLote += Common.CompletarCadeiaAEsquerda("", "0", 6); // posi��o 24 at� 29   (6) - Quantidade de T�tulos em Cobran�a
                trailerLote += Common.CompletarCadeiaAEsquerda("", "0", 15); // posi��o 30 at� 46  (15) - Valor Total dos T�tulos em Carteiras
                trailerLote += Common.CompletarCadeiaAEsquerda("", "0", 6); // posi��o 47 at� 52   (6) - Quantidade de T�tulos em Cobran�a
                trailerLote += Common.CompletarCadeiaAEsquerda("", "0", 15); // posi��o 53 at� 69   (15) - Valor Total dos T�tulos em Carteiras  
                trailerLote += Common.CompletarCadeiaAEsquerda("", "0", 6); // posi��o 70 at� 75   (6) - Quantidade de T�tulos em Cobran�a
                trailerLote += Common.CompletarCadeiaAEsquerda("", "0", 15); // posi��o 76 at� 92   (15)- Quantidade de T�tulos em Carteiras 
                trailerLote += Common.CompletarCadeiaAEsquerda("", " ", 31); // posi��o 93 at� 123  (31) - Uso Exclusivo FEBRABAN/CNAB
                trailerLote += Common.CompletarCadeiaAEsquerda("", " ", 117); // posi��o 124 at� 240  (117)- Uso Exclusivo FEBRABAN/CNAB 

                return trailerLote;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do lote no arquivo de remessa do CNAB400.", ex);
            }
        }

        public string GerarTrailerRemessaCnab240Sigcb(int numeroRegistro)
        {
            try
            {
                string trailer = Codigo.ToString().PadLeft(3, '0'); // posi��o 1 at� 3     (3) - c�digo do banco na compensa��o   
                trailer += "9999"; // posi��o 4 at� 7     (4) - Lote de Servi�o
                trailer += "9"; // posi��o 8 at� 8     (1) - Tipo de Registro
                trailer += Common.CompletarCadeiaAEsquerda("", " ", 9); // posi��o 9 at� 17     (9) - Uso Exclusivo FEBRABAN/CNAB
                trailer += Common.CompletarCadeiaAEsquerda("1".PadLeft(6, '0'), "0", 6); // posi��o 18 at� 23   (6) - Quantidade de Lotes do Arquivo

                #region Pega o Numero de Registros + 1(HeaderRemessa) + 1(HeaderLote) + 1(TrailerLote) + 1(TrailerRemessa)

                int vQtdeRegRemessa = numeroRegistro; // (numeroRegistro + 4);
                trailer += Common.CompletarCadeiaAEsquerda(vQtdeRegRemessa.ToString().PadLeft(6, '0'), "0", 6); // posi��o 24 at� 29   (6) - Quantidade de Registros do Arquivo

                #endregion

                trailer += Common.CompletarCadeiaAEsquerda("", " ", 6); // posi��o 30 at� 35   (6) - Uso Exclusivo FEBRABAN/CNAB
                trailer += Common.CompletarCadeiaAEsquerda("", " ", 205); // posi��o 36 at� 240(205) - Uso Exclusivo FEBRABAN/CNAB

                return trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        #endregion

        #region CNAB 400 - sidneiklein

        public bool ValidarRemessaCnab400(string numeroConvenio, IBanco banco, Cedente cedente, List<Boleto> boletos,
            int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //

            #region Pr� Valida��es

            if (banco == null)
            {
                vMsg += String.Concat("Remessa: O Banco � Obrigat�rio!", Environment.NewLine);
                vRetorno = false;
            }
            if (cedente == null)
            {
                vMsg += String.Concat("Remessa: O Cedente/Benefici�rio � Obrigat�rio!", Environment.NewLine);
                vRetorno = false;
            }
            if (boletos == null || boletos.Equals(0))
            {
                vMsg += String.Concat("Remessa: Dever� existir ao menos 1 boleto para gera��o da remessa!",
                    Environment.NewLine);
                vRetorno = false;
            }

            #endregion

            //
            foreach (Boleto boleto in boletos)
            {
                #region Valida��o de cada boleto

                if (boleto.Remessa == null)
                {
                    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento,
                        "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                else
                {
                    //#region Valida��es da Remessa que dever�o estar preenchidas quando CAIXA
                    //if (String.IsNullOrEmpty(boleto.Remessa.Ambiente))
                    //{
                    //    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Tipo Documento!", Environment.NewLine);
                    //    vRetorno = false;
                    //}
                    //#endregion
                }

                #endregion
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }

        #region CNAB400

        /// <summary>
        /// Gera o HEADER do arquivo de remessa conforme layout especificado.
        /// </summary>
        /// <param name="numeroConvenio"></param>
        /// <param name="cedente"></param>
        /// <param name="numeroArquivoRemessa"></param>
        /// <returns></returns>
        public string GerarHeaderRemessaCnab400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string header = "0"; //001-001 - Tipo de Registro
                header += "1"; //002-002 - Identificador Remessa
                header += "REMESSA"; //003-009 REM.TST
                header += "01"; //010-011 - Tipo de Serviço
                header += Common.CompletarCadeiaAEsquerda("COBRANCA", " ", 15); //012-026 - Literal do Serviço
                header += Common.CompletarCadeiaAEsquerda(cedente.ContaBancariaCedente.Agencia, " ", 4);
                header += Common.CompletarCadeiaAEsquerda(cedente.CodigoCedente, " ", 6); //027-042 - Código do Cedente
                header += Common.CompletarCadeiaAEsquerda("", " ", 10); //043-046 - Uso Exclusivo CAIXA
                header += Common.CompletarCadeiaAEsquerda(cedente.Nome.ToUpper(), " ", 30); //047-076 - Nome do Cedente
                header += CodigoBanco.PadLeft(3, '0'); //077-079 - Código do Banco
                header += Common.CompletarCadeiaAEsquerda("C ECON FEDERAL", " ", 15); //080-094 - Nome do Banco
                header += DateTime.Now.ToString("ddMMyy"); //095-100 - Data da Geração do Arquivo
                header += Common.CompletarCadeiaAEsquerda("", " ", 289);  //101-389 - Uso Exclusivo CAIXA
                header += Common.CompletarCadeiaAEsquerda("", " ", 5); //390-394 - Número Sequencial de Remessa
                header += "000001"; //395-400 - Número Sequencial Registro

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        public string GerarDetalheRemessaCnab400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                //Vari�veis Locais a serem Implementadas em n�vel de Config do Boleto...
                boleto.Remessa.CodigoOcorrencia = "01"; //remessa p/ CAIXA ECONOMICA FEDERAL
                //
                //base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);
                //
                string detalhe = "1"; //001-001

                #region Regra Tipo de Inscri��o Cedente

                string vCpfCnpjCedenteSomenteNumeros =
                    boleto.CedenteBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "");

                string vCpfCnpjEmi = "00";
                if (vCpfCnpjCedenteSomenteNumeros.Length.Equals(11)) vCpfCnpjEmi = "01"; //Cpf � sempre 11;
                else if (vCpfCnpjCedenteSomenteNumeros.Length.Equals(14)) vCpfCnpjEmi = "02"; //Cnpj � sempre 14;

                #endregion

                detalhe += vCpfCnpjEmi;
                detalhe += vCpfCnpjCedenteSomenteNumeros;
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.CedenteBoleto.ContaBancariaCedente.Agencia, " ", 4);
                detalhe += Common.CompletarCadeiaAEsquerda(string.Empty, " ", 6);
                detalhe += "0"; // 28-28 - Identificação Emissão
                detalhe += "0"; // 29-29 - Identificação Entrega/Distribuição
                detalhe += "00"; // 30-31 - Comissão de Permanência
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.NumeroDocumento, " ", 25); // 32-56 - Identificação Título na Empresa
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.NossoNumeroFormatado, " ", 17); // 57-73 - Noss Número
                detalhe += Common.CompletarCadeiaAEsquerda(string.Empty, " ", 3); // 74-76 - Campos em branco
                detalhe += Common.CompletarCadeiaAEsquerda(string.Empty, " ", 30); //77-106 - Mensagem a ser impressa
                detalhe += boleto.CarteiraCobranca.Codigo; // 107-108
                detalhe += "00"; // 109-110 - Tipo Ocorrência Arquivo Remessa
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.NumeroDocumento, " ", 10); //111-120
                detalhe += boleto.DataVencimento.ToString().ToDateTimeFromDdMmAa(); //121-126
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.ValorBoleto.ToString(), "0", 15); //127-139
                detalhe += Common.CompletarCadeiaAEsquerda(CodigoBanco, "0", 3);
                detalhe += "00000";
                detalhe += boleto.Especie.Codigo;
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.Aceite, " ", 1);
                if (boleto.DataDocumento == DateTime.MinValue)
                    boleto.DataDocumento = DateTime.Now;
                detalhe += boleto.DataDocumento.ToString().ToDateTimeFromDdMmAa();

                #region Instru��es

                string vInstrucao1 = "0";
                string vInstrucao2 = "0";
                string vInstrucao3 = "0";
                switch (boleto.InstrucoesDoBoleto.Count)
                {
                    case 1:
                        vInstrucao1 = boleto.InstrucoesDoBoleto[0].Codigo.ToString();
                        vInstrucao2 = "0";
                        vInstrucao3 = "0";
                        break;
                    case 2:
                        vInstrucao1 = boleto.InstrucoesDoBoleto[0].Codigo.ToString();
                        vInstrucao2 = boleto.InstrucoesDoBoleto[1].Codigo.ToString();
                        vInstrucao3 = "0";
                        break;
                    case 3:
                        vInstrucao1 = boleto.InstrucoesDoBoleto[0].Codigo.ToString();
                        vInstrucao2 = boleto.InstrucoesDoBoleto[1].Codigo.ToString();
                        vInstrucao3 = boleto.InstrucoesDoBoleto[2].Codigo.ToString();
                        break;
                }

                #endregion

                detalhe += Common.CompletarCadeiaAEsquerda(vInstrucao1, " ", 2);
                detalhe += Common.CompletarCadeiaAEsquerda(vInstrucao2, " ", 2);
                detalhe += boleto.JurosMora.ToString().PadLeft(15, '0');
                

                #region DataDesconto

                string vDataDesconto = "000000";
                if (!boleto.DataDesconto.Equals(DateTime.MinValue))
                    vDataDesconto = boleto.DataDesconto.ToString("ddMMyy");

                #endregion

                detalhe += vDataDesconto;
                detalhe += boleto.ValorDesconto.ToString().PadLeft(15, '0');
                detalhe += boleto.Iof.ToString().PadLeft(15, '0');
                detalhe += boleto.ValorAbatimento.ToString().PadLeft(15, '0');

                #region Regra Tipo de Inscri��o Sacado

                string vCpfCnpjSac = "99";
                if (boleto.SacadoBoleto.CpfCnpj.Length.Equals(11)) vCpfCnpjSac = "01"; //Cpf � sempre 11;
                else if (boleto.SacadoBoleto.CpfCnpj.Length.Equals(14)) vCpfCnpjSac = "02"; //Cnpj � sempre 14;

                #endregion

                detalhe += vCpfCnpjSac;
                detalhe += boleto.SacadoBoleto.CpfCnpj;
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.SacadoBoleto.Nome.ToUpper(), " ", 40);
                detalhe +=
                    Common.CompletarCadeiaAEsquerda(
                        boleto.SacadoBoleto.EnderecoSacado.TipoLogradouro.ToUpper() +
                        boleto.SacadoBoleto.EnderecoSacado.Logradouro.ToUpper(), " ", 40);
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.SacadoBoleto.EnderecoSacado.Bairro.ToUpper(), " ", 12);
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.SacadoBoleto.EnderecoSacado.Cep, " ", 8);
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.SacadoBoleto.EnderecoSacado.Cidade, " ", 15);
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.SacadoBoleto.EnderecoSacado.SiglaUf, " ", 2);

                #region DataMulta

                string vDataMulta = "000000";
                if (!boleto.DataMulta.Equals(DateTime.MinValue))
                    vDataMulta = boleto.DataMulta.ToString("ddMMyy");

                #endregion

                detalhe += Common.CompletarCadeiaAEsquerda(vDataMulta, " ", 6);
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.ValorMulta.ToString().PadLeft(10, '0'), " ", 10);
                detalhe += Common.CompletarCadeiaAEsquerda(boleto.SacadoBoleto.NomeAvalista, " ", 22);
                detalhe += Common.CompletarCadeiaAEsquerda(vInstrucao3, " ", 2);
                detalhe += Common.CompletarCadeiaAEsquerda(diasProtesto.ToString().PadLeft(2, '0'), " ", 2);
                detalhe += boleto.Moeda;
                detalhe += Common.CompletarCadeiaAEsquerda(numeroRegistro.ToString().PadLeft(6, '0'), " ", 6);

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        public string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal)
        {
            try
            {
                string trailer = "9"; //001-001 - Tipo de Registro
                trailer += Common.CompletarCadeiaAEsquerda("", " ", 393); //002-394 - Uso Exclusivo CAIXA
                trailer += numeroRegistro.ToString().PadLeft(6, '0'); //395-400- Número Sequencial do Registro

                return trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        #endregion

        private DetalheRetornoCnab400 LerDetalheRetornoCNAB400(string registro)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}