using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using VPMDataManager.Library.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {        
        private readonly IInventoryData _inventory;

        public InventoryController(IInventoryData inventory)
        {     
            _inventory = inventory;
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        public List<InventoryModel> Get()
        {
            return _inventory.GetInventory();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Post(InventoryModel item)
        {
            _inventory.SaveInventoryRecord(item);
        }
    }
}