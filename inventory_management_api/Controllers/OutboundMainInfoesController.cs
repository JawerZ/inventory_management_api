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
    public class OutboundMainInfoesController : ControllerBase
    {
        private readonly InventoryContext _context;

        public OutboundMainInfoesController(InventoryContext context)
        {
            _context = context;
        }

        // GET: api/OutboundMainInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutboundMainInfo>>> GetOutboundMainInfo()
        {
            return await _context.OutboundMainInfo.ToListAsync();
        }

        // DELETE: api/OutboundMainInfoes/5
        [HttpDelete("{orderNumber}")]
        public async Task<ActionResult<OutboundMainInfo>> DeleteOutboundMainInfo(string orderNumber)
        {
            var outboundMainInfoes = _context.OutboundMainInfo.Where(e => e.OutboundOrderNumber == orderNumber).ToList();
            if (outboundMainInfoes == null)
            {
                return NotFound();
            }
            OutboundMainInfo outboundMainInfo = outboundMainInfoes[0];
            _context.OutboundMainInfo.Remove(outboundMainInfo);
            List<OutboundDetailInfo> outboundDetailsInfoes = QueryOutboundDetailInfos(orderNumber);
            _context.OutboundDetailInfo.RemoveRange(outboundDetailsInfoes);
            foreach(OutboundDetailInfo outboundDetailInfo in outboundDetailsInfoes)
            {
                InventoryInfo inventoryInfo = QueryInventoryInfo(outboundDetailInfo.ProductName, outboundDetailInfo.ProductSpec);
                inventoryInfo.Count += outboundDetailInfo.Count;
                _context.Entry(inventoryInfo).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return outboundMainInfo;
        }

        private bool OutboundMainInfoExists(string id)
        {
            return _context.OutboundMainInfo.Any(e => e.OutboundOrderNumber == id);
        }
        private List<OutboundDetailInfo> QueryOutboundDetailInfos(string orderNumber)
        {
            return _context.OutboundDetailInfo.Where(e => e.OrderNumber == orderNumber).ToList();
        }
        private InventoryInfo QueryInventoryInfo(string name, string spect)
        {
            return _context.InventoryInfo.Where(e => e.ProductName == name && e.ProductSpec == spect).ToList()[0];
        }
    }
}
