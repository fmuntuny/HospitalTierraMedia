using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HospitalTierraMedia.Models
{
    public class Paciente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public int DNI { get; set; }
        public bool Activo { get; set; }
    }
}
