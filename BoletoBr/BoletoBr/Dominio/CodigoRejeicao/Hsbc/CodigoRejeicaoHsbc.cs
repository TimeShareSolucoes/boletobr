using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos.Hsbc;
using BoletoBr.Dominio.CodigoMovimento;

namespace BoletoBr.Dominio.CodigoRejeicao
{
    public enum EnumCodigoRejeicaoHsbc
    {
        /* Código de Rejeição de 1 a 151 */
        RegistroForaDeSequencia = 001,
        RegistroDuplicado = 002,
        TipoDeRegistroInvalido = 003,
        RegistroNaoPertenceAEstaPlanilha = 004,
        BduDoBeneficiarioNaoNumerico = 005,
        BduDoBeneficiarioNaoInformado = 006,
        BduDoBeneficiarioInexistente = 007,
        CodigoBeneficiarioNaoInformado = 008,
        CodigoBeneficiarioNaoNumerico = 009,
        CodigoBeneficiarioZerado = 010,
        CodigoBeneficiarioInexistente = 011,
        CodigoFormularioNaoInformado = 012,
        CodigoFormularioInexistente = 013,
        InformarCodBeneficiario = 015,
        InformarDataVencimentoParcelaUnicaOuDataPrimeiroVencimento = 016,
        InformarValorParcelaUnicaOuValorDasParcelas = 017,
        InformarTipoDaMoeda = 018,
        InformarCodigoDocumento = 019,
        PeriodicidadeDeVencimentoInvalida = 022,
        TipodeMontagemInvalido = 023,
        TipoDeMontagemNaoInformado = 024,
        QuantidadeDeCarnesInvalida = 025,
        QuantidadeDeCarnesNaoInformada = 026,
        NumeroParcelaDeInvalido = 027,
        NumeroParcelaDeNaoInformado = 028,
        NumeroParcelaAteInvalido = 029,
        NumeroParcelaAteNaoInformado = 030,
        QuantidadeParcelasInvalida = 031,
        QuantidadeParcelasNaoInformada = 032,
        ParcelaDeMaiorParcelaAte = 033,
        DataPrimeiroVencimentoInvalida = 034,
        DataPrimeiroVencimentoAnteriorADataDeHoje = 035,
        DataPrimeiroVencimentoJaInformadaNoCapa = 036,
        DataVencimentoParcelaUnicaInvalida = 037,
        DataVencimentoParcelaUnicaAnteriorADataDeHoje = 038,
        DataVencimentoParcelaUnicaJaInformadaNoCapa = 039,
        TipoDeMoedaInvalido = 040,
        ValorDaParcelaInvalido = 042,
        ValorDaParcelaNaoInformado = 043,
        ValorDaParcelaJaInformadoNoCapa = 044,
        ValorTotalDasParcelasInvalido = 045,
        ValorTotalDasParcelasNaoInformado = 046,
        ValorDaParcelaUnicaInvalido = 047,
        ValorDaParcelaUnicaNaoInformado = 048,
        ValorDaParcelaUnicaJaInformadoNoCapa = 049,
        ValorTotalDaParcelaUnicaInvalido = 050,
        ValorTotalDaParcelaUnicaNaoInformado = 051,
        CodigoDoDocumentoInvalido = 058,
        CodigoDoDocumentoNaoNumerico = 059,
        CodigoDoDocumentoNaoInformado = 060,
        BduDoBeneficiarioZerado = 070,
        BduDoBeneficiarioJaEstaEncerrado = 073,
        BeneficiarioEstaCancelado = 075,
        CodigoDoFormularioCanceladoOuZerado = 076,
        TipoDeMoedaDeveSerIgualEmTodosOsDetalhesDoLote = 080,
        SeMoedaInformadaObrigatorioValorParcelaEValorTotalParcelas = 081,
        SeValorInformadoObrigatorioTipoMoeda = 082,
        QtdParcNaoPodeSerSuperiorAoResultadoDoCalculo = 083,
        QtdParcNaoPodeSerSuperiorASessenta = 084,
        QtdParcTemQueSerMaiorQueZero = 085,
        QtdTotalDeParcAEmitirUltrapassouSessentaMil = 087,
        CodigoMaterialDisponivelSomenteParaEmissaoEmpresa = 089,
        TipoDeMoedaInvalido2 = 90,
        UsoDeParcelaUnicaIndevidoParaQtdParcelaUm = 091,
        ParaBoletoAutoEnvelopadoObrigatorioNomeDoPagador = 096,
        ParaBoletoAutoEnvelopadoObrigatorioRuaDoPagador = 097,
        ParaBoletoAutoEnvelopadoObrigatorioNumeroResidDoPagador = 098,
        ParaBoletoAutoEnvelopadoObrigatorioCidadeDoPagador = 099,
        ParaBoletoAutoEnvelopadoObrigatorioEstadoDoPagador = 100,
        CepDoPagadorInvalido = 112,
        ArquivoRecusadoPorDuplicidade = 114,
        ArquivoVazio = 115,
        ArquivoSemRegistrosAnexos = 116,
        SequenciaDeRegistrosInvalida = 117,
        CodigoDeRegistroDiferenteDeZero = 118,
        CodigoDeRemessaDiferenteDeUm = 119,
        LiteralRemessaDiferenteDeRemessa = 120,
        CodigoDeServicoDiferenteDeUm = 121,
        LiteralCobrancaDiferenteDeCobranca = 122,
        DataDeGravacaoNaoNumerica = 123,
        DensidadeDaGravacaoDiferenteDe1600E6250 = 124,
        LiteralDensidadeDiferenteDeBpi = 125,
        HoraDeGravacaoNaoNumerica = 126,
        NumeroSequencialNaoNumerico = 127,
        NumeroSequenciaForaDeSequencia = 128,
        CodigoDoBancoDiferenteDe399 = 129,
        LoteDeServicosDiferenteDeZeros = 130,
        LoteDeServicoHeaderForaDaSequencia = 131,
        CodigoLayoutDiferenteDe020 = 132,
        TipoDeRegistroHeaderInvalido = 133,
        TipoDeOperacaoDiferenteDeR = 134,
        TipoDeServicoDiferenteDe02 = 135,
        FormaDeLancamentoDiferenteDe00 = 136,
        VersaoDeLoteDiferenteDe010 = 137,
        NumeroDeRemessaNaoNumerico = 138,
        TipoDeMontagemSoPodeSerZeroOuUm = 139,
        QtdLotesDiferenteDoInformadoNoArquivo = 140,
        QtdDeRegistroTrailerArqDiferenteDoInformado = 141,
        ArquivoSemRegistroHeader = 142,
        ArquivoSemRegistroTrailer = 143,
        ArquivoSemRegistroHeaderETrailer = 144,
        FormularioIncompativelComCodigoDePostagem = 145,
        BeneficiarioSemCadastroDePostagemNoHsbc = 151,
    }

