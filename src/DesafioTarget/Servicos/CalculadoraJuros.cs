namespace DesafioTarget;

public static class CalculadoraJuros
{
    public static decimal Juros(decimal valor, DateOnly vencimento, DateOnly hoje)
    {
        if (valor < 0) throw new ArgumentException("O valor não pode ser negativo.");
        int dias = Math.Max(0, hoje.DayNumber - vencimento.DayNumber);
        return Math.Round(valor * 0.025m * dias, 2, MidpointRounding.AwayFromZero);
    }
}
