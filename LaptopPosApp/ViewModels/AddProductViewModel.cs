using Bogus.Bson;
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
    public class AddProductViewModel : AddItemViewModel, INotifyPropertyChanged
    {
        private DbContextBase dbContext;
        private IEnumerable<Product> Products => dbContext.Products.AsEnumerable();
        public IEnumerable<Manufacturer> Manufacturers { get; private set; }
        public IEnumerable<Category> Categories { get; private set; }
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

        public string NameValidationMessage { get; private set; } = string.Empty;
        public string QuantityValidationMessage { get; private set; } = string.Empty;
        public string PriceValidationMessage { get; private set; } = string.Empty;
        public string ManufacturerValidationMessage { get; private set; } = string.Empty;
        public string CategoryValidationMessage { get; private set; } = string.Empty;

        public string Name
        {
            get;
            set
            {
                field = value;
                NameValidationMessage = string.Empty;
            }
        } = string.Empty;
        public string Description 
        { 
            get;
            set;
        } = string.Empty;
        public ulong Price 
        { 
            get;
            set
            {
                field = value;
                PriceValidationMessage = string.Empty;
            }
        } = 0;
        public ulong Quantity 
        { 
            get; 
            set
            {
                field = value;
                QuantityValidationMessage = string.Empty;
            }
        } = 0;
        public Manufacturer? Manufacturer 
        { 
            get; 
            set
            {
                field = value;
                ManufacturerValidationMessage = string.Empty;
            }
        }
        public Category? Category
        {
            get; 
            set
            {
                field = value;
                CategoryValidationMessage = string.Empty;
            }
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
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
