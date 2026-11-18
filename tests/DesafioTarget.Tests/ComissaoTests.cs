using DesafioTarget;

namespace DesafioTarget.Tests;

internal static class ComissaoTests
{
    public static void Executar(Verificacao verificacao)
    {
        foreach (var (venda, esperado) in new[] { (0m, 0m), (99.99m, 0m), (100m, 1m),
            (499.99m, 5m), (500m, 25m), (1200.50m, 60.03m) })
            verificacao.Igual(esperado, CalculadoraComissao.Comissao(venda));
        verificacao.Rejeita(() => CalculadoraComissao.Comissao(-1));

        var comissoes = CalculadoraComissao.PorVendedor([
            new Venda("Ana", 100), new Venda("Ana", 500), new Venda("Bruno", 99)]);
        verificacao.Igual(26m, comissoes["Ana"]);
        verificacao.Igual(0m, comissoes["Bruno"]);
        verificacao.Rejeita(() => CalculadoraComissao.PorVendedor([new Venda(" ", 100)]));
    }
}
