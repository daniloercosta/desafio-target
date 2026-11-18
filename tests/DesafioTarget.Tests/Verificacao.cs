namespace DesafioTarget.Tests;

internal sealed class Verificacao
{
    public int Executadas { get; private set; }

    public void Igual<T>(T esperado, T recebido)
    {
        if (!EqualityComparer<T>.Default.Equals(esperado, recebido))
            throw new Exception($"Esperado: {esperado}. Recebido: {recebido}.");
        RegistrarSucesso();
    }

    public void Rejeita(Action acao)
    {
        try { acao(); }
        catch (ArgumentException) { RegistrarSucesso(); return; }
        throw new Exception("A operação deveria ter sido rejeitada.");
    }

    public void RegistrarSucesso() => Executadas++;
}
