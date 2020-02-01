using System.Collections.Generic;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Library.DataAccess
{
    public interface IInventoryData
    {
        List<InventoryModel> GetInventory();
        void SaveInventoryRecord(InventoryModel item);
    }
}