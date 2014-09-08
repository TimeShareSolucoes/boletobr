using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Bancos.Santander
{
    public class EscritorRemessaCnab400Santander : IEscritorArquivoRemessa
    {
        public List<string> EscreverArquivo(List<Boleto> boletosEscrever)
        {
            throw new NotImplementedException();
        }

        public void ValidarArquivoRemessa(Cedente cedente, List<Boleto> boletos, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        public string EscreverHeader(Boleto boleto, int numeroRegistro)
        {
            var header = new string(' ', 400);
            try
            {
                header = header.PreencherValorNaLinha(1, 1, "0");
                header = header.PreencherValorNaLinha(2, 2, "1");
                header = header.PreencherValorNaLinha(3, 9, "REMESSA".PadRight(7, ' '));
                header = header.PreencherValorNaLinha(10, 11, "01");
                header = header.PreencherValorNaLinha(12, 26, "COBRANCA".PadRight(15, ' '));
                // Código de Transmissão fornecido pelo Banco
                header = header.PreencherValorNaLinha(27, 46, boleto.CodigoDeTransmissao.PadLeft(20, '0')); 
                header = header.PreencherValorNaLinha(47, 76, boleto.CedenteBoleto.Nome.PadRight(30, ' '));
                header = header.PreencherValorNaLinha(77, 79, "033");
                header = header.PreencherValorNaLinha(80, 94, "SANTANDER".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("d").Replace("/", ""));
                header = header.PreencherValorNaLinha(101, 116, "0".PadLeft(16, '0'));
                header = header.PreencherValorNaLinha(117, 391, string.Empty.PadRight(294, ' '));
                header = header.PreencherValorNaLinha(392, 394, "000");
                header = header.PreencherValorNaLinha(395, 400, "000001");

                return header;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("BoletoBr{0}Falha na geração do HEADER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverDetalhe(Boleto boleto, int numeroRegistro)
        {
            #region Variáveis

            int identificadorMulta = 0;
            if (boleto.ValorMulta > 0)
                identificadorMulta = 4;

            /* Códigos de Carteira
             * 1 - Eletrônica com registro
             * 3 - Caucionada eletrônica
             * 4 - Cobrança sem registro
             * 5 - Rápida com registro
             * 6 - Caucionada rápida
             * 7 - Descontada eletrônica
             */

            var codigoCarteira = string.Empty;

            if (boleto.CarteiraCobranca.Codigo == "101")
                codigoCarteira = "5";
            if (boleto.CarteiraCobranca.Codigo == "102")
                codigoCarteira = "4";

            string enderecoSacado = boleto.SacadoBoleto.EnderecoSacado.TipoLogradouro + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Logradouro + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Numero + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Complemento.PadRight(40, ' ');

            var conta = boleto.CedenteBoleto.ContaBancariaCedente.Conta.Replace("-", "").PadLeft(10, '0');
            var complemento = conta.Substring(9, 10);

            #endregion

            var detalhe = new string(' ', 400);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, "1");
                detalhe = detalhe.PreencherValorNaLinha(2, 3, boleto.CedenteBoleto.CpfCnpj.Length == 11 ? "01" : "02");
                detalhe = detalhe.PreencherValorNaLinha(4, 17, boleto.CedenteBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", ""));
                // Código de Transmissão fornecido pelo Banco
                detalhe = detalhe.PreencherValorNaLinha(18, 37, boleto.CodigoDeTransmissao.PadLeft(20, '0'));
                detalhe = detalhe.PreencherValorNaLinha(38, 62, boleto.NumeroDocumento.PadRight(25, ' '));
                detalhe = detalhe.PreencherValorNaLinha(63, 70, boleto.NossoNumeroFormatado.PadLeft(8, ' '));
                detalhe = detalhe.PreencherValorNaLinha(71, 76, boleto.DataDesconto2.ToString("d").Replace("/", ""));
                detalhe = detalhe.PreencherValorNaLinha(77, 77, string.Empty.PadLeft(1, ' '));
                detalhe = detalhe.PreencherValorNaLinha(78, 78, identificadorMulta.ToString());
                detalhe = detalhe.PreencherValorNaLinha(79, 82, boleto.PercentualMulta.ToString().PadLeft(3, '0'));
                detalhe = detalhe.PreencherValorNaLinha(83, 84, "00");
                detalhe = detalhe.PreencherValorNaLinha(85, 97, "0000000000000"
                    /*Valor do título em outra unidade (consultar banco)*/);
                detalhe = detalhe.PreencherValorNaLinha(98, 101, string.Empty.PadLeft(4, ' '));
                detalhe = detalhe.PreencherValorNaLinha(102, 107, boleto.DataMulta.ToString("d").Replace("/", ""));
                detalhe = detalhe.PreencherValorNaLinha(108, 108, codigoCarteira);
                detalhe = detalhe.PreencherValorNaLinha(109, 110, "01" /* Código da Ocorrência*/);
                detalhe = detalhe.PreencherValorNaLinha(111, 120, boleto.NumeroDocumento.PadRight(10, ' '));
                detalhe = detalhe.PreencherValorNaLinha(121, 126, boleto.DataVencimento.ToString("d").Replace("/", ""));
                detalhe = detalhe.PreencherValorNaLinha(127, 139,
                    String.Format("{0:0.##}", boleto.ValorBoleto).PadLeft(11, '0'));
                detalhe = detalhe.PreencherValorNaLinha(140, 142, boleto.BancoBoleto.CodigoBanco.PadLeft(3, '0'));
                detalhe = detalhe.PreencherValorNaLinha(143, 147, string.Empty.PadLeft(5, '0'));
                // Espécie do documento padronizado para DM - Duplicata Mercantil
                detalhe = detalhe.PreencherValorNaLinha(148, 149, "01");
                detalhe = detalhe.PreencherValorNaLinha(150, 150, "N");
                detalhe = detalhe.PreencherValorNaLinha(151, 156, DateTime.Now.ToString("d").Replace("/", ""));

                #region INSTRUÇÕES REMESSA

                if (boleto.InstrucoesDoBoleto.Count > 2)
                    throw new Exception(string.Format("<BoletoBr>{0}Não são aceitas mais que 2 instruções padronizadas para remessa de boletos no banco Santander.", Environment.NewLine));

                var primeiraInstrucao = boleto.InstrucoesDoBoleto.FirstOrDefault();
                var segundaInstrucao = boleto.InstrucoesDoBoleto.LastOrDefault();

                if (primeiraInstrucao != null)
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, primeiraInstrucao.ToString());
                else
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, "00");

                if (segundaInstrucao != null)
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, segundaInstrucao.ToString());
                else
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, "00");

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(161, 173, String.Format("{0:0.##}", boleto.JurosMora).PadLeft(11, '0')); 
                detalhe = detalhe.PreencherValorNaLinha(174, 179, boleto.DataDesconto.ToString("d").Replace("/", ""));
                detalhe = detalhe.PreencherValorNaLinha(180, 192, String.Format("{0:0.##}", boleto.ValorDesconto).PadLeft(11, '0'));
                detalhe = detalhe.PreencherValorNaLinha(193, 205, String.Format("{0:0.##}", boleto.Iof).PadLeft(11, '0'));
                detalhe = detalhe.PreencherValorNaLinha(206, 218, String.Format("{0:0.##}", boleto.ValorAbatimento).PadLeft(11, '0'));
                detalhe = detalhe.PreencherValorNaLinha(219, 220, boleto.SacadoBoleto.CpfCnpj.Length == 11 ? "01" : "02");
                detalhe = detalhe.PreencherValorNaLinha(221, 234, boleto.SacadoBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", ""));
                detalhe = detalhe.PreencherValorNaLinha(235, 274, boleto.SacadoBoleto.Nome.PadRight(40, ' '));
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado);
                detalhe = detalhe.PreencherValorNaLinha(315, 326, boleto.SacadoBoleto.EnderecoSacado.Bairro.PadRight(12, ' '));
                detalhe = detalhe.PreencherValorNaLinha(327, 334, boleto.SacadoBoleto.EnderecoSacado.Cep.PadLeft(8, '0'));
                detalhe = detalhe.PreencherValorNaLinha(335, 349, boleto.SacadoBoleto.EnderecoSacado.Cidade.PadRight(15, ' '));
                detalhe = detalhe.PreencherValorNaLinha(350, 351, boleto.SacadoBoleto.EnderecoSacado.SiglaUf.PadRight(2, ' '));
                detalhe = detalhe.PreencherValorNaLinha(352, 381, boleto.SacadoBoleto.NomeAvalista.PadRight(30, ' '));
                detalhe = detalhe.PreencherValorNaLinha(382, 382, string.Empty.PadLeft(1, ' '));
                detalhe = detalhe.PreencherValorNaLinha(383, 383, "i".ToUpper()); // Identificador do Complemento
                detalhe = detalhe.PreencherValorNaLinha(384, 385, complemento);
                detalhe = detalhe.PreencherValorNaLinha(386, 391, string.Empty.PadLeft(6, ' '));
                detalhe = detalhe.PreencherValorNaLinha(392, 393, boleto.DiasProtesto.ToString());
                detalhe = detalhe.PreencherValorNaLinha(394, 394, string.Empty.PadRight(1, '0'));
                detalhe = detalhe.PreencherValorNaLinha(395, 400, numeroRegistro.ToString().PadLeft(6, ' '));

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do DETALHE do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverTrailer(int numeroRegistro, decimal valorTitulos)
        {
            var trailer = new string(' ', 400);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, "9");
                trailer = trailer.PreencherValorNaLinha(2, 7, numeroRegistro.ToString().PadLeft(6, '0') /*Quantidade total de linhas no arquivo*/);
                trailer = trailer.PreencherValorNaLinha(8, 20, valorTitulos.ToString("0.00").Replace(",", "").PadLeft(13, '0') /*Valor total dos títulos*/);
                trailer = trailer.PreencherValorNaLinha(21, 394, "0".PadRight(393, '0'));
                trailer = trailer.PreencherValorNaLinha(395, 400, numeroRegistro.ToString().PadLeft(6, '0'));

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do TRAILER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }
    }
}
