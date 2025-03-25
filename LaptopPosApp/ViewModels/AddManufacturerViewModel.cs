using CommunityToolkit.Mvvm.ComponentModel;
using LaptopPosApp.Dao;
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
    public partial class AddManufacturerViewModel : AddItemViewModel
    {
        private DbContextBase dbContext;
        public AddManufacturerViewModel(DbContextBase dbContext)
        {
            this.dbContext = dbContext;
        }

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string NameValidationMessage { get; private set; } = string.Empty;
        partial void OnNameChanged(string value)
        {
            NameValidationMessage = string.Empty;
        }

        protected override bool DoValidate()
        {
            var isValid = true;
            if (string.IsNullOrWhiteSpace(Name))
            {
                NameValidationMessage = "Tên hãng không hợp lệ";
                isValid = false;
            }
            else if (dbContext.Manufacturers.AsEnumerable().Any(manu => manu.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)))
            {
                NameValidationMessage = "Tên hãng đã tồn tại";
                isValid = false;
            }
            else
            {
                NameValidationMessage = string.Empty;
            }
            return isValid;
        }
        protected override bool DoSubmit()
        {
            Manufacturer manufacturer = new()
            {
                ID = 0,
                Name = Name
            };
            dbContext.Manufacturers.Add(manufacturer);
            dbContext.SaveChanges();
            return true;
        }
    }
}
