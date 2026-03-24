using Biblioteca.Domain.Entities;
using Xunit;

namespace Biblioteca.Tests;

public class UsuarioTests
{
    [Fact]
    public void Usuario_Valido_DeveSerCriadoComSucesso()
    {
        var usuario = new Usuario("Felipe", "felipe@email.com");

        Assert.Equal("Felipe", usuario.Nome);
        Assert.Equal("felipe@email.com", usuario.Email);
    }

    [Fact]
    public void Usuario_ComEspacos_DeveNormalizarNomeEEmail()
    {
        var usuario = new Usuario("  Felipe França  ", "  FELIPE@EMAIL.COM  ");

        Assert.Equal("Felipe França", usuario.Nome);
        Assert.Equal("felipe@email.com", usuario.Email);
    }

    [Fact]
    public void Usuario_ComIdInvalido_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Usuario(0, "Felipe", "felipe@email.com"));
    }

    [Fact]
    public void Usuario_ComNomeVazio_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() =>
            new Usuario("", "felipe@email.com"));
    }

    [Fact]
    public void Usuario_ComEmailVazio_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() =>
            new Usuario("Felipe", ""));
    }

    [Fact]
    public void Usuario_ComEmailInvalido_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() =>
            new Usuario("Felipe", "emailinvalido"));
    }

    [Fact]
    public void AtualizarDados_DeveAtualizarNomeEEmail()
    {
        var usuario = new Usuario("Felipe", "felipe@email.com");

        usuario.AtualizarDados("João", "joao@email.com");

        Assert.Equal("João", usuario.Nome);
        Assert.Equal("joao@email.com", usuario.Email);
    }

    [Fact]
    public void AtualizarDados_DeveNormalizarEmail()
    {
        var usuario = new Usuario("Felipe", "felipe@email.com");

        usuario.AtualizarDados("Felipe França", "  FELIPE.FRANCA@EMAIL.COM  ");

        Assert.Equal("Felipe França", usuario.Nome);
        Assert.Equal("felipe.franca@email.com", usuario.Email);
    }
}