using System;
using System.Collections.Generic;

namespace inventory_management_api.Models
{
    public partial class DeliveryMainInfo
    {
        public string DeliveryOrderNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Remark { get; set; }
    }
}
