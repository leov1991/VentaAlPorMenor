using System.Collections.Generic;
using System.Threading.Tasks;
using VPMDesktopUI.Library.Models;

namespace VPMDesktopUI.Library.API
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAllUsers();
    }
}