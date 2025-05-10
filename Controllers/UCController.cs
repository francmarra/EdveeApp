using Microsoft.AspNetCore.Mvc;
using Edveeeeeee.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Edveeeeeee.Models;
using Edveeeeeee.Models.ViewModels;

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

        public IActionResult EdVee(int id)
        {
            var uc = _context.UCs.FirstOrDefault(u => u.Id == id);
            if (uc == null) return NotFound();

            var model = new EdVeeViewModel
            {
                UnidadeCurricular = uc,
                Competencias = _context.Competencias.Where(c => c.UnidadeCurricularId == id).ToList(),
                Conteudos = _context.Conteudos.Where(c => c.UnidadeCurricularId == id).ToList(),
                Atividades = _context.Atividades.Where(a => a.UnidadeCurricularId == id).ToList(),
                Avaliacoes = _context.Avaliacoes.Where(a => a.UnidadeCurricularId == id).ToList()
            };

            return View(model);
        }

    }
}
