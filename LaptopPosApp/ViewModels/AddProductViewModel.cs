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
    public class AddProductViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Product> products;
        public IEnumerable<Category> categories { get ; private set; }
        public IEnumerable<Manufacturer> manufacturers{get ; private set; }
        public AddProductViewModel(IEnumerable<Product> products, IEnumerable<Category> categories, IEnumerable<Manufacturer> manufacturers)
        {
            this.products = products;
            this.categories = categories;
            this.manufacturers = manufacturers;
        }

        public string NameWarningMessage { get; private set; } = string.Empty;
        public string QuantityWarningMessage { get; private set; } = string.Empty;
        public string PriceWarningMessage { get; private set; } = string.Empty;
        public string DescriptionWarningMessage { get; private set; } = string.Empty;
        public string ManufacturerWarningMessage { get; private set; } = string.Empty;
        public string CategoryWarningMessage { get; private set; } = string.Empty;
        public bool IsValid =>
            string.IsNullOrWhiteSpace(NameWarningMessage) &&
            string.IsNullOrWhiteSpace(QuantityWarningMessage) &&
            string.IsNullOrWhiteSpace(PriceWarningMessage) &&
            string.IsNullOrWhiteSpace(DescriptionWarningMessage) &&
            string.IsNullOrWhiteSpace(ManufacturerWarningMessage) &&
            string.IsNullOrWhiteSpace(CategoryWarningMessage);
        public string Name
        {
            get;
            set
            {
                field = value;
                if (string.IsNullOrWhiteSpace(value))
                    NameWarningMessage = "Tên sản phẩm không hợp lệ";
                else if (products.Any(prod => prod.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase)))
                    NameWarningMessage = "Tên sản phẩm đã tồn tại";
                else
                    NameWarningMessage = string.Empty;
            }
        } = string.Empty;
        public string Description 
        { 
            get; 
            set
            {
                field = value;
                if (string.IsNullOrWhiteSpace(value))
                    DescriptionWarningMessage = "Vui lòng nhập mô tả sản phẩm";
                else
                    DescriptionWarningMessage = string.Empty;
            }
        } = string.Empty;
        public ulong Price 
        { 
            get;
            set
            {
                field = value;
                if ((int)value < 0)
                    PriceWarningMessage = "Giá sản phẩm không hợp lệ (phải >= 0)";
                else
                    PriceWarningMessage = string.Empty;
            }
        } = 0;
        public ulong Quantity 
        { 
            get; 
            set
            {
                field = value;
                if ((int)value < 0)
                    QuantityWarningMessage = "Số lượng sản phẩm không hợp lệ (phải >= 0)";
                else
                    QuantityWarningMessage = string.Empty;
            }
        } = 0;
        public Manufacturer? Manufacturer 
        { 
            get; 
            set
            {
                field = value;
                if (value == null)
                    ManufacturerWarningMessage = "Vui lòng chọn nhà sản xuất";
                else
                    ManufacturerWarningMessage = string.Empty;
            }
        }
        public Category? Category
        {
            get; 
            set
            {
                field = value;
                if (value == null)
                    CategoryWarningMessage = "Vui lòng chọn phân loại";
                else
                    CategoryWarningMessage = string.Empty;
            }
        }
        public bool WillAdd { get; set; }
        public void CategoryWarningMessageTrigger()
        {
            CategoryWarningMessage = "Vui lòng chọn phân loại";
        }
        public void ManufacturerWarningMessageTrigger()
        {
            ManufacturerWarningMessage = "Vui lòng chọn nhà sản xuất";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        
    }
}
