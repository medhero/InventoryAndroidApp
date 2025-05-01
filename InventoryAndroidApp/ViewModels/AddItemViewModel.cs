using InventoryAndroidApp.Models;
using InventoryAndroidApp.Services;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;

namespace InventoryAndroidApp.ViewModels
{
    public class AddItemViewModel : BaseViewModel
    {
        private readonly IInventoryApiService _inventoryService;

        public InventoryItem Item { get; set; }

        public ICommand SaveCommand { get; }

        public AddItemViewModel(IInventoryApiService inventoryService)
        {
            _inventoryService = inventoryService;
            Item = new InventoryItem(); 
            SaveCommand = new Command(async () => await SaveAsync());
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Item.ItemName))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Item name is required.", "OK");
                return;
            }

            if (Item.ItemName.Any(char.IsDigit))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Item name cannot contain numbers.", "OK");
                return;
            }

            try
            {
                IsBusy = true;

                Item.ItemId = Guid.NewGuid();
                Item.LastUpdated = DateTime.Now;

                await _inventoryService.CreateItemAsync(Item);

                await Toast.Make("Item added successfully!").Show();
                await Shell.Current.GoToAsync(".."); // navigate back
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to add item: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
