using CommunityToolkit.Mvvm.ComponentModel;
using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using LaptopPosApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    public partial class AddVouchersViewModel : AddItemViewModel
    {
        private DbContextBase _context;
        public List<VoucherType> VoucherTypes { get; set; } = new()
        {
            VoucherType.Percentage,
            VoucherType.Fixed,
        };

        public AddVouchersViewModel(DbContextBase context)
        {
            _context = context;
        }

        [ObservableProperty]
        public partial VoucherType Type { get; set; } = VoucherType.Fixed;
        [ObservableProperty]
        public partial long Value { get; set; } = 0;
        [ObservableProperty]
        public partial int Quantity { get; set; } = 0;
        [ObservableProperty]
        public partial DateTimeOffset StartDate { get; set; } = DateTimeOffset.Now;
        [ObservableProperty]
        public partial DateTimeOffset EndDate { get; set; } = DateTimeOffset.Now;
        [ObservableProperty]
        public partial long MinMoney { get; set; } = 0;  // minimum money paid in the past to email customers
        [ObservableProperty]
        public partial bool SendVouchersThroughMail { get; set; } = false;

        [ObservableProperty]
        public partial string ValueValidationMessage { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string QuantityValidationMessage { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string DateValidationMessage { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string MinMoneyValidationMessage { get; set; } = string.Empty; 

        partial void OnValueChanged(long value) 
        { 
            ValueValidationMessage = string.Empty;
        }

        partial void OnQuantityChanged(int value)
        {
            QuantityValidationMessage = string.Empty;
        }

        partial void OnStartDateChanged(DateTimeOffset value)
        {
            DateValidationMessage = string.Empty;
        }

        partial void OnEndDateChanged(DateTimeOffset value)
        {
            DateValidationMessage = string.Empty;
        }

        partial void OnMinMoneyChanged(long value)
        {
            MinMoneyValidationMessage = string.Empty;
        }

        partial void OnSendVouchersThroughMailChanged(bool value)
        {
            if (!value)
            {
                MinMoneyValidationMessage = string.Empty;
                return;
            }

            if (MinMoney < 8000000)
            {
                MinMoneyValidationMessage = "Khách hàng cần mua ít nhất 8 triệu đồng trong quá khứ để được gửi mã qua mail";
            }
        }

        protected override bool DoValidate()
        {
            var isValid = true;

            if (Value <= 0)
            {
                ValueValidationMessage = "Giá trị giảm không được bé hơn hoặc bằng 0";
                isValid = false;
            }
            else if (Type == VoucherType.Percentage && Value > 90)
            {
                ValueValidationMessage = "Không thể giảm quá 90%";
                isValid = false;
            }
            else if (Type == VoucherType.Fixed && Value < 100000)
            {
                ValueValidationMessage = "Không thể giảm ít hơn 100 nghìn nếu là loại cố định giá";
                isValid = false;
            }
            else
            {
                ValueValidationMessage = string.Empty;
            }

            if (Quantity <= 0)
            {
                QuantityValidationMessage = "Số phiếu tối thiểu được tạo không được bé hơn hoặc bằng 0";
                isValid = false;
            }
            else
            {
                QuantityValidationMessage = string.Empty;
            }

            if (StartDate > EndDate)
            {
                DateValidationMessage = "Ngày bắt đầu hiệu lực không thể lớn hơn ngày hết hạn";
                isValid = false;
            }
            else if (EndDate < DateTimeOffset.Now.AddDays(1)) {
                DateValidationMessage = "Ngày hết hạn phải lớn hơn ngày hôm nay ít nhất 1 ngày";
                isValid = false;
            }

            if (SendVouchersThroughMail && MinMoney < 8000000)
            {
                MinMoneyValidationMessage = "Khách hàng cần mua ít nhất 8 triệu đồng trong quá khứ để được gửi mã qua mail";
                isValid = false;
            }

            return isValid;
        }

        protected override bool DoSubmit()
        {
            IEnumerable<Customer>? customers = null;
            if (SendVouchersThroughMail)
            {
                // filter customers that paid the minimum amount of money required in the past
                customers = _context.Customers
                    .ToList()
                    .Where(c => c.Orders.Sum(o => o.TotalPrice) >= MinMoney)
                    .OrderBy(_ => Guid.NewGuid())  // randomize the order
                    .Take(Quantity);
            }

            long noVouchersToCreate = 0;
            if (SendVouchersThroughMail && customers?.Count() > 0)
            {
                noVouchersToCreate = customers.Count();
            }

            noVouchersToCreate = Math.Max(noVouchersToCreate, Quantity);

            for (int i = 0; i < noVouchersToCreate; i++)
            {
                StartDate = new DateTime(
                    StartDate.Year,
                    StartDate.Month,
                    StartDate.Day,
                    8,
                    0,
                    0,
                    0
                );
                EndDate = new DateTime(
                    EndDate.Year,
                    EndDate.Month,
                    EndDate.Day,
                    20,
                    59,
                    59,
                    0
                );
                Voucher newVoucher = new()
                {
                    Code = Guid.NewGuid().ToString(),
                    Type = Type,
                    Value = Value,
                    Quantity = 1,
                    StartDate = StartDate.DateTime,
                    EndDate = EndDate.DateTime,
                };
                _context.Vouchers.Add(newVoucher);
                if (customers?.Count() >= i + 1)
                {
                    Customer customer = customers.ElementAt(i);
                    Task.Run(() =>
                    {
                        SendMailService.SendVoucherEmail(customer, newVoucher);
                    });
                }
            }
            _context.SaveChanges();
            return true;
        }
    }
}
