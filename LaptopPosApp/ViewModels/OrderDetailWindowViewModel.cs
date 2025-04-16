using CommunityToolkit.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using LaptopPosApp.Services;
using LaptopPosApp.Views;
using LaptopPosApp.Views.Converters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    public partial class OrderDetailWindowViewModel: AddItemViewModel
    {
        private DbContextBase dbContext;
        public ObservableCollection<OrderProduct> CurrentOrder { get; set; }
        public ObservableCollection<Voucher> VouchersAdded { get; set; } = [];
        public OrderDetailWindowViewModel(DbContextBase dbContext, CurrentOrderService currentOrderService)
        {
            this.dbContext = dbContext;
            this.CurrentOrder = currentOrderService.CurrentOder;
            CurrentOrder.CollectionChanged += CurrentOrder_CollectionChanged;
            foreach (var item in CurrentOrder)
            {
                SubscribeToItemPropertyChanged(item);
            }
            UpdateTotalPrice();
        }
        CurrencyConverter currencyConverter = new ();
        [ObservableProperty]
        public partial long TotalPrice { get; set; } = 0;
        [ObservableProperty]
        public partial string TotalPriceString { get; set; } = string.Empty;
        private void UpdateTotalPrice()
        {
            TotalPrice = CurrentOrder.Select(x => x.Product.Price * x.Quantity).Sum();
            foreach (var voucher in VouchersAdded)
            {
                if (voucher.Type == VoucherType.Fixed)
                {
                    TotalPrice -= voucher.Value;
                }
                else if (voucher.Type == VoucherType.Percentage)
                {
                    TotalPrice -= TotalPrice * voucher.Value / 100;
                }
                TotalPrice = Math.Max(TotalPrice, 0);
            }
            TotalPriceString = "Tổng thành tiền: " + currencyConverter.Convert(TotalPrice, typeof(string), null, null).ToString();
        }
        private void CurrentOrder_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // Handle added items
            if (e.NewItems != null)
            {
                foreach (OrderProduct newItem in e.NewItems)
                {
                    SubscribeToItemPropertyChanged(newItem);
                }
            }

            // Handle removed items
            if (e.OldItems != null)
            {
                foreach (OrderProduct oldItem in e.OldItems)
                {
                    UnsubscribeFromItemPropertyChanged(oldItem);
                }
            }
            UpdateTotalPrice();
            if(CurrentOrder.Count == 0)
            {
                OrderDetailWindow.closeWindow();
            }
        }
        private void SubscribeToItemPropertyChanged(OrderProduct item)
        {
            item.PropertyChanged += OrderProduct_PropertyChanged;
        }

        private void UnsubscribeFromItemPropertyChanged(OrderProduct item)
        {
            item.PropertyChanged -= OrderProduct_PropertyChanged;
        }

        private void OrderProduct_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderProduct.Quantity))
            {
                UpdateTotalPrice();
            }
        }
        [ObservableProperty]
        public partial bool IsPaperBill { get; set; } = false;
        [ObservableProperty]
        public partial bool IsEmailBill { get; set; } = false;
        [ObservableProperty]
        public partial bool IsDelivery { get; set; } = false;
        [ObservableProperty]
        public partial string Voucher { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string Email { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string Phone { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string Address { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string NameValidationMessage { get; private set; } = string.Empty;
        [ObservableProperty]
        public partial string EmailValidationMessage { get; private set; } = string.Empty;
        [ObservableProperty]
        public partial string PhoneValidationMessage { get; private set; } = string.Empty;
        [ObservableProperty]
        public partial string AddressValidationMessage { get; private set; } = string.Empty;
        [ObservableProperty]
        public partial string VoucherValidationMessage { get; private set; } = string.Empty;

        partial void OnNameChanged(string value)
        {
            NameValidationMessage = string.Empty;
        }
        partial void OnEmailChanged(string value)
        {
            EmailValidationMessage = string.Empty;
        }
        partial void OnPhoneChanged(string value)
        {
            PhoneValidationMessage = string.Empty;
        }
        partial void OnAddressChanged(string value)
        {
            AddressValidationMessage = string.Empty;
        }
        partial void OnVoucherChanged(string value)
        {
            VoucherValidationMessage = string.Empty;
        }

        public void AddVoucher()
        {            
            if (string.IsNullOrEmpty(Name))
            {
                NameValidationMessage = "Bạn cần nhập tên trước khi sử dụng mã giảm giá";
                return;
            }
            var voucher = dbContext.Vouchers.FirstOrDefault(v => v.Code == Voucher);
            if (voucher == null)
            {
                VoucherValidationMessage = "Mã giảm giá không tồn tại";
            }
            else if(voucher.Quantity <= 0)
            {
                VoucherValidationMessage = "Mã giảm giá đã hết";
            }
            else if(voucher.EndDate < DateTime.Now || voucher.StartDate > DateTime.Now)
            {
                VoucherValidationMessage = "Mã giảm giá chưa tới hoặc đã quá hạn sử dụng";
            }
            else if(voucher.Orders.FirstOrDefault(o=>o.Customer.Name == Name) != null)
            {
                VoucherValidationMessage = "Bạn đã sử dụng mã giảm giá rồi";
            }
            else
            {
                VouchersAdded.Add(voucher);
                UpdateTotalPrice();
            }
        }

        public void RemoveVoucher(Voucher voucher)
        {
            VouchersAdded.Remove(voucher);
            UpdateTotalPrice();
        }

        protected override bool DoSubmit()
        {
            var customer = dbContext.Customers.FirstOrDefault(c => c.Name == Name);
            if (customer == null)
            {
                customer = new()
                {
                    ID = 0,
                    Name = Name,
                    Email = Email,
                    Phone = Phone,
                    Address = Address,
                    Orders = [],
                };
                dbContext.Customers.Add(customer);
            }           
            var order = new Order()
            {
                ID = 0,
                Timestamp = DateTime.Now,
                Customer = customer,
                Products = [.. CurrentOrder],
                Vouchers = [.. VouchersAdded],
                TotalPrice = TotalPrice
            };
            customer.Orders.Add(order);
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();
            return true;
        }

        protected override bool DoValidate()
        {
            var isValid = true;
            if (string.IsNullOrWhiteSpace(Name))
            {
                NameValidationMessage = "Tên khách hàng không được để trống!!!";
                isValid = false;
            }
            if (IsDelivery == true && string.IsNullOrWhiteSpace(Address))
            {
                AddressValidationMessage = "Giao hàng tận nơi nên địa chỉ không được để trống!!!";
                isValid = false;
            }
            if (IsDelivery == true && string.IsNullOrWhiteSpace(Phone))
            {
                PhoneValidationMessage = "Giao hàng tận nơi nên SĐT không được để trống!!!";
                isValid = false;
            }
            if (IsEmailBill)
            {
                if (string.IsNullOrWhiteSpace(Email))
                {
                    EmailValidationMessage = "Gửi hoá đơn qua email nên email không được để trống!!!";
                    isValid = false;
                }
                else if (!Email.IsEmail())
                {
                    EmailValidationMessage = "Email không hợp lệ";
                    isValid = false;
                }
            }          
            return isValid;
        }
    }
}
