using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Dominio;
using BoletoBr.Enums;

namespace BoletoBr.Arquivo.CNAB240.Remessa
{
    public class DetalheSegmentoQRemessaCnab240
    {
        public DetalheSegmentoQRemessaCnab240(Boleto boleto)
        {
            this.CodigoBanco = boleto.BancoBoleto.CodigoBanco;
            this.CodigoOcorrencia = boleto.Remessa.CodigoOcorrencia;
            this.NumeroInscricaoSacado = boleto.SacadoBoleto.CpfCnpj;
            this.NomeSacado = boleto.SacadoBoleto.Nome;
            //this.EnderecoSacado = boleto.SacadoBoleto.EnderecoSacado.LogradouroNumeroComplementoConcatenado;
            this.BairroSacado = boleto.SacadoBoleto.EnderecoSacado.Bairro;
            this.CepSacado = boleto.SacadoBoleto.EnderecoSacado.Cep;
            this.CidadeSacado = boleto.SacadoBoleto.EnderecoSacado.Cidade;
            this.UfSacado = boleto.SacadoBoleto.EnderecoSacado.SiglaUf;
        }

        public DetalheSegmentoQRemessaCnab240()
        {
            throw new System.NotImplementedException();
        }

        public string CodigoBanco { get; set; }
        public int LoteServico { get; set; }
        public int TipoRegistro { get; set; }
        public int NumeroRegistro { get; set; }
        public string Segmento { get; set; }
        public EnumCodigoOcorrenciaRemessa CodigoOcorrencia { get; set; }
        public string TipoInscricaoSacado { get; set; }
        public string NumeroInscricaoSacado { get; set; }
        public string NomeSacado { get; set; }
        public string EnderecoSacado { get; set; }
        public string BairroSacado { get; set; }
        public string CepSacado { get; set; }
        public string CidadeSacado { get; set; }
        public string UfSacado { get; set; }
        public string TipoInscricaoAvalista { get; set; }
        public string NumeroInscricaoAvalista { get; set; }
        public string NomeAvalista { get; set; }
        public string BancoCorrespondente { get; set; }
        public string NossoNumeroCorrespondente { get; set; }
    }
}
