using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Enums;
using Xunit;

namespace Biblioteca.Tests;

public class EmprestimoTests
{
    [Fact]
    public void Emprestimo_Valido_DeveSerCriadoComStatusAtivo()
    {
        var livro = new Livro("Clean Code", "Robert C. Martin", 2008);
        var usuario = new Usuario(1, "Felipe", "felipe@email.com");
        var dataPrevista = DateTime.Today.AddDays(7);

        var emprestimo = new Emprestimo(1, livro, usuario, dataPrevista);

        Assert.Equal(1, emprestimo.Id);
        Assert.Equal(livro, emprestimo.Livro);
        Assert.Equal(usuario, emprestimo.Usuario);
        Assert.Equal(dataPrevista.Date, emprestimo.DataPrevistaDevolucao);
        Assert.Equal(StatusEmprestimo.Ativo, emprestimo.Status);
        Assert.Null(emprestimo.DataDevolucao);
        Assert.False(livro.Disponivel);
    }

    [Fact]
    public void Emprestimo_ComDataNoPassado_DeveLancarExcecao()
    {
        var livro = new Livro("Clean Code", "Robert C. Martin", 2008);
        var usuario = new Usuario(1, "Felipe", "felipe@email.com");

        Assert.Throws<ArgumentException>(() =>
            new Emprestimo(1, livro, usuario, DateTime.Today.AddDays(-1)));
    }

    [Fact]
    public void Emprestimo_ComDataMaiorQue365Dias_DeveLancarExcecao()
    {
        var livro = new Livro("Clean Code", "Robert C. Martin", 2008);
        var usuario = new Usuario(1, "Felipe", "felipe@email.com");

        Assert.Throws<ArgumentException>(() =>
            new Emprestimo(1, livro, usuario, DateTime.Today.AddDays(366)));
    }

    [Fact]
    public void Devolver_DeveRegistrarDataDevolucaoETornarLivroDisponivel()
    {
        var livro = new Livro("Clean Code", "Robert C. Martin", 2008);
        var usuario = new Usuario(1, "Felipe", "felipe@email.com");
        var emprestimo = new Emprestimo(1, livro, usuario, DateTime.Today.AddDays(7));

        emprestimo.Devolver();

        Assert.NotNull(emprestimo.DataDevolucao);
        Assert.True(livro.Disponivel);
        Assert.Equal(StatusEmprestimo.Devolvido, emprestimo.Status);
    }

    [Fact]
    public void Devolver_QuandoJaDevolvido_DeveLancarExcecao()
    {
        var livro = new Livro("Clean Code", "Robert C. Martin", 2008);
        var usuario = new Usuario(1, "Felipe", "felipe@email.com");
        var emprestimo = new Emprestimo(1, livro, usuario, DateTime.Today.AddDays(7));

        emprestimo.Devolver();

        Assert.Throws<InvalidOperationException>(() => emprestimo.Devolver());
    }

    [Fact]
    public void AtualizarStatus_ComDataAposVencimento_DeveFicarAtrasado()
    {
        var livro = new Livro("Clean Code", "Robert C. Martin", 2008);
        var usuario = new Usuario(1, "Felipe", "felipe@email.com");
        var emprestimo = new Emprestimo(1, livro, usuario, DateTime.Today);

        emprestimo.AtualizarStatus(DateTime.Today.AddDays(1));

        Assert.Equal(StatusEmprestimo.Atrasado, emprestimo.Status);
    }

    [Fact]
    public void EstaAtrasado_AntesDoVencimento_DeveRetornarFalso()
    {
        var livro = new Livro("Clean Code", "Robert C. Martin", 2008);
        var usuario = new Usuario(1, "Felipe", "felipe@email.com");
        var emprestimo = new Emprestimo(1, livro, usuario, DateTime.Today.AddDays(5));

        var resultado = emprestimo.EstaAtrasado(DateTime.Today);

        Assert.False(resultado);
    }

    [Fact]
    public void EstaAtrasado_AposVencimento_DeveRetornarVerdadeiro()
    {
        var livro = new Livro("Clean Code", "Robert C. Martin", 2008);
        var usuario = new Usuario(1, "Felipe", "felipe@email.com");
        var emprestimo = new Emprestimo(1, livro, usuario, DateTime.Today);

        var resultado = emprestimo.EstaAtrasado(DateTime.Today.AddDays(1));

        Assert.True(resultado);
    }
}