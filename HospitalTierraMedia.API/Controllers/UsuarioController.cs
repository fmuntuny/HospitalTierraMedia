using HospitalTierraMedia.Models;
using HospitalTierraMedia.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using HospitalTierraMedia.API.Models.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HospitalTierraMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly string? _secretKey;
        public UsuarioController(IUsuarioService usuarioService, IConfiguration config)
        {
            _usuarioService = usuarioService;
            _secretKey = config.GetSection("JWT").GetSection("SecretKey").ToString();
        }

        [HttpGet("GetAllUsuarios")]
        public async Task<IActionResult> Get()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpGet("GetUsuarioById/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var usuario = await _usuarioService.GetById(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost("CreateUsuario")]
        public async Task<IActionResult> Post(Usuario usuario)
        {
            await _usuarioService.CreateAsync(usuario);
            return Ok("Usuario creado correctamente.");
        }

        [HttpPut("EditUsuario/{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Usuario nuevoUsuario)
        {
            var usuario = await _usuarioService.GetById(id);
            if (usuario == null)
                return NotFound();
            await _usuarioService.UpdateAsync(id, nuevoUsuario);
            return Ok("Usuario editado correctamente.");
        }

        /*// DELETE api/Usuario/EliminarUsuario/5
        [HttpDelete("/EliminarUsuario/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var usuario = await _usuarioService.GetById(id);
            if (usuario == null)
                return NotFound();
            await _usuarioService.DeleteAsync(id);
            return Ok("Usuario eliminado correctamente.");
        }*/
        [AllowAnonymous]
        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] UsuarioInicioSesionViewModel usuarioValidar)
        {
            var usuario = await _usuarioService.GetByEmail(usuarioValidar.Email);
            if (usuario == null)
                return NotFound();
            if (usuarioValidar.Contrasena == usuario.Contrasena &&  usuario.Activo == true && usuario.Rol == "admin")
            {
                var keyBytes = Encoding.ASCII.GetBytes(_secretKey);
                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuarioValidar.Email));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
                string tokenCreado = tokenHandler.WriteToken(tokenConfig);
                return Ok(new { token = tokenCreado });
            }
            else
            {
                return Unauthorized(new { token = "" });
            }
        }
    }
}
