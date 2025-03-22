using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public class Category: IHasId
    {
        public required int ID { get; set; }
        IComparable IHasId.ID => ID;
        public string Name { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new();
    }
}
