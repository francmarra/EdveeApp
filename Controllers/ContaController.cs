using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Edveeeeeee.Data;
using Edveeeeeee.Models;

namespace Edveeeeeee.Controllers
{
    public class ContaController : Controller
    {
        private readonly AppDbContext _context;

        public ContaController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var professor = _context.Professores
                .FirstOrDefault(p => p.Username == username && p.Password == password);

            if (professor != null)
            {
                HttpContext.Session.SetString("user", professor.Nome);
                HttpContext.Session.SetInt32("userId", professor.Id);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciais inválidas.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
