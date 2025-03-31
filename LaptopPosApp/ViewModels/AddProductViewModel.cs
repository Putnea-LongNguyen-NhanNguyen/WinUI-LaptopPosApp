using Bogus.Bson;
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
    public partial class AddProductViewModel : AddItemViewModel
    {
        private DbContextBase dbContext;
        private IEnumerable<Product> Products => dbContext.Products.AsEnumerable();
        public IEnumerable<Manufacturer> Manufacturers { get; private set; } = null!;
        public IEnumerable<Category> Categories { get; private set; } = null!;
        public AddProductViewModel(DbContextBase dbContext)
        {
            this.dbContext = dbContext;
            updateEnumerables();
        }
        private void updateEnumerables()
        {
            Manufacturers = dbContext.Manufacturers.AsEnumerable();
            Categories = dbContext.Categories.AsEnumerable();
        }

        [ObservableProperty]
        public partial string NameValidationMessage { get; private set; } = string.Empty;
        [ObservableProperty]
        public partial string QuantityValidationMessage { get; private set; } = string.Empty;
        [ObservableProperty]
        public partial string PriceValidationMessage { get; private set; } = string.Empty;
        [ObservableProperty]
        public partial string ManufacturerValidationMessage { get; private set; } = string.Empty;
        [ObservableProperty]
        public partial string CategoryValidationMessage { get; private set; } = string.Empty;

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string Description { get; set; } = string.Empty;
        [ObservableProperty]
        public partial long Price { get; set; } = 0;
        [ObservableProperty]
        public partial long Quantity { get; set; } = 0;
        [ObservableProperty]
        public partial Manufacturer? Manufacturer { get; set; }
        [ObservableProperty]
        public partial Category? Category { get; set; }

        partial void OnNameChanged(string value)
        {
            NameValidationMessage = string.Empty;
        }
        partial void OnPriceChanged(long value)
        {
            PriceValidationMessage = string.Empty;
        }
        partial void OnQuantityChanged(long value)
        {
            QuantityValidationMessage = string.Empty;
        }
        partial void OnManufacturerChanged(Manufacturer? value)
        {
            ManufacturerValidationMessage = string.Empty;
        }
        partial void OnCategoryChanged(Category? value)
        {
            CategoryValidationMessage = string.Empty;
        }

        protected override bool DoValidate()
        {
            updateEnumerables();
            var isValid = true;
            if (string.IsNullOrWhiteSpace(Name))
            {
                NameValidationMessage = "Tên sản phẩm không hợp lệ";
                isValid = false;
            }
            else if (Products.Any(prod => prod.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                NameValidationMessage = "Tên sản phẩm đã tồn tại";
                isValid = false;
            }
            else
            {
                NameValidationMessage = string.Empty;
            }

            if (Price <= 0)
            {
                PriceValidationMessage = "Giá sản phẩm không hợp lệ";
                isValid = false;
            }
            else
            {
                PriceValidationMessage = string.Empty;
            }

            if (Quantity < 0)
            {
                QuantityValidationMessage = "Số lượng sản phẩm không hợp lệ";
                isValid = false;
            }
            else
            {
                QuantityValidationMessage = string.Empty;
            }

            if (Manufacturer is null)
            {
                ManufacturerValidationMessage = "Vui lòng chọn nhà sản xuất";
                isValid = false;
            }
            else
            {
                ManufacturerValidationMessage = string.Empty;
            }

            if (Category is null)
            {
                CategoryValidationMessage = "Vui lòng chọn phân loại";
                isValid = false;
            }
            else
            {
                CategoryValidationMessage = string.Empty;
            }

            return isValid;
        }
        protected override bool DoSubmit()
        {
            dbContext.Products.Add(new()
            {
                ID = Guid.NewGuid().ToString(),
                Name = Name,
                Price = Price,
                Quantity = Quantity,
                Description = Description,
                Category = Category,
                Manufacturer = Manufacturer
            });
            dbContext.SaveChanges();
            return true;
        }
    }
}
