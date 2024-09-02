using HospitalTierraMedia.Models;

namespace HospitalTierraMedia.Web.Services
{
    public interface IServicePaciente
    {
        Task<List<Paciente>> GetAllPacientes(string token);
        Task<Paciente> GetPacienteById(string Id, string token);
        Task<bool> CreatePaciente(Paciente paciente, string token);
        Task<bool> EditPaciente(Paciente paciente, string token);
        Task<bool> DeletePaciente(string Id, string token);
    }
}
