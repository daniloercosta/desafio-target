using DesafioTarget;

namespace DesafioTarget.Tests;

internal static class JurosTests
{
    public static void Executar(Verificacao verificacao)
    {
        var hoje = new DateOnly(2026, 11, 17);
        verificacao.Igual(0m, CalculadoraJuros.Juros(100, hoje, hoje));
        verificacao.Igual(0m, CalculadoraJuros.Juros(100, hoje.AddDays(1), hoje));
        verificacao.Igual(7.50m, CalculadoraJuros.Juros(100, hoje.AddDays(-3), hoje));
        verificacao.Igual(5m, CalculadoraJuros.Juros(100, new DateOnly(2024, 2, 28), new DateOnly(2024, 3, 1)));
        verificacao.Rejeita(() => CalculadoraJuros.Juros(-1, hoje, hoje));
    }
}
