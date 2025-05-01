using Xunit;
using Moq;
using InventoryAndroidApp.ViewModels;
using InventoryAndroidApp.Services;
using InventoryAndroidApp.Models;
using System;
using System.Threading.Tasks;

namespace InventoryAndroidApp.Tests
{
    public class ItemDetailViewModelTests
    {
        [Fact]
        public async Task SaveChangesAsync_WithValidData_UpdatesItem()
        {
            // Arrange
            var item = new InventoryItem
            {
                ItemId = Guid.NewGuid(),
                ItemName = "Test Item",
                CurrentQuantity = 5,
                LastUpdated = DateTime.Now
            };

            var mockService = new Mock<IInventoryApiService>();
            var viewModel = new ItemDetailViewModel(mockService.Object)
            {
                OriginalItem = item
            };

            viewModel.EditableItem.ItemName = "Updated Name";
            viewModel.EditableItem.CurrentQuantity = 10;

            // Act
            viewModel.SaveChangesCommand.Execute(null); 

            // Assert
            mockService.Verify(s => s.UpdateItemAsync(item.ItemId, item), Times.Once);
            Assert.Equal("Updated Name", item.ItemName);
            Assert.Equal(10, item.CurrentQuantity);
        }
    }
}
