using Biblioteca.Domain.Entities;
using Xunit;

namespace Biblioteca.Tests;

public class UnitTest1
{
    [Fact]
    public void Livro_Novo_DeveComecarDisponivel()
    {
        var livro = new Livro(1, "Clean Code", "Robert C. Martin", 2008);
        Assert.True(livro.Disponivel);
    }
}