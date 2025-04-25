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
        public override IList<IFilter> GetAllFilters()
        {
            return [
                new FilterMultipleChoice<Voucher, VoucherType>
                {
                    Name = "Loại mã giảm giá",
                    Filterer = (query, selectedValues) =>
                    {
                        if (selectedValues.Count == 0)
                            return query;
                        return query.Where(v => selectedValues.Contains(v.Type));
                    },
                    Values = [
                        new() {
                            Key = "Giảm cố định",
                            Value = VoucherType.Fixed,
                        },
                        new() {
                            Key = "Giảm theo phần trăm",
                            Value = VoucherType.Percentage,
                        }
                    ]
                },
                new FilterRange<Voucher, DateTime>(
                    allItems.Select(v => v.EndDate).Min(),
                    allItems.Select(v => v.EndDate).Max()
                )
                {
                    Name = "Ngày hết hạn",
                    Filterer = (query, min, max) =>
                    {
                        return query.Where(v => v.EndDate >= min && v.EndDate <= max);
                    }
                },
            ];
        }
        public async Task StartAddFlow(Page parent)
        {
            var page = new AddVouchersPage();
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

        public void OpenVouchersMailWindow(Page parent)
        {
            var window = new SendVouchersMailWindow();
            window.Activate();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
