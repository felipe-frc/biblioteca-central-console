namespace Biblioteca.Web.Constants
{
    public static class Messages
    {
        // ============================================
        // LIVROS - SUCESSO
        // ============================================
        public const string LivroAdicionado = "Livro cadastrado com sucesso!";
        public const string LivroAtualizado = "Livro atualizado com sucesso!";
        public const string LivroRemovido = "Livro excluído com sucesso!";

        // ============================================
        // LIVROS - ERRO
        // ============================================
        public const string ErroCarregarLivros = "Erro ao carregar livros. Tente novamente.";
        public const string ErroSalvarLivro = "Erro ao salvar o livro. Verifique se os dados já existem.";
        public const string ErroAtualizarLivro = "Erro ao atualizar o livro.";
        public const string ErroExcluirLivro = "Erro ao excluir o livro. Tente novamente.";
        public const string ErroLivroComEmprestimo = "Não é possível excluir este livro porque ele já possui empréstimos registrados.";
        public const string ErroLivroNaoEncontrado = "Livro não encontrado.";

        // ============================================
        // USUÁRIOS - SUCESSO
        // ============================================
        public const string UsuarioAdicionado = "Usuário cadastrado com sucesso!";
        public const string UsuarioAtualizado = "Usuário atualizado com sucesso!";
        public const string UsuarioRemovido = "Usuário excluído com sucesso!";

        // ============================================
        // USUÁRIOS - ERRO
        // ============================================
        public const string ErroCarregarUsuarios = "Erro ao carregar usuários. Tente novamente.";
        public const string ErroSalvarUsuario = "Erro ao salvar o usuário.";
        public const string ErroAtualizarUsuario = "Erro ao atualizar o usuário.";
        public const string ErroExcluirUsuario = "Erro ao excluir o usuário. Tente novamente.";
        public const string ErroUsuarioNaoEncontrado = "Usuário não encontrado.";
        public const string ErroEmailDuplicado = "Este e-mail já está cadastrado.";
        public const string ErroEmailInvalido = "O e-mail é inválido.";

        // ============================================
        // EMPRÉSTIMOS - SUCESSO
        // ============================================
        public const string EmprestimoAdicionado = "Empréstimo registrado com sucesso!";
        public const string EmprestimoAtualizado = "Empréstimo atualizado com sucesso!";
        public const string EmprestimoDevolvidoComSucesso = "Empréstimo devolvido com sucesso!";

        // ============================================
        // EMPRÉSTIMOS - ERRO
        // ============================================
        public const string ErroCarregarEmprestimos = "Erro ao carregar empréstimos. Tente novamente.";
        public const string ErroSalvarEmprestimo = "Erro ao registrar empréstimo.";
        public const string ErroAtualizarEmprestimo = "Erro ao atualizar empréstimo.";
        public const string ErroExcluirEmprestimo = "Erro ao excluir empréstimo. Tente novamente.";
        public const string ErroEmprestimoNaoEncontrado = "Empréstimo não encontrado.";
        public const string ErroLivroNaoDisponivel = "Este livro não está disponível para empréstimo.";
        public const string ErroUsuarioComEmprestimoAtivo = "Este usuário já possui um empréstimo ativo deste livro.";
        public const string ErroUsuarioOuLivroNaoEncontrado = "Usuário ou livro não encontrado.";
        public const string ErroDataDevolucaoInvalida = "A data de devolução deve ser no futuro.";

        // ============================================
        // VALIDAÇÃO
        // ============================================
        public const string ErroValidacao = "Erro de validação. Verifique os dados informados.";
        public const string ErroConflitoConcorrencia = "O registro foi modificado por outro usuário. Atualize a página e tente novamente.";

        // ============================================
        // GERAL
        // ============================================
        public const string ErroInesperado = "Ocorreu um erro inesperado. Tente novamente.";
    }
}
