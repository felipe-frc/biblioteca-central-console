using Biblioteca.Domain.Entities;
using Biblioteca.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Web.Controllers
{
    public class LivrosController : Controller
    {
        private readonly BibliotecaDbContext _context;

        public LivrosController(BibliotecaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var livros = _context.Livros
                .AsNoTracking()
                .OrderBy(l => l.Id)
                .ToList();

            return View(livros);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string titulo, string autor, int anoPublicacao)
        {
            try
            {
                var livro = new Livro(titulo, autor, anoPublicacao);

                _context.Livros.Add(livro);
                _context.SaveChanges();

                TempData["Sucesso"] = "Livro cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        public IActionResult Edit(int id)
        {
            var livro = _context.Livros.FirstOrDefault(l => l.Id == id);
            if (livro is null)
                return NotFound();

            return View(livro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string titulo, string autor, int anoPublicacao)
        {
            var livro = _context.Livros.FirstOrDefault(l => l.Id == id);
            if (livro is null)
                return NotFound();

            try
            {
                livro.AtualizarDados(titulo, autor, anoPublicacao);
                _context.SaveChanges();

                TempData["Sucesso"] = "Livro atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(livro);
            }
        }

        public IActionResult Delete(int id)
        {
            var livro = _context.Livros.AsNoTracking().FirstOrDefault(l => l.Id == id);
            if (livro is null)
                return NotFound();

            bool temEmprestimoRelacionado = _context.Emprestimos.Any(e => e.Livro.Id == id);
            ViewBag.TemEmprestimoAtivo = temEmprestimoRelacionado;

            return View(livro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var livro = _context.Livros.FirstOrDefault(l => l.Id == id);
            if (livro is null)
                return NotFound();

            bool temEmprestimoRelacionado = _context.Emprestimos.Any(e => e.Livro.Id == id);

            if (temEmprestimoRelacionado)
            {
                TempData["Erro"] = "Não é possível excluir este livro porque ele já possui empréstimos registrados (histórico).";
                return RedirectToAction(nameof(Index));
            }

            _context.Livros.Remove(livro);
            _context.SaveChanges();

            TempData["Sucesso"] = "Livro excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}