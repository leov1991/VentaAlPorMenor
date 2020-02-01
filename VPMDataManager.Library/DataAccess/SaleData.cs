using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using VPMDataManager.Library.Internal.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Library.DataAccess
{
    public class SaleData : ISaleData
    {
        private readonly IProductData _productData;
        private readonly ISQLDataAccess _sql;

        public SaleData(IProductData productData, ISQLDataAccess sql)
        {

            _productData = productData;
            _sql = sql;
        }
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {

            List<SaleDetailDbModel> details = new List<SaleDetailDbModel>();
            var taxRate = ConfigHelper.GetTaxRate() / 100;
            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDbModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                var productInfo = _productData.GetProductById(item.ProductId);

                if (productInfo == null)
                    throw new System.Exception($"El producto con Id {item.ProductId} no se encuentra en la base de datos.");

                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;

                if (productInfo.IsTaxable)
                {
                    detail.Tax = detail.PurchasePrice * taxRate;
                }

                details.Add(detail);
            }

            SaleDbModel sale = new SaleDbModel
            {
                Subtotal = details.Sum(d => d.PurchasePrice),
                Tax = details.Sum(d => d.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.Subtotal + sale.Tax;


            try
            {
                _sql.StartTransaction("VPMData");
                _sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                var p = new { sale.CashierId, sale.SaleDate };

                sale.Id = _sql.LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup", p).FirstOrDefault();

                foreach (var item in details)
                {
                    item.SaleId = sale.Id;

                    _sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                }

                _sql.CommitTransaction();
            }
            catch
            {
                _sql.RollbackTransaction();
                throw;
            }






        }

        public List<SaleReportModel> GetSalesReport()
        {
            var output = _sql.LoadData<SaleReportModel, dynamic>("[dbo].[spSale_SaleReport]", new { }, "VPMData");

            return output;
        }

    }
}
