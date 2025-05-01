using InventoryAndroidApp.Views;
namespace InventoryAndroidApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(AddItemPage), typeof(AddItemPage));

        }
    }
}

