using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public partial class Product: ObservableObject, IHasId
    {
        public required string ID { get; set; }
        IComparable IHasId.ID => ID;
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        [ObservableProperty]
        public partial long Price { get; set; } = 0;
        [ObservableProperty]
        public partial long Quantity { get; set; } = 0;
        public Manufacturer? Manufacturer { get; set; }
        public Category? Category { get; set; }
        public List<ProductTemporaryPrice> TemporaryPrices { get; set; } = new();
        public string Image { get; set; } = string.Empty;
        [NotMapped]
        public string ImagePath => File.Exists(Image) ? Image : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/laptopDemoImg.jpg");
    }
}
