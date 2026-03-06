using Biblioteca.Domain.Entities;
using Biblioteca.Services;
using Xunit;

namespace Biblioteca.Tests;

public class BibliotecaServiceTests
{
    [Fact]
    public void CadastrarLivro_ComIdDuplicado_DeveLancarExcecao()
    {
        var service = new BibliotecaService();
        service.CadastrarLivro(new Livro(1, "Livro A", "Autor A", 2000));

        Assert.Throws<InvalidOperationException>(() =>
            service.CadastrarLivro(new Livro(1, "Livro B", "Autor B", 2001))
        );
    }

    [Fact]
    public void RemoverLivro_ComEmprestimoEmAberto_DeveLancarExcecao()
    {
        var service = new BibliotecaService();
        service.CadastrarLivro(new Livro(1, "Clean Code", "Robert", 2008));
        service.CadastrarUsuario(new Usuario(1, "Felipe", "felipe@email.com"));

        service.RealizarEmprestimo(1, 1, DateTime.Today.AddDays(3));

        Assert.Throws<InvalidOperationException>(() => service.RemoverLivro(1));
    }
}