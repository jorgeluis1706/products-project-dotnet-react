using Microsoft.EntityFrameworkCore;
using ProductsApi.Models;  

namespace ProductsApi.Contexts
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductColorVariation> ProductColorVariations { get; set; } = null!;
        
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed some data for testing
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "T-Shirt", Description = "Cotton T-Shirt", Price = 19.99m, InStock = true },
                new Product { Id = 2, Name = "Jeans", Description = "Blue Denim Jeans", Price = 49.99m, InStock = true }
            );

            modelBuilder.Entity<ProductColorVariation>().HasData(
                new ProductColorVariation { Id = 1, ProductId = 1, Name = "Red", Price = 2000 },
                new ProductColorVariation { Id = 2, ProductId = 1, Name = "Blue", Price = 2000 },
                new ProductColorVariation { Id = 3, ProductId = 2, Name = "Black", Price = 2000 }
            );
        }
    }
}