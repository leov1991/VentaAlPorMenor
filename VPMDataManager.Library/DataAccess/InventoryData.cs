using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using VPMDataManager.Library.Internal.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Library.DataAccess
{
    public class InventoryData : IInventoryData
    {        
        private readonly ISQLDataAccess _sql;

        public InventoryData(ISQLDataAccess sql)
        {
     
            _sql = sql;
        }
        public List<InventoryModel> GetInventory()
        {   

            var output = _sql.LoadData<InventoryModel, dynamic>("[dbo].[spInventory_GetAll]", new { }, "VPMData");

            return output;
        }


        public void SaveInventoryRecord(InventoryModel item)
        {            
            _sql.SaveData("dbo.spInventory_Insert", item, "VPMData");
        }
    }
}
