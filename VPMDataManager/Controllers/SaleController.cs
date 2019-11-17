using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.Http;
using VPMDataManager.Library.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {

        [HttpPost]
        public void Post(SaleModel model)
        {
            SaleData data = new SaleData();
            string cashierId = RequestContext.Principal.Identity.GetUserId();
            data.SaveSale(model, cashierId);

        }

        [HttpGet]
        [Route("report")]
        public List<SaleReportModel> GetSalesReport()
        {
            SaleData data = new SaleData();
            return data.GetSalesReport();
        }
    }
}
