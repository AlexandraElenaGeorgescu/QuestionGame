using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameAppApi.Game.Models
{
    public class GameObj
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public int CurrentQuestionIndex { get; set; }

    }
}
