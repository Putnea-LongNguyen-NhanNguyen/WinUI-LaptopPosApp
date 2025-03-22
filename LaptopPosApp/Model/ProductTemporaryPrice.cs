using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public class ProductTemporaryPrice
    {
        public required string ProductID { get; set; }
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public required ulong Price { get; set; }
        public Product Product { get; set; } = null!;
    }
}
