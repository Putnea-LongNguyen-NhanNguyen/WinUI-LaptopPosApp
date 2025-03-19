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
                .Entity<Product>()
                .HasOne(e => e.Category)
                .WithMany(e => e.Products);
            modelBuilder
                .Entity<Product>()
                .HasOne(e => e.Manufacturer)
                .WithMany(e => e.Products);
        }
    }
}
