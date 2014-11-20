using System;
using System.Collections.Generic;
using System.Linq;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Enums;
using BoletoBr.Fabricas;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.Itau
{
    public class EscritorRemessaCnab400Itau : IEscritorArquivoRemessaCnab400
    {
        private RemessaCnab400 _remessaEscrever;

        public EscritorRemessaCnab400Itau(RemessaCnab400 remessaEscrever)
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
                header = header.PreencherValorNaLinha(10, 11, "01");
                header = header.PreencherValorNaLinha(12, 26, "COBRANCA".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(27, 30, infoHeader.Agencia.PadLeft(4, '0'));
                header = header.PreencherValorNaLinha(31, 32, "00");
                header = header.PreencherValorNaLinha(33, 37, infoHeader.ContaCorrente.PadLeft(5, '0'));
                header = header.PreencherValorNaLinha(38, 38, infoHeader.DvContaCorrente);
                header = header.PreencherValorNaLinha(39, 46, string.Empty.PadRight(8, ' '));
                header = header.PreencherValorNaLinha(47, 76, nomeEmpresa.PadRight(30, ' '));
                header = header.PreencherValorNaLinha(77, 79, "341");
                header = header.PreencherValorNaLinha(80, 94, "BANCO ITAU SA".PadRight(15, ' '));
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("ddMMyy").Replace("/", ""));
                header = header.PreencherValorNaLinha(101, 394, string.Empty.PadRight(294, ' '));
                header = header.PreencherValorNaLinha(395, 400, "000001");

                return header;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("BoletoBr{0}Falha na geração do HEADER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverDetalhe(DetalheRemessaCnab400 infoDetalhe)
        {
            if (String.IsNullOrEmpty(infoDetalhe.BairroPagador))
                throw new Exception("Não foi informado o bairro do pagador " + infoDetalhe.NomePagador + "(" + infoDetalhe.InscricaoPagador + ")");

            if (String.IsNullOrEmpty(infoDetalhe.CepPagador) || infoDetalhe.CepPagador.Length < 8)
                throw new Exception("CEP Inválida! Verifique o CEP do pagador " + infoDetalhe.NomePagador + "(" + infoDetalhe.InscricaoPagador + ")");

            // Na geração do detalhe na remessa não está sendo tratado os casos de cancelamento das instruções nas posições 34-37

            #region Variáveis

            var objBanco = BancoFactory.ObterBanco(infoDetalhe.CodigoBanco);

            string nossoNumeroCarteira =
                infoDetalhe.NossoNumeroFormatado.Replace(".", "").Replace("/", "").Replace("-", "").Substring(0, 3);
            string nossoNumeroSequencial =
                infoDetalhe.NossoNumeroFormatado.Replace(".", "").Replace("/", "").Replace("-", "").Substring(3, 8);
            string nossoNumeroDigito =
                infoDetalhe.NossoNumeroFormatado.Replace(".", "").Replace("/", "").Replace("-", "").Substring(11, 1);

            string carteiraCob = infoDetalhe.CarteiraCobranca.PadLeft(3, ' ');
            string enderecoSacado = string.Empty;
            string bairroSacado = string.Empty;
            string cidadeSacado = string.Empty;

            string nomeSacado = string.Empty;

            #endregion

            if (String.IsNullOrEmpty(infoDetalhe.EnderecoPagador))
                enderecoSacado.PadRight(40, ' ');
            else
                if (infoDetalhe.EnderecoPagador.Length > 40)
                    enderecoSacado = infoDetalhe.EnderecoPagador.Substring(0, 40).ToUpper();
                else
                    enderecoSacado = infoDetalhe.EnderecoPagador.PadRight(40, ' ').ToUpper();

            if (String.IsNullOrEmpty(infoDetalhe.BairroPagador))
                bairroSacado.PadRight(12, ' ');
            else
                if (infoDetalhe.BairroPagador.Length > 12)
                    bairroSacado = infoDetalhe.BairroPagador.Substring(0, 12).ToUpper();
                else
                    bairroSacado = infoDetalhe.BairroPagador.PadRight(12, ' ').ToUpper();

            if (String.IsNullOrEmpty(infoDetalhe.CidadePagador))
                cidadeSacado.PadRight(15, ' ');
            else
                if (infoDetalhe.CidadePagador.Length > 15)
                    cidadeSacado = infoDetalhe.CidadePagador.Substring(0, 15).ToUpper();
                else
                    cidadeSacado = infoDetalhe.CidadePagador.PadRight(15, ' ').ToUpper();

            if (String.IsNullOrEmpty(infoDetalhe.NomePagador))
                nomeSacado.PadRight(30, ' ');
            else
                if (infoDetalhe.NomePagador.Length > 30)
                    nomeSacado = infoDetalhe.NomePagador.Substring(0, 30).ToUpper();
                else
                    nomeSacado = infoDetalhe.NomePagador.PadRight(30, ' ').ToUpper();


            var detalhe = new string(' ', 400);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, "1"); // Identificação do Registro Transação
                detalhe = detalhe.PreencherValorNaLinha(2, 3,
                    infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11
                        ? "01"
                        : "02"); // Tipo de Inscrição da Empresa
                detalhe = detalhe.PreencherValorNaLinha(4, 17,
                    infoDetalhe.InscricaoCedente.Replace(".", "").Replace("/", "").Replace("-", ""));
                    // Nro de Inscrição da Empresa (CPF/CNPJ)
                detalhe = detalhe.PreencherValorNaLinha(18, 21, infoDetalhe.Agencia.PadLeft(4, '0')); // Agência Mantenedora da Conta
                detalhe = detalhe.PreencherValorNaLinha(22, 23, string.Empty.PadRight(2, '0'));
                    // Complemento de Registro
                detalhe = detalhe.PreencherValorNaLinha(24, 28, infoDetalhe.ContaCorrente.PadLeft(5, '0'));
                    // Nro da Conta Corrente da Empresa
                detalhe = detalhe.PreencherValorNaLinha(29, 29, infoDetalhe.DvContaCorrente);
                    // Dígito de Auto Conferência Ag/Conta Empresa
                detalhe = detalhe.PreencherValorNaLinha(30, 33, string.Empty.PadRight(4, ' '));
                    // Complemento de Registro

                if (infoDetalhe.CodigoOcorrencia.Codigo != 35 && infoDetalhe.CodigoOcorrencia.Codigo != 38)
                    detalhe = detalhe.PreencherValorNaLinha(34, 37, "0000"); // Cód. Instrução/Alegação a ser cancelada

                const string doc = "DOC";
                var seuNumero = doc + infoDetalhe.NossoNumeroFormatado.PadRight(25 - doc.Length, ' ');

                detalhe = detalhe.PreencherValorNaLinha(38, 62, seuNumero); // Identificação do Título na Empresa
                detalhe = detalhe.PreencherValorNaLinha(63, 70, nossoNumeroSequencial);
                    // Identificação do Título no Banco

                // Se Moeda = REAL, preenche com zeros
                if (infoDetalhe.Moeda == "9" || infoDetalhe.Moeda == "09" || infoDetalhe.Moeda == "R$" ||
                    infoDetalhe.Moeda == "REAL")
                    detalhe = detalhe.PreencherValorNaLinha(71, 83, string.Empty.PadLeft(13, '0'));
                    // Quantidade de Moeda Variável
                    // Caso contrário, preenche com a quantidade
                else
                    detalhe = detalhe.PreencherValorNaLinha(71, 83,
                        infoDetalhe.QuantidadeMoeda.ToString("F5").Replace(".", "").Replace(",", "").PadLeft(13, '0')); // Quantidade de Moeda Variável
                detalhe = detalhe.PreencherValorNaLinha(84, 86, infoDetalhe.CarteiraCobranca.PadLeft(3, '0'));
                    // Número da Carteira no Banco
                detalhe = detalhe.PreencherValorNaLinha(87, 107, string.Empty.PadRight(21, ' '));
                    // Identificação da Operação no Banco
                /* Código da Carteira */
                // Modalidade de Carteira D - Direta
                if (carteiraCob == "108" || carteiraCob == "109" || carteiraCob == "110" || carteiraCob == "111")
                    detalhe = detalhe.PreencherValorNaLinha(108, 108, "I");
                // Modalidade de Carteira S - Sem Registro
                if (carteiraCob == "103" || carteiraCob == "173" || carteiraCob == "196" || carteiraCob == "198")
                    detalhe = detalhe.PreencherValorNaLinha(108, 108, "I");
                // Modalidade de Carteira E - Escritural
                if (carteiraCob == "104" || carteiraCob == "112" || carteiraCob == "138")
                    detalhe = detalhe.PreencherValorNaLinha(108, 108, "I");
                if (carteiraCob == "147")
                    detalhe = detalhe.PreencherValorNaLinha(108, 108, "E");
                detalhe = detalhe.PreencherValorNaLinha(109, 110,
                    infoDetalhe.CodigoOcorrencia.Codigo.ToString().PadLeft(2, '0')); // Identificação da Ocorrência
                detalhe = detalhe.PreencherValorNaLinha(111, 120, infoDetalhe.NumeroDocumento.Replace("-", "")); 
                detalhe = detalhe.PreencherValorNaLinha(121, 126, infoDetalhe.DataVencimento.ToString("ddMMyy"));
                    // Data de Vencimento do Título
                detalhe = detalhe.PreencherValorNaLinha(127, 139,
                    infoDetalhe.ValorBoleto.ToString("f").Replace(".", "").Replace(",", "").PadLeft(13, '0'));
                    // Valor Nominal do Título
                detalhe = detalhe.PreencherValorNaLinha(140, 142, "341"); // Nro do Banco na Câmara de Compensação
                detalhe = detalhe.PreencherValorNaLinha(143, 147, string.Empty.PadLeft(5, '0'));
                    // Agência onde o título será cobrado
                // Espécie do documento padronizado para DM - Duplicata Mercantil
                detalhe = detalhe.PreencherValorNaLinha(148, 149, infoDetalhe.Especie.Sigla.Equals("DM") ? "01" : infoDetalhe.Especie.Codigo.ToString());
                if (String.IsNullOrEmpty(infoDetalhe.Aceite))
                    detalhe = detalhe.PreencherValorNaLinha(150, 150, "N");
                else
                    detalhe = detalhe.PreencherValorNaLinha(150, 150, infoDetalhe.Aceite.Equals("A") ? "A" : "N");
                    // Identificação de Título Aceitou ou Não Aceito
                detalhe = detalhe.PreencherValorNaLinha(151, 156, infoDetalhe.DataEmissao.ToString("ddMMyy"));
                    // Data da Emissão do Título

                #region INSTRUÇÕES REMESSA

                if (infoDetalhe.Instrucoes.Count > 2)
                    throw new Exception(
                        string.Format(
                            "<BoletoBr>{0}Não são aceitas mais que 2 instruções padronizadas para remessa de boletos no banco Itaú.",
                            Environment.NewLine));

                var primeiraInstrucao = infoDetalhe.Instrucoes.FirstOrDefault();
                var segundaInstrucao = infoDetalhe.Instrucoes.LastOrDefault();

                // No caso da instrução "39", se informar "00" na posição 392-393 será impresso no boleto a literal "NÃO RECEBER APÓS O VENCIMENTO".
                if (primeiraInstrucao != null)
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, primeiraInstrucao.ToString());
                else
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, "39");

                //if (segundaInstrucao != null)
                //    detalhe = detalhe.PreencherValorNaLinha(159, 160, segundaInstrucao.ToString());
                //else
                // Pagável em qualquer agência bancária até a data de vencimento.
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, "90");

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(161, 173,
                    infoDetalhe.ValorMoraDia.ToString("f").Replace(",", "").PadLeft(13, '0'));
                    // Valor de Mora Por Dia de Atraso

                if (infoDetalhe.DataDesconto == DateTime.MinValue)
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, string.Empty.PadLeft(6, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(174, 179, infoDetalhe.DataDesconto.ToString("ddMMyy"));
                        // Data Limite para Concesão de Desconto

                detalhe = detalhe.PreencherValorNaLinha(180, 192,
                    infoDetalhe.ValorDesconto.ToString().Replace(",", "").PadLeft(13, '0'));
                    // Valor do Desconto a ser Concedido
                detalhe = detalhe.PreencherValorNaLinha(193, 205,
                    infoDetalhe.ValorIof.ToString().Replace(",", "").PadLeft(13, '0'));
                    // Valor do I.O.F. recolhido p/ notas seguro
                detalhe = detalhe.PreencherValorNaLinha(206, 218,
                    infoDetalhe.ValorAbatimento.ToString().Replace(",", "").PadLeft(13, '0'));
                    // Valor do Abatimento a ser concedido
                detalhe = detalhe.PreencherValorNaLinha(219, 220,
                    infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11
                        ? "01"
                        : "02"); // Identificação do tipo de inscrição/sacado
                detalhe = detalhe.PreencherValorNaLinha(221, 234,
                    infoDetalhe.InscricaoPagador.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                    // Nro de Inscrição do Sacado (CPF/CNPJ)
                detalhe = detalhe.PreencherValorNaLinha(235, 264, nomeSacado.PadRight(30, ' ')); // Nome do Sacado
                detalhe = detalhe.PreencherValorNaLinha(265, 274, string.Empty.PadRight(10, ' ')); // Complemento de registro
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado.PadRight(40, ' '));
                    // Rua, Número, e Complemento do Sacado
                detalhe = detalhe.PreencherValorNaLinha(315, 326, bairroSacado.PadRight(12, ' ')); // Bairro do Sacado

                var Cep = infoDetalhe.CepPagador;

                if (Cep.Contains("."))
                    Cep = Cep.Replace(".", "");
                if (Cep.Contains("-"))
                    Cep = Cep.Replace("-", "");

                detalhe = detalhe.PreencherValorNaLinha(327, 334, Cep);
                detalhe = detalhe.PreencherValorNaLinha(335, 349, cidadeSacado.PadRight(15, ' '));
                detalhe = detalhe.PreencherValorNaLinha(350, 351, infoDetalhe.UfPagador.PadRight(2, ' '));

                if (String.IsNullOrEmpty(infoDetalhe.NomeAvalistaOuMensagem2))
                    detalhe = detalhe.PreencherValorNaLinha(352, 381, string.Empty.PadRight(30, ' '));
                        // Nome do Sacador ou Avalista
                else
                    detalhe = detalhe.PreencherValorNaLinha(352, 381, infoDetalhe.NomeAvalistaOuMensagem2.PadRight(30, ' '));
                        // Nome do Sacador ou Avalista

                detalhe = detalhe.PreencherValorNaLinha(382, 385, string.Empty.PadRight(4, ' '));
                    // Complemento do Registro

                if (infoDetalhe.DataJurosMora == DateTime.MinValue)
                    detalhe = detalhe.PreencherValorNaLinha(386, 391, string.Empty.PadLeft(6, '0')); // Data de Mora
                else
                    detalhe = detalhe.PreencherValorNaLinha(386, 391, infoDetalhe.DataJurosMora.ToString("ddMMyy"));
                        // Data de Mora
                detalhe = detalhe.PreencherValorNaLinha(392, 393, infoDetalhe.NroDiasParaProtesto.ToString().PadLeft(2, '0'));
                    // Quantidade de Dias Posição 392 a 393
                detalhe = detalhe.PreencherValorNaLinha(394, 394, string.Empty.PadRight(1, ' '));
                    // Complemento do Registro
                detalhe = detalhe.PreencherValorNaLinha(395, 400, infoDetalhe.NumeroSequencialRegistro.ToString().PadLeft(6, '0'));
                    // Nro Sequencial do Registro no Arquivo

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do DETALHE do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverRegistroMensagemFrente(DetalheFrenteRemessaCnab400 infoDetalheFrente)
        {
            var registroMensagemFrente = new string(' ', 400);
            try
            {
                registroMensagemFrente = registroMensagemFrente.PreencherValorNaLinha(1, 1, "7");
                registroMensagemFrente = registroMensagemFrente.PreencherValorNaLinha(2, 4, infoDetalheFrente.CodigoFlash.PadRight(3, ' '));
                registroMensagemFrente = registroMensagemFrente.PreencherValorNaLinha(5, 6, infoDetalheFrente.NroLinha1.ToString());
                registroMensagemFrente = registroMensagemFrente.PreencherValorNaLinha(7, 134, infoDetalheFrente.ConteudoLinha1);
                registroMensagemFrente = registroMensagemFrente.PreencherValorNaLinha(135, 136, infoDetalheFrente.NroLinha2.ToString());
                registroMensagemFrente = registroMensagemFrente.PreencherValorNaLinha(137, 264, infoDetalheFrente.ConteudoLinha2);
                registroMensagemFrente = registroMensagemFrente.PreencherValorNaLinha(265, 266, infoDetalheFrente.NroLinha3.ToString());
                registroMensagemFrente = registroMensagemFrente.PreencherValorNaLinha(267, 393, infoDetalheFrente.ConteudoLinha3);
                registroMensagemFrente = registroMensagemFrente.PreencherValorNaLinha(395, 400, infoDetalheFrente.NumeroSequencialRegistro.ToString().PadLeft(6, '0'));

                return registroMensagemFrente;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do REGISTRO DE MENSAGENS do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverTrailer(TrailerRemessaCnab400 infoTrailer)
        {
            var trailer = new string(' ', 400);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, "9");
                trailer = trailer.PreencherValorNaLinha(2, 394, string.Empty.PadRight(393, ' '));
                // Contagem total de linhas do arquivo no formato '000000' - 6 dígitos
                trailer = trailer.PreencherValorNaLinha(395, 400, infoTrailer.NumeroSequencialRegistro.ToString().PadLeft(6, '0'));

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

            foreach (var detalheAdicionar in remessaEscrever.RegistrosDetalhe)
            {
                listaRetornar.AddRange(new[] { EscreverDetalhe(detalheAdicionar) });
            }

            listaRetornar.Add(EscreverTrailer(remessaEscrever.Trailer));

            return listaRetornar;
        }

        public void ValidarRemessa(RemessaCnab400 remessaValidar)
        {
            throw new NotImplementedException();
        }
    }
}
