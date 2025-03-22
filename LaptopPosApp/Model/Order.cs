using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public class Order: IHasId
    {
        public required int ID { get; set; } = 0;
        IComparable IHasId.ID => ID;
        public required DateTime Timestamp { get; set; }
        public required Customer Customer { get; set; }
        public List<OrderProduct> Products { get; set; } = new();
        public List<Voucher> Vouchers { get; set; } = new();
        public required ulong TotalPrice { get; set; }
    }
}
