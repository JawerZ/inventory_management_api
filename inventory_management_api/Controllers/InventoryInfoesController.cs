using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inventory_management_api.Models;

namespace inventory_management_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryInfoesController : ControllerBase
    {
        private readonly InventoryContext _context;

        public InventoryInfoesController(InventoryContext context)
        {
            _context = context;
        }

        // GET: api/InventoryInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryInfo>>> GetInventoryInfo()
        {
            return await _context.InventoryInfo.ToListAsync();
        }

        // GET: api/InventoryInfoes/query
        [HttpGet("query")]
        public async Task<ActionResult<List<InventoryInfo>>> GetInventoryInfo(string name,string spect)
        {
            var inventoryInfos = await _context.InventoryInfo.Where(e => e.ProductName == name && e.ProductSpec == spect).ToListAsync();
            if (inventoryInfos == null)
            {
                return NotFound();
            }
            return inventoryInfos;
        }

    
    }
}
