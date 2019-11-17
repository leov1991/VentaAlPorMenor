﻿using System.Collections.Generic;
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

            SQLDataAccess sql = new SQLDataAccess();

            sql.SaveData("dbo.spSale_Insert", sale, "VPMData");

            var p = new { sale.CashierId, sale.SaleDate };

            sale.Id = sql.LoadData<int, dynamic>("dbo.spSale_Lookup", p, "VPMData").FirstOrDefault();

            foreach (var item in details)
            {
                item.SaleId = sale.Id;

                sql.SaveData("dbo.spSaleDetail_Insert", item, "VPMData");
            }


        }

        //public List<ProductModel> GetProducts()
        //{


        //    var output = sql.LoadData<ProductModel, dynamic>("[dbo].[spProduct_GetAll]", new { }, "VPMData");

        //    return output;
        //}
    }
}
