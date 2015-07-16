using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BoletoBr.Dominio;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.BRB
{
    public class LeitorRetornoCnab400BRB : ILeitorArquivoRetornoCnab400
    {
        private readonly List<string> _linhasArquivo;

        public LeitorRetornoCnab400BRB(List<string> linhasArquivo)
        {
            _linhasArquivo = linhasArquivo;
        }

        public RetornoCnab400 ProcessarRetorno(TipoArquivo tipoArquivo)
        {
            throw new System.NotImplementedException();
        }

        public RetornoCnab400 ProcessarRetorno()
        {
            #region Validações

            ValidaArquivoRetorno();

            #endregion

            var objRetornar = new RetornoCnab400 {RegistrosDetalhe = new List<DetalheRetornoCnab400>()};

            foreach (var linhaAtual in _linhasArquivo)
            {
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "0")
                {
                    objRetornar.Header = ObterHeader(linhaAtual);
                }
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "1")
                {
                    var objDetalhe = ObterRegistrosDetalhe(linhaAtual);
                    objRetornar.RegistrosDetalhe.Add(objDetalhe);
                }
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "9")
                {
                    objRetornar.Trailer = ObterTrailer(linhaAtual);
                }
            }

            return objRetornar;
        }

        public void ValidaArquivoRetorno()
        {
            if (_linhasArquivo == null)
                throw new Exception("Dados do arquivo de retorno estão nulos. Impossível processar.");

            if (_linhasArquivo.Count <= 0)
                throw new Exception("Dados do arquivo de retorno não estão corretos. Impossível processar.");

            if (_linhasArquivo.Count < 3)
                throw new Exception("Dados do arquivo de retorno não contém o mínimo de 3 linhas. Impossível processar.");

            var qtdLinhasHeader =
                _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(1, 1) == "0");

            if (qtdLinhasHeader <= 0)
                throw new Exception("Não foi encontrado HEADER do arquivo de retorno.");

            if (qtdLinhasHeader > 1)
                throw new Exception("Não é permitido mais de um HEADER no arquivo de retorno.");

            var qtdLinhasDetalhe = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(1, 1) == "1");

            if (qtdLinhasDetalhe <= 0)
                throw new Exception("Não foi encontrado DETALHE do arquivo de retorno.");

            var qtdLinhasTrailer = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(1, 1) == "9");

            if (qtdLinhasTrailer <= 0)
                throw new Exception("Não foi encontrado TRAILER do arquivo de retorno.");

            if (qtdLinhasTrailer > 1)
                throw new Exception("Não é permitido mais de um TRAILER no arquivo de retorno.");
        }

        public HeaderRetornoCnab400 ObterHeader(string linha)
        {
            var objRetornar = new HeaderRetornoCnab400
            {
                CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt(),
                CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).BoletoBrToInt(),
                LiteralRetorno = linha.ExtrairValorDaLinha(3, 9),
                CodigoDoServico = linha.ExtrairValorDaLinha(10, 11),
                LiteralServico = linha.ExtrairValorDaLinha(12, 19),
                ContaCorrente = linha.ExtrairValorDaLinha(27, 46),
                NomeDoBeneficiario = linha.ExtrairValorDaLinha(47, 76),
                CodigoDoBanco = linha.ExtrairValorDaLinha(77, 79),
                NomeDoBanco = linha.ExtrairValorDaLinha(80, 82),
                DataGeracaoGravacao = Convert.ToDateTime(linha.ExtrairValorDaLinha(95, 102).ToDateTimeFromDdMmAaaa()),
                DataCredito = Convert.ToDateTime(linha.ExtrairValorDaLinha(380, 387).ToDateTimeFromDdMmAaaa()),
                NumeroSequencial = linha.ExtrairValorDaLinha(395, 400)
            };

            return objRetornar;
        }

        public DetalheRetornoCnab400 ObterRegistrosDetalhe(string linha)
        {
            var objRetornar = new DetalheRetornoCnab400
            {
                CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt()
            };

            if (objRetornar.CodigoDoRegistro == 1)
            {
                objRetornar.CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt();
                objRetornar.TipoInscricao = linha.ExtrairValorDaLinha(2, 3).BoletoBrToInt();
                objRetornar.NumeroInscricao = linha.ExtrairValorDaLinha(4, 17).BoletoBrToLong();
                //18 a 20	Branco
                objRetornar.ContaCorrente = linha.ExtrairValorDaLinha(21, 37);
                //38 a 62	Branco
                //63 a 70	Branco
                objRetornar.NossoNumero = linha.ExtrairValorDaLinha(71, 82);
                //83 a 92	Branco
                objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(93, 105);
                //106 a 108	Branco
                objRetornar.CodigoInstrucao = linha.ExtrairValorDaLinha(109, 110);

                /*
                 * 00 - Baixa por Devolução 
                 * 02 - Entrada de Título; 
                 * 05 - Liquidação S/Registro; 
                 * 06 - Liquidação Normal C/Registro; 
                 * 09 - Liquidação Rejeitada; 
                 * 15 - Liquidação Regularizada S/Registro e 
                 * 16 - Liquidação Regularizada C/Registro.
                 */
                switch (objRetornar.CodigoInstrucao)
                {
                    case "00":
                        objRetornar.DescricaoInstrucao = "Baixa por Devolução";
                        break;
                    case "02":
                        objRetornar.DescricaoInstrucao = "Entrada de Título";
                        break;
                    case "05":
                        objRetornar.DescricaoInstrucao = "Liquidação S/Registro";
                        break;
                    case "06":
                        objRetornar.DescricaoInstrucao = "Liquidação Normal C/Registro";
                        break;
                    case "09":
                        objRetornar.DescricaoInstrucao = "Liquidação Rejeitada";
                        break;
                    case "15":
                        objRetornar.DescricaoInstrucao = "Liquidação Regularizada S/Registro";
                        break;
                    case "16":
                        objRetornar.DescricaoInstrucao = "Liquidação Regularizada C/Registro";
                        break;
                    default:
                        objRetornar.DescricaoInstrucao = "Não foi localizado o código: " + objRetornar.CodigoInstrucao;
                        break;
                }

                objRetornar.DataDaOcorrencia =
                    Convert.ToDateTime(linha.ExtrairValorDaLinha(111, 118).ToDateTimeFromDdMmAaaa());
                //119 a 128	Ignorar Esse Campo
                objRetornar.NumeroDocumento = linha.ExtrairValorDaLinha(129, 140);
                objRetornar.CodigoDoRateio = linha.ExtrairValorDaLinha(141, 144);
                //145 a 148	Branco
                objRetornar.DataDeVencimento =
                    Convert.ToDateTime(linha.ExtrairValorDaLinha(149, 156).ToDateTimeFromDdMmAaaa());
                objRetornar.ValorDoTituloParcela = linha.ExtrairValorDaLinha(157, 169).BoletoBrToDecimal()/100;
                objRetornar.BancoCobrador = linha.ExtrairValorDaLinha(170, 172).BoletoBrToInt();
                objRetornar.AgenciaCobradora = linha.ExtrairValorDaLinha(173, 177).BoletoBrToInt();
                objRetornar.Especie = linha.ExtrairValorDaLinha(178, 179);
                objRetornar.ValorDespesas = linha.ExtrairValorDaLinha(180, 192).BoletoBrToDecimal()/100;
                objRetornar.ValorOutrasDespesas = linha.ExtrairValorDaLinha(193, 205).BoletoBrToDecimal()/100;
                objRetornar.ValorJurosAtraso = linha.ExtrairValorDaLinha(206, 218).BoletoBrToDecimal()/100;
                objRetornar.ValorIof = linha.ExtrairValorDaLinha(219, 231).BoletoBrToDecimal()/100;
                objRetornar.ValorAbatimento = linha.ExtrairValorDaLinha(232, 244).BoletoBrToDecimal()/100;
                objRetornar.ValorDesconto = linha.ExtrairValorDaLinha(245, 257).BoletoBrToDecimal()/100;
                objRetornar.ValorLiquidoRecebido = linha.ExtrairValorDaLinha(258, 270).BoletoBrToDecimal()/100;
                //Contempla Abatimentos e Descontos
                objRetornar.ValorOutrosDebitos = linha.ExtrairValorDaLinha(271, 283).BoletoBrToDecimal()/100;
                //Contempla Juros e Multas
                objRetornar.ValorOutrosCreditos = linha.ExtrairValorDaLinha(284, 296).BoletoBrToDecimal()/100;
                //297 a 298	Branco
                //299 a 299	Branco
                objRetornar.DataLiquidacao =
                    Convert.ToDateTime(linha.ExtrairValorDaLinha(300, 307).ToDateTimeFromDdMmAaaa());
                //308 a 364	Branco
                objRetornar.MotivoCodigoRejeicao = linha.ExtrairValorDaLinha(365, 394);
                objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt();
            }

            return objRetornar;
        }

        public TrailerRetornoCnab400 ObterTrailer(string linha)
        {
            var objRetornar = new TrailerRetornoCnab400
            {
                CodigoDoRegistro = linha.ExtrairValorDaLinha(1, 1).BoletoBrToInt(),
                CodigoDeRetorno = linha.ExtrairValorDaLinha(2, 2).BoletoBrToInt(),
                TipoRegistro = linha.ExtrairValorDaLinha(3, 4),
                CodigoDoBanco = linha.ExtrairValorDaLinha(5, 7),
                QtdRegistrosConfirmacaoEntrada = linha.ExtrairValorDaLinha(58, 62).BoletoBrToInt(),
                ValorRegistrosConfirmacaoEntrada = linha.ExtrairValorDaLinha(63, 74).BoletoBrToDecimal()/100,
                QtdRegistrosLiquidacao = linha.ExtrairValorDaLinha(87, 91).BoletoBrToInt(),
                ValorTotalCobranca = linha.ExtrairValorDaLinha(92, 103).BoletoBrToDecimal()/100,
                NumeroSequencial = linha.ExtrairValorDaLinha(395, 400).BoletoBrToInt()
            };

            return objRetornar;
        }
    }
}
