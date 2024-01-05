using GameAppApi.API.PublicModels;

public interface IUserService
{
    User GetUserByUsername(string username);
    Task<User> CreateUser(User newUser);
    void SeedModerator(string username, string password);
}
