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
        public IQueryable<Category> Categories => categories.AsQueryable();
        public IQueryable<Manufacturer> Manufacturers => manufacturers.AsQueryable();
        public IQueryable<Product> Products => products.AsQueryable();
        private List<Category> categories;
        private List<Manufacturer> manufacturers;
        private List<Product> products;
        public MockDao()
        {
            categories = new List<Category>()
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
            manufacturers = new List<Manufacturer>()
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
                .RuleFor(o => o.Price, f => f.Finance.Amount(1000000, 20000000, 0))
                .RuleFor(o => o.Category, f => f.PickRandom(categories))
                .RuleFor(o => o.Manufacturer, f => f.PickRandom(manufacturers));
            products = productGen.GenerateBetween(1, 50);
        }
    }
}
