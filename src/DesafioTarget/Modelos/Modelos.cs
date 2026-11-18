namespace DesafioTarget;

public record Venda(string Vendedor, decimal Valor);
public record DadosVendas(List<Venda> Vendas);
public record Produto(int CodigoProduto, string DescricaoProduto, int Estoque);
public record DadosEstoque(List<Produto> Estoque);
public enum TipoMovimentacao { Entrada = 1, Saida = 2 }
public record Movimentacao(Guid Id, int CodigoProduto, TipoMovimentacao Tipo,
    string Descricao, int Quantidade, int SaldoFinal);

