using System.Collections.Generic;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Library.DataAccess
{
    public interface IUserData
    {
        List<UserModel> GetUserById(string id);
    }
}