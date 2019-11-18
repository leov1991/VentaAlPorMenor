using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using VPMDataManager.Library.Internal.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Library.DataAccess
{
    public class InventoryData
    {
        private readonly IConfiguration _config;

        public InventoryData(IConfiguration config)
        {
            _config = config;
        }
        public List<InventoryModel> GetInventory()
        {
            SQLDataAccess sql = new SQLDataAccess(_config);

            var output = sql.LoadData<InventoryModel, dynamic>("[dbo].[spInventory_GetAll]", new { }, "VPMData");

            return output;
        }


        public void SaveInventoryRecord(InventoryModel item)
        {
            SQLDataAccess sql = new SQLDataAccess(_config);

            sql.SaveData("dbo.spInventory_Insert", item, "VPMData");
        }
    }
}
