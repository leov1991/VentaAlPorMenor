using System.Collections.Generic;
using System.Threading.Tasks;
using VPMDesktopUI.Library.Models;

namespace VPMDesktopUI.Library.API
{
    public interface IUserEndpoint
    {
        Task AddUserToRole(string userId, string roleName);
        Task<Dictionary<string, string>> GetAllRoles();
        Task<List<UserModel>> GetAllUsers();
        Task RemoveUserFromRole(string userId, string roleName);
    }
}