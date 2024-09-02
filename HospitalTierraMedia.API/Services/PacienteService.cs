using HospitalTierraMedia.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HospitalTierraMedia.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IMongoCollection<Paciente> _pacienteCollection;
        private readonly IOptions<DataBaseSettings> _dbSettings;

        public PacienteService(IOptions<DataBaseSettings> dbSettings)
        {
            _dbSettings = dbSettings;
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(dbSettings.Value.DataBaseName);
            _pacienteCollection = mongoDataBase.GetCollection<Paciente>(dbSettings.Value.PacientesCollectionName);
        }

        public async Task<List<Paciente>> GetAllAsync() =>
            await _pacienteCollection.Find(_ => true).ToListAsync();

        public async Task<Paciente> GetById(string id) =>
            await _pacienteCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Paciente paciente)
        {
            if (string.IsNullOrEmpty(paciente.Id))
            {
                paciente.Id = ObjectId.GenerateNewId().ToString(); // Genera un nuevo ObjectId como cadena
            }

            await _pacienteCollection.InsertOneAsync(paciente);
        }

        public async Task UpdateAsync(string id, Paciente paciente) =>
            await _pacienteCollection.ReplaceOneAsync(x => x.Id == id, paciente);

        public async Task DeleteAsync(string id)
        {
            var paciente = await _pacienteCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (paciente != null)
            {
                paciente.Activo = false;
                await _pacienteCollection.ReplaceOneAsync(x => x.Id == id, paciente);
            }
        }
    }
}
