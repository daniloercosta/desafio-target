namespace DesafioTarget;

public class ControleEstoque
{
    private readonly Dictionary<int, Produto> produtos;
    private readonly List<Movimentacao> movimentos = [];
    public IEnumerable<Produto> Produtos => produtos.Values.OrderBy(p => p.CodigoProduto);
    public IReadOnlyList<Movimentacao> Movimentos => movimentos.AsReadOnly();

    public ControleEstoque(IEnumerable<Produto> estoque)
    {
        produtos = new();
        foreach (var produto in estoque)
        {
            if (produto.CodigoProduto <= 0 || produto.Estoque < 0 || string.IsNullOrWhiteSpace(produto.DescricaoProduto))
                throw new ArgumentException("Produto inválido no estoque inicial.");
            if (!produtos.TryAdd(produto.CodigoProduto, produto))
                throw new ArgumentException("Código de produto duplicado.");
        }
    }

    public Movimentacao Movimentar(int codigo, TipoMovimentacao tipo, int quantidade, string descricao)
    {
        if (!produtos.TryGetValue(codigo, out var produto)) throw new ArgumentException("Produto não encontrado.");
        if (!Enum.IsDefined(tipo)) throw new ArgumentException("Tipo de movimentação inválido.");
        if (quantidade <= 0) throw new ArgumentException("A quantidade deve ser positiva.");
        if (string.IsNullOrWhiteSpace(descricao)) throw new ArgumentException("Informe a descrição da movimentação.");
        if (tipo == TipoMovimentacao.Saida && quantidade > produto.Estoque)
            throw new ArgumentException("Estoque insuficiente.");

        int saldo = checked(produto.Estoque + (tipo == TipoMovimentacao.Entrada ? quantidade : -quantidade));
        var movimento = new Movimentacao(Guid.NewGuid(), codigo, tipo, descricao.Trim(), quantidade, saldo);
        produtos[codigo] = produto with { Estoque = saldo };
        movimentos.Add(movimento);
        return movimento;
    }
}
