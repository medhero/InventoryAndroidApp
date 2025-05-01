using InventoryAndroidApp.Services;
using InventoryAndroidApp.ViewModels;

namespace InventoryAndroidApp.Views
{
    public partial class AddItemPage : ContentPage
    {
        public AddItemPage(IInventoryApiService inventoryService)
        {
            InitializeComponent();
            BindingContext = new AddItemViewModel(inventoryService);
        }
    }
}
