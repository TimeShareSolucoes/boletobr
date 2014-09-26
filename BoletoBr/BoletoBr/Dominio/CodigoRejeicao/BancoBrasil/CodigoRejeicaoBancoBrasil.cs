using System;

namespace BoletoBr.Dominio.CodigoRejeicao.BancoBrasil
{
    // C�digos de rejeicoes de 1 a 64 associados ao c�digo de movimento 3, 26 e 30

    #region Enumerado

    public enum EnumCodigoRejeicaoBancoBrasil
    {
        CodigoBancoInvalido = 1,
        CodigoRegistroDetalheInvalido = 2,
        CodigoSegmentoInvalido = 3,
        CodigoMovimentoNaoPermitidoParaCarteira = 4,
        CodigoMovimentoInvalido = 5,
        TipoNumeroInscricaoCedenteInvalidos = 6,
        AgenciaContaDVInvalido = 7,
        NossoNumeroInvalido = 8,
        NossoNumeroDuplicado = 9,
        CarteiraInvalida = 10,
        FormaCadastramentoTituloInvalido = 11,
        TipoDocumentoInvalido = 12,
        IdentificacaoEmissaoBloquetoInvalida = 13,
        IdentificacaoDistribuicaoBloquetoInvalida = 14,
        CaracteristicasCobrancaIncompativeis = 15,
        DataVencimentoInvalida = 16,
        DataVencimentoAnteriorDataEmissao = 17,
        VencimentoForadoPrazodeOperacao = 18,
        TituloCargoBancosCorrespondentesVencimentoInferiorXXDias = 19,
        ValorTituloInvalido = 20,
        EspecieTituloInvalida = 21,
        EspecieNaoPermitidaParaCarteira = 22,
        AceiteInvalido = 23,
        DataEmissaoInvalida = 24,
        DataEmissaoPosteriorData = 25,
        CodigoJurosMoraInvalido = 26,
        ValorJurosMoraInvalido = 27,
        CodigoDescontoInvalido = 28,
        ValorDescontoMaiorIgualValorTitulo = 29,
        DescontoConcederNaoConfere = 30,
        ConcessaoDescontoJaExiste = 31,
        ValorIOFInvalido = 32,
        ValorAbatimentoInvalido = 33,
        ValorAbatimentoMaiorIgualValorTitulo = 34,
        AbatimentoConcederNaoConfere = 35,
        ConcessaoAbatimentoJaExiste = 36,
        CodigoProtestoInvalido = 37,
        PrazoProtestoInvalido = 38,
        PedidoProtestoNaoPermitidoParaTitulo = 39,
        TituloComOrdemProtestoEmitida = 40,
        PedidoCancelamentoParaTitulosSemInstrucaoProtesto = 41,
        CodigoParaBaixaDevolucaoInvalido = 42,
        PrazoParaBaixaDevolucaoInvalido = 43,
        CodigoMoedaInvalido = 44,
        NomeSacadoNaoInformado = 45,
        TipoNumeroInscricaoSacadoInvalidos = 46,
        EnderecoSacadoNaoInformado = 47,
        CEPInvalido = 48,
        CEPSemPracaCobranca = 49,
        CEPReferenteBancoCorrespondente = 50,
        CEPIncompativelComUnidadeFederacao = 51,
        UnidadeFederacaoInvalida = 52,
        TipoNumeroInscricaoSacadorAvalistaInvalido = 53,
        SacadorAvalistaNaoInformado = 54,
        NossoNumeroBancoCorrespondenteNaoInformado = 55,
        CodigoBancoCorrespondenteNaoInformado = 56,
        CodigoMultaInvalido = 57,
        DataMultaInvalida = 58,
        ValorPercentualMultaInvalido = 59,
        MovimentoParaTituloNaoCadastrado = 60,
        AlteracaoAgenciaCobradoraInvalida = 61,
        TipoImpressaoInvalido = 62,
        EntradaParaTituloJaCadastrado = 63,
        NumeroLinhaInvalido = 64,
        CodigoBancoDebitoInvalido = 65,
        AgenciaContaDVParaDebitoInvalido = 66,
        DadosParaDebitoIncompativel = 67,
        ArquivoEmDuplicidade = 88,
        ContratoInexistente = 99,
    }

    #endregion

    public class CodigoRejeicaoBancoBrasil : AbstractCodigoRejeicao, ICodigoRejeicao
    {
        #region Construtores

