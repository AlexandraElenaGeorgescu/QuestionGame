﻿namespace GameAppApi.API.DatabaseSettings
{
    public class MongoDBSettings : IMongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string GamesCollectionName { get; set; }
        public string UsersCollectionName { get; set; }

    }
}
