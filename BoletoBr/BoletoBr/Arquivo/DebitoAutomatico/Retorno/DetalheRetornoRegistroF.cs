using System; 

namespace BoletoBr.Arquivo.DebitoAutomatico.Retorno
{
    /// <summary>
    ///  Registro “F” - Retorno do Débito Automático
    ///  Gerado pelo Banco para a Empresa.
    ///  Será gerado um registro “F”, para cada registro de débito (registro “E”), enviado anteriormente.
    /// </summary> 
    public class DetalheRetornoRegistroF
    {
        /// <summary>
        /// "F"
        /// </summary>
        public string CodigoRegistro { get; set; }

        /// <summary>
        /// O conteúdo deverá ser idêntico ao anteriormente enviado
        /// pelo Banco, no registro tipo “B”
        /// </summary>
        public string IdentificacaoClienteEmpresa { get; set; }

        /// <summary>
        /// O conteúdo deverá ser idêntico ao anteriormente enviado
        /// pelo Banco, no registro tipo “B”
        /// </summary>
        public string AgenciaDebito { get; set; }

        /// <summary>
        /// O conteúdo deverá ser idêntico ao anteriormente enviado
        /// pelo Banco, no registro tipo “B”
        /// </summary>
        public string IdentificacaoClienteBanco { get; set; }

        /// <summary>
        /// B05-Data da Opção/Exclusão
        ///Conterá :
        /// Data de Exclusão, se Código de Movimento igual a 1, Data de Inclusão, se Código de Movimento igual a 2. Formato AAAAMMDD
        /// </summary>
        public DateTime DataVencimento { get; set; }

        /// <summary>
        /// 1 = Exclusão de optante pelo débito automático
        /// 2 = Inclusão de optante pelo débito automático
        /// </summary>
        public decimal ValorDebitado { get; set; }

        /// <summary>
        /// “00” = Débito efetuado “01” = Débito não efetuado - Insuficiência de fundos “02” = Débito não efetuado - Conta corrente não cadastrada
        /// “04” = Débito não efetuado - Outras restrições
        /// “05” = Débito não efetuado – valor do débito excede valor limite aprovado. 
        /// “10” = Débito não efetuado - Agência em regime de encerramento 
        /// “12” = Débito não efetuado - Valor inválido 
        /// “13” = Débito não efetuado - Data de lançamento inválida 
        /// “14” = Débito não efetuado - Agência inválida 
        /// “15” = Débito não efetuado - conta corrente inválida 
        /// “18” = Débito não efetuado - Data do débito anterior à do processamento 
        /// “30” = Débito não efetuado - Sem contrato de débito automático
        /// “31” = Débito efetuado em data diferente da data informada – feriado na praça de débito
        /// “96” = Manutenção do Cadastro 
        /// “97” = Cancelamento - Não encontrado 
        /// “98” = Cancelamento - Não efetuado, fora do tempo hábil 
        /// “99” = Cancelamento - cancelado conforme solicitação
        /// </summary>
        public string CodRetorno { get; set; }
        public string RetornoTratado
        {
            get
            {
                switch (CodRetorno)
                {
                    case "00": return "Débito efetuado ";
                    case "01": return "Débito não efetuado - Insuficiência de fundos";
                    case "02": return "Débito não efetuado - Conta corrente não cadastrada"; 
                    case "04": return "Débito não efetuado - Outras restrições";
                    case "05": return "Débito não efetuado – valor do débito excede valor limite aprovado.";
                    case "10": return "Débito não efetuado - Agência em regime de encerramento";
                    case "12": return "Débito não efetuado - Valor inválido";
                    case "13": return "Débito não efetuado - Data de lançamento inválida";
                    case "14": return "Débito não efetuado - Agência inválida";
                    case "15": return "Débito não efetuado - conta corrente inválida";
                    case "18": return "Débito não efetuado - Data do débito anterior à do processamento";
                    case "30": return "Débito não efetuado - Sem contrato de débito automático";
                    case "31": return "Débito efetuado em data diferente da data informada – feriado na praça de débito";
                    case "96": return "Manutenção do Cadastro";
                    case "97": return "Cancelamento - Não encontrado";
                    case "98": return "Cancelamento - Não efetuado, fora do tempo hábil";
                    case "99": return "Cancelamento - cancelado conforme solicitação";
                    default:
                        return "Código inválido";
                }
            }
        }

        /// <summary>
        /// O conteúdo deverá ser idêntico ao anteriormente enviado pela Empresa, no registro tipo “E” 
        /// </summary>
        public string UsoEmpresa { get; set; }

        /// <summary>
        /// O conteúdo será ser idêntico ao anteriormente enviado pela Empresa, no registro tipo “E”
        /// </summary>
        public int CodigoMovimento { get; set; }

        /// <summary>
        /// Campo gerado para controle, não é lido pelo arquivo
        /// </summary>
        public bool Confirmado { get; set; }

        /// <summary>
        /// Campo gerado para controle, não é lido pelo arquivo
        /// </summary>
        public string Observacao { get; set; }
    }
}
