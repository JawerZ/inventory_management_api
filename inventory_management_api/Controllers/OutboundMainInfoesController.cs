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

        // GET: api/OutboundMainInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OutboundMainInfo>> GetOutboundMainInfo(string id)
        {
            var outboundMainInfo = await _context.OutboundMainInfo.FindAsync(id);

            if (outboundMainInfo == null)
            {
                return NotFound();
            }

            return outboundMainInfo;
        }

        // PUT: api/OutboundMainInfoes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOutboundMainInfo(string id, OutboundMainInfo outboundMainInfo)
        {
            if (id != outboundMainInfo.OutboundOrderNumber)
            {
                return BadRequest();
            }

            _context.Entry(outboundMainInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OutboundMainInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OutboundMainInfoes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<OutboundMainInfo>> PostOutboundMainInfo(OutboundMainInfo outboundMainInfo)
        {
            _context.OutboundMainInfo.Add(outboundMainInfo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OutboundMainInfoExists(outboundMainInfo.OutboundOrderNumber))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOutboundMainInfo", new { id = outboundMainInfo.OutboundOrderNumber }, outboundMainInfo);
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
