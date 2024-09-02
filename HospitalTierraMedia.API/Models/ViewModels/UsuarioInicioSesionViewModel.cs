using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HospitalTierraMedia.API.Models.ViewModels
{
    public class UsuarioInicioSesionViewModel
    {
        public string Email { get; set; }
        public string Contrasena { get; set; }
    }
}
