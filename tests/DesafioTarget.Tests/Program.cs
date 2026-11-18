using DesafioTarget;

int executados = 0;
foreach (var (venda, esperado) in new[] { (0m, 0m), (99.99m, 0m), (100m, 1m),
    (499.99m, 5m), (500m, 25m), (1200.50m, 60.03m) })
    Igual(esperado, CalculadoraComissao.Comissao(venda));
Rejeita(() => CalculadoraComissao.Comissao(-1));

var comissoes = CalculadoraComissao.PorVendedor([
    new Venda("Ana", 100), new Venda("Ana", 500), new Venda("Bruno", 99)]);
Igual(26m, comissoes["Ana"]);
Igual(0m, comissoes["Bruno"]);
Rejeita(() => CalculadoraComissao.PorVendedor([new Venda(" ", 100)]));

var hoje = new DateOnly(2026, 11, 17);
Igual(0m, CalculadoraJuros.Juros(100, hoje, hoje));
Igual(0m, CalculadoraJuros.Juros(100, hoje.AddDays(1), hoje));
Igual(7.50m, CalculadoraJuros.Juros(100, hoje.AddDays(-3), hoje));
Igual(5m, CalculadoraJuros.Juros(100, new DateOnly(2024, 2, 28), new DateOnly(2024, 3, 1)));
Rejeita(() => CalculadoraJuros.Juros(-1, hoje, hoje));

var estoque = new ControleEstoque([new Produto(101, "Caneta Azul", 150)]);
var entrada = estoque.Movimentar(101, TipoMovimentacao.Entrada, 10, "Compra");
Igual(160, entrada.SaldoFinal);
var saida = estoque.Movimentar(101, TipoMovimentacao.Saida, 160, "Venda");
Igual(0, saida.SaldoFinal);
Igual(false, entrada.Id == saida.Id);
Rejeita(() => estoque.Movimentar(101, TipoMovimentacao.Saida, 1, "Venda"));
Rejeita(() => estoque.Movimentar(999, TipoMovimentacao.Entrada, 1, "Compra"));
Rejeita(() => estoque.Movimentar(101, TipoMovimentacao.Entrada, 0, "Compra"));
Rejeita(() => estoque.Movimentar(101, TipoMovimentacao.Entrada, -1, "Compra"));
Rejeita(() => estoque.Movimentar(101, (TipoMovimentacao)3, 1, "Compra"));
Rejeita(() => estoque.Movimentar(101, TipoMovimentacao.Entrada, 1, " "));
Igual(0, estoque.Produtos.Single().Estoque);
Igual(2, estoque.Movimentos.Count);
Rejeita(() => new ControleEstoque([new Produto(1, "A", -1)]));
var limite = new ControleEstoque([new Produto(1, "A", int.MaxValue)]);
try { limite.Movimentar(1, TipoMovimentacao.Entrada, 1, "Compra"); throw new Exception("Esperava overflow."); }
catch (OverflowException) { executados++; }
Igual(int.MaxValue, limite.Produtos.Single().Estoque);
Igual(0, limite.Movimentos.Count);
Console.WriteLine($"{executados} verificações passaram.");

void Igual<T>(T esperado, T recebido)
{
    if (!EqualityComparer<T>.Default.Equals(esperado, recebido))
        throw new Exception($"Esperado: {esperado}. Recebido: {recebido}.");
    executados++;
}
void Rejeita(Action acao)
{
    try { acao(); }
    catch (ArgumentException) { executados++; return; }
    throw new Exception("A operação deveria ter sido rejeitada.");
}
