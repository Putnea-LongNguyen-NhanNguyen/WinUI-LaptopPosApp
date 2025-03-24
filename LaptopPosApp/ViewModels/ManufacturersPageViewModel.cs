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
    class ManufacturersPageViewModel : PaginatableViewModel<Manufacturer>
    {
        private readonly DbContextBase dbContext;

        public ManufacturersPageViewModel(DbContextBase dbContext): base(dbContext.Manufacturers)
        {
            this.dbContext = dbContext;
        }

        public async Task StartAddFlow(Page parent)
        {
            var page = new AddManufacturerPage();
            var contentDialog = new ContentDialog()
            {
                XamlRoot = parent.XamlRoot,
                Content = page,
                Title = "Thêm hãng mới",
            };
            page.ContentDialog = contentDialog;
            await contentDialog.ShowAsync();
            Refresh();
        }

        public void Remove(IEnumerable<Manufacturer> items)
        {
            var deleted = false;
            foreach (var item in items)
            {
                dbContext.Manufacturers.Remove(item);
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
