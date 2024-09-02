using HospitalTierraMedia.Models;
using HospitalTierraMedia.Web.Models;

namespace HospitalTierraMedia.Web.Services
{
    public interface IServiceUsuario
    {
        Task<ResultadoInicioSesion> Autenticar(string email, string contrasena);
        Task<List<Paciente>> GetAllUsuarios();
        Task<Paciente> GetUsuarioById(string Id);
        Task<Paciente> CreateUsuario(Usuario usuario);
        Task<bool> EditUsuario(Usuario usuario);
    }
}
