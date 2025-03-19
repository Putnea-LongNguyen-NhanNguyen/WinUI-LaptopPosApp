using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public interface IHasId
    {
        public IComparable ID { get; }
    }
}
