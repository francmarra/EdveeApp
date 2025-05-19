using System.Diagnostics;
using Edveeeeeee.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Edveeeeeee.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Connections()
        {
            // Example data - adapt to your needs. Cells are string IDs, connections are lists of cell indices
            var cells = new List<string> { "1", "2", "3" };
            var connections = new List<List<int>>
            {
                new List<int> { 1, 2, 3 },
                new List<int> { 2, 3, 1 },
                new List<int> { 3, 1, 2 }
            };

            ViewBag.Cells = cells;
            ViewBag.Connections = connections;

            return View();
        }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("user")))
            {
                return RedirectToAction("Login", "Conta");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
