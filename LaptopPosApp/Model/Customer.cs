using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public partial class Customer: ObservableObject, IHasId
    {
        [ObservableProperty]
        public required partial int ID { get; set; } = 0;
        IComparable IHasId.ID => ID;
        [ObservableProperty]
        public required partial string Name { get; set; }
        [ObservableProperty]
        public partial string Address { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string Phone { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string Email { get; set; } = string.Empty;
        public List<Order> Orders { get; set; } = new();
    }
}
