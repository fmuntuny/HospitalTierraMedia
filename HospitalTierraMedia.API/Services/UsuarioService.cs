using HospitalTierraMedia.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HospitalTierraMedia.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IMongoCollection<Usuario> _usuarioCollection;
        private readonly IOptions<DataBaseSettings> _dbSettings;

        public UsuarioService(IOptions<DataBaseSettings> dbSettings)
        {
            _dbSettings = dbSettings;
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(dbSettings.Value.DataBaseName);
            _usuarioCollection = mongoDataBase.GetCollection<Usuario>(dbSettings.Value.UsuariosCollectionName);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync() =>
            await _usuarioCollection.Find(_ => true).ToListAsync();

        public async Task<Usuario> GetById(string id) =>
            await _usuarioCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Usuario usuario) =>
            await _usuarioCollection.InsertOneAsync(usuario);

        public async Task UpdateAsync(string id, Usuario usuario) =>
            await _usuarioCollection.ReplaceOneAsync(x => x.Id == id, usuario);

        public async Task<Usuario> GetByEmail(string email) =>
            await _usuarioCollection.Find(x => x.Email == email).FirstOrDefaultAsync();

        /*public async Task DeleteAsync(string id)
        {
            var usuario = await _usuarioCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (usuario != null)
            {
                usuario.Activo = false;
                await _usuarioCollection.ReplaceOneAsync(x => x.Id == id, usuario);
            }
        }*/
    }
}
