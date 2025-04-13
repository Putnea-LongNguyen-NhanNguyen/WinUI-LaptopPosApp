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

        public ObservableCollection<Customer> Customers { get; set; }
        public ObservableCollection<Customer> FilteredCustomers { get; set; }
        [ObservableProperty]
        public partial Customer? SelectedCustomer { get; set; }
        public ObservableCollection<Voucher> Vouchers { get; set; }
        [ObservableProperty]
        public partial Voucher? SelectedVoucher { get; set; }

        public bool CanAddToMailList => SelectedCustomer != null && SelectedVoucher != null;
        public bool CanSend => MailList.Count > 0;

        // store vouchers that are used up
        private readonly List<Voucher> ranOutVouchers = [];

        public ObservableCollection<CustomerVoucher> MailList { get; set; } = [];

        partial void OnSelectedCustomerChanged(Customer? value)
        {
            OnPropertyChanged(nameof(CanAddToMailList));
        }

        partial void OnSelectedVoucherChanged(Voucher? value)
        {
            OnPropertyChanged(nameof(CanAddToMailList));
        }

        public SendVouchersMailViewModel(DbContextBase context)
        {
            _context = context;
            Customers = [.. _context.Customers.ToList().Where(c => !string.IsNullOrWhiteSpace(c.Email))];
            FilteredCustomers = [.. Customers];
            Vouchers = [.. _context.Vouchers.ToList()];
        }

        public void FilterCustomer(string nameQuery)
        {
            FilteredCustomers = [.. Customers.Where(c => c.Name.Contains(nameQuery, StringComparison.CurrentCultureIgnoreCase))];
        }

        public void AddToMailList()
        {
            if (SelectedCustomer != null && SelectedVoucher != null)
            {
                MailList.Add(new CustomerVoucher() { Customer = SelectedCustomer, Voucher = SelectedVoucher });
                SelectedCustomer = null;

                var voucher = Vouchers.FirstOrDefault(v => v.Code == SelectedVoucher.Code);
                if (voucher != null)
                {
                    voucher.Quantity--;
                    if (voucher.Quantity <= 0)
                    {
                        Vouchers.Remove(voucher);
                        ranOutVouchers.Add(voucher);
                    }
                }

                SelectedVoucher = null;
                OnPropertyChanged(nameof(CanSend));
            }
        }

        public void RemoveFromMailList(CustomerVoucher customerVoucher)
        {
            MailList.Remove(customerVoucher);
            var voucher = _context.Vouchers.FirstOrDefault(v => v.Code == customerVoucher.Voucher.Code);
            if (voucher != null)
            {
                voucher.Quantity++;
                if (ranOutVouchers.Contains(voucher))
                {
                    Vouchers.Add(voucher);
                }
                ranOutVouchers.Remove(voucher);
            }
            OnPropertyChanged(nameof(CanSend));
        }

        public void Send()
        {
            Task task = Task.Run(() =>
            {
                // stupid method
                Dictionary<Customer, List<Voucher>> dict = [];
                foreach (var item in MailList)
                {
                    if (!dict.ContainsKey(item.Customer))
                        dict.Add(item.Customer, []);
                    dict[item.Customer].Add(item.Voucher);
                }
                SendMailService.SendVoucherEmail(dict);
            });
        }
    }

    public class CustomerVoucher
    {
        public required Customer Customer { get; set; }
        public required Voucher Voucher { get; set; }
    }
}