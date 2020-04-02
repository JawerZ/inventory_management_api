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
    public class DeliveryMainInfoesController : ControllerBase
    {
        private readonly InventoryContext _context;

        public DeliveryMainInfoesController(InventoryContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryMainInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryMainInfo>>> GetDeliveryMainInfo()
        {
            return await _context.DeliveryMainInfo.ToListAsync();
        }
        //PUT:api/DeliveryMainInfoes/00003
        [HttpPut("{orderNumber}")]
        public async Task<ActionResult<IEnumerable<DeliveryDetailInfo>>> PutDeliveryMainInfo(string orderNumber, DeliveryMainInfo deliveryMainInfo)
        {
            if (orderNumber != deliveryMainInfo.DeliveryOrderNumber)
            {
                return BadRequest();
            }
            _context.Entry(deliveryMainInfo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryMainInfoExists(orderNumber))
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
        [HttpPost]
        public async Task<ActionResult<IEnumerable<DeliveryMainInfo>>> PostDeliveryMainInfo(DeliveryMainInfo deliveryMainInfo)
        {
             _context.DeliveryMainInfo.Add(deliveryMainInfo);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDeliveryMainInfo",deliveryMainInfo);
        }
        private bool DeliveryMainInfoExists(string orderNumber)
        {
            return _context.DeliveryMainInfo.Any(e => e.DeliveryOrderNumber == orderNumber);
        }
        [HttpDelete("{orderNumber}")]
        public async Task<ActionResult<IEnumerable<DeliveryMainInfo>>> DeleteDeliveryMainInfo(string orderNumber)
        {
            var deliveryMainInfos = _context.DeliveryMainInfo.Where(e => e.DeliveryOrderNumber == orderNumber).ToList();
            DeliveryMainInfo deliveryMainInfo = deliveryMainInfos[0];
            if(deliveryMainInfo == null)
            {
                return NotFound();
            }
            _context.DeliveryMainInfo.Remove(deliveryMainInfo);
            List<DeliveryDetailInfo> deliveryDetailInfos = QueryDeliveryDetailInfos(deliveryMainInfo.DeliveryOrderNumber);
            _context.DeliveryDetailInfo.RemoveRange(deliveryDetailInfos);
            foreach(DeliveryDetailInfo deliveryDetailInfo in deliveryDetailInfos)
            {
                InventoryInfo inventoryInfo = QueryInventoryInfo(deliveryDetailInfo.ProductName,deliveryDetailInfo.ProductSpec);
                inventoryInfo.Count -= deliveryDetailInfo.Count;
                _context.Entry(inventoryInfo).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return deliveryMainInfos;
        }
        private List<DeliveryDetailInfo>QueryDeliveryDetailInfos(string orderNumber)
        {
            return _context.DeliveryDetailInfo.Where(e => e.OrderNumber == orderNumber).ToList();
        }
        private InventoryInfo QueryInventoryInfo(string name,string spec)
        {
            return _context.InventoryInfo.Where(e => e.ProductName == name && e.ProductSpec == spec).ToList()[0];
        }
    }
}
