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
    public partial class Product : ObservableObject, IHasId
    {
        public required string ID { get; set; }
        IComparable IHasId.ID => ID;
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        [ObservableProperty]
        public partial long Price { get; set; } = 0;
        partial void OnPriceChanged(long price)
        {
            OnPropertyChanged(nameof(CurrentPrice));
        }

        [NotMapped]
        public long CurrentPrice => TemporaryPrices
            .Where(tp => DateTimeOffset.Now >= tp.StartDate && DateTimeOffset.Now <= tp.EndDate)
            .FirstOrDefault()?.Price ?? Price;

        [ObservableProperty]
        public partial long Quantity { get; set; } = 0;
        public Manufacturer? Manufacturer { get; set; }
        public Category? Category { get; set; }
        [ObservableProperty]
        public partial List<ProductTemporaryPrice> TemporaryPrices { get; set; } = new();
        partial void OnTemporaryPricesChanged(List<ProductTemporaryPrice> temporaryPrices)
        {
            OnPropertyChanged(nameof(CurrentPrice));
        }

        public string Image { get; set; } = string.Empty;
        [NotMapped]
        public string ImagePath => File.Exists(Image) ? Image : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/laptopDemoImg.jpg");
    }
}
