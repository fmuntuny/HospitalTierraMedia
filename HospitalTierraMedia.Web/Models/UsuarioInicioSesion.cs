using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HospitalTierraMedia.Web.Models
{
    public class UsuarioInicioSesion
    {
        public string Email { get; set; }
        public string Contrasena { get; set; }
    }
}
