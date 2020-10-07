using System;
using System.Collections.Generic;
using System.Linq;
using BoletoBr.Arquivo.DebitoAutomatico.Retorno;
using BoletoBr.Interfaces;

namespace BoletoBr.Bancos.DebitoAutomatico
{
    public class LeitorRetornoDebitoAutomatico : ILeitorArquivoRetornoDebitoAutomatico
    {
        private readonly List<string> _linhasArquivo;

        public LeitorRetornoDebitoAutomatico(List<string> linhasArquivo)
        {
            _linhasArquivo = linhasArquivo;
        }

        public RetornoDebitoAutomatico ProcessarRetorno()
        {
            /* Validações */

            #region Validações

            ValidaArquivoRetorno();

            #endregion

            var objRetornar = new RetornoDebitoAutomatico();

            foreach (var linhaAtual in _linhasArquivo)
            {
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "A")
                {
                    objRetornar.Header = ObterHeader(linhaAtual);
                }
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "B")
                {
                    var objDetalhe = ObterDetalheRegistroB(linhaAtual);
                    objRetornar.DetalheRetornoRegistroB.Add(objDetalhe);
                }
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "H")
                {
                    var objDetalhe = ObterDetalheRegistroH(linhaAtual);
                    objRetornar.DetalheRetornoRegistroH.Add(objDetalhe);
                }
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "F")
                {
                    var objDetalhe = ObterDetalheRegistroF(linhaAtual);
                    objRetornar.DetalheRetornoRegistroF.Add(objDetalhe);
                }
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "J")
                {
                    var objDetalhe = ObterDetalheRegistroJ(linhaAtual);
                    objRetornar.DetalheRetornoRegistroJ.Add(objDetalhe);
                }
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "T")
                {
                    var objDetalhe = ObterDetalheRegistroT(linhaAtual);
                    objRetornar.DetalheRetornoRegistroT.Add(objDetalhe);
                }
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "X")
                {
                    var objDetalhe = ObterDetalheRegistroX(linhaAtual);
                    objRetornar.DetalheRetornoRegistroX.Add(objDetalhe);
                }
                if (linhaAtual.ExtrairValorDaLinha(1, 1) == "Z")
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
                _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(1, 1) == "A");

            if (qtdLinhasHeader <= 0)
                throw new Exception("Não foi encontrado HEADER do arquivo de retorno.");

            if (qtdLinhasHeader > 1)
                throw new Exception("Não é permitido mais de um HEADER no arquivo de retorno.");

            var qtdLinhasDetalhe = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(1, 1) == "B"
                                                           || wh.ExtrairValorDaLinha(1, 1) == "F"
                                                           || wh.ExtrairValorDaLinha(1, 1) == "H"
                                                           || wh.ExtrairValorDaLinha(1, 1) == "J"
                                                           || wh.ExtrairValorDaLinha(1, 1) == "T"
                                                           || wh.ExtrairValorDaLinha(1, 1) == "X");

            if (qtdLinhasDetalhe <= 0)
                throw new Exception("Não foi encontrado DETALHE do arquivo de retorno.");

            var qtdLinhasTrailer = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(1, 1) == "Z");

            if (qtdLinhasTrailer <= 0)
                throw new Exception("Não foi encontrado TRAILER do arquivo de retorno.");

            if (qtdLinhasTrailer > 1)
                throw new Exception("Não é permitido mais de um TRAILER no arquivo de retorno.");
        }

        public HeaderRetornoDebitoAutomatico ObterHeader(string linha)
        {
            var objRetornar = new HeaderRetornoDebitoAutomatico();

                objRetornar.CodigoRegistro = linha.ExtrairValorDaLinha(1, 1);
                objRetornar.CodigoRemessa = linha.ExtrairValorDaLinha(2, 2).BoletoBrToInt();
                objRetornar.CodigoConvenio = linha.ExtrairValorDaLinha(3, 22).Trim();
                objRetornar.NomeEmpresa = linha.ExtrairValorDaLinha(23, 42).Trim();
                objRetornar.CodigoBanco = linha.ExtrairValorDaLinha(43, 45).Trim();
                objRetornar.NomeBanco = linha.ExtrairValorDaLinha(46, 65).Trim();
                objRetornar.DataGeracao = Convert.ToDateTime(linha.ExtrairValorDaLinha(66, 73).ToDateTimeFromAaaaMmDd());
                objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(74, 79).BoletoBrToInt();
                objRetornar.VersaoLayout = linha.ExtrairValorDaLinha(80, 81);
                objRetornar.IdentificacaoServico = linha.ExtrairValorDaLinha(82, 98);
            

            return objRetornar;
        }

        public DetalheRetornoRegistroB ObterDetalheRegistroB(string linha)
        {
            var objRetornar = new DetalheRetornoRegistroB
            {
                CodigoRegistro = linha.ExtrairValorDaLinha(1, 1)
            };

            if (objRetornar.CodigoRegistro == "B")
            {
                objRetornar.IdentificacaoClienteEmpresa = linha.ExtrairValorDaLinha(2, 26).Trim();
                objRetornar.AgenciaDebito = linha.ExtrairValorDaLinha(27, 30).Trim();
                objRetornar.IdentificacaoClienteBanco = linha.ExtrairValorDaLinha(31, 44).Trim();
                objRetornar.Data = Convert.ToDateTime(linha.ExtrairValorDaLinha(45, 52).ToDateTimeFromAaaaMmDd());
                objRetornar.CodigoMovimento = linha.ExtrairValorDaLinha(150, 150).BoletoBrToInt();
            }

            return objRetornar;
        }

        public DetalheRetornoRegistroF ObterDetalheRegistroF(string linha)
        {
            var objRetornar = new DetalheRetornoRegistroF
            {
                CodigoRegistro = linha.ExtrairValorDaLinha(1, 1)
            };

            if (objRetornar.CodigoRegistro == "F")
            {
                objRetornar.IdentificacaoClienteEmpresa = linha.ExtrairValorDaLinha(2, 26).Trim();
                objRetornar.AgenciaDebito = linha.ExtrairValorDaLinha(27, 30).Trim();
                objRetornar.IdentificacaoClienteBanco = linha.ExtrairValorDaLinha(31, 44).Trim();
                objRetornar.DataVencimento = Convert.ToDateTime(linha.ExtrairValorDaLinha(45, 52).ToDateTimeFromAaaaMmDd());
                objRetornar.ValorDebitado = linha.ExtrairValorDaLinha(53, 67).BoletoBrToDecimal();
                objRetornar.CodRetorno = linha.ExtrairValorDaLinha(68, 69);
                objRetornar.UsoEmpresa = linha.ExtrairValorDaLinha(70, 139).Trim();
                objRetornar.CodigoMovimento = linha.ExtrairValorDaLinha(150, 150).BoletoBrToInt();
            }

            return objRetornar;
        }

        public DetalheRetornoRegistroH ObterDetalheRegistroH(string linha)
        {
            var objRetornar = new DetalheRetornoRegistroH
            {
                CodigoRegistro = linha.ExtrairValorDaLinha(1, 1)
            };

            if (objRetornar.CodigoRegistro == "H")
            {
                objRetornar.IdentificacaoClienteEmpresaAnterior = linha.ExtrairValorDaLinha(2, 26).Trim();
                objRetornar.AgenciaDebito = linha.ExtrairValorDaLinha(27, 30).Trim();
                objRetornar.IdentificacaoClienteBanco = linha.ExtrairValorDaLinha(31, 44).Trim();
                objRetornar.IdentificacaoClienteEmpresaAtual = linha.ExtrairValorDaLinha(45, 69).Trim();
                objRetornar.Ocorrencia = linha.ExtrairValorDaLinha(70, 127).Trim();
                objRetornar.CodigoMovimento = linha.ExtrairValorDaLinha(150, 150).BoletoBrToInt();
            }

            return objRetornar;
        }

        public DetalheRetornoRegistroJ ObterDetalheRegistroJ(string linha)
        {
            var objRetornar = new DetalheRetornoRegistroJ
            {
                CodigoRegistro = linha.ExtrairValorDaLinha(1, 1)
            };

            if (objRetornar.CodigoRegistro == "J")
            {
                objRetornar.NumeroSequencial = linha.ExtrairValorDaLinha(2, 7).BoletoBrToInt();
                objRetornar.DataGeracao = Convert.ToDateTime(linha.ExtrairValorDaLinha(8, 15).ToDateTimeFromAaaaMmDd());
                objRetornar.TotalRegistroProcessado = linha.ExtrairValorDaLinha(16, 21).BoletoBrToInt();
                objRetornar.ValorTotalProcessado = linha.ExtrairValorDaLinha(22, 38).BoletoBrToDecimal();
                objRetornar.DataProcessamento = Convert.ToDateTime(linha.ExtrairValorDaLinha(39, 46).ToDateTimeFromAaaaMmDd());
            }

            return objRetornar;
        }

        public DetalheRetornoRegistroT ObterDetalheRegistroT(string linha)
        {
            var objRetornar = new DetalheRetornoRegistroT
            {
                CodigoRegistro = linha.ExtrairValorDaLinha(1, 1)
            };

            if (objRetornar.CodigoRegistro == "T")
            {
                objRetornar.QuantidadeDebitado = linha.ExtrairValorDaLinha(2, 7).BoletoBrToInt();
                objRetornar.TotalDebitado = linha.ExtrairValorDaLinha(8, 24).BoletoBrToDecimal();
            }

            return objRetornar;
        }

        public DetalheRetornoRegistroX ObterDetalheRegistroX(string linha)
        {
            var objRetornar = new DetalheRetornoRegistroX
            {
                CodigoRegistro = linha.ExtrairValorDaLinha(1, 1)
            };

            if (objRetornar.CodigoRegistro == "X")
            {
                objRetornar.CodigoAgencia = linha.ExtrairValorDaLinha(2, 5);
                objRetornar.NomeAgencia = linha.ExtrairValorDaLinha(6, 35).Trim();
                objRetornar.EnderecoAgencia = linha.ExtrairValorDaLinha(36, 65).Trim();
                objRetornar.Numero = linha.ExtrairValorDaLinha(66, 70);
                objRetornar.CodigoCep = linha.ExtrairValorDaLinha(71, 75);
                objRetornar.SufixoCep = linha.ExtrairValorDaLinha(76, 78);
                objRetornar.NomeCidade = linha.ExtrairValorDaLinha(79, 98).Trim();
                objRetornar.SiglaEstado = linha.ExtrairValorDaLinha(99, 100);
                objRetornar.SituacaoAgencia = linha.ExtrairValorDaLinha(101, 101);

            }

            return objRetornar;
        }

        public TrailerRetornoDebitoAutomatico ObterTrailer(string linha)
        {
            var objRetornar = new TrailerRetornoDebitoAutomatico
            {
                CodigoRegistro = linha.ExtrairValorDaLinha(1, 1),
                QuantidadeRegistros = linha.ExtrairValorDaLinha(2, 7).BoletoBrToInt(),
                TotalRegistros = linha.ExtrairValorDaLinha(8, 24).BoletoBrToDecimal(),

            };

            return objRetornar;
        }
    }
}
