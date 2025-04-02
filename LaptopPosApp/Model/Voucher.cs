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
        public List<Order> Orders { get; set; } = new();
    }
}
