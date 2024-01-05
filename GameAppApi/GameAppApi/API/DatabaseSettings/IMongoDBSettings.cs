namespace GameAppApi.API.DatabaseSettings
{
    public interface IMongoDBSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string GamesCollectionName { get; set; }
        string UsersCollectionName { get; set; }
    }
}
