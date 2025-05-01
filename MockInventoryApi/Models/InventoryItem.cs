using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAndroidApp.MockInventoryApi.Models
{
    public class InventoryItem
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int CurrentQuantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
