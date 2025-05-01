using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using InventoryAndroidApp.Models;
using InventoryAndroidApp.Services;

namespace InventoryAndroidApp.ViewModels
{
    public class EditItemPopupViewModel : INotifyPropertyChanged
    {
        //private readonly IFavoriteService _favoriteService;

        public event PropertyChangedEventHandler PropertyChanged;

        public InventoryItem InventoryItem { get; }

        public TaskCompletionSource<InventoryItem?> SaveTaskCompletionSource { get; private set; }

        public EditItemPopupViewModel(InventoryItem destination)
        {
            InventoryItem = destination ?? throw new ArgumentNullException(nameof(destination));
            SaveTaskCompletionSource = new TaskCompletionSource<InventoryItem?>();
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void Save()
        {
            if (string.IsNullOrWhiteSpace(InventoryItem.ItemName))
            {
                SaveTaskCompletionSource.TrySetResult(null);
                return;
            }
            
            SaveTaskCompletionSource.TrySetResult(InventoryItem);
        }

        public void Cancel()
        {
            SaveTaskCompletionSource.TrySetResult(null);
        }
    }
}
