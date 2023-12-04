using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace GameAppApi.Authentification.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // This should be hashed
        public string Role { get; set; } // "Moderator" or "Player"
    }

}