        public CodigoRejeicaoBancoBrasil()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoRejeicaoBancoBrasil(int codigo)
        {
            try
            {
                this.carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        #endregion

        #region Metodos Privados

        private void carregar(int codigo)
        {
            try
            {
                this.Banco = new Bancos.Brasil.BancoBrasil();

                switch ((EnumCodigoRejeicaoBancoBrasil) codigo)
                {
                    case EnumCodigoRejeicaoBancoBrasil.CodigoBancoInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoBancoInvalido;
                        this.Descricao = "C�digo do banco inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CodigoRegistroDetalheInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoRegistroDetalheInvalido;
                        this.Descricao = "C�digo do registro detalhe inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CodigoSegmentoInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoSegmentoInvalido;
                        this.Descricao = "C�digo do segmento inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CodigoMovimentoNaoPermitidoParaCarteira:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoMovimentoNaoPermitidoParaCarteira;
                        this.Descricao = "C�digo do movimento n�o permitido para a carteira";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CodigoMovimentoInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoMovimentoInvalido;
                        this.Descricao = "C�digo do movimento inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.TipoNumeroInscricaoCedenteInvalidos:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.TipoNumeroInscricaoCedenteInvalidos;
                        this.Descricao = "Tipo/N�mero de inscri��o do cendente inv�lidos";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.AgenciaContaDVInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.AgenciaContaDVInvalido;
                        this.Descricao = "Ag�ncia/Conta/DV inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.NossoNumeroInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.NossoNumeroInvalido;
                        this.Descricao = "Nosso n�mero inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.NossoNumeroDuplicado:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.NossoNumeroDuplicado;
                        this.Descricao = "Nosso n�mero duplicado";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CarteiraInvalida:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CarteiraInvalida;
                        this.Descricao = "Carteira inv�lida";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.FormaCadastramentoTituloInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.FormaCadastramentoTituloInvalido;
                        this.Descricao = "Forma de cadastramento do t�tulo inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.TipoDocumentoInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.TipoDocumentoInvalido;
                        this.Descricao = "Tipo de documento inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.IdentificacaoEmissaoBloquetoInvalida:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.IdentificacaoEmissaoBloquetoInvalida;
                        this.Descricao = "Identifica��o da emiss�o do bloqueto inv�lida";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.IdentificacaoDistribuicaoBloquetoInvalida:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.IdentificacaoDistribuicaoBloquetoInvalida;
                        this.Descricao = "Identifica��o da distribui��o do bloqueto inv�lida";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CaracteristicasCobrancaIncompativeis:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CaracteristicasCobrancaIncompativeis;
                        this.Descricao = "Caracter�sticas da cobran�a incompat�veis";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.DataVencimentoInvalida:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.DataVencimentoInvalida;
                        this.Descricao = "Data de vencimento inv�lida";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.DataVencimentoAnteriorDataEmissao:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.DataVencimentoAnteriorDataEmissao;
                        this.Descricao = "Data de vencimento anterior a data de emiss�o";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.VencimentoForadoPrazodeOperacao:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.VencimentoForadoPrazodeOperacao;
                        this.Descricao = "Vencimento fora do prazo de emiss�o";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.TituloCargoBancosCorrespondentesVencimentoInferiorXXDias:
                        this.Codigo =
                            (int)
                                EnumCodigoRejeicaoBancoBrasil.TituloCargoBancosCorrespondentesVencimentoInferiorXXDias;
                        this.Descricao = "Titulo a cargo de bancos correspondentes com vencimento inferior a XX dias";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.ValorTituloInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ValorTituloInvalido;
                        this.Descricao = "Valor do t�tulo inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.EspecieTituloInvalida:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.EspecieTituloInvalida;
                        this.Descricao = "Esp�cie do t�tulo inv�lida";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.EspecieNaoPermitidaParaCarteira:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.EspecieNaoPermitidaParaCarteira;
                        this.Descricao = "Esp�cie n�o permitida para a carteira";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.AceiteInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.AceiteInvalido;
                        this.Descricao = "Aceite inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.DataEmissaoInvalida:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.DataEmissaoInvalida;
                        this.Descricao = "Data de emiss�o inv�lida";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.DataEmissaoPosteriorData:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.DataEmissaoPosteriorData;
                        this.Descricao = "Data de emiss�o posterior a data";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CodigoJurosMoraInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoJurosMoraInvalido;
                        this.Descricao = "C�digo de juros de mora inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.ValorJurosMoraInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ValorJurosMoraInvalido;
                        this.Descricao = "Valor/Taxa de juros de mora inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CodigoDescontoInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoDescontoInvalido;
                        this.Descricao = "C�digo do desconto inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.ValorDescontoMaiorIgualValorTitulo:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ValorDescontoMaiorIgualValorTitulo;
                        this.Descricao = "Valor do desconto maior ou igual ao valor do t�tulo";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.DescontoConcederNaoConfere:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.DescontoConcederNaoConfere;
                        this.Descricao = "Desconto a conceder n�o confere";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.ConcessaoDescontoJaExiste:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ConcessaoDescontoJaExiste;
                        this.Descricao = "Concess�o de desconto - j� existe desconto anterior";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.ValorIOFInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ValorIOFInvalido;
                        this.Descricao = "Valor do IOF inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.ValorAbatimentoInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ValorAbatimentoInvalido;
                        this.Descricao = "Valor do abatimento inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.ValorAbatimentoMaiorIgualValorTitulo:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ValorAbatimentoMaiorIgualValorTitulo;
                        this.Descricao = "Valor do abatimento maior ou igual ao valor do t�tulo";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.AbatimentoConcederNaoConfere:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.AbatimentoConcederNaoConfere;
                        this.Descricao = "Abatimento a conceder n�o confere";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.ConcessaoAbatimentoJaExiste:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ConcessaoAbatimentoJaExiste;
                        this.Descricao = "Concess�o de abatimento - j� existe abatimendo anterior";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CodigoProtestoInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoProtestoInvalido;
                        this.Descricao = "C�digo para protesto inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.PrazoProtestoInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.PrazoProtestoInvalido;
                        this.Descricao = "Prazo para protesto inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.PedidoProtestoNaoPermitidoParaTitulo:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.PedidoProtestoNaoPermitidoParaTitulo;
                        this.Descricao = "Pedido de protesto n�o permitido para o t�tulo";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.TituloComOrdemProtestoEmitida:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.TituloComOrdemProtestoEmitida;
                        this.Descricao = "T�tulo com ordem de protesto emitida";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.PedidoCancelamentoParaTitulosSemInstrucaoProtesto:
                        this.Codigo =
                            (int) EnumCodigoRejeicaoBancoBrasil.PedidoCancelamentoParaTitulosSemInstrucaoProtesto;
                        this.Descricao = "Pedido de cancelamento para t�tulos sem instru��o de protesto";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CodigoParaBaixaDevolucaoInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoParaBaixaDevolucaoInvalido;
                        this.Descricao = "C�digo para baixa/devolu��o inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.PrazoParaBaixaDevolucaoInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.PrazoParaBaixaDevolucaoInvalido;
                        this.Descricao = "Prazo para baixa/devolu��o inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CodigoMoedaInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoMoedaInvalido;
                        this.Descricao = "C�digo da moeda inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.NomeSacadoNaoInformado:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.NomeSacadoNaoInformado;
                        this.Descricao = "Nome do sacado n�o informado";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.TipoNumeroInscricaoSacadoInvalidos:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.TipoNumeroInscricaoSacadoInvalidos;
                        this.Descricao = "Tipo/N�mero de inscri��o do sacado inv�lidos";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.EnderecoSacadoNaoInformado:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.EnderecoSacadoNaoInformado;
                        this.Descricao = "Endere�o do sacado n�o informado";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CEPInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CEPInvalido;
                        this.Descricao = "CEP inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CEPSemPracaCobranca:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CEPSemPracaCobranca;
                        this.Descricao = "CEP sem pra�a de cobran�a";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CEPReferenteBancoCorrespondente:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CEPReferenteBancoCorrespondente;
                        this.Descricao = "CEP referente a um banco correspondente";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CEPIncompativelComUnidadeFederacao:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CEPIncompativelComUnidadeFederacao;
                        this.Descricao = "CEP incompat�vel com a UF";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.UnidadeFederacaoInvalida:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.UnidadeFederacaoInvalida;
                        this.Descricao = "UF inv�lida";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.TipoNumeroInscricaoSacadorAvalistaInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.TipoNumeroInscricaoSacadorAvalistaInvalido;
                        this.Descricao = "Tipo/N�mero de inscri��o do sacador/avalista inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.SacadorAvalistaNaoInformado:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.SacadorAvalistaNaoInformado;
                        this.Descricao = "Sacador/Avalista n�o informado";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.NossoNumeroBancoCorrespondenteNaoInformado:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.NossoNumeroBancoCorrespondenteNaoInformado;
                        this.Descricao = "Nosso n�mero no banco correspondente n�o informado";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CodigoBancoCorrespondenteNaoInformado:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoBancoCorrespondenteNaoInformado;
                        this.Descricao = "C�digo do banco correspondente n�o informado";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CodigoMultaInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoMultaInvalido;
                        this.Descricao = "C�digo da multa inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.DataMultaInvalida:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.DataMultaInvalida;
                        this.Descricao = "Data da multa inv�lida";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.ValorPercentualMultaInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ValorPercentualMultaInvalido;
                        this.Descricao = "Valor/Percentual da multa inv�lida";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.MovimentoParaTituloNaoCadastrado:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.MovimentoParaTituloNaoCadastrado;
                        this.Descricao = "Movimento para t�tulo n�o cadastrado";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.AlteracaoAgenciaCobradoraInvalida:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.AlteracaoAgenciaCobradoraInvalida;
                        this.Descricao = "Altera��o da ag�ncia cobradora/dv inv�lida";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.TipoImpressaoInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.TipoImpressaoInvalido;
                        this.Descricao = "Tipo de impress�o inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.EntradaParaTituloJaCadastrado:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.EntradaParaTituloJaCadastrado;
                        this.Descricao = "Entrada para t�tulo j� cadastrado";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.NumeroLinhaInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.NumeroLinhaInvalido;
                        this.Descricao = "N�mero da linha inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.CodigoBancoDebitoInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoBancoDebitoInvalido;
                        this.Descricao = "C�digo do banco para d�bito inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.AgenciaContaDVParaDebitoInvalido:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.AgenciaContaDVParaDebitoInvalido;
                        this.Descricao = "Ag�ncia/Conta/DV para d�bito inv�lido";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.DadosParaDebitoIncompativel:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.DadosParaDebitoIncompativel;
                        this.Descricao = "Dados para d�bito incompat�vel com a identifica��o da emiss�o do boleto";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.ArquivoEmDuplicidade:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ArquivoEmDuplicidade;
                        this.Descricao = "Arquivo em duplicidade";
                        break;
                    case EnumCodigoRejeicaoBancoBrasil.ContratoInexistente:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ContratoInexistente;
                        this.Descricao = "Contrato inexistente";
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

        private void Ler(int codigo)
        {
            try
            {
                switch (codigo)
                {
                    case 1:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoBancoInvalido;
                        this.Descricao = "C�digo do banco inv�lido";
                        break;
                    case 2:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoRegistroDetalheInvalido;
                        this.Descricao = "C�digo do registro detalhe inv�lido";
                        break;
                    case 3:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoSegmentoInvalido;
                        this.Descricao = "C�digo do segmento inv�lido";
                        break;
                    case 4:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoMovimentoNaoPermitidoParaCarteira;
                        this.Descricao = "C�digo do movimento n�o permitido para a carteira";
                        break;
                    case 5:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoMovimentoInvalido;
                        this.Descricao = "C�digo do movimento inv�lido";
                        break;
                    case 6:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.TipoNumeroInscricaoCedenteInvalidos;
                        this.Descricao = "Tipo/N�mero de inscri��o do cendente inv�lidos";
                        break;
                    case 7:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.AgenciaContaDVInvalido;
                        this.Descricao = "Ag�ncia/Conta/DV inv�lido";
                        break;
                    case 8:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.NossoNumeroInvalido;
                        this.Descricao = "Nosso n�mero inv�lido";
                        break;
                    case 9:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.NossoNumeroDuplicado;
                        this.Descricao = "Nosso n�mero duplicado";
                        break;
                    case 10:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CarteiraInvalida;
                        this.Descricao = "Carteira inv�lida";
                        break;
                    case 11:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.FormaCadastramentoTituloInvalido;
                        this.Descricao = "Forma de cadastramento do t�tulo inv�lido";
                        break;
                    case 12:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.TipoDocumentoInvalido;
                        this.Descricao = "Tipo de documento inv�lido";
                        break;
                    case 13:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.IdentificacaoEmissaoBloquetoInvalida;
                        this.Descricao = "Identifica��o da emiss�o do bloqueto inv�lida";
                        break;
                    case 14:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.IdentificacaoDistribuicaoBloquetoInvalida;
                        this.Descricao = "Identifica��o da distribui��o do bloqueto inv�lida";
                        break;
                    case 15:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CaracteristicasCobrancaIncompativeis;
                        this.Descricao = "Caracter�sticas da cobran�a incompat�veis";
                        break;
                    case 16:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.DataVencimentoInvalida;
                        this.Descricao = "Data de vencimento inv�lida";
                        break;
                    case 17:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.DataVencimentoAnteriorDataEmissao;
                        this.Descricao = "Data de vencimento anterior a data de emiss�o";
                        break;
                    case 18:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.VencimentoForadoPrazodeOperacao;
                        this.Descricao = "Vencimento fora do prazo de emiss�o";
                        break;
                    case 19:
                        this.Codigo =
                            (int)
                                EnumCodigoRejeicaoBancoBrasil.TituloCargoBancosCorrespondentesVencimentoInferiorXXDias;
                        this.Descricao = "Titulo a cargo de bancos correspondentes com vencimento inferior a XX dias";
                        break;
                    case 20:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ValorTituloInvalido;
                        this.Descricao = "Valor do t�tulo inv�lido";
                        break;
                    case 21:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.EspecieTituloInvalida;
                        this.Descricao = "Esp�cie do t�tulo inv�lida";
                        break;
                    case 22:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.EspecieNaoPermitidaParaCarteira;
                        this.Descricao = "Esp�cie n�o permitida para a carteira";
                        break;
                    case 23:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.AceiteInvalido;
                        this.Descricao = "Aceite inv�lido";
                        break;
                    case 24:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.DataEmissaoInvalida;
                        this.Descricao = "Data de emiss�o inv�lida";
                        break;
                    case 25:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.DataEmissaoPosteriorData;
                        this.Descricao = "Data de emiss�o posterior a data";
                        break;
                    case 26:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoJurosMoraInvalido;
                        this.Descricao = "C�digo de juros de mora inv�lido";
                        break;
                    case 27:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ValorJurosMoraInvalido;
                        this.Descricao = "Valor/Taxa de juros de mora inv�lido";
                        break;
                    case 28:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoDescontoInvalido;
                        this.Descricao = "C�digo do desconto inv�lido";
                        break;
                    case 29:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ValorDescontoMaiorIgualValorTitulo;
                        this.Descricao = "Valor do desconto maior ou igual ao valor do t�tulo";
                        break;
                    case 30:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.DescontoConcederNaoConfere;
                        this.Descricao = "Desconto a conceder n�o confere";
                        break;
                    case 31:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ConcessaoDescontoJaExiste;
                        this.Descricao = "Concess�o de desconto - j� existe desconto anterior";
                        break;
                    case 32:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ValorIOFInvalido;
                        this.Descricao = "Valor do IOF inv�lido";
                        break;
                    case 33:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ValorAbatimentoInvalido;
                        this.Descricao = "Valor do abatimento inv�lido";
                        break;
                    case 34:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ValorAbatimentoMaiorIgualValorTitulo;
                        this.Descricao = "Valor do abatimento maior ou igual ao valor do t�tulo";
                        break;
                    case 35:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.AbatimentoConcederNaoConfere;
                        this.Descricao = "Abatimento a conceder n�o confere";
                        break;
                    case 36:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ConcessaoAbatimentoJaExiste;
                        this.Descricao = "Concess�o de abatimento - j� existe abatimendo anterior";
                        break;
                    case 37:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoProtestoInvalido;
                        this.Descricao = "C�digo para protesto inv�lido";
                        break;
                    case 38:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.PrazoProtestoInvalido;
                        this.Descricao = "Prazo para protesto inv�lido";
                        break;
                    case 39:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.PedidoProtestoNaoPermitidoParaTitulo;
                        this.Descricao = "Pedido de protesto n�o permitido para o t�tulo";
                        break;
                    case 40:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.TituloComOrdemProtestoEmitida;
                        this.Descricao = "T�tulo com ordem de protesto emitida";
                        break;
                    case 41:
                        this.Codigo =
                            (int) EnumCodigoRejeicaoBancoBrasil.PedidoCancelamentoParaTitulosSemInstrucaoProtesto;
                        this.Descricao = "Pedido de cancelamento para t�tulos sem instru��o de protesto";
                        break;
                    case 42:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoParaBaixaDevolucaoInvalido;
                        this.Descricao = "C�digo para baixa/devolu��o inv�lido";
                        break;
                    case 43:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.PrazoParaBaixaDevolucaoInvalido;
                        this.Descricao = "Prazo para baixa/devolu��o inv�lido";
                        break;
                    case 44:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoMoedaInvalido;
                        this.Descricao = "C�digo da moeda inv�lido";
                        break;
                    case 45:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.NomeSacadoNaoInformado;
                        this.Descricao = "Nome do sacado n�o informado";
                        break;
                    case 46:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.AbatimentoConcederNaoConfere;
                        this.Descricao = "Tipo/N�mero de inscri��o do sacado inv�lidos";
                        break;
                    case 47:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.EnderecoSacadoNaoInformado;
                        this.Descricao = "Endere�o do sacado n�o informado";
                        break;
                    case 48:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CEPInvalido;
                        this.Descricao = "CEP inv�lido";
                        break;
                    case 49:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CEPSemPracaCobranca;
                        this.Descricao = "CEP sem pra�a de cobran�a";
                        break;
                    case 50:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CEPReferenteBancoCorrespondente;
                        this.Descricao = "CEP referente a um banco correspondente";
                        break;
                    case 51:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CEPIncompativelComUnidadeFederacao;
                        this.Descricao = "CEP incompat�vel com a UF";
                        break;
                    case 52:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.UnidadeFederacaoInvalida;
                        this.Descricao = "UF inv�lida";
                        break;
                    case 53:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.TipoNumeroInscricaoSacadorAvalistaInvalido;
                        this.Descricao = "Tipo/N�mero de inscri��o do sacador/avalista inv�lido";
                        break;
                    case 54:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.SacadorAvalistaNaoInformado;
                        this.Descricao = "Sacador/Avalista n�o informado";
                        break;
                    case 55:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.NossoNumeroBancoCorrespondenteNaoInformado;
                        this.Descricao = "Nosso n�mero no banco correspondente n�o informado";
                        break;
                    case 56:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoBancoCorrespondenteNaoInformado;
                        this.Descricao = "C�digo do banco correspondente n�o informado";
                        break;
                    case 57:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoMultaInvalido;
                        this.Descricao = "C�digo da multa inv�lido";
                        break;
                    case 58:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.DataMultaInvalida;
                        this.Descricao = "Data da multa inv�lida";
                        break;
                    case 59:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ValorPercentualMultaInvalido;
                        this.Descricao = "Valor/Percentual da multa inv�lida";
                        break;
                    case 60:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.MovimentoParaTituloNaoCadastrado;
                        this.Descricao = "Movimento para t�tulo n�o cadastrado";
                        break;
                    case 61:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.AlteracaoAgenciaCobradoraInvalida;
                        this.Descricao = "Altera��o da ag�ncia cobradora/dv inv�lida";
                        break;
                    case 62:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.TipoImpressaoInvalido;
                        this.Descricao = "Tipo de impress�o inv�lido";
                        break;
                    case 63:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.EntradaParaTituloJaCadastrado;
                        this.Descricao = "Entrada para t�tulo j� cadastrado";
                        break;
                    case 64:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.NumeroLinhaInvalido;
                        this.Descricao = "N�mero da linha inv�lido";
                        break;
                    case 65:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.CodigoBancoDebitoInvalido;
                        this.Descricao = "C�digo do banco para d�bito inv�lido";
                        break;
                    case 66:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.AgenciaContaDVParaDebitoInvalido;
                        this.Descricao = "Ag�ncia/Conta/DV para d�bito inv�lido";
                        break;
                    case 67:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.DadosParaDebitoIncompativel;
                        this.Descricao = "Dados para d�bito incompat�vel com a identifica��o da emiss�o do boleto";
                        break;
                    case 88:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ArquivoEmDuplicidade;
                        this.Descricao = "Arquivo em duplicidade";
                        break;
                    case 99:
                        this.Codigo = (int) EnumCodigoRejeicaoBancoBrasil.ContratoInexistente;
                        this.Descricao = "Contrato inexistente";
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
