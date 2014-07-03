using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Dominio
{
    public class DetalheSegmentoERetornoCnab240
    {
        /// <summary>
        /// C�digo fornecido pelo Banco Central para identifica��o do Banco que est� recebendo ou enviando o arquivo, com o qual se firmou o contrato de presta��o de servi�os.
        /// </summary>
        public int CodigoBanco { get; private set; }

        /// <summary>
        /// N�mero seq�encial para identificar univocamente um lote de servi�o. Criado e controlado pelo respons�vel pela gera��o magn�tica dos dados contidos no arquivo.
        /// Preencher com '0001' para o primeiro lote do arquivo. Para os demais: n�mero do lote anterior acrescido de 1. O n�mero n�o poder� ser repetido dentro do arquivo.
        /// Se registro for Header do Arquivo preencher com '0000'
        /// Se registro for Trailer do Arquivo preencher com '9999'
        /// </summary>
        public int LoteServico { get; private set; }

        /// <summary>
        /// C�digo adotado pela FEBRABAN para identificar o tipo de registro.
        /// Dom�nio:
        /// '0' = Header de Arquivo
        /// '1' = Header de Lote
        /// '2' = Registros Iniciais do Lote
        /// '3' = Detalhe
        /// '4' = Registros Finais do Lote
        /// '5' = Trailer de Lote
        /// '9' = Trailer de Arquivo
        /// </summary>
        public string TipoRegistro { get; private set; }

        /// <summary>
        /// N�mero adotado e controlado pelo respons�vel pela gera��o magn�tica dos dados contidos no arquivo, para identificar a seq��ncia de registros encaminhados no lote.
        /// Deve ser inicializado sempre em '1', em cada novo lote.
        /// </summary>
        public int NumeroRegistro { get; private set; }

        /// <summary>
        /// C�digo adotado pela FEBRABAN para identificar o segmento do registro.
        /// </summary>
        public string Segmento { get; private set; }

        /// <summary>
        /// Texto de observa��es destinado para uso exclusivo da FEBRABAN.
        /// Preencher com Brancos.
        /// </summary>
        public string UsoExclusivoFebrabanCnab { get; private set; }

        /// <summary>
        /// C�digo que identifica o tipo de inscri��o da Empresa ou Pessoa F�sica perante uma Institui��o governamental.Dom�nio:
        /// '0' = Isento / N�o Informado
        /// '1' = CPF
        /// '2' = CGC / CNPJ
        /// '3' = PIS / PASEP
        /// '9' = Outros
        /// Preenchimento deste campo � obrigat�rio para DOC e TED (Forma de Lan�amento = 03, 41, 43)
        /// Para pagamento para o SIAPE com cr�dito em conta, o CPF dever� ser do 1� titular.
        /// </summary>
        //public TipoInscricao TipoInscricaoCliente { get; private set; }

        /// <summary>
        /// N�mero de inscri��o da Empresa ou Pessoa F�sica perante uma Institui��o governamental.
        /// </summary>
        public long NumeroInscricaoCliente { get; private set; }

        /// <summary>
        /// C�digo adotado pelo Banco para identificar o Contrato entre este e a Empresa Cliente.
        /// </summary>
        public string CodigoConvenioBanco { get; private set; }

        /// <summary>
        /// C�digo adotado pelo Banco respons�vel pela conta, para identificar a qual unidade est� vinculada a conta corrente.
        /// </summary>
        public int AgenciaMantenedoraConta { get; private set; }

        /// <summary>
        /// C�digo adotado pelo Banco respons�vel pela conta corrente, para verifica��o da autenticidade do C�digo da Ag�ncia.
        /// </summary>
        public string DigitoVerificadorAgencia { get; private set; }

        /// <summary>
        /// N�mero adotado pelo Banco, para identificar univocamente a conta corrente utilizada pelo Cliente.
        /// </summary>
        public long NumeroContaCorrente { get; private set; }

        /// <summary>
        /// C�digo adotado pelo respons�vel pela conta corrente, para verifica��o da autenticidade do N�mero da Conta Corrente.
        /// Para os Bancos que se utilizam de duas posi��es para o D�gito Verificador do N�mero da Conta Corrente, preencher este campo com a 1� posi��o deste d�gito.
        /// Exemplo :
        /// N�mero C/C = 45981-36
        /// Neste caso -> D�gito Verificador da Conta = 3
        /// </summary>
        public string DigitoVerificadorConta { get; private set; }

        /// <summary>
        /// C�digo adotado pelo Banco respons�vel pela conta corrente, para verifica��o da autenticidade do par C�digo da Ag�ncia / N�mero da Conta Corrente.
        /// Para os Bancos que se utilizam de duas posi��es para o D�gito Verificador do N�mero da Conta Corrente, preencher este campo com a 2� posi��o deste d�gito.
        /// Exemplo :
        /// N�mero C/C = 45981-36
        /// Neste caso -> D�gito Verificador da Ag/Conta = 6
        /// </summary>
        public string DigitoVerificadorAgenciaConta { get; private set; }

        /// <summary>
        /// Nome que identifica a pessoa, f�sica ou jur�dica, a qual se quer fazer refer�ncia.
        /// </summary>
        public string NomeEmpresa { get; private set; }

        /// <summary>
        /// Texto de observa��es destinado para uso exclusivo da FEBRABAN.
        /// Preencher com Brancos.
        /// </summary>
        public string UsoExclusivoFebrabanCnab2 { get; private set; }

        /// <summary>
        /// Identifica se o Lan�amento incide sobre valores dispon�veis ou bloqueados, possibilitando a recomposi��o das posi��es dos saldos.
        /// Dom�nio:
        /// 'DPV' = TIPO DISPON�VEL
        /// Lan�amento ocorrido em Saldo Dispon�vel
        /// 'SCR' = TIPO VINCULADO
        /// Lan�amento ocorrido em Saldo Dispon�vel ou Vinculado (a crit�rio de cada banco), por�m pendente de libera��o por regras internas do banco
        /// 'SSR' = TIPO BLOQUEADO
        /// Lan�amento ocorrido em Saldo Bloqueado
        /// 'CDS' = COMPOSI��O DE DIVERSOS SALDOS
        /// Lan�amento ocorrido em diversos saldos
        /// A condi��o de recurso Dispon�vel, Vinculado ou Bloqueado para os c�digos, SCR, SSR e CDS � crit�rio de cada banco.
        /// </summary>
        public string NaturezaLancamento { get; private set; }

        /// <summary>
        /// C�digo adotado pela FEBRABAN para identificar a padroniza��o a ser utilizada no complemento.
        /// Dom�nio:
        /// '00' = Sem Informa��o do Complemento do Lan�amento
        /// '01' = Identifica��o da Origem do Lan�amento
        /// </summary>
        //public TipoComplementoLancamento TipoComplementoLancamento { get; private set; }

        /// <summary>
        /// Texto de informa��es complementares ao Lan�amento.
        /// Para Tipo do Complemento = 01, o campo complemento ter� o seguinte formato:
        /// Banco Origem Lan�amento 114 116 3 Num
        /// Ag�ncia Origem Lan�amento 117 121 5 Num
        /// Uso Exclusivo FEBRABAN/ CNAB 122 133 12 Alfa preencher com brancos
        /// </summary>
        public string ComplementoLancamento { get; private set; }

        /// <summary>
        /// C�digo adotado pela FEBRABAN para identifica��o de Lan�amentos desobrigados de recolhimento do CPMF.
        /// Dom�nio:
        /// 'S' = Isento
        /// 'N' = N�o Isento
        /// </summary>
        //public IsencaoCpmf IdentificacaoIsencaoCpmf { get; private set; }

        /// <summary>
        /// Data de efetiva��o do Lan�amento.
        /// Utilizar o formato DDMMAAAA, onde:
        /// DD = dia
        /// MM = m�s
        /// AAAA = ano
        /// </summary>
        public DateTime? DataContabil { get; private set; }

        /// <summary>
        /// Data de ocorr�ncia dos fatos, itens, componentes do extrato banc�rio.
        /// Utilizar o formato DDMMAAAA, onde:
        /// DD = dia
        /// MM = m�s
        /// AAAA = ano
        /// </summary>
        public DateTime DataLancamento { get; private set; }

        /// <summary>
        /// Valor do Lan�amento efetuado, expresso em moeda corrente.
        /// </summary>
        public decimal ValorLancamento { get; private set; }

        /// <summary>
        /// C�digo adotado pela FEBRABAN para caracterizar o item que est� sendo representado no extrato banc�rio.
        /// Dom�nio:
        /// 'D' = D�bito
        /// 'C' = Cr�dito
        /// </summary>
        //public TipoLancamento TipoLancamento { get; private set; }

        /// <summary>
        /// C�digo adotado pela FEBRABAN, para identificar a categoria padr�o do Lan�amento, para concilia��o entre Bancos.
        /// Dom�nio:
        /// D�bitos:
        /// '101' = Cheques
        /// '102' = Encargos
        /// '103' = Estornos
        /// '104' = Lan�amento Avisado
        /// '105' = Tarifas
        /// '106' = Aplica��o
        /// '107' = Empr�stimo / Financiamento
        /// '108' = C�mbio
        /// '109' = CPMF
        /// '110' = IOF
        /// '111' = Imposto de Renda
        /// '112' = Pagamento Fornecedores
        /// '113' = Pagamentos Sal�rio
        /// '114' = Saque Eletr�nico
        /// '115' = A��es
        /// '117' = Transfer�ncia entre Contas
        /// '118' = Devolu��o da Compensa��o
        /// '119' = Devolu��o de Cheque Depositado
        /// '120' = Transfer�ncia Interbanc�ria (DOC, TED)
        /// '121' = Antecipa��o a Fornecedores
        /// '122' = OC / AEROPS
        /// Cr�ditos:
        /// '201' = Dep�sitos
        /// '202' = L�quido de Cobran�a
        /// '203' = Devolu��o de Cheques
        /// '204' = Estornos
        /// '205' = Lan�amento Avisado
        /// '206' = Resgate de Aplica��o
        /// '207' = Empr�stimo / Financiamento
        /// '208' = C�mbio
        /// '209' = Transfer�ncia Interbanc�ria (DOC, TED)
        /// '210' = A��es
        /// '211' = Dividendos
        /// '212' = Seguro
        /// '213' = Transfer�ncia entre Contas
        /// '214' = Dep�sitos Especiais
        /// '215' = Devolu��o da Compensa��o
        /// '216' = OCT
        /// '217' = Pagamentos Fornecedores
        /// '218' = Pagamentos Diversos
        /// '219' = Pagamentos Sal�rios
        /// </summary>
        //public CategoriaLancamento CategoriaLancamento { get; private set; }

        /// <summary>
        /// C�digo adotado por cada Banco para identificar o descritivo do Lan�amento. Observar que no Extrato de Conta Corrente para Concilia��o Banc�ria este campo possui 4 caracteres, enquanto no Extrato para Gest�o de Caixa ele possui 5 caracteres.
        /// </summary>
        public string CodigoHistorico { get; private set; }

        /// <summary>
        /// Texto descritivo do hist�rico do Lan�amento do extrato banc�rio.
        /// </summary>
        public string HistoricoLancamento { get; private set; }

        /// <summary>
        /// N�mero que identifica o documento que gerou o Lan�amento. Para uso na concilia��o autom�tica de Conta Corrente, o n�mero do documento n�o pode ser maior que 6 posi��es num�ricas. O complemento est� limitado de acordo com as restri��es de cada banco.
        /// </summary>
        public string NumeroDocumentoComplemento { get; private set; }

        public void LerDetalheSegmentoERetornoCNAB240(string registro)
        {
            try
            {
                if (registro.Substring(13, 1) != "E")
                    throw new Exception("Registro inv�lido. O detalhe n�o possu� as caracter�sticas do segmento E.");

                //CodigoBanco = LeitorLinhaPosicao.ExtrairInt32DaPosicao(registro, 1, 3);
                //LoteServico = LeitorLinhaPosicao.ExtrairInt32DaPosicao(registro, 4, 7);
                //TipoRegistro = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 8, 8);
                //NumeroRegistro = LeitorLinhaPosicao.ExtrairInt32DaPosicao(registro, 9, 13);
                //Segmento = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 14, 14);
                //UsoExclusivoFebrabanCnab = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 15, 17);
                //TipoInscricaoCliente = (TipoInscricao)LeitorLinhaPosicao.ExtrairInt32DaPosicao(registro, 18, 18);
                //NumeroInscricaoCliente = LeitorLinhaPosicao.ExtrairInt64DaPosicao(registro, 19, 32);
                //CodigoConvenioBanco = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 33, 52);
                //AgenciaMantenedoraConta = LeitorLinhaPosicao.ExtrairInt32DaPosicao(registro, 53, 57);
                //DigitoVerificadorAgencia = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 58, 58);
                //NumeroContaCorrente = LeitorLinhaPosicao.ExtrairInt64DaPosicao(registro, 59, 70);
                //DigitoVerificadorConta = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 71, 71);
                //DigitoVerificadorAgenciaConta = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 72, 72);
                //NomeEmpresa = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 73, 102);
                //UsoExclusivoFebrabanCnab2 = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 103, 108);
                //NaturezaLancamento = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 109, 111);
                //TipoComplementoLancamento = (TipoComplementoLancamento)LeitorLinhaPosicao.ExtrairInt32OpcionalDaPosicao(registro, 112, 113).GetValueOrDefault();
                //ComplementoLancamento = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 114, 133);
                //IdentificacaoIsencaoCpmf = (IsencaoCpmf)LeitorLinhaPosicao.ExtrairDaPosicao(registro, 134, 134)[0];
                //DataContabil = LeitorLinhaPosicao.ExtrairDataOpcionalDaPosicao(registro, 135, 142);
                //DataLancamento = LeitorLinhaPosicao.ExtrairDataDaPosicao(registro, 143, 150);
                //ValorLancamento = decimal.Parse(LeitorLinhaPosicao.ExtrairDaPosicao(registro, 151, 168)) / 100m;
                //TipoLancamento = (TipoLancamento)LeitorLinhaPosicao.ExtrairDaPosicao(registro, 169, 169)[0];
                //CategoriaLancamento = (CategoriaLancamento)LeitorLinhaPosicao.ExtrairInt32DaPosicao(registro, 170, 172);
                //CodigoHistorico = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 173, 176);
                //HistoricoLancamento = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 177, 201);
                //NumeroDocumentoComplemento = LeitorLinhaPosicao.ExtrairDaPosicao(registro, 202, 240);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO E.", ex);
            }
        }
    }
}

