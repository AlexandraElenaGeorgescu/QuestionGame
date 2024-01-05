// IAdminService.cs

using GameAppApi.API.PublicModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameAppApi.UserAdministration.Services
{
    public interface IAdminService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(string id);
        Task Remove(string id);
    }
}
