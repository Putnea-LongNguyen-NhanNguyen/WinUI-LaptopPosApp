﻿using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using LaptopPosApp.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                Title = "Thêm khách hàng mới",
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

        public void OpenHistoryWindow(int customerId)
        {
            var customer = _context.Customers.AsEnumerable().FirstOrDefault(c => c.ID == customerId);
            if (customer == null)
            {
                Debug.WriteLine("CustomersPage, how is customer null here");
                return;
            }

            // i like prop drilling
            var window = new CustomerOrderHistoryWindow(new CustomerOrderHistoryViewModel(customer, _context));
            window.Activate();
        }
    }
}
