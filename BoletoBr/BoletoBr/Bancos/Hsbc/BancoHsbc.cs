using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using BoletoBr.CalculoModulo;
using BoletoBr.Bancos;
using BoletoBr.Dominio;

namespace BoletoBr.Bancos.Hsbc
{
    public class BancoHsbc : IBanco
    {
        #region Variáveis

        private int _digitoAutoConferenciaCodigoBarras;
        private string _digitoAutoConferenciaNossoNumero;
        private readonly List<CarteiraCobranca> _carteirasCobrancaHsbc;

        #endregion

        #region Propriedades

        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }

        #endregion

        #region Construtores

        public BancoHsbc()
        {
            CodigoBanco = "399";
            DigitoBanco = "9";
            NomeBanco = "HSBC";
            this.LocalDePagamento = "Pagável em qualquer banco até o vencimento.";
            this.MoedaBanco = "9";

            /* Adiciona carteiras de cobrança */
            _carteirasCobrancaHsbc = new List<CarteiraCobranca>();
            _carteirasCobrancaHsbc.Add(new CarteiraCobrancaHsbcCnr());
            _carteirasCobrancaHsbc.Add(new CarteiraCobrancaHsbcCsb());
        }

        #endregion

        #region Métodos de formatação do boleto

        public string LocalDePagamento { get; private set; }
        public string MoedaBanco { get; private set; }

        public List<CarteiraCobranca> GetCarteirasCobranca()
        {
            return _carteirasCobrancaHsbc;
        }

