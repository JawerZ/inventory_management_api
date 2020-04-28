using System;
using System.Collections.Generic;

namespace Inventory.Core.Model.Models
{
    public partial class DeliveryDetailInfo
    {
        public string ProductName { get; set; }
        public string ProductSpec { get; set; }
        public int Count { get; set; }
        public string OrderNumber { get; set; }
        public int DeliveryDetailId { get; set; }
    }
}
