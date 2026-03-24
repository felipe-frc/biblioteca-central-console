using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Enums;


 namespace Biblioteca.Services
{
    public class BibliotecaService : IEmprestimoService
    {
        private List<Livro> _livros = new();
        private List<Usuario> _usuarios = new();
        private List<Emprestimo> _emprestimos = new();

        public Emprestimo RealizarEmprestimo(int idLivro, int idUsuario, DateTime dataPrevistaDevolucao)
        {
            if (idLivro <= 0)
                throw new ArgumentOutOfRangeException(nameof(idLivro), "O id deve ser maior do que zero.");

            if (idUsuario <= 0)
                throw new ArgumentOutOfRangeException(nameof(idUsuario), "O id deve ser maior do que zero.");

            if (dataPrevistaDevolucao.Date < DateTime.Today)
                throw new ArgumentException("A data prevista para devolução não pode ser menor do que a data de hoje.", nameof(dataPrevistaDevolucao));

            if (dataPrevistaDevolucao.Date > DateTime.Today.AddDays(365))
                throw new ArgumentException("A data prevista para devolução não pode ultrapassar 365 dias a partir de hoje.", nameof(dataPrevistaDevolucao));

            var livro = BuscarLivroPorId(idLivro);
            var usuario = BuscarUsuarioPorId(idUsuario);

            if (livro is null)
                throw new InvalidOperationException("Livro não encontrado.");

            if (usuario is null)
                throw new InvalidOperationException("Usuário não encontrado.");

            int novoId = _emprestimos.Any() ? _emprestimos.Max(e => e.Id) + 1 : 1;
            var emprestimo = new Emprestimo(novoId, livro, usuario, dataPrevistaDevolucao);

            _emprestimos.Add(emprestimo);
            return emprestimo;
        }

        public void CadastrarLivro(Livro livro)
        {
            if (livro is null)
                throw new ArgumentNullException(nameof(livro));

            if (_livros.Any(l => l.Id == livro.Id))
                throw new InvalidOperationException("Já existe um livro com esse Id.");

            _livros.Add(livro);
        }

        public void CadastrarUsuario(Usuario usuario)
        {
            if (usuario is null)
                throw new ArgumentNullException(nameof(usuario));

            if (_usuarios.Any(u => u.Id == usuario.Id))
                throw new InvalidOperationException("Já existe um usuário com esse Id.");

            string emailNormalizado = usuario.Email.Trim().ToLowerInvariant();

            if (_usuarios.Any(u => u.Email.Trim().ToLowerInvariant() == emailNormalizado))
                throw new InvalidOperationException("Já existe um usuário com esse e-mail.");

            _usuarios.Add(usuario);
        }

        private Livro? BuscarLivroPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id deve ser maior do que zero.");

            return _livros.FirstOrDefault(l => l.Id == id);
        }

        private Usuario? BuscarUsuarioPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id deve ser maior do que zero.");

            return _usuarios.FirstOrDefault(u => u.Id == id);
        }

        public void DevolverEmprestimo(int idEmprestimo)
        {
            if (idEmprestimo <= 0)
                throw new ArgumentOutOfRangeException(nameof(idEmprestimo), "Id do empréstimo deve ser maior que zero.");

            var emprestimo = _emprestimos.FirstOrDefault(e => e.Id == idEmprestimo);

            if (emprestimo is null)
                throw new InvalidOperationException("Empréstimo não encontrado.");

            emprestimo.Devolver();
        }

        public List<Emprestimo> ListarEmprestimosAtrasados(DateTime dataReferencia, int page = 1, int pageSize = 10)
        {
            const int defaultPageSize = 10;

            if (page <= 0)
                page = 1;

            if (pageSize <= 0)
                pageSize = defaultPageSize;

            var atrasados = _emprestimos
                .Where(e =>
                {
                    e.AtualizarStatus(dataReferencia);
                    return e.Status == StatusEmprestimo.Atrasado;
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return atrasados;
        }

        public List<Livro> ListarLivrosDisponiveis()
        {
            var disponiveis = new List<Livro>();

            foreach (Livro livro in _livros)
            {
                if (livro.Disponivel)
                {
                    disponiveis.Add(livro);
                }
            }

            return disponiveis;
        }

        public List<Emprestimo> ListarEmprestimosDoUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentOutOfRangeException(nameof(idUsuario), "Id do usuário deve ser maior que zero.");

            var usuario = BuscarUsuarioPorId(idUsuario);
            if (usuario is null)
                throw new InvalidOperationException("Usuário não encontrado.");

            var emprestimosDoUsuario = new List<Emprestimo>();

            foreach (var emprestimo in _emprestimos)
            {
                if (emprestimo.Usuario.Id == idUsuario)
                {
                    emprestimosDoUsuario.Add(emprestimo);
                }
            }

            return emprestimosDoUsuario;
        }

        public void RemoverLivro(int idLivro)
        {
            if (idLivro <= 0)
                throw new ArgumentOutOfRangeException(nameof(idLivro), "Id do livro deve ser maior que zero.");

            var livro = BuscarLivroPorId(idLivro);
            if (livro is null)
                throw new InvalidOperationException("Livro não encontrado.");

            bool existeHistoricoEmprestimo =
                _emprestimos.Any(e => e.Livro.Id == idLivro);

            if (existeHistoricoEmprestimo)
                throw new InvalidOperationException("Não é possível remover o livro pois ele possui empréstimos registrados.");

            _livros.Remove(livro);
        }

        public void RemoverUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentOutOfRangeException(nameof(idUsuario), "Id do usuário deve ser maior que zero.");

            var usuario = BuscarUsuarioPorId(idUsuario);
            if (usuario is null)
                throw new InvalidOperationException("Usuário não encontrado.");

            bool existeHistoricoEmprestimo =
                _emprestimos.Any(e => e.Usuario.Id == idUsuario);

            if (existeHistoricoEmprestimo)
                throw new InvalidOperationException("Não é possível remover o usuário pois ele possui empréstimos registrados.");

            _usuarios.Remove(usuario);
        }

        public List<Emprestimo> ListarEmprestimosAtivos()
        {
            var emprestimosAtivos = new List<Emprestimo>();

            foreach (var emprestimo in _emprestimos)
            {
                if (emprestimo.DataDevolucao is null)
                    emprestimosAtivos.Add(emprestimo);
            }

            return emprestimosAtivos;
        }

        public List<Emprestimo> ListarTodos()
        {
            return _emprestimos;
        }

        public Emprestimo Realizar(int livroId, int usuarioId, DateTime dataPrevistaDevolucao)
        {
            return RealizarEmprestimo(livroId, usuarioId, dataPrevistaDevolucao);
        }

        public void Devolver(int emprestimoId)
        {
            DevolverEmprestimo(emprestimoId);
        }

    }






}
