using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InventoryAndroidApp.Models
{
    public class InventoryItem : INotifyPropertyChanged
    {
        private Guid _itemId;
        private string _itemName;
        private int _currentQuantity;
        private DateTime _lastUpdated;

        public Guid ItemId
        {
            get => _itemId;
            set => SetProperty(ref _itemId, value);
        }

        public string ItemName
        {
            get => _itemName;
            set => SetProperty(ref _itemName, value);
        }

        public int CurrentQuantity
        {
            get => _currentQuantity;
            set => SetProperty(ref _currentQuantity, value);
        }

        public DateTime LastUpdated
        {
            get => _lastUpdated;
            set => SetProperty(ref _lastUpdated, value);
        }

        public InventoryItem Clone()
        {
            return (InventoryItem)MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
