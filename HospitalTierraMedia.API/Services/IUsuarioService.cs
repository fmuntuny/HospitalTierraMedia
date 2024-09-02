using HospitalTierraMedia.Models;

namespace HospitalTierraMedia.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario> GetById(string id);
        Task<Usuario> GetByEmail(string email);
        Task CreateAsync(Usuario usuario);
        Task UpdateAsync(string id, Usuario usuario);
        /*Task DeleteAsync(string id);*/
    }
}