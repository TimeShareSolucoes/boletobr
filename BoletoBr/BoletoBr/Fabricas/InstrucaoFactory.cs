using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Bancos;
using BoletoBr.Dominio.Instrucao;

namespace BoletoBr.Fabricas
{
    public class InstrucaoFactory
    {
        public static IInstrucao ObterInstrucaoPadronizada(IBanco banco, int codigoInstrucao, int qtdDias)
        {
            /* HSBC */
            if (banco.CodigoBanco == "399")
            {
                var bancoHsbc = BancoFactory.ObterBanco("399", "9");
                switch (codigoInstrucao)
                {
                    case 9:
                        const string textoInstrucaoProtestoHsbc = "Protestar após 5 dias úteis.";
                        return new InstrucaoPadronizada(bancoHsbc, codigoInstrucao, qtdDias, textoInstrucaoProtestoHsbc);
                        break;
                    case 10:
                        const string textoInstrucaoNaoProtestarHsbc = "Não protestar.";
                        return new InstrucaoPadronizada(bancoHsbc, codigoInstrucao, qtdDias, textoInstrucaoNaoProtestarHsbc);
                        break;
                }
            }

            throw new Exception(
                String.Format(
                    "Não foi possível obter instrução padronizada. Banco: {0} Código Instrução: {1} Qtd Dias: {2}",
                    banco.CodigoBanco, codigoInstrucao, qtdDias));
        }

        public static IInstrucao ObterInstrucaoCustomizada(IBanco banco, string descricaoInstrucao)
        {
            var objRetornar = new InstrucaoCustomizada(banco, descricaoInstrucao);

            return objRetornar;
        }
    }
}
