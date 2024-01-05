using GameAppApi.Game.Models;
using GameAppApi.Game.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameAppApi.Game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("score/{username}")]
        public async Task<ActionResult<GameObj>> GetScore(string username)
        {
            var game = await _gameService.Get(username);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        [HttpPost("submit-score")]
        public async Task<ActionResult<GameObj>> SubmitScore(GameObj game)
        {
            await _gameService.Create(game);
            return CreatedAtRoute("GetGame", new { id = game.Id.ToString() }, game);
        }

        [HttpPost("play")]
        public async Task<ActionResult<GameObj>> PlayGame([FromBody] dynamic payload)
        {
            string username = payload.username;
            string userAnswer = payload.answer; // Assuming the answer is passed in the payload

            var game = await _gameService.PlayGame(username, userAnswer);
            return Ok(game);
        }
    }
}
