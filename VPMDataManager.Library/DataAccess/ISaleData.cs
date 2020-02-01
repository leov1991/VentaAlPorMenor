using System.Collections.Generic;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Library.DataAccess
{
    public interface ISaleData
    {
        List<SaleReportModel> GetSalesReport();
        void SaveSale(SaleModel saleInfo, string cashierId);
    }
}