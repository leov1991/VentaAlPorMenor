using System.Collections.Generic;
using VPMDataManager.Library.Internal.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string id)
        {
            SQLDataAccess sql = new SQLDataAccess();

            var p = new { Id = id };

            var output = sql.LoadData<UserModel, dynamic>("[dbo].[spUserLookup]", p, "VPMData");

            return output;
        }
    }
}
