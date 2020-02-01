using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using VPMDataManager.Library.Internal.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Library.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISQLDataAccess _sql;

        public UserData(IConfiguration config, ISQLDataAccess sql)
        {
            _sql = sql;
        }
        public List<UserModel> GetUserById(string id)
        {
            var output = _sql.LoadData<UserModel, dynamic>("[dbo].[spUserLookup]", new { Id = id }, "VPMData");

            return output;
        }
    }
}
