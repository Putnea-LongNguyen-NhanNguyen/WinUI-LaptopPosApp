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
using vietnam_qr_pay_csharp;

namespace LaptopPosApp.ViewModels
{
    public partial class OrderDetailWindowViewModel: AddItemViewModel
    {
        private readonly DbContextBase dbContext;
        public ObservableCollection<OrderProduct> CurrentOrder { get; set; }
        public ObservableCollection<Voucher> VouchersAdded { get; set; } = [];
        public OrderDetailWindowViewModel(DbContextBase dbContext, CurrentOrderService currentOrderService)
        {
            this.dbContext = dbContext;
            this.CurrentOrder = currentOrderService.CurrentOder;
            CurrentOrder.CollectionChanged += CurrentOrder_CollectionChanged;
            VouchersAdded.CollectionChanged += VouchersAdded_CollectionChanged;
            foreach (var item in CurrentOrder)
            {
                SubscribeToItemPropertyChanged(item);
            }
            UpdateTotalPrice();
        }

        private readonly CurrencyConverter currencyConverter = new ();
        [ObservableProperty]
        public partial long TotalPrice { get; set; } = 0;
        [ObservableProperty]
        public partial string TotalPriceString { get; set; } = string.Empty;
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
        private void UpdateTotalPrice()
        {
            TotalPrice = CurrentOrder.Select(x => x.Product.CurrentPrice * x.Quantity).Sum();
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

        private void VouchersAdded_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateTotalPrice();
            VoucherValidationMessage = string.Empty;
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
                OrderDetailWindow.CloseWindow();
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
        partial void OnNameChanged(string value)
        {
            NameValidationMessage = string.Empty;
            VouchersAdded.Clear();
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
            else if(voucher.Orders.Any(o=>o.Customer.Name == Name))
            {
                VoucherValidationMessage = "Bạn đã sử dụng mã giảm giá rồi";
            }
            else if(VouchersAdded.Any(v=>v.Code == voucher.Code))
            {
                VoucherValidationMessage = "Bạn đã nhập mã giảm giá này rồi";
            }
            else
            {
                VouchersAdded.Add(voucher);
            }
        }

        public void RemoveVoucher(Voucher voucher)
        {
            VouchersAdded.Remove(voucher);
        }

        protected override bool DoSubmit()
        {
            var customer = dbContext.Customers.FirstOrDefault(c => c.Email == Email);
            customer ??= dbContext.Customers.FirstOrDefault(c => c.Phone == Phone);
            if (customer == null) {
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
                TotalPrice = TotalPrice,
                Status = IsDelivery ? OrderStatus.Delivering : OrderStatus.Delivered,
                DeliveryAddress = IsDelivery ? Address : string.Empty,
                DeliveryDate = IsDelivery ? DateTimeOffset.Now.AddDays(5) : DateTimeOffset.Now,
            };
            
            foreach (var v in VouchersAdded)
            {
                v.Quantity--;
            }
            customer.Orders.Add(order);
            dbContext.Orders.Add(order);

            foreach (var product in order.Products)
            {
                var product2 = dbContext.Products.Where(p => p.ID == product.ProductID).FirstOrDefault();
                if (product2 != null)
                {
                    product2.Quantity -= product.Quantity;
                    dbContext.Products.Update(product2);
                }
            }

            dbContext.SaveChanges();
            CurrentOrder.Clear();

            if (IsEmailBill)
            {
                Task.Run(() => SendMailService.SendOrderEmail(order));
            }

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
            if (!string.IsNullOrWhiteSpace(Phone) && !Phone.IsPhoneNumber())
            {
                PhoneValidationMessage = "SĐT không hợp lệ!!!";
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
                    EmailValidationMessage = "Email không hợp lệ!!!";
                    isValid = false;
                }
            }          
            return isValid;
        }

        public string CreateMomoQR(long amount, string purpose)
        {
            // Số tài khoản trong ví MoMo
            var accountNumber = Environment.GetEnvironmentVariable("MOMO_NUMBER")!;

            var momoQR = QRPay.InitVietQR(
                bankBin: BankApp.BanksObject[BankKey.BANVIET].bin,
                bankNumber: accountNumber,
                amount: amount.ToString(), // Số tiền (không bắt buộc)
                purpose
            );

            // Trong mã QR của MoMo có chứa thêm 1 mã tham chiếu tương ứng với STK
            momoQR.additionalData.reference = "MOMOW2W" + accountNumber.Substring(10);

            // Mã QR của MoMo có thêm 1 trường ID 80 với giá trị là 3 số cuối của SỐ ĐIỆN THOẠI của tài khoản nhận tiền
            momoQR.SetUnreservedField("80", Environment.GetEnvironmentVariable("MOMO_LAST_3_NUMBERS")!);
            return momoQR.Build();
        }
    }
}
