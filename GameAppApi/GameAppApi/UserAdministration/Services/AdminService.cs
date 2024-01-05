using GameAppApi.API.DatabaseSettings;
using GameAppApi.API.PublicModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameAppApi.UserAdministration.Services
{
    public class AdminService: IAdminService
    {
        private readonly IMongoCollection<User> _users;

        public AdminService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await _users.Find(user => true).ToListAsync();
        }
        public async Task<User> GetUserById(string id)
        {
            if (Guid.TryParse(id, out Guid guidId))
            {
                // Convert the Guid to MongoDB Bson binary data
                var filter = Builders<User>.Filter.Eq(u => u.Id, guidId);
                return await _users.Find(filter).FirstOrDefaultAsync();
            }
            else
            {
                throw new ArgumentException("Invalid GUID format.");
            }
        }

        public async Task Remove(string id)
        {
            if (Guid.TryParse(id, out Guid guidId))
            {
                // Convert the Guid to MongoDB Bson binary data
                var filter = Builders<User>.Filter.Eq(u => u.Id, guidId);
                await _users.DeleteOneAsync(filter);
            }
            else
            {
                throw new ArgumentException("Invalid GUID format.");
            }
        }


    }
}
