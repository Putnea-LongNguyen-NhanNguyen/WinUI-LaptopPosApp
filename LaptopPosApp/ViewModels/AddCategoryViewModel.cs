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
    public class AddCategoryViewModel : AddItemViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private DbContextBase dbContext;
        public AddCategoryViewModel(DbContextBase dbContext)
        {
            this.dbContext = dbContext;
        }

        public string Name {
            get;
            set
            {
                field = value;
                NameValidationMessage = string.Empty;
            }
        } = string.Empty;
        public string NameValidationMessage { get; set; } = string.Empty;

        protected override bool DoValidate()
        {
            var isValid = true;
            if (string.IsNullOrWhiteSpace(Name))
            {
                NameValidationMessage = "Tên danh mục không hợp lệ";
                isValid = false;
            }
            else if (dbContext.Categories.AsEnumerable().Any(manu => manu.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)))
            {
                NameValidationMessage = "Tên danh mục đã tồn tại";
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
            Category category = new()
            {
                ID = 0,
                Name = Name
            };
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();
            return true;
        }
    }
}
