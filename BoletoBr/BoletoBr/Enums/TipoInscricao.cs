using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoBr.Enums
{
    public enum TipoInscricao
    {
        IsentoOuNaoInformado = 0,
        Cpf = 1,
        CgcOuCnpj = 2,
        PisOuPasep = 3,
        Outros = 9
    }
}
