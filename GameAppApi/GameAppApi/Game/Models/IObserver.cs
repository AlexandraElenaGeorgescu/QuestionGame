namespace GameAppApi.Game.Models
{
    public interface IObserver
    {
        void Update(GameObj game, bool gameEnded);
    }
}
