using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WireDev.Erp.V1.Models.Enums;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Context
{
    public class PurchaseDbContext : DbContext
    {
        public PurchaseDbContext(DbContextOptions<PurchaseDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            _ = builder.Entity<Purchase>().HasKey("Uuid");
            Purchase p = new();
            p.Post();
            _ = builder.Entity<Purchase>().HasData(p);

            _ = builder.Entity<Purchase>()
                .Property(e => e.Items)
                .HasConversion(v => JsonSerializer.Serialize(v, null as JsonSerializerOptions),
                   v => JsonSerializer.Deserialize<List<TransactionItem>>(v, null as JsonSerializerOptions),
                   new ValueComparer<List<TransactionItem>>(
                       (c1, c2) => c1.SequenceEqual(c2),
                       c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                       c => c.ToList()));

        }
    }
}