using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Arquivo.CNAB240.Remessa;
using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;
using BoletoBr.Enums;
using BoletoBr.Fabricas;
using BoletoBr.View.Win.Dominio;
using BoletoBr.View.XtraReport;
using DevExpress.Utils.NonclientArea;

namespace BoletoBr.View.Win
{
    public class Funcoes
    {
        public static Boleto GerarBoleto(CarteiraBoleto carteiraBoleto, SacadoBoleto sacado, decimal valor,
            DateTime dataVencimento, string numeroDocumento)
        {
            try
            {
                if (carteiraBoleto == null) throw new Exception("Carteira inválida.");
                if (sacado == null) throw new Exception("Dados do sacado inválido.");
                if (valor <= 0) throw new Exception("Valor deve ser maior que 0.");
                if (dataVencimento == DateTime.MinValue) throw new Exception("Data de vencimento inválida.");
                if (numeroDocumento.Trim().Length == 0) throw new Exception("Número documento inválido.");

                /* Geração pelo BoletoBr */
                BoletoBr.Boleto boletoBancarioGerado = TransformaDeFormatoBoletoParaFormatoBoletoBr(carteiraBoleto,
                    sacado, valor, dataVencimento, numeroDocumento);
                if (boletoBancarioGerado == null)
                    throw new Exception("Os dados do(s) boleto(s) não são válidos!");

                return boletoBancarioGerado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static BoletoBr.Boleto TransformaDeFormatoBoletoParaFormatoBoletoBr(CarteiraBoleto carteiraBoleto,
            SacadoBoleto sacado, decimal valor, DateTime dataVencimento, string numeroDocumento)
        {
            var vencimento = dataVencimento;
            var valorBoleto = valor;
            var nroConvenio = carteiraBoleto.NumeroConvenio;
            var identificadorInternoDocumento = numeroDocumento;

            #region Dados Cedente

            var codigoCedente = carteiraBoleto.CodigoCedente.BoletoBrToStringSafe();
            var digitoCedente = carteiraBoleto.DigitoCodigoCedente.BoletoBrToInt();
            var codTransmissao = carteiraBoleto.CodigoTransmissao.BoletoBrToStringSafe();
            var cpfcnpjcedente = carteiraBoleto.CpfCnpjCedente.BoletoBrToStringSafe();
            var descricaocedente = carteiraBoleto.NomeCedente.BoletoBrToStringSafe();

            var agencia = carteiraBoleto.NumeroAgencia;
            var digitoAgencia = carteiraBoleto.DigitoAgencia;
            var conta = carteiraBoleto.NumeroConta;
            var digitoConta = carteiraBoleto.DigitoConta;

            var enderecoCedente = carteiraBoleto.EnderecoCedente;
            var bairroCedente = carteiraBoleto.BairroCedente;
            var complementoEndCedente = carteiraBoleto.ComplementoCedente;
            var numeroEnderecoCedente = carteiraBoleto.NumeroCedente;
            var cepCedente = carteiraBoleto.CepCedente;
            var cidadeCedente = carteiraBoleto.CidadeCedente;
            var ufEnderecoCedente = carteiraBoleto.UfCedente;

            var contaBancariaCedente = new BoletoBr.ContaBancaria(agencia, digitoAgencia, conta, digitoConta);
            var cedente = new BoletoBr.Cedente(codigoCedente, nroConvenio, digitoCedente, cpfcnpjcedente,
                descricaocedente, contaBancariaCedente, new Endereco()
                {
                    Bairro = bairroCedente,
                    Cep = cepCedente,
                    Cidade = cidadeCedente,
                    Complemento = complementoEndCedente,
                    Logradouro = enderecoCedente,
                    Numero = numeroEnderecoCedente,
                    SiglaUf = ufEnderecoCedente,
                });

            #endregion

            var codBanco = carteiraBoleto.CodigoBanco.PadLeft(3, '0');

            var boleto = new BoletoBr.Boleto();
            var objBanco = BoletoBr.Fabricas.BancoFactory.ObterBanco(codBanco);
            boleto.TipoModalidade = "";
            boleto.Moeda = objBanco.MoedaBanco;
            boleto.BancoBoleto = objBanco;
            boleto.Aceite = "N";
            boleto.DataProcessamento = DateTime.Now;
            boleto.CarteiraCobranca = new CarteiraCobranca();
            
            #region Dados Carteira 

            var numeroCarteira = carteiraBoleto.NumeroCarteira;
            var variacaoCarteira = "";

            #region Variação Carteiras BB

            /* Banco do Brasil tratar variação carteira (Compor variação sempre com o numero da carteira separado por / ou -) */
            if (codBanco == "001")
            {
                if (numeroCarteira.Contains("/"))
                {
                    var carteiraVariacao = numeroCarteira.Split(Convert.ToChar("/"));

                    numeroCarteira = carteiraVariacao[0];
                    variacaoCarteira = carteiraVariacao[1];
                }
                else if (numeroCarteira.Contains("-"))
                {
                    var carteiraVariacao = numeroCarteira.Split(Convert.ToChar("-"));

                    numeroCarteira = carteiraVariacao[0];
                    variacaoCarteira = carteiraVariacao[1];
                }
            }

            #endregion

            boleto.CodigoDeTransmissao = codTransmissao;

            boleto.CarteiraCobranca.Codigo = numeroCarteira;
            boleto.CarteiraCobranca.Variacao = variacaoCarteira;
            boleto.CarteiraCobranca.Descricao = carteiraBoleto.DescricaoCarteira;
            boleto.CarteiraCobranca.BancoEmiteBoleto = carteiraBoleto.BancoGeraBoleto;
            boleto.CarteiraCobranca.Tipo = carteiraBoleto.TipoArquivoRemessa;

            if (string.IsNullOrEmpty(descricaocedente))
                throw new Exception(
                    $"Não foi informado o nome do beneficário na carteira de cobrança: {boleto.CarteiraCobranca.Descricao}.");

            #endregion

            #region Instruções/Mensagem

            var existeMensagem1 = string.IsNullOrEmpty(carteiraBoleto.Instrucao1) == false;
            var existeMensagem2 = string.IsNullOrEmpty(carteiraBoleto.Instrucao2) == false;
            var existeMensagem3 = string.IsNullOrEmpty(carteiraBoleto.Instrucao3) == false;
            var existeMensagem4 = string.IsNullOrEmpty(carteiraBoleto.Instrucao4) == false;
            var existeMensagem5 = string.IsNullOrEmpty(carteiraBoleto.Instrucao5) == false;
            var existeMensagem6 = string.IsNullOrEmpty(carteiraBoleto.Instrucao6) == false;

            if (existeMensagem1)
            {
                var instrucaoAdd = new InstrucaoCustomizada(carteiraBoleto.Instrucao1);
                boleto.InstrucoesDoBoleto.Add(instrucaoAdd);
            }
            if (existeMensagem2)
            {
                var instrucaoAdd = new InstrucaoCustomizada(carteiraBoleto.Instrucao2);
                boleto.InstrucoesDoBoleto.Add(instrucaoAdd);
            }
            if (existeMensagem3)
            {
                var instrucaoAdd = new InstrucaoCustomizada(carteiraBoleto.Instrucao3);
                boleto.InstrucoesDoBoleto.Add(instrucaoAdd);
            }
            if (existeMensagem4)
            {
                var instrucaoAdd = new InstrucaoCustomizada(carteiraBoleto.Instrucao4);
                boleto.InstrucoesDoBoleto.Add(instrucaoAdd);
            }
            if (existeMensagem5)
            {
                var instrucaoAdd = new InstrucaoCustomizada(carteiraBoleto.Instrucao5);
                boleto.InstrucoesDoBoleto.Add(instrucaoAdd);
            }
            if (existeMensagem6)
            {
                var instrucaoAdd = new InstrucaoCustomizada(carteiraBoleto.Instrucao6);
                boleto.InstrucoesDoBoleto.Add(instrucaoAdd);
            }

            #endregion

            #region Dados Sacado

            var cpfcnpjSacado = sacado.CpfCnpj;
            var nomeSacado = sacado.Nome;

            boleto.SacadoBoleto = new BoletoBr.Sacado(nomeSacado, cpfcnpjSacado,
                TransformaEnderecoParaEnderecoBoletoBr(sacado));

            #endregion

            /* Ajuste */
            boleto.LocalPagamento = boleto.BancoBoleto.LocalDePagamento;
            boleto.DataVencimento = vencimento;
            boleto.CedenteBoleto = cedente;

            // Padronizado para DM - Duplicata Mercantil
            boleto.Especie = codBanco == "399"
                ? new EspecieDocumento(02, "Duplicata Mercantil", "PD")
                : new EspecieDocumento(02, "Duplicata Mercantil", "DM");
            boleto.CodigoOcorrenciaRemessa = objBanco.ObtemCodigoOcorrencia(EnumCodigoOcorrenciaRemessa.Registro, 0,
                DateTime.Now);

            if (boleto.DataDocumento <= DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;

            boleto.NumeroDocumento = identificadorInternoDocumento;
            boleto.IdentificadorInternoBoleto = identificadorInternoDocumento;

            boleto.ValorBoleto = valorBoleto;
            boleto.PercentualJurosMora = carteiraBoleto.ValorJuros;
            boleto.InformacoesAdicionais = new List<InformacoesAdicionais>();

            objBanco.FormatarBoleto(boleto);

            return boleto;
        }

        private static BoletoBr.Endereco TransformaEnderecoParaEnderecoBoletoBr(SacadoBoleto sacadoBoleto)
        {
            var ret = new BoletoBr.Endereco()
            {
                TipoLogradouro = "",
                Logradouro = sacadoBoleto.Logradouro,
                Complemento = sacadoBoleto.Complemento,
                Numero = sacadoBoleto.Numero,
                Bairro = sacadoBoleto.Bairro,
                Cidade = sacadoBoleto.Cidade,
                Cep = sacadoBoleto.Cep,
                SiglaUf = sacadoBoleto.Uf
            };

            return ret;
        }

        public static BoletoBr.View.XtraReport.ModeloBoleto RetornaModeloBoletoUtilizar(string modeloBoleto)
        {
            ModeloBoleto modeloBoletoUtilizar;
            modeloBoleto = modeloBoleto.ToUpper();

            if (modeloBoleto == "CARNE")
                modeloBoletoUtilizar = ModeloBoleto.Carne;
            else if (modeloBoleto == "FATURA")
                modeloBoletoUtilizar = ModeloBoleto.Fatura;
            else if (modeloBoleto == "FATURA-CARTA")
                modeloBoletoUtilizar = ModeloBoleto.FaturaCarta;
            else if (modeloBoleto == "CARNE-A5")
                modeloBoletoUtilizar = ModeloBoleto.CarneA5;
            else
                modeloBoletoUtilizar = ModeloBoleto.Normal;

            return modeloBoletoUtilizar;
        }

        public static string GerarArquivoRemessa(CarteiraBoleto carteiraBoleto, SacadoBoleto sacado, decimal valor,
            DateTime dataVencimento, string numeroDocumento)
        {
            try
            {
                var listaBoletoBrRemessa = new List<Boleto>();
                var sequenciaRemessaCarteiraBoleto = 1;

                try
                {
                    // Transforma boletos e adiciona lançamentos na lista
                    listaBoletoBrRemessa.Add(TransformaDeFormatoBoletoParaFormatoBoletoBr(carteiraBoleto, sacado,
                        valor, dataVencimento, numeroDocumento));
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                #region Validações (Banco e Carteira)

                /* Garantir que todos os boletos pertencem a mesmo banco e carteira de boleto */
                var bancosEncontrados = listaBoletoBrRemessa.GroupBy(gb => gb.BancoBoleto.CodigoBanco);
                if (bancosEncontrados.Count() > 1)
                    throw new Exception(
                        "Há mais de 1 banco associado aos boletos que foram informados para geração de remessa.");

                var carteirasCobrancaEncontradas = listaBoletoBrRemessa.GroupBy(gb => gb.CarteiraCobranca.Codigo);
                if (carteirasCobrancaEncontradas.Count() > 1)
                {
                    throw new Exception(
                        "Há mais de 1 tipo de carteira de cobrança associado aos boletos que foram informados para geração de remessa.");
                }

                if (listaBoletoBrRemessa.Any(qry => qry.DataVencimento.Date < DateTime.Now.Date))
                    throw new Exception(
                        "Há boletos com data de vencimento menor que a data atual. Impossível continuar.");

                #endregion

                var carteiraCobrancaPadrao = listaBoletoBrRemessa.First().CarteiraCobranca;
                var boletoPadrao = listaBoletoBrRemessa.First();

                var fabricaRemessa = new RemessaFactory();

                #region #Criação do Diretório e Arquivo.REM

                var data = $"{DateTime.Now.ToString("ddMMyy")}";
                var dataBradesco = $"{DateTime.Now.ToString("ddMM")}";
                string nomeArquivo = null;

                // Se for BRADESCO formata nome do arquivo para CBDDMM??.REM
                if (boletoPadrao.BancoBoleto.CodigoBanco == "237")
                    nomeArquivo = $"{"CB"}{dataBradesco}{"TS"}{".REM"}";
                else
                /*
                        * C -> Cobrança
                        * Data da geração em formato DDMMAA
                        * G -> Geral
                    */
                    nomeArquivo = $"{"C"}{data}{"G"}{".REM"}";

                var dir = Directory.CreateDirectory(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    boletoPadrao.BancoBoleto.CodigoBanco));

                var caminho = dir.FullName + "\\" + nomeArquivo;

                #endregion

                #region #Informações Essenciais

                /* Usado no Header do Arquivo
                 * Cada arquivo enviado a CAIXA deverá conter um número único.
                 */
                // TODO: Criar tabela para controlar quantidade de arquivos remessa enviados ao banco.
                const int sequencialArquivo = 1;

                /* Usado no Header de Lote
                * Para cada remessa enviada deverá ser acrescido em 1, se a remessa for recusada a numeração não poderá ser aproveitada.
                */
                // TODO: Criar tabela para controlar quantidade de arquivos remessa gerados.
                int sequencialRemessa = sequenciaRemessaCarteiraBoleto;

                /* Usado no Trailer de Lote
                * Conta a quantidade de itens na lista multiplica por 2 ( cada boleto terá um detalhe segmento P e um detalhe segmento Q ) e adiciona + 2 (Header do Lote e Trailer do Lote)
                */
                // TODO: Atualmente só considera 1 lote de informações no arquivo.
                var qtdSegmantos = carteiraBoleto.ValorMulta > 0 && carteiraBoleto.BancoGeraBoleto ? 3 : 2;
                var qtdRegistrosNoLote = (listaBoletoBrRemessa.Count * qtdSegmantos) + 2;

                /* Usado no Trailer do Arquivo */
                // TODO: Atualmente só gera 1 lote de informações no arquivo.
                const int qtdLotesArquivo = 1;

                // Conta a quantidade de itens na lista  multiplica por 2 ( cada boleto terá um detalhe segmento P e um detalhe segmento Q ) e adiciona + 4 (Header do Arquivo, Header do Lote, Trailer do Lote e Trailer do Arquivo)
                var qtdRegistrosNoArquivoCnab240 = (listaBoletoBrRemessa.Count*2) + 4;

                // Conta a quantidade de itens na lista e adiciona + 2 (Header do Arquivo + Trailer do Arquivo)
                var qtdRegistrosNoArquivoCnab400 = listaBoletoBrRemessa.Count();
                qtdRegistrosNoArquivoCnab400 += 2;

                // Pega número de linhas de instruções de cada boleto
                // No caso do Bradesco desconsiderar a qtd de instruções
                // Itau desconsiderar
                // HSBC desconsiderar
                var qtdRegInstrucoes = 0;
                if (boletoPadrao.BancoBoleto.CodigoBanco != "237" &&
                    boletoPadrao.BancoBoleto.CodigoBanco != "341" &&
                    boletoPadrao.BancoBoleto.CodigoBanco != "399")
                    qtdRegInstrucoes += listaBoletoBrRemessa.Sum(boleto => boleto.InstrucoesDoBoleto.Count);

                Boleto primeiroBoletoDaLista = listaBoletoBrRemessa.First();

                #endregion

                #region CNAB240

                // Gera Remessa Cnab240
                if (carteiraCobrancaPadrao.Tipo == "CNAB240")
                {
                    var remessa = new RemessaCnab240
                    {
                        Header = new HeaderRemessaCnab240(primeiroBoletoDaLista, sequencialRemessa)
                    };

                    var loteRemessa = new LoteRemessaCnab240
                    {
                        HeaderLote = new HeaderLoteRemessaCnab240(primeiroBoletoDaLista, sequencialRemessa),
                        TrailerLote = new TrailerLoteRemessaCnab240(qtdRegistrosNoLote)
                    };

                    remessa.Trailer = new TrailerRemessaCnab240(qtdLotesArquivo, qtdRegistrosNoArquivoCnab240);

                    var escritor = EscritorArquivoRemessaFactory.ObterEscritorRemessa(remessa);

                    var remessaPronta = fabricaRemessa.GerarRemessa(remessa.Header, loteRemessa.HeaderLote,
                        listaBoletoBrRemessa, loteRemessa.TrailerLote, remessa.Trailer);

                    var linhasEscrever = escritor.EscreverTexto(remessaPronta);

                    // Escreve em UTF-8
                    File.WriteAllLines(caminho, linhasEscrever.ToArray(), Encoding.GetEncoding(1252));
                }

                #endregion

                #region CNAB400

                // Gera Remessa Cnab400
                if (carteiraCobrancaPadrao.Tipo == "CNAB400")
                {
                    var remessa = new RemessaCnab400
                    {
                        Header = new HeaderRemessaCnab400(primeiroBoletoDaLista, sequencialRemessa, 1),
                        RegistrosDetalhe = new List<DetalheRemessaCnab400>(),
                        Trailer = new TrailerRemessaCnab400(listaBoletoBrRemessa.Sum(s => s.ValorBoleto),
                            qtdRegistrosNoArquivoCnab400 + qtdRegInstrucoes)
                    };

                    var escritor = EscritorArquivoRemessaFactory.ObterEscritorRemessa(remessa);

                    var sequencial = 2;
                    DetalheRemessaCnab400 detalhe;

                    foreach (var boletoRemessa400 in listaBoletoBrRemessa)
                    {
                        detalhe = new DetalheRemessaCnab400(boletoRemessa400, sequencial);
                        remessa.RegistrosDetalhe.Add(detalhe);
                        sequencial++;
                    }

                    var remessaPronta = fabricaRemessa.GerarRemessa(remessa.Header, listaBoletoBrRemessa,
                        remessa.RegistrosDetalhe, remessa.Trailer);
                    var linhasEscrever = escritor.EscreverTexto(remessaPronta);
                    var linhasTratadasEscrever = new List<string>();
                    foreach (var linha in linhasEscrever)
                    {
                        /*/
                            * -> Codificação ASCII 8-bit (Estendido)
                            * Encoding.GetEncoding(437)
                            * 
                            * -> Codificação ASCII 7-bit
                            * Encoding.GetEncoding(1252)
                            * 
                        /*/

                        if (boletoPadrao.BancoBoleto.CodigoBanco == "756")
                        {
                            var bytes = System.Text.Encoding.Default.GetBytes(linha);
                            var linhaConvertida = System.Text.Encoding.Default.GetString(bytes);

                            var linhaTratada = linhaConvertida;

                            linhasTratadasEscrever.Add(linhaTratada);
                        }
                        else
                        {
                            var bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(linha);
                            var linhaConvertida = System.Text.Encoding.UTF8.GetString(bytes);

                            var linhaTratada = linhaConvertida;

                            linhasTratadasEscrever.Add(linhaTratada);
                        }
                    }

                    if (boletoPadrao.BancoBoleto.CodigoBanco == "756")
                        File.WriteAllLines(caminho, linhasTratadasEscrever, Encoding.Default);
                    else
                        File.WriteAllLines(caminho, linhasTratadasEscrever);
                }

                #endregion

                return caminho;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
