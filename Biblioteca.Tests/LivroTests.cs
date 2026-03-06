using Biblioteca.Domain.Entities;
using Xunit;

namespace Biblioteca.Tests;

public class LivroTests
{
    [Fact]
    public void MarcarComoEmprestado_DeveTornarIndisponivel()
    {
        var livro = new Livro(1, "Clean Code", "Robert C. Martin", 2008);

        livro.MarcarComoEmprestado();

        Assert.False(livro.Disponivel);
    }

    [Fact]
    public void MarcarComoEmprestado_QuandoJaEmprestado_DeveLancarExcecao()
    {
        var livro = new Livro(1, "Clean Code", "Robert C. Martin", 2008);
        livro.MarcarComoEmprestado();

        Assert.Throws<InvalidOperationException>(() => livro.MarcarComoEmprestado());
    }
}