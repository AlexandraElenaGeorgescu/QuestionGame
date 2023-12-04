﻿using GameAppApi.Authentification.DatabaseSettings;
using GameAppApi.Authentification.Models;
using MongoDB.Driver;

namespace GameAppApi.Authentification.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public User GetUserByUsername(string username)
        {
            return _users.Find(user => user.Username == username).FirstOrDefault();
        }

        public async Task<User> CreateUser(User newUser)
        {
            await _users.InsertOneAsync(newUser);
            return newUser;
        }
        public void SeedModerator(string username, string password)
        {
            var moderatorExists = _users.Find(u => u.Role == Role.Moderator).Any();
            if (!moderatorExists)
            {
                _users.InsertOne(new User
                {
                    Username = username,
                    Password = password, 
                    Role = Role.Moderator
                });
            }
        }

    }
}
