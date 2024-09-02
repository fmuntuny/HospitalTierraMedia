using HospitalTierraMedia.Models;
using HospitalTierraMedia.Web.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace HospitalTierraMedia.Web.Services
{
    public class ServiceUsuario : IServiceUsuario
    {
        private readonly HttpClient _httpClient;
        private static string _baseURL = "http://api:80/";
        public ServiceUsuario(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResultadoInicioSesion> Autenticar(string email, string contrasena)
        {
            var url = _baseURL + "api/Usuario/GetToken";
            ResultadoInicioSesion result= new ResultadoInicioSesion();
            var requestData = new
            {
                email = email,
                contrasena = contrasena
            };
            var jsonContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, jsonContent);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                result.Token = responseBody;
                return result;
            }
            else
            {
                return new ResultadoInicioSesion();
            }
        }
        public Task<Paciente> CreateUsuario(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditUsuario(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public Task<List<Paciente>> GetAllUsuarios()
        {
            throw new NotImplementedException();
        }

        public Task<Paciente> GetUsuarioById(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
