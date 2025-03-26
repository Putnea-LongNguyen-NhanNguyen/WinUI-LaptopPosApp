using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LaptopPosApp.Model
{
    public partial class ProductTemporaryPrice: ObservableObject
    {
        [ObservableProperty]
        public required partial string ProductID { get; set; }
        [ObservableProperty]
        public partial DateTime StartDate { get; set; }
        [ObservableProperty]
        public partial DateTime? EndDate { get; set; }
        [ObservableProperty]
        public required partial ulong Price { get; set; }
        public Product Product { get; set; } = null!;
    }
}
