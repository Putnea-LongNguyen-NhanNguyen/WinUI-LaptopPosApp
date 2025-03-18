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

        public string Name { get; set; } = "";

        public bool IsValid
        {
            get
            {
                return !(string.IsNullOrWhiteSpace(Name) || _manufacturers.Any(manu => manu.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase)));
            }
            set;
        } = true;
        public string WarningMessage 
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return "Tên hãng không hợp lệ";
                else if (_manufacturers.Any(manu => manu.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase)))
                    return "Tên hãng đã tồn tại";
                else return "";
            }
            set;
        } = "";

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
