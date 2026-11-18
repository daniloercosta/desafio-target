using DesafioTarget.Tests;

var verificacao = new Verificacao();
ComissaoTests.Executar(verificacao);
JurosTests.Executar(verificacao);
ControleEstoqueTests.Executar(verificacao);
Console.WriteLine($"{verificacao.Executadas} verificações passaram.");
