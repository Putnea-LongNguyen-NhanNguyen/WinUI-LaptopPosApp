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
        public SortedSet<ProductTemporaryPrice> TemporaryPrices { get; set; } = new(
            Comparer<ProductTemporaryPrice>.Create((a, b) =>
            {
                var result = a.StartDate.CompareTo(b.StartDate);
                if (result != 0)
                {
                    return result;
                }
                if (a.EndDate is null)
                {
                    return b.EndDate is null ? 0 : 1;
                }
                if (b.EndDate is null)
                {
                    return -1;
                }
                return a.EndDate.Value.CompareTo(b.EndDate.Value);
            })
        );
    }
}
