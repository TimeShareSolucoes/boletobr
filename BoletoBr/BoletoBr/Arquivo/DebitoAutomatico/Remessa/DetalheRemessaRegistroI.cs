using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Arquivo.DebitoAutomatico.Remessa
{
    /// <summary>
    ///  Registro “I” - Incentivo de Débito Automático
    ///  Gerado pela Empresa para o Banco.
    ///  Este registro deverá ser gerado somente para os consumidores que ainda não são optantes pelo Débito Automático. De
    ///  posse destas informações, o Banco irá trabalhar para incentivar a adesão ao Débito Automático.
    ///  A geração deste registro é opcional.
    /// </summary>
    public class DetalheRemessaRegistroI
    {
        public DetalheRemessaRegistroI(string identificacaoClienteEmpresa,
                                       string cpfCnpj,
                                       string nome,
                                       string cidade,
                                       string estado)
        {


            this.IdentificacaoClienteEmpresa = identificacaoClienteEmpresa;
            this.CpfCnpj = cpfCnpj;
            this.Nome = nome;
            this.Cidade = cidade;
            this.Estado = estado;
        }

        /// <summary>
        /// "I"
        /// </summary>
        public string CodigoRegistro => "I";

        /// <summary>
        /// Esta identificação deverá :
        ///Conter um processo de validação(DV),
        ///Ser única para cada cliente da empresa,
        ///Ser a mesma em todos os débitos consecutivos de um
        ///mesmo cliente
        /// </summary>
        public string IdentificacaoClienteEmpresa { get; set; }

        /// <summary>
        /// Conterá :
        /// CNPJ, se o Tipo de Identificação for igual a “1”.
        /// Deverá obedecer ao seguinte formato
        /// NNNNNNNNFFFFDD
        /// CPF, se o Tipo de Identificação for igual a “2”. Deverá
        /// obedecer ao seguinte formato 000NNNNNNNNNDD
        /// </summary>
        public string CpfCnpj { get; set; }

        /// <summary>
        /// “1” = para CNPJ, “2” = para CPF 
        /// </summary>
        public int TipoIdentificacao
        {
            get
            {
                if (CpfCnpj.Length == 11) return 2;
                else return 1;
            }
        }

        /// <summary>
        /// Informar o Nome do Consumidor / Contribuinte, que será utilizado para identificação visual
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Cidade onde foi consumido o serviço / fato gerador do tributo
        /// </summary>
        public string Cidade { get; set; }

        /// <summary>
        /// Sigla do Estado onde foi consumido o serviço / fato gerador do tributo
        /// </summary>
        public string Estado { get; set; }

    }
}

