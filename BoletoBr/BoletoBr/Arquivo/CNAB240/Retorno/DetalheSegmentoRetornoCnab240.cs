using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Dominio;

namespace BoletoBr.Arquivo.CNAB240.Retorno
{
    public class DetalheSegmentoRetornoCnab240
    {
        public DetalheSegmentoRetornoCnab240()
        {
            this.RegistrosDetalheSegmentoT = new DetalheSegmentoTRetornoCnab240();
            this.RegistrosDetalheSegmentoU = new DetalheSegmentoURetornoCnab240();
        }
        public DetalheSegmentoTRetornoCnab240 RegistrosDetalheSegmentoT { get; set; }
        public DetalheSegmentoURetornoCnab240 RegistrosDetalheSegmentoU { get; set; }
    }
}