        public CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira)
        {
            return GetCarteirasCobranca().Find(fd => fd.Codigo == codigoCarteira);
        }

        /// <summary>
        /// Valida se o boleto está preenchido com os campos mínimos requeridos.
        /// Dispara uma ApplicationException caso esteja faltando alguma informação.
        /// </summary>
        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            //Verifica as carteiras implementadas
            if (!boleto.CarteiraCobranca.Codigo.Equals("CSB") &
                !boleto.CarteiraCobranca.Codigo.Equals("CNR"))
                throw new NotImplementedException("Carteira n�o implementada. Utilize a carteira 'CSB' ou 'CNR'.");

            //Verifica se o nosso n�mero � v�lido
            if (boleto.NossoNumeroFormatado.ToStringSafe() == string.Empty)
                throw new NotImplementedException("Nosso número inválido");

            //Verifica se o nosso n�mero � v�lido
            if (boleto.NossoNumeroFormatado.ToStringSafe().ToLong() == 0)
                throw new NotImplementedException("Nosso número inválido");

            //Verifica se o tamanho para o NossoNumero s�o 10 d�gitos (5 range + 5 numero sequencial) - Válido para carteira CSB
            if (boleto.CarteiraCobranca.Codigo.Equals("CSB"))
            if (Convert.ToInt32(boleto.NossoNumeroFormatado).ToString().Length > 10)
                throw new NotImplementedException("A quantidade de dígitos do nosso número para a carteira " +
                                                  boleto.CarteiraCobranca.Codigo + ", são 10 números.");
            else if (Convert.ToInt32(boleto.NossoNumeroFormatado).ToString().Length < 10)
                boleto.SetNossoNumeroFormatado(boleto.NossoNumeroFormatado.PadLeft(10, '0'));

            //Verifica se data do documento � valida
            //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataDocumento == DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;
        }

        public void FormataMoedaBoleto(Boleto boleto)
        {
            if (string.IsNullOrEmpty(boleto.Moeda))
                throw new Exception("Espécie/Moeda para o boleto não foi informada.");

            if ((boleto.Moeda == "9") || (boleto.Moeda == "REAL") || (boleto.Moeda == "R$"))
                boleto.Moeda = "R$";
            else
                boleto.Moeda = "0";
        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            boleto.NumeroDocumento = boleto.NumeroDocumento.PadLeft(7, '0');
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            try
            {
                if (boleto.CarteiraCobranca.Codigo == "CSB")
                {
                    string nossoNumeroComposto =
                        boleto.CedenteBoleto.CodigoCedente.PadLeft(5, '0')
                        +
                        boleto.SequencialNossoNumero.PadLeft(5, '0');

                    string digitoAutoConferenciaNossoNumero = Common.Mod11(nossoNumeroComposto, 7).ToString();

                    string nossoNumeroFormatado =
                        nossoNumeroComposto + digitoAutoConferenciaNossoNumero;

                    boleto.SetNossoNumeroFormatado(nossoNumeroFormatado);
                    return;
                }
                if (boleto.CarteiraCobranca.Codigo == "CNR")
                {
                    /* Seguindo documentação CNR - Cobrança Não Registrada
                     * Disponível em: https://www.hsbc.com.br/1/PA_esf-ca-app-content/content/hbbr-pws-gip16/portugues/business/comum/pdf/cnrbarra.pdf
                     */

                    string codigoDoPagador = boleto.SequencialNossoNumero;
                    string primeiroDigitoVerificador =
                        CalculaPrimeiroDigitoVerificadorCnrTipo4(boleto.SequencialNossoNumero);
                    string segundoDigitoVerificador =
                        CalculaSegundoDigitoVerificadorCnrTipo4(boleto.SequencialNossoNumero,
                            primeiroDigitoVerificador, boleto.CedenteBoleto.CodigoCedente,
                            boleto.DataVencimento);

                    boleto.SetNossoNumeroFormatado(
                        String.Format("{0}{1}4{2}",
                            codigoDoPagador,
                            primeiroDigitoVerificador,
                            segundoDigitoVerificador));

                    /* Padroniza com 16 dígitos */
                    boleto.SetNossoNumeroFormatado(
                        boleto.NossoNumeroFormatado.PadLeft(16, '0'));
                    return;
                }

                throw new NotImplementedException("Modelo de carteira de cobrança: " + boleto.CarteiraCobranca.Codigo +
                                                  " não está implementado.");
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao tentar formatar nosso número.", ex);
            }
        }

        public void FormataLinhaDigitavel(Boleto boleto)
        {
            string nossoNumeroLinhaDigitavel = boleto.NossoNumeroFormatado.PadLeft(13, '0');
            string codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0');
            string digitoAutoConferenciaNossoNumero = Common.Mod11(boleto.NossoNumeroFormatado, 7).ToString();

            string C1 = string.Empty;
            string C2 = string.Empty;
            string C3 = string.Empty;
            string C5 = string.Empty;

            string AAA;
            string B;
            string CCCCC;
            string X;


            string DDDDDD;
            string DD;
            string EEEE;
            string EEEEEEEE;
            string Y;
            string FFFFFFF;
            string FFFFF;
            string GGGGG;
            string Z;

            if (boleto.CarteiraCobranca.Codigo == "CSB")
            {
                #region AAABC.CCCCX

                AAA = this.CodigoBanco.PadLeft(3, '0');
                B = boleto.Moeda.ToString();
                CCCCC = boleto.NossoNumeroFormatado.Substring(0, 5);
                X = Common.Mod10(AAA + B + CCCCC).ToString();

                C1 = String.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
                C1 += String.Format("{0}{1} ", CCCCC.Substring(1, 4), X);

                #endregion

                #region DDDDD.DEEEEY

                DDDDDD = boleto.NossoNumeroFormatado.Substring(5, 5) + digitoAutoConferenciaNossoNumero;
                EEEE = boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0');
                Y = Common.Mod10(DDDDDD + EEEE).ToString();

                C2 = String.Format("{0}.", DDDDDD.Substring(0, 5));
                C2 += string.Format("{0}{1}{2} ", DDDDDD.Substring(5, 1), EEEE, Y);

                #endregion

                #region FFFFF.FF001Z

                FFFFFFF = boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(7, '0');
                Z = Common.Mod10(FFFFFFF + "001").ToString();

                C3 = String.Format("{0}.", FFFFFFF.Substring(0, 5));
                C3 += String.Format("{0}001{1}", FFFFFFF.Substring(5, 2), Z);

                #endregion
            }
            if (boleto.CarteiraCobranca.Codigo == "CNR")
            {
                #region AAABC.CCCCX

                AAA = this.CodigoBanco.PadLeft(3, '0');
                B = boleto.Moeda.ToString();
                CCCCC = boleto.CedenteBoleto.CodigoCedente.Substring(0, 5);
                X = Common.Mod10(AAA + B + CCCCC).ToString();

                C1 = string.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
                C1 += string.Format("{0}{1} ", CCCCC.Substring(1, 4), X);

                #endregion AAABC.CCDDX

                #region DDEEE.EEEEEY

                DD = boleto.CedenteBoleto.CodigoCedente.Substring(5, 2);
                EEEEEEEE = nossoNumeroLinhaDigitavel.Substring(0, 8);
                Y = Common.Mod10(DD + EEEEEEEE).ToString();

                C2 = string.Format("{0}{1}.", DD, EEEEEEEE.Substring(0, 3));
                C2 += string.Format("{0}{1} ", EEEEEEEE.Substring(3, 5), Y);

                #endregion DDEEE.EEEEEY

                #region FFFFF.GGGGGZ

                FFFFF = nossoNumeroLinhaDigitavel.Substring(8, 5);
                GGGGG =
                    (boleto.DataVencimento.DayOfYear + boleto.DataVencimento.ToString("yy").Substring(1, 1)).PadLeft(4,
                        '0') + "2";

                Z = Common.Mod10(FFFFF + GGGGG).ToString();

                C3 = string.Format("{0}.", FFFFF);
                C3 += string.Format("{0}{1}", GGGGG, Z);

                #endregion FFFFF.GGGGGZ
            }

            string W = String.Format(" {0} ", _digitoAutoConferenciaCodigoBarras);

            #region HHHHIIIIIIIIII

            string HHHH = Common.FatorVencimento(boleto.DataVencimento).ToString();
            string IIIIIIIIII = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");

            IIIIIIIIII = IIIIIIIIII.PadLeft(10, '0');
            C5 = HHHH + IIIIIIIIII;

            #endregion HHHHHHHHHHHHHH

            boleto.LinhaDigitavelBoleto = C1 + C2 + C3 + W + C5;
        }

        public void FormataCodigoBarra(Boleto boleto)
        {
            try
            {
                /* Preenche com 0´s a esquerda
                 * 10 caracteres
                 */
                string valorBoletoTexto =
                    boleto.ValorBoleto.ToString("f")
                        .Replace(",", "")
                        .Replace(".", "")
                        .PadLeft(10, '0');

                string numeroDocumentoFormatado =
                    boleto.NumeroDocumento.PadLeft(7, '0');

                string codigoBarraSemDigitoVerificador = null;

                if (boleto.CarteiraCobranca.Codigo == "CSB")
                {
                    codigoBarraSemDigitoVerificador =
                        String.Format("{0}{1}{2}{3}{4}{5}{6}001",
                            this.CodigoBanco,
                            boleto.Moeda,
                            //9999 --> 21/02/2025
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoletoTexto,
                            boleto.NossoNumeroFormatado + boleto.DigitoNossoNumero,
                            boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'),
                            boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(7, '0')
                            );
                }
                if (boleto.CarteiraCobranca.Codigo == "CNR")
                {
                    codigoBarraSemDigitoVerificador =
                        String.Format("{0}{1}{2}{3}{4}{5}{6}2",
                            this.CodigoBanco,
                            boleto.Moeda,
                            //9999 --> 21/02/2025
                            Common.FatorVencimento(boleto.DataVencimento),
                            valorBoletoTexto,
                            boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0'),
                            boleto.SequencialNossoNumero.PadLeft(13, '0'),
                            (boleto.DataVencimento.DayOfYear +
                             boleto.DataVencimento.ToString("yy").Substring(1, 1)).PadLeft(4, '0')
                            );
                }

                /* 
                 * 1. Calcula dígito de auto conferência
                 * 2. Insere no meio do código de barras
                 * 3. Atribui ao boleto
                 */
                string codigoBarraComDigitoVerificador = null;

                _digitoAutoConferenciaCodigoBarras = Common.Mod11(codigoBarraSemDigitoVerificador, 9, 0);

                codigoBarraComDigitoVerificador =
                    Common.Left(codigoBarraSemDigitoVerificador, 4) +
                    _digitoAutoConferenciaCodigoBarras +
                    Common.Right(codigoBarraSemDigitoVerificador, 39);

                boleto.CodigoBarraBoleto = codigoBarraComDigitoVerificador;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao formatar código de barras.", ex);
            }
        }

        public void FormatarBoleto(Boleto boleto)
        {
            boleto.ValidaDadosEssenciaisDoBoleto();

            ValidaBoletoComNormasBanco(boleto);

            // Calcula o DAC do Nosso N�mero
            // Nosso N�mero = Range(5) + Numero Sequencial(5)
            _digitoAutoConferenciaNossoNumero = Common.Mod11(boleto.NossoNumeroFormatado, 7).ToString();

            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento = "PAGAR PREFERENCIALMENTE EM AGÊNCIAS DO " + NomeBanco;

            FormataNumeroDocumento(boleto);
            FormataNossoNumero(boleto);
            FormataLinhaDigitavel(boleto);
            FormataCodigoBarra(boleto);
        }

        /// <summary>
        /// Calcula primeiro dígito verificador
        /// </summary>
        /// <param name="codigoPagador">Equivalente a número do documento.</param>
        /// <returns></returns>
        public string CalculaPrimeiroDigitoVerificadorCnrTipo4(string codigoPagador)
        {
            return Common.Mod11Base9(codigoPagador).ToString();
        }

        public string CalculaSegundoDigitoVerificadorCnrTipo4(string codigoPagador, string primeiroDigitoVerificador,
            string codigoBeneficiario, DateTime dataVencimento)
        {
            return Common.Mod11Base9(
                (
                    long.Parse(codigoPagador + primeiroDigitoVerificador + "4") +
                    long.Parse(codigoBeneficiario) +
                    long.Parse(dataVencimento.ToString("ddMMyy"))
                    )
                    .ToString()
                )
                .ToString();
        }

        #endregion

        #region Métodos de geração do arquivo remessa

        #region Header

        /// <summary>
        /// HEADER do arquivo CNAB
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public string GerarHeaderRemessa(IBanco banco, TipoArquivo tipoArquivo, Sacado nomeSacado, int numeroArquivoRemessa, DateTime ddmmaaaaGeracao, DateTime hhmmssGeracao)
        {
            throw new Exception("Função não implementada");
            //try
            //{
            //    string _header = " ";

            //    base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

            //    switch (tipoArquivo)
            //    {
            //        case TipoArquivo.Cnab240:
            //            if (boletos.Remessa.TipoDocumento.Equals("2") || boletos.Remessa.TipoDocumento.Equals("1"))
            //                _header = GerarHeaderRemessaCNAB240(/*cedente*/); --> Banco CEF pede Cedente, confirmar na documentação do HSBC
            //            else
            //                _header = GerarHeaderRemessaCNAB240(/*cedente*/); --> Banco CEF pede Cedente, confirmar na documentação do HSBC
            //            break;
            //        case TipoArquivo.Cnab400:
            //            _header = GerarHeaderRemessaCNAB400(0, cedente, numeroArquivoRemessa);
            //            break;
            //        case TipoArquivo.Outro:
            //            throw new Exception("Tipo de arquivo inexistente.");
            //    }

            //    return _header;

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Erro durante a gera��o do HEADER do arquivo de REMESSA.", ex);
            //}
        }

        public string GerarHeaderRemessa(IBanco banco, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            throw new Exception("Função não implementada");
        }

        public string GerarHeaderRemessaCnab240()
        {
            throw new Exception("Função não implementada");
        }

        public string GerarHeaderRemessaCnab400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            throw new Exception("Função não implementada");
        }

        public string GerarHeaderRemessa(string numeroConvenio, Cedente cendente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        public string GerarHeaderRemessa(string numeroConvenio, Cedente cendente, TipoArquivo tipoArquivo, int numeroArquivoRemessa,
            List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }

        # endregion

        #region Detalhe

        /// <summary>
        /// DETALHE do arquivo CNAB
        /// Gera o DETALHE do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }

        public string GerarHeaderRemessa(Cedente cendente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheRemessaCNAB240()
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }

        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }

        # endregion DETALHE

        # region Trailer

        /// <summary>
        /// TRAILER do arquivo CNAB
        /// Gera o TRAILER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente,
            decimal vltitulostotal)
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }

        public string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cendente, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        public string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cendente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
        }

        public string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cendente, int numeroArquivoRemessa, TipoArquivo tipoArquivo,
            Boleto boletos)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente,
            Boleto boletos)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, Sacado sacado)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
        }

        public string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            throw new NotImplementedException();
        }

        public string GerarTrailerArquivoRemessa(int numeroRegistro, Boleto boletos)
        {
            throw new NotImplementedException();
        }

        public string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            throw new NotImplementedException();
        }

        public string GerarTrailerLoteRemessa(int numeroRegistro, Boleto boletos)
        {
            throw new NotImplementedException();
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

        public void LerDetalheRetornoCnab400(string registro)
        {
            LerDetalheRetornoCnab400(registro);
        }

        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }

        public string GerarTrailerRemessa400(int numeroRegistro)
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }

        # endregion

        #endregion

        #region Métodos de validação para geração do arquivo remessa

        /// <summary>
        /// Efetua as Valida��es dentro da classe Boleto, para garantir a gera��o da remessa
        /// </summary>
        public bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente,
            List<Boleto> boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            switch (tipoArquivo)
            {
                case TipoArquivo.Cnab240:
                    vRetorno = ValidarRemessaCnab240(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
                    break;
                case TipoArquivo.Cnab400:
                    vRetorno = ValidarRemessaCnab400(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
                    break;
                case TipoArquivo.Outro:
                    throw new Exception("Tipo de arquivo inexistente.");
            }
            mensagem = vMsg;
            return vRetorno;
        }

        public bool ValidarRemessaCnab240(string numeroConvenio, IBanco banco, Cedente cedente, List<Boleto> boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //

            #region Pré Validações
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
            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += String.Concat("Remessa: Dever� existir ao menos 1 boleto para gera��o da remessa!", Environment.NewLine);
                vRetorno = false;
            }
            #endregion

            //
            foreach (Boleto boleto in boletos)
            {
                #region Validação de cada boleto
                if (boleto.Remessa == null)
                {
                    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                else if (boleto.Remessa.TipoDocumento.Equals("1") && String.IsNullOrEmpty(boleto.SacadoBoleto.EnderecoSacado.Cep)) //1 - SICGB - Com registro
                {
                    //Para o "Remessa.TipoDocumento = "1", o CEP � Obrigat�rio!
                    vMsg += String.Concat("Para o Tipo Documento [1 - SIGCB - COM REGISTRO], o CEP do SACADO � Obrigat�rio!", Environment.NewLine);
                    vRetorno = false;
                }
                if (boleto.NossoNumeroFormatado.Length > 15)
                    boleto.SetNossoNumeroFormatado(boleto.NossoNumeroFormatado.Substring(0, 15));
            }
                #endregion
            //
            mensagem = vMsg;
            return vRetorno;
        }

        public bool ValidarRemessaCnab400(string numeroConvenio, IBanco banco, Cedente cedente, List<Boleto> boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //

            #region Pré Validações

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
            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += String.Concat("Remessa: Dever� existir ao menos 1 boleto para gera��o da remessa!",
                    Environment.NewLine);
                vRetorno = false;
            }

            #endregion

            //
            foreach (Boleto boleto in boletos)
            {
                #region Validação de cada boleto

                if (boleto.Remessa == null)
                {
                    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento,
                        "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }

                #endregion
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }
        #endregion
    }
}
