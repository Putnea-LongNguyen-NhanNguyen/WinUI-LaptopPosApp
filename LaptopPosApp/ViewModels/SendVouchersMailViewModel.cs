using CommunityToolkit.Mvvm.ComponentModel;
using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using LaptopPosApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    public partial class SendVouchersMailViewModel : ObservableObject
    {
        readonly DbContextBase _context;

        public ObservableCollection<Voucher> Vouchers { get; set; }
        [ObservableProperty]
        public partial Voucher? SelectedVoucher { get; set; }

        [ObservableProperty]
        public partial long MinMoney { get; set; } = 8000000;

        public bool CanAddToMailList => SelectedVoucher != null && MinMoney >= 8000000;
        public bool CanSend => MailList.Count > 0;

        public ObservableCollection<VoucherMinMoney> MailList { get; set; } = [];

        partial void OnSelectedVoucherChanged(Voucher? value)
        {
            OnPropertyChanged(nameof(CanAddToMailList));
        }

        partial void OnMinMoneyChanged(long value)
        {
            OnPropertyChanged(nameof(CanAddToMailList));
        }

        public SendVouchersMailViewModel(DbContextBase context)
        {
            _context = context;
            Vouchers = [.._context.Vouchers];
        }

        public void AddSelectedToMailList()
        {
            if (SelectedVoucher == null)
                return;

            MailList.Add(new VoucherMinMoney() { Voucher = SelectedVoucher, MinMoney = MinMoney});
            Vouchers.Remove(SelectedVoucher);
            OnPropertyChanged(nameof(CanSend));
        }

        public void RemoveFromMailList(VoucherMinMoney obj)
        {
            MailList.Remove(obj);
            Vouchers.Add(obj.Voucher);
            OnPropertyChanged(nameof(CanSend));
        }

        public void Send()
        {
            foreach (var voucher in MailList)
            {
                // gacha
                var customer = _context.Customers
                    .ToList()
                    .Where(c => c.Orders.Sum(o => o.TotalPrice) >= voucher.MinMoney)
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(1);

                if (customer == null)
                    continue;

                if (customer.Count() <= 0)
                    continue;
                Task task = Task.Run(() =>
                {
                    SendMailService.SendVoucherEmail(customer.ElementAt(0), voucher.Voucher);
                });
            }
        }

        public class VoucherMinMoney
        {
            public required Voucher Voucher { get; set; }
            public long MinMoney { get; set; }
        }
    }
}