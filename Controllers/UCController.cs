using Microsoft.AspNetCore.Mvc;
using Edveeeeeee.Models;

namespace Edveeeeeee.Controllers
{
    public class UCController : Controller
    {
        public IActionResult Index()
        {
            var ucs = new List<UnidadeCurricular>
            {
                new UnidadeCurricular { Nome = "Engenharia de Software", Codigo = "14307", Turmas = "PL6, T1", Regime = "1.º Semestre" },
                new UnidadeCurricular { Nome = "Programação Multiplataforma", Codigo = "14213", Turmas = "PL4, T1", Regime = "2.º Semestre" },
                // Podes adicionar mais UCs aqui
            };

            return View(ucs);
        }
    }
}
