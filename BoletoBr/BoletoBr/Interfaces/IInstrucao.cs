namespace BoletoBr.Interfaces
{
    public interface IInstrucao
    {
        int Codigo { get; set; }
        int QtdDias { get; set; }
        double Valor { get; set; }
        string TextoInstrucao { get; }
    }
}
