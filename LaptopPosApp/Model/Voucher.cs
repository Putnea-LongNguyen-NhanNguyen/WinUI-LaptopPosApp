using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public enum VoucherType
    {
        Fixed,
        Percentage
    }
    public partial class Voucher: ObservableObject
    {
        public required string Code { get; set; }
        [ObservableProperty]
        public required partial VoucherType Type { get; set; }
        [ObservableProperty]
        public required partial long Value { get; set; }
        [ObservableProperty]
        public required partial long Quantity { get; set; }
        [ObservableProperty]
        public required partial DateTime StartDate { get; set; }
        [ObservableProperty]
        public required partial DateTime EndDate { get; set; }
        public List<Order> Orders { get; set; } = [];

        partial void OnTypeChanged(VoucherType value)
        {
            if (value == VoucherType.Percentage && Value > 90)
            {
                Value = 90;
            }
            else if (value == VoucherType.Fixed && Value < 100000)
            {
                Value = 100000;
            }
                OnPropertyChanged(nameof(ValueString));
        }

        partial void OnValueChanged(long value)
        {
            OnPropertyChanged(nameof(ValueString));
        }

        public string ValueString => Type == VoucherType.Fixed ? Value.ToString("#,### đ") : $"{Value}%";
    }
}
