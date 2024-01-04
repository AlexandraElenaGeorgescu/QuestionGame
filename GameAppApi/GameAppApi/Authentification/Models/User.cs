﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace GameAppApi.Authentification.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
        public string Role { get; set; } // "Moderator" or "Player"
    }

}
