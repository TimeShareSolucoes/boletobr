using System;

namespace BoletoBr.CalculoModulo
{
    class CalculadoraMod10Basa : ICalculadoraModulo10
    {
        public string Calcular(string valor)
        {
            int Digito, Soma = 0, Peso = 2, m1;
            string m2;
            for (int i = valor.Length; i > 0; i--)
            {
                m1 = (Convert.ToInt32(Common.Mid(valor, i, 1)) * Peso);
                m2 = m1.ToString();

                for (int j = 1; j <= m2.Length; j++)
                {
                    Soma += Convert.ToInt32(Common.Mid(m2, j, 1));
                }

                if (Peso == 2)
                    Peso = 1;
                else
                    Peso = Peso + 1;
            }
            Digito = ((10 - (Soma % 10)) % 10);
            
            return Digito.ToString();
        }
    }
}
