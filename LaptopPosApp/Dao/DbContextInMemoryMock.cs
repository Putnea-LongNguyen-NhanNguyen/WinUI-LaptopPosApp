using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using LaptopPosApp.Model;
using Microsoft.EntityFrameworkCore;

namespace LaptopPosApp.Dao
{
    public partial class DbContextInMemoryMock : DbContextBase
    {
        private static List<Category> _seedCategories = new()
        {
            new ()
            {
                ID = 1,
                Name = "Gaming"
            },
            new ()
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
        private static List<Manufacturer> _seedManufacturers = new()
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
        public DbContextInMemoryMock() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=InMemoryMockDb;Mode=Memory;Cache=Shared");
            optionsBuilder.UseSeeding((context, _) =>
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                Categories.AddRange(_seedCategories);
                Manufacturers.AddRange(_seedManufacturers);
                var productGen = new Faker<Product>()
                    .StrictMode(true)
                    .RuleFor(o => o.ID, f => f.Random.String(15, 'A', 'Z'))
                    .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                    .RuleFor(o => o.Description, f => f.Lorem.Sentences(2))
                    .RuleFor(o => o.Price, f => (ulong)f.Finance.Amount(1000000, 20000000, 0))
                    .RuleFor(o => o.Category, f => f.PickRandom(_seedCategories.AsEnumerable()))
                    .RuleFor(o => o.Manufacturer, f => f.PickRandom(_seedManufacturers.AsEnumerable()));
                Products.AddRange(productGen.GenerateBetween(10, 50));
                context.ChangeTracker.AutoDetectChangesEnabled = true;
                context.ChangeTracker.DetectChanges();
                context.SaveChanges();
            });
        }
    }
}
