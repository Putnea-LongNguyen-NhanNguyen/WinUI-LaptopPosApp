using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public enum OrderStatus
    {
        Delivering,
        Delivered,
        Returned,
    }
    public partial class Order: ObservableObject, IHasId
    {
        [ObservableProperty]
        public required partial int ID { get; set; } = 0;
        IComparable IHasId.ID => ID;
        [ObservableProperty]
        public required partial DateTime Timestamp { get; set; }
        [ObservableProperty]
        public required partial Customer Customer { get; set; }
        [ObservableProperty]
        public partial List<OrderProduct> Products { get; set; } = [];
        [ObservableProperty]
        public partial List<Voucher> Vouchers { get; set; } = [];
        [ObservableProperty]
        public required partial long TotalPrice { get; set; }
        [ObservableProperty]
        public partial OrderStatus Status { get; set; }
        [ObservableProperty]
        public partial DateTimeOffset DeliveryDate { get; set; }
        [ObservableProperty]
        public partial string DeliveryAddress { get; set; } = string.Empty;
        partial void OnDeliveryAddressChanged(string value)
        {
            OnPropertyChanged(nameof(HomeDelivery));
        }

        [NotMapped]
        public bool HomeDelivery => !string.IsNullOrEmpty(DeliveryAddress);
    }
}
