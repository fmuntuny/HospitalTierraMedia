using HospitalTierraMedia.Models;
using HospitalTierraMedia.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HospitalTierraMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;
        public PacienteController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        // GET: api/Paciente/GetAllPacientes
        [HttpGet("GetAllPacientes")]
        public async Task<IActionResult> Get()
        {
            var pacientes = await _pacienteService.GetAllAsync();
            return Ok(pacientes);
        }

        // GET api/Paciente/GetById/5
        [HttpGet("GetPacienteById/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var paciente = await _pacienteService.GetById(id);
            if (paciente == null)
            {
                return NotFound();
            }
            return Ok(paciente);
        }

        // POST api/Paciente
        [HttpPost("CreatePaciente")]
        public async Task<IActionResult> Create([FromBody] Paciente nuevoPaciente)
        {
            if (nuevoPaciente == null)
            {
                return BadRequest("Paciente no puede ser nulo.");
            }

            await _pacienteService.CreateAsync(nuevoPaciente);
            return Ok("Paciente creado correctamente.");
        }

        // PUT api/Paciente/5
        [HttpPut("EditPaciente/{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Paciente nuevoPaciente)
        {
            var paciente = await _pacienteService.GetById(id);
            if (paciente == null)
                return NotFound();
            await _pacienteService.UpdateAsync(id, nuevoPaciente);
            return Ok("Paciente editado correctamente.");
        }

        // DELETE api/Paciente/EliminarPaciente/5
        [HttpPut("/DeletePaciente/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var paciente = await _pacienteService.GetById(id);
            if (paciente == null)
                return NotFound();
            await _pacienteService.DeleteAsync(id);
            return Ok("Paciente eliminado correctamente.");
        }
    }
}
