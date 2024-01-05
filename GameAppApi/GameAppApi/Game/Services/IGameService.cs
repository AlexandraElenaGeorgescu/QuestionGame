using GameAppApi.Game.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameAppApi.Game.Services
{
    public interface IGameService
    {
        Task<List<GameObj>> Get();
        Task<GameObj> Get(string username);
        Task<GameObj> Create(GameObj game);
        Task Update(string id, GameObj gameIn);
        Task Remove(GameObj gameIn);
        Task Remove(string id);
        public Task<ActionResult<PlayGameResponse>> PlayGame(string username, string userAnswer);
        public Task<ActionResult> GetNextQuestionForUser(string username);
        public Task<GameObj> StartNewGame(string username);

    }
}
