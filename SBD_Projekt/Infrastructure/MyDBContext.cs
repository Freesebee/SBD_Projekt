using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SBDProjekt.Models;

namespace SBDProjekt.Infrastructure
{
    public class MyDBContext : IdentityDbContext<ApplicationUser>
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Opinion> Opinions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<DiscountedProduct> DiscountedProduct { get; set; }
        public DbSet<FavouriteProduct> FavouriteProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<IdentityRole>()
                .HasData(
                    new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() },
                    new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() });
        }
    }
}