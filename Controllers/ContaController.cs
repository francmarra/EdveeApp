using Microsoft.AspNetCore.Mvc;

namespace Edveeeeeee.Controllers
{
    public class ContaController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Autenticação mock (simples)
            if (username == "prof" && password == "1234")
            {
                HttpContext.Session.SetString("user", username);
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
