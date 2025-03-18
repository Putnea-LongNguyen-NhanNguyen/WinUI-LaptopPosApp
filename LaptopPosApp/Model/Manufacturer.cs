using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public class Manufacturer: IHasId<Manufacturer>
    {
        public required int ID { get; set; }
        IComparable IHasId<Manufacturer>.ID => ID;
        public string Name { get; set; } = string.Empty;
    }
}
