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
        internal class IDComparer<T>: Comparer<T> where T: IHasId<T>
        {
            public override int Compare(T? a, T? b)
            {
                if (a == null && b == null)
                    return 0;
                if (a == null)
                    return -1;
                if (b == null)
                    return 1;
                return a.ID.CompareTo(b.ID);
            }
        }
        public IQueryable<Category> Categories => categories.AsQueryable();
        public IQueryable<Manufacturer> Manufacturers => manufacturers.AsQueryable();
        public IQueryable<Product> Products => products.AsQueryable();
        private readonly SortedSet<Category> categories;
        private readonly SortedSet<Manufacturer> manufacturers;
        private readonly SortedSet<Product> products;
        public MockDao()
        {
            categories = new(new IDComparer<Category>())
            {
                new()
                {
                    ID = 1,
                    Name = "Gaming"
                },
                new()
                {
                    ID = 2,
                    Name = "Văn phòng"
                },
                new()
                {
                    ID = 3,
                    Name = "Workstation"
                },
            };
            manufacturers = new(new IDComparer<Manufacturer>())
            {
                new()
                {
                    ID = 1,
                    Name = "Asus"
                },
                new()
                {
                    ID = 2,
                    Name = "Acer"
                },
                new()
                {
                    ID = 3,
                    Name = "Lenovo"
                },
                new()
                {
                    ID = 4,
                    Name = "HP"
                },
                new()
                {
                    ID = 5,
                    Name = "Dell"
                },
                new()
                {
                    ID = 6,
                    Name = "MSI"
                },
                new()
                {
                    ID = 7,
                    Name = "Apple"
                },
                new()
                {
                    ID = 8,
                    Name = "Microsoft"
                },
                new()
                {
                    ID = 9,
                    Name = "Corsair"
                },
                new()
                {
                    ID = 10,
                    Name = "LG"
                },
                new()
                {
                    ID = 11,
                    Name = "Google"
                },
            };
            var productGen = new Faker<Product>()
                .StrictMode(true)
                .RuleFor(o => o.ID, f => f.Random.String(15, 'A', 'Z'))
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Lorem.Sentences(2))
                .RuleFor(o => o.Price, f => (ulong)f.Finance.Amount(1000000, 20000000, 0))
                .RuleFor(o => o.Category, f => f.PickRandom(categories.AsEnumerable()))
                .RuleFor(o => o.Manufacturer, f => f.PickRandom(manufacturers.AsEnumerable()));
            products = new(
                productGen.GenerateBetween(1, 50),
                new IDComparer<Product>()
            );
        }
    }
}
