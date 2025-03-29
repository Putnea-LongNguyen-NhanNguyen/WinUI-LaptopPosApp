using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using LaptopPosApp.Views;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    class CustomersPageViewModel(DbContextBase context) : PaginatableViewModel<Customer>(context.Customers)
    {
        private DbContextBase _context = context;

        public async Task StartAddFlow(Page parent)
        {
            var page = new AddCustomerPage();
            var contentDialog = new ContentDialog()
            {
                XamlRoot = parent.XamlRoot,
                Content = page,
                Title = "Thêm mã giảm mới",
            };
            page.ContentDialog = contentDialog;
            await contentDialog.ShowAsync();
            Refresh();
        }

        public void Remove(IEnumerable<Customer> items)
        {
            var deleted = false;
            foreach (var item in items)
            {
                _context.Customers.Remove(item);
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
            _context.SaveChanges();
        }
    }
}
