using InventoryAndroidApp.ViewModels;

namespace InventoryAndroidApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        private readonly ItemDetailViewModel _viewModel;

        public ItemDetailPage(ItemDetailViewModel vm)
        {
            InitializeComponent();
            BindingContext = _viewModel = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.OnPageAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }
    }
}
