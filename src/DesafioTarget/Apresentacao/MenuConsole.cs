using System.Globalization;

namespace DesafioTarget;

internal class MenuConsole
{
    public void Executar()
    {
        var cultura = CultureInfo.GetCultureInfo("pt-BR");
        try
        {
            var vendas = LeitorJson.Ler<DadosVendas>("vendas.json").Vendas
                ?? throw new ArgumentException("Lista de vendas ausente.");
            var estoque = new ControleEstoque(LeitorJson.Ler<DadosEstoque>("estoque.json").Estoque
                ?? throw new ArgumentException("Lista de estoque ausente."));

            while (true)
            {
                Console.WriteLine("\n1 - Comissões\n2 - Movimentar estoque\n3 - Consultar estoque\n4 - Histórico\n5 - Juros\n0 - Sair");
                var opcao = Perguntar("Opção: ");
                if (opcao is null or "0") break;
                try
                {
                    switch (opcao)
                    {
                        case "1":
                            foreach (var comissao in CalculadoraComissao.PorVendedor(vendas))
                                Console.WriteLine($"{comissao.Key}: {Moeda(comissao.Value)}");
                            break;
                        case "2":
                            ListarEstoque();
                            int codigo = LerInteiro("Código do produto: ");
                            var tipo = (TipoMovimentacao)LerInteiro("1 - Entrada / 2 - Saída: ");
                            int quantidade = LerInteiro("Quantidade: ");
                            string descricao = Perguntar("Descrição: ") ?? throw new EndOfStreamException();
                            var movimento = estoque.Movimentar(codigo, tipo, quantidade, descricao);
                            Console.WriteLine($"Movimentação: {movimento.Id}\nEstoque final: {movimento.SaldoFinal}");
                            break;
                        case "3": ListarEstoque(); break;
                        case "4":
                            if (estoque.Movimentos.Count == 0) Console.WriteLine("Nenhuma movimentação registrada.");
                            foreach (var m in estoque.Movimentos)
                                Console.WriteLine($"{m.Id} | Produto {m.CodigoProduto} | {m.Tipo} | {m.Descricao} | Quantidade {m.Quantidade} | Saldo {m.SaldoFinal}");
                            break;
                        case "5":
                            if (!decimal.TryParse(Perguntar("Valor (ex.: 1000,50, sem separador de milhar): "),
                                NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, cultura, out decimal valor))
                                throw new ArgumentException("Valor inválido. Use vírgula nos centavos.");
                            if (!DateOnly.TryParseExact(Perguntar("Vencimento (dd/MM/aaaa): "), "dd/MM/yyyy", cultura,
                                DateTimeStyles.None, out var vencimento)) throw new ArgumentException("Data inválida.");
                            var hoje = DateOnly.FromDateTime(DateTime.Today);
                            decimal juros = CalculadoraJuros.Juros(valor, vencimento, hoje);
                            Console.WriteLine($"Data de cálculo: {hoje:dd/MM/yyyy}\nJuros: {Moeda(juros)}\nTotal: {Moeda(valor + juros)}");
                            break;
                        default: Console.WriteLine("Opção inválida."); break;
                    }
                }
                catch (ArgumentException ex) { Console.WriteLine(ex.Message); }
                catch (OverflowException) { Console.WriteLine("Valor acima do limite suportado."); }
            }

            void ListarEstoque()
            {
                foreach (var p in estoque.Produtos) Console.WriteLine($"{p.CodigoProduto} | {p.DescricaoProduto} | Estoque: {p.Estoque}");
            }
            string Moeda(decimal valor) => valor.ToString("C2", cultura);
        }
        catch (EndOfStreamException) { Console.WriteLine("Entrada encerrada."); }
        catch (Exception ex) when (ex is IOException or System.Text.Json.JsonException or ArgumentException)
        {
            Console.Error.WriteLine($"Erro: {ex.Message}");
            Environment.ExitCode = 1;
        }
    }

    static string? Perguntar(string mensagem)
    {
        Console.Write(mensagem);
        return Console.ReadLine();
    }
    static int LerInteiro(string mensagem) => int.TryParse(Perguntar(mensagem), out int valor)
        ? valor : throw new ArgumentException("Informe um número inteiro válido.");
}
