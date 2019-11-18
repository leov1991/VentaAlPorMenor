using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using VPMDataManager.Library.Internal.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Library.DataAccess
{
    public class ProductData
    {
        private readonly IConfiguration _config;

        public ProductData(IConfiguration config)
        {
            _config = config;
        }
        public List<ProductModel> GetProducts()
        {
            SQLDataAccess sql = new SQLDataAccess(_config);

            var output = sql.LoadData<ProductModel, dynamic>("[dbo].[spProduct_GetAll]", new { }, "VPMData");

            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            SQLDataAccess sql = new SQLDataAccess(_config);

            var output = sql.LoadData<ProductModel, dynamic>("[dbo].[spProduct_GetById]", new { Id = productId }, "VPMData").FirstOrDefault();

            return output;
        }
    }
}
