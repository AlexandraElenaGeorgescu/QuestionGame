using GameAppApi.Game.Models;
using GameAppApi.Game.Services;

public class ScoreObserver : IObserver
{
    public void Update(GameObj game, bool gameEnded)
    {
        if (gameEnded)
        {
            // Implement the logic for when the game ends and the score needs to be updated.
            Console.WriteLine($"Game ended. Final score for {game.Username}: {game.Score}");
        }
    }
}
