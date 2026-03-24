using Biblioteca.Domain.Entities;

namespace Biblioteca.Services
{
    public interface IEmprestimoService
    {
        List<Emprestimo> ListarTodos();

        Emprestimo Realizar(int livroId, int usuarioId, DateTime dataPrevistaDevolucao);

        void Devolver(int emprestimoId);
    }
}