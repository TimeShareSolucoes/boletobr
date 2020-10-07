﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Santander
{


    /*
        * 033 - Banco Santander
        * Carteira 101 - Cobrança Simples Rápida com Registro - RCR
        * Carteira 102 - Cobrança Simples sem Registro - CSR
        * Carteira 104 - Cobrança Simples Rápida com Registro - RCR - Banco emite
        * Carteira 201 - Cobrança Penhor Rápida com Registro - RCR
        * Carteira COB - Cobrança simples, sem registro (antiga)
        * Carteira CSR - Cobrança Simples sem Registro (antiga)
        * Carteira ECR - Cobrança Simples com Registro (antiga)
        */

    /* Códigos do Banco
     * 008-6 -> Banco Santander Meridional S.A.
     * 033-4 -> Banco do Estado de São Paulo S.A. - Banespa
     * 353-0 -> Banco Santander Brasil S.A.
     * Atualmente o banco Santander recepciona boletos/remessa sob o código 033-7
     */

    public class BancoSantander : IBanco
    {
        public string CodigoBanco { get; set; }
        public string DigitoBanco { get; set; }
        public string NomeBanco { get; set; }
        public Image LogotipoBancoParaExibicao { get; set; }
        public string LocalDePagamento { get; private set; }
        public string MoedaBanco { get; private set; }

        public BancoSantander()
        {
            CodigoBanco = "033";
            DigitoBanco = "7";
            NomeBanco = "Santander";
            LocalDePagamento = "Pagar preferencialmente no banco santander";
            MoedaBanco = "9";
        }

        public void ValidaBoletoComNormasBanco(Boleto boleto)
        {
            boleto.SetNossoNumeroFormatado(boleto.NossoNumeroFormatado.Replace("-", ""));

            //throw new NotImplementedException("Função não implementada.");
            if (
                !((boleto.CarteiraCobranca.Codigo == "102") || (boleto.CarteiraCobranca.Codigo == "101") ||
                  (boleto.CarteiraCobranca.Codigo == "201")|| (boleto.CarteiraCobranca.Codigo == "104")))
                throw new NotImplementedException("Carteira não implementada.");

            //Banco 008  - Utilizar somente 09 posições do Nosso Numero (08 posições + DV), zerando os 04 primeiros dígitos
            if (CodigoBanco == "008")
            {
                if (boleto.NossoNumeroFormatado.Length != 8)
                    throw new NotImplementedException("Nosso Número deve ter 8 posições para o banco 008.");
            }

            if (CodigoBanco == "033")
            {
                if (boleto.NossoNumeroFormatado.Length == 7 && boleto.CarteiraCobranca.Codigo.Equals("101"))
                    boleto.SetNossoNumeroFormatado(boleto.IdentificadorInternoBoleto.PadLeft(13, '0'));

                if (boleto.NossoNumeroFormatado.Length != 13)
                    throw new NotSupportedException("Nosso Número deve ter 13 posições para o banco 033.");
            }

            //Banco 353  - Utilizar somente 08 posições do Nosso Numero (07 posições + DV), zerando os 05 primeiros dígitos
            if (CodigoBanco == "353")
            {
                if (boleto.NossoNumeroFormatado.Length != 7)
                    throw new NotImplementedException("Nosso Número deve ter 7 posições para o banco 353.");
            }

            if (boleto.CedenteBoleto.CodigoCedente.ToString().Length > 7)
                throw new NotImplementedException("Código cedente deve ter no máximo 7 posições.");

            if (EspecieDocumento.ValidaSigla(boleto.Especie) == "")
                boleto.Especie = new EspecieDocumento(Convert.ToInt32("02"));

            if (boleto.PercentualIOS > 10 &
                (CodigoBanco == "008" || CodigoBanco == "033" || CodigoBanco == "353"))
                throw new Exception("O percentual do IOS é limitado a 9% para o Banco Santander");

            var nossoNumeroFormatadoA = boleto.NossoNumeroFormatado.Substring(0, boleto.NossoNumeroFormatado.Length - 1);
            var nossoNumeroFormatadoB = boleto.NossoNumeroFormatado.Remove(0, boleto.NossoNumeroFormatado.Length - 1);
            boleto.SetNossoNumeroFormatado(string.Format("{0}-{1}", nossoNumeroFormatadoA, nossoNumeroFormatadoB));

            /* TAMANHO DO CODIGO DE BARRAS DEVE SER 44 POSIÇÕES, CASO NÃO, GERAR EXCEPTION DE CODIGO DE BARRAS INVALIDO */
            if (boleto.CodigoBarraBoleto.Length != 44)
                throw new Exception(
                    "O código de barras gerado para o boleto é invalido, verifique as configurações de carteira.");
        }

        public void FormataMoeda(Boleto boleto)
        {
            boleto.Moeda = MoedaBanco;

            if (String.IsNullOrEmpty(boleto.Moeda))
                throw new Exception("Espécie/Moeda para o boleto não foi informada.");

            if ((boleto.Moeda == "9") || (boleto.Moeda == "REAL") || (boleto.Moeda == "R$"))
                boleto.Moeda = "R$";
            else
                boleto.Moeda = "8";
        }

        public void FormatarBoleto(Boleto boleto)
        {
            //Atribui local de pagamento
            boleto.LocalPagamento = LocalDePagamento;

            boleto.ValidaDadosEssenciaisDoBoleto();

            FormataNumeroDocumento(boleto);
            FormataNossoNumero(boleto);
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataMoeda(boleto);

            ValidaBoletoComNormasBanco(boleto);

            boleto.CedenteBoleto.CodigoCedenteFormatado = String.Format("{0}/{1}",
                boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'),
                boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0'));
        }

        /// <summary>
        /// 
        ///   *******
        /// 
        ///	O Código de barra para cobrança contém 44 posições dispostas da seguinte forma:
        ///    01 a 03 -  3 - 033 fixo - Código do banco
        ///    04 a 04 -  1 - 9 fixo - Código da moeda (R$)
        ///    05 a 05 -  1 - Dígito verificador do Código de barras
        ///    06 a 09 -  4 - Fator de vencimento
        ///    10 a 19 - 10 - Valor
        ///    20 a 20 -  1 - Fixo 9
        ///    21 a 27 -  7 - Código do cedente padrão satander
        ///    28 a 40 - 13 - Nosso número
        ///    41 - 41 - 1 -  IOS  - Seguradoras(Se 7% informar 7. Limitado  a 9%) Demais clientes usar 0 
        ///    42 - 44 - 3 - Tipo de modalidade da carteira 101, 102, 201
        /// 
        ///   *******
        /// 
        /// </summary>
        public void FormataCodigoBarra(Boleto boleto)
        {
            var codigoBanco = CodigoBanco.PadLeft(3, '0'); //3
            var codigoMoeda = MoedaBanco; //1
            var fatorVencimento = Common.FatorVencimento(boleto.DataVencimento).ToString().PadLeft(4, '0'); //4
            var valorNominal = boleto.ValorBoleto.ToStringParaValoresDecimais().PadLeft(10, '0'); //10
            const string fixo = "9"; //1
            var codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0'); //7
            var nossoNumero = boleto.NossoNumeroFormatado.Replace("-", "").Replace(",", "").Replace(".", "").PadLeft(13, '0'); //13
            var ios = boleto.PercentualIOS.ToString(); //1
            var tipoCarteira = boleto.CarteiraCobranca.Codigo; //3;

            boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", codigoBanco, codigoMoeda,
                fatorVencimento, valorNominal, fixo, codigoCedente, nossoNumero, ios, tipoCarteira);

            var calculoDv = Mod10Mod11Santander(boleto.CodigoBarraBoleto, 9).ToString();

            boleto.CodigoBarraBoleto = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", codigoBanco, codigoMoeda,
                calculoDv, fatorVencimento, valorNominal, fixo, codigoCedente, nossoNumero, ios, tipoCarteira);
        }

        /// <summary>
        /// 
        ///   *******
        /// 
        ///	A Linha Digitavel para cobrança contém 44 posições dispostas da seguinte forma:
        ///   1º Grupo - 
        ///    01 a 03 -  3 - 033 fixo - Código do banco
        ///    04 a 04 -  1 - 9 fixo - Código da moeda (R$) outra moedas 8
        ///    05 a 05 –  1 - Fixo 9
        ///    06 a 09 -  4 - Código cedente padrão santander
        ///    10 a 10 -  1 - Código DV do primeiro grupo
        ///   2º Grupo -
        ///    11 a 13 –  3 - Restante do código cedente
        ///    14 a 20 -  7 - 7 primeiros campos do nosso número
        ///    21 a 21 - 13 - Código DV do segundo grupo
        ///   3º Grupo -  
        ///    22 - 27 - 6 -  Restante do nosso número
        ///    28 - 28 - 1 - IOS  - Seguradoras(Se 7% informar 7. Limitado  a 9%) Demais clientes usar 0 
        ///    29 - 31 - 3 - Tipo de carteira
        ///    32 - 32 - 1 - Código DV do terceiro grupo
        ///   4º Grupo -
        ///    33 - 33 - 1 - Composto pelo DV do código de barras
        ///   5º Grupo -
        ///    34 - 36 - 4 - Fator de vencimento
        ///    37 - 47 - 10 - Valor do título
        ///   *******
        /// 
        /// </summary>
        public void FormataLinhaDigitavel(Boleto boleto)
        {
            var nossoNumero = boleto.NossoNumeroFormatado.Replace("-", "").PadLeft(13, '0'); //13
            var codigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0');
            var fatorVencimento = Common.FatorVencimento(boleto.DataVencimento).ToString();
            var ios = boleto.PercentualIOS.ToString(); //1

            #region Grupo1

            var codigoBanco = CodigoBanco.PadLeft(3, '0'); //3
            var codigoModeda = MoedaBanco; //1
            const string fixo = "9"; //1
            var codigoCedente1 = codigoCedente.Substring(0, 4); //4
            var calculoDv1 =
                Common.Mod10(string.Format("{0}{1}{2}{3}", codigoBanco, codigoModeda, fixo, codigoCedente1))
                    .ToString(CultureInfo.InvariantCulture); //1

            var grupo1 = string.Format("{0}{1}{2}.{3}{4}", codigoBanco, codigoModeda, fixo, codigoCedente1,
                calculoDv1);

            #endregion

            #region Grupo2

            var codigoCedente2 = codigoCedente.Substring(4, 3); //3
            var nossoNumero1 = nossoNumero.Substring(0, 7); //7
            var calculoDv2 =
                Common.Mod10(string.Format("{0}{1}", codigoCedente2, nossoNumero1))
                    .ToString(CultureInfo.InvariantCulture);
            var grupo2 = string.Format("{0}{1}{2}", codigoCedente2, nossoNumero1, calculoDv2);

            grupo2 = " " + grupo2.Substring(0, 5) + "." + grupo2.Substring(5, 6);

            #endregion

            #region Grupo3

            var nossoNumero2 = nossoNumero.Substring(7, 6); //6
            var tipoCarteira = boleto.CarteiraCobranca.Codigo; //3
            var calculoDv3 =
                Common.Mod10(string.Format("{0}{1}{2}", nossoNumero2, ios, tipoCarteira))
                    .ToString(CultureInfo.InvariantCulture); //1

            var grupo3 = string.Format("{0}{1}{2}{3}", nossoNumero2, ios, tipoCarteira, calculoDv3);
            grupo3 = " " + grupo3.Substring(0, 5) + "." + grupo3.Substring(5, 6) + " ";

            #endregion

            #region Grupo4

            var dVcodigoBanco = CodigoBanco.PadLeft(3, '0'); //3
            var dVcodigoMoeda = MoedaBanco; //1
            var dVvalorNominal = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "").PadLeft(10, '0');
            //10
            const string dVfixo = "9"; //1
            var dVcodigoCedente = boleto.CedenteBoleto.CodigoCedente.PadLeft(7, '0'); //7
            var dVtipoCarteira = boleto.CarteiraCobranca.Codigo; //3;

            var calculoDVcodigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                dVcodigoBanco, dVcodigoMoeda, fatorVencimento, dVvalorNominal, dVfixo, dVcodigoCedente, nossoNumero,
                ios, dVtipoCarteira);

            var grupo4 = Mod10Mod11Santander(calculoDVcodigo, 9) + " ";

            #endregion

            #region Grupo5

            var valorNominal = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "").PadLeft(10, '0'); //10

            var grupo5 = string.Format("{0}{1}", fatorVencimento, valorNominal);

            #endregion

            boleto.LinhaDigitavelBoleto = string.Format("{0}{1}{2}{3}{4}", grupo1, grupo2, grupo3, grupo4, grupo5);
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            if (String.IsNullOrEmpty(boleto.IdentificadorInternoBoleto) ||
                String.IsNullOrEmpty(boleto.IdentificadorInternoBoleto.TrimStart('0')))
                throw new Exception("Sequencial Nosso Número não foi informado.");

            boleto.SetNossoNumeroFormatado(String.Format("{0}{1}",
                boleto.IdentificadorInternoBoleto, Mod11Santander(boleto.IdentificadorInternoBoleto)).PadLeft(13, '0'));
        }

        public void FormataNumeroDocumento(Boleto boleto)
        {
            if (String.IsNullOrEmpty(boleto.NumeroDocumento) ||
                String.IsNullOrEmpty(boleto.NumeroDocumento.TrimStart('0')))
                throw new Exception("Número do Documento não foi informado.");

            boleto.NumeroDocumento = boleto.NumeroDocumento.PadLeft(10, '0');
        }

        public IEspecieDocumento ObtemEspecieDocumento(EnumEspecieDocumento especie)
        {
            #region Código Espécie

            // 02 - DM - DUPLICATA MERCANTIL                 
            // 04 - DS - DUPLICATA DE SERVICO                
            // 07 - LC - LETRA DE C�MBIO (SOMENTE PARA BANCO 353)
            // 30 - LC - LETRA DE C�MBIO (SOMENTE PARA BANCO 008)
            // 12 - NP - NOTA PROMISSORIA                    
            // 13 - NR - NOTA PROMISSORIA RURAL 
            // 17 - RC - RECIBO                              
            // 20 - AP - APOLICE DE SEGURO                   
            // 97 - CH - CHEQUE
            // 98 - ND - NOTA PROMISSORIA DIRETA

            #endregion

            switch (especie)
            {
                case EnumEspecieDocumento.DuplicataMercantil:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 02,
                        Descricao = "Duplicata mercantil",
                        Sigla = "DM"
                    };
                }
                case EnumEspecieDocumento.DuplicataServico:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 04,
                        Descricao = "Duplicata de serviço",
                        Sigla = "DS"
                    };
                }
                // Somente para banco 353
                case EnumEspecieDocumento.LetraCambio353:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 07,
                        Descricao = "Letra de câmbio",
                        Sigla = "LC"
                    };
                }
                case EnumEspecieDocumento.NotaPromissoria:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 12,
                        Descricao = "Nota promissória",
                        Sigla = "NP"
                    };
                }
                case EnumEspecieDocumento.NotaPromissoriaRural:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 13,
                        Descricao = "Nota promissória rural",
                        Sigla = "NP"
                    };
                }
                case EnumEspecieDocumento.Recibo:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 17,
                        Descricao = "Recibo",
                        Sigla = "RC"
                    };
                }
                case EnumEspecieDocumento.ApoliceSeguro:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 20,
                        Descricao = "Apólice seguro",
                        Sigla = "AP"
                    };
                }
                // Somente para banco 008
                case EnumEspecieDocumento.LetraCambio008:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 30,
                        Descricao = "Letra de câmbio",
                        Sigla = "LC"
                    };
                }
                case EnumEspecieDocumento.Cheque:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 97,
                        Descricao = "Cheque",
                        Sigla = "CH"
                    };
                }
                case EnumEspecieDocumento.NotaPromissoariaDireta:
                {
                    return new EspecieDocumento((int) especie)
                    {
                        Codigo = 98,
                        Descricao = "Nota promissória direta",
                        Sigla = "ND"
                    };
                }
            }

            throw new Exception(
                String.Format("Não foi possível obter instrução padronizada. Banco: {0} Código Espécie: {1}",
                    CodigoBanco, especie.ToString()));
        }

        public IInstrucao ObtemInstrucaoPadronizada(EnumTipoInstrucao tipoInstrucao, double valorInstrucao,
            DateTime dataInstrucao, int diasInstrucao)
        {
            switch (tipoInstrucao)
            {
                case EnumTipoInstrucao.NaoHaInstrucoes:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 00,
                        QtdDias = (int) valorInstrucao,
                        TextoInstrucao = ""
                    };
                }
                case EnumTipoInstrucao.BaixarAposQuinzeDiasDoVencto:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 02,
                        QtdDias = (int) valorInstrucao,
                        TextoInstrucao = "Baixar após 15 dias do vencimento."
                    };
                }
                case EnumTipoInstrucao.BaixarAposTrintaDiasDoVencto:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 03,
                        QtdDias = (int) valorInstrucao,
                        TextoInstrucao = "Baixar após 30 dias do vencimento."
                    };
                }
                case EnumTipoInstrucao.NaoBaixar:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 04,
                        QtdDias = (int) valorInstrucao,
                        TextoInstrucao = "Não baixar."
                    };
                }
                case EnumTipoInstrucao.Protestar:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 06,
                        QtdDias = (int) valorInstrucao,
                        TextoInstrucao = "Protestar após " + valorInstrucao + " dias úteis."
                    };
                }
                case EnumTipoInstrucao.NaoProtestar:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 07,
                        QtdDias = (int) valorInstrucao,
                        TextoInstrucao = "Não protestar."
                    };
                }
                case EnumTipoInstrucao.NaoCobrarJurosDeMora:
                {
                    return new InstrucaoPadronizada
                    {
                        Codigo = 08,
                        QtdDias = (int) valorInstrucao,
                        TextoInstrucao = "Não cobrar juros de mora."
                    };
                }
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter instrução padronizada. Banco: {0} Código Instrução: {1} Qtd Dias/Valor: {2}",
                    CodigoBanco, tipoInstrucao.ToString(), valorInstrucao));
        }

        public ICodigoOcorrencia ObtemCodigoOcorrencia(EnumCodigoOcorrenciaRemessa ocorrencia, double valorOcorrencia,
            DateTime dataOcorrencia)
        {
            switch (ocorrencia)
            {
                case EnumCodigoOcorrenciaRemessa.Registro:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 01,
                        Descricao = "Entrada de título"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.Baixa:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 02,
                        Descricao = "Pedido de baixa"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.ConcessaoDeAbatimento:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 04,
                        Descricao = "Concessão de abatimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDeAbatimento:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 05,
                        Descricao = "Cancelamento de abatimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeVencimento:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 06,
                        Descricao = "Alteração de vencimento"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDaIdentificacaoDotituloNaEmpresa:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 07,
                        Descricao = "Alteração da identificação do título na empresa"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoSeuNumero:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 08,
                        Descricao = "Alteração seu número"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.Protesto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 09,
                        Descricao = "Pedido de Protesto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.ConcessaoDeDesconto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 10,
                        Descricao = "Concessão de Desconto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.CancelamentoDeDesconto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 11,
                        Descricao = "Cancelamento de desconto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.SustarProtesto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 18,
                        Descricao = "Pedido de Sustação de Protesto"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.AlteracaoDeOutrosDados:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 31,
                        Descricao = "Alteração de outros dados"
                    };
                }
                case EnumCodigoOcorrenciaRemessa.NaoProtestar:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 98,
                        Descricao = "Não Protestar"
                    };
                }
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter Código de Comando/Movimento/Ocorrência. Banco: {0} Código: {1}",
                    CodigoBanco, ocorrencia.ToString()));
        }

        public ICodigoOcorrencia ObtemCodigoOcorrenciaByInt(int numeroOcorrencia)
        {
            switch (numeroOcorrencia)
            {
                case 01:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 01,
                        Descricao = "TÍTULO NÃO EXISTE"
                    };
                case 02:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 02,
                        Descricao = "ENTRADA TÍT. CONFIRMADA"
                    };
                case 03:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 03,
                        Descricao = "ENTRADA REJEITADA"
                    };
                case 04:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 04,
                        Descricao = "4 - NÃO IDENTIFICADO"
                    };
                case 05:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 05,
                        Descricao = "5 - NÃO IDENTIFICADO"
                    };
                case 06:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 06,
                        Descricao = "LIQUIDAÇÃO"
                    };
                case 07:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 07,
                        Descricao = "LIQUIDAÇÃO POR CONTA"
                    };
                case 08:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 08,
                        Descricao = "LIQUIDAÇÃO POR SALDO"
                    };
                case 09:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 09,
                        Descricao = "BAIXA AUTOMÁTICA"
                    };
                case 10:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 10,
                        Descricao = "TÍT. BAIX. CONF. INSTRUÇÃO"
                    };
                case 11:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 11,
                        Descricao = "EM SER"
                    };
                case 12:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 12,
                        Descricao = "ABATIMENTO CONCEDIDO"
                    };
                case 13:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 13,
                        Descricao = "ABATIMENTO CANCELADO"
                    };
                case 14:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 14,
                        Descricao = "PRORROGAÇÃO DE VENCIMENTO"
                    };
                case 15:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 15,
                        Descricao = "CONFIRMAÇÃO DE PROTESTO"
                    };
                case 16:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 16,
                        Descricao = "TÍT. JÁ BAIXADO/LIQUIDADO"
                    };
                case 17:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 17,
                        Descricao = "LIQUIDADO EM CARTÓRIO"
                    };
                case 21:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 21,
                        Descricao = "TÍT. ENVIADO A CARTÓRIO"
                    };
                case 22:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 22,
                        Descricao = "TÍT. RETIRADO DE CARTÓRIO"
                    };
                case 24:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 24,
                        Descricao = "CUSTAS DE CARTÓRIO"
                    };
                case 25:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 25,
                        Descricao = "PROTESTAR TÍTULO"
                    };
                case 26:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 26,
                        Descricao = "SUSTAR"
                    };
                case 35:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 35,
                        Descricao = "TÍTULO DDA RECONHECIDO PELO PAGADOR"
                    };
                case 36:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 36,
                        Descricao = "TÍTULO DDA NÃO RECONHECIDO PELO PAGADOR"
                    };
                case 37:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 37,
                        Descricao = "TÍTULO DDA RECUSADO PELA CIP"
                    };
                case 38:
                    return new CodigoOcorrencia(numeroOcorrencia)
                    {
                        Codigo = 38,
                        Descricao = "RECEBIMENTO DA INSTRUÇÃO NÃO PROTESTAR"
                    };
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter Código de Comando/Movimento/Ocorrência. Banco: {0} Código: {1}",
                    CodigoBanco, numeroOcorrencia.ToString()));
        }

        public ICodigoOcorrencia ObtemCodigoOcorrencia(EnumCodigoOcorrenciaRetorno ocorrencia)
        {
            switch (ocorrencia)
            {
                case EnumCodigoOcorrenciaRetorno.RetTituloNaoExiste:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 01,
                        Descricao = "TÍTULO NÃO EXISTE"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetRegistroConfirmado:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 02,
                        Descricao = "ENTRADA TÍT. CONFIRMADA"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetRegistroRecusado:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 03,
                        Descricao = "ENTRADA TÍT. REJEITADA"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetLiquidado:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 06,
                        Descricao = "LIQUIDAÇÃO"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetLiquidadoPorConta:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 07,
                        Descricao = "LIQUIDAÇÃO POR CONTA"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetLiquidadoSaldoRestante:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 08,
                        Descricao = "LIQUIDAÇÃO POR SALDO"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetBaixaAutomatica:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 09,
                        Descricao = "BAIXA AUTOMÁTICA"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetBaixado:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 10,
                        Descricao = "TÍT. BAIX. CONF. INSTRUÇÃO"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetTituloEmSer:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 11,
                        Descricao = "EM SER"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetAbatimentoConcedido:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 12,
                        Descricao = "ABATIMENTO CONCEDIDO"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetAbatimentoCancelado:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 13,
                        Descricao = "ABATIMENTO CANCELADO"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetProrrogVencto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 14,
                        Descricao = "PRORROGAÇÃO DE VENCIMENTO"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetConfirmacaoProtesto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 15,
                        Descricao = "CONFIRMAÇÃO DE PROTESTO"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetJaBaixado:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 16,
                        Descricao = "TÍT. JÁ BAIXADO/LIQUIDADO"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetLiquidadoEmCartorio:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 17,
                        Descricao = "LIQUIDADO EM CARTÓRIO"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetEncaminhadoACartorio:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 21,
                        Descricao = "TÍT. ENVIADO A CARTÓRIO"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetRetiradoDeCartorio:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 22,
                        Descricao = "TÍT. RETIRADO DE CARTÓRIO"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetCustasCartorioDistribuidor:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 24,
                        Descricao = "CUSTAS DE CARTÓRIO"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetRecebimentoInstrucaoProtestar:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 25,
                        Descricao = "PROTESTAR TÍTULO"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetRecebimentoInstrucaoSustarProtesto:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 26,
                        Descricao = "SUSTAR"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetTituloDdaReconhecido:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 35,
                        Descricao = "TÍTULO DDA RECONHECIDO PELO PAGADOR"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetTituloDdaNaoReconhecido:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 36,
                        Descricao = "TÍTULO DDA NÃO RECONHECIDO PELO PAGADOR"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetTituloDdaRecusado:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 37,
                        Descricao = "TÍTULO DDA RECUSADO PELA CIP"
                    };
                }
                case EnumCodigoOcorrenciaRetorno.RetNaoProtestar:
                {
                    return new CodigoOcorrencia((int) ocorrencia)
                    {
                        Codigo = 38,
                        Descricao = "RECEBIMENTO DA INSTRUÇÃO NÃO PROTESTAR"
                    };
                }
            }
            throw new Exception(
                String.Format(
                    "Não foi possível obter Código de Comando/Movimento/Ocorrência. Banco: {0} Código: {1}",
                    CodigoBanco, ocorrencia.ToString()));
        }

        public RetornoGenerico LerArquivoRetorno(List<string> linhasArquivo)
        {
            if (linhasArquivo == null || linhasArquivo.Any() == false)
                throw new ApplicationException("Arquivo informado é inválido/Não existem títulos no retorno.");

            /* Identifica o layout: 240 ou 400 */
            if (linhasArquivo.First().Length == 240)
            {
                //var leitor = new LeitorRetornoCnab240Santander(linhasArquivo);
                //var retornoProcessado = leitor.ProcessarRetorno();

                //var objRetornar = new RetornoGenerico(retornoProcessado);
                //return objRetornar;
            }
            if (linhasArquivo.First().Length == 400)
            {
                var leitor = new LeitorRetornoCnab400Santander(linhasArquivo);
                var retornoProcessado = leitor.ProcessarRetorno();

                var objRetornar = new RetornoGenerico(retornoProcessado);
                return objRetornar;
            }

            throw new Exception("Arquivo de RETORNO com " + linhasArquivo.First().Length + " posições, não é suportado.");
        }

        public RemessaCnab240 GerarArquivoRemessaCnab240(List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }

        public RemessaCnab400 GerarArquivoRemessaCnab400(List<Boleto> boletos)
        {
            throw new NotImplementedException();
        }



        public int Mod11SantanderObsolet(string seq)
        {
            var ndig = 0;
            var nresto = 0;
            var total = 0;
            var multiplicador = 2;

            char[] posicaoSeq = seq.ToCharArray();
            Array.Reverse(posicaoSeq);
            var sequencia = new string(posicaoSeq);

            while (sequencia.Length > 0)
            {
                int valorPosicao = Convert.ToInt32(sequencia.Substring(sequencia.Length - 1, 1));
                total += valorPosicao * multiplicador;
                multiplicador++;

                if (multiplicador == 10)
                {
                    multiplicador = 2;
                }

                sequencia = sequencia.Remove(sequencia.Length - 1, 1);
            }

            nresto = total - ((total / 11) * 11);

            if (nresto == 0 || nresto == 1)
                ndig = 0;
            else if (nresto == 10)
                ndig = 1;
            else
                ndig = (11 - nresto);

            return ndig;
        }

        public int Mod11Santander(string seq)
        {
            var ndig = 0;
            var nresto = 0;
            var total = 0;
            var multiplicador = 2;

            var posicaoAtual = 1;
            while (posicaoAtual <= seq.Length)
            {
                int valorPosicao = Convert.ToInt32(seq.Substring(seq.Length - posicaoAtual, 1));
                total += valorPosicao*multiplicador;
                posicaoAtual++;
                multiplicador++;
                if (multiplicador == 10)
                    multiplicador = 2;
            }

            nresto = total - ((total/11)*11);

            if (nresto == 0 || nresto == 1)
                ndig = 0;
            else if (nresto == 10)
                ndig = 1;
            else
                ndig = (11 - nresto);

            return ndig;
        }

        protected static int Mod10Mod11Santander(string seq, int lim)
        {
            var ndig = 0;
            var nresto = 0;
            var total = 0;
            var multiplicador = 2;

            char[] posicaoSeq = seq.ToCharArray();
            Array.Reverse(posicaoSeq);
            var sequencia = new string(posicaoSeq);

            while (sequencia.Length > 0)
            {
                int valorPosicao = Convert.ToInt32(sequencia.Substring(0, 1));
                total += valorPosicao*multiplicador;
                multiplicador++;

                if (multiplicador == 10)
                {
                    multiplicador = 2;
                }

                sequencia = sequencia.Remove(0, 1);
            }

            nresto = (total*10)%11; //nresto = (((total * 10) / 11) % 10); Jefhtavares em 19/03/14

            if (nresto == 0 || nresto == 1 || nresto == 10)
                ndig = 1;
            else
                ndig = nresto;

            return ndig;
        }

        protected static int Mult10Mod11Santander(string seq, int lim, int flag)
        {
            var mult = 0;
            var total = 0;
            var pos = 1;
            var ndig = 0;
            var nresto = 0;
            var num = string.Empty;

            mult = 1 + (seq.Length%(lim - 1));

            if (mult == 1)
                mult = lim;

            while (pos <= seq.Length)
            {
                num = Common.Mid(seq, pos, 1);
                total += Convert.ToInt32(num)*mult;

                mult -= 1;
                if (mult == 1)
                    mult = lim;

                pos += 1;
            }

            nresto = ((total*10)%11);

            if (flag == 1)
                return nresto;
            else
            {
                if (nresto == 0 || nresto == 1)
                    ndig = 0;
                else if (nresto == 10)
                    ndig = 1;
                else
                    ndig = (11 - nresto);

                return ndig;
            }
        }
        public int CodigoJurosMora(CodigoJurosMora codigoJurosMora)
        {
            return 0;
        }

        public int CodigoProteso(bool protestar = true)
        {
            var codProtesto = 0;
            if (protestar)
                codProtesto = 7;
            else
                codProtesto = 7;
            return codProtesto;
        }
    }
}
