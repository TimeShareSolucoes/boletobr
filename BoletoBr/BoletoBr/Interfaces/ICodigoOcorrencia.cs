namespace BoletoBr.Interfaces
{
    public interface ICodigoOcorrencia
    {
        int Codigo { get; set; }
        int QtdDias { get; set; }
        double Valor { get; set; }
        string Descricao { get; }
    }
}
