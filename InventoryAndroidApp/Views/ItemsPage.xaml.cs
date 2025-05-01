using InventoryAndroidApp.ViewModels;

namespace InventoryAndroidApp.Views
{
    public partial class ItemsPage : ContentPage
    {
        private readonly ItemsViewModel _viewModel;

        public ItemsPage(ItemsViewModel vm)
        {
            InitializeComponent();
            BindingContext = _viewModel = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

                _viewModel.RefreshCommand.Execute(null);
        }
    }
}
