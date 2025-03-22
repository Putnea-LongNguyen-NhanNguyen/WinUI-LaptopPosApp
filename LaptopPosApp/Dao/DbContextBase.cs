using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopPosApp.Model;
using Microsoft.EntityFrameworkCore;

namespace LaptopPosApp.Dao
{
    public abstract class DbContextBase: DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Primary Keys
            modelBuilder
                .Entity<Category>()
                .HasKey(e => e.ID);
            modelBuilder
                .Entity<Manufacturer>()
                .HasKey(e => e.ID);
            modelBuilder
                .Entity<Product>()
                .HasKey(e => e.ID);
            modelBuilder
                .Entity<ProductTemporaryPrice>()
                .HasKey(e => new { e.ProductID, e.StartDate, e.EndDate });
            modelBuilder
                .Entity<Customer>()
                .HasKey(e => e.ID);
            modelBuilder
                .Entity<Order>()
                .HasKey(e => e.ID);
            modelBuilder
                .Entity<OrderProduct>()
                .HasKey(e => new { e.OrderID, e.ProductID });
            modelBuilder
                .Entity<Voucher>()
                .HasKey(e => e.Code);
            #endregion

            #region Relationships
            modelBuilder
                .Entity<Product>()
                .HasOne(e => e.Category)
                .WithMany(e => e.Products);
            modelBuilder
                .Entity<Product>()
                .HasOne(e => e.Manufacturer)
                .WithMany(e => e.Products);
            modelBuilder
                .Entity<ProductTemporaryPrice>()
                .HasOne(e => e.Product)
                .WithMany(e => e.TemporaryPrices)
                .HasForeignKey(e => e.ProductID);
            modelBuilder
                .Entity<Order>()
                .HasOne(e => e.Customer)
                .WithMany(e => e.Orders);
            modelBuilder
                .Entity<Order>()
                .HasMany(e => e.Vouchers)
                .WithMany(e => e.Orders);
            modelBuilder
                .Entity<OrderProduct>()
                .HasOne(e => e.Order)
                .WithMany(e => e.Products)
                .HasForeignKey(e => e.OrderID);
            modelBuilder
                .Entity<OrderProduct>()
                .HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductID);
            #endregion
        }
    }
}
