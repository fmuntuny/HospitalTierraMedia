using HospitalTierraMedia.Models;
using HospitalTierraMedia.Web.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HospitalTierraMedia.Web.Services
{
    public class ServicePaciente : IServicePaciente
    {
        private readonly HttpClient _httpClient;
        private static string _baseURL = "http://api:80/";
        public ServicePaciente(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CreatePaciente(Paciente paciente, string token)
        {
            try
            {
                string url = _baseURL + "api/Paciente/CreatePaciente";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var jsonContent = JsonSerializer.Serialize(paciente);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch 
            {
                return false;
            }
        }

        public async Task<bool> EditPaciente(Paciente paciente, string token)
        {
            try
            {
                string url = _baseURL + $"api/Paciente/EditPaciente/{paciente.Id}";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var jsonContent = JsonSerializer.Serialize(paciente);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Paciente>> GetAllPacientes(string token)
        {
            var url = _baseURL + "api/Paciente/GetAllPacientes";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var pacientes = JsonSerializer.Deserialize<List<Paciente>?>(responseBody);
                return pacientes;
            }
            else
            {
                return new List<Paciente>();
            }
        }

        public async Task<Paciente> GetPacienteById(string Id, string token)
        {
            var url = _baseURL + "api/Paciente/GetPacienteById/" + Id;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var paciente = JsonSerializer.Deserialize<Paciente?>(responseBody);
                return paciente;
            }
            else
            {
                return new Paciente();
            }
        }

        public async Task<bool> DeletePaciente(string id, string token)
        {
            try
            {
                var paciente = await GetPacienteById(id, token);
                paciente.Activo = false;
                string url = _baseURL + $"api/Paciente/EditPaciente/{paciente.Id}";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var jsonContent = JsonSerializer.Serialize(paciente);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
