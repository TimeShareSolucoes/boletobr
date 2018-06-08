using System;
using System.Collections.Generic;
using System.Linq;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Enums;
using BoletoBr.Fabricas;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Itau
{
    public class EscritorRemessaCnab400Banrisul : IEscritorArquivoRemessaCnab400
    {
        private RemessaCnab400 _remessaEscrever;

        public EscritorRemessaCnab400Banrisul(RemessaCnab400 remessaEscrever)
        {
            _remessaEscrever = remessaEscrever;
        }

        public string EscreverHeader(HeaderRemessaCnab400 infoHeader)
        {
            var nomeEmpresa = "";
            if (infoHeader.NomeEmpresa.Length > 30)
                nomeEmpresa = infoHeader.NomeEmpresa.Substring(0, 30);
            else
                nomeEmpresa = infoHeader.NomeEmpresa.ToUpper();

            var header = new string(' ', 400);
            try
            {
                header = header.PreencherValorNaLinha(1, 1, "0");
                header = header.PreencherValorNaLinha(2, 2, "1");
                header = header.PreencherValorNaLinha(3, 9, "REMESSA");
                header = header.PreencherValorNaLinha(10,  26, string.Empty.PadLeft(17,' '));
                header = header.PreencherValorNaLinha(27, 30, infoHeader.Agencia.PadLeft(4, '0'));
                header = header.PreencherValorNaLinha(31, 38, infoHeader.CodigoEmpresa.PadLeft(8,'0'));
                header = header.PreencherValorNaLinha(39, 39, infoHeader.DvContaCorrente.PadLeft(1, '0'));
                header = header.PreencherValorNaLinha(40, 46, string.Empty.PadRight(7, ' '));
                header = header.PreencherValorNaLinha(47, 76, nomeEmpresa.PadRight(30, ' '));
                header = header.PreencherValorNaLinha(77, 87, "041BANRISUL");
                header = header.PreencherValorNaLinha(88, 94, string.Empty.PadRight(7, ' '));
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("ddMMyy").Replace("/", ""));
                header = header.PreencherValorNaLinha(101, 109, string.Empty.PadRight(9, ' '));
                var carteiras = new List<string>() {"R", "S", "X"};
                if (carteiras.Contains(infoHeader.CodigoCarteira.ToUpper()))
                {
                    header = header.PreencherValorNaLinha(110, 113, "0808"); // Para homologação usar <8808>
                    header = header.PreencherValorNaLinha(114, 114, string.Empty.PadRight(1, ' '));
                    header = header.PreencherValorNaLinha(115, 115, "P"); // Para homologação usar <X>
                    header = header.PreencherValorNaLinha(116, 116, string.Empty.PadRight(1, ' '));
                    header = header.PreencherValorNaLinha(117, 126, infoHeader.CodigoClienteOfficeBanking.PadLeft(10,'0'));
                }
                else
                    header = header.PreencherValorNaLinha(110, 126, string.Empty.PadRight(17, ' ')); // Usados apenas para carteiras R, S e X

                header = header.PreencherValorNaLinha(127, 394, string.Empty.PadRight(268, ' '));
                header = header.PreencherValorNaLinha(395, 400, "000001");

                return header;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("BoletoBr{0}Falha na geração do HEADER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverDetalhe(DetalheRemessaCnab400 infoDetalhe, int sequenciaDetalhe)
        {
            string nomeSacado = string.Empty;
            if (String.IsNullOrEmpty(infoDetalhe.NomePagador))
                nomeSacado.PadRight(30, ' ');
            else if (infoDetalhe.NomePagador.Length > 30)
                nomeSacado = infoDetalhe.NomePagador.Substring(0, 30).ToUpper();
            else
                nomeSacado = infoDetalhe.NomePagador.PadRight(30, ' ').ToUpper();

            string enderecoSacado = string.Empty;
            if (String.IsNullOrEmpty(infoDetalhe.EnderecoPagador))
                enderecoSacado.PadRight(40, ' ');
            else if (infoDetalhe.EnderecoPagador.Length > 40)
                enderecoSacado = infoDetalhe.EnderecoPagador.Substring(0, 40).ToUpper();
            else
                enderecoSacado = infoDetalhe.EnderecoPagador.PadRight(40, ' ').ToUpper();

            string cidadeSacado = string.Empty;
            if (String.IsNullOrEmpty(infoDetalhe.CidadePagador))
                cidadeSacado.PadRight(15, ' ');
            else if (infoDetalhe.CidadePagador.Length > 15)
                cidadeSacado = infoDetalhe.CidadePagador.Substring(0, 15).ToUpper();
            else
                cidadeSacado = infoDetalhe.CidadePagador.PadRight(15, ' ').ToUpper();

            /*
             * Não usado nos segmentos atuais. 25/05/2018
             * string bairroSacado = string.Empty;
             * if (String.IsNullOrEmpty(infoDetalhe.BairroPagador))
             *     bairroSacado.PadRight(12, ' ');
             * else if (infoDetalhe.BairroPagador.Length > 12)
             *     bairroSacado = infoDetalhe.BairroPagador.Substring(0, 12).ToUpper();
             * else
             *     bairroSacado = infoDetalhe.BairroPagador.PadRight(12, ' ').ToUpper();
             */
            var detalhe = new string(' ', 400);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, "1"); // Identificação do Registro Transação
                detalhe = detalhe.PreencherValorNaLinha(2, 17, string.Empty.PadLeft(16,' '));
                detalhe = detalhe.PreencherValorNaLinha(18, 21, infoDetalhe.Agencia.PadLeft(4,'0'));
                detalhe = detalhe.PreencherValorNaLinha(22, 29, infoDetalhe.CodigoCedente.PadLeft(8,'0'));
                detalhe = detalhe.PreencherValorNaLinha(30, 30, (infoDetalhe.DVCedente.Length > 1 ? infoDetalhe.DVCedente.Substring(0,1) : infoDetalhe.DVCedente) .PadLeft(1,'0'));
                detalhe = detalhe.PreencherValorNaLinha(31, 37, string.Empty.PadLeft(7,' '));
                detalhe = detalhe.PreencherValorNaLinha(38, 62, infoDetalhe.NumeroDocumento.PadLeft(25,'0'));
                detalhe = detalhe.PreencherValorNaLinha(63, 72, infoDetalhe.NossoNumeroFormatado.Replace(",","").Replace("-","").Replace(".","").PadLeft(10,'0'));
                detalhe = detalhe.PreencherValorNaLinha(73, 104, (infoDetalhe.MensagemLinha1 != null ? infoDetalhe.MensagemLinha1.Length <= 32 ? infoDetalhe.MensagemLinha1.PadLeft(32, ' '): infoDetalhe.MensagemLinha1.Substring(0,32): "").PadLeft(32, ' '));
                detalhe = detalhe.PreencherValorNaLinha(105, 107, string.Empty.PadLeft(3,' '));
                detalhe = detalhe.PreencherValorNaLinha(108, 108, infoDetalhe.CarteiraCobranca.PadLeft(1,' '));
                /*
                 CÓDIGO DE OCORRÊNCIA
                - Campo obrigatório.
                - Conteúdo:
                01 – Remessa
                02 – Pedido baixa
                04 – Concessão de abatimento
                05 – Cancelamento de abatimento
                06 – Alteração de vencimento
                07 – Alteração de uso empresa
                08 – Alteração do Seu Número
                09 – Protestar imediatamente
                10 – Sustação de protesto
                11 – Não cobrar juros de mora
                12 - Reembolso e transferência Desconto e Vendor
                13 – Reembolso e devolução Desconto e Vendor
                14 – Dados do sacador
                16 – Alteração do número de dias para protesto
                17 – Protestar imediatamente para fins de falência
                18 – Alteração do nome do Pagador
                19 – Alteração do endereço do Pagador
                20 – Alteração da cidade do Pagador
                21 – Alteração do CEP do Pagador (mudança de portadora)
                68 – Acerto dos dados do rateio de crédito Vide item 2.6.1
                69 – Cancelamento dos dados do rateio Vide item 2.6.1                 */
                detalhe = detalhe.PreencherValorNaLinha(109, 110, "01");

                /*
                 *  SEU NÚMERO
                 *  - Campo alfanumérico obrigatório.
                 *  - Seu Número com até 13 dígitos:
                 *  Informe VIDE038050 nas posições 111-120;
                 *  Informe o seu número com até 13 dígitos nas posições 038-050.
                 *  O seu número informado será impresso no bloqueto e informado no arquivo retorno.
                 */
                detalhe = detalhe.PreencherValorNaLinha(111, 120, "VIDE038050");
                detalhe = detalhe.PreencherValorNaLinha(121, 126, infoDetalhe.DataVencimento.ToString("ddMMyy"));
                detalhe = detalhe.PreencherValorNaLinha(127, 139, infoDetalhe.ValorBoleto.ToStringParaVoloresDecimais().PadLeft(13, '0'));
                // Valor Nominal do Título
                detalhe = detalhe.PreencherValorNaLinha(140, 142, "041");
                detalhe = detalhe.PreencherValorNaLinha(143, 147, string.Empty.PadLeft(5, '0'));


                /*
                 * TIPO DE DOCUMENTO
                 * - Campo numérico obrigatório.
                 * - Para todos os documentos será considerado Duplicata Mercantil.
                 * - Carteiras N, R, S e X: deixe o campo em branco.
                 * - Conteúdo:
                 * todo:04 – Cobrança Direta:
                 * O Banco imprime o bloqueto e envia para a Agência do Beneficiário, para que seja
                 * encaminhado ao Pagador.
                 * Para Tipo de Carteira D (posição 108), o bloqueto terá formato carnê.
                 * todo:06 – Cobrança Escritural:
                 * O Banco emite o bloqueto e envia ao Pagador pelo Correio.
                 * todo:08 – Cobrança Credenciada Banrisul – CCB:
                 * O Banco não emite o bloqueto. O bloqueto é impresso e expedido pelo Beneficiário. Vide
                 * item 5 (Notas). O Banco poderá fornecer formulários pré-impressos.
                 * todo:09 – Títulos de Terceiros:
                 * O Banco emite o bloqueto e envia ao Pagador pelo Correio.
                 * Obrigatório o preenchimento das posições 073-104 com o CNPJ/CPF e o NOME DO
                 * SACADOR e a inclusão do registro tipo 1 – Dados do Sacador no arquivo remessa.                 
                 */
                var TipoDocumento = "08";
                if (infoDetalhe.BancoEmiteBoleto)
                    TipoDocumento = "06";
                detalhe = detalhe.PreencherValorNaLinha(148, 149, TipoDocumento.PadLeft(2,'0'));
                if (String.IsNullOrEmpty(infoDetalhe.Aceite))
                    detalhe = detalhe.PreencherValorNaLinha(150, 150, "N");
                else
                    detalhe = detalhe.PreencherValorNaLinha(150, 150, infoDetalhe.Aceite.Equals("A") ? "A" : "N");
                // Identificação de Título Aceitou ou Não Aceito
                detalhe = detalhe.PreencherValorNaLinha(151, 156, infoDetalhe.DataEmissao.ToString("ddMMyy"));
                // Data da Emissão do Título

                #region INSTRUÇÕES REMESSA

                /*
                 * CÓDIGO DA 1ª INSTRUÇÃO
                 * - Campo numérico opcional.
                 * - Conteúdo:
                 * 09 – Protestar caso impago NN dias após o vencimento (posições 370-371 = NN).
                 * O número de dias para protesto deverá ser igual ou maior do que 03.
                 * 15 – Devolver se impago após NN dias do vencimento (posições 370-371 = NN).
                 * Obs.: Para o número de dias igual a 00 será impresso no bloqueto: “NÃO RECEBER APÓS
                 * O VENCIMENTO”.
                 * 18 – Após NN dias do vencimento, cobrar xx,x% de multa.
                 * 20 – Após NN dias do vencimento, cobrar xx,x% de multa ao mês ou fração.
                 * 23 – Não protestar.
                 * Notas:
                 * 1 – Códigos 09 ou 15:
                 * Obrigatório o preenchimento do campo Número de dias para protesto nas posições 370-
                 * 371.
                 * 2 – Códigos 18 e 20:
                 * Informe a taxa nas posições 322-324, com apenas uma casa decimal.
                 * Informe o número de dias nas posições 325-326. Se for igual a 00, considera-se “Após o
                 * vencimento”.
                 * 3 – Carteiras N, R, S e X:
                 * Não sendo autorizado a informar a taxa, este campo deve estar em branco.
                 * Se autorizado, informe a taxa nas posições 322-326, com 3 casas decimais.
                 */


                var codigoInstrucao = infoDetalhe.NroDiasParaProtesto > 0 ? "09" : 
                                        infoDetalhe.PrazoBaixaDevolucao > 0 ? "15" :
                                        "23";
                var Instrucao = infoDetalhe.NroDiasParaProtesto > 0 ? infoDetalhe.NroDiasParaProtesto :
                    infoDetalhe.PrazoBaixaDevolucao > 0 ? infoDetalhe.PrazoBaixaDevolucao : 0;
                detalhe = detalhe.PreencherValorNaLinha(157, 158, codigoInstrucao.PadLeft(2,'0'));
                detalhe = detalhe.PreencherValorNaLinha(159, 160, string.Empty.PadLeft(2,' '));
                #endregion
                #region VALOR JUROS

                var carteiras = new List<string>() { "N", "R", "S", "X" };
                if (infoDetalhe.ValorMoraDia > 0 && !carteiras.Contains(infoDetalhe.CarteiraCobranca.ToUpper()))
                {
                    detalhe = detalhe.PreencherValorNaLinha(161, 161, "1");
                    detalhe = detalhe.PreencherValorNaLinha(162, 173, infoDetalhe.ValorMoraDia.ToStringParaVoloresDecimais().PadLeft(12,'0'));
                }else
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, string.Empty.PadLeft(13,' '));
                #endregion

                if (infoDetalhe.DataLimiteConcessaoDesconto > DateTime.MinValue && infoDetalhe.ValorDesconto > 0)
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, infoDetalhe.DataLimiteConcessaoDesconto.ToString("ddMMyy"));
                else
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, string.Empty.PadLeft(6, '0'));
                detalhe = detalhe.PreencherValorNaLinha(180, 192, infoDetalhe.ValorDesconto.ToStringParaVoloresDecimais().PadLeft(13, '0'));
                detalhe = detalhe.PreencherValorNaLinha(193, 205, infoDetalhe.ValorIof.ToStringParaVoloresDecimais().PadLeft(13, '0'));
                detalhe = detalhe.PreencherValorNaLinha(206, 218, infoDetalhe.ValorAbatimento.ToStringParaVoloresDecimais().PadLeft(13, '0'));
                detalhe = detalhe.PreencherValorNaLinha(219, 220, infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11 ? "01": "02"); 
                detalhe = detalhe.PreencherValorNaLinha(221, 234, infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(235, 269, nomeSacado.PadRight(35, ' ')); 
                detalhe = detalhe.PreencherValorNaLinha(270, 274, string.Empty.PadRight(5, ' '));
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado.PadRight(40, ' '));
                detalhe = detalhe.PreencherValorNaLinha(315, 321, string.Empty.PadRight(7, ' '));

                if (infoDetalhe.PercentualMulta > 0)
                {
                    detalhe = detalhe.PreencherValorNaLinha(322, 324, infoDetalhe.PercentualMulta.ToStringParaVoloresDecimais(1).PadLeft(3,'0'));
                    detalhe = detalhe.PreencherValorNaLinha(325, 326, "01"); //caso informado percentual de multa será aplicado no primeiro dia de inadimplência
                }
                else
                    detalhe = detalhe.PreencherValorNaLinha(322, 326, string.Empty.PadLeft(5, '0'));
                var Cep = infoDetalhe.CepPagador;
                if (Cep.Contains("."))
                    Cep = Cep.Replace(".", "");
                if (Cep.Contains("-"))
                    Cep = Cep.Replace("-", "");
                if (Cep.Trim().Length != 8)
                    throw new Exception("Cep invalido");
                detalhe = detalhe.PreencherValorNaLinha(327, 334, Cep.Trim());
                
                detalhe = detalhe.PreencherValorNaLinha(335, 349, cidadeSacado.PadRight(15, ' '));
                detalhe = detalhe.PreencherValorNaLinha(350, 351, infoDetalhe.UfPagador.PadRight(2, ' '));
                detalhe = detalhe.PreencherValorNaLinha(352, 355, string.Empty.PadLeft(4,'0'));//TAXA AO DIA PARA PAGAMENTO ANTECIPADO
                detalhe = detalhe.PreencherValorNaLinha(356, 357, string.Empty.PadLeft(2, ' '));
                detalhe = detalhe.PreencherValorNaLinha(358, 369, string.Empty.PadLeft(12, carteiras.Contains(infoDetalhe.CarteiraCobranca.ToUpper()) ? ' ': '0'));//VALOR PARA CÁLCULO DO DESCONTO
                detalhe = detalhe.PreencherValorNaLinha(370, 371, infoDetalhe.NroDiasParaProtesto.ToString().PadLeft(2, '0'));
                detalhe = detalhe.PreencherValorNaLinha(372, 394, string.Empty.PadLeft(23, ' '));
                // Nro Sequencial do Registro no Arquivo
                detalhe = detalhe.PreencherValorNaLinha(395, 400, sequenciaDetalhe.ToString().PadLeft(6, '0'));
                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do DETALHE do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }
        
       

        public string EscreverTrailer(TrailerRemessaCnab400 infoTrailer, int sequenciaTrailer)
        {
            var trailer = new string(' ', 400);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, "9");
                trailer = trailer.PreencherValorNaLinha(2, 27, string.Empty.PadRight(26, ' '));
                trailer = trailer.PreencherValorNaLinha(28, 40, infoTrailer.ValorTotalTitulos.ToStringParaVoloresDecimais().PadLeft(13,'0'));
                trailer = trailer.PreencherValorNaLinha(41, 394, string.Empty.PadRight(354, ' '));
                trailer = trailer.PreencherValorNaLinha(395, 400, sequenciaTrailer.ToString().PadLeft(6, '0'));

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do TRAILER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public List<string> EscreverTexto(RemessaCnab400 remessaEscrever)
        {
            List<string> listaRetornar = new List<string>();

            listaRetornar.Add(EscreverHeader(remessaEscrever.Header));

            var sequencial = 2;
            foreach (var detalheAdicionar in remessaEscrever.RegistrosDetalhe)
            {
                listaRetornar.AddRange(new[] { EscreverDetalhe(detalheAdicionar, sequencial) });
                sequencial++;
            }

            listaRetornar.Add(EscreverTrailer(remessaEscrever.Trailer, sequencial));

            return listaRetornar;
        }

        public void ValidarRemessa(RemessaCnab400 remessaValidar)
        {
            throw new NotImplementedException();
        }
    }
}
