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
    public class Voucher
    {
        public required string Code { get; set; }
        public required VoucherType Type { get; set; }
        public required ulong Value { get; set; }
        public required ulong Quantity { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public List<Order> Orders { get; set; } = new();
    }
}
