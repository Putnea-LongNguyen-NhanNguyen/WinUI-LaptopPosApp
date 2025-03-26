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
                    .RuleFor(o => o.ID, f => f.Random.String(15, 'A', 'Z'))
                    .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                    .RuleFor(o => o.Description, f => f.Lorem.Sentences(2))
                    .RuleFor(o => o.Price, f => (ulong)f.Finance.Amount(1000000, 20000000, 0))
                    .RuleFor(o => o.Category, f => f.PickRandom(_seedCategories))
                    .RuleFor(o => o.Manufacturer, f => f.PickRandom(_seedManufacturers))
                    .RuleFor(o => o.Quantity, f => (ulong)f.Random.Int(1, 100));
                var products = productGen.GenerateBetween(10, 50);
                Products.AddRange(products);

                var voucherFixedGen = new Faker<Voucher>()
                    .StrictMode(true)
                    .RuleFor(o => o.Code, f => f.Random.String(15, 'A', 'z'))
                    .RuleFor(o => o.Type, f => VoucherType.Fixed)
                    .RuleFor(o => o.Value, f => f.Random.ULong(300000, 3000000))
                    .RuleFor(o => o.Quantity, f => f.Random.ULong(2, 10))
                    .RuleFor(o => o.StartDate, f => f.Date.Recent(10))
                    .RuleFor(o => o.EndDate, f => f.Date.Future(90))
                    .RuleFor(o => o.Orders, f => new());
                var vouchers = voucherFixedGen.GenerateBetween(10, 50);
                Vouchers.AddRange(vouchers);

                var customerGen = new Faker<Customer>()
                    .StrictMode(true)
                    .RuleFor(o => o.ID, f => 0)
                    .RuleFor(o => o.Name, f => f.Name.FullName())
                    .RuleFor(o => o.Address, f => f.Address.FullAddress())
                    .RuleFor(o => o.Phone, f => f.Phone.PhoneNumber())
                    .RuleFor(o => o.Email, f => f.Internet.Email())
                    .RuleFor(o => o.Orders, f => new());
                var customers = customerGen.GenerateBetween(10, 50);
                Customers.AddRange(customers);

                var orderGen = new Faker<Order>()
                    .RuleFor(o => o.ID, f => 0)
                    .RuleFor(o => o.Timestamp, f => f.Date.Between(DateTime.Today.AddDays(-700), DateTime.Today))
                    .RuleFor(o => o.Customer, f => f.PickRandom(customers))
                    .RuleFor(o => o.Vouchers, f => [f.PickRandom(vouchers)]);
                var orders = orderGen.GenerateBetween(10, 50);
                orders.ForEach(o =>
                {
                    Faker faker = new Faker();
                    var productsOrdered = faker.PickRandom(products, 3);
                    var orderProduct = productsOrdered.Select(p =>
                    {
                        return new OrderProduct()
                        {
                            OrderID = o.ID,
                            ProductID = p.ID,
                            Quantity = faker.Random.Int(1, 3),
                            Order = o,
                            Product = p
                        };
                    });
                    o.Products.AddRange(orderProduct);
                    o.TotalPrice = o.Products.Aggregate(0ul, (acc, op) => acc + op.Product.Price * (ulong)op.Quantity);
                    var reductions = o.Vouchers.Aggregate(0ul, (acc, v) =>
                        acc + (v.Type == VoucherType.Fixed ? v.Value : (ulong)(o.TotalPrice * (v.Value / 100.0)))
                    );
                    o.TotalPrice = o.TotalPrice > reductions ? o.TotalPrice - reductions : 0;
                });
                Orders.AddRange(orders);

                context.ChangeTracker.AutoDetectChangesEnabled = true;
                context.ChangeTracker.DetectChanges();
                context.SaveChanges();
            });
        }
    }
}
