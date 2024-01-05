using GameAppApi.Game.Models;
using System.Threading.Tasks;

namespace GameAppApi.Game.Services
{
    public interface IGameRepository
    {
        Task<GameObj> GetGameByUserAsync(string username);
        Task UpdateGameAsync(GameObj game);
    }
}