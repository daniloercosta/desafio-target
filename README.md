# Desafio Target

Aplicação de console em C# para os três exercícios do desafio: comissão de vendas, movimentação de estoque e cálculo de juros.

## Como executar

É necessário ter o SDK do .NET 8 ou superior instalado.

Na pasta da solução:

```sh
dotnet run --project src/DesafioTarget
```

O menu permite calcular as comissões, movimentar e consultar o estoque, consultar o histórico e calcular os juros. Os dados fornecidos no enunciado estão em `src/DesafioTarget/Dados` e são copiados para a pasta de saída no build.

Para conferir as regras:

```sh
dotnet run --project tests/DesafioTarget.Tests
```

Os testes são um executável sem pacotes externos. Uma falha encerra a execução com erro. São verificados os limites das faixas de comissão, arredondamento, datas, entradas e saídas, validações e preservação do saldo após operações rejeitadas.

## Decisões

- A comissão é calculada por venda e depois somada por vendedor. Valores abaixo de R$ 100 não geram comissão; de R$ 100 até menos de R$ 500 geram 1%; a partir de R$ 500 geram 5%.
- Valores monetários usam `decimal`. Cada comissão e o valor final dos juros são arredondados para duas casas, com meio centavo arredondado para cima.
- Entradas e saídas usam quantidades inteiras positivas. Saídas não podem deixar o estoque negativo. Cada movimentação recebe um GUID e guarda a descrição, o tipo, a quantidade e o saldo final.
- O estoque e seu histórico ficam em memória durante a execução. Ao reiniciar, o saldo volta ao JSON original. O enunciado não exige persistência das movimentações.
- No terceiro exercício, considerei juros simples de 2,5% ao dia sobre o valor original, pois o enunciado não especifica capitalização. Fórmula: `juros = valor × 0,025 × dias de atraso`.
- Vencimento no dia atual ou no futuro não gera juros. São considerados dias corridos, sem incluir o dia do vencimento. A data atual vem do relógio local do computador.
- Para digitar valores, use vírgula nos centavos, sem separador de milhar. Datas usam `dd/MM/aaaa`.

## Organização

- `Program.cs`: configura o console e inicia o menu.
- `Apresentacao/MenuConsole.cs`: menu, entrada de dados e exibição dos resultados.
- `Dados/LeitorJson.cs`: leitura e desserialização dos arquivos JSON.
- `Modelos`: dados de vendas, produtos e movimentações.
- `Servicos`: regras de comissão, juros e controle de estoque, sem dependência do console ou dos arquivos.

A separação segue responsabilidade única, mantendo as regras independentes da interface. Classes estáticas atendem aos cálculos sem estado; o controle de estoque mantém os produtos e o histórico da execução.
