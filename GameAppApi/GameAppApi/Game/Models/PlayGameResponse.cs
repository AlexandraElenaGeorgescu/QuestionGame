namespace GameAppApi.Game.Models
{
    public class PlayGameResponse
    {
        public GameObj Game { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public string CorrectAnswer { get; set; }
    }

}
