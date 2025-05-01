using InventoryAndroidApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAndroidApp.Services
{
    public interface IInventoryApiService
    {
        Task<List<InventoryItem>> GetAllItemsAsync();
        Task<InventoryItem> CreateItemAsync(InventoryItem item);
        Task<InventoryItem> UpdateItemAsync(Guid id, InventoryItem item);
        Task<bool> DeleteItemAsync(Guid id);
    }
}
