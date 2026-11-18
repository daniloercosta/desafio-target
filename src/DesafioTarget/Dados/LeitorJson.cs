using System.Text.Json;

namespace DesafioTarget;

internal static class LeitorJson
{
    private static readonly JsonSerializerOptions Opcoes = new() { PropertyNameCaseInsensitive = true };

    public static T Ler<T>(string nome)
    {
        string caminho = Path.Combine(AppContext.BaseDirectory, "Dados", nome);
        string conteudo = File.ReadAllText(caminho);
        return JsonSerializer.Deserialize<T>(conteudo, Opcoes)
            ?? throw new ArgumentException($"Arquivo {nome} vazio.");
    }
}
