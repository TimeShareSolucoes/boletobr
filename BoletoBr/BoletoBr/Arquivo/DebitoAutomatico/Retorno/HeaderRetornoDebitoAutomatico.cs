using System; 

namespace BoletoBr.Arquivo.DebitoAutomatico.Retorno
{
    public class HeaderRetornoDebitoAutomatico
    {
        /// <summary>
        /// “A”
        /// </summary>
        public string CodigoRegistro { get; set; }

        /// <summary>
        /// 1 = Remessa - Enviado pela Empresa para o Banco
        /// 2 = Retorno - Enviado pelo Banco para a Empresa
        /// </summary>
        public int CodigoRemessa { get; set; }

        /// <summary>
        /// Código atribuído pelo Banco, para seu controle interno.
        /// Este código será informado à Empresa, pelo Banco, antes
        /// da implantação do serviço de débito automático.
        /// </summary>
        public string CodigoConvenio { get; set; }

        /// <summary>
        /// Nome da Empresa
        /// </summary>
        public string NomeEmpresa { get; set; }

        /// <summary>
        /// Código do Banco na Câmara de compensação.
        /// </summary>
        public string CodigoBanco { get; set; }

        /// <summary>
        /// Nome do Banco. 
        /// </summary>
        public string NomeBanco { get; set; }

        /// <summary>
        /// Data de geração do arquivo (AAAAMMDD). 
        /// </summary>
        public DateTime DataGeracao { get; set; }

        /// <summary>
        /// Este número deverá evoluir de 1 em 1, para cada arquivo
        /// gerado, e terá uma seqüência para o Banco e outra para a
        /// Empresa.
        /// OBS. : O NSA deverá ser rigorosamente observado, pois
        /// arquivos que não estiverem na seqüência serão rejeitados,
        /// implicando no não processamento dos mesmos. 
        /// </summary>
        public int NumeroSequencial { get; set; }

        /// <summary>
        /// 04 (a partir de 02.05.2007) 
        /// </summary>
        public string VersaoLayout { get; set; }

        /// <summary>
        /// DÉBITO AUTOMÁTICO
        /// </summary>
        public string IdentificacaoServico { get; set; }

        /// <summary>
        /// Brancos
        /// Para teste, informe a palavra TESTE nas posições 146-150 
        /// </summary>
        public bool RemessaTeste { get; set; }
    }
}
