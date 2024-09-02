using HospitalTierraMedia.Models;
using HospitalTierraMedia.Web.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace HospitalTierraMedia.Web.Controllers
{
    public class PacienteController : Controller
    {
        private readonly IServicePaciente _pacienteService;

        public PacienteController(IServicePaciente pacienteService)
        {
            _pacienteService = pacienteService;
        }

        private string validateToken()
        {
            string jsonToken = Request.Cookies["authToken"];
            string token = "";
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(jsonToken))
                {
                    if (doc.RootElement.TryGetProperty("token", out JsonElement tokenElement))
                    {
                        token = tokenElement.GetString();
                        var expirationClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
                        if (expirationClaim != null)
                        {
                            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationClaim.Value));
                            if (expirationTime < DateTimeOffset.UtcNow)
                            {
                                TempData["ErrorMessage"] = "Error: Ha caducado la sesión.";
                                Response.Cookies.Delete("authToken");
                                return "Ha caducado la sesión.";
                            }
                        }
                        return token;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error: Ha caducado la sesión.";
                        Response.Cookies.Delete("authToken");
                        return "Ha caducado la sesión.";
                    }
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Error: No se pudo obtener el token.";
                Response.Cookies.Delete("authToken");
                return "Hubo un error obteniendo el token.";
            }
        }

        [HttpGet]
        [Route("Paciente")]
        public async Task<IActionResult> Index()
        {
            string token = validateToken();
            var pacientes = await _pacienteService.GetAllPacientes(token);
            var viewModel = new PacientesViewModel
            {
                Pacientes = pacientes
            };
            return View(viewModel);
        }

        [HttpGet]
        [Route("Paciente/Search")]
        public async Task<IActionResult> Index(string searchDni)
        {
            string token = validateToken();
            var pacientes = await _pacienteService.GetAllPacientes(token);
            var viewModel = new PacientesViewModel();

            if (!string.IsNullOrEmpty(searchDni))
            {
                viewModel.Pacientes = pacientes.Where(p => p.DNI.ToString().Contains(searchDni)).ToList();
                return View(viewModel);
            }
            viewModel.Pacientes = pacientes.Where(x => x.Activo == true).ToList();
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            string token = validateToken();
            var paciente = await _pacienteService.GetPacienteById(id, token);
            if (paciente == null)
            {
                return NotFound();
            }
            else
            {
                var pacienteViewModel = new PacienteViewModel
                {
                    Id = paciente.Id,
                    Nombre = paciente.Nombre,
                    Apellido = paciente.Apellido,
                    Direccion = paciente.Direccion,
                    DNI = paciente.DNI
                };
                return View("Edit", pacienteViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PacienteViewModel pacienteViewModel)
        {
            if (ModelState.IsValid)
            {
                string token = validateToken();
                var pacienteEditado = await _pacienteService.GetPacienteById(pacienteViewModel.Id, token);
                pacienteEditado.Id = pacienteViewModel.Id;
                pacienteEditado.DNI = pacienteViewModel.DNI;
                pacienteEditado.Activo = pacienteViewModel.Activo;
                pacienteEditado.Apellido = pacienteViewModel.Apellido;
                pacienteEditado.Direccion = pacienteViewModel.Direccion;
                pacienteEditado.Nombre = pacienteViewModel.Nombre;
                var updated = await _pacienteService.EditPaciente(pacienteEditado, token);
                if (updated)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se pudo actualizar el paciente.");
                }
            }
            return View(pacienteViewModel);
        }

        public IActionResult Create()
        {
            string token = validateToken();
            string id = ObjectId.GenerateNewId().ToString();
            var pacienteViewModel = new PacienteViewModel
            {
                Id = id,
                Nombre = "",
                Apellido = "",
                Direccion = "",
                DNI = 0,
                Activo = true
            };
            return View("Create", pacienteViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PacienteViewModel pacienteViewModel)
        {
            if (ModelState.IsValid)
            {
                string token = validateToken();
                var nuevoPaciente = new Paciente
                {
                    Id = "",
                    DNI = pacienteViewModel.DNI,
                    Activo = true,
                    Apellido = pacienteViewModel.Apellido,
                    Direccion = pacienteViewModel.Direccion,
                    Nombre = pacienteViewModel.Nombre
                };
                bool resultado = await _pacienteService.CreatePaciente(nuevoPaciente, token);
                if (resultado)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se pudo crear el paciente.");
                }
            }
            return View(pacienteViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID del paciente no proporcionado.");
            }
            string token = validateToken();
            bool resultado = await _pacienteService.DeletePaciente(id, token);

            if (resultado)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return StatusCode(500, "Error al eliminar el paciente.");
            }
        }
    }
}
