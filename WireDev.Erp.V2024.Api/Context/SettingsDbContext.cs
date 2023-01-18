using System;
using Microsoft.EntityFrameworkCore;
using WireDev.Erp.V1.Models;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Context
{
	public class SettingsDbContext : DbContext
	{
		public SettingsDbContext(DbContextOptions<SettingsDbContext> options) : base(options)
        {

		}

        public virtual DbSet<Settings> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            _ = builder.Entity<Settings>().HasKey("Uuid");
            _ = builder.Entity<Price>().HasData(new Settings());
        }

    }
}

