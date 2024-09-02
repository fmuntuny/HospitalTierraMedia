using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HospitalTierraMedia.Models;

namespace HospitalTierraMedia.API.Models.ViewModels
{
    public class UsuarioInicioSesionViewModel
    {
        public string Mensaje { get; set; }
        public List<Paciente> Pacientes { get; set; }
        public Paciente Paciente { get; set; }
    }
}
