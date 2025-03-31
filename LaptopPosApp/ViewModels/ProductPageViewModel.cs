using Bogus;
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
        public IEnumerable<Category> Categories { get; private set; }
        public IEnumerable<Manufacturer> Manufacturers { get; private set; }

        public ProductPageViewModel(DbContextBase dbContext) : base(dbContext.Products)
        {
            this.dbContext = dbContext;
            Categories = dbContext.Categories.AsEnumerable();
            Manufacturers = dbContext.Manufacturers.AsEnumerable();
        }

        public async Task StartAddFlow(Page parent)
        {
            var page = new AddProductPage();
            var contentDialog = new ContentDialog()
            {
                XamlRoot = parent.XamlRoot,
                Content = page,
                Title = "Thêm sản phẩm mới",
            };
            page.ContentDialog = contentDialog;
            await contentDialog.ShowAsync();
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
