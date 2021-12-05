using SBDProjekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace SBDProjekt.Infrastructure
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Opinion> Opinions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasMany(x => x.ProductEnjoyers)
                .WithMany(x => x.FavouriteProducts)
                .UsingEntity<FavouriteProduct>(
                    x => x.HasOne(p => p.Client).WithMany().HasForeignKey(x => x.ClientId),
                    x => x.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId)
                );

            modelBuilder.Entity<Sale>()
                .HasMany(x => x.DiscountedProducts)
                .WithMany(x => x.Sales)
                .UsingEntity<DiscountedProduct>(
                    x => x.HasOne(p => p.Product).WithMany().HasForeignKey(x => x.ProductId),
                    x => x.HasOne(x => x.Sale).WithMany().HasForeignKey(x => x.SaleId)
                );

            modelBuilder.Entity<OrderedProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderedProduct>()
                .HasOne(op => op.Product)
                .WithMany(o => o.OrderedProducts)
                .HasForeignKey(op => op.ProductId);
            
            modelBuilder.Entity<OrderedProduct>()
                .HasOne(op => op.Order)
                .WithMany(p => p.OrderedProduct)
                .HasForeignKey(op => op.OrderId);
        }

        public DbSet<SBDProjekt.Models.DiscountedProduct> DiscountedProduct { get; set; }

        public DbSet<SBDProjekt.Models.FavouriteProduct> FavouriteProduct { get; set; }
        
    }
}