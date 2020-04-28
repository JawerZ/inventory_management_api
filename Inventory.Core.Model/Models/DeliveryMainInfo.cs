using System;
using System.Collections.Generic;

namespace Inventory.Core.Model.Models
{
    public partial class DeliveryMainInfo
    {
        public string DeliveryOrderNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Remark { get; set; }
        public int DeliveryMainId { get; set; }
    }
}
