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
    public class DeliveryDetailInfoesController : ControllerBase
    {
        private readonly InventoryContext _context;

        public DeliveryDetailInfoesController(InventoryContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryDetailInfoes
        [HttpGet("{orderNumber}")]
        public async Task<ActionResult<IEnumerable<DeliveryDetailInfo>>> GetDeliveryDetailInfo(string orderNumber)
        {
            return await _context.DeliveryDetailInfo.Where(e=>e.OrderNumber == orderNumber).ToListAsync();
        }

        // POST: api/DeliveryDetailInfoes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<List <DeliveryDetailInfo>>> PostDeliveryDetailInfo(string orderNumber,DateTime date,string remark, List<DeliveryDetailInfo> deliveryDetailInfos )
        {
            //string orderNumber = deliveryDetailInfos[0].OrderNumber;
            DeliveryMainInfo deliveryMainInfo = new DeliveryMainInfo();
            deliveryMainInfo.DeliveryDate = date;
            deliveryMainInfo.DeliveryOrderNumber = orderNumber;
            deliveryMainInfo.Remark = remark;
            if (DeliveryMainInfoExists(orderNumber))
            {
                _context.Entry(deliveryMainInfo).State = EntityState.Modified;
            }
            else
            {
                _context.DeliveryMainInfo.Add(deliveryMainInfo);
                await _context.SaveChangesAsync();
            }
            var oldDeliveryDetailInfos = await _context.DeliveryDetailInfo.Where(e=>e.OrderNumber == orderNumber).ToListAsync();
            _context.DeliveryDetailInfo.RemoveRange(oldDeliveryDetailInfos);
            foreach(DeliveryDetailInfo oldDeliveryDetailInfo in oldDeliveryDetailInfos)
            {
                InventoryInfo inventoryInfo = QueryInventoryInfo(oldDeliveryDetailInfo.ProductName, oldDeliveryDetailInfo.ProductSpec)[0];
                inventoryInfo.Count -= oldDeliveryDetailInfo.Count;
                _context.Entry(inventoryInfo).State = EntityState.Modified;
            }
            _context.DeliveryDetailInfo.AddRange(deliveryDetailInfos);
            List<int> ids = new List<int>();
            foreach (DeliveryDetailInfo deliveryDetailInfo in deliveryDetailInfos)
            {
                ids.Add(deliveryDetailInfo.DetailId);
                if (InventoryInfoExists(deliveryDetailInfo.ProductName,deliveryDetailInfo.ProductSpec))
                {
                        InventoryInfo inventoryInfo = QueryInventoryInfo(deliveryDetailInfo.ProductName, deliveryDetailInfo.ProductSpec)[0];
                        inventoryInfo.Count += deliveryDetailInfo.Count ;
                        _context.Entry(inventoryInfo).State = EntityState.Modified;
                }
                else
                { 
                        InventoryInfo inventoryInfo = new InventoryInfo();
                        inventoryInfo.Count = deliveryDetailInfo.Count;
                        inventoryInfo.ProductName = deliveryDetailInfo.ProductName;
                        inventoryInfo.ProductSpec = deliveryDetailInfo.ProductSpec;
                        _context.InventoryInfo.Add(inventoryInfo);
                }
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetDeliveryDetailInfo",ids,deliveryDetailInfos);
        }

        // DELETE: api/DeliveryDetailInfoes/5
        private bool InventoryInfoExists(string name, string spec)
        {
            return _context.InventoryInfo.Any(e => e.ProductName == name && e.ProductSpec == spec);
        }
        private bool DeliveryMainInfoExists(string orderNumber)
        {
            return _context.DeliveryMainInfo.Any(e => e.DeliveryOrderNumber == orderNumber);
        }
        private  List<DeliveryDetailInfo> QueryDeliveryDetailInfo(string name, string spec ,string orderNumber)
        {
            return _context.DeliveryDetailInfo.Where(e => e.ProductName == name && e.ProductSpec == spec && e.OrderNumber == orderNumber).ToList();
        }
        private List<InventoryInfo> QueryInventoryInfo(string name, string spec)
        {
            return _context.InventoryInfo.Where(e => e.ProductName == name && e.ProductSpec == spec).ToList();
        }
    }
}
