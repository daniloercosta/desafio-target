namespace DesafioTarget;

public static class CalculadoraComissao
{
    public static IReadOnlyDictionary<string, decimal> PorVendedor(IEnumerable<Venda> vendas)
    {
        var lista = vendas.ToList();
        if (lista.Any(v => string.IsNullOrWhiteSpace(v.Vendedor)))
            throw new ArgumentException("Venda sem identificação do vendedor.");

        return lista.GroupBy(v => v.Vendedor)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Sum(v => Comissao(v.Valor)));
    }

    public static decimal Comissao(decimal venda)
    {
        if (venda < 0) throw new ArgumentException("A venda não pode ter valor negativo.");
        decimal taxa = venda < 100 ? 0 : venda < 500 ? 0.01m : 0.05m;
        return Math.Round(venda * taxa, 2, MidpointRounding.AwayFromZero);
    }
}
