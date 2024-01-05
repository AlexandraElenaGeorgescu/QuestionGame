using GameAppApi.Game.Models;
using GameAppApi.Game.Services;
using System;
using System.Threading.Tasks;

namespace GameAppApi.Game.Services
{
    public class ScoreService : IObserver
    {
        private readonly IGameRepository _gameRepository;

        public ScoreService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }
        // Inside ScoreService.cs
        public async void Update(GameObj game, bool gameEnded)
        {
            if (gameEnded)
            {
                var dbGame = await _gameRepository.GetGameByUserAsync(game.Username);
                if (dbGame != null)
                {
                    dbGame.Score = game.Score; // Update the score
                    await _gameRepository.UpdateGameAsync(dbGame);
                    Console.WriteLine($"Game ended. Final score for {dbGame.Username}: {dbGame.Score}");
                }
            }
        }

    }
}
