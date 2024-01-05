using GameAppApi.API.PublicModels;
using GameAppApi.UserAdministration.Services;

public class FakeAdminService : IAdminService
{
    private readonly List<User> _users = new List<User>();

    public Task<List<User>> GetAllUsers()
    {
        return Task.FromResult(_users);
    }

    public Task<User> GetUserById(string id)
    {
        var user = _users.FirstOrDefault(u => u.Id.ToString() == id);
        return Task.FromResult(user);
    }

    public Task Remove(string id)
    {
        _users.RemoveAll(u => u.Id.ToString() == id);
        return Task.CompletedTask;
    }

    // Use this method in tests to setup data.
    public void AddUser(User user)
    {
        _users.Add(user);
    }
}
