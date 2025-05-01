using System;
using System.Collections.Generic;
using System.Linq;
using InventoryAndroidApp.MockInventoryApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAndroidApp.MockInventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private static List<InventoryItem> _items = new()
        {
            new InventoryItem
            {
                ItemId = Guid.NewGuid(),
                ItemName = "Tent",
                CurrentQuantity = 2,
                LastUpdated = DateTime.Now
            },
            new InventoryItem
            {
                ItemId = Guid.NewGuid(),
                ItemName = "Map",
                CurrentQuantity = 10,
                LastUpdated = DateTime.Now
            },
            new InventoryItem
            {
                ItemId = Guid.NewGuid(),
                ItemName = "Flashlight",
                CurrentQuantity = 5,
                LastUpdated = DateTime.Now
            }
        };

        [HttpGet]
        public ActionResult<List<InventoryItem>> Get()
        {
            return Ok(_items);
        }

        [HttpPost]
        public ActionResult<InventoryItem> Post(InventoryItem item)
        {
            item.ItemId = Guid.NewGuid();
            item.LastUpdated = DateTime.Now;
            _items.Add(item);
            return CreatedAtAction(nameof(Get), new { id = item.ItemId }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, InventoryItem updatedItem)
        {
            var item = _items.FirstOrDefault(i => i.ItemId == id);
            if (item == null) return NotFound();

            item.ItemName = updatedItem.ItemName;
            item.CurrentQuantity = updatedItem.CurrentQuantity;
            item.LastUpdated = DateTime.Now;

            return Ok(item);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var item = _items.FirstOrDefault(i => i.ItemId == id);
            if (item == null) return NotFound();

            _items.Remove(item);
            return NoContent();
        }
    }
}