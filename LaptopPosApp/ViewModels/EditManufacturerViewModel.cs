using LaptopPosApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    class EditManufacturerViewModel : INotifyPropertyChanged
    {
        private readonly IQueryable<Manufacturer> _manufacturers;
        public Manufacturer Manufacturer 
        { 
            get;
            set
            {
                field = value;
                if (string.IsNullOrWhiteSpace(value.Name))
                    WarningMessage = "Tên hãng không hợp lệ";
                else if (value.Name != _oldName && _manufacturers.Any(manu => manu.Name.Equals(value.Name, StringComparison.CurrentCultureIgnoreCase)))
                    WarningMessage = "Tên hãng đã tồn tại";
                else
                    WarningMessage = string.Empty;
                Debug.WriteLine("yay");
            }
        }
        private readonly string _oldName;
        public EditManufacturerViewModel(IQueryable<Manufacturer> manufacturers, Manufacturer editingItem)
        {
            _manufacturers = manufacturers;
            _oldName = editingItem.Name;
            Manufacturer = editingItem;
            Name = editingItem.Name;
        }

        public string Name
        {
            get;
            set
            {
                field = value;
                if (string.IsNullOrWhiteSpace(value))
                    WarningMessage = "Tên hãng không hợp lệ";
                else if (value != _oldName && _manufacturers.Any(manu => manu.Name.Equals(value, StringComparison.CurrentCultureIgnoreCase)))
                    WarningMessage = "Tên hãng đã tồn tại";
                else
                    WarningMessage = string.Empty;
                Manufacturer.Name = value;
            }
        } = string.Empty;

        public string WarningMessage { get; private set; } = string.Empty;
        public bool IsValid => string.IsNullOrWhiteSpace(WarningMessage) && Name != _oldName;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
