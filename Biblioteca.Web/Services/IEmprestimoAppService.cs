using Biblioteca.Domain.Entities;

namespace Biblioteca.Web.Services
{
    public interface IEmprestimoAppService
    {
        Emprestimo Realizar(int livroId, int usuarioId, DateTime dataPrevistaDevolucao);
        void Devolver(int emprestimoId);
    }
}