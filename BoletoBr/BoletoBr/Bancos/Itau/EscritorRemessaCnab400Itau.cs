using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BoletoBr.Arquivo.CNAB400.Retorno;
using BoletoBr.Dominio;
using BoletoBr.Dominio.Instrucao;

namespace BoletoBr.Bancos.Itau
{
    public class EscritorRemessaCnab400Itau : IEscritorArquivoRemessa
    {
        public List<string> EscreverArquivo(List<Boleto> boletosEscrever)
        {
            throw new NotImplementedException();
        }

        public void ValidarArquivoRemessa() { }

        public string EscreverHeader(Boleto boleto, int numeroRegistro)
        {
            string header = string.Empty;
            try
            {
                header += "0";
                header += "1";
                header += "REMESSA";
                header += "01";
                header += "COBRANCA".PadRight(15, ' ');
                header += boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0');
                header += "00";
                header += boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(5, '0');
                header += boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta;
                header += string.Empty.PadRight(8, ' ');
                header += boleto.CedenteBoleto.Nome.PadRight(30, ' ');
                header += "341";
                header += "BANCO ITAU SA";
                header += DateTime.Now.ToString().ToDateTimeFromDdMmAa();
                header += string.Empty.PadRight(294, ' ');
                header += "000001";

                return header;
            }
            catch (Exception e)
            {
                throw new Exception(String.Concat("Falha na geração do HEADER do arquivo de REMESSA.", Environment.NewLine, e));
            }
        }

