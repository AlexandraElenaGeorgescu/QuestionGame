using GameAppApi.API.DatabaseSettings;
using GameAppApi.Game.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace GameAppApi.Game.Services
{
    public class GameRepository : IGameRepository
    {
        private readonly IMongoCollection<GameObj> _games;

        public GameRepository(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _games = database.GetCollection<GameObj>(settings.GamesCollectionName);
        }

        public async Task<GameObj> GetGameByUserAsync(string username)
        {
            return await _games.Find<GameObj>(game => game.Username == username).FirstOrDefaultAsync();
        }

        public async Task UpdateGameAsync(GameObj game)
        {
            await _games.ReplaceOneAsync(g => g.Id == game.Id, game);
        }
    }
}
