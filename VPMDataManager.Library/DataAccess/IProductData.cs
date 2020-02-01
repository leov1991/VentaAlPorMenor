using System.Collections.Generic;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Library.DataAccess
{
    public interface IProductData
    {
        ProductModel GetProductById(int productId);
        List<ProductModel> GetProducts();
    }
}