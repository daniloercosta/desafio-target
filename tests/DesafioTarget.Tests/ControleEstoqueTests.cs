using DesafioTarget;

namespace DesafioTarget.Tests;

internal static class ControleEstoqueTests
{
    public static void Executar(Verificacao verificacao)
    {
        var estoque = new ControleEstoque([new Produto(101, "Caneta Azul", 150)]);
        var entrada = estoque.Movimentar(101, TipoMovimentacao.Entrada, 10, "Compra");
        verificacao.Igual(160, entrada.SaldoFinal);
        var saida = estoque.Movimentar(101, TipoMovimentacao.Saida, 160, "Venda");
        verificacao.Igual(0, saida.SaldoFinal);
        verificacao.Igual(false, entrada.Id == saida.Id);
        verificacao.Rejeita(() => estoque.Movimentar(101, TipoMovimentacao.Saida, 1, "Venda"));
        verificacao.Rejeita(() => estoque.Movimentar(999, TipoMovimentacao.Entrada, 1, "Compra"));
        verificacao.Rejeita(() => estoque.Movimentar(101, TipoMovimentacao.Entrada, 0, "Compra"));
        verificacao.Rejeita(() => estoque.Movimentar(101, TipoMovimentacao.Entrada, -1, "Compra"));
        verificacao.Rejeita(() => estoque.Movimentar(101, (TipoMovimentacao)3, 1, "Compra"));
        verificacao.Rejeita(() => estoque.Movimentar(101, TipoMovimentacao.Entrada, 1, " "));
        verificacao.Igual(0, estoque.Produtos.Single().Estoque);
        verificacao.Igual(2, estoque.Movimentos.Count);
        verificacao.Rejeita(() => new ControleEstoque([new Produto(1, "A", -1)]));
        var limite = new ControleEstoque([new Produto(1, "A", int.MaxValue)]);
        try { limite.Movimentar(1, TipoMovimentacao.Entrada, 1, "Compra"); throw new Exception("Esperava overflow."); }
        catch (OverflowException) { verificacao.RegistrarSucesso(); }
        verificacao.Igual(int.MaxValue, limite.Produtos.Single().Estoque);
        verificacao.Igual(0, limite.Movimentos.Count);
    }
}
