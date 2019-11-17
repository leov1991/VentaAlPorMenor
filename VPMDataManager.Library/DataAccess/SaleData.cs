using System.Collections.Generic;
using System.Linq;
using VPMDataManager.Library.Internal.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Library.DataAccess
{
    public class SaleData
    {

        public void SaveSale(SaleModel saleInfo, string cashierId)
        {

            List<SaleDetailDbModel> details = new List<SaleDetailDbModel>();
            ProductData products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate() / 100;
            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDbModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                var productInfo = products.GetProductById(item.ProductId);

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

            using (SQLDataAccess sql = new SQLDataAccess())
            {
                try
                {
                    sql.StartTransaction("VPMData");
                    sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                    var p = new { sale.CashierId, sale.SaleDate };

                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup", p).FirstOrDefault();

                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;

                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }

                    // Si todo salió bien va a ejecutar el commit al final del using
                }
                catch
                {
                    sql.RollbackTransaction();
                    throw;
                }

            }




        }

        //public List<ProductModel> GetProducts()
        //{


        //    var output = sql.LoadData<ProductModel, dynamic>("[dbo].[spProduct_GetAll]", new { }, "VPMData");

        //    return output;
        //}
    }
}
