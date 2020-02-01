using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using VPMDataManager.Library.Internal.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Library.DataAccess
{
    public class ProductData : IProductData
    {        
        private readonly ISQLDataAccess _sql;

        public ProductData(ISQLDataAccess sql)
        {            
            _sql = sql;
        }
        public List<ProductModel> GetProducts()
        {
            var output = _sql.LoadData<ProductModel, dynamic>("[dbo].[spProduct_GetAll]", new { }, "VPMData");

            return output;
        }

        public ProductModel GetProductById(int productId)
        {            

            var output = _sql.LoadData<ProductModel, dynamic>("[dbo].[spProduct_GetById]", new { Id = productId }, "VPMData").FirstOrDefault();

            return output;
        }
    }
}
