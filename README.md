# 📚 Biblioteca Áurea

Sistema de gerenciamento de biblioteca desenvolvido em **C# com ASP.NET Core MVC**, com controle de **livros, usuários e empréstimos**.

---

## 🚀 Funcionalidades

- Cadastro, edição e exclusão de livros
- Cadastro, edição e exclusão de usuários
- Registro de empréstimos e devoluções
- Validações de regras de negócio
- Paginação nas listagens
- Testes automatizados das entidades principais

---

## 🛠️ Tecnologias utilizadas

- .NET 8
- ASP.NET Core MVC
- Entity Framework Core
- SQLite
- xUnit

---

## 📁 Estrutura do projeto

- **Biblioteca** → regras de negócio, entidades, enums e serviços
- **Biblioteca.Web** → interface web MVC
- **Biblioteca.Tests** → testes automatizados

---

## 🧠 Regras de negócio

- Livro emprestado fica indisponível
- Não é possível emprestar livro indisponível
- Não é possível excluir usuário com histórico de empréstimos
- Não é possível excluir livro com histórico de empréstimos
- A data prevista de devolução não pode ser no passado
- A data prevista de devolução não pode ultrapassar **365 dias**

---

## ▶️ Como executar o projeto

### 1. Restaurar dependências

```bash id="restore"
dotnet restore
```

### 2. Compilar o projeto

```bash id="build"
dotnet build
```

### 3. Executar a aplicação web

```bash id="run"
dotnet run --project Biblioteca.Web/Biblioteca.Web.csproj
```

### 4. Executar os testes

```bash id="test"
dotnet test
```

---

## 👨‍💻 Autor

Felipe França
Estudante de Engenharia de Computação
