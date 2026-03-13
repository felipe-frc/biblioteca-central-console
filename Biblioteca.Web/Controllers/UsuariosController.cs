using Biblioteca.Domain.Entities;
using Biblioteca.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly BibliotecaDbContext _context;

        public UsuariosController(BibliotecaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var usuarios = _context.Usuarios
                .AsNoTracking()
                .OrderBy(u => u.Id)
                .ToList();

            return View(usuarios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string nome, string email)
        {
            try
            {
                var usuario = new Usuario(nome, email);

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                TempData["Sucesso"] = "Usuário cadastrado com sucesso!";
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
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario is null)
                return NotFound();

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string nome, string email)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario is null)
                return NotFound();

            try
            {
                usuario.AtualizarDados(nome, email);
                _context.SaveChanges();

                TempData["Sucesso"] = "Usuário atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(usuario);
            }
        }

        public IActionResult Delete(int id)
        {
            var usuario = _context.Usuarios.AsNoTracking().FirstOrDefault(u => u.Id == id);
            if (usuario is null)
                return NotFound();

            bool temEmprestimoRelacionado = _context.Emprestimos.Any(e => e.Usuario.Id == id);
            ViewBag.TemEmprestimoAtivo = temEmprestimoRelacionado;

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario is null)
                return NotFound();

            bool temEmprestimoRelacionado = _context.Emprestimos.Any(e => e.Usuario.Id == id);

            if (temEmprestimoRelacionado)
            {
                TempData["Erro"] = "Não é possível excluir este usuário porque ele já possui empréstimos registrados (histórico).";
                return RedirectToAction(nameof(Index));
            }

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            TempData["Sucesso"] = "Usuário excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}