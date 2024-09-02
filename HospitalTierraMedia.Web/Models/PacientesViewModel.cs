using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HospitalTierraMedia.Models
{
    public class PacientesViewModel
    {
        public List<Paciente>? Pacientes { get; set; }
    }
}
