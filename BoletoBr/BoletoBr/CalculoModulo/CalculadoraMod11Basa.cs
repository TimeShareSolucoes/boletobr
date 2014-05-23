using System;

namespace BoletoBr.CalculoModulo
{
    public class CalculadoraMod11Basa : ICalculadoraModulo11
    {
        private readonly int _baseCalculo;

        public CalculadoraMod11Basa(int baseCalculo)
        {
            _baseCalculo = baseCalculo;
        }

        public string Calcular(string valor)
        {
            int Digito, Soma = 0, Peso = 2;
            for (int i = valor.Length; i > 0; i--)
            {
                Soma = Soma + (Convert.ToInt32(Common.Mid(valor, i, 1)) * Peso);
                if (Peso == _baseCalculo)
                    Peso = 2;
                else
                    Peso = Peso + 1;
            }
            if (((Soma % 11) == 0) || ((Soma % 11) == 10) || ((Soma % 11) == 1))
            {
                Digito = 1;
            }
            else
            {
                Digito = 11 - (Soma % 11);
            }
            return Digito.ToString();
        }
    }
}
