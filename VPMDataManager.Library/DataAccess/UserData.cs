using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using VPMDataManager.Library.Internal.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Library.DataAccess
{
    public class UserData
    {
        private readonly IConfiguration _config;

        public UserData(IConfiguration config)
        {
            _config = config;
        }
        public List<UserModel> GetUserById(string id)
        {
            SQLDataAccess sql = new SQLDataAccess(_config);

            var p = new { Id = id };

            var output = sql.LoadData<UserModel, dynamic>("[dbo].[spUserLookup]", p, "VPMData");

            return output;
        }
    }
}
