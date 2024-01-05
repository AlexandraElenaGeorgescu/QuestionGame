using GameAppApi.API.DatabaseSettings;
using GameAppApi.Game.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameAppApi.Game.Services
{
    public class GameService : IGameService
    {
        private readonly IMongoCollection<GameObj> _games;
        private readonly List<IObserver> _observers = new List<IObserver>();
        private readonly QuestionService _questionService;

        public GameService(IMongoDBSettings settings, QuestionService questionService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _games = database.GetCollection<GameObj>(settings.GamesCollectionName);
            _questionService = questionService;
        }

        // ISubject implementation
        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(GameObj game, bool gameEnded)
        {
            foreach (var observer in _observers)
            {
                observer.Update(game, gameEnded);
            }
        }


        // IGameService implementation
        public async Task<List<GameObj>> Get() =>
            await _games.Find(game => true).ToListAsync();

        public async Task<GameObj> Get(string username) =>
            await _games.Find<GameObj>(game => game.Username == username).FirstOrDefaultAsync();

        public async Task<GameObj> Create(GameObj game)
        {
            await _games.InsertOneAsync(game);
            Notify(game, false); // Notify observers when a new game is created
            return game;
        }

        public async Task Update(string id, GameObj gameIn)
        {
            await _games.ReplaceOneAsync(game => game.Id.ToString() == id, gameIn);
            Notify(gameIn, false);  //  notify observers about the update
        }

        public async Task Remove(GameObj gameIn)
        {
            await _games.DeleteOneAsync(game => game.Id == gameIn.Id);
            Notify(gameIn, true);
        }

        public async Task Remove(string id)
        {
            await _games.DeleteOneAsync(game => game.Id.ToString() == id); ;
        }

        public async Task<GameObj> PlayGame(string username, string userAnswer)
        {
            var game = await _games.Find<GameObj>(g => g.Username == username).FirstOrDefaultAsync();

            if (game == null)
            {
                // If the game doesn't exist, create a new one
                game = new GameObj { Username = username, Score = 0, QuestionIndex = 0 };
                await _games.InsertOneAsync(game);
            }

            var currentQuestion = await _questionService.Get(game.CurrentQuestionId);

            bool correctAnswer = currentQuestion.CorrectAnswer.Equals(userAnswer, StringComparison.OrdinalIgnoreCase);

            if (correctAnswer)
            {
                game.Score++; // Increase score
                game.QuestionIndex++; // Move to next question
                await _games.ReplaceOneAsync(g => g.Id == game.Id, game);
            }
            else
            {
                // End the game and notify observers
                Notify(game, true); // gameEnded is true
            }

            return game;
        }
    }
}