using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using LaptopPosApp.Model;

namespace LaptopPosApp.Dao
{
    public class MockDao: IDao
    {
        public IEnumerable<Category> Categories { get; private set; }
        public IEnumerable<Manufacturer> Manufacturers { get; private set; }
        public IEnumerable<Product> Products { get; private set; }

        public MockDao()
        {
            Categories = new List<Category>()
            {
                new Category()
                {
                    ID = 1,
                    Name = "Gaming"
                },
                new Category()
                {
                    ID = 2,
                    Name = "Văn phòng"
                },
                new Category()
                {
                    ID = 3,
                    Name = "Workstation"
                },
            };
            Manufacturers = new List<Manufacturer>()
            {
                new Manufacturer()
                {
                    ID = 1,
                    Name = "Asus"
                },
                new Manufacturer()
                {
                    ID = 2,
                    Name = "Acer"
                },
                new Manufacturer()
                {
                    ID = 3,
                    Name = "Lenovo"
                },
                new Manufacturer()
                {
                    ID = 4,
                    Name = "HP"
                },
                new Manufacturer()
                {
                    ID = 5,
                    Name = "Dell"
                },
            };
            var productGen = new Faker<Product>()
                .StrictMode(true)
                .RuleFor(o => o.ID, f => f.Random.String(15, 'A', 'Z'))
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Lorem.Sentences(2))
                .RuleFor(o => o.Category, f => f.PickRandom(Categories))
                .RuleFor(o => o.Manufacturer, f => f.PickRandom(Manufacturers));
            Products = productGen.GenerateBetween(1, 50);
        }
    }
}
