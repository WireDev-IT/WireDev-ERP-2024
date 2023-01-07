using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WireDev.Erp.V1.Models.Enums;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Context
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Price> Prices { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            _ = builder.Entity<Price>().HasKey("Uuid");
            _ = builder.Entity<Product>().HasKey("Uuid");
            _ = builder.Entity<Purchase>().HasKey("Uuid");
            _ = builder.Entity<Category>().HasKey("Uuid");

            _ = builder.Entity<Product>().HasData(new Product() { Name = "Default_Product" });
            _ = builder.Entity<Price>().HasData(new Price() { Description = "Default_Price" });
            _ = builder.Entity<Category>().HasData(new Category() { Description = "Default_Price" });

            _ = builder.Entity<Product>()
                .Property(e => e.Metadata)
                .HasConversion(v => JsonSerializer.Serialize(v, null as JsonSerializerOptions),
                               v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, null as JsonSerializerOptions));
            _ = builder.Entity<Product>()
                .Property(e => e.Properties)
                .HasConversion(v => JsonSerializer.Serialize(v, null as JsonSerializerOptions),
                               v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, null as JsonSerializerOptions));
            _ = builder.Entity<Product>()
                .Property(e => e.Categories)
                .HasConversion(v => JsonSerializer.Serialize(v, null as JsonSerializerOptions),
                               v => JsonSerializer.Deserialize<List<Guid>>(v, null as JsonSerializerOptions));
            _ = builder.Entity<Product>()
                .Property(e => e.Prices)
                .HasConversion(v => JsonSerializer.Serialize(v, null as JsonSerializerOptions),
                               v => JsonSerializer.Deserialize<List<Guid>>(v, null as JsonSerializerOptions));
            _ = builder.Entity<Purchase>()
                .Property(e => e.Items)
                .HasConversion(v => JsonSerializer.Serialize(v, null as JsonSerializerOptions),
                               v => JsonSerializer.Deserialize<Dictionary<(Guid productId, Guid priceId, TransactionType type), uint>>(v, null as JsonSerializerOptions));
        }
    }
}