using System;
using System.Collections.Generic;

namespace Inventory.Core.Model.Models
{
    public partial class InventoryInfo
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductSpec { get; set; }
        public int Count { get; set; }
    }
}