    public class CodigoRejeicaoHsbc : AbstractCodigoRejeicao, ICodigoRejeicao
    {
        #region Construtores

        public CodigoRejeicaoHsbc(int codigo)
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
                this.Banco = new BancoHsbc();

                switch ((EnumCodigoRejeicaoHsbc) codigo)
                {
                    case EnumCodigoRejeicaoHsbc.RegistroForaDeSequencia:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.RegistroForaDeSequencia;
                        this.Descricao = "Registro fora da sequencia";
                        break;
                    case EnumCodigoRejeicaoHsbc.RegistroDuplicado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.RegistroDuplicado;
                        this.Descricao = "Registro duplicado";
                        break;
                    case EnumCodigoRejeicaoHsbc.TipoDeRegistroInvalido:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeRegistroInvalido;
                        this.Descricao = "Tipo de registro inválido";
                        break;
                    case EnumCodigoRejeicaoHsbc.RegistroNaoPertenceAEstaPlanilha:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.RegistroNaoPertenceAEstaPlanilha;
                        this.Descricao = "Registro não pertence a esta planilha";
                        break;
                    case EnumCodigoRejeicaoHsbc.BduDoBeneficiarioNaoNumerico:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.BduDoBeneficiarioNaoNumerico;
                        this.Descricao = "BDU do beneficiário não numérico";
                        break;
                    case EnumCodigoRejeicaoHsbc.BduDoBeneficiarioNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.BduDoBeneficiarioNaoInformado;
                        this.Descricao = "BDU do beneficiário não informado";
                        break;
                    case EnumCodigoRejeicaoHsbc.BduDoBeneficiarioInexistente:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.BduDoBeneficiarioInexistente;
                        this.Descricao = "BDU do beneficiário inexistente";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoBeneficiarioNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoBeneficiarioNaoInformado;
                        this.Descricao = "Código beneficiário não informado";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoBeneficiarioNaoNumerico:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoBeneficiarioNaoNumerico;
                        this.Descricao = "Código beneficiário não numérico";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoBeneficiarioZerado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoBeneficiarioZerado;
                        this.Descricao = "Código beneficiário zerado";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoBeneficiarioInexistente:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoBeneficiarioInexistente;
                        this.Descricao = "Código beneficiário inexistente";
                        break;

                    case EnumCodigoRejeicaoHsbc.CodigoFormularioNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoFormularioNaoInformado;
                        this.Descricao = "Código formulário não informado";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoFormularioInexistente:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoFormularioInexistente;
                        this.Descricao = "Código formuláiro inexistente";
                        break;
                    case EnumCodigoRejeicaoHsbc.InformarCodBeneficiario:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.InformarCodBeneficiario;
                        this.Descricao = "Form. Laser c/ cód. Barras - Informar cód. Beneficiário";
                        break;
                    case EnumCodigoRejeicaoHsbc.InformarDataVencimentoParcelaUnicaOuDataPrimeiroVencimento:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.InformarDataVencimentoParcelaUnicaOuDataPrimeiroVencimento;
                        this.Descricao = "Informar a data de vencimento da parcela única e/ou data do primeiro vencimento";
                        break;
                    case EnumCodigoRejeicaoHsbc.InformarValorParcelaUnicaOuValorDasParcelas:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.InformarValorParcelaUnicaOuValorDasParcelas;
                        this.Descricao = "Informar o valor da parcela única e/ou valor das parcelas";
                        break;
                    case EnumCodigoRejeicaoHsbc.InformarTipoDaMoeda:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.InformarTipoDaMoeda;
                        this.Descricao = "Form. Laser com cód. Barras - Informar tipo da moeda";
                        break;
                    case EnumCodigoRejeicaoHsbc.InformarCodigoDocumento:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.InformarCodigoDocumento;
                        this.Descricao = "Form. Laser com cód. Barras - Informar cód. Documento";
                        break;
                    case EnumCodigoRejeicaoHsbc.PeriodicidadeDeVencimentoInvalida:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.PeriodicidadeDeVencimentoInvalida;
                        this.Descricao = "Periodicidade de vencimento inválida";
                        break;
                    case EnumCodigoRejeicaoHsbc.TipodeMontagemInvalido:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipodeMontagemInvalido;
                        this.Descricao = "Tipo de montagem inválido";
                        break;
                    case EnumCodigoRejeicaoHsbc.TipoDeMontagemNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeMontagemNaoInformado;
                        this.Descricao = "Tipo de montagem não informado";
                        break;
                    case EnumCodigoRejeicaoHsbc.QuantidadeDeCarnesInvalida:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QuantidadeDeCarnesInvalida;
                        this.Descricao = "Quantidade de carnês inválida";
                        break;
                    case EnumCodigoRejeicaoHsbc.QuantidadeDeCarnesNaoInformada:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QuantidadeDeCarnesNaoInformada;
                        this.Descricao = "Quantidade de carnês não informada";
                        break;
                    case EnumCodigoRejeicaoHsbc.NumeroParcelaDeInvalido:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.NumeroParcelaDeInvalido;
                        this.Descricao = "Número parcela 'De' inválido";
                        break;
                    case EnumCodigoRejeicaoHsbc.NumeroParcelaDeNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.NumeroParcelaDeNaoInformado;
                        this.Descricao = "Número parcela 'De' não informado";
                        break;
                    case EnumCodigoRejeicaoHsbc.NumeroParcelaAteInvalido:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.NumeroParcelaAteInvalido;
                        this.Descricao = "Número parcela 'Até' inválido";
                        break;
                    case EnumCodigoRejeicaoHsbc.NumeroParcelaAteNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.NumeroParcelaAteNaoInformado;
                        this.Descricao = "Número parcela 'Até' não informado";
                        break;
                    case EnumCodigoRejeicaoHsbc.QuantidadeParcelasInvalida:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QuantidadeParcelasInvalida;
                        this.Descricao = "Quantidade parcelas inválida";
                        break;
                    case EnumCodigoRejeicaoHsbc.QuantidadeParcelasNaoInformada:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QuantidadeParcelasNaoInformada;
                        this.Descricao = "Quantidade parcelas não informada";
                        break;
                    case EnumCodigoRejeicaoHsbc.ParcelaDeMaiorParcelaAte:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ParcelaDeMaiorParcelaAte;
                        this.Descricao = "Parcela 'De' maior que parcela 'Até'";
                        break;
                    case EnumCodigoRejeicaoHsbc.DataPrimeiroVencimentoInvalida:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DataPrimeiroVencimentoInvalida;
                        this.Descricao = "Data primeiro vencimento inválida";
                        break;
                    case EnumCodigoRejeicaoHsbc.DataPrimeiroVencimentoAnteriorADataDeHoje:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DataPrimeiroVencimentoAnteriorADataDeHoje;
                        this.Descricao = "Data primeiro vencimento anterior a data de hoje";
                        break;
                    case EnumCodigoRejeicaoHsbc.DataPrimeiroVencimentoJaInformadaNoCapa:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DataPrimeiroVencimentoJaInformadaNoCapa;
                        this.Descricao = "Data primeiro vencimento já informada no Capa";
                        break;
                    case EnumCodigoRejeicaoHsbc.DataVencimentoParcelaUnicaInvalida:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DataVencimentoParcelaUnicaInvalida;
                        this.Descricao = "Data vencimento parcela única inválida";
                        break;
                    case EnumCodigoRejeicaoHsbc.DataVencimentoParcelaUnicaAnteriorADataDeHoje:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DataVencimentoParcelaUnicaAnteriorADataDeHoje;
                        this.Descricao = "Data vencimento parcela única anterior a data de hoje";
                        break;
                    case EnumCodigoRejeicaoHsbc.DataVencimentoParcelaUnicaJaInformadaNoCapa:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DataVencimentoParcelaUnicaJaInformadaNoCapa;
                        this.Descricao = "Data vencimento parcela única já informado no Capa";
                        break;
                    case EnumCodigoRejeicaoHsbc.TipoDeMoedaInvalido:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeMoedaInvalido;
                        this.Descricao = "Tipo de moeda inválido";
                        break;
                    case EnumCodigoRejeicaoHsbc.ValorDaParcelaInvalido:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorDaParcelaInvalido;
                        this.Descricao = "Valor da parcela inválido";
                        break;
                    case EnumCodigoRejeicaoHsbc.ValorDaParcelaNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorDaParcelaNaoInformado;
                        this.Descricao = "Valor da parcela não informado";
                        break;
                    case EnumCodigoRejeicaoHsbc.ValorDaParcelaJaInformadoNoCapa:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorDaParcelaJaInformadoNoCapa;
                        this.Descricao = "Valor da parcela já informado no Capa";
                        break;
                    case EnumCodigoRejeicaoHsbc.ValorTotalDasParcelasInvalido:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorTotalDasParcelasInvalido;
                        this.Descricao = "Valor total das parcelas inválido";
                        break;
                    case EnumCodigoRejeicaoHsbc.ValorTotalDasParcelasNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorTotalDasParcelasNaoInformado;
                        this.Descricao = "Valor total das parcelas não informado";
                        break;
                    case EnumCodigoRejeicaoHsbc.ValorDaParcelaUnicaInvalido:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorDaParcelaUnicaInvalido;
                        this.Descricao = "Valor da parcela única inválido";
                        break;
                    case EnumCodigoRejeicaoHsbc.ValorDaParcelaUnicaNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorDaParcelaUnicaNaoInformado;
                        this.Descricao = "Valor da parcela única não informado";
                        break;
                    case EnumCodigoRejeicaoHsbc.ValorDaParcelaUnicaJaInformadoNoCapa:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorDaParcelaUnicaJaInformadoNoCapa;
                        this.Descricao = "Valor da parcela única já informado no Capa";
                        break;
                    case EnumCodigoRejeicaoHsbc.ValorTotalDaParcelaUnicaInvalido:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorTotalDaParcelaUnicaInvalido;
                        this.Descricao = "Valor total da parcela única inválido";
                        break;
                    case EnumCodigoRejeicaoHsbc.ValorTotalDaParcelaUnicaNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorTotalDaParcelaUnicaNaoInformado;
                        this.Descricao = "Valor total da parcela única não informado";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoDoDocumentoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDoDocumentoInvalido;
                        this.Descricao = "Código do documento inválido";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoDoDocumentoNaoNumerico:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDoDocumentoNaoNumerico;
                        this.Descricao = "Código do documento não numérico";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoDoDocumentoNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDoDocumentoNaoInformado;
                        this.Descricao = "Código do documento não informado";
                        break;
                    case EnumCodigoRejeicaoHsbc.BduDoBeneficiarioZerado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.BduDoBeneficiarioZerado;
                        this.Descricao = "BDU do beneficiário zerado";
                        break;
                    case EnumCodigoRejeicaoHsbc.BduDoBeneficiarioJaEstaEncerrado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.BduDoBeneficiarioJaEstaEncerrado;
                        this.Descricao = "BDU do beneficiário já está encerrado";
                        break;
                    case EnumCodigoRejeicaoHsbc.BeneficiarioEstaCancelado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.BeneficiarioEstaCancelado;
                        this.Descricao = "Beneficiário está cancelado";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoDoFormularioCanceladoOuZerado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDoFormularioCanceladoOuZerado;
                        this.Descricao = "Código do formulário cancelado ou zerado";
                        break;
                    case EnumCodigoRejeicaoHsbc.TipoDeMoedaDeveSerIgualEmTodosOsDetalhesDoLote:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeMoedaDeveSerIgualEmTodosOsDetalhesDoLote;
                        this.Descricao = "Tipo de moeda deve ser igual em todos os detalhes do lote";
                        break;
                    case EnumCodigoRejeicaoHsbc.SeMoedaInformadaObrigatorioValorParcelaEValorTotalParcelas:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.SeMoedaInformadaObrigatorioValorParcelaEValorTotalParcelas;
                        this.Descricao = "Se o tipo de moeda foi informado, é obrigatório informar o 'Valor Parc.' e o 'Total Valor da Parc.'";
                        break;
                    case EnumCodigoRejeicaoHsbc.SeValorInformadoObrigatorioTipoMoeda:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.SeValorInformadoObrigatorioTipoMoeda;
                        this.Descricao = "Se um dos valores foi informado, é obrigatório informar o tipo de moeda";
                        break;
                    case EnumCodigoRejeicaoHsbc.QtdParcNaoPodeSerSuperiorAoResultadoDoCalculo:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QtdParcNaoPodeSerSuperiorAoResultadoDoCalculo;
                        this.Descricao = "O campo 'Qt. Parc.' não pode ser superior ao resultado do cálculo (Parc. Até - Parc. De) + 1";
                        break;
                    case EnumCodigoRejeicaoHsbc.QtdParcNaoPodeSerSuperiorASessenta:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QtdParcNaoPodeSerSuperiorASessenta;
                        this.Descricao = "A quantidade de parcelas não pode ser superior a 60";
                        break;
                    case EnumCodigoRejeicaoHsbc.QtdParcTemQueSerMaiorQueZero:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QtdParcTemQueSerMaiorQueZero;
                        this.Descricao = "O preenchimento da 'Qt. Parc.' é obrigatório e tem que ser maior que zero";
                        break;
                    case EnumCodigoRejeicaoHsbc.QtdTotalDeParcAEmitirUltrapassouSessentaMil:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QtdTotalDeParcAEmitirUltrapassouSessentaMil;
                        this.Descricao = "A quantidade total de parcelas a emitir (Qt. Parc. X Qt. Carnês) ultrapassou 60.000";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoMaterialDisponivelSomenteParaEmissaoEmpresa:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoMaterialDisponivelSomenteParaEmissaoEmpresa;
                        this.Descricao = "Código material disponível somente para emissão empresa";
                        break;
                    case EnumCodigoRejeicaoHsbc.TipoDeMoedaInvalido2:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeMoedaInvalido2;
                        this.Descricao = "Tipo de moeda inválido";
                        break;
                    case EnumCodigoRejeicaoHsbc.UsoDeParcelaUnicaIndevidoParaQtdParcelaUm:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.UsoDeParcelaUnicaIndevidoParaQtdParcelaUm;
                        this.Descricao = "Uso de parcela única indevido para quantidade parcela 1";
                        break;
                    case EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioNomeDoPagador:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioNomeDoPagador;
                        this.Descricao = "Para boleto auto-envelopado, obrigatório informar o nome do pagador";
                        break;
                    case EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioRuaDoPagador:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioRuaDoPagador;
                        this.Descricao = "Para boleto auto-envelopado, obritatório informar a rua do pagador";
                        break;
                    case EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioNumeroResidDoPagador:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioNumeroResidDoPagador;
                        this.Descricao = "Para boleto auto-envelopado, obrigatório informar o número da residência do pagador";
                        break;
                    case EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioCidadeDoPagador:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioCidadeDoPagador;
                        this.Descricao = "Para boleto auto-envelopado, obrigatório informar a cidade do pagador";
                        break;
                    case EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioEstadoDoPagador:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioEstadoDoPagador;
                        this.Descricao = "Para boleto auto-envelopado, obrigatório informar o Estado do pagador";
                        break;
                    case EnumCodigoRejeicaoHsbc.CepDoPagadorInvalido:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CepDoPagadorInvalido;
                        this.Descricao = "CEP do pagador inválido";
                        break;
                    case EnumCodigoRejeicaoHsbc.ArquivoRecusadoPorDuplicidade:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ArquivoRecusadoPorDuplicidade;
                        this.Descricao = "Arquivo recusado por duplicidade";
                        break;
                    case EnumCodigoRejeicaoHsbc.ArquivoVazio:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ArquivoVazio;
                        this.Descricao = "Arquivo vazio";
                        break;
                    case EnumCodigoRejeicaoHsbc.ArquivoSemRegistrosAnexos:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ArquivoSemRegistrosAnexos;
                        this.Descricao = "Arquivo sem registros anexos";
                        break;
                    case EnumCodigoRejeicaoHsbc.SequenciaDeRegistrosInvalida:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.SequenciaDeRegistrosInvalida;
                        this.Descricao = "Sequencia de registros inválida";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoDeRegistroDiferenteDeZero:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDeRegistroDiferenteDeZero;
                        this.Descricao = "Código de registro diferente de zero";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoDeRemessaDiferenteDeUm:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDeRemessaDiferenteDeUm;
                        this.Descricao = "Código de remessa diferente de um (1)";
                        break;
                    case EnumCodigoRejeicaoHsbc.LiteralRemessaDiferenteDeRemessa:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.LiteralRemessaDiferenteDeRemessa;
                        this.Descricao = "Literal remessa diferente de (Remessa)";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoDeServicoDiferenteDeUm:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDeServicoDiferenteDeUm;
                        this.Descricao = "Código de serviço diferente de um (1)";
                        break;
                    case EnumCodigoRejeicaoHsbc.LiteralCobrancaDiferenteDeCobranca:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.LiteralCobrancaDiferenteDeCobranca;
                        this.Descricao = "Literal cobrança diferente de (Cobrança)";
                        break;
                    case EnumCodigoRejeicaoHsbc.DataDeGravacaoNaoNumerica:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DataDeGravacaoNaoNumerica;
                        this.Descricao = "Data de gravação não numérica";
                        break;
                    case EnumCodigoRejeicaoHsbc.DensidadeDaGravacaoDiferenteDe1600E6250:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DensidadeDaGravacaoDiferenteDe1600E6250;
                        this.Descricao = "Densidade de gravação diferente de 1600 e 6250";
                        break;
                    case EnumCodigoRejeicaoHsbc.LiteralDensidadeDiferenteDeBpi:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.LiteralDensidadeDiferenteDeBpi;
                        this.Descricao = "Literal densidade diferente de (BPI)";
                        break;
                    case EnumCodigoRejeicaoHsbc.HoraDeGravacaoNaoNumerica:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.HoraDeGravacaoNaoNumerica;
                        this.Descricao = "Hora de gravação não numérica";
                        break;
                    case EnumCodigoRejeicaoHsbc.NumeroSequencialNaoNumerico:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.NumeroSequencialNaoNumerico;
                        this.Descricao = "Número sequencial não numérico";
                        break;
                    case EnumCodigoRejeicaoHsbc.NumeroSequenciaForaDeSequencia:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.NumeroSequenciaForaDeSequencia;
                        this.Descricao = "Número sequencia fora de sequencia";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoDoBancoDiferenteDe399:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDoBancoDiferenteDe399;
                        this.Descricao = "Código do banco diferente de '399'";
                        break;
                    case EnumCodigoRejeicaoHsbc.LoteDeServicosDiferenteDeZeros:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.LoteDeServicosDiferenteDeZeros;
                        this.Descricao = "Lote de serviços diferente de zeros";
                        break;
                    case EnumCodigoRejeicaoHsbc.LoteDeServicoHeaderForaDaSequencia:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.LoteDeServicoHeaderForaDaSequencia;
                        this.Descricao = "Lote de serviço Header fora de sequencia";
                        break;
                    case EnumCodigoRejeicaoHsbc.CodigoLayoutDiferenteDe020:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoLayoutDiferenteDe020;
                        this.Descricao = "Código Layout diferente de 020";
                        break;
                    case EnumCodigoRejeicaoHsbc.TipoDeRegistroHeaderInvalido:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeRegistroHeaderInvalido;
                        this.Descricao = "Tipo de registro Header inválido";
                        break;
                    case EnumCodigoRejeicaoHsbc.TipoDeOperacaoDiferenteDeR:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeOperacaoDiferenteDeR;
                        this.Descricao = "Tipo de operação diferente de R";
                        break;
                    case EnumCodigoRejeicaoHsbc.TipoDeServicoDiferenteDe02:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeServicoDiferenteDe02;
                        this.Descricao = "Tipo de serviço diferente de 02";
                        break;
                    case EnumCodigoRejeicaoHsbc.FormaDeLancamentoDiferenteDe00:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.FormaDeLancamentoDiferenteDe00;
                        this.Descricao = "Forma de lançamento diferente de 00 (zeros)";
                        break;
                    case EnumCodigoRejeicaoHsbc.VersaoDeLoteDiferenteDe010:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.VersaoDeLoteDiferenteDe010;
                        this.Descricao = "Versão de lote diferente de 010 (dez)";
                        break;
                    case EnumCodigoRejeicaoHsbc.NumeroDeRemessaNaoNumerico:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.NumeroDeRemessaNaoNumerico;
                        this.Descricao = "Número de remessa não numérico";
                        break;
                    case EnumCodigoRejeicaoHsbc.TipoDeMontagemSoPodeSerZeroOuUm:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeMontagemSoPodeSerZeroOuUm;
                        this.Descricao = "Tipo de montagem só pode ser 0 (zero) ou 1 (um)";
                        break;
                    case EnumCodigoRejeicaoHsbc.QtdLotesDiferenteDoInformadoNoArquivo:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QtdLotesDiferenteDoInformadoNoArquivo;
                        this.Descricao = "Quantidade Lotes diferente do informado no arquivo";
                        break;
                    case EnumCodigoRejeicaoHsbc.QtdDeRegistroTrailerArqDiferenteDoInformado:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QtdDeRegistroTrailerArqDiferenteDoInformado;
                        this.Descricao = "Quantidade de registro trailer Arq. Diferente do informado";
                        break;
                    case EnumCodigoRejeicaoHsbc.ArquivoSemRegistroHeader:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ArquivoSemRegistroHeader;
                        this.Descricao = "Arquivo sem o registro Header";
                        break;
                    case EnumCodigoRejeicaoHsbc.ArquivoSemRegistroTrailer:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ArquivoSemRegistroTrailer;
                        this.Descricao = "Arquivo sem o registro Trailer";
                        break;
                    case EnumCodigoRejeicaoHsbc.ArquivoSemRegistroHeaderETrailer:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ArquivoSemRegistroHeaderETrailer;
                        this.Descricao = "Arquivo sem o registro Header e Trailer";
                        break;
                    case EnumCodigoRejeicaoHsbc.FormularioIncompativelComCodigoDePostagem:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.FormularioIncompativelComCodigoDePostagem;
                        this.Descricao = "Formulário incompatível com código de postagem";
                        break;
                    case EnumCodigoRejeicaoHsbc.BeneficiarioSemCadastroDePostagemNoHsbc:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.BeneficiarioSemCadastroDePostagemNoHsbc;
                        this.Descricao = "Beneficiário sem cadastro de postagem no HSBC";
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
                    case 001:
                        this.Codigo = (int) EnumCodigoRejeicaoHsbc.RegistroForaDeSequencia;
                        this.Descricao = "Registro fora da sequencia";
                        break;
                    case 002:
                        this.Codigo = (int) EnumCodigoRejeicaoHsbc.RegistroDuplicado;
                        this.Descricao = "Registro duplicado";
                        break;
                    case 003:
                        this.Codigo = (int) EnumCodigoRejeicaoHsbc.TipoDeRegistroInvalido;
                        this.Descricao = "Tipo de registro inválido";
                        break;
                    case 004:
                        this.Codigo = (int) EnumCodigoRejeicaoHsbc.RegistroNaoPertenceAEstaPlanilha;
                        this.Descricao = "Registro não pertence a esta planilha";
                        break;
                    case 005:
                        this.Codigo = (int) EnumCodigoRejeicaoHsbc.BduDoBeneficiarioNaoNumerico;
                        this.Descricao = "BDU do beneficiário não numérico";
                        break;
                    case 006:
                        this.Codigo = (int) EnumCodigoRejeicaoHsbc.BduDoBeneficiarioNaoInformado;
                        this.Descricao = "BDU do beneficiário não informado";
                        break;
                    case 007:
                        this.Codigo = (int) EnumCodigoRejeicaoHsbc.BduDoBeneficiarioInexistente;
                        this.Descricao = "BDU do beneficiário inexistente";
                        break;
                    case 008:
                        this.Codigo = (int) EnumCodigoRejeicaoHsbc.CodigoBeneficiarioNaoInformado;
                        this.Descricao = "Código beneficiário não informado";
                        break;
                    case 009:
                        this.Codigo = (int) EnumCodigoRejeicaoHsbc.CodigoBeneficiarioNaoNumerico;
                        this.Descricao = "Código beneficiário não numérico";
                        break;
                    case 010:
                        this.Codigo = (int) EnumCodigoRejeicaoHsbc.CodigoBeneficiarioZerado;
                        this.Descricao = "Código beneficiário zerado";
                        break;
                    case 011:
                        this.Codigo = (int) EnumCodigoRejeicaoHsbc.CodigoBeneficiarioInexistente;
                        this.Descricao = "Código beneficiário inexistente";
                        break;

                    case 012:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoFormularioNaoInformado;
                        this.Descricao = "Código formulário não informado";
                        break;
                    case 013:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoFormularioInexistente;
                        this.Descricao = "Código formuláiro inexistente";
                        break;
                    case 015:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.InformarCodBeneficiario;
                        this.Descricao = "Form. Laser c/ cód. Barras - Informar cód. Beneficiário";
                        break;
                    case 016:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.InformarDataVencimentoParcelaUnicaOuDataPrimeiroVencimento;
                        this.Descricao = "Informar a data de vencimento da parcela única e/ou data do primeiro vencimento";
                        break;
                    case 017:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.InformarValorParcelaUnicaOuValorDasParcelas;
                        this.Descricao = "Informar o valor da parcela única e/ou valor das parcelas";
                        break;
                    case 018:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.InformarTipoDaMoeda;
                        this.Descricao = "Form. Laser com cód. Barras - Informar tipo da moeda";
                        break;
                    case 019:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.InformarCodigoDocumento;
                        this.Descricao = "Form. Laser com cód. Barras - Informar cód. Documento";
                        break;
                    case 022:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.PeriodicidadeDeVencimentoInvalida;
                        this.Descricao = "Periodicidade de vencimento inválida";
                        break;
                    case 023:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipodeMontagemInvalido;
                        this.Descricao = "Tipo de montagem inválido";
                        break;
                    case 024:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeMontagemNaoInformado;
                        this.Descricao = "Tipo de montagem não informado";
                        break;
                    case 025:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QuantidadeDeCarnesInvalida;
                        this.Descricao = "Quantidade de carnês inválida";
                        break;
                    case 026:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QuantidadeDeCarnesNaoInformada;
                        this.Descricao = "Quantidade de carnês não informada";
                        break;
                    case 027:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.NumeroParcelaDeInvalido;
                        this.Descricao = "Número parcela 'De' inválido";
                        break;
                    case 028:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.NumeroParcelaDeNaoInformado;
                        this.Descricao = "Número parcela 'De' não informado";
                        break;
                    case 029:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.NumeroParcelaAteInvalido;
                        this.Descricao = "Número parcela 'Até' inválido";
                        break;
                    case 030:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.NumeroParcelaAteNaoInformado;
                        this.Descricao = "Número parcela 'Até' não informado";
                        break;
                    case 031:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QuantidadeParcelasInvalida;
                        this.Descricao = "Quantidade parcelas inválida";
                        break;
                    case 032:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QuantidadeParcelasNaoInformada;
                        this.Descricao = "Quantidade parcelas não informada";
                        break;
                    case 033:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ParcelaDeMaiorParcelaAte;
                        this.Descricao = "Parcela 'De' maior que parcela 'Até'";
                        break;
                    case 034:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DataPrimeiroVencimentoInvalida;
                        this.Descricao = "Data primeiro vencimento inválida";
                        break;
                    case 035:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DataPrimeiroVencimentoAnteriorADataDeHoje;
                        this.Descricao = "Data primeiro vencimento anterior a data de hoje";
                        break;
                    case 036:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DataPrimeiroVencimentoJaInformadaNoCapa;
                        this.Descricao = "Data primeiro vencimento já informada no Capa";
                        break;
                    case 037:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DataVencimentoParcelaUnicaInvalida;
                        this.Descricao = "Data vencimento parcela única inválida";
                        break;
                    case 038:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DataVencimentoParcelaUnicaAnteriorADataDeHoje;
                        this.Descricao = "Data vencimento parcela única anterior a data de hoje";
                        break;
                    case 039:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DataVencimentoParcelaUnicaJaInformadaNoCapa;
                        this.Descricao = "Data vencimento parcela única já informado no Capa";
                        break;
                    case 040:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeMoedaInvalido;
                        this.Descricao = "Tipo de moeda inválido";
                        break;
                    case 042:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorDaParcelaInvalido;
                        this.Descricao = "Valor da parcela inválido";
                        break;
                    case 043:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorDaParcelaNaoInformado;
                        this.Descricao = "Valor da parcela não informado";
                        break;
                    case 044:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorDaParcelaJaInformadoNoCapa;
                        this.Descricao = "Valor da parcela já informado no Capa";
                        break;
                    case 045:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorTotalDasParcelasInvalido;
                        this.Descricao = "Valor total das parcelas inválido";
                        break;
                    case 046:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorTotalDasParcelasNaoInformado;
                        this.Descricao = "Valor total das parcelas não informado";
                        break;
                    case 047:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorDaParcelaUnicaInvalido;
                        this.Descricao = "Valor da parcela única inválido";
                        break;
                    case 048:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorDaParcelaUnicaNaoInformado;
                        this.Descricao = "Valor da parcela única não informado";
                        break;
                    case 049:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorDaParcelaUnicaJaInformadoNoCapa;
                        this.Descricao = "Valor da parcela única já informado no Capa";
                        break;
                    case 050:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorTotalDaParcelaUnicaInvalido;
                        this.Descricao = "Valor total da parcela única inválido";
                        break;
                    case 051:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ValorTotalDaParcelaUnicaNaoInformado;
                        this.Descricao = "Valor total da parcela única não informado";
                        break;
                    case 058:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDoDocumentoInvalido;
                        this.Descricao = "Código do documento inválido";
                        break;
                    case 059:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDoDocumentoNaoNumerico;
                        this.Descricao = "Código do documento não numérico";
                        break;
                    case 060:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDoDocumentoNaoInformado;
                        this.Descricao = "Código do documento não informado";
                        break;
                    case 070:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.BduDoBeneficiarioZerado;
                        this.Descricao = "BDU do beneficiário zerado";
                        break;
                    case 073:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.BduDoBeneficiarioJaEstaEncerrado;
                        this.Descricao = "BDU do beneficiário já está encerrado";
                        break;
                    case 075:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.BeneficiarioEstaCancelado;
                        this.Descricao = "Beneficiário está cancelado";
                        break;
                    case 076:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDoFormularioCanceladoOuZerado;
                        this.Descricao = "Código do formulário cancelado ou zerado";
                        break;
                    case 080:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeMoedaDeveSerIgualEmTodosOsDetalhesDoLote;
                        this.Descricao = "Tipo de moeda deve ser igual em todos os detalhes do lote";
                        break;
                    case 081:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.SeMoedaInformadaObrigatorioValorParcelaEValorTotalParcelas;
                        this.Descricao = "Se o tipo de moeda foi informado, é obrigatório informar o 'Valor Parc.' e o 'Total Valor da Parc.'";
                        break;
                    case 082:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.SeValorInformadoObrigatorioTipoMoeda;
                        this.Descricao = "Se um dos valores foi informado, é obrigatório informar o tipo de moeda";
                        break;
                    case 083:
                        this.Codigo = (int) EnumCodigoRejeicaoHsbc.QtdParcNaoPodeSerSuperiorAoResultadoDoCalculo;
                        this.Descricao = "O campo 'Qt. Parc.' não pode ser superior ao resultado do cálculo (Parc. Até - Parc. De) + 1";
                        break;
                    case 084:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QtdParcNaoPodeSerSuperiorASessenta;
                        this.Descricao = "A quantidade de parcelas não pode ser superior a 60";
                        break;
                    case 085:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QtdParcTemQueSerMaiorQueZero;
                        this.Descricao = "O preenchimento da 'Qt. Parc.' é obrigatório e tem que ser maior que zero";
                        break;
                    case 087:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QtdTotalDeParcAEmitirUltrapassouSessentaMil;
                        this.Descricao = "A quantidade total de parcelas a emitir (Qt. Parc. X Qt. Carnês) ultrapassou 60.000";
                        break;
                    case 089:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoMaterialDisponivelSomenteParaEmissaoEmpresa;
                        this.Descricao = "Código material disponível somente para emissão empresa";
                        break;
                    case 090:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeMoedaInvalido2;
                        this.Descricao = "Tipo de moeda inválido";
                        break;
                    case 091:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.UsoDeParcelaUnicaIndevidoParaQtdParcelaUm;
                        this.Descricao = "Uso de parcela única indevido para quantidade parcela 1";
                        break;
                    case 096:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioNomeDoPagador;
                        this.Descricao = "Para boleto auto-envelopado, obrigatório informar o nome do pagador";
                        break;
                    case 097:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioRuaDoPagador;
                        this.Descricao = "Para boleto auto-envelopado, obritatório informar a rua do pagador";
                        break;
                    case 098:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioNumeroResidDoPagador;
                        this.Descricao = "Para boleto auto-envelopado, obrigatório informar o número da residência do pagador";
                        break;
                    case 099:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioCidadeDoPagador;
                        this.Descricao = "Para boleto auto-envelopado, obrigatório informar a cidade do pagador";
                        break;
                    case 100:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ParaBoletoAutoEnvelopadoObrigatorioEstadoDoPagador;
                        this.Descricao = "Para boleto auto-envelopado, obrigatório informar o Estado do pagador";
                        break;
                    case 112:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CepDoPagadorInvalido;
                        this.Descricao = "CEP do pagador inválido";
                        break;
                    case 114:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ArquivoRecusadoPorDuplicidade;
                        this.Descricao = "Arquivo recusado por duplicidade";
                        break;
                    case 115:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ArquivoVazio;
                        this.Descricao = "Arquivo vazio";
                        break;
                    case 116:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ArquivoSemRegistrosAnexos;
                        this.Descricao = "Arquivo sem registros anexos";
                        break;
                    case 117:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.SequenciaDeRegistrosInvalida;
                        this.Descricao = "Sequencia de registros inválida";
                        break;
                    case 118:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDeRegistroDiferenteDeZero;
                        this.Descricao = "Código de registro diferente de zero";
                        break;
                    case 119:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDeRemessaDiferenteDeUm;
                        this.Descricao = "Código de remessa diferente de um (1)";
                        break;
                    case 120:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.LiteralRemessaDiferenteDeRemessa;
                        this.Descricao = "Literal remessa diferente de (Remessa)";
                        break;
                    case 121:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDeServicoDiferenteDeUm;
                        this.Descricao = "Código de serviço diferente de um (1)";
                        break;
                    case 122:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.LiteralCobrancaDiferenteDeCobranca;
                        this.Descricao = "Literal cobrança diferente de (Cobrança)";
                        break;
                    case 123:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DataDeGravacaoNaoNumerica;
                        this.Descricao = "Data de gravação não numérica";
                        break;
                    case 124:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.DensidadeDaGravacaoDiferenteDe1600E6250;
                        this.Descricao = "Densidade de gravação diferente de 1600 e 6250";
                        break;
                    case 125:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.LiteralDensidadeDiferenteDeBpi;
                        this.Descricao = "Literal densidade diferente de (BPI)";
                        break;
                    case 126:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.HoraDeGravacaoNaoNumerica;
                        this.Descricao = "Hora de gravação não numérica";
                        break;
                    case 127:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.NumeroSequencialNaoNumerico;
                        this.Descricao = "Número sequencial não numérico";
                        break;
                    case 128:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.NumeroSequenciaForaDeSequencia;
                        this.Descricao = "Número sequencia fora de sequencia";
                        break;
                    case 129:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoDoBancoDiferenteDe399;
                        this.Descricao = "Código do banco diferente de '399'";
                        break;
                    case 130:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.LoteDeServicosDiferenteDeZeros;
                        this.Descricao = "Lote de serviços diferente de zeros";
                        break;
                    case 131:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.LoteDeServicoHeaderForaDaSequencia;
                        this.Descricao = "Lote de serviço Header fora de sequencia";
                        break;
                    case 132:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.CodigoLayoutDiferenteDe020;
                        this.Descricao = "Código Layout diferente de 020";
                        break;
                    case 133:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeRegistroHeaderInvalido;
                        this.Descricao = "Tipo de registro Header inválido";
                        break;
                    case 134:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeOperacaoDiferenteDeR;
                        this.Descricao = "Tipo de operação diferente de R";
                        break;
                    case 135:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeServicoDiferenteDe02;
                        this.Descricao = "Tipo de serviço diferente de 02";
                        break;
                    case 136:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.FormaDeLancamentoDiferenteDe00;
                        this.Descricao = "Forma de lançamento diferente de 00 (zeros)";
                        break;
                    case 137:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.VersaoDeLoteDiferenteDe010;
                        this.Descricao = "Versão de lote diferente de 010 (dez)";
                        break;
                    case 138:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.NumeroDeRemessaNaoNumerico;
                        this.Descricao = "Número de remessa não numérico";
                        break;
                    case 139:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.TipoDeMontagemSoPodeSerZeroOuUm;
                        this.Descricao = "Tipo de montagem só pode ser 0 (zero) ou 1 (um)";
                        break;
                    case 140:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QtdLotesDiferenteDoInformadoNoArquivo;
                        this.Descricao = "Quantidade Lotes diferente do informado no arquivo";
                        break;
                    case 141:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.QtdDeRegistroTrailerArqDiferenteDoInformado;
                        this.Descricao = "Quantidade de registro trailer Arq. Diferente do informado";
                        break;
                    case 142:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ArquivoSemRegistroHeader;
                        this.Descricao = "Arquivo sem o registro Header";
                        break;
                    case 143:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ArquivoSemRegistroTrailer;
                        this.Descricao = "Arquivo sem o registro Trailer";
                        break;
                    case 144:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.ArquivoSemRegistroHeaderETrailer;
                        this.Descricao = "Arquivo sem o registro Header e Trailer";
                        break;
                    case 145:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.FormularioIncompativelComCodigoDePostagem;
                        this.Descricao = "Formulário incompatível com código de postagem";
                        break;
                    case 151:
                        this.Codigo = (int)EnumCodigoRejeicaoHsbc.BeneficiarioSemCadastroDePostagemNoHsbc;
                        this.Descricao = "Beneficiário sem cadastro de postagem no HSBC";
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
