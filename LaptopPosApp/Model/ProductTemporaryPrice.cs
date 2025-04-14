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
        public required string ProductID { get; set; }
        [ObservableProperty]
        public partial DateTimeOffset StartDate { get; set; }
        [ObservableProperty]
        public partial DateTimeOffset EndDate { get; set; }
        [ObservableProperty]
        public required partial long Price { get; set; }
        public Product Product { get; set; } = null!;
    }
}
