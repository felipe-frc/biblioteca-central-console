using Biblioteca.Domain.Entities;
using Biblioteca.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Web.Controllers
{
    public class EmprestimosController : Controller
    {
        private readonly BibliotecaDbContext _context;

        public EmprestimosController(BibliotecaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var emprestimos = _context.Emprestimos
                .Include(e => e.Livro)
                .Include(e => e.Usuario)
                .OrderByDescending(e => e.Id)
                .ToList();

            return View(emprestimos);
        }

        public IActionResult Create()
        {
            ViewBag.Usuarios = _context.Usuarios.ToList();
            ViewBag.Livros = _context.Livros.Where(l => l.Disponivel).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int usuarioId, int livroId, DateTime dataPrevistaDevolucao)
        {
            void CarregarListas()
            {
                ViewBag.Usuarios = _context.Usuarios.ToList();
                ViewBag.Livros = _context.Livros.Where(l => l.Disponivel).ToList();
            }

            if (usuarioId <= 0 || livroId <= 0)
            {
                TempData["Erro"] = "Selecione um usuário e um livro.";
                CarregarListas();
                return View();
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == usuarioId);
            if (usuario is null)
            {
                TempData["Erro"] = "Usuário não encontrado.";
                CarregarListas();
                return View();
            }

            var livro = _context.Livros.FirstOrDefault(l => l.Id == livroId);
            if (livro is null)
            {
                TempData["Erro"] = "Livro não encontrado.";
                CarregarListas();
                return View();
            }

            if (!livro.Disponivel)
            {
                TempData["Erro"] = "Livro indisponível para empréstimo.";
                CarregarListas();
                return View();
            }

            if (dataPrevistaDevolucao.Date < DateTime.Today)
            {
                TempData["Erro"] = "A data prevista de devolução não pode ser no passado.";
                CarregarListas();
                return View();
            }

            try
            {
                var emprestimo = new Emprestimo(livro, usuario, dataPrevistaDevolucao);

                _context.Emprestimos.Add(emprestimo);
                _context.SaveChanges();

                TempData["Sucesso"] = "Empréstimo realizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao realizar empréstimo: {ex.Message}";
                CarregarListas();
                return View();
            }
        }

        public IActionResult Devolver(int id)
        {
            var emprestimo = _context.Emprestimos
                .Include(e => e.Livro)
                .Include(e => e.Usuario)
                .FirstOrDefault(e => e.Id == id);

            if (emprestimo is null)
                return NotFound();

            return View(emprestimo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DevolverConfirmed(int id)
        {
            var emprestimo = _context.Emprestimos
                .Include(e => e.Livro)
                .Include(e => e.Usuario)
                .FirstOrDefault(e => e.Id == id);

            if (emprestimo is null)
                return NotFound();

            if (emprestimo.DataDevolucao is not null)
            {
                TempData["Erro"] = "Este empréstimo já foi devolvido.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                emprestimo.Devolver();
                _context.SaveChanges();

                TempData["Sucesso"] = "Empréstimo devolvido com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}