using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WireDev.Erp.V1.Models;
using WireDev.Erp.V1.Models.Statistics;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Context
{
	public class ApplicationDataDbContext : DbContext
	{
		public ApplicationDataDbContext(DbContextOptions<ApplicationDataDbContext> options) : base(options)
        {

		}

        public virtual DbSet<Settings> Settings { get; set; }

        public virtual DbSet<TotalStats> TotalStats { get; set; }
        public virtual DbSet<YearStats> YearStats { get; set; }
        public virtual DbSet<MonthStats> MonthStats { get; set; }
        public virtual DbSet<DayStats> DayStats { get; set; }
        public virtual DbSet<ProductStats> ProductStats { get; set; }

        public virtual DbSet<Purchase> Purchases { get; set; }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            _ = builder.Entity<Settings>().HasKey("Uuid");
            _ = builder.Entity<Settings>().HasData(new Settings());

            _ = builder.Entity<DayStats>().HasKey("Date");
            _ = builder.Entity<MonthStats>().HasKey("Date");
            _ = builder.Entity<YearStats>().HasKey("Date");
            _ = builder.Entity<TotalStats>().HasKey("Date");
            _ = builder.Entity<ProductStats>().HasKey("ProductId");
            _ = builder.Entity<ProductStats>()
                .Property(e => e.Transactions)
                .HasConversion(v => JsonSerializer.Serialize(v, null as JsonSerializerOptions),
                   v => JsonSerializer.Deserialize<Dictionary<long, TransactionItem>>(v, null as JsonSerializerOptions));

            _ = builder.Entity<Purchase>().HasKey("Uuid");
            Purchase purchase = new(); purchase.Post();
            _ = builder.Entity<Purchase>().HasData(purchase);
            _ = builder.Entity<Purchase>()
                .Property(e => e.Items)
                .HasConversion(v => JsonSerializer.Serialize(v, null as JsonSerializerOptions),
                   v => JsonSerializer.Deserialize<List<TransactionItem>>(v, null as JsonSerializerOptions),
                   new ValueComparer<List<TransactionItem>>(
                       (c1, c2) => c1.SequenceEqual(c2),
                       c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                       c => c.ToList()));

            Product product = new(9999) { Name = "Default_Product", };
            product.Prices.Add(new Price() { RetailValue = 10, SellValue = 15, Description = "Defaul_Price" });
            _ = builder.Entity<Product>().HasData(product);
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
            _ = builder.Entity<Group>().HasData(new Group(99) { Name = "Default_Group" });
        }
    }
}