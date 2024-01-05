using GameAppApi.Game.Models;

namespace GameAppApi.Game.Services
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(GameObj game);
    }
}
