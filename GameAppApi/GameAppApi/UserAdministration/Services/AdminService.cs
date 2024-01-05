using GameAppApi.API.DatabaseSettings;
using GameAppApi.API.PublicModels;
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
            return await _users.Find<User>(user => user.Id.ToString() == id).FirstOrDefaultAsync();
        }

        public async Task Remove(string id)
        {
            await _users.DeleteOneAsync(user => user.Id.ToString() == id);
        }
    }
}
