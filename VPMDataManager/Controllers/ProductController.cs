using System.Collections.Generic;
using System.Web.Http;
using VPMDataManager.Library.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Controllers
{
    [Authorize]
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {
        public List<ProductModel> Get()
        {
            ProductData data = new ProductData();
            return data.GetProducts();
        }

    }
}
