using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public class Product: IHasId
    {
        public required string ID { get; set; }
        IComparable IHasId.ID => ID;
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public ulong Price { get; set; } = 0;
        public ulong Quantity { get; set; } = 0;
        public Manufacturer? Manufacturer { get; set; }
        public Category? Category { get; set; }
        public List<ProductTemporaryPrice> TemporaryPrices { get; set; } = new();
    }
}
