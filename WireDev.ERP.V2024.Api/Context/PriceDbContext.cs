using System;
using Microsoft.EntityFrameworkCore;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Context
{
	public class PriceDbContext : DbContext
	{
		public PriceDbContext(DbContextOptions<PriceDbContext> options) : base(options)
        {

		}

        public virtual DbSet<Price> Prices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            _ = builder.Entity<Price>().HasKey("Uuid");
            _ = builder.Entity<Price>().HasData(new Price() { Description = "Default_Price" });
        }

    }
}

