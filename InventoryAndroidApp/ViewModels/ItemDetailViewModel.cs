using InventoryAndroidApp;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using InventoryAndroidApp.Services;
using InventoryAndroidApp.Models;

namespace InventoryAndroidApp.ViewModels
{
    [QueryProperty(nameof(OriginalItem), "Item")]
    public class ItemDetailViewModel : BaseViewModel
    {
        private readonly IInventoryApiService _inventoryService;

        private InventoryItem _originalItem;
        public InventoryItem OriginalItem
        {
            get => _originalItem;
            set
            {
                _originalItem = value;
                EditableItem = _originalItem.Clone();
            }
        }

        private InventoryItem _editableItem;
        public InventoryItem EditableItem
        {
            get => _editableItem;
            set
            {
                _editableItem = value;
                OnPropertyChanged();
            }
        }

        public ICommand UpdateQuantityCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand DeleteItemCommand { get; }

        public ItemDetailViewModel(IInventoryApiService inventoryService)
        {
            _inventoryService = inventoryService;

            UpdateQuantityCommand = new Command<string>(async (change) => await UpdateQuantityAsync(change));
            SaveChangesCommand = new Command(async () => await SaveChangesAsync());
            DeleteItemCommand = new Command(async () => await DeleteItemAsync());
        }

        private async Task UpdateQuantityAsync(string change)
        {
            if (EditableItem == null) return;

            try
            {
                IsBusy = true;
                var amount = int.Parse(change);
                EditableItem.CurrentQuantity += amount;
                EditableItem.LastUpdated = DateTime.Now;

                await Toast.Make($"Quantity {(amount > 0 ? "increased" : "decreased")} by {Math.Abs(amount)}").Show();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to update quantity: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveChangesAsync()
        {
            if (EditableItem == null) return;

            try
            {
                IsBusy = true;
                EditableItem.LastUpdated = DateTime.Now;

                if (string.IsNullOrWhiteSpace(EditableItem.ItemName))
                {
                    await Shell.Current.DisplayAlert("Validation Error", "Item name is required.", "OK");
                    return;
                }

                if (EditableItem.ItemName.Any(char.IsDigit))
                {
                    await Shell.Current.DisplayAlert("Validation Error", "Item name cannot contain numbers.", "OK");
                    return;
                }

                // Apply changes to the original
                _originalItem.ItemName = EditableItem.ItemName;
                _originalItem.CurrentQuantity = EditableItem.CurrentQuantity;
                _originalItem.LastUpdated = EditableItem.LastUpdated;

                await _inventoryService.UpdateItemAsync(_originalItem.ItemId, _originalItem);

                await Toast.Make("Changes saved successfully!").Show();
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to save changes: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteItemAsync()
        {
            if (_originalItem == null) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Confirm Delete",
                $"Are you sure you want to delete {_originalItem.ItemName}?",
                "Delete",
                "Cancel");

            if (!confirm) return;

            try
            {
                IsBusy = true;
                await _inventoryService.DeleteItemAsync(_originalItem.ItemId);

                await Toast.Make($"{_originalItem.ItemName} deleted").Show();
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to delete item: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnPageAppearing()
        {
            OnPropertyChanged(nameof(EditableItem));
        }
    }
}
