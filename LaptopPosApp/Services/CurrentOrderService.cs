using LaptopPosApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Services
{
    public class CurrentOrderService
    {
        public ObservableCollection<OrderProduct> CurrentOder { get; } = new();
    }
}
