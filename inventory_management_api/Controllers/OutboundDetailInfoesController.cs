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
    public class OutboundDetailInfoesController : ControllerBase
    {
        private readonly InventoryContext _context;

        public OutboundDetailInfoesController(InventoryContext context)
        {
            _context = context;
        }

        // GET: api/OutboundDetailInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutboundDetailInfo>>> GetOutboundDetailInfo()
        {
            return await _context.OutboundDetailInfo.ToListAsync();
        }

        // GET: api/OutboundDetailInfoes/5
        [HttpGet("query")]
        public async Task<ActionResult<OutboundDetailInfo>> GetOutboundDetailInfo(int id)
        {
            var outboundDetailInfo = await _context.OutboundDetailInfo.FindAsync(id);

            if (outboundDetailInfo == null)
            {
                return NotFound();
            }

            return outboundDetailInfo;
        }

        // PUT: api/OutboundDetailInfoes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOutboundDetailInfo(int id, OutboundDetailInfo outboundDetailInfo)
        {
            if (id != outboundDetailInfo.DetailId)
            {
                return BadRequest();
            }

            _context.Entry(outboundDetailInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OutboundDetailInfoExists(id))
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

        // POST: api/OutboundDetailInfoes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<List<OutboundDetailInfo>>> PostOutboundDetailInfo(string orderNumber,DateTime date,string remark,List<OutboundDetailInfo> outboundDetailInfoes)
        {
            OutboundMainInfo outboundMainInfo = new OutboundMainInfo();
            outboundMainInfo.OutboundOrderNumber = orderNumber;
            outboundMainInfo.OutboundDate = date;
            outboundMainInfo.Remark = remark;
            if (OutboundMainInfoExists(orderNumber))
            {
                _context.Entry(outboundMainInfo).State = EntityState.Modified;
            }
            else
            {
                _context.OutboundMainInfo.Add(outboundMainInfo);
                await _context.SaveChangesAsync();
            }
            var oldOutboundDetailInfoes = _context.OutboundDetailInfo.Where(e => e.OrderNumber == orderNumber).ToList();
            _context.OutboundDetailInfo.RemoveRange(oldOutboundDetailInfoes);
            foreach(OutboundDetailInfo oldOutboundDetailInfo in oldOutboundDetailInfoes)
            {
                InventoryInfo inventoryInfo = QueryInventoryInfo(oldOutboundDetailInfo.ProductName, oldOutboundDetailInfo.ProductSpec);
                inventoryInfo.Count += oldOutboundDetailInfo.Count;
                _context.Entry(inventoryInfo).State = EntityState.Modified;
            }
            _context.OutboundDetailInfo.AddRange(outboundDetailInfoes);
            List<int> ids = new List<int>();
            foreach(OutboundDetailInfo outboundDetailInfo in outboundDetailInfoes)
            {
                ids.Add(outboundDetailInfo.DetailId);
                if (InventoryInfoExists(outboundDetailInfo.ProductName, outboundDetailInfo.ProductSpec))
                {
                    InventoryInfo inventoryInfo = QueryInventoryInfo(outboundDetailInfo.ProductName, outboundDetailInfo.ProductSpec);
                    inventoryInfo.Count -= outboundDetailInfo.Count;
                    _context.Entry(inventoryInfo).State = EntityState.Modified;
                }
                else
                {
                    return NotFound();
                }
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetOutboundDetailInfo",ids , outboundDetailInfoes);
        }

        // DELETE: api/OutboundDetailInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OutboundDetailInfo>> DeleteOutboundDetailInfo(int id)
        {
            var outboundDetailInfo = await _context.OutboundDetailInfo.FindAsync(id);
            if (outboundDetailInfo == null)
            {
                return NotFound();
            }

            _context.OutboundDetailInfo.Remove(outboundDetailInfo);
            await _context.SaveChangesAsync();

            return outboundDetailInfo;
        }
        private bool OutboundMainInfoExists(string orderNumber)
        {
            return _context.OutboundMainInfo.Any(e => e.OutboundOrderNumber == orderNumber);
        }
        private bool InventoryInfoExists(string name,string spec)
        {
            return _context.InventoryInfo.Any(e => e.ProductName == name && e.ProductSpec == spec);
        }
        private bool OutboundDetailInfoExists(int id)
        {
            return _context.OutboundDetailInfo.Any(e => e.DetailId == id);
        }
        private InventoryInfo QueryInventoryInfo(string name, string spec)
        {
            return _context.InventoryInfo.Where(e => e.ProductName == name && e.ProductSpec == spec).ToList()[0];
        }
        private OutboundMainInfo QueryOutboundMainInfo(string orderNumber)
        {
            return _context.OutboundMainInfo.Where(e => e.OutboundOrderNumber == orderNumber).ToList()[0];
        }
    }
}
