using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopPosApp.Model;

namespace LaptopPosApp.Dao
{
    public interface IDao
    {
        public IEnumerable<Product> Products { get; }
        public IEnumerable<Category> Categories { get; }
        public IEnumerable<Manufacturer> Manufacturers { get; }
    }
}
