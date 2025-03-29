using CommunityToolkit.Mvvm.ComponentModel;
using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using System;
using System.Collections.Generic;
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
        public partial ulong Value { get; set; } = 0;
        [ObservableProperty]
        public partial ulong Quantity { get; set; } = 0;
        [ObservableProperty]
        public partial DateTimeOffset StartDate { get; set; } = DateTimeOffset.Now;
        [ObservableProperty]
        public partial DateTimeOffset EndDate { get; set; } = DateTimeOffset.Now;

        [ObservableProperty]
        public partial string ValueValidationMessage { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string QuantityValidationMessage { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string DateValidationMessage { get; set; } = string.Empty;

        partial void OnTypeChanged(VoucherType value)
        {

        }

        partial void OnValueChanged(ulong value) 
        { 
            ValueValidationMessage = string.Empty;
        }

        partial void OnQuantityChanged(ulong value)
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
                QuantityValidationMessage = "Số phiếu giảm giá không được bé hơn hoặc bằng 0";
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

            return isValid;
        }

        protected override bool DoSubmit()
        {
            _context.Vouchers.Add(new()
            {
                Code = Guid.NewGuid().ToString(),
                Type = Type,
                Value = Value,
                Quantity = Quantity,
                StartDate = StartDate.DateTime,
                EndDate = EndDate.DateTime,
            });
            _context.SaveChanges();
            return true;
        }
    }
}
