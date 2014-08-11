using System.Collections.Generic;
using System.Drawing;
using System.IO;
using BoletoBr.CalculoModulo;
using BoletoBr.Dominio;

namespace BoletoBr.Bancos
{
    public interface IBanco
    {
        string CodigoBanco { get; set; }
        string DigitoBanco { get; set; }
        string NomeBanco { get; set; }
        Image LogotipoBancoParaExibicao { get; set; }
        string LocalDePagamento { get; }
        /// <summary>
        /// O código da moeda é especifico em cada banco.
        ///                MOEDA
        /// COD - BANCO |REAL|VARIÁVEL
        /// 104 - CAIXA |  9 |   ?
        /// 341 - ITAÚ  |  0 |   1          
        /// 399 - HSBC  |  9 |   0            
        /// </summary>
        string MoedaBanco { get; }
        List<CarteiraCobranca> GetCarteirasCobranca();
        CarteiraCobranca GetCarteiraCobrancaPorCodigo(string codigoCarteira);
        void ValidaBoletoComNormasBanco(Boleto boleto);

        /// <summary>
        /// Formata o código/sigla da Espécie/Moeda do boleto.
        /// </summary>
        /// <param name="boleto"></param>
        void FormataMoeda(Boleto boleto);

        /// <summary>
        /// Efetua os cálculos de linha digitável do boleto.
        /// </summary>
        /// <param name="boleto"></param>
        void FormatarBoleto(Boleto boleto);

        /// <summary>
        /// Formata código de barras seguindo regras específicas do banco
        /// </summary>
        /// <param name="boleto"></param>
        void FormataCodigoBarra(Boleto boleto);

        /// <summary>
        /// Formata a linha digitável do boleto, seguindo as regras específicas do banco.
        /// </summary>
        /// <param name="boleto"></param>
        void FormataLinhaDigitavel(Boleto boleto);

        /// <summary>
        /// Formata o Nosso número do boleto, seguindo as regras específicas do banco.
        /// </summary>
        /// <param name="boleto"></param>
        void FormataNossoNumero(Boleto boleto);

        /// <summary>
        /// Formata o número do documento, seguindo as regras específicas do banco.
        /// </summary>
        /// <param name="boleto"></param>
        void FormataNumeroDocumento(Boleto boleto);

        /// <summary>
        /// Faz a leitura do arquivo de retorno.
        /// </summary>
        /// <param name="banco"></param><param name="arquivo"></param>
        void LerArquivoRetorno(IBanco banco, Stream arquivo);

        string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa);
        /// <summary>
        /// Gera o header do arquivo de remessa
        /// </summary>
        string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos);
        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa
        /// </summary>
        string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo);
        /// <summary>
        /// Gera o header de arquivo do arquivo de remessa
        /// </summary>
        string GerarHeaderRemessa(Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa);
        /// <summary>
        /// Gera o Trailer do arquivo de remessa
        /// </summary>
        string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal);
        /// <summary>
        /// Gera o header de lote do arquivo de remessa
        /// </summary>
        string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa);
        /// <summary>
        /// Gera o header de lote do arquivo de remessa
        /// </summary>
        string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo);
        /// <summary>
        /// Gera o header de lote do arquivo de remessa
        /// </summary>
        string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo, Boleto boletos);
        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa - SEGMENTO P
        /// </summary>
        string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio);
        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa - SEGMENTO P
        /// </summary>
        string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente);
        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa - SEGMENTO P
        /// </summary>
        string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente, Boleto boletos);
        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa - SEGMENTO Q
        /// </summary>
        string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo);
        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa - SEGMENTO Q
        /// </summary>
        string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, Sacado sacado);
        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa - SEGMENTO R
        /// </summary>
        string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo);
        /// <summary>
        /// Gera o Trailer de arquivo do arquivo de remessa
        /// </summary>
        string GerarTrailerArquivoRemessa(int numeroRegistro);
        /// <summary>
        /// Gera o Trailer de arquivo do arquivo de remessa
        /// </summary>
        string GerarTrailerArquivoRemessa(int numeroRegistro, Boleto boletos);
        /// <summary>
        /// Gera o Trailer de lote do arquivo de remessa
        /// </summary>
        string GerarTrailerLoteRemessa(int numeroRegistro);
        /// <summary>
        /// Gera o Trailer de lote do arquivo de remessa
        /// </summary>
        string GerarTrailerLoteRemessa(int numeroRegistro, Boleto boletos);

        DetalheSegmentoTRetornoCnab240 LerDetalheSegmentoTRetornoCnab240(string registro);

        DetalheSegmentoURetornoCnab240 LerDetalheSegmentoURetornoCnab240(string registro);

        DetalheSegmentoWRetornoCnab240 LerDetalheSegmentoWRetornoCnab240(string registro);

        DetalheRetornoGenericoCnab400 LerDetalheRetornoCnab400(string registro);

        Cedente Cedente { get; }
        int Codigo { get; set; }
        string Nome { get; }
        string Digito { get; }
    }
}
