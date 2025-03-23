using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using LaptopPosApp.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    class ProductPageViewModel : PaginatableViewModel<Product>
    {
        private readonly DbContextBase dbContext;

        public ProductPageViewModel(DbContextBase dbContext) : base(dbContext.Products)
        {
            this.dbContext = dbContext;
        }

        public async Task StartAddFlow(Page parent)
        {
            var vm = new AddProductViewModel(dbContext.Products.ToArray(), dbContext.Categories.ToArray(), dbContext.Manufacturers.ToArray());
            var page = new AddProductPage(vm);
            var contentDialog = new ContentDialog()
            {
                XamlRoot = parent.XamlRoot,
                Content = page,
                Title = "Thêm sản phẩm mới",
            };
            page.ContentDialog = contentDialog;
            await contentDialog.ShowAsync();

            if (!vm.WillAdd)
                return;

            string newName = vm.Name;
            ulong newPrice = vm.Price;
            ulong newQuantity = vm.Quantity;
            string newDescription = vm.Description;
            Category? newCategory = vm.Category;
            Manufacturer? newManufacturer = vm.Manufacturer;

            // add in DAO
            Debug.WriteLine("add product: " + newName);
            dbContext.Products.Add(new()
            {
                ID = "Prod0", // temporary value for EF
                Name = newName,
                Price = newPrice,
                Quantity = newQuantity,
                Description = newDescription,
                Category = newCategory,
                Manufacturer = newManufacturer
            });
            SaveChanges();
            Refresh();
        }

        public void Remove(IEnumerable<Product> items)
        {
            var deleted = false;
            foreach (var item in items)
            {
                dbContext.Products.Remove(item);
                deleted = true;
            }
            if (deleted)
            {
                SaveChanges();
                Refresh();
            }
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
    }
}
