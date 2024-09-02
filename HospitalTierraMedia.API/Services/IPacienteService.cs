using HospitalTierraMedia.Models;

namespace HospitalTierraMedia.Services
{
    public interface IPacienteService
    {
        Task<List<Paciente>> GetAllAsync();
        Task<Paciente> GetById(string id);
        Task CreateAsync(Paciente paciente);
        Task UpdateAsync(string id, Paciente paciente);
        Task DeleteAsync(string id);
    }
}