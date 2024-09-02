using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HospitalTierraMedia.Models
{
    public class PacienteViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Apellido es obligatorio.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo Dirección es obligatorio.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo DNI es obligatorio.")]
        [RegularExpression(@"\d{8}", ErrorMessage = "El DNI debe tener 8 dígitos.")]
        public int DNI { get; set; }

        public bool Activo { get; set; }
    }
}
