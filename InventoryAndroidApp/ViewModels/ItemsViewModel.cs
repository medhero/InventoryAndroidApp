using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using InventoryAndroidApp.Models;
using InventoryAndroidApp.Services;
using InventoryAndroidApp.Views;
using Microsoft.Maui.Controls;

namespace InventoryAndroidApp.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private readonly IInventoryApiService _inventoryService;

        public ObservableCollection<InventoryItem> InventoryItems { get; } = new();
        public ObservableCollection<InventoryItem> FilteredInventoryItems { get; } = new();

        private string _searchQuery = string.Empty;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (SetProperty(ref _searchQuery, value))
                {
                    FilterInventoryItems(value);
                    IsClearVisible = !string.IsNullOrEmpty(value);
                }
            }
        }

        private bool _isClearVisible;
        public bool IsClearVisible
        {
            get => _isClearVisible;
            set => SetProperty(ref _isClearVisible, value);
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        // Add a selected item property to bind to the selected item in the UI
        private InventoryItem _selectedInventoryItem;
        public InventoryItem SelectedInventoryItem
        {
            get => _selectedInventoryItem;
            set
            {
                if (SetProperty(ref _selectedInventoryItem, value) && value != null)
                {
                    OnInventoryItemTapped(value);
                    // Reset selection after processing
                    SelectedInventoryItem = null;
                }
            }
        }
        public ICommand NavigateToAddItemCommand { get; }
        public ICommand ClearSearchCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand TapInventoryItemCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand AddNewItemCommand { get; }

        public ItemsViewModel(IInventoryApiService inventoryService)
        {
            //Title = "Inventory Items";
            _inventoryService = inventoryService;

            SearchCommand = new Command<string>(FilterInventoryItems);
            NavigateToAddItemCommand = new Command(async () => await NavigateToAddItemPageAsync());
            TapInventoryItemCommand = new Command<InventoryItem>(OnInventoryItemTapped);
            ClearSearchCommand = new Command(ClearSearch);
            RefreshCommand = new Command(async () => await RefreshItemsAsync());

            // Load items initially
            Task.Run(async () => await LoadInventoryItemsAsync());
        }

        private void ClearSearch()
        {
            SearchQuery = string.Empty;
        }

        public async Task RefreshItemsAsync()
        {
            IsRefreshing = true;
            await LoadInventoryItemsAsync();
            IsRefreshing = false;
        }

        public async Task LoadInventoryItemsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                InventoryItems.Clear();
                FilteredInventoryItems.Clear();

                var items = await _inventoryService.GetAllItemsAsync();

                foreach (var item in items.OrderBy(i => i.ItemName))
                {
                    InventoryItems.Add(item);
                    FilteredInventoryItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                // Log the actual exception for debugging
                System.Diagnostics.Debug.WriteLine($"Error loading items: {ex}");
                await Shell.Current.DisplayAlert("Error", "Failed to load inventory items. Please try again.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void FilterInventoryItems(string query)
        {
            FilteredInventoryItems.Clear();

            if (string.IsNullOrWhiteSpace(query))
            {
                foreach (var item in InventoryItems)
                    FilteredInventoryItems.Add(item);
                return;
            }

            var lowerQuery = query.ToLowerInvariant();

            var matches = InventoryItems
                .Where(i => (i.ItemName?.ToLowerInvariant().Contains(lowerQuery) == true))
                .ToList();

            foreach (var match in matches)
                FilteredInventoryItems.Add(match);
        }

        private async void OnInventoryItemTapped(InventoryItem inventoryItem)
        {
            if (inventoryItem == null || IsBusy)
                return;

            try
            {
                await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}", true, new Dictionary<string, object>
                {
                    { "Item", inventoryItem }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
                await Shell.Current.DisplayAlert("Error", "Navigation failed. Please try again.", "OK");
            }
        }

        private async Task NavigateToAddItemPageAsync()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(AddItemPage));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
                await Shell.Current.DisplayAlert("Error", "Could not navigate to add item page.", "OK");
            }
        }
    }
}

