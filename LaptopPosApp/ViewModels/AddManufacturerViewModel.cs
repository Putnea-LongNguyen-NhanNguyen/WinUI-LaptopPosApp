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
    class AddManufacturerViewModel : INotifyPropertyChanged
    {
        private readonly IQueryable<Manufacturer> _manufacturers;
        public AddManufacturerViewModel(IQueryable<Manufacturer> manufacturers)
        {
            _manufacturers = manufacturers;
        }

        public string WarningMessage { get; private set; } = string.Empty;
        public bool IsValid => string.IsNullOrWhiteSpace(WarningMessage);
        public string Name
        {
            get;
            set
            {
                field = value;
                if (string.IsNullOrWhiteSpace(value))
                    WarningMessage = "Tên hãng không hợp lệ";
                else if (_manufacturers.Any(manu => manu.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase)))
                    WarningMessage = "Tên hãng đã tồn tại";
                else
                    WarningMessage = string.Empty;
            }
        } = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
