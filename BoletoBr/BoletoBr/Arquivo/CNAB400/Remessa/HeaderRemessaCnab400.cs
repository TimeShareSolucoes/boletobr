
using System;


namespace BoletoBr.Arquivo.CNAB400.Remessa
{
    public class HeaderRemessaCnab400
    {
        //public int TipoDeRegistro { get; set; }
        //public int Operacao { get; set; }
        //public string LiteralDeRemessa { get; set; }
        //public int CodigoDoServico { get; set; }
        //public string LiteralDeServico { get; set; }

        //public string NomeDoBanco { get; set; }
        //public DateTime DataDeGravacao { get; set; }
        //public string IdSistema { get; set; }
        //public int NumeroSequencialRegistro { get; set; }

        // Usado para identificar o banco na fábrica de arquivos de remessa
        public int CodigoBanco { get; set; }
        
        #region #DADOS 237|BRADESCO

        public string CodigoEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
        public int NumeroSequencialRemessa { get; set; }

        #endregion #DADOS 237|BRADESCO


    }
}
