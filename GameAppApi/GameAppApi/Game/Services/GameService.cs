using GameAppApi.API.DatabaseSettings;
using GameAppApi.Game.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameAppApi.Game.Services
{
    public class GameService : IGameService
    {
        private readonly IMongoCollection<GameObj> _games;
        private readonly List<IObserver> _observers = new List<IObserver>();
        private readonly IQuestionService _questionService;

        public GameService(IMongoDBSettings settings, IQuestionService questionService)
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
        public async Task<GameObj> EnsureGameExists(string username)
        {
            var game = await _games.Find<GameObj>(g => g.Username == username).FirstOrDefaultAsync();
            if (game == null)
            {
                game = new GameObj { Username = username, Score = 0, CurrentQuestionIndex = 0 };
                await _games.InsertOneAsync(game);
            }
            return game;
        }

        public async Task<ActionResult<PlayGameResponse>> PlayGame(string username, string userAnswer)
        {
            var game = await EnsureGameExists(username);
            var currentQuestion = await _questionService.GetQuestionByIndex(game.CurrentQuestionIndex);
            var isCorrect = false; // Default to false

            if (currentQuestion == null)
            {
                var lastQuestion = await _questionService.GetQuestionByIndex(game.CurrentQuestionIndex - 1);
                Notify(game, true); // No more questions, game ends.
                return new PlayGameResponse { Game = game, IsCorrectAnswer = isCorrect, CorrectAnswer = lastQuestion.CorrectAnswer };
            }

            if (currentQuestion.CorrectAnswer.Equals(userAnswer, StringComparison.OrdinalIgnoreCase))
            {
                game.Score++; // Increase score
                game.CurrentQuestionIndex++; // Move to next question
                isCorrect = true;
                await _games.ReplaceOneAsync(g => g.Id == game.Id, game);
            }
            else
            {
                Notify(game, true); // Wrong answer, game ends.
            }

            await _games.ReplaceOneAsync(g => g.Id == game.Id, game); // Update the game state
            return new PlayGameResponse { Game = game, IsCorrectAnswer = isCorrect };
        }
        public async Task<GameObj> StartNewGame(string username)
        {
            var existingGame = await _games.Find<GameObj>(g => g.Username == username).FirstOrDefaultAsync();

            if (existingGame != null)
            {
                // Reset the score and question index for the existing game
                existingGame.Score = 0;
                existingGame.CurrentQuestionIndex = 0;

                // Update the existing game in the database
                await _games.ReplaceOneAsync(g => g.Id == existingGame.Id, existingGame);

                // Notify observers that a new game has started
                Notify(existingGame, false);

                return existingGame;
            }
            else
            {
                // Create a new game object for the user
                var newGame = new GameObj
                {
                    Username = username,
                    Score = 0,
                    CurrentQuestionIndex = 0,
                };

                // Insert the new game into the database
                await _games.InsertOneAsync(newGame);

                // Notify observers that a new game has started
                Notify(newGame, false);

                return newGame;
            }
        }
        public async Task<ActionResult> GetNextQuestionForUser(string username)
        {
            var game = await EnsureGameExists(username);

            var totalQuestions = await _questionService.CountQuestions();
            if (game.CurrentQuestionIndex >= totalQuestions)
            {
                // No more questions left to answer, player has won the game.
                return new JsonResult(new { Message = "Congratulations! You've answered all questions correctly and won the game!" });
            }

            var nextQuestion = await _questionService.GetQuestionByIndex(game.CurrentQuestionIndex);

            return new JsonResult(nextQuestion);
        }

    }
}