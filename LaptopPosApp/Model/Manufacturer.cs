using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LaptopPosApp.Model
{
    public partial class Manufacturer: ObservableObject, IHasId
    {
        [ObservableProperty]
        public required partial int ID { get; set; }
        IComparable IHasId.ID => ID;

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial List<Product> Products { get; set; } = new();
        public override string ToString()
        {
            return Name;
        }
    }
}
