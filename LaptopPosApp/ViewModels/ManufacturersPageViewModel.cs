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
    class ManufacturersPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public IList Items { get; private set; } = Array.Empty<Manufacturer>();
        private readonly DbContextBase dbContext;

        public int Count { get; private set; }
        public int PerPage
        {
            get;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Items per page must be positive");
                field = value;
                Refresh();
            }
        } = 5;
        public int PageCount => (int)Math.Max(1, Math.Ceiling((double)Count / PerPage));
        private int currentPage = 1;
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                Refresh();
            }
        }
        public bool Refreshing { get; private set; }
        public ManufacturersPageViewModel(DbContextBase dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Refresh()
        {
            Refreshing = true;
            Count = dbContext.Manufacturers.Count();
            currentPage = Math.Clamp(currentPage, 1, PageCount);
            Items = dbContext.Manufacturers
                .Skip((currentPage - 1) * PerPage)
                .Take(PerPage)
                .ToArray();
            Refreshing = false;
        }

        public async Task StartAddFlow(Page parent)
        {
            var vm = new AddManufacturerViewModel(dbContext.Manufacturers.ToArray());
            var page = new AddManufacturerPage(vm);
            var contentDialog = new ContentDialog()
            {
                XamlRoot = parent.XamlRoot,
                Content = page,
                Title = "Thêm hãng mới",
            };
            page.ContentDialog = contentDialog;
            await contentDialog.ShowAsync();

            if (!vm.WillAdd)
                return;

            string newName = vm.Name;
            // add in DAO
            Debug.WriteLine("add manufacturer: " + newName);
            dbContext.Manufacturers.Add(new()
            {
                ID = 0, // temporary value for EF
                Name = newName
            });
            SaveChanges();
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
