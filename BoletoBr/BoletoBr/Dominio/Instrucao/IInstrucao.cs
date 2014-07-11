using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;

namespace BoletoBr.Dominio.Instrucao
{
    public interface IInstrucao
    {
        /// <summary>
        /// Valida os dados referentes a instrução
        /// </summary>
        void Valida();

        IBanco Banco { get; set; }
        int Codigo { get; set; }
        string Descricao { get; set; }
        int QuantidadeDias { get; set; }
    }
}