        public string EscreverDetalhe(Boleto boleto, int numeroRegistro)
        {
            string carteiraCob = boleto.CarteiraCobranca.Codigo.PadLeft(3, ' ');
            string instrucao1 = string.Empty;
            string instrucao2 = string.Empty;
            string enderecoSacado = boleto.SacadoBoleto.EnderecoSacado.TipoLogradouro + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Logradouro + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Numero + " " +
                                    boleto.SacadoBoleto.EnderecoSacado.Complemento.PadRight(40, ' ');

            string detalhe = string.Empty;
            try
            {
                detalhe += "1"; // Identificação do Registro Transação
                detalhe += boleto.CedenteBoleto.CpfCnpj.Length == 11 ? "01" : "02"; // Tipo de Inscrição da Empresa
                detalhe += boleto.CedenteBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", ""); // Nro de Inscrição da Empresa (CPF/CNPJ)
                detalhe += boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'); // Agência Mantenedora da Conta
                detalhe += string.Empty.PadRight(2, '0'); // Complemento de Registro
                detalhe += boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(5, '0'); // Nro da Conta Corrente da Empresa
                detalhe += boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta; // Dígito de Auto Conferência Ag/Conta Empresa
                detalhe += string.Empty.PadRight(4, ' '); // Complemento de Registro
                //detalhe += boleto.InstrucoesDoBoleto; // Cód. Instrução/Alegação a ser cancelada
                detalhe += boleto.NumeroDocumento.PadLeft(25, ' '); // Identificação do Título na Empresa
                detalhe += boleto.NossoNumeroFormatado.PadLeft(8, ' '); // Identificação do Título no Banco
                if (boleto.BancoBoleto.MoedaBanco == "9")
                    String.Format("{0:0.#####}", boleto.QuantidadeMoeda);
                else
                    boleto.QuantidadeMoeda = 0;
                detalhe += boleto.QuantidadeMoeda; // Quantidade de Moeda Variável
                detalhe += boleto.CarteiraCobranca.Codigo.PadLeft(3, ' '); // Número da Carteira no Banco
                detalhe += string.Empty.PadRight(21, ' '); // Identificação da Operação no Banco
                /* Código da Carteira */
                // Modalidade de Carteira D - Direta
                if (carteiraCob == "108")
                    detalhe += "D";
                // Modalidade de Carteira S - Sem Registro
                if (carteiraCob == "103" || carteiraCob == "173" || carteiraCob == "196")
                    detalhe += "S";
                // Modalidade de Carteira E - Escritural
                if (carteiraCob == "104" || carteiraCob == "112" || carteiraCob == "138" || carteiraCob == "147")
                    detalhe += "E";
                //detalhe += boleto.CodigoOcorrencia; // Identificação da Ocorrência
                detalhe += boleto.NumeroDocumento.PadLeft(10, ' '); // Nro do Documento de Cobrança
                detalhe += boleto.DataVencimento.ToString().ToDateTimeFromDdMmAa(); // Data de Vencimento do Título
                detalhe += String.Format("{0:0.##}", boleto.ValorBoleto).PadLeft(11, '0'); // Valor Nominal do Título
                detalhe += boleto.BancoBoleto.CodigoBanco.PadLeft(3, '0'); // Nro do Banco na Câmara de Compensação
                detalhe += string.Empty.PadLeft(5, '0'); // Agência onde o título será cobrado
                detalhe += boleto.Especie; // Espécie do Título
                detalhe += boleto.Aceite; // Identificação de Título Aceitou ou Não Aceito
                detalhe += DateTime.Now.ToString().ToDateTimeFromDdMmAa(); // Data da Emissão do Título

                switch (boleto.InstrucoesDoBoleto.Count)
                {
                    case 1:
                        instrucao1 = boleto.InstrucoesDoBoleto[0].Codigo.ToString();
                        instrucao2 = "0";
                        break;
                    case 2:
                        instrucao1 = boleto.InstrucoesDoBoleto[0].Codigo.ToString();
                        instrucao2 = boleto.InstrucoesDoBoleto[1].Codigo.ToString();                        
                        break;
                }

                detalhe += instrucao1; // Instrução de Cobrança 1
                detalhe += instrucao2; // Instrução de Cobrança 2
                detalhe += String.Format("{0:0.##}", boleto.JurosMora).PadLeft(11, '0'); // Valor de Mora Por Dia de Atraso
                detalhe += boleto.DataDesconto.ToString().ToDateTimeFromDdMmAa(); // Data Limite para Concesão de Desconto
                detalhe += String.Format("{0:0.##}", boleto.ValorDesconto).PadLeft(11, '0'); // Valor do Desconto a ser Concedido
                detalhe += String.Format("{0:0.##}", boleto.Iof).PadLeft(11, '0'); // Valor do I.O.F. recolhido p/ notas seguro
                detalhe += String.Format("{0:0.##}", boleto.ValorAbatimento).PadLeft(11, '0'); // Valor do Abatimento a ser concedido
                detalhe += boleto.SacadoBoleto.CpfCnpj.Length == 11 ? "01" : "02"; // Identificação do tipo de inscrição/sacado
                detalhe += boleto.SacadoBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", ""); // Nro de Inscrição do Sacado (CPF/CNPJ)
                detalhe += boleto.SacadoBoleto.Nome.PadRight(30, ' '); // Nome do Sacado
                detalhe += string.Empty.PadRight(10, '0'); // Complemento de Registro
                detalhe += enderecoSacado; // Rua, Número, e Complemento do Sacado
                detalhe += boleto.SacadoBoleto.EnderecoSacado.Bairro.PadRight(12, ' '); // Bairro do Sacado
                detalhe += boleto.SacadoBoleto.EnderecoSacado.Cep.PadLeft(8, ' '); // Cep do Sacado
                detalhe += boleto.SacadoBoleto.EnderecoSacado.Cidade.PadRight(15, ' '); // Cidado do Sacado
                detalhe += boleto.SacadoBoleto.EnderecoSacado.SiglaUf.PadRight(2, ' '); // UF do Sacado
                detalhe += boleto.SacadoBoleto.NomeAvalista.PadRight(30, ' '); // Nome do Sacador ou Avalista
                detalhe += string.Empty.PadRight(4, '0'); // Complemento do Registro
                detalhe += boleto.DataJurosMora.ToString().ToDateTimeFromDdMmAa(); // Data de Mora
                //detalhe += boleto.QtdDias; // Quantidade de Dias
                detalhe += string.Empty.PadRight(1, '0'); // Complemento do Registro
                detalhe += numeroRegistro; // Nro Sequencial do Registro no Arquivo

                return detalhe;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public string EscreverTrailer(int numeroRegistro)
        {
            string trailer = string.Empty;
            try
            {
                trailer += "9";
                trailer += string.Empty.PadRight(393, ' ');
                // Contagem total de linhas do arquivo no formato '000000' - 6 dígitos
                trailer += numeroRegistro;

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception(String.Concat("Falha na geração do TRAILER do arquivo de REMESSA.", Environment.NewLine, e));
            }
        }
    }
}
