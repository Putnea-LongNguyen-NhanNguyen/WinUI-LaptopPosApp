﻿using System;
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
    public partial class DbContextWithMock : DbContextBase
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
        public DbContextWithMock(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSeeding((context, _) =>
            {
                CreatedMarkers.Add(new() { Id = 1 });
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                Categories.AddRange(_seedCategories);
                Manufacturers.AddRange(_seedManufacturers);
                var productGen = new Faker<Product>()
                    .StrictMode(false)
                    .RuleFor(o => o.ID, f => Guid.NewGuid().ToString())
                    .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                    .RuleFor(o => o.Description, f => f.Lorem.Sentences(2))
                    .RuleFor(o => o.Price, f => (long)f.Finance.Amount(1000000, 20000000, 0))
                    .RuleFor(o => o.Category, f => f.PickRandom(_seedCategories))
                    .RuleFor(o => o.Manufacturer, f => f.PickRandom(_seedManufacturers))
                    .RuleFor(o => o.Quantity, f => f.Random.Long(1, 100));
                var products = productGen.GenerateBetween(10, 50);
                products[0].TemporaryPrices.Add(new ProductTemporaryPrice()
                {
                    ProductID = products[0].ID,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(10),
                    Price = 1
                });
                Products.AddRange(products);

                var voucherFixedGen = new Faker<Voucher>()
                    .StrictMode(true)
                    .RuleFor(o => o.Code, f => Guid.NewGuid().ToString())
                    .RuleFor(o => o.Type, f => VoucherType.Fixed)
                    .RuleFor(o => o.Value, f => f.Random.Long(300000, 3000000))
                    .RuleFor(o => o.Quantity, f => 1)
                    .RuleFor(o => o.StartDate, f => f.Date.Recent(10))
                    .RuleFor(o => o.EndDate, f => f.Date.Soon(90))
                    .RuleFor(o => o.Orders, f => []);
                Vouchers.AddRange(voucherFixedGen.GenerateBetween(10, 50));

                var voucherPercentageGen = new Faker<Voucher>()
                    .StrictMode(true)
                    .RuleFor(o => o.Code, f => Guid.NewGuid().ToString())
                    .RuleFor(o => o.Type, f => VoucherType.Percentage)
                    .RuleFor(o => o.Value, f => f.Random.Int(10, 30))
                    .RuleFor(o => o.Quantity, f => 1)
                    .RuleFor(o => o.StartDate, f => f.Date.Recent(10))
                    .RuleFor(o => o.EndDate, f => f.Date.Soon(90))
                    .RuleFor(o => o.Orders, f => new());

                var vouchers = new List<Voucher>();
                vouchers.AddRange(voucherFixedGen.GenerateBetween(10, 50));
                vouchers.AddRange(voucherPercentageGen.GenerateBetween(10, 50));
                Vouchers.AddRange(vouchers);

                var customerGen = new Faker<Customer>()
                    .StrictMode(true)
                    .RuleFor(o => o.ID, f => 0)
                    .RuleFor(o => o.Name, f => f.Name.FullName())
                    .RuleFor(o => o.Address, f => f.Address.FullAddress())
                    .RuleFor(o => o.Phone, f => f.Phone.PhoneNumber())
                    .RuleFor(o => o.Email, f => f.Internet.Email())
                    .RuleFor(o => o.Orders, f => []);
                var customers = customerGen.GenerateBetween(10, 50);
                Customers.AddRange(customers);

                var orderGen = new Faker<Order>()
                    .RuleFor(o => o.ID, f => 0)
                    .RuleFor(o => o.Timestamp, f => f.Date.Between(DateTime.Today.AddDays(-700), DateTime.Today))
                    .RuleFor(o => o.Customer, f => f.PickRandom(customers))
                    .RuleFor(o => o.Status, f => f.PickRandom(new List<OrderStatus>() { OrderStatus.Delivered, OrderStatus.Delivering, OrderStatus.Returned }))
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
                            Product = p,
                        };
                    });
                    o.Products.AddRange(orderProduct);
                    o.TotalPrice = o.Products.Aggregate(0L, (acc, op) => acc + op.Product.Price * op.Quantity);
                    var reductions = o.Vouchers.Aggregate(0L, (acc, v) =>
                        acc + (v.Type == VoucherType.Fixed ? v.Value : (long)(o.TotalPrice * (v.Value / 100.0)))
                    );
                    o.TotalPrice = o.TotalPrice > reductions ? o.TotalPrice - reductions : 0;

                    if (o.Status == OrderStatus.Delivering)
                    {
                        o.DeliveryAddress = faker.Address.StreetAddress();
                        o.DeliveryDate = faker.Date.SoonOffset(30);
                    }
                    else
                    {
                        o.DeliveryDate = faker.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now.AddDays(-5));
                    }
                });
                Orders.AddRange(orders);

                context.ChangeTracker.AutoDetectChangesEnabled = true;
                context.ChangeTracker.DetectChanges();
                context.SaveChanges();
            });
        }
    }
}
