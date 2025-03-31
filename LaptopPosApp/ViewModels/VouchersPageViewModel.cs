using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using LaptopPosApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    class VouchersPageViewModel(DbContextBase context) : PaginatableViewModel<Voucher>(context.Vouchers)
    {
        private readonly DbContextBase _context = context;
        public List<VoucherType> VoucherTypes { get; set; } = new()
        {
            VoucherType.Fixed,
            VoucherType.Percentage,
        };

        public async Task StartAddFlow(Page parent)
        {
            var page = new AddVouchersPage();
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

        public void Remove(IEnumerable<Voucher> items)
        {
            var deleted = false;
            foreach (var item in items)
            {
                _context.Vouchers.Remove(item);
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
