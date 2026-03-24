namespace Biblioteca.Domain.Entities
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Entidade que representa um usuário da biblioteca.
    /// </summary>
    public class Usuario
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public int Id { get; private set; }
        public string Nome { get; private set; } = default!;
        public string Email { get; private set; } = default!;

        private Usuario() { }

        /// <summary>
        /// Construtor para criação de novo usuário (sem ID - gerado pelo banco).
        /// </summary>
        public Usuario(string nome, string email)
        {
            ValidarDados(nome, email);

            Nome = nome.Trim();
            Email = email.Trim().ToLowerInvariant();
        }

        /// <summary>
        /// Construtor para criação de usuário com ID (usado em testes/console).
        /// </summary>
        public Usuario(int id, string nome, string email)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "O ID deve ser maior que zero.");

            ValidarDados(nome, email);

            Id = id;
            Nome = nome.Trim();
            Email = email.Trim().ToLowerInvariant();
        }

        /// <summary>
        /// Valida os dados do usuário.
        /// </summary>
        private static void ValidarDados(string nome, string email)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome não pode ser vazio.", nameof(nome));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("O email não pode ser vazio.", nameof(email));

            string nomeTratado = nome.Trim();
            string emailTratado = email.Trim();

            if (string.IsNullOrWhiteSpace(nomeTratado))
                throw new ArgumentException("O nome não pode ser vazio.", nameof(nome));

            if (string.IsNullOrWhiteSpace(emailTratado))
                throw new ArgumentException("O email não pode ser vazio.", nameof(email));

            if (!IsValidEmail(emailTratado))
                throw new ArgumentException("Email inválido. Use o formato: usuario@dominio.com", nameof(email));
        }

        /// <summary>
        /// Valida o formato do email usando regex.
        /// </summary>
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || email.Length > 254)
                return false;

            // Validação básica com regex
            if (!EmailRegex.IsMatch(email))
                return false;

            // Validação adicional: não pode ter espaços
            if (email.Contains(" "))
                return false;

            return true;
        }

        /// <summary>
        /// Atualiza os dados do usuário.
        /// </summary>
        public void AtualizarDados(string nome, string email)
        {
            ValidarDados(nome, email);

            Nome = nome.Trim();
            Email = email.Trim().ToLowerInvariant();
        }
    }
}