using System;
using System.Collections.Generic;

namespace Inventory.Core.Model.Models
{
    public partial class OutboundMainInfo
    {
        public string OutboundOrderNumber { get; set; }
        public DateTime OutboundDate { get; set; }
        public string Remark { get; set; }
        public int OutboundMainId { get; set; }
    }
}
