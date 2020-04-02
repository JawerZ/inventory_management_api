using System;
using System.Collections.Generic;

namespace inventory_management_api.Models
{
    public partial class OutboundMainInfo
    {
        public string OutboundOrderNumber { get; set; }
        public DateTime OutboundDate { get; set; }
        public string Remark { get; set; }
    }
}
