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
        public IQueryable<Product> Products { get; }
        public IQueryable<Category> Categories { get; }
        public IQueryable<Manufacturer> Manufacturers { get; }
    }
}
