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
    public class AddManufacturerViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Manufacturer> manufacturers;
        public AddManufacturerViewModel(IEnumerable<Manufacturer> manufacturers)
        {
            this.manufacturers = manufacturers;
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
                else if (manufacturers.Any(manu => manu.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase)))
                    WarningMessage = "Tên hãng đã tồn tại";
                else
                    WarningMessage = string.Empty;
            }
        } = string.Empty;
        public bool WillAdd { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
