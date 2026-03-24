using Biblioteca.Domain.Entities;
using Biblioteca.Services;
using System.Globalization;

var service = new BibliotecaService();

// Seed (dados iniciais)
service.CadastrarLivro(new Livro(1, "Clean Code", "Robert C. Martin", 2008));
service.CadastrarLivro(new Livro(2, "The Pragmatic Programmer", "Andrew Hunt", 1999));
service.CadastrarUsuario(new Usuario(1, "Felipe", "felipe@email.com"));

while (true)
{
    Console.Clear();

    Console.WriteLine("----- Biblioteca Central -----");
    Console.WriteLine();
    Console.WriteLine("Olá, seja bem-vindo(a) a nossa Biblioteca Central!");
    Console.WriteLine("Selecione uma opção:");
    Console.WriteLine();
    Console.WriteLine("1 - Cadastrar Livro");
    Console.WriteLine("2 - Cadastrar Usuário");
    Console.WriteLine("3 - Realizar Empréstimo");
    Console.WriteLine("4 - Listar Livros Disponíveis");
    Console.WriteLine("5 - Devolver Empréstimo");
    Console.WriteLine("6 - Listar Empréstimos do Usuário");
    Console.WriteLine("7 - Listar Empréstimos Atrasados");
    Console.WriteLine("8 - Remover Livro");
    Console.WriteLine("9 - Remover Usuário");
    Console.WriteLine("0 - Sair");
    Console.WriteLine();

    int opcao = LerOpcaoMenu("Digite a opção: ");

    try
    {
        switch (opcao)
        {
            case 1:
                Console.WriteLine("Cadastro de Livro");
                Console.WriteLine();
                CadastrarLivro(service);
                break;

            case 2:
                Console.WriteLine("Cadastro de Usuário");
                Console.WriteLine();
                CadastrarUsuario(service);
                break;

            case 3:
                Console.WriteLine("Realizar Empréstimo");
                Console.WriteLine();
                RealizarEmprestimo(service);
                break;

            case 4:
                Console.WriteLine("Listar Livros Disponíveis");
                Console.WriteLine();
                ListarLivrosDisponiveis(service);
                break;

            case 5:
                Console.WriteLine("Devolver Empréstimo");
                Console.WriteLine();
                DevolverEmprestimo(service);
                break;

            case 6:
                Console.WriteLine("Listar Empréstimos do Usuário");
                Console.WriteLine();
                ListarEmprestimosDoUsuario(service);
                break;

            case 7:
                Console.WriteLine("Listar Empréstimos Atrasados");
                Console.WriteLine();
                ListarEmprestimosAtrasados(service);
                break;

            case 8:
                Console.WriteLine("Remover Livro");
                Console.WriteLine();
                RemoverLivro(service);
                break;

            case 9:
                Console.WriteLine("Remover Usuário");
                Console.WriteLine();
                RemoverUsuario(service);
                break;

            case 0:
                return;

            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine($"Erro: {ex.Message}");
    }

    Console.WriteLine();
    Console.WriteLine("Pressione uma tecla para continuar...");
    Console.ReadKey();
}

static void CadastrarLivro(BibliotecaService service)
{
    int idLivro = LerInt("Id: ");
    string titulo = LerTextoObrigatorio("Título: ");
    string autor = LerTextoObrigatorio("Autor: ");
    int anoPublicacao = LerInt("Ano de publicação: ");

    var livro = new Livro(idLivro, titulo, autor, anoPublicacao);
    service.CadastrarLivro(livro);

    Console.WriteLine();
    Console.WriteLine("Livro cadastrado com sucesso!");
}

static void CadastrarUsuario(BibliotecaService service)
{
    int idUsuario = LerInt("Id: ");
    string nome = LerTextoObrigatorio("Nome: ");
    string email = LerTextoObrigatorio("Email: ");

    var usuario = new Usuario(idUsuario, nome, email);
    service.CadastrarUsuario(usuario);

    Console.WriteLine();
    Console.WriteLine("Usuário cadastrado com sucesso!");
}

static void RealizarEmprestimo(BibliotecaService service)
{
    int idLivro = LerInt("Id do Livro: ");
    int idUsuario = LerInt("Id do Usuário: ");
    DateTime dataPrevista = LerData("Data prevista de devolução (dd/MM/yyyy): ");

    var emprestimo = service.RealizarEmprestimo(idLivro, idUsuario, dataPrevista);

    Console.WriteLine();
    Console.WriteLine("Empréstimo realizado com sucesso!");
    Console.WriteLine($"Id: {emprestimo.Id}");
    Console.WriteLine($"Livro: {emprestimo.Livro.Titulo}");
    Console.WriteLine($"Usuário: {emprestimo.Usuario.Nome}");
    Console.WriteLine($"Prevista: {emprestimo.DataPrevistaDevolucao:dd/MM/yyyy}");
    Console.WriteLine($"Status: {emprestimo.Status}");
}

static void ListarLivrosDisponiveis(BibliotecaService service)
{
    var disponiveis = service.ListarLivrosDisponiveis();

    if (disponiveis.Count == 0)
    {
        Console.WriteLine("Nenhum livro disponível no momento.");
        return;
    }

    Console.WriteLine("Livros disponíveis:");
    foreach (var livro in disponiveis)
    {
        Console.WriteLine($"{livro.Id} | {livro.Titulo} | {livro.Autor} | {livro.AnoPublicacao}");
    }
}

static void DevolverEmprestimo(BibliotecaService service)
{
    int idEmprestimo = LerInt("Id do Empréstimo: ");
    service.DevolverEmprestimo(idEmprestimo);

    Console.WriteLine();
    Console.WriteLine("Empréstimo devolvido com sucesso!");
}

static void ListarEmprestimosDoUsuario(BibliotecaService service)
{
    int idUsuario = LerInt("Id do Usuário: ");
    var emprestimos = service.ListarEmprestimosDoUsuario(idUsuario);

    Console.WriteLine();
    if (emprestimos.Count == 0)
    {
        Console.WriteLine("Esse usuário não possui empréstimos.");
        return;
    }

    Console.WriteLine("Empréstimos:");
    foreach (var e in emprestimos)
    {
        string devolucao = e.DataDevolucao is null ? "-" : e.DataDevolucao.Value.ToString("dd/MM/yyyy");
        Console.WriteLine($"{e.Id} | Livro: {e.Livro.Titulo} | Prevista: {e.DataPrevistaDevolucao:dd/MM/yyyy} | Devolução: {devolucao} | Status: {e.Status}");
    }
}

static void ListarEmprestimosAtrasados(BibliotecaService service)
{
    DateTime dataReferencia = LerData("Data de referência (dd/MM/yyyy): ");
    var atrasados = service.ListarEmprestimosAtrasados(dataReferencia);

    Console.WriteLine();
    if (atrasados.Count == 0)
    {
        Console.WriteLine("Nenhum empréstimo atrasado nessa data.");
        return;
    }

    Console.WriteLine("Empréstimos atrasados:");
    foreach (var e in atrasados)
    {
        Console.WriteLine($"{e.Id} | Livro: {e.Livro.Titulo} | Usuário: {e.Usuario.Nome} | Prevista: {e.DataPrevistaDevolucao:dd/MM/yyyy} | Status: {e.Status}");
    }
}

static void RemoverLivro(BibliotecaService service)
{
    int idLivro = LerInt("Id do Livro: ");
    service.RemoverLivro(idLivro);

    Console.WriteLine();
    Console.WriteLine("Livro removido com sucesso!");
}

static void RemoverUsuario(BibliotecaService service)
{
    int idUsuario = LerInt("Id do Usuário: ");
    service.RemoverUsuario(idUsuario);

    Console.WriteLine();
    Console.WriteLine("Usuário removido com sucesso!");
}

// ===== Helpers (polimento v1) =====

static int LerOpcaoMenu(string label)
{
    while (true)
    {
        Console.Write(label);
        string? entrada = Console.ReadLine();

        if (int.TryParse(entrada, out int opcao))
            return opcao;

        Console.WriteLine("Opção inválida. Digite um número.");
    }
}

static int LerInt(string label)
{
    while (true)
    {
        Console.Write(label);
        string? entrada = Console.ReadLine();

        if (int.TryParse(entrada, out int valor))
            return valor;

        Console.WriteLine("Valor inválido. Digite um número inteiro.");
    }
}

static string LerTextoObrigatorio(string label)
{
    while (true)
    {
        Console.Write(label);
        string? texto = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(texto))
            return texto.Trim();

        Console.WriteLine("Campo obrigatório. Tente novamente.");
    }
}

static DateTime LerData(string label)
{
    while (true)
    {
        Console.Write(label);
        string? entrada = Console.ReadLine();

        if (DateTime.TryParseExact(
                entrada,
                "dd/MM/yyyy",
                CultureInfo.GetCultureInfo("pt-BR"),
                DateTimeStyles.None,
                out DateTime data))
        {
            return data.Date;
        }

        Console.WriteLine("Data inválida. Use o formato dd/MM/yyyy (ex: 06/03/2026).");
    }
}