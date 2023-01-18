using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
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
        public virtual DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            _ = builder.Entity<Product>().HasKey("Uuid");
            _ = builder.Entity<Product>().HasData(new Product(9999) { Name = "Default_Product" });

            _ = builder.Entity<Product>()
                .Property(e => e.Metadata)
                .HasConversion(v => JsonSerializer.Serialize(v, null as JsonSerializerOptions),
                               v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, null as JsonSerializerOptions));

            _ = builder.Entity<Product>()
                .Property(e => e.Properties)
                .HasConversion(v => JsonSerializer.Serialize(v, null as JsonSerializerOptions),
                               v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, null as JsonSerializerOptions));

            _ = builder.Entity<Product>()
                .Property(e => e.Prices)
                .HasConversion(v => JsonSerializer.Serialize(v, null as JsonSerializerOptions),
                               v => JsonSerializer.Deserialize<List<Price>>(v, null as JsonSerializerOptions),
                               new ValueComparer<List<Price>>(
                                   (c1, c2) => c1.SequenceEqual(c2),
                                   c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                   c => c.ToList()));

            _ = builder.Entity<Product>()
                .Property(e => e.EAN)
                .HasConversion(v => JsonSerializer.Serialize(v, null as JsonSerializerOptions),
                               v => JsonSerializer.Deserialize<List<ulong>>(v, null as JsonSerializerOptions),
                               new ValueComparer<List<ulong>>(
                                   (c1, c2) => c1.SequenceEqual(c2),
                                   c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                   c => c.ToList()));

            _ = builder.Entity<Group>().HasKey("Uuid");
            _ = builder.Entity<Group>().HasData(new Group(99) { Name = "Default_Group" }) ;
        }
    }
}