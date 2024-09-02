using HospitalTierraMedia.Web.Models;
using HospitalTierraMedia.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HospitalTierraMedia.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string _token;
        private readonly IServiceUsuario _serviceUsuario;
        public HomeController(ILogger<HomeController> logger, IServiceUsuario serviceUsuario)
        {
            _logger = logger;
            _serviceUsuario = serviceUsuario;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AutenticarAsync(UsuarioInicioSesion usuario)
        {
            var token = await _serviceUsuario.Autenticar(usuario.Email, usuario.Contrasena);
            if (!string.IsNullOrEmpty(token.Token)) 
            {
                Response.Cookies.Append("authToken", token.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(5)
            });
                return RedirectToAction("Index", "Paciente");
            }
            TempData["ErrorMessage"] = "Las credenciales son incorrectas.";
            Response.Cookies.Delete("authToken");
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}