using Microsoft.AspNetCore.Mvc;
using Edveeeeeee.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Edveeeeeee.Models;

namespace Edveeeeeee.Controllers
{
    public class UCController : Controller
    {
        private readonly AppDbContext _context;

        public UCController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Conta");
            }

            var ucs = _context.UCs
                .Where(uc => uc.ProfessorId == userId)
                .ToList();

            return View(ucs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UnidadeCurricular uc)
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null) return RedirectToAction("Login", "Conta");

            uc.ProfessorId = (int)userId;
            _context.UCs.Add(uc);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var uc = _context.UCs.FirstOrDefault(x => x.Id == id);
            if (uc == null) return NotFound();
            return View(uc);
        }

        [HttpPost]
        public IActionResult Edit(UnidadeCurricular uc)
        {
            _context.UCs.Update(uc);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
