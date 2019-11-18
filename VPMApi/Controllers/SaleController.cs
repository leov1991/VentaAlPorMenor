using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using VPMDataManager.Library.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly IConfiguration _config;

        public SaleController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public void Post(SaleModel model)
        {
            SaleData data = new SaleData(_config);
            string cashierId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            data.SaveSale(model, cashierId);

        }

        [HttpGet]
        [Route("report")]
        [Authorize(Roles = "Admin,Supervisor")]
        public List<SaleReportModel> GetSalesReport()
        {
            SaleData data = new SaleData(_config);
            return data.GetSalesReport();
        }
    }
}