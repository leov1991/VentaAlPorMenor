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
        private readonly ISaleData _saleData;

        public SaleController(ISaleData saleData)
        {            
            _saleData = saleData;
        }

        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public void Post(SaleModel model)
        {   
            string cashierId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _saleData.SaveSale(model, cashierId);

        }

        [HttpGet]
        [Route("report")]
        [Authorize(Roles = "Admin,Supervisor")]
        public List<SaleReportModel> GetSalesReport()
        {            
            return _saleData.GetSalesReport();
        }
    }
}