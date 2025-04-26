using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public partial class OrderProduct: ObservableObject
    {
        public required int OrderID { get; set; }
        public required string ProductID { get; set; }
        [ObservableProperty]
        public partial int Quantity { get; set; } = 1;
        [ObservableProperty]
        public partial DateTimeOffset? ReturnDate { get; set; }
        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
