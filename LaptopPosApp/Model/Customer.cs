using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public class Customer: IHasId
    {
        public required int ID { get; set; } = 0;
        IComparable IHasId.ID => ID;
        public required string Name { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<Order> Orders { get; set; } = new();
    }
}
